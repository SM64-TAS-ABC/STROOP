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
    public class MapObjectFloorObject : MapFloorObject
    {
        private readonly uint _objAddress;

        public MapObjectFloorObject(uint objAddress)
            : base()
        {
            _objAddress = objAddress;
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            return TriangleUtilities.GetObjectTrianglesForObject(_objAddress)
                .FindAll(tri => tri.IsFloor());
        }

        public override string GetName()
        {
            return "Floor Tris for " + PositionAngle.GetMapNameForObject(_objAddress);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
