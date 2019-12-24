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
using System.Drawing.Imaging;
using STROOP.Models;

namespace STROOP.Map3
{
    public abstract class Map3CeilingObject : Map3HorizontalTriangleObject
    {
        public Map3CeilingObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Red;
        }
    }
}
