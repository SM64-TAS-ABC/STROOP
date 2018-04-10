using STROOP.Forms;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class StringUtilities
    {
        
        public static string Cap(string stringValue, int length)
        {
            if (stringValue == null) return stringValue;
            if (stringValue.Length <= length) return stringValue;
            return stringValue.Substring(0, length);
        }

        public static string GetEmptyChars(int count)
        {
            string emptyChar = "​";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                builder.Append(emptyChar);
            }
            return builder.ToString();
        }

        public static string FormatIntegerWithSign(int num)
        {
            return (num > 0 ? "+" : "") + num;
        }

    }
} 
