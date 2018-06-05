using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class HexUtilities
    {
        public static string FormatByValue(object number, int? numDigits = null, bool usePrefix = true)
        {
            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string prefix = usePrefix ? "0x" : "";
            return prefix + String.Format("{0:X" + numDigitsString + "}", number);
        }

        public static object FormatByValueIfInteger(object value, int? numDigits = null, bool usePrefix = true)
        {
            int? intValueNullable = ParsingUtilities.ParseIntNullable(value);
            if (intValueNullable.HasValue) return FormatByValue(intValueNullable.Value, numDigits, usePrefix);
            uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(value);
            if (uintValueNullable.HasValue) return FormatByValue(uintValueNullable.Value, numDigits, usePrefix);
            return value;
        }

        public static string FormatByMemory(object number, int? numDigits = null, bool usePrefix = true)
        {
            byte[] bytes = TypeUtilities.GetBytes(number);
            List<byte> byteList = new List<byte>(bytes);
            byteList.Reverse();
            List<string> stringList = byteList.ConvertAll(b => String.Format("{0:X2}", b));
            string byteString = String.Join("", stringList);
            if (numDigits.HasValue) byteString = StringUtilities.ExactLength(byteString, numDigits.Value, true, '0');
            string prefix = usePrefix ? "0x" : "";
            return prefix + byteString;
        }

        public static string FormatValue(object number, int? numDigits = null, bool usePrefix = true)
        {
            if (!TypeUtilities.IsNumber(number)) throw new ArgumentOutOfRangeException();

            object numberInteger = number;
            if (number is float floatValue) numberInteger = Math.Round(floatValue);
            if (number is double doubleValue) numberInteger = Math.Round(doubleValue);

            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string prefix = usePrefix ? "0x" : "";
            return prefix + String.Format("{0:X" + numDigitsString + "}", numberInteger);
        }

        public static string FormatMemory(object number, int? numDigits = null, bool usePrefix = true)
        {
            if (!TypeUtilities.IsNumber(number)) throw new ArgumentOutOfRangeException();

            byte[] bytes = TypeUtilities.GetBytes(number);
            List<byte> byteList = new List<byte>(bytes);
            byteList.Reverse();
            List<string> stringList = byteList.ConvertAll(b => String.Format("{0:X2}", b));
            string byteString = String.Join("", stringList);
            if (numDigits.HasValue) byteString = StringUtilities.ExactLength(byteString, numDigits.Value, true, '0');
            string prefix = usePrefix ? "0x" : "";
            return prefix + byteString;
        }
    }
}
