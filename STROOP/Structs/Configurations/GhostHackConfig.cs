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

        // return new List<uint> { 0x80400000 + 0x10 * (Config.Stream.GetUInt32(0x804003FC) + 67) };
    }
}
