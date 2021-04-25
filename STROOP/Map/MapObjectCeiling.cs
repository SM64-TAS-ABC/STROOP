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
using System.Drawing.Imaging;
using STROOP.Models;

namespace STROOP.Map
{
    public abstract class MapObjectCeiling : MapObjectHorizontalTriangle
    {
        public MapObjectCeiling()
            : base()
        {
            Size = 160;
            Opacity = 0.5;
            Color = Color.Red;
        }
    }
}
