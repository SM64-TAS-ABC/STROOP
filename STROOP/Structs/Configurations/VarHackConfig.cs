using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class VarHackConfig
    {
        public static readonly uint VarHackMemoryAddress = 0x80370000;
        public static readonly uint StructSize = 0x20;
        public static readonly uint MaxPossibleVars = 432;

        public static readonly uint AddressOffset = 0x00;
        public static readonly uint XPosOffset = 0x04;
        public static readonly uint YPosOffset = 0x06;
        public static readonly uint StringOffset = 0x08;
        public static readonly uint UsePointerOffset = 0x1B;
        public static readonly uint PointerOffsetOffset = 0x1C;
        public static readonly uint SignedOffset = 0x1E;
        public static readonly uint TypeOffset = 0x1F;

        public static readonly int DefaultXPos = 10;
        public static readonly int DefaultYPos = 192;
        public static readonly int DefaultYDelta = 17;

        public static readonly int CharacterWidth = 8;
        public static readonly int CharacterHeight = 12;

        public static readonly string CoinChar = Char.ConvertFromUtf32(43);
        public static readonly string MarioHeadChar = Char.ConvertFromUtf32(44);
        public static readonly string StarChar = Char.ConvertFromUtf32(45);

        public static readonly int MaxStringLength = 17;

        public static RomHack ShowVarRomHack;
    }
}
