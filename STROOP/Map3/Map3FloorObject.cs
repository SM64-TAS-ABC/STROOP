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
    public class Map3FloorObject : Map3TriangleObject
    {
        public Map3FloorObject()
            : base()
        {
            Color = Color.Blue;
            Opacity = 0.5;
        }

        public override void DrawOnControl()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            if (triAddress == 0) return;
            TriangleDataModel tri = new TriangleDataModel(triAddress);
            DrawTriangle(tri.Get2DVertices());
        }
    }
}
