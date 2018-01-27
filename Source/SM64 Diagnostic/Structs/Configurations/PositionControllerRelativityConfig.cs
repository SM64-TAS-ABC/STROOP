using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class PositionControllerRelativityConfig
    {
        public enum PositionControllerRelativity
        {
            Recommended,
            Mario,
            Custom,
        };

        public static PositionControllerRelativity Relativity;
        public static double CustomAngle;
    }
}
