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
using System.IO;
using System.ComponentModel;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Utilities
{
    public class ProcessStream
    {
        Emulator _emulator;
        IntPtr _processHandle;
        Process _process;
        Queue<double> _fpsTimes = new Queue<double>();
        BackgroundWorker _streamUpdater;
        byte[] _ram;
        bool _lastUpdateBeforePausing = false;
        int _interval;
        object _enableLocker = new object();
        object _fpsQueueLocker = new object();

        public event EventHandler OnUpdate;
        public event EventHandler OnStatusChanged;
        public event EventHandler OnDisconnect;
        public event EventHandler FpsUpdated;
        public event EventHandler WarnReadonlyOff;
        public event EventHandler OnClose;

        public bool Readonly = true;
        public bool ShowWarning = true;

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
                    running = !(IsSuspended || IsClosed || !IsEnabled || !_streamUpdater.IsBusy);
                }
                return running;
            }
        }

        public ProcessStream(Process process = null, Emulator emulator = null)
        {
            _process = process;

            _interval = (int) (1000.0f / Config.RefreshRateFreq);
            _ram = new byte[Config.RamSize];

            _streamUpdater = new BackgroundWorker();
            _streamUpdater.DoWork += ProcessUpdateTask;
            _streamUpdater.WorkerSupportsCancellation = true;
            _streamUpdater.RunWorkerAsync();

            SwitchProcess(_process, _emulator);
        }

        private void LogException(Exception e)
        {
            try
            {
                var log = String.Format("{0}\n{1}\n{2}\n", e.Message, e.TargetSite.ToString(), e.StackTrace);
                File.AppendAllText("error.txt", log);
            }
            catch (Exception) { }
        }

        private void ExceptionHandler(Task obj)
        {
            LogException(obj.Exception);
            throw obj.Exception;
        }

        ~ProcessStream()
        {
            if (_process != null)
                _process.Exited -= ProcessClosed;

            _streamUpdater.CancelAsync();
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
            int numOfBytes = 0;
            return Kernal32NativeMethods.ProcessWriteMemory(_processHandle, (IntPtr)(absoluteAddressing ? address : 
                ConvertAddressEndianess((int)((address + _emulator.RamStart) & ~0x80000000U), buffer.Length)),
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
            OnDisconnect?.Invoke(this, new EventArgs());
        }

        public byte GetByte(uint address, bool absoluteAddress = false)
        {
            return ReadRamLittleEndian(address, 1, absoluteAddress)[0];
        }

        public sbyte GetSByte(uint address, bool absoluteAddress = false)
        {
            return (sbyte)ReadRamLittleEndian(address, 1, absoluteAddress)[0];
        }

        public short GetInt16(uint address, bool absoluteAddress = false)
        { 
            return BitConverter.ToInt16(ReadRamLittleEndian(address, 2, absoluteAddress), 0);
        }

        public ushort GetUInt16(uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToUInt16(ReadRamLittleEndian(address, 2, absoluteAddress), 0);
        }

        public int GetInt32(uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToInt32(ReadRamLittleEndian(address, 4, absoluteAddress), 0);
        }

        public uint GetUInt32(uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToUInt32(ReadRamLittleEndian(address, 4, absoluteAddress), 0);
        }

        public float GetSingle(uint address, bool absoluteAddress = false)
        {
            return BitConverter.ToSingle(ReadRamLittleEndian(address, 4, absoluteAddress), 0);
        }

        public byte[] ReadRamLittleEndian(uint address, int length, bool absoluteAddress = false)
        {
            byte[] readBytes = new byte[length];

            if (absoluteAddress)
                address = address - _emulator.RamStart;
            else
                address = ConvertAddressEndianess(address & ~0x80000000U, length);

            if (address + length > _ram.Length)
                return new byte[length];

            Array.Copy(_ram, address, readBytes, 0, length);
            return readBytes;
        }

        readonly byte[] _fixAddress = { 0x03, 0x02, 0x01, 0x00 };
        public byte[] ReadRam(uint address, int length)
        {
            byte[] readBytes = new byte[length];
            address &= ~0x80000000U;

            if (address + length > _ram.Length)
                return new byte[length];

            for (uint i = 0; i < length; i++, address++)
            {
                readBytes[i] = _ram[address & ~0x00000003 | _fixAddress[address & 0x3]];
            }

            return readBytes;
        }

        public bool CheckReadonlyOff()
        {
            if (ShowWarning)
                WarnReadonlyOff?.Invoke(this, new EventArgs());

            return Readonly;
        }

        public bool SetValue(byte value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(new byte[] { value }, address, absoluteAddress);
        }

        public bool SetValue(sbyte value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(new byte[] { (byte)value }, address, absoluteAddress);
        }

        public bool SetValue(Int16 value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
        }

        public bool SetValue(UInt16 value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
        }

        public bool SetValue(Int32 value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
        }

        public bool SetValue(UInt32 value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
        }

        public bool SetValue(float value, uint address, bool absoluteAddress = false)
        {
            return WriteRamLittleEndian(BitConverter.GetBytes(value), address, absoluteAddress);
        }

        public bool WriteRamLittleEndian(byte[] buffer, uint address, bool absoluteAddress = false, int bufferStart = 0, int? length = null, bool safeWrite = true)
        {
            if (length == null)
                length = buffer.Length - bufferStart;

            if (CheckReadonlyOff())
                return false;

            byte[] writeBytes = new byte[length.Value];
            Array.Copy(buffer, bufferStart, writeBytes, 0, length.Value);

            // Attempt to pause the game before writing 
            bool preSuspended = IsSuspended;
            if (safeWrite)
                Suspend();

            // Write memory to game/process
            bool result = WriteProcessMemory((int)address, writeBytes, absoluteAddress);

            // Resume stream 
            if (safeWrite && !preSuspended)
                Resume();

            return result;
        }

        public bool WriteRam(byte[] buffer, uint address, int bufferStart = 0, int? length = null, bool safeWrite = true)
        {
            address &= ~0x80000000U;

            if (length == null)
                length = buffer.Length - bufferStart;

            if (CheckReadonlyOff())
                return false;

            bool success = true;

            // Attempt to pause the game before writing 
            bool preSuspended = IsSuspended;
            if (safeWrite)
                Suspend();

            // Take care of first alignment
            int bufPos = bufferStart;
            uint alignment = _fixAddress[address & 0x03] + 1U;
            if (alignment < 4)
            {
                byte[] writeBytes = new byte[Math.Min(alignment, length.Value)];
                Array.Copy(buffer, bufPos, writeBytes, 0, writeBytes.Length);
                success &= WriteProcessMemory((int)address, writeBytes.Reverse().ToArray());
                length -= writeBytes.Length;
                bufPos += writeBytes.Length;
                address += alignment;
            }

            // Take care of middle
            if (length >= 4)
            {
                byte[] writeBytes = new byte[length.Value & ~0x03];
                for (int i = 0; i < writeBytes.Length; bufPos += 4, i += 4)
                {
                    writeBytes[i] = buffer[bufPos + 3];
                    writeBytes[i + 1] = buffer[bufPos + 2];
                    writeBytes[i + 2] = buffer[bufPos + 1];
                    writeBytes[i + 3] = buffer[bufPos];
                }
                success &= WriteProcessMemory((int)(address + _emulator.RamStart), writeBytes, true);
                address += (uint)writeBytes.Length;
                length -= writeBytes.Length;
            }

            // Take care of last
            if (length > 0)
            {
                byte[] writeBytes = new byte[length.Value];
                Array.Copy(buffer, bufPos, writeBytes, 0, writeBytes.Length);
                success &= WriteProcessMemory((int)address, writeBytes.Reverse().ToArray());
            }

            // Resume stream 
            if (safeWrite && !preSuspended)
                Resume();

            return success;
        }

        public void Stop()
        {
            _streamUpdater.CancelAsync();
        }

        public bool RefreshRam()
        {
            try
            {
                // Read whole ram value to buffer
                return ReadProcessMemory(0, _ram);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ProcessUpdateTask(object sender, DoWorkEventArgs e)
        {
            var worker = sender as BackgroundWorker;
            var prevTime = Stopwatch.StartNew();
            while (!e.Cancel)
            {
                if (worker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                prevTime.Restart();
                if (!IsRunning & !_lastUpdateBeforePausing)
                    goto FrameLimitStreamUpdate;

                _lastUpdateBeforePausing = false;

                int timeToWait;
                try
                {
                    if (!RefreshRam())
                        continue;

                    OnUpdate?.Invoke(this, new EventArgs());

                    foreach (var lockVar in LockedVariables)
                        lockVar.Value.Update();
                }
                catch (Exception ee)
                {
                    LogException(ee);
                    MessageBox.Show("A Fatal Error has occured. See output.txt for details. The program will now exit.", 
                        "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                    break;
                }

                FrameLimitStreamUpdate:

                // Calculate delay to match correct FPS
                prevTime.Stop();
                timeToWait = _interval - (int)prevTime.ElapsedMilliseconds;
                timeToWait = Math.Max(timeToWait, 0);

                // Calculate Fps
                lock (_fpsQueueLocker)
                {
                    if (_fpsTimes.Count() >= 10)
                        _fpsTimes.Dequeue();
                    _fpsTimes.Enqueue(prevTime.ElapsedMilliseconds + timeToWait);
                }
                FpsUpdated?.Invoke(this, new EventArgs());
            
                Task.Delay(timeToWait).Wait();
            }

            OnClose?.BeginInvoke(this, new EventArgs(), null, null);
        }

        public int ConvertAddressEndianess(int address, int dataSize)
        {
            switch (dataSize)
            {
                case 1:
                case 2:
                case 3:
                    return (int)(address & ~0x03) | (_fixAddress[dataSize - 1] - address & 0x03);
                default:
                    return address;
            }
        }

        public uint ConvertAddressEndianess(uint address, int dataSize)
        {
            switch (dataSize)
            {
                case 1:
                case 2:
                case 3:
                    return (uint)(address & ~0x03) | (_fixAddress[dataSize - 1] - address & 0x03);
                default:
                    return address;
            }
        }
    }
}
