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

        public static PositionAngleId PointPosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Custom);
        private static PositionAngle PointPosAngle
        {
            get => PositionAngle.FromId(PointPosAngleId);
        }

        public static PositionAngleId AnglePosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Custom);
        private static PositionAngle AnglePosAngle
        {
            get => PositionAngle.FromId(AnglePosAngleId);
        }

        public static double CustomX = 0;
        public static double CustomY = 0;
        public static double CustomZ = 0;
        public static double CustomAngle = 0;

        public static double PointX
        {
            get { return PointPosAngle.X; }
        }

        public static double PointY
        {
            get { return PointPosAngle.Y; }
        }

        public static double PointZ
        {
            get { return PointPosAngle.Z; }
        }

        public static double PointAngle
        {
            get { return AnglePosAngle.Angle ?? 0; }
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
