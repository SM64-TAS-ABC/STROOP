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
                    if (valUS.HasValue) return valUS.Value;
                    break;
                case RomVersion.JP:
                    if (valJP.HasValue) return valJP.Value;
                    break;
                case RomVersion.PAL:
                    if (valPAL.HasValue) return valPAL.Value;
                    break;
            }
            return 0;
        }
        public static ushort SwitchRomVersion(ushort? valUS = null, ushort? valJP = null, ushort? valPAL = null)
        {
            switch (Version)
            {
                case RomVersion.US:
                    if (valUS.HasValue) return valUS.Value;
                    break;
                case RomVersion.JP:
                    if (valJP.HasValue) return valJP.Value;
                    break;
                case RomVersion.PAL:
                    if (valPAL.HasValue) return valPAL.Value;
                    break;
            }
            return 0;
        }

        public static ProcessStream Stream;
        public static ObjectAssociations ObjectAssociations;

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
        public static ActionTable MarioActions;
        public static AnimationTable MarioAnimations;
        public static PendulumSwingTable PendulumSwings;
        public static WaypointTable RacingPenguinWaypoints;
        public static WaypointTable KoopaTheQuick1Waypoints;
        public static WaypointTable KoopaTheQuick2Waypoints;
        public static MissionTable Missions;
        public static CourseDataTable CourseData;
        public static FlyGuyDataTable FlyGuyData;
        public static PositionControllerRelativeAngleConfig PositionControllerRelativeAngle;
    }
}
