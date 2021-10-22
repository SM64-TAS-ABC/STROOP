using STROOP.Forms;
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
    public static class WaterLevelCalculator
    {
        public static int GetWaterLevelIndex()
        {
            uint objAddress = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Water Level Manager")[0].Address;
            int timer = Config.Stream.GetInt(objAddress + 0xF4);
            int index = (timer % 65536) / 512;
            return index;
        }

        public static int GetWaterLevelFromIndex(int index)
        {
            return waterLevels[index % waterLevels.Count];
        }

        public static List<int> waterLevels = new List<int>()
        {
            29,
            30,
            31,
            31,
            32,
            33,
            34,
            35,
            36,
            37,
            38,
            39,
            40,
            41,
            42,
            42,
            43,
            44,
            45,
            45,
            46,
            47,
            47,
            48,
            48,
            49,
            49,
            49,
            50,
            50,
            50,
            50,
            50,
            50,
            51,
            50,
            50,
            50,
            50,
            50,
            50,
            49,
            49,
            49,
            48,
            48,
            47,
            47,
            46,
            45,
            45,
            44,
            43,
            42,
            42,
            41,
            40,
            39,
            38,
            37,
            36,
            35,
            34,
            33,
            32,
            31,
            31,
            30,
            29,
            28,
            27,
            26,
            25,
            24,
            23,
            22,
            21,
            20,
            19,
            19,
            18,
            17,
            16,
            16,
            15,
            14,
            14,
            13,
            13,
            12,
            12,
            12,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            11,
            12,
            12,
            12,
            13,
            13,
            14,
            14,
            15,
            16,
            16,
            17,
            18,
            19,
            19,
            20,
            21,
            22,
            23,
            24,
            25,
            26,
            27,
            28,
        };
    }
}
