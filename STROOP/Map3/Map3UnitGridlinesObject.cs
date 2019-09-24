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

namespace STROOP.Map3
{
    public class Map3UnitGridlinesObject : Map3LineObject
    {
        public Map3UnitGridlinesObject()
            : base()
        {
            Size = 3;
            Color = Color.Black;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            return new List<(float x, float z)>()
            {
                (-8192, -8192),
                (8192, 8192),
                (-8192, 8192),
                (8192, -8192),
            };
        }
    }
}
