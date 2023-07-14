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
        public static List<int> GetPointsInRange(int min, int max, int gap, bool convertBounds, bool convertGap)
        {
            List<int> output = new List<int>();

            return output;
        }

        public static short GetNext(int value, int gap)
        {
            int unconverted = MaybeUnconvert(value);
            unconverted += gap;
            return MaybeConvert(unconverted);
        }

        public static short Normalize(int value)
        {
            return MaybeConvert(MaybeUnconvert(value));
        }

        public static short MaybeConvert(int value)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return (short)value;
            }

            int newValue = value > 0 ? value * 4 : value * 4 - 1;
            return (short)newValue;
        }

        public static short MaybeUnconvert(int value)
        {
            if (!SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                return (short)value;
            }

            int newValue = value > 0 ? value / 4 : (value + 1) / 4;
            return (short)newValue;
        }
    }
}
