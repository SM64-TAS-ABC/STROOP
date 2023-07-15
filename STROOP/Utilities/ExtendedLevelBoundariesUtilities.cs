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

        public static List<float> GetCustomGridlinesValues(int numSubdivides, Coordinate coordinate, bool showOnlyWhatIsVisible)
        {
            long min = -8192 * TriangleVertexMultiplier;
            long max = 8192 * TriangleVertexMultiplier;

            if (showOnlyWhatIsVisible)
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

                min = Math.Max(min, (long)Math.Floor(mapMin));
                max = Math.Min(max, (long)Math.Ceiling(mapMax));
            }

            if (numSubdivides >= 2 && numSubdivides <= 16384 && 16384 % numSubdivides == 0)
            {
                long gap = 16384 / numSubdivides;
                float gapPixels = gap * Config.CurrentMapGraphics.MapViewScaleValue;
                if (gapPixels < 4) return new List<float>();

                return GetValuesInRange(min, max, gap, coordinate == Coordinate.Y, ValueOffsetType.GO_THROUGH_VALUE, 0, false, true, false)
                    .ConvertAll(value => (float)value);
            }
            else
            {
                bool isOdd = numSubdivides % 2 == 1;
                float gap = (16384 * TriangleVertexMultiplier) / (float)numSubdivides;
                float gapPixels = gap * Config.CurrentMapGraphics.MapViewScaleValue;
                if (gapPixels < 4) return new List<float>();

                long minMultiple = (long)Math.Floor(min / gap);
                long maxMultiple = (long)Math.Ceiling(max / gap);

                List<float> values = new List<float>();
                for (long multiple = minMultiple; isOdd ? multiple < maxMultiple : multiple <= maxMultiple; multiple++)
                {
                    float value = multiple * gap;
                    if (isOdd)
                    {
                        value += gap / 2;
                    }
                    values.Add(value);
                }
                return values;
            }
        }

        public static List<long> GetValuesInRange(
            long min, long max, long gap, bool isY,
            ValueOffsetType valueOffsetType, long goThroughValue, bool convertBounds, bool convertGap, bool padBounds)
        {
            long multiplier = convertGap && SavedSettingsConfig.UseExtendedLevelBoundaries ? 4 : 1;
            long padding = padBounds ? 2 : 0;
            long multipliedGap = multiplier * gap;

            long distBefore = 0;
            long distAfter = 0;
            if (padBounds && goThroughValue != 0)
            {
                goThroughValue = Convert(goThroughValue, isY);
                long goThroughValueMod = ((goThroughValue % multipliedGap) + multipliedGap) % multipliedGap;
                distBefore = goThroughValueMod;
                distAfter = multipliedGap - goThroughValueMod;
            }

            if (valueOffsetType == ValueOffsetType.GO_THROUGH_VALUE)
            {
                min = ((min - distBefore) / multipliedGap - padding) * multipliedGap + distBefore;
                max = ((max + distAfter) / multipliedGap + padding) * multipliedGap - distAfter;
            }
            else if (valueOffsetType == ValueOffsetType.SPACED_AROUND_ZERO)
            {
                min = (min / multipliedGap - padding) * multipliedGap;
                max = (max / multipliedGap + padding) * multipliedGap;
                min = GetNext(min, -gap / 2, isY);
                max = GetNext(max, gap / 2, isY);
            }

            if (convertBounds)
            {
                min = Convert(min, isY);
                max = Convert(max, isY);
            }
            else if (SavedSettingsConfig.UseExtendedLevelBoundaries)
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

            return value / 4;
        }
    }
}
