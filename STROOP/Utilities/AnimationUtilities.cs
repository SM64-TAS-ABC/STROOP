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
    public static class AnimationUtilities
    {
        private static Dictionary<int, (int, int)> _dictionary;

        static AnimationUtilities()
        {
            _dictionary = new Dictionary<int, (int, int)>();
            for (int i = 0; i <= 208; i++)
            {
                uint address1 = 0x80064040 + 0x8 + (uint)i * 0x8 + 0x0;
                uint address2 = 0x80064040 + 0x8 + (uint)i * 0x8 + 0x4;
                int value1 = Config.Stream.GetInt32(address1);
                int value2 = Config.Stream.GetInt32(address2);
                _dictionary[i] = (value1, value2);
            }
        }

        public static void ReplaceAnimation(int animationToBeReplaced, int animationToReplaceIt)
        {
            (int value1, int value2) = _dictionary[animationToReplaceIt];
            uint address1 = 0x80064040 + 0x8 + (uint)animationToBeReplaced * 0x8 + 0x0;
            uint address2 = 0x80064040 + 0x8 + (uint)animationToBeReplaced * 0x8 + 0x4;
            Config.Stream.SetValue(value1, address1);
            Config.Stream.SetValue(value2, address2);
        }
    }
}
