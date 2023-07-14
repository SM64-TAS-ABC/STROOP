using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class ExtendedLevelBoundariesUtilities
    {
        public static List<int> GetValuesInRange(int min, int max, int gap, bool convertBounds, bool convertGap)
        {
            min = (min / gap) * gap;
            max = (max / gap) * gap;

            if (convertBounds)
            {
                min = Convert(min);
                max = Convert(max);
            }

            int increment(int i)
            {
                if (convertGap)
                {
                    return GetNext(i, gap);
                }
                else
                {
                    return i + gap;
                }
            }

            List<int> values = new List<int>();
            for (int i = min; i <= max; i = increment(i))
            {
                values.Add(i);
            }
            return values;
        }

        public static int GetNext(int value, int gap)
        {
            int unconverted = Unconvert(value);
            unconverted += gap;
            return Convert(unconverted);
        }

        public static int Normalize(int value)
        {
            return Convert(Unconvert(value));
        }

        public static int Convert(int value)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return (short)value;
            }

            return value > 0 ? value * 4 : value * 4 - 1;
        }

        public static int Unconvert(int value)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return (short)value;
            }

            return value > 0 ? value / 4 : (value + 1) / 4;
        }
    }
}
