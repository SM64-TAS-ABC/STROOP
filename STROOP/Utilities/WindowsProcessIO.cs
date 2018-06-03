using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    class WindowsProcessRamIO : IEmuRamIO
    {
        IntPtr _processHandle;
        Process _process;
        bool _isSuspended = false;
        UIntPtr _baseOffset;
        uint _ramSize;
        Emulator _emulator;

        public bool IsSuspended => _isSuspended;

        public event EventHandler OnClose;

        public WindowsProcessRamIO(Process process, Emulator emulator, uint ramSize)
        {
            _process = process;
            _emulator = emulator;
            _ramSize = ramSize;

            CalculateOffset();
            _process.EnableRaisingEvents = true;

            _processHandle = Kernal32NativeMethods.ProcessGetHandleFromId(0x0838, false, _process.Id);
            _process.Exited += _process_Exited;
        }

        private void _process_Exited(object sender, EventArgs e)
        {
            Dispose();
            OnClose.Invoke(sender, e);
        }

        protected virtual void CalculateOffset()
        {
            // Find DLL offset if needed
            IntPtr dllOffset = new IntPtr();

            if (_emulator != null && _emulator.Dll != null)
            {
                ProcessModule dll = _process.Modules.Cast<ProcessModule>()
                    ?.FirstOrDefault(d => d.ModuleName == _emulator.Dll);

                if (dll == null)
                    throw new ArgumentNullException("Could not find ");

                dllOffset = dll.BaseAddress;
            }

            _baseOffset = (UIntPtr)(_emulator.RamStart + (UInt64)dllOffset.ToInt64());
        }

        public bool Suspend()
        {
            Kernal32NativeMethods.SuspendProcess(_process);
            _isSuspended = true;
            return true;
        }

        public bool Resume()
        {
            // Resume all threads
            Kernal32NativeMethods.ResumeProcess(_process);
            _isSuspended = false;
            return true;
        }

        public bool ReadRelative(uint address, byte[] buffer)
        {
            return ReadAbsolute(GetAbsoluteAddress(address), buffer);
        }

        public bool ReadAbsolute(UIntPtr address, byte[] buffer)
        {
            if (_process == null)
                return false;

            int numOfBytes = 0;
            return Kernal32NativeMethods.ProcessReadMemory(_processHandle, address, buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public bool WriteRelative(uint address, byte[] buffer)
        {
            return WriteAbsolute(GetAbsoluteAddress(address), buffer);
        }

        public bool WriteAbsolute(UIntPtr address, byte[] buffer)
        {
            int numOfBytes = 0;

            // Safety bounds check
            if (address.ToUInt64() < _baseOffset.ToUInt64()
                || address.ToUInt64() + (uint)buffer.Length >= _baseOffset.ToUInt64() + _ramSize)
                return false;

            return Kernal32NativeMethods.ProcessWriteMemory(_processHandle, address,
                buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public UIntPtr GetAbsoluteAddress(uint n64Address)
        {
            n64Address &= ~0x80000000U;
            return (UIntPtr)(_baseOffset.ToUInt64() + n64Address);
        }

        public uint GetRelativeAddress(UIntPtr absoluteAddress)
        {
            return 0x80000000 | (uint)(absoluteAddress.ToUInt64() - _baseOffset.ToUInt64());
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _process.Exited -= _process_Exited;
                }

                // Close old process
                Kernal32NativeMethods.CloseProcess(_processHandle);

                disposedValue = true;
            }
        }
        
        ~WindowsProcessRamIO()
        {
            Dispose(false);
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
