using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace STROOP.Utilities
{
    public static class ColorUtilities
    {
        private static readonly Dictionary<string, string> ColorToParamsDictionary =
            new Dictionary<string, string>()
            {
                ["Red"] = "#FFCCCC",
                ["Yellow"] = "#FFECCC",
                ["Green"] = "#CFFFCC",
                ["LightBlue"] = "#CCFFFA",
                ["Blue"] = "#CCD0FF",
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
    }
}
