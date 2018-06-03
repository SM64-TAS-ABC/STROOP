using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class PositionAngle
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly double? Angle;

        public PositionAngle(double x, double y, double z, double? angle = null)
        {
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
        }
    }
}
