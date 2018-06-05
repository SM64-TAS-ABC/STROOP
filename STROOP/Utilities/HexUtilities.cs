using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class HexUtilities
    {
        public static string FormatValue(object number, int? numDigits = null, bool usePrefix = true)
        {
            if (!TypeUtilities.IsIntegerNumber(number)) throw new ArgumentOutOfRangeException();

            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string prefix = usePrefix ? "0x" : "";
            return prefix + String.Format("{0:X" + numDigitsString + "}", number);
        }

        public static object FormatValueAsInteger(object number, int? numDigits = null, bool usePrefix = true)
        {
            if (!TypeUtilities.IsNumber(number)) throw new ArgumentOutOfRangeException();

            object numberInteger = number;
            if (number is float floatValue) numberInteger = Math.Round(floatValue);
            if (number is double doubleValue) numberInteger = Math.Round(doubleValue);

            int? intValueNullable = ParsingUtilities.ParseIntNullable(numberInteger);
            if (intValueNullable.HasValue) return FormatValue(intValueNullable.Value, numDigits, usePrefix);
            uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(numberInteger);
            if (uintValueNullable.HasValue) return FormatValue(uintValueNullable.Value, numDigits, usePrefix);
            return number;
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
