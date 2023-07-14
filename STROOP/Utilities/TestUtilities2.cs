using STROOP.Controls;
using STROOP.Forms;
using STROOP.M64;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Utilities
{
    public static class TestUtilities2
    {
        public static void Test()
        {
            int min = (int)SpecialConfig.CustomX;
            int max = (int)SpecialConfig.CustomY;
            int gap = (int)SpecialConfig.CustomZ;

            bool isY = SpecialConfig.Custom2X != 0;
            bool convertBounds = SpecialConfig.Custom2Y != 0;
            bool convertGap = SpecialConfig.Custom2Z != 0;

            List<int> values = ExtendedLevelBoundariesUtilities.GetValuesInRange(min, max, gap, isY, convertBounds, convertGap);
            Config.Print($"min={min} max={max} gap={gap} convertBounds={convertBounds} convertGap={convertGap}");
            Config.Print(string.Join(",", values));
            Config.Print();
        }
    }
}
