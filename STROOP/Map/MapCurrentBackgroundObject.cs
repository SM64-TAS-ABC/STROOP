using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;

namespace STROOP.Map
{
    public class MapCurrentBackgroundObject : MapBackgroundObject
    {
        public MapCurrentBackgroundObject()
            : base()
        {
        }

        public override Image GetInternalImage()
        {
            return MapUtilities.GetBackgroundImage();
        }

        public override string GetName()
        {
            return "Current Background";
        }
    }
}
