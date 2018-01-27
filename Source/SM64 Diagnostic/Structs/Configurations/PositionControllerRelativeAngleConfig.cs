using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs.Configurations
{
    public static class PositionControllerRelativeAngleConfig
    {
        public enum RelativityType
        {
            Recommended,
            Mario,
            Custom,
        };

        public static RelativityType Relativity;
        public static double CustomAngle;
    }
}
