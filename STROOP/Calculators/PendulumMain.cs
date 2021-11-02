using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class PendulumMain
    {
        public static void Test()
        {
            TtcFloatPendulum pendulum = new TtcFloatPendulum(null);
            for (int i = 0; i <= 288; i++)
            {
                float angle = pendulum.PerformSwing(i % 2 == 0);
                Config.Print(i + ": " + angle);
            }
            for (int i = 0; true; i++)
            {
                float angle = pendulum.PerformSwing(true);
                Config.Print(i + ": " + angle);
                if (angle == 33578192) break;
            }
        }
    }
}
