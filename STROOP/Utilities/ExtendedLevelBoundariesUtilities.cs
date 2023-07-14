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

            long courseWidth = 16384;

            if (courseWidth >= numSubdivides && courseWidth % numSubdivides == 0 ||
                numSubdivides >= courseWidth && numSubdivides % courseWidth == 0)
            {
                float gap = courseWidth / (float)numSubdivides;
                long minMultiple = (long)Math.Floor(min / gap);
                long maxMultiple = (long)Math.Ceiling(max / gap);

                List<float> values = new List<float>();
                for (long multiple = minMultiple; multiple <= maxMultiple; multiple++)
                {
                    float value = multiple * gap;
                    values.Add(value);
                }
                return values;
            }

            return new List<float>();
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
                min = GetNext(min, -gap / 2, isY, true);
                max = GetNext(max, gap / 2, isY, true);
            }

            if (convertBounds)
            {
                min = Convert(min, isY);
                max = Convert(max, isY);
            }
            else if (SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                min = Normalize(min, isY, true);
                max = Normalize(max, isY, true);
            }

            long increment(long i)
            {
                if (convertGap)
                {
                    return GetNext(i, gap, isY, false);
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

        public static long GetNext(long value, long gap, bool isY, bool negativeLeniency)
        {
            long unconverted = Unconvert(value, isY, negativeLeniency);
            unconverted += gap;
            return Convert(unconverted, isY);
        }

        public static long Normalize(long value, bool isY, bool negativeLeniency)
        {
            return Convert(Unconvert(value, isY, negativeLeniency), isY);
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

        public static long Unconvert(long value, bool isY, bool negativeLeniency)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return value;
            }

            long offset = isY || negativeLeniency ? 0 : 1;
            return value > 0 ? value / 4 : (value + offset) / 4;
        }
    }
}
