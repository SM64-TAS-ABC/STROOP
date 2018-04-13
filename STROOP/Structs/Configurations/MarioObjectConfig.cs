using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class MarioObjectConfig
    {
        public static uint PointerAddress { get { return RomVersionConfig.Switch(PointerAddressUS, PointerAddressJP); } }
        public static readonly uint PointerAddressUS = 0x80361158;
        public static readonly uint PointerAddressJP = 0x8035FDE8;

        public static readonly uint AnimationOffset = 0x38;
        public static readonly uint AnimationTimerOffset = 0x40;

        public static readonly uint GraphicValue = 0x800F0860;
        public static readonly uint BehaviorValue = 0x13002EC0;
    }
}
