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
        public static double PointX = 0;
        public static double PointY = 0;
        public static double PointZ = 0;

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
