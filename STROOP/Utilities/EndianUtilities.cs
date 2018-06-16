using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class EndianUtilities
    {
        public static int SwapEndianness(int address)
        {
            return SwapEndianness(address, 1);
        }

        public static int SwapEndianness(int address, int structSize)
        {
            if (structSize == 4)
            {
                return address;
            }
            if (structSize == 2)
            {
                int baseValue = (address / 4) * 4;
                int modValue = address % 4;
                return baseValue + (2 - modValue);
            }
            if (structSize == 1)
            {
                int baseValue = (address / 4) * 4;
                int modValue = address % 4;
                return baseValue + (3 - modValue);
            }
            throw new ArgumentOutOfRangeException();
        }

        public static uint SwapEndianness(uint address)
        {
            return SwapEndianness(address, 1);
        }

        public static uint SwapEndianness(uint address, int structSize)
        {
            if (structSize == 4)
            {
                return address;
            }
            if (structSize == 2)
            {
                uint baseValue = (address / 4) * 4;
                uint modValue = address % 4;
                return baseValue + (2 - modValue);
            }
            if (structSize == 1)
            {
                uint baseValue = (address / 4) * 4;
                uint modValue = address % 4;
                return baseValue + (3 - modValue);
            }
            throw new ArgumentOutOfRangeException();
        }

    }
}
