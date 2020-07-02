using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public class TriangleShape
    {
        public readonly double X1;
        public readonly double Y1;
        public readonly double Z1;
        public readonly double X2;
        public readonly double Y2;
        public readonly double Z2;
        public readonly double X3;
        public readonly double Y3;
        public readonly double Z3;

        public readonly double NormX;
        public readonly double NormY;
        public readonly double NormZ;
        public readonly double NormOffset;

        public TriangleShape(
            double x1,
            double y1,
            double z1,
            double x2,
            double y2,
            double z2,
            double x3,
            double y3,
            double z3)
        {
            X1 = x1;
            Y1 = y1;
            Z1 = z1;
            X2 = x2;
            Y2 = y2;
            Z2 = z2;
            X3 = x3;
            Y3 = y3;
            Z3 = z3;

            (NormX, NormY, NormZ, NormOffset) = TriangleUtilities.GetNorms((int)X1, (int)Y1, (int)Z1, (int)X2, (int)Y2, (int)Z2, (int)X3, (int)Y3, (int)Z3);
        }

        public double GetY(double x, double z)
        {
            return -1 * (NormX * x + NormZ * z + NormOffset) / NormY;
        }
    }
}
