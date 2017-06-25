using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class Config
    {
        public static int RefreshRateFreq;
        public static List<Emulator> Emulators = new List<Emulator>();
        public static uint RamSize;
        public static ObjectGroupsConfig ObjectGroups;
        public static ObjectSlotsConfig ObjectSlots;
        public static MarioConfig Mario;
        public static HudConfig Hud;
        public static HackConfig Hacks;
        public static DebugConfig Debug;
        public static CameraConfig Camera;
        public static TriangleOffsetsConfig TriangleOffsets;
        public static ActionTable MarioActions;
        public static uint LevelAddress;
        public static uint AreaAddress;
        public static uint LoadingPointAddress;
        public static uint MissionAddress;
        public static uint HolpX;
        public static uint HolpY;
        public static uint HolpZ;
        public static uint RngRecordingAreaAddress;
        public static uint RngAddress;
      
        public static bool SlotIndexsFromOne;
        public static bool MoveCameraWithPu = true;
        public static bool ShowOverlays = true;
        public static bool ScaleDiagonalPositionControllerButtons = false;
    }
}
