using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class GhostHackConfig
    {
        public static readonly uint MemoryAddress = 0x80400000;
        public static readonly uint FrameDataStructSize = 0x10;
        public static readonly uint NumFramesAddress = 0x804003FC;
        public static readonly uint FrameOffset = 67;
    }
}
