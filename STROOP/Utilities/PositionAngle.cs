using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public class PositionAngle
    {
        public readonly double X;
        public readonly double Y;
        public readonly double Z;
        public readonly double? Angle;

        public PositionAngle(double x, double y, double z, double? angle = null)
        {
            X = x;
            Y = y;
            Z = z;
            Angle = angle;
        }

        public static PositionAngle Custom()
        {
            return new PositionAngle(SpecialConfig.CustomX, SpecialConfig.CustomY, SpecialConfig.CustomZ, SpecialConfig.CustomAngle);
        }

        public static PositionAngle Mario()
        {
            return new PositionAngle(DataModels.Mario.X, DataModels.Mario.Y, DataModels.Mario.Z, DataModels.Mario.FacingYaw);
        }

        public static PositionAngle Holp()
        {
            return new PositionAngle(DataModels.Mario.HolpX, DataModels.Mario.HolpY, DataModels.Mario.HolpZ);
        }

        public static PositionAngle Camera()
        {
            return new PositionAngle(DataModels.Camera.X, DataModels.Camera.Y, DataModels.Camera.Z, DataModels.Camera.FacingYaw);
        }

        public static PositionAngle Object(uint address)
        {
            return new PositionAngle(
                Config.Stream.GetSingle(address + ObjectConfig.XOffset),
                Config.Stream.GetSingle(address + ObjectConfig.YOffset),
                Config.Stream.GetSingle(address + ObjectConfig.ZOffset),
                Config.Stream.GetSingle(address + ObjectConfig.YawFacingOffset));
        }

        public static PositionAngle ObjectHome(uint address)
        {
            return new PositionAngle(
                Config.Stream.GetSingle(address + ObjectConfig.HomeXOffset),
                Config.Stream.GetSingle(address + ObjectConfig.HomeYOffset),
                Config.Stream.GetSingle(address + ObjectConfig.HomeZOffset));
        }

        public static PositionAngle FromId(PositionAngleId positionAngleId)
        {
            switch (positionAngleId.PositionAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    return Custom();
                case PositionAngleTypeEnum.Mario:
                    return Mario();
                case PositionAngleTypeEnum.Holp:
                    return Holp();
                case PositionAngleTypeEnum.Camera:
                    return Camera();
                case PositionAngleTypeEnum.Object:
                    return Object(positionAngleId.Address.Value);
                case PositionAngleTypeEnum.ObjectHome:
                    return ObjectHome(positionAngleId.Address.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
