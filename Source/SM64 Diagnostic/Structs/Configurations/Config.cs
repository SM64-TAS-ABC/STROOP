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
        public static InputConfig Input;
        public static FileConfig File;
        public static CameraHackConfig CameraHack;
        public static TriangleOffsetsConfig TriangleOffsets;
        public static ActionTable MarioActions;
        public static AnimationTable MarioAnimations;
        public static PendulumSwingTable PendulumSwings;
        public static MissionTable Missions;
        public static CourseDataTable CourseData;
        public static GotoRetrieveConfig GotoRetrieve;
        public static uint LevelAddress;
        public static uint AreaAddress;
        public static uint LoadingPointAddress;
        public static uint MissionAddress;
        public static uint RngRecordingAreaAddress;
        public static uint RngAddress;
        public static uint GlobalTimerAddress;

        public static bool SlotIndexsFromOne = true;
        public static bool MoveCameraWithPu = true;
        public static bool ShowOverlayHeldObject = true;
        public static bool ShowOverlayStoodOnObject = true;
        public static bool ShowOverlayInteractionObject = true;
        public static bool ShowOverlayUsedObject = true;
        public static bool ShowOverlayCameraObject = true;
        public static bool ShowOverlayCameraHackObject = true;
        public static bool ShowOverlayClosestObject = true;
        public static bool ShowOverlayFloorObject = true;
        public static bool ShowOverlayWallObject = true;
        public static bool ShowOverlayCeilingObject = true;
        public static bool ShowOverlayParentObject = false;
        public static bool ScaleDiagonalPositionControllerButtons = false;
        public static bool PositionControllersRelativeToMario = false;
        public static bool DisableActionUpdateWhenCloning = false;
        public static bool NeutralizeTriangleWith21 = true;
    }
}
