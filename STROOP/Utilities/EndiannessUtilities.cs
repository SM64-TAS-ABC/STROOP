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
                    return address;
            }
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
