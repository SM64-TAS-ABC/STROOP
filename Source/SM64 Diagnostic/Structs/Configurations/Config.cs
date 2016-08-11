using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Structs
{
    public class Config
    {
        public int RefreshRateFreq;
        public string ProcessName;
        public uint RamStartAddress;
        public uint RamSize;
        public ObjectGroupsConfig ObjectGroups;
        public ObjectSlotsConfig ObjectSlots;
        public MarioConfig Mario;
        public DebugConfig Debug;
        public TriangleOffsetsConfig TriangleOffsets;
        public uint LevelAddress;
        public uint AreaAddress;
        public uint LoadingPointAddress;
        public uint MissionAddress;
        public bool SlotIndexsFromOne;
        public uint HolpX;
        public uint HolpY;
        public uint HolpZ;
        public uint CameraX;
        public uint CameraY;
        public uint CameraZ;
        public uint CameraRot;
        public uint RngRecordingAreaAddress;
        public uint RngAddress;
    }
}
