using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class EndianessUtilitiies
    {
        static readonly byte[] _fixAddress = { 0x00, 0x03, 0x02, 0x01, 0x00 };

        public static UIntPtr SwapAddressEndianess(UIntPtr address, int dataSize)
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

        public static uint SwapAddressEndianess(uint address, int dataSize)
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
    }
}
