using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static STROOP.Utilities.Kernal32NativeMethods;

namespace STROOP.Utilities
{
    class WindowsProcessRamIO : IEmuRamIO
    {
        protected IntPtr _processHandle;
        protected Process _process;
        protected bool _isSuspended = false;
        protected UIntPtr _baseOffset;
        protected uint _ramSize;
        protected Emulator _emulator;

        public bool IsSuspended => _isSuspended;

        public event EventHandler OnClose;

        public WindowsProcessRamIO(Process process, Emulator emulator, uint ramSize)
        {
            _process = process;
            _emulator = emulator;
            _ramSize = ramSize;

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
            SuspendProcess(_process);
            _isSuspended = true;
            return true;
        }

        public bool Resume()
        {
            // Resume all threads
            ResumeProcess(_process);
            _isSuspended = false;
            return true;
        }

        public bool ReadRelative(uint address, byte[] buffer, EndiannessType endianness)
        {
            return ReadAbsolute(GetAbsoluteAddress(address, buffer.Length), buffer, endianness);
        }

        static readonly byte[] _swapByteOrder = new byte[] { 0x03, 0x02, 0x01, 0x00 };
        public bool ReadAbsolute(UIntPtr address, byte[] buffer, EndiannessType endianness)
        {
            if (_process == null)
                return false;

            if (_emulator.Endianness == endianness)
            {
                int numOfBytes = 0;
                return ProcessReadMemory(_processHandle, address, buffer, (IntPtr)buffer.Length, ref numOfBytes);
            }
            else
            {
                // Read padded if misaligned address
                byte[] swapBytes;
                UIntPtr alignedAddress = EndiannessUtilities.AlignedAddressFloor(address);
                if (EndiannessUtilities.AddressIsMisaligned(address))
                    swapBytes = new byte[buffer.Length + 4];
                else
                    swapBytes = new byte[buffer.Length];

                // Read memory
                int numOfBytes = 0;
                if (!ProcessReadMemory(_processHandle, alignedAddress, swapBytes, (IntPtr)swapBytes.Length, ref numOfBytes))
                    return false;

                // Copy and swap bytes
                int index = (int)(address.ToUInt64() - alignedAddress.ToUInt64());
                for (int i = 0; i < buffer.Length; i++, index++)
                    buffer[i] = swapBytes[index & ~0x03 | _swapByteOrder[index & 0x03]]; // Swap bytes

                return true;
            }
        }

        public bool WriteRelative(uint address, byte[] buffer, EndiannessType endianness)
        {
            return WriteAbsolute(GetAbsoluteAddress(address, buffer.Length), buffer, endianness);
        }

        public bool WriteAbsolute(UIntPtr address, byte[] buffer, EndiannessType endianness)
        {
            int numOfBytes = 0;

            // Safety bounds check
            if (address.ToUInt64() < _baseOffset.ToUInt64()
                || address.ToUInt64() + (uint)buffer.Length >= _baseOffset.ToUInt64() + _ramSize)
                return false;

            if (_emulator.Endianness == endianness)
            {
                return ProcessWriteMemory(_processHandle, address,
                    buffer, (IntPtr)buffer.Length, ref numOfBytes);
            }
            else
            {
                bool success = true;
                IEnumerable<byte> bytes = buffer;

                int numberToWrite = Math.Min(EndiannessUtilities.NumberOfBytesToAlignment(address), buffer.Length);
                if (numberToWrite > 0)
                {
                    byte[] toWrite = bytes.Take(numberToWrite).Reverse().ToArray();
                    success &= ProcessWriteMemory(_processHandle, address,
                        toWrite, (IntPtr)toWrite.Length, ref numOfBytes);

                    bytes = bytes.Skip(toWrite.Length);
                    address = EndiannessUtilities.AlignedAddressCeil(address);
                }

                numberToWrite = bytes.Count();
                if (bytes.Count() >= 4)
                {
                    byte[] toWrite = EndiannessUtilities.SwapByteEndianness(bytes.Take(bytes.Count() & ~0x03).ToArray());

                    success &= ProcessWriteMemory(_processHandle, address,
                        toWrite, (IntPtr)toWrite.Length, ref numOfBytes);

                    bytes = bytes.Skip(toWrite.Length);
                    address += toWrite.Length;
                }

                numberToWrite = bytes.Count();
                if (numberToWrite > 0)
                {
                    byte[] toWrite = bytes.Reverse().ToArray();

                    success &= ProcessWriteMemory(_processHandle, address,
                        buffer, (IntPtr)buffer.Length, ref numOfBytes);
                }

                return success;
            }
        }

        public UIntPtr GetAbsoluteAddress(uint n64Address, int size)
        {
            n64Address &= ~0x80000000U;
            UIntPtr absoluteAddress = (UIntPtr)(_baseOffset.ToUInt64() + n64Address);
            if (_emulator.Endianness == EndiannessType.Big)
                return absoluteAddress;
            else
                return EndiannessUtilities.SwapAddressEndianness(absoluteAddress, size);
        }

        public uint GetRelativeAddress(UIntPtr absoluteAddress, int size)
        {
            uint n64address = 0x80000000 | (uint)(absoluteAddress.ToUInt64() - _baseOffset.ToUInt64());
            if (_emulator.Endianness == EndiannessType.Big)
                return n64address;
            else
                return EndiannessUtilities.SwapAddressEndianness(n64address, size);
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
