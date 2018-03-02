using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs.Configurations
{
    public static class PuParamsConfig
    {
        public static int Param1 = 0;
        public static int Param2 = 1;

        public static double Hypotenuse
        {
            get
            {
                return MoreMath.GetHypotenuse(Param1, Param2);
            }
        }
    }
}
