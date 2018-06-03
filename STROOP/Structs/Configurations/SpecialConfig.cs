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

        // - Custom

        public static double CustomX = 0;
        public static double CustomY = 0;
        public static double CustomZ = 0;
        public static double CustomAngle = 0;

        // - Self

        public static PositionAngleId SelfPosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Mario);
        private static PositionAngle SelfPosAngle
        {
            get => PositionAngle.FromId(SelfPosAngleId);
        }

        public static double SelfX
        {
            get { return SelfPosAngle.X; }
        }

        public static double SelfY
        {
            get { return SelfPosAngle.Y; }
        }

        public static double SelfZ
        {
            get { return SelfPosAngle.Z; }
        }

        // - Point pos

        public static PositionAngleId PointPosPosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Custom);
        private static PositionAngle PointPosPosAngle
        {
            get => PositionAngle.FromId(PointPosPosAngleId);
        }

        public static double PointX
        {
            get { return PointPosPosAngle.X; }
        }

        public static double PointY
        {
            get { return PointPosPosAngle.Y; }
        }

        public static double PointZ
        {
            get { return PointPosPosAngle.Z; }
        }

        // Point angle

        public static PositionAngleId PointAnglePosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Custom);
        private static PositionAngle PointAnglePosAngle
        {
            get => PositionAngle.FromId(PointAnglePosAngleId);
        }

        public static double PointAngle
        {
            get { return PointAnglePosAngle.Angle ?? 0; }
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
