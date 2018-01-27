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
        public static uint ObjectReferenceAddress { get { return Config.SwitchRomVersion(ObjectReferenceAddressUS, ObjectReferenceAddressJP); } }
        public static readonly uint ObjectReferenceAddressUS = 0x80361158;
        public static readonly uint ObjectReferenceAddressJP = 0x8035FDE8;

        public static readonly uint ObjectAnimationOffset = 0x38;
        public static readonly uint ObjectAnimationTimerOffset = 0x40;

        public static readonly uint MarioGraphic = 0x800F0860;
        public static readonly uint MarioBehavior = 0x13002EC0;
    }
}
