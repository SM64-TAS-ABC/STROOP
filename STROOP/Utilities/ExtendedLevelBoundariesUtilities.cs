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
        public enum ValueOffsetType { GO_THROUGH_VALUE, SPACED_AROUND_ZERO, BASED_ON_MIN }

        public static int TriangleVertexMultiplier => SavedSettingsConfig.UseExtendedLevelBoundaries ? 4 : 1;

        public static List<float> GetCustomGridlinesValues(int numSubdivides, Coordinate coordinate)
        {
            float mapMin = 0;
            float mapMax = 0;
            if (coordinate == Coordinate.X)
            {
                mapMin = Config.CurrentMapGraphics.MapViewXMin;
                mapMax = Config.CurrentMapGraphics.MapViewXMax;
            }
            if (coordinate == Coordinate.Y)
            {
                mapMin = Config.CurrentMapGraphics.MapViewYMin;
                mapMax = Config.CurrentMapGraphics.MapViewYMax;
            }
            if (coordinate == Coordinate.Z)
            {
                mapMin = Config.CurrentMapGraphics.MapViewZMin;
                mapMax = Config.CurrentMapGraphics.MapViewZMax;
            }

            long min = Math.Max(Convert(-8192, false), (long)Math.Floor(mapMin));
            long max = Math.Min(Convert(8192, false), (long)Math.Ceiling(mapMax));

            List<float> values = new List<float>();
            for (int i = 0; i <= numSubdivides; i++)
            {
                float p = i / (float)numSubdivides;
                float value = min + p * (max - min);
                values.Add(value);
            }
            return values;
        }

        public static List<long> GetValuesInRange(
            long min, long max, long gap, bool isY,
            ValueOffsetType valueOffsetType, long goThroughValue, bool convertBounds, bool convertGap, bool padBounds)
        {
            long multiplier = convertGap && SavedSettingsConfig.UseExtendedLevelBoundaries ? 4 : 1;
            long padding = padBounds ? 1 : 0;
            if (valueOffsetType == ValueOffsetType.GO_THROUGH_VALUE)
            {
                min = (min / gap / multiplier - padding) * gap * multiplier;
                max = (max / gap / multiplier + padding) * gap * multiplier;
            }
            else if (valueOffsetType == ValueOffsetType.SPACED_AROUND_ZERO)
            {
                min = (min / gap / multiplier - padding) * gap * multiplier - gap * multiplier / 2;
                max = (max / gap / multiplier + padding) * gap * multiplier + gap * multiplier / 2;
            }

            if (convertBounds)
            {
                min = Convert(min, isY);
                max = Convert(max, isY);
            }
            else
            {
                min = Normalize(min, isY);
                max = Normalize(max, isY);
            }

            long increment(long i)
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

            List<long> values = new List<long>();
            for (long i = min; i <= max; i = increment(i))
            {
                values.Add(i);
            }
            return values;
        }

        public static long GetNext(long value, long gap, bool isY)
        {
            long unconverted = Unconvert(value, isY);
            unconverted += gap;
            return Convert(unconverted, isY);
        }

        public static long Normalize(long value, bool isY)
        {
            return Convert(Unconvert(value, isY), isY);
        }

        public static long Convert(long value, bool isY)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return value;
            }

            long offset = isY ? 0 : 1;
            return value > 0 ? value * 4 : value * 4 - offset;
        }

        public static long Unconvert(long value, bool isY)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return value;
            }

            long offset = isY ? 0 : 1;
            return value > 0 ? value / 4 : (value + offset) / 4;
        }
    }
}
