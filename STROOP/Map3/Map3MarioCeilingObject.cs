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
    public class Map3MarioCeilingObject : Map3HorizontalTriangleObject
    {
        public Map3MarioCeilingObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Red;
        }

        protected override List<List<(float x, float y, float z)>> GetVertexLists()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            return Map3Utilities.GetTriangleVertexLists(triAddress);
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
