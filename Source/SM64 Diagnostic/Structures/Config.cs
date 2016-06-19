using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public struct Config
    {
        public int RefreshRateFreq;
        public string ProcessName;
        public uint RamStartAddress;
        public uint RamSize;
        public ObjectGroupsConfig ObjectGroups;
        public ObjectSlotsConfig ObjectSlots;
        public MarioConfig Mario;
        public uint LevelAddress;
        public uint AreaAddress;
        public bool SlotIndexsFromOne;
    }
}
