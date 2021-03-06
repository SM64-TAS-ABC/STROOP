﻿using System;
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
    public class MapObjectPunchFloor : MapObjectCustomFloor
    {
        private static List<uint> GetPunchTriangleList()
        {
            return TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsFloor())
                .FindAll(tri =>
                {
                    double slopeAccel = tri.SlopeAccel;
                    double slopeDecel = 0.5 * tri.SlopeDecelValue;
                    double normalH = Math.Sqrt(tri.NormX * tri.NormX + tri.NormZ * tri.NormZ);
                    return slopeAccel * normalH > slopeDecel;
                })
                .ConvertAll(tri => tri.Address);
        }

        public MapObjectPunchFloor()
            : base(GetPunchTriangleList())
        {
        }

        public override string GetName()
        {
            return "Punch Floor Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
