using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class RngIndexer
    {
        static int[] _rngTableIndex;

        public static int GetRngIndex(ushort rngValue)
        {
            return _rngTableIndex[rngValue];
        }

        public static int GetRngIndexDiff(ushort rngValue1, ushort rngValue2)
        {
            int preRng = _rngTableIndex[rngValue1];
            int currentRng = _rngTableIndex[rngValue2];
            return (currentRng >= preRng) ? currentRng - preRng : currentRng - preRng + 65114;
        }

        static RngIndexer()
        {
            GenerateRngTable();
        }

        public static void GenerateRngTable()
        {
            _rngTableIndex = Enumerable.Repeat<int>(-1, ushort.MaxValue + 1).ToArray();
            ushort _currentRng = 0;
            for (ushort i = 0; i < 65114; i++)
            {
                _rngTableIndex[_currentRng] = i;
                _currentRng = NextRNG(_currentRng);
            }

            int naIndex = -1;
            for (int i = 0; i < _rngTableIndex.Length; i++)
            {
                if (_rngTableIndex[i] == -1)
                {
                    _rngTableIndex[i] = naIndex;
                    naIndex--;
                }
            }
        }

        public static ushort NextRNG(ushort rng)
        {
            if (rng == 0x560A)
                rng = 0;
            ushort s0 = (ushort)(rng << 8);
            s0 ^= rng;
            rng = (ushort)((s0 >> 8) | (s0 << 8));
            s0 = (ushort)((s0 & 0x00FF) << 1);
            s0 ^= rng;
            ushort s1 = (ushort)(0xFF80 ^ (s0 >> 1));
            if ((s0 & 1) == 0)
            {
                if (s1 == 0xAA55)
                    rng = 0;
                else
                    rng = (ushort)(s1 ^ 0x1FF4);
            }
            else
                rng = (ushort)(s1 ^ 0x8180);

            return rng;
        }
    }
}
