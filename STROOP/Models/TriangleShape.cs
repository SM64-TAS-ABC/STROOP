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

            List<double> v12 = new List<double>() { X2 - X1, Y2 - Y1, Z2 - Z1 };
            List<double> v13 = new List<double>() { X3 - X1, Y3 - Y1, Z3 - Z1 };

            double normXUnscaled = v12[1] * v13[2] - v12[2] * v13[1];
            double normYUnscaled = v12[2] * v13[0] - v12[0] * v13[2];
            double normZUnscaled = v12[0] * v13[1] - v12[1] * v13[0];

            double magnitude = Math.Sqrt(normXUnscaled * normXUnscaled + normYUnscaled * normYUnscaled + normZUnscaled * normZUnscaled);
            NormX = normXUnscaled / magnitude;
            NormY = normYUnscaled / magnitude;
            NormZ = normZUnscaled / magnitude;
            NormOffset = -1 * (NormX * X1 + NormY * Y1 + NormZ * Z1);
        }

        public double GetY(double x, double z)
        {
            return -1 * (NormX * x + NormZ * z + NormOffset) / NormY;
        }
    }
}
