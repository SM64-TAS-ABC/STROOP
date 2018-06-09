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

        public static PositionAngleId SelfPosPosAngle =
            new PositionAngleId(PositionAngleTypeEnum.Mario);

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

        public static PositionAngleId SelfAnglePosAngle =
            new PositionAngleId(PositionAngleTypeEnum.Mario);

        public static double SelfAngle
        {
            get => SelfAnglePosAngle.Angle;
        }

        // - Point pos

        public static PositionAngleId PointPosPosAngle =
            new PositionAngleId(PositionAngleTypeEnum.Custom);

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

        // - Point angle

        public static PositionAngleId PointAnglePosAngle =
            new PositionAngleId(PositionAngleTypeEnum.Custom);

        public static double PointAngle
        {
            get => PointAnglePosAngle.Angle;
        }
        
        // PU vars

        public static int PuParam1 = 0;
        public static int PuParam2 = 1;

        public static double PuHypotenuse
        {
            get => MoreMath.GetHypotenuse(PuParam1, PuParam2);
        }

        // Mupen vars

        public static int MupenLagOffset = 0;
    }
}
