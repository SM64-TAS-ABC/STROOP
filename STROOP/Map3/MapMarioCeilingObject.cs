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
    public class MapMarioCeilingObject : MapCeilingObject
    {
        public MapMarioCeilingObject()
            : base()
        {
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            return MapUtilities.GetTriangles(triAddress);
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
