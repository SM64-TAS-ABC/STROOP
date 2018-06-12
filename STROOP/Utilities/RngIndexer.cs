using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class RngIndexer
    {
        public static readonly int RNG_COUNT = 65114;
        public static readonly int NON_RESET_RNG_COUNT = 65534;

        private static readonly Dictionary<int, ushort> IndexToRNGDictionary;
        private static readonly Dictionary<ushort, int> RNGToIndexDictionary;

        static RngIndexer()
        {
            IndexToRNGDictionary = new Dictionary<int, ushort>();
            RNGToIndexDictionary = new Dictionary<ushort, int>();

            ushort rngValue = 0;
            for (int index = 0; index < RNG_COUNT; index++)
            {
                IndexToRNGDictionary.Add(index, rngValue);
                RNGToIndexDictionary.Add(rngValue, index);
                rngValue = GetNextRNG(rngValue, false);
            }

            for (int index = RNG_COUNT; rngValue != 0; index++)
            {
                RNGToIndexDictionary.Add(rngValue, index - NON_RESET_RNG_COUNT);
                rngValue = GetNextRNG(rngValue, false);
            }

            RNGToIndexDictionary.Add(58704, RNG_COUNT - NON_RESET_RNG_COUNT - 2);
            RNGToIndexDictionary.Add(22026, RNG_COUNT - NON_RESET_RNG_COUNT - 1);
        }

        private static ushort GetNextRNG(ushort rng, bool earlyReset = true)
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
                if ((s1 == 0xAA55) && earlyReset)
                    rng = 0;
                else
                    rng = (ushort)(s1 ^ 0x1FF4);
            }
            else
                rng = (ushort)(s1 ^ 0x8180);

            return rng;
        }

        public static int GetRngIndex()
        {
            return GetRngIndex(Config.Stream.GetUInt16(MiscConfig.RngAddress));
        }

        public static int GetRngIndex(ushort rngValue)
        {
            return RNGToIndexDictionary[rngValue];
        }

        public static ushort GetRngValue(int index)
        {
            index = MoreMath.NonNegativeModulus(index, RNG_COUNT);
            return IndexToRNGDictionary[index];
        }

        public static int GetRngIndexDiff(ushort rngValue1, ushort rngValue2)
        {
            int index1 = GetRngIndex(rngValue1);
            int index2 = GetRngIndex(rngValue2);
            return MoreMath.NonNegativeModulus(index2 - index1, RNG_COUNT);
        }
    }
}
