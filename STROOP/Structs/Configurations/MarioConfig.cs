using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class MarioConfig
    {
        public static uint StructAddress { get => RomVersionConfig.Switch(StructAddressUS, StructAddressJP); }
        public static readonly uint StructAddressUS = 0x8033B170;
        public static readonly uint StructAddressJP = 0x80339E00;

        public static readonly uint XOffset = 0x3C;
        public static readonly uint YOffset = 0x40;
        public static readonly uint ZOffset = 0x44;

        public static readonly uint FacingYawOffset = 0x2E;
        public static readonly uint FacingPitchOffset = 0x2C;
        public static readonly uint FacingRollOffset = 0x30;

        public static readonly uint IntendedYawOffset = 0x24;
        public static readonly uint IntendedPitchOffset = 0x22;
        public static readonly uint IntendedRollOffset = 0x26;

        public static readonly uint HSpeedOffset = 0x54;
        public static readonly uint VSpeedOffset = 0x4C;

        public static readonly uint SlidingSpeedXOffset = 0x58;
        public static readonly uint SlidingSpeedZOffset = 0x5C;
        public static readonly uint SlidingYawOffset = 0x38;

        public static readonly uint HolpXOffset = 0x258;
        public static readonly uint HolpYOffset = 0x25C;
        public static readonly uint HolpZOffset = 0x260;

        public static uint StoodOnObjectPointerAddress { get => RomVersionConfig.Switch(StoodOnObjectPointerAddressUS, StoodOnObjectPointerAddressJP); }
        public static readonly uint StoodOnObjectPointerAddressUS = 0x80330E34;
        public static readonly uint StoodOnObjectPointerAddressJP = 0x8032FED4;

        public static readonly uint InteractionObjectPointerOffset = 0x78;
        public static readonly uint HeldObjectPointerOffset = 0x7C;
        public static readonly uint UsedObjectPointerOffset = 0x80;

        public static readonly uint WallTriangleOffset = 0x60;
        public static readonly uint FloorTriangleOffset = 0x68;
        public static readonly uint CeilingTriangleOffset = 0x64;

        public static readonly uint FloorYOffset = 0x70;
        public static readonly uint CeilingYOffset = 0x6C;

        public static readonly uint ActionOffset = 0x0C;
        public static readonly uint PrevActionOffset = 0x10;

        public static readonly uint TwirlYawOffset = 0x3A;
        public static readonly uint PeakHeightOffset = 0xBC;
        public static readonly uint WaterLevelOffset = 0x76;
        public static readonly uint AreaPointerOffset = 0x90;
    }
}
