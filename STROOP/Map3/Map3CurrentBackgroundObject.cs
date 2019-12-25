using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;

namespace STROOP.Map3
{
    public class Map3CurrentBackgroundObject : Map3BackgroundObject
    {
        public Map3CurrentBackgroundObject()
            : base()
        {
        }

        public override Image GetImage()
        {
            return Map3Utilities.GetBackgroundImage();
        }

        public override string GetName()
        {
            return "Current Background";
        }
    }
}
