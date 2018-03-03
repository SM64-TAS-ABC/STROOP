using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class SpecialConfig
    {
        public static double CustomPointX = 0;
        public static double CustomPointY = 0;
        public static double CustomPointZ = 0;

        public static int PuParam1 = 0;
        public static int PuParam2 = 1;

        public static double PuHypotenuse
        {
            get
            {
                return MoreMath.GetHypotenuse(PuParam1, PuParam2);
            }
        }
    }
}
