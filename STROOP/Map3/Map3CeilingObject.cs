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
            Opacity = 0.5;
            Color = Color.Red;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            if (triAddress == 0) return new List<(float x, float z)>();
            TriangleDataModel tri = new TriangleDataModel(triAddress);
            return tri.Get2DVertices();
        }

        public override string GetName()
        {
            return "Ceiling Tri";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleCeilingImage;
        }
    }
}
