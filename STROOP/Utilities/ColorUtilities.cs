using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class ColorUtilities
    {
        public static readonly Dictionary<string, string> ColorToParamsDictionary =
            new Dictionary<string, string>()
            {
                ["Red"] = "#FFD7D7",
                ["Orange"] = "#FFE2B7",
                ["Yellow"] = "#FFFFD0",
                ["Green"] = "#CFFFCC",
                ["LightBlue"] = "#CCFFFA",
                ["Blue"] = "#CADDFF",
                ["Purple"] = "#E5CCFF",
                ["Pink"] = "#FFCCFF",
                ["Grey"] = "#D0D0D0",
                ["Turquoise"] = "#AAFFE6",
                ["Brown"] = "#EBBE96",
            };

        public static readonly List<Color> ColorList =
            ColorToParamsDictionary.Values.ToList()
              .ConvertAll(html => ColorTranslator.FromHtml(html));

        private static readonly Dictionary<string, string> ParamsToColorDictionary =
            DictionaryUtilities.ReverseDictionary(ColorToParamsDictionary);

        public static Color GetColorFromString(string colorString)
        {
            if (colorString.Substring(0, 1) != "#")
                colorString = ColorToParamsDictionary[colorString];
            return ColorTranslator.FromHtml(colorString);
        }

        public static string ConvertColorToString(Color color)
        {
            string colorParams = ConvertColorToParams(color);
            if (ParamsToColorDictionary.ContainsKey(colorParams))
                return ParamsToColorDictionary[colorParams];
            return colorParams;
        }

        public static string ConvertColorToParams(Color color)
        {
            string r = String.Format("{0:X2}", color.R);
            string g = String.Format("{0:X2}", color.G);
            string b = String.Format("{0:X2}", color.B);
            return "#" + r + g + b;
        }

        public static Color LastCustomColor = SystemColors.Control;
        public static Color GetColorForVariable()
        {
            int? inputtedNumber = KeyboardUtilities.GetCurrentlyInputtedNumber();

            if (inputtedNumber.HasValue &&
                inputtedNumber.Value > 0 &&
                inputtedNumber.Value <= ColorList.Count)
            {
                return ColorList[inputtedNumber.Value - 1];
            }
            return SystemColors.Control;
        }

        public static Color? GetColorForHighlight()
        {
            int? inputtedNumber = KeyboardUtilities.GetCurrentlyInputtedNumber();
            switch (inputtedNumber)
            {
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
                default:
                    return null;
            }
        }

        public static Color? ConvertDecimalToColor(string text)
        {
            List<int?> numbersNullable = ParsingUtilities.ParseIntList(text);
            if (numbersNullable.Count != 3) return null;
            if (numbersNullable.Any(number => !number.HasValue)) return null;
            if (numbersNullable.Any(number => number.Value < 0 || number.Value > 255)) return null;
            List<int> numbers = numbersNullable.ConvertAll(number => number.Value);
            return Color.FromArgb(numbers[0], numbers[1], numbers[2]);
        }

        public static string ConvertColorToDecimal(Color color)
        {
            return color.R + "," + color.G + "," + color.B;
        }

        public static Color InterpolateColor(Color c1, Color c2, double amount)
        {
            amount = MoreMath.Clamp(amount, 0, 1);
            byte r = (byte)((c1.R * (1 - amount)) + c2.R * amount);
            byte g = (byte)((c1.G * (1 - amount)) + c2.G * amount);
            byte b = (byte)((c1.B * (1 - amount)) + c2.B * amount);
            return Color.FromArgb(r, g, b);
        }

        public static Color? GetColorFromDialog(Color? defaultColor = null)
        {
            ColorDialog colorDialog = new ColorDialog();
            colorDialog.FullOpen = true;
            if (defaultColor.HasValue) colorDialog.Color = defaultColor.Value;
            if (colorDialog.ShowDialog() == DialogResult.OK) return colorDialog.Color;
            return null;
        }

        public static Color AddAlpha(Color color, byte alpha)
        {
            return Color.FromArgb(alpha, color.R, color.G, color.B);
        }

        public static Color HSL2RGB(double h, double sl, double l)
        {
            double v;
            double r, g, b;

            r = l;   // default to gray
            g = l;
            b = l;
            v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                double m;
                double sv;
                int sextant;
                double fract, vsf, mid1, mid2;
                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return Color.FromArgb(Convert.ToByte(r * 255.0f), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f));
        }

        public static Color Rainbow(float progress)
        {
            float div = (Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;
            ascending = MoreMath.Clamp(ascending, 0, 255);
            descending = MoreMath.Clamp(descending, 0, 255);

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }
    }
}
