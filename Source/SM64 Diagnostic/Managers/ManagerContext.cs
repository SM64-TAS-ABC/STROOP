using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Managers
{
    public class ManagerContext
    {
        public static ManagerContext Current;

        public CameraManager CameraManager;
        public DebugManager DebugManager;
        public DisassemblyManager DisassemblyManager;
        public HackManager HackManager;
        public HudManager HudManager;
        public MapManager MapManager;
        public MarioManager MarioManager;
        public MiscManager MiscManager;
        public ObjectManager ObjectManager;
        public ObjectSlotsManager ObjectSlotManager;
        public OptionsManager OptionsManager;
        public ScriptManager ScriptManager;
        public TriangleManager TriangleManager;
        public DataManager WaterManager;
        public InputManager InputManager;
        public ActionsManager ActionsManager;
        public PuManager PuManager;
        public FileManager FileManager;
        public DataManager QuarterFrameManager;
        public DataManager CameraHackManager;
    }
}
