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
                Config.Stream.GetUInt16(address + ObjectConfig.YawFacingOffset));
        }

        public static PositionAngle ObjectHome(uint address)
        {
            return new PositionAngle(
                Config.Stream.GetSingle(address + ObjectConfig.HomeXOffset),
                Config.Stream.GetSingle(address + ObjectConfig.HomeYOffset),
                Config.Stream.GetSingle(address + ObjectConfig.HomeZOffset));
        }

        public static PositionAngle Tri(uint address, int vertex)
        {
            TriangleDataModel tri = new TriangleDataModel(address);
            switch (vertex)
            {
                case 1:
                    return new PositionAngle(tri.X1, tri.Y1, tri.Z1);
                case 2:
                    return new PositionAngle(tri.X2, tri.Y2, tri.Z2);
                case 3:
                    return new PositionAngle(tri.X3, tri.Y3, tri.Z3);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static PositionAngle FromId(PositionAngleId posAngleId)
        {
            switch (posAngleId.PosAngleType)
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
                    return Object(posAngleId.Address.Value);
                case PositionAngleTypeEnum.ObjectHome:
                    return ObjectHome(posAngleId.Address.Value);
                case PositionAngleTypeEnum.Tri:
                    return Tri(posAngleId.Address.Value, posAngleId.TriVertex.Value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool SetX(double value, PositionAngleId posAngleId)
        {
            switch (posAngleId.PosAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    SpecialConfig.CustomX = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return Config.Stream.SetValue((float)value, MarioConfig.StructAddress + MarioConfig.XOffset);
                case PositionAngleTypeEnum.Holp:
                    return Config.Stream.SetValue((float)value, MarioConfig.StructAddress + MarioConfig.HolpXOffset);
                case PositionAngleTypeEnum.Camera:
                    return Config.Stream.SetValue((float)value, CameraConfig.CameraStructAddress + CameraConfig.XOffset);
                case PositionAngleTypeEnum.Object:
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + ObjectConfig.XOffset);
                case PositionAngleTypeEnum.ObjectHome:
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + ObjectConfig.HomeXOffset);
                case PositionAngleTypeEnum.Tri:
                    uint triVertexOffset;
                    switch (posAngleId.TriVertex.Value)
                    {
                        case 1:
                            triVertexOffset = TriangleOffsetsConfig.X1;
                            break;
                        case 2:
                            triVertexOffset = TriangleOffsetsConfig.X2;
                            break;
                        case 3:
                            triVertexOffset = TriangleOffsetsConfig.X3;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + triVertexOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool SetY(double value, PositionAngleId posAngleId)
        {
            switch (posAngleId.PosAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    SpecialConfig.CustomY = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return Config.Stream.SetValue((float)value, MarioConfig.StructAddress + MarioConfig.YOffset);
                case PositionAngleTypeEnum.Holp:
                    return Config.Stream.SetValue((float)value, MarioConfig.StructAddress + MarioConfig.HolpYOffset);
                case PositionAngleTypeEnum.Camera:
                    return Config.Stream.SetValue((float)value, CameraConfig.CameraStructAddress + CameraConfig.YOffset);
                case PositionAngleTypeEnum.Object:
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + ObjectConfig.YOffset);
                case PositionAngleTypeEnum.ObjectHome:
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + ObjectConfig.HomeYOffset);
                case PositionAngleTypeEnum.Tri:
                    uint triVertexOffset;
                    switch (posAngleId.TriVertex.Value)
                    {
                        case 1:
                            triVertexOffset = TriangleOffsetsConfig.Y1;
                            break;
                        case 2:
                            triVertexOffset = TriangleOffsetsConfig.Y2;
                            break;
                        case 3:
                            triVertexOffset = TriangleOffsetsConfig.Y3;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + triVertexOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool SetZ(double value, PositionAngleId posAngleId)
        {
            switch (posAngleId.PosAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    SpecialConfig.CustomZ = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return Config.Stream.SetValue((float)value, MarioConfig.StructAddress + MarioConfig.ZOffset);
                case PositionAngleTypeEnum.Holp:
                    return Config.Stream.SetValue((float)value, MarioConfig.StructAddress + MarioConfig.HolpZOffset);
                case PositionAngleTypeEnum.Camera:
                    return Config.Stream.SetValue((float)value, CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
                case PositionAngleTypeEnum.Object:
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + ObjectConfig.ZOffset);
                case PositionAngleTypeEnum.ObjectHome:
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + ObjectConfig.HomeZOffset);
                case PositionAngleTypeEnum.Tri:
                    uint triVertexOffset;
                    switch (posAngleId.TriVertex.Value)
                    {
                        case 1:
                            triVertexOffset = TriangleOffsetsConfig.Z1;
                            break;
                        case 2:
                            triVertexOffset = TriangleOffsetsConfig.Z2;
                            break;
                        case 3:
                            triVertexOffset = TriangleOffsetsConfig.Z3;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    return Config.Stream.SetValue((float)value, posAngleId.Address.Value + triVertexOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static bool SetAngle(double value, PositionAngleId posAngleId)
        {
            ushort valueUShort = MoreMath.NormalizeAngleUshort(value);
            switch (posAngleId.PosAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    SpecialConfig.CustomAngle = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return Config.Stream.SetValue(valueUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                case PositionAngleTypeEnum.Holp:
                    return false;
                case PositionAngleTypeEnum.Camera:
                    return Config.Stream.SetValue(valueUShort, CameraConfig.CameraStructAddress + CameraConfig.FacingYawOffset);
                case PositionAngleTypeEnum.Object:
                    bool success = true;
                    success &= Config.Stream.SetValue(valueUShort, posAngleId.Address.Value + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(valueUShort, posAngleId.Address.Value + ObjectConfig.YawMovingOffset);
                    return success;
                case PositionAngleTypeEnum.ObjectHome:
                    return false;
                case PositionAngleTypeEnum.Tri:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
