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
    public class Map3ObjectWallObject : Map3WallObject
    {
        private readonly uint _objAddress;

        public Map3ObjectWallObject(uint objAddress)
            : base()
        {
            _objAddress = objAddress;

            Opacity = 0.5;
            Color = Color.Green;
        }

        protected override List<(float x1, float z1, float x2, float z2, bool xProjection)> GetWallData()
        {
            return TriangleUtilities.GetObjectTrianglesForObject(_objAddress)
                .FindAll(tri => tri.IsWall())
                .ConvertAll(tri =>
                {
                    if (tri.X1 == tri.X2 && tri.Z1 == tri.Z2)
                        return ((float)tri.X1, (float)tri.Z1, (float)tri.X3, (float)tri.Z3, tri.XProjection);
                    if (tri.X1 == tri.X3 && tri.Z1 == tri.Z3)
                        return ((float)tri.X1, (float)tri.Z1, (float)tri.X2, (float)tri.Z2, tri.XProjection);
                    else
                        return ((float)tri.X2, (float)tri.Z2, (float)tri.X3, (float)tri.Z3, tri.XProjection);
                });
        }

        public override string GetName()
        {
            return "Wall Tris for " + PositionAngle.GetMapNameForObject(_objAddress);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }
    }
}
