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
    public class Map3CeilingObject : Map3TriangleObject
    {
        public Map3CeilingObject()
            : base()
        {
        }

        public override void DrawOnControl()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            if (triAddress == 0) return;
            TriangleDataModel tri = new TriangleDataModel(triAddress);
            DrawTriangle(tri.Get2DVertices(), Color.Red);
        }
    }
}
