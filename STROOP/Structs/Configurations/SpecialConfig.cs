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

        public static PositionAngle SelfPosPA = PositionAngle.Mario;

        public static double SelfX
        {
            get => SelfPosPA.X;
        }

        public static double SelfY
        {
            get => SelfPosPA.Y;
        }

        public static double SelfZ
        {
            get => SelfPosPA.Z;
        }

        // - Self angle

        public static PositionAngle SelfAnglePA = PositionAngle.Mario;

        public static double SelfAngle
        {
            get => SelfAnglePA.Angle;
        }

        // - Point pos

        public static PositionAngle PointPosPA = PositionAngle.Custom;

        public static double PointX
        {
            get => PointPosPA.X;
        }

        public static double PointY
        {
            get => PointPosPA.Y;
        }

        public static double PointZ
        {
            get => PointPosPA.Z;
        }

        // - Point angle

        public static PositionAngle PointAnglePA = PositionAngle.Custom;

        public static double PointAngle
        {
            get => PointAnglePA.Angle;
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
