using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using System.Threading;

namespace SM64_Diagnostic.Utilities
{
    public class ProcessStream
    {
        Emulator _emulator;
        IntPtr _processHandle;
        Process _process;
        Queue<double> _fpsTimes = new Queue<double>();
        Task _streamUpdater;
        byte[] _ram;
        bool _lastUpdateBeforePausing = false;
        int _interval;
        object _enableLocker = new object();
        object _fpsQueueLocker = new object();

        CancellationTokenSource _cancelToken = new CancellationTokenSource();

        public event EventHandler OnUpdate;
        public event EventHandler OnStatusChanged;
        public event EventHandler FpsUpdated;

        public Dictionary<WatchVariableLock, WatchVariableLock> LockedVariables = 
            new Dictionary<WatchVariableLock, WatchVariableLock>();

        public uint ProcessMemoryOffset
        {
            get
            {
                return _emulator == null ? 0 : _emulator.RamStart;
            }
        }

        public byte[] Ram
        {
            get
            {
                return _ram;
            }
        }

        public string ProcessName
        {
            get
            {
                return _emulator == null ? "(No Emulator)" : _emulator.ProcessName;
            }
        }

        public double Fps
        {
            get
            {
                double fps;
                lock (_fpsQueueLocker)
                {
                    fps = _fpsTimes.Count == 0 ? 0 : 1000 / _fpsTimes.Average();
                }
                return fps;
            }
        }

        public Boolean IsSuspended = false;
        public Boolean IsClosed = true;
        public Boolean IsEnabled = false;
        public Boolean IsRunning
        {
            get
            {
                bool running;
                lock(_enableLocker)
                {
                    running = !(IsSuspended || IsClosed || !IsEnabled);
                }
                return running;
            }
        }

        public ProcessStream(Process process = null, Emulator emulator = null)
        {
            _process = process;

            _interval = (int) (1000.0f / Config.RefreshRateFreq);
            _ram = new byte[Config.RamSize];

            _streamUpdater = Task.Factory.StartNew(async() => await ProcessUpdateTask(_cancelToken.Token), _cancelToken.Token);

            SwitchProcess(_process, _emulator);
        }

        ~ProcessStream()
        {
            if (_process != null)
                _process.Exited -= ProcessClosed;

            _cancelToken?.Cancel();
            _streamUpdater?.Wait();
            _cancelToken?.Dispose();
        }

