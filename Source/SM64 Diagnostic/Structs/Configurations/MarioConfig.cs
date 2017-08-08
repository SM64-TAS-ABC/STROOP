using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct MarioConfig
    {
        public uint StructAddress { get { return Config.SwitchRomVersion(StructAddressUS, StructAddressJP); } }
        public uint StructAddressUS;
        public uint StructAddressJP;

        public uint ActionOffset;
        public uint PrevActionOffset;
        public uint XOffset;
        public uint YOffset;
        public uint ZOffset;
        public uint RotationOffset;
        public uint YawFacingOffset;
        public uint YawIntendedOffset;

        public uint StoodOnObjectPointer { get { return Config.SwitchRomVersion(StoodOnObjectPointerUS, StoodOnObjectPointerJP); } }
        public uint StoodOnObjectPointerUS;
        public uint StoodOnObjectPointerJP;

        public uint InteractionObjectPointerOffset;
        public uint HeldObjectPointerOffset;
        public uint UsedObjectPointerOffset;
        public uint FloorTriangleOffset;
        public uint WallTriangleOffset;
        public uint CeilingTriangleOffset;
        public uint HSpeedOffset;
        public uint VSpeedOffset;
        public uint GroundYOffset;
        public uint CeilingYOffset;
        public uint StructSize;
        public uint SlidingSpeedXOffset;
        public uint SlidingSpeedZOffset;
        public uint PeakHeightOffset;

        public uint ObjectReferenceAddress { get { return Config.SwitchRomVersion(ObjectReferenceAddressUS, ObjectReferenceAddressJP); } }
        public uint ObjectReferenceAddressUS;
        public uint ObjectReferenceAddressJP;

        public uint ObjectAnimationOffset;
        public uint ObjectAnimationTimerOffset;
        public uint HOLPXOffset;
        public uint HOLPYOffset;
        public uint HOLPZOffset;
    }
}
