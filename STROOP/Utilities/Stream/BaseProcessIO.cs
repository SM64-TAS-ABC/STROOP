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
    abstract class BaseProcessIO : IEmuRamIO
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
                int numOfBytes = 0;
                return ReadFunc(address, buffer);
            }
            else
            {
                // Read padded if misaligned address
                byte[] swapBytes;
                UIntPtr alignedAddress = EndiannessUtilities.AlignedAddressFloor(address);
                swapBytes = new byte[(buffer.Length / 4) * 4 + 8];

                // Read memory
                int numOfBytes = 0;
                if (!ReadFunc(address, buffer))
                    return false;

                // Un-aligned
                int i = Math.Min(EndiannessUtilities.NumberOfBytesToAlignment(address), buffer.Length);
                if (i > 0) 
                    swapBytes.Take(i).Reverse().ToArray().CopyTo(buffer, 0);

                // Copy and swap bytes
                int index = i == 0 ? 0 : 4;
                for (; i < buffer.Length; i++, index++)
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
                IEnumerable<byte> bytes = buffer;

                int numberToWrite = Math.Min(EndiannessUtilities.NumberOfBytesToAlignment(address), buffer.Length);
                if (numberToWrite > 0)
                {
                    byte[] toWrite = bytes.Take(numberToWrite).Reverse().ToArray();
                    success &= WriteFunc(EndiannessUtilities.AlignedAddressFloor(address), toWrite);

                    bytes = bytes.Skip(toWrite.Length);
                    address += toWrite.Length;
                }

                numberToWrite = bytes.Count();
                if (bytes.Count() >= 4)
                {
                    byte[] toWrite = EndiannessUtilities.SwapByteEndianness(bytes.Take(bytes.Count() & ~0x03).ToArray());

                    success &= WriteFunc(address, toWrite);

                    bytes = bytes.Skip(toWrite.Length);
                    address += toWrite.Length;
                }

                numberToWrite = bytes.Count();
                if (numberToWrite > 0)
                {
                    byte[] toWrite = bytes.Reverse().ToArray();
                    address = EndiannessUtilities.SwapAddressEndianness(address, toWrite.Length);

                    success &= WriteFunc(address, toWrite);
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
