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
    public class PositionAngleId
    {
        public readonly PositionAngleTypeEnum PosAngleType;
        public readonly uint? Address;
        public readonly int? TriVertex;

        public double X { get => PositionAngle.FromId(this).X; }
        public double Y { get => PositionAngle.FromId(this).Y; }
        public double Z { get => PositionAngle.FromId(this).Z; }
        public double? Angle { get => PositionAngle.FromId(this).Angle; }

        public PositionAngleId(
            PositionAngleTypeEnum posAngleType,
            uint? address = null,
            int? triVertex = null)
        {
            PosAngleType = posAngleType;
            Address = address;
            TriVertex = triVertex;

            bool shouldHaveAddress =
                posAngleType == PositionAngleTypeEnum.Object ||
                posAngleType == PositionAngleTypeEnum.ObjectHome ||
                posAngleType == PositionAngleTypeEnum.Tri;
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveTriVertex =
                posAngleType == PositionAngleTypeEnum.Tri;
            if (triVertex.HasValue != shouldHaveTriVertex)
                throw new ArgumentOutOfRangeException();
        }

        public override string ToString()
        {
            string addressString = Address.HasValue ? " " + HexUtilities.FormatValue(Address.Value, 8) : "";
            string triVertexString = TriVertex.HasValue ? " V" + TriVertex.Value : "";
            return PosAngleType + addressString + triVertexString;
        }

        public static PositionAngleId FromString(string stringValue)
        {
            stringValue = stringValue.ToLower();
            List<string> parts = ParsingUtilities.ParseStringList(stringValue);

            if (parts.Count == 1 && parts[0] == "custom")
            {
                return new PositionAngleId(PositionAngleTypeEnum.Custom);
            }
            else if (parts.Count == 1 && parts[0] == "mario")
            {
                return new PositionAngleId(PositionAngleTypeEnum.Mario);
            }
            else if (parts.Count == 1 && parts[0] == "holp")
            {
                return new PositionAngleId(PositionAngleTypeEnum.Holp);
            }
            else if (parts.Count == 1 && (parts[0] == "cam" || parts[0] == "camera"))
            {
                return new PositionAngleId(PositionAngleTypeEnum.Camera);
            }
            else if (parts.Count == 2 && (parts[0] == "obj" || parts[0] == "object"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return new PositionAngleId(PositionAngleTypeEnum.Object, address.Value);
            }
            else if (parts.Count == 2 && (parts[0] == "objhome" || parts[0] == "objecthome"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return new PositionAngleId(PositionAngleTypeEnum.ObjectHome, address.Value);
            }
            else if (parts.Count == 3 && (parts[0] == "tri" || parts[0] == "triangle"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                if (parts[2].Length >= 1 && parts[2].Substring(0, 1) == "v") parts[2] = parts[2].Substring(1);
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[2]);
                if (!triVertex.HasValue || triVertex.Value < 1 || triVertex.Value > 3) return null;
                return new PositionAngleId(PositionAngleTypeEnum.Tri, address.Value, triVertex.Value);
            }

            return null;
        }








        public bool SetX(double value)
        {
            switch (PosAngleType)
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
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.XOffset);
                case PositionAngleTypeEnum.ObjectHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeXOffset);
                case PositionAngleTypeEnum.Tri:
                    uint triVertexOffset;
                    switch (TriVertex.Value)
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
                    return Config.Stream.SetValue((float)value, Address.Value + triVertexOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetY(double value)
        {
            switch (PosAngleType)
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
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.YOffset);
                case PositionAngleTypeEnum.ObjectHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeYOffset);
                case PositionAngleTypeEnum.Tri:
                    uint triVertexOffset;
                    switch (TriVertex.Value)
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
                    return Config.Stream.SetValue((float)value, Address.Value + triVertexOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetZ(double value)
        {
            switch (PosAngleType)
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
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ZOffset);
                case PositionAngleTypeEnum.ObjectHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeZOffset);
                case PositionAngleTypeEnum.Tri:
                    uint triVertexOffset;
                    switch (TriVertex.Value)
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
                    return Config.Stream.SetValue((float)value, Address.Value + triVertexOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetAngle(double value)
        {
            ushort valueUShort = MoreMath.NormalizeAngleUshort(value);
            switch (PosAngleType)
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
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawMovingOffset);
                    return success;
                case PositionAngleTypeEnum.ObjectHome:
                    return false;
                case PositionAngleTypeEnum.Tri:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }






        public static double GetDistance(PositionAngleId p1, PositionAngleId p2)
        {
            return MoreMath.GetDistanceBetween(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z);
        }

        public static double GetHDistance(PositionAngleId p1, PositionAngleId p2)
        {
            return MoreMath.GetDistanceBetween(p1.X, p1.Z, p2.X, p2.Z);
        }

        public static double GetXDistance(PositionAngleId p1, PositionAngleId p2)
        {
            return p2.X - p1.X;
        }

        public static double GetYDistance(PositionAngleId p1, PositionAngleId p2)
        {
            return p2.Y - p1.Y;
        }

        public static double GetZDistance(PositionAngleId p1, PositionAngleId p2)
        {
            return p2.Z - p1.Z;
        }

        public static double GetAngle(PositionAngleId p1, PositionAngleId p2)
        {
            return MoreMath.AngleTo_AngleUnits(p1.X, p1.Z, p2.X, p2.Z);
        }

        public static double GetDAngle(PositionAngleId p1, PositionAngleId p2)
        {
            double angle = MoreMath.AngleTo_AngleUnits(p1.X, p1.Z, p2.X, p2.Z);
            double angleDiff = p1.Angle.Value - angle;
            return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
        }

        public static double GetInGameAngle(PositionAngleId p1, PositionAngleId p2)
        {
            return InGameTrigUtilities.InGameAngleTo(
                (float)p1.X, (float)p1.Z, (float)p2.X, (float)p2.Z);
        }

        public static double GetInGameDAngle(PositionAngleId p1, PositionAngleId p2)
        {
            double angle = InGameTrigUtilities.InGameAngleTo(
                (float)p1.X, (float)p1.Z, (float)p2.X, (float)p2.Z);
            double angleDiff = p1.Angle.Value - angle;
            return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
        }

        private static bool CombineBools(params bool[] bools)
        {
            bool success = true;
            foreach (bool b in bools)
            {
                success &= b;
            }
            return success;
        }

        public static bool SetDistance(PositionAngleId id1, PositionAngleId id2, string distance, bool move1)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetDistance(id1, id2, distanceDouble.Value, move1);
        }

        public static bool SetDistance(PositionAngleId p1, PositionAngleId p2, double distance, bool move1)
        {
            PositionAngleId pF = move1 ? p2 : p1;
            PositionAngleId pM = move1 ? p1 : p2;
            (double x, double y, double z) = MoreMath.ExtrapolateLine3D(pF.X, pF.Y, pF.Z, pM.X, pM.Y, pM.Z, distance);
            return CombineBools(pM.SetX(x), pM.SetY(y), pM.SetZ(z));
        }
    }
}
