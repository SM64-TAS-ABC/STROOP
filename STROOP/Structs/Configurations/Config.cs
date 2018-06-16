using STROOP.Managers;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs.Configurations
{
    public static class Config
    {
        public static uint RamSize;

        public static List<Emulator> Emulators = new List<Emulator>();
        public static ProcessStream Stream;
        public static ObjectAssociations ObjectAssociations;
        public static StroopMainForm StroopMainForm;
        public static TabControl TabControlMain;

        public static CameraManager CameraManager;
        public static DebugManager DebugManager;
        public static DisassemblyManager DisassemblyManager;
        public static DecompilerManager DecompilerManager;
        public static HackManager HackManager;
        public static HudManager HudManager;
        public static MapManager MapManager;
        public static Map2Manager Map2Manager;
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
        public static CamHackManager CamHackManager;
        public static MemoryManager MemoryManager;
        public static CoinManager CoinManager;
        public static ScriptManager ScriptManager;
        public static GfxManager GfxManager;
        public static M64Manager M64Manager;

        public static List<DataManager> GetDataManagers()
        {
            List<DataManager> dataManagerList =
                ControlUtilities.GetFieldsOfType<DataManager>(typeof(Config), null);
            dataManagerList.Sort((d1, d2) => d1.TabIndex - d2.TabIndex);
            return dataManagerList;
        }
    }
}
