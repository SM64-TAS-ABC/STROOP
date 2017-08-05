using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class Config
    {
        public enum RomVersion { US, JP, PAL };
        public static RomVersion Version = RomVersion.US;
        public static uint SwitchRomVersion(uint? valUS = null, uint? valJP = null, uint? valPAL = null)
        {
            switch (Version)
            {
                case RomVersion.US:
                    if (valUS != null) return (uint)valUS;
                    break;
                case RomVersion.JP:
                    if (valJP != null) return (uint)valJP;
                    break;
                case RomVersion.PAL:
                    if (valPAL != null) return (uint)valPAL;
                    break;
            }
            return 0;
        }

        public static ProcessStream Stream;
        public static ObjectAssociations ObjectAssociations;
        public static ObjectGroupsConfig ObjectGroups;
        public static ObjectSlotsConfig ObjectSlots;

        public static int RefreshRateFreq;
        public static List<Emulator> Emulators = new List<Emulator>();
        public static uint RamSize;
        public static MarioConfig Mario;
        public static HudConfig Hud;
        public static HackConfig Hacks;
        public static DebugConfig Debug;
        public static CameraConfig Camera;
        public static InputConfig Input;
        public static FileConfig File;
        public static WaypointConfig Waypoint;
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

        public static uint LevelIndexAddress { get { return Config.SwitchRomVersion(LevelIndexAddressUS, LevelIndexAddressJP); } }
        public static uint LevelIndexAddressUS;
        public static uint LevelIndexAddressJP;

        public static uint HackedAreaAddress;

        public static uint GlobalTimerAddress { get { return Config.SwitchRomVersion(GlobalTimerAddressUS, GlobalTimerAddressJP); } }
        public static uint GlobalTimerAddressUS;
        public static uint GlobalTimerAddressJP;

        public static uint RngAddress { get { return Config.SwitchRomVersion(RngAddressUS, RngAddressJP); } }
        public static uint RngAddressUS;
        public static uint RngAddressJP;

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
