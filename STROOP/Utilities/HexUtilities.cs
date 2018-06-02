using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class HexUtilities
    {
        public static string Format(object number, int? numDigits = null, bool usePrefix = true)
        {
            if (number is float || number is double)
                return FormatByMemory(number, numDigits, usePrefix);
            else
                return FormatByValue(number, numDigits, usePrefix);
        }

        public static string FormatByValue(object number, int? numDigits = null, bool usePrefix = true)
        {
            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string prefix = usePrefix ? "0x" : "";
            return prefix + String.Format("{0:X" + numDigitsString + "}", number);
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
    }
}
