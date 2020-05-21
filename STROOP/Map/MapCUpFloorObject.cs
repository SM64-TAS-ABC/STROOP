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
    public class MapCUpFloorObject : MapCustomFloorObject
    {
        private static List<uint> GetCUpTriangleList()
        {
            return TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsFloor())
                .FindAll(tri =>
                {
                    // Speed - 4 + 1.7 * normal.H
                    double slopeAccel = tri.SlopeAccel;
                    double normalH = Math.Sqrt(tri.NormX * tri.NormX + tri.NormZ * tri.NormZ);
                    return slopeAccel * normalH > 4;
                })
                .ConvertAll(tri => tri.Address);
        }

        public MapCUpFloorObject()
            : base(GetCUpTriangleList())
        {
        }

        public override string GetName()
        {
            return "C-Up Floor Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
