using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class HackConfig
    {
        public static RomHack SpawnHack;
        public static readonly uint BehaviorAddress = 0x8033D3D0;
        public static readonly uint GfxIdAddress = 0x8033D3D4;
        public static readonly uint ExtraAddress = 0x8033D3D8;
    }
}
