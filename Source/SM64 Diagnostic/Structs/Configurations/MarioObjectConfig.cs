using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class MarioObjectConfig
    {
        public static uint PointerAddress { get { return Config.SwitchRomVersion(PointerAddressUS, PointerAddressJP); } }
        public static readonly uint PointerAddressUS = 0x80361158;
        public static readonly uint PointerAddressJP = 0x8035FDE8;

        public static readonly uint AnimationOffset = 0x38;
        public static readonly uint AnimationTimerOffset = 0x40;

        public static readonly uint GraphicValue = 0x800F0860;
        public static readonly uint BehaviorValue = 0x13002EC0;
    }
}
