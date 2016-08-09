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

        [Flags]
        private enum ThreadAccess : int
        {
            TERMINATE = (0x0001),
            SUSPEND_RESUME = (0x0002),
            GET_CONTEXT = (0x0008),
            SET_CONTEXT = (0x0010),
            SET_INFORMATION = (0x0020),
            QUERY_INFORMATION = (0x0040),
            SET_THREAD_TOKEN = (0x0080),
            IMPERSONATE = (0x0100),
            DIRECT_IMPERSONATION = (0x0200)
        }

        #region DLLImports
        [DllImport("kernel32.dll")]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        [DllImport("kernel32.dll")]
        static extern uint SuspendThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        static extern int ResumeThread(IntPtr hThread);

        [DllImport("kernel32.dll")]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        static extern bool ReadProcessMemory(int hProcess,
            int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
            byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);
        #endregion

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
            _offset = (int)(config.RamStartAddress & 0x0FFFFFFF);
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
            CloseHandle(_processHandle);

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
            _processHandle = OpenProcess(0x0838, false, _process.Id);

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
            return ReadProcessMemory((int) _processHandle, absoluteAddressing ? address : address + _offset,
                buffer, buffer.Length, ref numOfBytes);
        }

        public bool WriteProcessMemory(int address, byte[] buffer, bool absoluteAddressing = false)
        {
            if (_process == null)
                return false;

            int numOfBytes = 0;
            return WriteProcessMemory((int)_processHandle, absoluteAddressing ? address : (int)(address + _offset),
                buffer, buffer.Length, ref numOfBytes);
        }

        public void Suspend()
        {
            if (IsSuspended || _process == null)
                return;

            _lastUpdateBeforePausing = true;

            // Pause all threads
            foreach (ProcessThread pT in _process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                    continue;

                SuspendThread(pOpenThread);
                CloseHandle(pOpenThread);
            }

            IsSuspended = true;
            OnStatusChanged?.Invoke(this, new EventArgs());
        }

        public void Resume()
        {
            if (!IsSuspended || _process == null)
                return;

            // Resume all threads
            foreach (ProcessThread pT in _process.Threads)
            {
                IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)pT.Id);

                if (pOpenThread == IntPtr.Zero)
                    continue;

                int suspendCount = 0;
                do
                {
                    suspendCount = ResumeThread(pOpenThread);
                } while (suspendCount > 0);

                CloseHandle(pOpenThread);
            }

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
            if (safeWrite)
                Suspend();

            // Write memory to game/process
            bool result =  WriteProcessMemory((int)address, writeBytes, absoluteAddress);

            // Resume stream 
            if (safeWrite)
                Resume();

            return result;
        }
        
        public bool WriteRam(byte[] buffer, uint address, int length, bool absoluteAddress = false, bool? fixAddress = null)
        {
            return WriteRam(buffer, 0, address, buffer.Length, absoluteAddress, fixAddress);
        }

        private void OnTick(object sednder, EventArgs e)
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
