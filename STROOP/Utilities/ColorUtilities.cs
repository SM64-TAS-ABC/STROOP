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
            int? inputtedNumber = GetCurrentlyInputtedNumber();
            switch(inputtedNumber)
            {
                default:
                    return defaultColor;
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Red;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Red;
                case 5:
                    return Color.Red;
                case 6:
                    return Color.Red;
                case 7:
                    return Color.Red;
                case 8:
                    return Color.Red;
                case 9:
                    return Color.Red;
                case 0:
                    return Color.Red;
            }
        }

        private static int? GetCurrentlyInputtedNumber()
        {
            if (Keyboard.IsKeyDown(Key.D1)) return 1;
            if (Keyboard.IsKeyDown(Key.D2)) return 2;
            if (Keyboard.IsKeyDown(Key.D3)) return 3;
            if (Keyboard.IsKeyDown(Key.D4)) return 4;
            if (Keyboard.IsKeyDown(Key.D5)) return 5;
            if (Keyboard.IsKeyDown(Key.D6)) return 6;
            if (Keyboard.IsKeyDown(Key.D7)) return 7;
            if (Keyboard.IsKeyDown(Key.D8)) return 8;
            if (Keyboard.IsKeyDown(Key.D9)) return 9;
            if (Keyboard.IsKeyDown(Key.D0)) return 0;
            return null;
        }
    }
}
