using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class GhostHackConfig
    {
        public static readonly uint DataStartAddress = 0x80400490;
        public static readonly uint DataStructSize = 0x30;
        public static readonly uint NumFramesAddress = 0x804003FC;
        public static uint CurrentGhostStruct { get => DataStartAddress + Config.Stream.GetUInt32(NumFramesAddress) * DataStructSize; }

        public static readonly uint XOffset = 0x00;
        public static readonly uint YOffset = 0x28;
        public static readonly uint ZOffset = 0x08;
        public static readonly uint HSpeedOffset = 0x18;
        public static readonly uint YSpeedOffset = 0x1C;
        public static readonly uint YawFacingOffset = 0x06;
        public static readonly uint YawIntendedOffset = 0x24;
        public static readonly uint ActionOffset = 0x20;
    }
}
