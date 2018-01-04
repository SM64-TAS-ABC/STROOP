using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;

namespace SM64_Diagnostic.Utilities
{
    public static class ParsingUtilities
    {

        public static uint ParseHex(string str)
        {
            int prefixPos = str.IndexOf("0x");
            if (prefixPos == -1)
                return uint.Parse(str, NumberStyles.HexNumber);
            else
                return uint.Parse(str.Substring(prefixPos + 2), NumberStyles.HexNumber);
        }

        public static uint? ParseHexNullable(string str)
        {
            if (str == null) return null;
            try
            {
                uint parsed = ParseHex(str);
                return parsed;
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static UInt64 ParseExtHex(string str)
        {
            return UInt64.Parse(str.Substring(str.IndexOf("0x") + 2), NumberStyles.HexNumber);
        }

        public static bool TryParseHex(string str, out uint hex)
        {
            // This is what you call lazy programming (not in python though)
            try
            {
                hex = ParseHex(str);
            }
            catch (FormatException)
            {
                hex = new uint();
                return false;
            }

            return true;
        }

        public static bool IsHex(string str)
        {
            return (str.Contains("0x"));
        }

        public static bool TryParseExtHex(string str, out UInt64 hex)
        {
            // This is what you call lazy programming
            try
            {
                hex = ParseExtHex(str);
            }
            catch (FormatException)
            {
                hex = new UInt64();
                return false;
            }

            return true;
        }

        public static int? ParseIntNullable(string text)
        {
            int parsed;
            if (int.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static int ParseInt(string text)
        {
            return ParseIntNullable(text) ?? 0;
        }

        public static uint? ParseUIntNullable(string text)
        {
            uint parsed;
            if (uint.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static uint ParseUInt(string text)
        {
            return ParseUIntNullable(text) ?? 0;
        }

        public static short? ParseShortNullable(string text)
        {
            short parsed;
            if (short.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static short ParseShort(string text)
        {
            return ParseShortNullable(text) ?? 0;
        }

        public static ushort? ParseUShortNullable(string text)
        {
            ushort parsed;
            if (ushort.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static ushort ParseUShort(string text)
        {
            return ParseUShortNullable(text) ?? 0;
        }

        public static long? ParseLongNullable(string text)
        {
            long parsed;
            if (long.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static long ParseLong(string text)
        {
            return ParseLongNullable(text) ?? 0;
        }

        public static ulong? ParseULongNullable(string text)
        {
            ulong parsed;
            if (ulong.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static ulong ParseULong(string text)
        {
            return ParseULongNullable(text) ?? 0;
        }

        public static byte? ParseByteNullable(string text)
        {
            byte parsed;
            if (byte.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static byte ParseByte(string text)
        {
            return ParseByteNullable(text) ?? 0;
        }

        public static sbyte? ParseSByteNullable(string text)
        {
            sbyte parsed;
            if (sbyte.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static sbyte ParseSByte(string text)
        {
            return ParseSByteNullable(text) ?? 0;
        }

        public static float? ParseFloatNullable(string text)
        {
            float parsed;
            if (float.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static float ParseFloat(string text)
        {
            return ParseFloatNullable(text) ?? 0;
        }

        public static double? ParseDoubleNullable(string text)
        {
            double parsed;
            if (double.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static double ParseDouble(string text)
        {
            return ParseDoubleNullable(text) ?? 0;
        }

        public static bool? ParseBoolNullable(string text)
        {
            bool parsed;
            if (bool.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static bool ParseBool(string text)
        {
            return ParseBoolNullable(text) ?? false;
        }

        public static List<string> ParseTextStrings(string text)
        {
            text = text
                .Replace('\n', ' ')
                .Replace('\r', ' ')
                .Replace('\t', ' ')
                .Replace(',', ' ')
                .Replace('(', ' ')
                .Replace(')', ' ');
            text = Regex.Replace(text, @"\s+", " ");
            string[] stringArray = text.Split(' ');

            List<string> stringList = new List<string>();
            stringList.AddRange(stringArray);
            return stringList;
        }

    }
}
