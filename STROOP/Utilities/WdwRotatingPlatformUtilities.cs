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
    public static class WdwRotatingPlatformUtilities
    {
        public static ushort GoalAngle = 0;

        private static Dictionary<ushort, int> _dictionary;

        static WdwRotatingPlatformUtilities()
        {
            _dictionary = new Dictionary<ushort, int>();
            int counter = 0;
            for (ushort angle = 0; !(angle == 0 && counter != 0); angle += 1120)
            {
                _dictionary[angle] = counter;
                counter++;
            }
        }

        public static int? GetIndex(ushort angle)
        {
            if (_dictionary.ContainsKey(angle))
            {
                return _dictionary[angle];
            }
            else
            {
                return null;
            }
        }

        public static double GetFramesToGoalAngle(ushort currentAngle)
        {
            double currentIndex = GetIndex(currentAngle) ?? double.NaN;
            double goalIndex = GetIndex(GoalAngle) ?? double.NaN;
            return MoreMath.NonNegativeModulus(goalIndex - currentIndex, _dictionary.Count);
        }

        public static ushort? GetAngleNumFramesBeforeGoal(int numFrames)
        {
            int? goalIndexNullable = GetIndex(GoalAngle);
            if (!goalIndexNullable.HasValue) return null;
            int goalIndex = goalIndexNullable.Value;
            int newIndex = MoreMath.NonNegativeModulus(goalIndex - numFrames, _dictionary.Count);
            return _dictionary.ToList()[newIndex].Key;
        }
    }
}
