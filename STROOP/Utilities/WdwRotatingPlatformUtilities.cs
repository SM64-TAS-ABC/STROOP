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

        private static Dictionary<ushort, int> _angleToIndexDictionary;
        private static Dictionary<int, ushort> _indexToAngleDictionary;

        static WdwRotatingPlatformUtilities()
        {
            _angleToIndexDictionary = new Dictionary<ushort, int>();
            _indexToAngleDictionary = new Dictionary<int, ushort>();
            int index = 0;
            for (ushort angle = 0; !(angle == 0 && index != 0); angle += 1120)
            {
                _angleToIndexDictionary[angle] = index;
                _indexToAngleDictionary[index] = angle;
                index++;
            }
        }

        public static int? GetIndex(ushort angle)
        {
            if (_angleToIndexDictionary.ContainsKey(angle))
            {
                return _angleToIndexDictionary[angle];
            }
            else
            {
                return null;
            }
        }

        public static ushort GetAngle(int index)
        {
            index = MoreMath.NonNegativeModulus(index, _indexToAngleDictionary.Count);
            return _indexToAngleDictionary[index];
        }

        public static double GetFramesToGoalAngle(ushort currentAngle)
        {
            double currentIndex = GetIndex(currentAngle) ?? double.NaN;
            double goalIndex = GetIndex(GoalAngle) ?? double.NaN;
            return MoreMath.NonNegativeModulus(goalIndex - currentIndex, _angleToIndexDictionary.Count);
        }

        public static ushort? GetAngleNumFramesBeforeGoal(int numFrames)
        {
            int? goalIndexNullable = GetIndex(GoalAngle);
            if (!goalIndexNullable.HasValue) return null;
            int goalIndex = goalIndexNullable.Value;
            return GetAngle(goalIndex - numFrames);
        }
    }
}
