using STROOP.Managers;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class GenericMath
    {

        public static object Round(object value, int numDigits)
        {
            if (value is float floatValue)
            {
                return (float)Math.Round(floatValue, numDigits);
            }

            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(value);
            if (!doubleValueNullable.HasValue) return value;
            double doubleValue = doubleValueNullable.Value;
            return Math.Round(doubleValue, numDigits);
        }

        public static object Round2(object value, int numDigits)
        {
            if (value is float floatValue)
            {
                return floatValue.ToString("F" + numDigits);
            }
            if (value is double doubleValue)
            {
                return doubleValue.ToString("F" + numDigits);
            }
            return value;
        }

    }
}
