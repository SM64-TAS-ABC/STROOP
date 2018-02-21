using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;

namespace STROOP.Utilities
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
            catch (Exception)
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

        public static int? ParseIntNullable(object obj)
        {
            string text = obj?.ToString();
            int parsed;
            if (int.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static int ParseInt(object obj)
        {
            return ParseIntNullable(obj) ?? 0;
        }

        public static uint? ParseUIntNullable(object obj)
        {
            string text = obj?.ToString();
            uint parsed;
            if (uint.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static uint ParseUInt(object obj)
        {
            return ParseUIntNullable(obj) ?? 0;
        }

        public static short? ParseShortNullable(object obj)
        {
            string text = obj?.ToString();
            short parsed;
            if (short.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static short ParseShort(object obj)
        {
            return ParseShortNullable(obj) ?? 0;
        }

        public static ushort? ParseUShortNullable(object obj)
        {
            string text = obj?.ToString();
            ushort parsed;
            if (ushort.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static ushort ParseUShort(object obj)
        {
            return ParseUShortNullable(obj) ?? 0;
        }

        public static long? ParseLongNullable(object obj)
        {
            string text = obj?.ToString();
            long parsed;
            if (long.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static long ParseLong(object obj)
        {
            return ParseLongNullable(obj) ?? 0;
        }

        public static ulong? ParseULongNullable(object obj)
        {
            string text = obj?.ToString();
            ulong parsed;
            if (ulong.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static ulong ParseULong(object obj)
        {
            return ParseULongNullable(obj) ?? 0;
        }

        public static byte? ParseByteNullable(object obj)
        {
            string text = obj?.ToString();
            byte parsed;
            if (byte.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static byte ParseByte(object obj)
        {
            return ParseByteNullable(obj) ?? 0;
        }

        public static sbyte? ParseSByteNullable(object obj)
        {
            string text = obj?.ToString();
            sbyte parsed;
            if (sbyte.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static sbyte ParseSByte(object obj)
        {
            return ParseSByteNullable(obj) ?? 0;
        }

        public static float? ParseFloatNullable(object obj)
        {
            string text = obj?.ToString();
            float parsed;
            if (float.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static float ParseFloat(object obj)
        {
            return ParseFloatNullable(obj) ?? 0;
        }

        public static double? ParseDoubleNullable(object obj)
        {
            string text = obj?.ToString();
            double parsed;
            if (double.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static double ParseDouble(object obj)
        {
            return ParseDoubleNullable(obj) ?? 0;
        }

        public static bool? ParseBoolNullable(object obj)
        {
            string text = obj?.ToString();
            bool parsed;
            if (bool.TryParse(text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        public static bool ParseBool(string obj)
        {
            return ParseBoolNullable(obj) ?? false;
        }

        public static List<string> ParseStringList(string text)
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

        public static List<uint> ParseUIntList(string text)
        {
            return ParseStringList(text).ConvertAll(stringValue => ParseUInt(stringValue));
        }

        public static byte? ParseByteRoundingWrapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseByteRoundingWrapping(doubleValue.Value);
            return null;
        }

        public static sbyte? ParseSByteRoundingWrapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseSByteRoundingWrapping(doubleValue.Value);
            return null;
        }

        public static short? ParseShortRoundingWrapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseShortRoundingWrapping(doubleValue.Value);
            return null;
        }

        public static ushort? ParseUShortRoundingWrapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseUShortRoundingWrapping(doubleValue.Value);
            return null;
        }

        public static int? ParseIntRoundingWrapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseIntRoundingWrapping(doubleValue.Value);
            return null;
        }

        public static uint? ParseUIntRoundingWrapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseUIntRoundingWrapping(doubleValue.Value);
            return null;
        }

        public static byte ParseByteRoundingWrapping(double value)
        {
            return (byte)MoreMath.GetIntegerInRangeWrapped(value, 1.0 + byte.MaxValue - byte.MinValue, false);
        }

        public static sbyte ParseSByteRoundingWrapping(double value)
        {
            return (sbyte)MoreMath.GetIntegerInRangeWrapped(value, 1.0 + sbyte.MaxValue - sbyte.MinValue, true);
        }

        public static short ParseShortRoundingWrapping(double value)
        {
            return (short)MoreMath.GetIntegerInRangeWrapped(value, 1.0 + short.MaxValue - short.MinValue, true);
        }

        public static ushort ParseUShortRoundingWrapping(double value)
        {
            return (ushort)MoreMath.GetIntegerInRangeWrapped(value, 1.0 + ushort.MaxValue - ushort.MinValue, false);
        }

        public static int ParseIntRoundingWrapping(double value)
        {
            return (int)MoreMath.GetIntegerInRangeWrapped(value, 1.0 + int.MaxValue - int.MinValue, true);
        }

        public static uint ParseUIntRoundingWrapping(double value)
        {
            return (uint)MoreMath.GetIntegerInRangeWrapped(value, 1.0 + uint.MaxValue - uint.MinValue, false);
        }



        public static byte? ParseByteRoundingCapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseByteRoundingCapping(doubleValue.Value);
            return null;
        }

        public static sbyte? ParseSByteRoundingCapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseSByteRoundingCapping(doubleValue.Value);
            return null;
        }

        public static short? ParseShortRoundingCapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseShortRoundingCapping(doubleValue.Value);
            return null;
        }

        public static ushort? ParseUShortRoundingCapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseUShortRoundingCapping(doubleValue.Value);
            return null;
        }

        public static int? ParseIntRoundingCapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseIntRoundingCapping(doubleValue.Value);
            return null;
        }

        public static uint? ParseUIntRoundingCapping(object value)
        {
            double? doubleValue = ParseDoubleNullable(value.ToString());
            if (doubleValue.HasValue) return ParseUIntRoundingCapping(doubleValue.Value);
            return null;
        }

        public static byte ParseByteRoundingCapping(double value)
        {
            return (byte)MoreMath.GetIntegerInRangeCapped(value, 1.0 + byte.MaxValue - byte.MinValue, false);
        }

        public static sbyte ParseSByteRoundingCapping(double value)
        {
            return (sbyte)MoreMath.GetIntegerInRangeCapped(value, 1.0 + sbyte.MaxValue - sbyte.MinValue, true);
        }

        public static short ParseShortRoundingCapping(double value)
        {
            return (short)MoreMath.GetIntegerInRangeCapped(value, 1.0 + short.MaxValue - short.MinValue, true);
        }

        public static ushort ParseUShortRoundingCapping(double value)
        {
            return (ushort)MoreMath.GetIntegerInRangeCapped(value, 1.0 + ushort.MaxValue - ushort.MinValue, false);
        }

        public static int ParseIntRoundingCapping(double value)
        {
            return (int)MoreMath.GetIntegerInRangeCapped(value, 1.0 + int.MaxValue - int.MinValue, true);
        }

        public static uint ParseUIntRoundingCapping(double value)
        {
            return (uint)MoreMath.GetIntegerInRangeCapped(value, 1.0 + uint.MaxValue - uint.MinValue, false);
        }



    }
}
