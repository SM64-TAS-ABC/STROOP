using STROOP.Forms;
using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class Input
    {
        public readonly int X;
        public readonly int Y;

        public Input(int x, int y)
        {
            X = x;
            Y = y;
        }

        public float GetScaledMagnitude()
        {
            return MoreMath.GetScaledInputMagnitude(X, Y, false);
        }

        public override string ToString()
        {
            return String.Format("({0},{1})", X, -1 * Y);
        }
    }
}
