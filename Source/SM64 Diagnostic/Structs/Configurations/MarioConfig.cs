using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct MarioConfig
    {
        public uint StructAddress;
        public uint ActionOffset;
        public uint PrevActionOffset;
        public uint XOffset;
        public uint YOffset;
        public uint ZOffset;
        public uint RotationOffset;
        public uint YawFacingOffset;
        public uint YawIntendedOffset;
        public uint StoodOnObjectPointer;
        public uint InteractionObjectPointerOffset;
        public uint HeldObjectPointerOffset;
        public uint UsedObjectPointerOffset;
        public float MoveToObjectYOffset;
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
        public uint ObjectReferenceAddress;
        public uint ObjectAnimationOffset;
        public uint ObjectAnimationTimerOffset;
        public uint HOLPXOffset;
        public uint HOLPYOffset;
        public uint HOLPZOffset;
    }
}
