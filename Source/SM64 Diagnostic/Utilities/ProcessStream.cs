using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.Utilities
{
    public class ProcessStream
    {
        IntPtr _processHandle;
        Process _process;
        Timer _timer;
        int _offset;
        byte[] _ram;
        bool _lastUpdateBeforePausing = false;
        Config _config;

        public event EventHandler OnUpdate;
        public event EventHandler OnStatusChanged;

        public uint ProcessMemoryOffset
        {
            get
            {
                return (uint) _offset;
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
                return _config.ProcessName;
            }
        }

        public ProcessStream(Config config, Process process = null)
        {
            _offset = (int)(config.RamStartAddress & 0x7FFFFFFF);
            _process = process;
            _config = config;

            _timer = new Timer();
            _timer.Interval = (int) (1000.0f / config.RefreshRateFreq);
            _timer.Tick += OnTick;

            _ram = new byte[config.RamSize];

            SwitchProcess(_process);
        }

        ~ProcessStream()
        {
            if (_process != null)
                _process.Exited -= ProcessClosed;
        }

        public bool SwitchProcess(Process newProcess)
        {
            // Close old process
            _timer.Enabled = false;
            NativeMethods.CloseProcess(_processHandle);

            // Make sure old process is running
            if (IsSuspended)
                Resume();

            // Disconnect events
            if (_process != null)
                _process.Exited -= ProcessClosed;

            // Make sure the new process has a value
            if (newProcess == null)
            {
                OnStatusChanged?.Invoke(this, new EventArgs());
                return false;
            }

            // Open and set new process
            _process = newProcess;
            _processHandle = NativeMethods.ProcessGetHandleFromId(0x0838, false, _process.Id);

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
            OnStatusChanged?.Invoke(this, new EventArgs());

            _timer.Enabled = true;

            return true;
        }

        public Boolean IsSuspended = false;
        public Boolean IsClosed = true;
        public Boolean IsRunning
        {
            get
            {
                return !(IsSuspended || IsClosed);
            }
        }

        public bool ReadProcessMemory(int address, byte[] buffer, bool absoluteAddressing = false)
        {
            if (_process == null)
                return false;

            int numOfBytes = 0;
            return NativeMethods.ProcessReadMemory(_processHandle, (IntPtr) (absoluteAddressing ? address : address + _offset),
                buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public bool WriteProcessMemory(int address, byte[] buffer, bool absoluteAddressing = false)
        {
            if (_process == null)
                return false;

            int numOfBytes = 0;
            return NativeMethods.ProcessWriteMemory(_processHandle, (IntPtr)(absoluteAddressing ? address : address + _offset),
                buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public void Suspend()
        {
            if (IsSuspended || _process == null)
                return;

            _lastUpdateBeforePausing = true;

            NativeMethods.SuspendProcess(_process);

            IsSuspended = true;
            OnStatusChanged?.Invoke(this, new EventArgs());
        }

        public void Resume()
        {
            if (!IsSuspended || _process == null)
                return;

            // Resume all threads
            NativeMethods.ResumeProcess(_process);

            IsSuspended = false;
            OnStatusChanged?.Invoke(this, new EventArgs());
        }

        private void ProcessClosed(object sender, EventArgs e)
        {
            IsClosed = true;
            _timer.Enabled = false;
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
                address = (uint)(address - _offset);

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

        private void OnTick(object sender, EventArgs e)
        {
            if (!IsRunning & !_lastUpdateBeforePausing)
                return;

            _lastUpdateBeforePausing = false;

            // Read whole ram value to buffer
            if (!ReadProcessMemory(0, _ram))
                return;
            _timer.Enabled = false;
            OnUpdate?.Invoke(this, e);
            _timer.Enabled = true;
        }
    }
}
