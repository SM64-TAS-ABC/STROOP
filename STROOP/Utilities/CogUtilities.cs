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
    public static class CogUtilities
    {

        private static Dictionary<int, int> _rotationIndexDictionary;

        static CogUtilities()
        {
            _rotationIndexDictionary = new Dictionary<int, int>();
            int remainder = 0;
            int index = 0;
            while (true)
            {
                _rotationIndexDictionary[remainder] = index;
                remainder = (remainder + 14) % 50;
                index++;
                if (remainder == 0) break;
            }
        }

        public static int? GetRotationIndex(int cogYaw)
        {
            int remainder = cogYaw % 50;
            if (_rotationIndexDictionary.ContainsKey(remainder))
                return _rotationIndexDictionary[remainder];
            else
                return null;
        }

    }
}
