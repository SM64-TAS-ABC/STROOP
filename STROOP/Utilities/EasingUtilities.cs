using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class EasingUtilities
    {
        public static double Ease(double proportion)
        {
            return proportion;
        }

        public static double EaseIn(double power, double t)
        {
            return Math.Pow(t, power);
        }

        public static double EaseOut (double power, double t)
        {
            return 1 - Math.Abs(Math.Pow(t - 1, power));
        }

        public static double EaseInOut (double power, double t)
        {
            return t < 0.5 ? EaseIn(power, t * 2) / 2 : EaseOut(power, t * 2 - 1) / 2 + 0.5;
        }
    }
}
