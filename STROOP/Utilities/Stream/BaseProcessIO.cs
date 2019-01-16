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
    public abstract class BaseProcessIO : IEmuRamIO
    {
        protected uint _ramSize;

        public abstract event EventHandler OnClose;

        protected abstract bool WriteFunc(UIntPtr address, byte[] buffer);
        protected abstract bool ReadFunc(UIntPtr address, byte[] buffer);
        protected abstract UIntPtr BaseOffset { get; }
        protected abstract EndiannessType Endianness { get; }

        public abstract bool IsSuspended { get; }

        public abstract string Name { get; }

        public abstract bool Suspend();
        public abstract bool Resume();

        public BaseProcessIO(uint ramSize)
        {
            _ramSize = ramSize;
        }

        public bool ReadRelative(uint address, byte[] buffer, EndiannessType endianness)
        {
            return ReadAbsolute(GetAbsoluteAddress(address, buffer.Length), buffer, endianness);
        }

        static readonly byte[] _swapByteOrder = new byte[] { 0x03, 0x02, 0x01, 0x00 };
        public bool ReadAbsolute(UIntPtr address, byte[] buffer, EndiannessType endianness)
        {
            if (Endianness == endianness)
            {
                return ReadFunc(address, buffer);
            }
            else
            {
                // Not a between alignment, or aligned write
                if (buffer.Length >= 4 && (buffer.Length % 4 != 0))
                    throw new Exception("Misaligned data");

                address = EndiannessUtilities.SwapAddressEndianness(address, buffer.Length);
                byte[] readBytes = new byte[buffer.Length];
                if (!ReadFunc(address, readBytes))
                    return false;

                readBytes = EndiannessUtilities.SwapByteEndianness(readBytes);
                Buffer.BlockCopy(readBytes, 0, buffer, 0, buffer.Length);
                
                return true;
            }
        }

        public bool WriteRelative(uint address, byte[] buffer, EndiannessType endianness)
        {
            return WriteAbsolute(GetAbsoluteAddress(address, buffer.Length), buffer, endianness);
        }

        public bool WriteAbsolute(UIntPtr address, byte[] buffer, EndiannessType endianness)
        {
            // Safety bounds check
            if (address.ToUInt64() < BaseOffset.ToUInt64()
                || address.ToUInt64() + (uint)buffer.Length >= BaseOffset.ToUInt64() + _ramSize)
                return false;
            
            if (Endianness == endianness)
            {
                return WriteFunc(address, buffer);
            }
            else
            {
                bool success = true;
                byte[] toWrite = EndiannessUtilities.SwapByteEndianness(buffer);

                // Between alignment writes
                if (buffer.Length < 4)
                {
                    address = EndiannessUtilities.SwapAddressEndianness(address, toWrite.Length);
                    success &= WriteFunc(address, toWrite);
                }
                else if (buffer.Length % 4 == 0) // Full alignment writes
                { 
                    success &= WriteFunc(address, toWrite);
                }
                else 
                {
                    throw new Exception("Misaligned data");
                }

                return success;
            }
        }

        public UIntPtr GetAbsoluteAddress(uint n64Address, int size)
        {
            n64Address &= ~0x80000000U;
            UIntPtr absoluteAddress = (UIntPtr)(BaseOffset.ToUInt64() + n64Address);
            return EndiannessUtilities.SwapAddressEndianness(absoluteAddress, size);
        }

        public uint GetRelativeAddress(UIntPtr absoluteAddress, int size)
        {
            uint n64address = 0x80000000 | (uint)(absoluteAddress.ToUInt64() - BaseOffset.ToUInt64());
            return EndiannessUtilities.SwapAddressEndianness(n64address, size);
        }
    }
}
