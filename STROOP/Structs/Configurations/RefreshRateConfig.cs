using STROOP.Managers;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class RefreshRateConfig
    {
        public static bool LimitRefreshRate = true;

        public static uint RefreshRateFreq;

        public static double RefreshRateInterval
        {
            get
            {
                uint freq = LimitRefreshRate ? RefreshRateFreq : 0;
                if (freq == 0) return 0;
                else return 1000.0 / freq;
            }
        }
    }
}
