using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class ColorUtilities
    {
        private static readonly Dictionary<string, string> ColorToParamsDictionary =
            new Dictionary<string, string>()
            {
                ["Red"] = "#FFCCCC",
                ["Orange"] = "#FFE2B7",
                ["Yellow"] = "#FFFFD0",
                ["Green"] = "#CFFFCC",
                ["LightBlue"] = "#CCFFFA",
                ["Blue"] = "#CADDFF",
                ["Purple"] = "#E5CCFF",
                ["Pink"] = "#FFCCFF",
                ["Grey"] = "#D0D0D0",
            };

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

        public static Color LastSelectedColor = SystemColors.Control;
        public static Color GetColorForVariable()
        {
            int? inputtedNumber = KeyboardUtilities.GetCurrentlyInputtedNumber();

            if (inputtedNumber == 0) return LastSelectedColor;

            if (inputtedNumber.HasValue &&
                inputtedNumber.Value > 0 &&
                inputtedNumber.Value <= ColorToParamsDictionary.Count)
            {
                int index = inputtedNumber.Value - 1;
                string colorString = ColorToParamsDictionary.ToList()[index].Value;
                return ColorTranslator.FromHtml(colorString);
            }
            return SystemColors.Control;
        }

        public static Color GetColorForHighlight(Color defaultColor)
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
    }
}
