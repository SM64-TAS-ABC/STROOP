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
            for (int gap = 1; gap <= 2; gap++)
            {
                for (int i = -20; i <= 20; i++)
                {
                    Config.Print($"value={i} gap={gap} next={ExtendedLevelBoundariesUtilities.GetNext(i, gap)}");
                }
            }
        }
    }
}
