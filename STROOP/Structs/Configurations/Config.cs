using STROOP.Forms;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Map.Map3D;
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
        public static MapAssociations MapAssociations;
        public static StroopMainForm StroopMainForm;
        public static TabControlEx TabControlMain;
        public static Label DebugText;

        public static MapGraphics MapGraphics;
        public static MapGui MapGui;
        public static Map3DGraphics Map3DGraphics;
        public static Map3DCamera Map3DCamera;

        public static CameraManager CameraManager;
        public static DebugManager DebugManager;
        public static DisassemblyManager DisassemblyManager;
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
        public static WaterManager WaterManager;
        public static SnowManager SnowManager;
        public static InputManager InputManager;
        public static ActionsManager ActionsManager;
        public static PuManager PuManager;
        public static TasManager TasManager;
        public static FileManager FileManager;
        public static MainSaveManager MainSaveManager;
        public static AreaManager AreaManager;
        public static DataManager QuarterFrameManager;
        public static DataManager CustomManager;
        public static VarHackManager VarHackManager;
        public static CamHackManager CamHackManager;
        public static MemoryManager MemoryManager;
        public static SearchManager SearchManager;
        public static CellsManager CellsManager;
        public static CoinManager CoinManager;
        public static GfxManager GfxManager;
        public static PaintingManager PaintingManager;
        public static MusicManager MusicManager;
        public static ScriptManager ScriptManager;
        public static WarpManager WarpManager;
        public static SoundManager SoundManager;
        public static M64Manager M64Manager;

        public static List<IVariableAdder> GetVariableAdders()
        {
            List<IVariableAdder> variableAdders = new List<IVariableAdder>();

            // get popouts
            List<VariablePopOutForm> popouts = FormManager.GetPopOutForms();
            variableAdders.AddRange(popouts);

            // get tabs
            List<VariableAdder> tabVariableAdders =
                ControlUtilities.GetFieldsOfType<VariableAdder>(typeof(Config), null);
            tabVariableAdders.Sort((d1, d2) => d1.TabIndex - d2.TabIndex);
            variableAdders.AddRange(tabVariableAdders);

            return variableAdders;
        }

        public static void Print(object formatNullable = null, params object[] args)
        {
            object format = formatNullable ?? "";
            string formatted = String.Format(format.ToString(), args);
            System.Diagnostics.Trace.WriteLine(formatted);
        }

        public static void SetDebugText(object obj)
        {
            DebugText.Visible = true;
            DebugText.Text = obj.ToString();
        }
    }
}
