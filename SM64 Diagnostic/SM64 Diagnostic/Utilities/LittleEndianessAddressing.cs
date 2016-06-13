using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public unsafe static class LittleEndianessAddressing
    {
        public static int AddressFix(int address, int dataSize)
        {
            switch (dataSize)
            {
                case 1:
                    return (int)(address & 0xFFFFFFFC) | (0x03 - (address & 0x03));
                case 2:
                    return (int)(address & 0xFFFFFFFD) | (0x02 - (address & 0x02));
                default:
                    return address;
            }
        }
    }
}
