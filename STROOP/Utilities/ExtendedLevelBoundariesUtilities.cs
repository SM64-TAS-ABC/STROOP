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
        public enum ValueOffsetType { GO_THROUGH_ZERO, SPACED_AROUND_ZERO, BASED_ON_MIN }

        public static int TriangleVertexMultiplier => SavedSettingsConfig.UseExtendedLevelBoundaries ? 4 : 1;

        public static List<float> GetValuesInRangeFloat(int min, int max, int numSubdivides)
        {
            min = Convert(min, false);
            max = Convert(max, false);

            List<float> values = new List<float>();
            for (int i = 0; i <= numSubdivides; i++)
            {
                float p = i / (float)numSubdivides;
                float value = min + p * (max - min);
                values.Add(value);
            }
            return values;
        }

        public static List<int> GetValuesInRange(int min, int max, int gap, bool isY, ValueOffsetType valueOffsetType, bool convertBounds, bool convertGap)
        {
            if (valueOffsetType == ValueOffsetType.GO_THROUGH_ZERO)
            {
                min = (min / gap) * gap;
                max = (max / gap) * gap;
            }
            else if (valueOffsetType == ValueOffsetType.SPACED_AROUND_ZERO)
            {
                min = (min / gap) * gap - gap / 2;
                max = (max / gap) * gap + gap / 2;
            }

            if (convertBounds)
            {
                min = Convert(min, isY);
                max = Convert(max, isY);
            }

            int increment(int i)
            {
                if (convertGap)
                {
                    return GetNext(i, gap, isY);
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

        public static int GetNext(int value, int gap, bool isY)
        {
            int unconverted = Unconvert(value, isY);
            unconverted += gap;
            return Convert(unconverted, isY);
        }

        public static int Normalize(int value, bool isY)
        {
            return Convert(Unconvert(value, isY), isY);
        }

        public static int Convert(int value, bool isY)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return (short)value;
            }

            int offset = isY ? 0 : 1;
            return value > 0 ? value * 4 : value * 4 - offset;
        }

        public static int Unconvert(int value, bool isY)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return (short)value;
            }

            int offset = isY ? 0 : 1;
            return value > 0 ? value / 4 : (value + offset) / 4;
        }
    }
}
