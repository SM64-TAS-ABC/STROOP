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
    public static class WaterUtilities
    {
        public static List<(int y, int xMin, int xMax, int zMin, int zMax)> GetWaterLevels()
        {
            uint waterAddress = Config.Stream.GetUInt(MiscConfig.WaterPointerAddress);
            int numWaterLevels = waterAddress == 0 ? 0 : Config.Stream.GetShort(waterAddress);

            if (numWaterLevels > 100) numWaterLevels = 100;

            uint baseOffset = 0x04;
            uint waterStructSize = 0x0C;

            List<(int y, int xMin, int xMax, int zMin, int zMax)> output =
                new List<(int y, int xMin, int xMax, int zMin, int zMax)>();
            for (int i = 0; i < numWaterLevels; i++)
            {
                int xMin = Config.Stream.GetShort((uint)(waterAddress + baseOffset + i * waterStructSize + 0x00));
                int zMin = Config.Stream.GetShort((uint)(waterAddress + baseOffset + i * waterStructSize + 0x02));
                int xMax = Config.Stream.GetShort((uint)(waterAddress + baseOffset + i * waterStructSize + 0x04));
                int zMax = Config.Stream.GetShort((uint)(waterAddress + baseOffset + i * waterStructSize + 0x06));
                int y = Config.Stream.GetShort((uint)(waterAddress + baseOffset + i * waterStructSize + 0x08));
                output.Add((y, xMin, xMax, zMin, zMax));
            }
            return output;
        }

        public static int GetCurrentWater()
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            return GetWaterAtPos(marioX, marioZ);
        }

        public static int GetWaterAtPos(float x, float z)
        {
            List<(int y, int xMin, int xMax, int zMin, int zMax)> waterLevels = GetWaterLevels();
            for (int i = 0; i < waterLevels.Count; i++)
            {
                var w = waterLevels[i];
                if (x > w.xMin && x < w.xMax && z > w.zMin && z < w.zMax)
                {
                    return i + 1;
                }
            }
            return -1;
        }
    }
}
