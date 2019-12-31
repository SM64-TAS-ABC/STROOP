using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Structs.Configurations;
using STROOP.Map3.Map;
using STROOP.Map3.Map.Graphics;

namespace STROOP.Managers
{
    public class Map4Manager
    {
        public bool IsLoaded { get; private set; }
        public bool Visible { get => Config.Map4Graphics.Visible; set => Config.Map4Graphics.Visible = value; }

        public Map4Manager()
        {
        }

        public void Load()
        {
            // Create new graphics control
            Config.Map4Graphics = new Map4Graphics();
            Config.Map4Graphics.Load();
            Config.Map4Controller = new Map4Controller();
            Config.Map4Camera = new Map4Camera();

            IsLoaded = true;
        }

        public void Update()
        {
            if (!IsLoaded)
                return;
            Config.Map4Controller.Update();
        }
    }
}
