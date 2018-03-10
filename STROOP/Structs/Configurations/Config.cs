using STROOP.Managers;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class Config
    {
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
        public static uint RamSize;

        public static List<Emulator> Emulators = new List<Emulator>();
        public static ProcessStream Stream;
        public static ObjectAssociations ObjectAssociations;
        public static StroopMainForm StroopMainForm;

        public static CameraManager CameraManager;
        public static DebugManager DebugManager;
        public static DisassemblyManager DisassemblyManager;
        public static DecompilerManager DecompilerManager;
        public static HackManager HackManager;
        public static HudManager HudManager;
        public static MapManager MapManager;
        public static ModelManager ModelManager;
        public static MarioManager MarioManager;
        public static MiscManager MiscManager;
        public static ObjectManager ObjectManager;
        public static ObjectSlotsManager ObjectSlotsManager;
        public static OptionsManager OptionsManager;
        public static TestingManager TestingManager;
        public static InjectionManager InjectionManager;
        public static TriangleManager TriangleManager;
        public static DataManager WaterManager;
        public static InputManager InputManager;
        public static ActionsManager ActionsManager;
        public static PuManager PuManager;
        public static TasManager TasManager;
        public static FileManager FileManager;
        public static AreaManager AreaManager;
        public static DataManager QuarterFrameManager;
        public static DataManager CustomManager;
        public static VarHackManager VarHackManager;
        public static DataManager CameraHackManager;
        public static ScriptManager ScriptManager;
        public static GfxManager GfxManager;
    }
}
