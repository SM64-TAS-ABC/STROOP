using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class EndiannessUtilities
    {
        static readonly byte[] _fixAddress = { 0x00, 0x03, 0x02, 0x01, 0x00 };

        public static UIntPtr SwapAddressEndianness(UIntPtr address, int dataSize)
        {
            switch (dataSize)
            {
                case 1:
                case 2:
                case 3:
                    return new UIntPtr((address.ToUInt64() & ~0x03UL) | (_fixAddress[dataSize] - address.ToUInt64() & 0x03UL));
                default:
                    if (AddressIsMisaligned(address))
                        throw new Exception("Misaligned data");
                    return address;
            }
        }

        public static uint SwapAddressEndianness(uint address, int dataSize)
        {
            switch (dataSize)
            {
                case 1:
                case 2:
                case 3:
                    return (uint)(address & ~0x03) | (_fixAddress[dataSize] - address & 0x03);
                default:
                    if (AddressIsMisaligned(address))
                        throw new Exception("Misaligned data");
                    return address;
            }
        }

        public static bool DataIsMisaligned(UIntPtr address, int dataSize, EndiannessType endianness)
        {
            // Get the number of bytes remaining to alignment for the address
            int bytesToAlignment = NumberOfBytesToAlignment(address);
            if (endianness == EndiannessType.Little) // Little endianess goes backwards in count, not forwards
                bytesToAlignment = _fixAddress[bytesToAlignment];

            // All datasize greater than 4 must already have an aligned address, and an aligned data size (multiple of 4)
            if (dataSize >= 4)
                return (bytesToAlignment != 0 || dataSize % 4 != 0);

            // If we are already aligned, we really have 4 bytes remaining
            if (bytesToAlignment == 0)
                bytesToAlignment = 4;

            // Be sure the bytes fit in the remaining section and do go past the 4-byte alignment
            return (bytesToAlignment < dataSize);
        }
        
        public static bool AddressIsMisaligned(UIntPtr address)
        {
            return (address.ToUInt64() & 0x03) != 0;
        }

        public static bool AddressIsMisaligned(uint address)
        {
            return (address & 0x03) != 0;
        }

        static readonly byte[] _bytesToAlignment = new byte[] { 0x00, 0x03, 0x02, 0x01 };
        public static int NumberOfBytesToAlignment(UIntPtr address)
        {
            return _bytesToAlignment[address.ToUInt64() & 0x03];
        }

        public static int NumberOfBytesToAlignment(uint address)
        {
            return _bytesToAlignment[address & 0x03];
        }

        public static uint AlignedAddressFloor(uint address)
        {
            return (address & ~0x03U);
        }

        public static UIntPtr AlignedAddressFloor(UIntPtr address)
        {
            return (UIntPtr)(address.ToUInt64() & ~0x03U);
        }

        public static uint AlignedAddressCeil(uint address)
        {
            return ((address & ~0x03U) + 4);
        }

        public static UIntPtr AlignedAddressCeil(UIntPtr address)
        {
            return (UIntPtr)((address.ToUInt64() & ~0x03U) + 4);
        }

        public static byte[] SwapByteEndianness(byte[] bytes)
        {
            if (bytes.Length < 3)
                return bytes.Reverse().ToArray();

            if (bytes.Length % 4 != 0)
                throw new ArgumentException("Bytes are not a multiple of 4");

            byte[] result = new byte[bytes.Length];
            for (int i = 0; i < bytes.Length; i += 4)
            {
                result[i]       = bytes[i + 3];
                result[i + 1]   = bytes[i + 2];
                result[i + 2]   = bytes[i + 1];
                result[i + 3]   = bytes[i];
            }
            return result;
        }
    }
}
