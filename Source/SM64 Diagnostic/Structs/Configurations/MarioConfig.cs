﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct MarioConfig
    {
        public uint MarioStructAddress;
        public uint ActionOffset;
        public uint XOffset;
        public uint YOffset;
        public uint ZOffset;
        public uint RotationOffset;
        public uint StandingOngObjectPointer;
        public uint InteractingObjectPointerOffset;
        public uint HoldingObjectPointerOffset;
        public float MoveToObjectYOffset;
        public uint FloorTriangleOffset;
        public uint HSpeedOffset;
        public uint GroundYOffset;
        public uint CeilingYOffset;
        public uint StructSize;
        public uint SlidingSpeedXOffset;
        public uint SlidingSpeedZOffset;
    }
}
