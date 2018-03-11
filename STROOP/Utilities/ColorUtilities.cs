using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Input;

namespace STROOP.Utilities
{
    public static class ColorUtilities
    {
        public static Color InterpolateColor(Color c1, Color c2, double amount)
        {
            amount = MoreMath.Clamp(amount, 0, 1);
            byte r = (byte)((c1.R * (1 - amount)) + c2.R * amount);
            byte g = (byte)((c1.G * (1 - amount)) + c2.G * amount);
            byte b = (byte)((c1.B * (1 - amount)) + c2.B * amount);
            return Color.FromArgb(r, g, b);
        }

        public static string ToString(Color color)
        {
            string r = String.Format("{0:X2}", color.R);
            string g = String.Format("{0:X2}", color.G);
            string b = String.Format("{0:X2}", color.B);
            return r + g + b;
        }

        public static Color GetColorByInput(Color defaultColor)
        {
            int? inputtedNumber = KeyboardUtilities.GetCurrentlyInputtedNumber();
            switch (inputtedNumber)
            {
                default:
                    return defaultColor;
                case 1:
                    return Color.Red;
                case 2:
                    return Color.Orange;
                case 3:
                    return Color.Yellow;
                case 4:
                    return Color.Green;
                case 5:
                    return Color.Blue;
                case 6:
                    return Color.Purple;
                case 7:
                    return Color.Pink;
                case 8:
                    return Color.Brown;
                case 9:
                    return Color.Black;
                case 0:
                    return Color.White;
            }
        }
    }
}
