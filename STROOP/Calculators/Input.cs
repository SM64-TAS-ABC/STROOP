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
    // Y value is inputted and stored in sm64 convention
    // Y value is displayed in mupen convention
    public class Input
    {
        public static bool USE_TAS_INPUT_Y = true;

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
            return String.Format("({0},{1})", X, (USE_TAS_INPUT_Y ? -1 : 1) * Y);
        }
    }
}
