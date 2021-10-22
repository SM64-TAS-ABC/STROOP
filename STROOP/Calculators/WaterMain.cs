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
    public static class WaterMain
    {
        public static void Test()
        {
            int waterLevelIndex = WaterLevelCalculator.GetWaterLevelIndex();

            WaterState state = new WaterState();
            for (int i = 0; i < 1000; i++)
            {
                int waterLevel = WaterLevelCalculator.GetWaterLevelFromIndex(waterLevelIndex);
                Config.Print(i + " = " + waterLevel);
                waterLevelIndex++;

                //state.Update()
            }
        }
    }
}
