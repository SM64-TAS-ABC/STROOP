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
            object numberFormatted = number;

            // Make sure it's a number
            if (!TypeUtilities.IsNumber(numberFormatted))
            {
                numberFormatted = ParsingUtilities.ParseDoubleNullable(numberFormatted);
                if (numberFormatted == null) return number.ToString();
            }

            // Convert floats/doubles into ints/uints
            if (numberFormatted is float || numberFormatted is double)
            {
                if (numberFormatted is float floatValue) numberFormatted = Math.Round(floatValue);
                if (numberFormatted is double doubleValue) numberFormatted = Math.Round(doubleValue);

                int? intValueNullable = ParsingUtilities.ParseIntNullable(numberFormatted);
                if (intValueNullable.HasValue) numberFormatted = intValueNullable.Value;
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(numberFormatted);
                if (uintValueNullable.HasValue) numberFormatted = uintValueNullable.Value;
            }

            if (!TypeUtilities.IsIntegerNumber(numberFormatted)) return number.ToString();

            string numDigitsString = numDigits.HasValue ? numDigits.Value.ToString() : "";
            string hexString = String.Format("{0:X" + numDigitsString + "}", numberFormatted);
            string prefix = usePrefix ? "0x" : "";
            if (numDigits.HasValue)
            {
                hexString = StringUtilities.ExactLength(hexString, numDigits.Value, true, '0');
            }
            return prefix + hexString;
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
