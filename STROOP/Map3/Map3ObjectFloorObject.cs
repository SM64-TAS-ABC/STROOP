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
    public class Map3ObjectFloorObject : Map3HorizontalTriangleObject
    {
        private readonly uint _objAddress;

        public Map3ObjectFloorObject(uint objAddress)
            : base()
        {
            _objAddress = objAddress;

            Opacity = 0.5;
            Color = Color.Blue;
        }

        protected override List<List<(float x, float y, float z)>> GetVertexLists()
        {
            return TriangleUtilities.GetObjectTrianglesForObject(_objAddress)
                .FindAll(tri => tri.IsFloor())
                .ConvertAll(tri => tri.Get3DVertices());
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
