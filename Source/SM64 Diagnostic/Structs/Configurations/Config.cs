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
        public static ushort SwitchRomVersion(ushort? valUS = null, ushort? valJP = null, ushort? valPAL = null)
        {
            switch (Version)
            {
                case RomVersion.US:
                    if (valUS != null) return (ushort)valUS;
                    break;
                case RomVersion.JP:
                    if (valJP != null) return (ushort)valJP;
                    break;
                case RomVersion.PAL:
                    if (valPAL != null) return (ushort)valPAL;
                    break;
            }
            return 0;
        }

        public static ProcessStream Stream;
        public static ObjectAssociations ObjectAssociations;
        public static ObjectGroupsConfig ObjectGroups;
        public static ObjectSlotsConfig ObjectSlots;

        public static uint RefreshRateFreq;
        public static double RefreshRateInterval
        {
            get
            {
                uint freq = RefreshRateFreq;
                if (freq == 0) return 0;
                else return 1000.0 / freq;
            }
        }

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
        public static VarHackConfig VarHack = new VarHackConfig();
        public static TriangleOffsetsConfig TriangleOffsets;
        public static TriangleConfig Triangle;
        public static ActionTable MarioActions;
        public static AnimationTable MarioAnimations;
        public static PendulumSwingTable PendulumSwings;
        public static WaypointTable RacingPenguinWaypoints;
        public static WaypointTable KoopaTheQuick1Waypoints;
        public static WaypointTable KoopaTheQuick2Waypoints;
        public static MissionTable Missions;
        public static CourseDataTable CourseData;
        public static FlyGuyDataTable FlyGuyData;
        public static GotoRetrieveConfig GotoRetrieve;
        public static PositionControllerRelativeAngleConfig PositionControllerRelativeAngle;

        public static uint LevelAddress { get { return Config.SwitchRomVersion(LevelAddressUS, LevelAddressJP); } }
        public static uint LevelAddressUS;
        public static uint LevelAddressJP;

        public static uint AreaAddress { get { return Config.SwitchRomVersion(AreaAddressUS, AreaAddressJP); } }
        public static uint AreaAddressUS;
        public static uint AreaAddressJP;

        public static uint LoadingPointAddress { get { return Config.SwitchRomVersion(LoadingPointAddressUS, LoadingPointAddressJP); } }
        public static uint LoadingPointAddressUS;
        public static uint LoadingPointAddressJP;

        public static uint MissionAddress { get { return Config.SwitchRomVersion(MissionAddressUS, MissionAddressJP); } }
        public static uint MissionAddressUS;
        public static uint MissionAddressJP;

        public static uint LevelIndexAddress { get { return Config.SwitchRomVersion(LevelIndexAddressUS, LevelIndexAddressJP); } }
        public static uint LevelIndexAddressUS;
        public static uint LevelIndexAddressJP;

        public static uint WaterLevelMedianAddress { get { return Config.SwitchRomVersion(WaterLevelMedianAddressUS, WaterLevelMedianAddressJP); } }
        public static uint WaterLevelMedianAddressUS;
        public static uint WaterLevelMedianAddressJP;

        public static uint WaterPointerAddress { get { return Config.SwitchRomVersion(WaterPointerAddressUS, WaterPointerAddressJP); } }
        public static uint WaterPointerAddressUS;
        public static uint WaterPointerAddressJP;

        public static uint CurrentFileAddress { get { return Config.SwitchRomVersion(CurrentFileAddressUS, CurrentFileAddressJP); } }
        public static uint CurrentFileAddressUS;
        public static uint CurrentFileAddressJP;

        public static uint SpecialTripleJumpAddress { get { return Config.SwitchRomVersion(SpecialTripleJumpAddressUS, SpecialTripleJumpAddressJP); } }
        public static uint SpecialTripleJumpAddressUS;
        public static uint SpecialTripleJumpAddressJP;

        public static uint HackedAreaAddress { get { return Config.SwitchRomVersion(HackedAreaAddressUS, HackedAreaAddressJP); } }
        public static uint HackedAreaAddressUS;
        public static uint HackedAreaAddressJP;

        public static uint GlobalTimerAddress { get { return Config.SwitchRomVersion(GlobalTimerAddressUS, GlobalTimerAddressJP); } }
        public static uint GlobalTimerAddressUS;
        public static uint GlobalTimerAddressJP;

        public static uint RngAddress { get { return Config.SwitchRomVersion(RngAddressUS, RngAddressJP); } }
        public static uint RngAddressUS;
        public static uint RngAddressJP;

        public static uint AnimationTimerAddress { get { return Config.SwitchRomVersion(AnimationTimerAddressUS, AnimationTimerAddressJP); } }
        public static uint AnimationTimerAddressUS;
        public static uint AnimationTimerAddressJP;

        public static uint MusicOnAddress { get { return Config.SwitchRomVersion(MusicOnAddressUS, MusicOnAddressJP); } }
        public static uint MusicOnAddressUS;
        public static uint MusicOnAddressJP;

        public static byte MusicOnMask;

        public static uint MusicVolumeAddress { get { return Config.SwitchRomVersion(MusicVolumeAddressUS, MusicVolumeAddressJP); } }
        public static uint MusicVolumeAddressUS;
        public static uint MusicVolumeAddressJP;

        public static bool SlotIndexsFromOne = true;
        public static bool MoveCameraWithPu = true;
        public static bool ScaleDiagonalPositionControllerButtons = false;
        public static bool ExcludeDustForClosestObject = true;
        public static bool NeutralizeTrianglesWith21 = true;
        public static short NeutralizeTriangleValue(bool? use21Nullable = null)
        {
            bool use21 = use21Nullable ?? NeutralizeTrianglesWith21;
            return (short)(use21 ? 21 : 0);
        }
        public static bool UseMisalignmentOffsetForDistanceToLine = true;

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
    }
}
