using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public struct HackConfig
    {
        public RomHack SpawnHack;
        public uint BehaviorAddress;
        public uint GfxIdAddress;
        public uint ExtraAddress;
    }
}
