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
    public class Map3MarioFloorObject : Map3TriangleObject
    {
        public Map3MarioFloorObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Blue;
        }

        protected override List<List<(float x, float z)>> GetVertexLists()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            return Map3Utilities.GetTriangleVertexLists(triAddress);
        }

        public override string GetName()
        {
            return "Floor Tri";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