        public bool SwitchProcess(Process newProcess, Emulator emulator)
        {
            lock (_enableLocker)
            {
                IsEnabled = false;
            }

            // Make sure old process is running
            if (IsSuspended)
                Resume();

            // Close old process
            Kernal32NativeMethods.CloseProcess(_processHandle);

            // Disconnect events
            if (_process != null)
                _process.Exited -= ProcessClosed;

            // Make sure the new process has a value
            if (newProcess == null)
            {
                _processHandle = new IntPtr();
                _process = null;
                _emulator = null;
                OnStatusChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // Open and set new process
            _process = newProcess;
            _emulator = emulator;
            _processHandle = Kernal32NativeMethods.ProcessGetHandleFromId(0x0838, false, _process.Id);

            if ((int)_processHandle == 0)
            {
                OnStatusChanged?.Invoke(this, new EventArgs());
                return false;
            }

            try
            {
                _process.EnableRaisingEvents = true;
            }
            catch (Exception)
            {
                return false;
            }
            _process.Exited += ProcessClosed;

            IsSuspended = false;
            IsClosed = false;
            lock (_enableLocker)
            {
                IsEnabled = true;
            }
            OnStatusChanged?.Invoke(this, new EventArgs());

            return true;
        }

        public bool ReadProcessMemory(int address, byte[] buffer, bool absoluteAddressing = false)
        {
            if (_process == null)
                return false;

            int numOfBytes = 0;
            return Kernal32NativeMethods.ProcessReadMemory(_processHandle, (IntPtr) (absoluteAddressing ? address : address + _emulator.RamStart),
                buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public bool WriteProcessMemory(int address, byte[] buffer, bool absoluteAddressing = false)
        {
            if (_process == null)
                return false;

            int numOfBytes = 0;
            return Kernal32NativeMethods.ProcessWriteMemory(_processHandle, (IntPtr)(absoluteAddressing ? address : address + _emulator.RamStart),
                buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public void Suspend()
        {
            if (IsSuspended || _process == null)
                return;

            _lastUpdateBeforePausing = true;

            Kernal32NativeMethods.SuspendProcess(_process);

            IsSuspended = true;
            OnStatusChanged?.Invoke(this, new EventArgs());
        }

        public void Resume()
        {
            if (!IsSuspended || _process == null)
                return;

            // Resume all threads
            Kernal32NativeMethods.ResumeProcess(_process);

            IsSuspended = false;
            OnStatusChanged?.Invoke(this, new EventArgs());
        }

        private void ProcessClosed(object sender, EventArgs e)
        {
            IsClosed = true;
            lock (_enableLocker)
            {
                IsEnabled = false;
            }
            OnStatusChanged?.Invoke(this, new EventArgs());
        }

        public byte[] ReadRam(uint address, int length, bool absoluteAddress = false, bool? fixAddress = null)
        {
            byte[] readBytes = new byte[length];
            address &= ~0x80000000U;

            // Fix little endianess addressing
            if ((fixAddress.HasValue && fixAddress.Value) || (fixAddress == null && !absoluteAddress))
                address = (uint)LittleEndianessAddressing.AddressFix((int)address, length);

            // Handling absolute addressing (remove process offset from address)
            if (absoluteAddress)
                address = address - _emulator.RamStart;

            if (address + length > _ram.Length)
                return new byte[length];

            // Retrieve ram bytes from final address
            Array.Copy(_ram, address, readBytes, 0, length);

            return readBytes;
        }

        public bool WriteRam(byte[] buffer, uint address, bool absoluteAddress = false, bool? fixAddress = null)
        {
            return WriteRam(buffer, address, buffer.Length, absoluteAddress, fixAddress);
        }

        public bool WriteRam(byte[] buffer, int bufferStart, uint address, int length, bool absoluteAddress = false, bool? fixAddress = null, bool safeWrite = true)
        {
            byte[] writeBytes = new byte[length];
            address &= ~0x80000000U;
            Array.Copy(buffer, bufferStart, writeBytes, 0, length);
            
            // Fix little endianess addresssing
            if ((fixAddress.HasValue && fixAddress.Value) || (fixAddress == null && !absoluteAddress))
                address = (uint)LittleEndianessAddressing.AddressFix((int)address, length);

            // Attempt to pause the game before writing 
            bool preSuspended = IsSuspended;
            if (safeWrite)
                Suspend();

            // Write memory to game/process
            bool result =  WriteProcessMemory((int)address, writeBytes, absoluteAddress);

            // Resume stream 
            if (safeWrite && !preSuspended)
                Resume();

            return result;
        }
        
        public bool WriteRam(byte[] buffer, uint address, int length, bool absoluteAddress = false, bool? fixAddress = null)
        {
            return WriteRam(buffer, 0, address, length, absoluteAddress, fixAddress);
        }

        public void Stop()
        {
            _cancelToken?.Cancel();
            _streamUpdater?.Wait();
        }

        private async Task ProcessUpdateTask(CancellationToken ct)
        {
            var prevTime = Stopwatch.StartNew();
            while (!ct.IsCancellationRequested)
            {
                prevTime.Restart();
                if (!IsRunning & !_lastUpdateBeforePausing)
                    goto FrameLimitStreamUpdate;

                _lastUpdateBeforePausing = false;

                // Read whole ram value to buffer
                if (!ReadProcessMemory(0, _ram))
                    return;
                OnUpdate?.Invoke(this, new EventArgs());

                foreach (var lockVar in LockedVariables)
                    lockVar.Value.Update();

                FrameLimitStreamUpdate:

                // Calculate delay to match correct FPS
                prevTime.Stop();
                int timeToWait = _interval - (int)prevTime.ElapsedMilliseconds;
                timeToWait = timeToWait > 0 ? timeToWait : 0;

                // Calculate Fps
                lock (_fpsQueueLocker)
                {
                    if (_fpsTimes.Count() >= 10)
                        _fpsTimes.Dequeue();
                    _fpsTimes.Enqueue(prevTime.ElapsedMilliseconds + timeToWait);
                }
                FpsUpdated?.Invoke(this, new EventArgs());

                await Task.Delay(timeToWait);
            }

            ct.ThrowIfCancellationRequested();
        }
    }
}
