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
        // Point vars

        private static double _pointX = 0;
        public static double PointX
        {
            get { return _pointX; }
            set { _pointX = value; }
        }

        private static double _pointY = 0;
        public static double PointY
        {
            get { return _pointY; }
            set { _pointY = value; }
        }

        private static double _pointZ = 0;
        public static double PointZ
        {
            get { return _pointZ; }
            set { _pointZ = value; }
        }

        private static double _pointAngle = 0;
        public static double PointAngle
        {
            get { return _pointAngle; }
            set { _pointAngle = value; }
        }

        // PU vars

        public static int PuParam1 = 0;
        public static int PuParam2 = 1;

        public static double PuHypotenuse
        {
            get
            {
                return MoreMath.GetHypotenuse(PuParam1, PuParam2);
            }
        }

        // Mupen vars

        public static int MupenLagOffset = 0;
    }
}
