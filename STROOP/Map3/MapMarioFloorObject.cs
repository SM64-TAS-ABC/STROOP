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

namespace STROOP.Map3
{
    public class MapMarioFloorObject : MapFloorObject
    {
        public MapMarioFloorObject()
            : base()
        {
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            return MapUtilities.GetTriangles(triAddress);
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
