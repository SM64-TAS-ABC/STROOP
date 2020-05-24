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
    }
}
