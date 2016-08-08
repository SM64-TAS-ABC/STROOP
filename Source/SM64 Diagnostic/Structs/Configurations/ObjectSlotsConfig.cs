﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct ObjectSlotsConfig
    {
        public uint LinkStartAddress;
        public uint StructSize;
        public uint HeaderOffset;
        public uint NextLinkOffset;
        public uint PreviousLinkOffset;
        public uint BehaviorScriptOffset;
        public uint ObjectActiveOffset;
        public uint ObjectXOffset;
        public uint ObjectYOffset;
        public uint ObjectZOffset;
        public uint ObjectRotationOffset;
        public float MoveToMarioYOffset;
        public int MaxSlots;
    }
}
