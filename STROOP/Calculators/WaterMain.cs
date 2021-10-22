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
            Config.Print(state);
            for (int i = 0; i < 10; i++)
            {
                Input input = new Input(-40, 50);
                int waterLevel = WaterLevelCalculator.GetWaterLevelFromIndex(waterLevelIndex);
                state.Update(input, waterLevel);
                Config.Print(state);
                waterLevelIndex++;
            }
        }
    }
}
