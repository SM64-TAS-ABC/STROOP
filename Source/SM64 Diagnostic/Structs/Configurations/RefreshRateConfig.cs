using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class RefreshRateConfig
    {
        public static uint RefreshRateFreq;
        public static double RefreshRateInterval
        {
            get
            {
                uint freq = RefreshRateFreq;
                if (freq == 0) return 0;
                else return 1000.0 / freq;
            }
        }
    }
}
