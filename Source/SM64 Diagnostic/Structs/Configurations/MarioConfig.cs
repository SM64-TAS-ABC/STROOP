using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class MarioConfig
    {
        public static uint StructAddress { get { return Config.SwitchRomVersion(StructAddressUS, StructAddressJP); } }
        public static readonly uint StructAddressUS = 0x8033B170;
        public static readonly uint StructAddressJP = 0x80339E00;

        public static readonly uint ActionOffset = 0x0C;
        public static readonly uint PrevActionOffset = 0x10;
        public static readonly uint XOffset = 0x3C;
        public static readonly uint YOffset = 0x40;
        public static readonly uint ZOffset = 0x44;
        public static readonly uint RotationOffset = 0x2A;
        public static readonly uint YawFacingOffset = 0x2E;
        public static readonly uint YawIntendedOffset = 0x24;

        public static uint StoodOnObjectPointer { get { return Config.SwitchRomVersion(StoodOnObjectPointerUS, StoodOnObjectPointerJP); } }
        public static readonly uint StoodOnObjectPointerUS = 0x80330E34;
        public static readonly uint StoodOnObjectPointerJP = 0x8032FED4;

        public static readonly uint InteractionObjectPointerOffset = 0x78;
        public static readonly uint HeldObjectPointerOffset = 0x7C;
        public static readonly uint UsedObjectPointerOffset = 0x80;
        public static readonly uint FloorTriangleOffset = 0x68;
        public static readonly uint WallTriangleOffset = 0x60;
        public static readonly uint CeilingTriangleOffset = 0x64;
        public static readonly uint HSpeedOffset = 0x54;
        public static readonly uint VSpeedOffset = 0x4C;
        public static readonly uint FloorYOffset = 0x70;
        public static readonly uint CeilingYOffset = 0x6C;
        public static readonly uint StructSize = 0xC4;
        public static readonly uint SlidingSpeedXOffset = 0x58;
        public static readonly uint SlidingSpeedZOffset = 0x5C;
        public static readonly uint SlidingYawOffset = 0x38;
        public static readonly uint TwirlYawOffset = 0x3A;
        public static readonly uint PeakHeightOffset = 0xBC;

        public static uint ObjectReferenceAddress { get { return Config.SwitchRomVersion(ObjectReferenceAddressUS, ObjectReferenceAddressJP); } }
        public static readonly uint ObjectReferenceAddressUS = 0x80361158;
        public static readonly uint ObjectReferenceAddressJP = 0x8035FDE8;

        public static readonly uint ObjectAnimationOffset = 0x38;
        public static readonly uint ObjectAnimationTimerOffset = 0x40;
        public static readonly uint HOLPXOffset = 0x258;
        public static readonly uint HOLPYOffset = 0x25C;
        public static readonly uint HOLPZOffset = 0x260;

        public static readonly uint WaterLevelOffset = 0x76;
        public static readonly uint AreaPointerOffset = 0x90;
    }
}
