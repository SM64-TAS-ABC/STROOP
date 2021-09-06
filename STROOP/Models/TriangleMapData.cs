using STROOP.Structs.Configurations;
using STROOP.Utilities;
using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;

namespace STROOP.Models
{
    public class TriangleMapData
    {
        public readonly float X1;
        public readonly float Y1;
        public readonly float Z1;
        public readonly float X2;
        public readonly float Y2;
        public readonly float Z2;
        public readonly TriangleDataModel Tri;

        public TriangleMapData(
            float x1,
            float y1,
            float z1,
            float x2,
            float y2,
            float z2,
            TriangleDataModel tri)
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
            Tri = tri;
        }
    }
}
