using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public class VarHackConfig
    {
        public readonly uint VarHackMemoryAddress = 0x80370000;
        public readonly uint StructSize = 0x20;
        public readonly uint MaxPossibleVars = 432;

        public readonly uint AddressOffset = 0x00;
        public readonly uint XPosOffset = 0x04;
        public readonly uint YPosOffset = 0x06;
        public readonly uint StringOffset = 0x08;
        public readonly uint UsePointerOffset = 0x1B;
        public readonly uint PointerOffsetOffset = 0x1C;
        public readonly uint SignedOffset = 0x1E;
        public readonly uint TypeOffset = 0x1F;

        public readonly int MaxStringLength = 17;

        public RomHack ShowVarRomHack;
    }
}
