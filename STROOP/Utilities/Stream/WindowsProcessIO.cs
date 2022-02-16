﻿using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STROOP.Utilities.Kernal32NativeMethods;

namespace STROOP.Utilities
{
    class WindowsProcessRamIO : BaseProcessIO, IDisposable
    {
        protected IntPtr _processHandle;
        protected Process _process;
        protected bool _isSuspended = false;
        protected UIntPtr _baseOffset;
        protected Emulator _emulator;

        public override bool IsSuspended => _isSuspended;

        protected override EndiannessType Endianness => _emulator.Endianness;
        protected override UIntPtr BaseOffset => _baseOffset;

        public override string Name => _process.ProcessName;
        public override Process Process => _process;

        public override event EventHandler OnClose;

        public WindowsProcessRamIO(Process process, Emulator emulator) : base()
        {
            _process = process;
            _emulator = emulator;

            _process.EnableRaisingEvents = true;

            ProcessAccess accessFlags = ProcessAccess.PROCESS_QUERY_LIMITED_INFORMATION | ProcessAccess.SUSPEND_RESUME 
                | ProcessAccess.VM_OPERATION | ProcessAccess.VM_READ | ProcessAccess.VM_WRITE;
            _processHandle = ProcessGetHandleFromId(accessFlags, false, _process.Id);
            try
            {
                CalculateOffset();
            }
            catch (Exception e)
            {
                CloseProcess(_processHandle);
                throw e;
            }

            _process.Exited += _process_Exited;
        }

        private void _process_Exited(object sender, EventArgs e)
        {
            Dispose();
            OnClose.Invoke(sender, e);
        }

        protected override bool ReadFunc(UIntPtr address, byte[] buffer)
        {
            int numOfBytes = 0;
            return ProcessReadMemory(_processHandle, address, buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        protected override bool WriteFunc(UIntPtr address, byte[] buffer)
        {
            int numOfBytes = 0;
            return ProcessWriteMemory(_processHandle, address, buffer, (IntPtr)buffer.Length, ref numOfBytes);
        }

        public override byte[] ReadAllMemory()
        {
            List<byte> output = new List<byte>();
            byte[] buffer = new byte[1];
            int numBytes = 1;

            for (uint address = 0; true; address++)
            {
                bool success = ProcessReadMemory(_processHandle, (UIntPtr)address, buffer, (IntPtr)buffer.Length, ref numBytes);
                if (!success) break;
                output.Add(buffer[0]);
            }

            return output.ToArray();
        }

        protected virtual void CalculateOffset()
        {
            // Find DLL offset if needed
            IntPtr dllOffset = new IntPtr();

            if (_emulator != null && _emulator.Dll != null)
            {
                ProcessModule dll = _process.Modules.Cast<ProcessModule>()
                    ?.FirstOrDefault(d => d.ModuleName.Equals(_emulator.Dll, StringComparison.OrdinalIgnoreCase));

                if (dll == null)
                    throw new ArgumentNullException("Could not find ");

                dllOffset = dll.BaseAddress;
            }

            _baseOffset = (UIntPtr)(_emulator.RamStart + (UInt64)dllOffset.ToInt64());
        }

        public override bool Suspend()
        {
            SuspendProcess(_process);
            _isSuspended = true;
            return true;
        }

        public override bool Resume()
        {
            // Resume all threads
            ResumeProcess(_process);
            _isSuspended = false;
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (IsSuspended)
                        Resume();
                    _process.Exited -= _process_Exited;
                }

                // Close old process
                CloseProcess(_processHandle);

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
