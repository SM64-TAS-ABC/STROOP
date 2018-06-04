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

        // - Self pos

        public static PositionAngleId SelfPosPosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Mario);
        private static PositionAngle SelfPosPosAngle
        {
            get => PositionAngle.FromId(SelfPosPosAngleId);
        }

        public static double SelfX
        {
            get => SelfPosPosAngle.X;
        }

        public static double SelfY
        {
            get => SelfPosPosAngle.Y;
        }

        public static double SelfZ
        {
            get => SelfPosPosAngle.Z;
        }

        // - Self angle

        public static PositionAngleId SelfAnglePosAngleId =
            new PositionAngleId(PositionAngleTypeEnum.Mario);
        private static PositionAngle SelfAnglePosAngle
        {
            get => PositionAngle.FromId(SelfAnglePosAngleId);
        }

        public static double SelfAngle
        {
            get => SelfAnglePosAngle.Angle ?? 0;
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
            get => PointPosPosAngle.X;
        }

        public static double PointY
        {
            get => PointPosPosAngle.Y;
        }

        public static double PointZ
        {
            get => PointPosPosAngle.Z;
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
            get => PointAnglePosAngle.Angle ?? 0;
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
