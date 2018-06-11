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
        private readonly PositionAngleTypeEnum PosAngleType;
        public readonly uint? Address;
        public readonly int? TriVertex;
        public readonly PositionAngle PosPA;
        public readonly PositionAngle AnglePA;

        private enum PositionAngleTypeEnum
        {
            Custom,
            Mario,
            Holp,
            Camera,
            Ghost,
            Obj,
            ObjHome,
            Tri,
            Hybrid,
        }

        private uint? GetGhostAddress()
        {
            List<uint> addresses = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Mario Ghost")
                .ConvertAll(objectDataModel => objectDataModel.Address);
            return addresses.Count > 0 ? addresses[0] : (uint?)null;
        }

        public double X
        {
            get
            {
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomX;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpXOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
                    case PositionAngleTypeEnum.Ghost:
                        uint? ghostAddress = GetGhostAddress();
                        if (!ghostAddress.HasValue) return Double.NaN;
                        return Config.Stream.GetSingle(ghostAddress.Value + ObjectConfig.GraphicsXOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.XOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeXOffset);
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
                        return Config.Stream.GetInt16(Address.Value + triVertexOffset);
                    case PositionAngleTypeEnum.Hybrid:
                        return PosPA.X;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public double Y
        {
            get
            {
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomY;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpYOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
                    case PositionAngleTypeEnum.Ghost:
                        uint? ghostAddress = GetGhostAddress();
                        if (!ghostAddress.HasValue) return Double.NaN;
                        return Config.Stream.GetSingle(ghostAddress.Value + ObjectConfig.GraphicsYOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeYOffset);
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
                        return Config.Stream.GetInt16(Address.Value + triVertexOffset);
                    case PositionAngleTypeEnum.Hybrid:
                        return PosPA.Y;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public double Z
        {
            get
            {
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomZ;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpZOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
                    case PositionAngleTypeEnum.Ghost:
                        uint? ghostAddress = GetGhostAddress();
                        if (!ghostAddress.HasValue) return Double.NaN;
                        return Config.Stream.GetSingle(ghostAddress.Value + ObjectConfig.GraphicsZOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ZOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeZOffset);
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
                        return Config.Stream.GetInt16(Address.Value + triVertexOffset);
                    case PositionAngleTypeEnum.Hybrid:
                        return PosPA.Z;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public double Angle
        {
            get
            {
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomAngle;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.Ghost:
                        uint? ghostAddress = GetGhostAddress();
                        if (!ghostAddress.HasValue) return Double.NaN;
                        return Config.Stream.GetUInt16(ghostAddress.Value + ObjectConfig.GraphicsYawOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetUInt16(Address.Value + ObjectConfig.YawFacingOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Tri:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return AnglePA.Angle;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private PositionAngle(
            PositionAngleTypeEnum posAngleType,
            uint? address = null,
            int? triVertex = null,
            PositionAngle posPA = null,
            PositionAngle anglePA = null)
        {
            PosAngleType = posAngleType;
            Address = address;
            TriVertex = triVertex;
            PosPA = posPA;
            AnglePA = anglePA;

            bool shouldHaveAddress =
                posAngleType == PositionAngleTypeEnum.Obj ||
                posAngleType == PositionAngleTypeEnum.ObjHome ||
                posAngleType == PositionAngleTypeEnum.Tri;
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveTriVertex =
                posAngleType == PositionAngleTypeEnum.Tri;
            if (triVertex.HasValue != shouldHaveTriVertex)
                throw new ArgumentOutOfRangeException();

            bool shouldHavePAs =
                PosAngleType == PositionAngleTypeEnum.Hybrid;
            if ((posPA != null) != shouldHavePAs)
                throw new ArgumentOutOfRangeException();
            if ((anglePA != null) != shouldHavePAs)
                throw new ArgumentOutOfRangeException();
        }

        public static PositionAngle Custom = new PositionAngle(PositionAngleTypeEnum.Custom);
        public static PositionAngle Mario = new PositionAngle(PositionAngleTypeEnum.Mario);
        public static PositionAngle Holp = new PositionAngle(PositionAngleTypeEnum.Holp);
        public static PositionAngle Camera = new PositionAngle(PositionAngleTypeEnum.Camera);
        public static PositionAngle Ghost = new PositionAngle(PositionAngleTypeEnum.Ghost);
        public static PositionAngle Obj(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.Obj, address);
        public static PositionAngle ObjHome(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjHome, address);
        public static PositionAngle Tri(uint address, int triVertex) =>
            new PositionAngle(PositionAngleTypeEnum.Tri, address, triVertex);
        public static PositionAngle Hybrid(PositionAngle posPA, PositionAngle anglePA) =>
            new PositionAngle(PositionAngleTypeEnum.Hybrid, null, null, posPA, anglePA);

        public static PositionAngle FromString(string stringValue)
        {
            stringValue = stringValue.ToLower();
            List<string> parts = ParsingUtilities.ParseStringList(stringValue);

            if (parts.Count == 1 && parts[0] == "custom")
            {
                return Custom;
            }
            else if (parts.Count == 1 && parts[0] == "mario")
            {
                return Mario;
            }
            else if (parts.Count == 1 && parts[0] == "holp")
            {
                return Holp;
            }
            else if (parts.Count == 1 && (parts[0] == "cam" || parts[0] == "camera"))
            {
                return Camera;
            }
            else if (parts.Count == 1 && parts[0] == "ghost")
            {
                return Ghost;
            }
            else if (parts.Count == 2 && (parts[0] == "obj" || parts[0] == "object"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return Obj(address.Value);
            }
            else if (parts.Count == 2 && (parts[0] == "objhome" || parts[0] == "objecthome"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return ObjHome(address.Value);
            }
            else if (parts.Count == 3 && (parts[0] == "tri" || parts[0] == "triangle"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                if (parts[2].Length >= 1 && parts[2].Substring(0, 1) == "v") parts[2] = parts[2].Substring(1);
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[2]);
                if (!triVertex.HasValue || triVertex.Value < 1 || triVertex.Value > 3) return null;
                return Tri(address.Value, triVertex.Value);
            }

            return null;
        }

        public override string ToString()
        {
            List<string> strings = new List<string>();
            strings.Add(PosAngleType.ToString());
            if (Address.HasValue) strings.Add(HexUtilities.FormatValue(Address.Value, 8));
            if (TriVertex.HasValue) strings.Add("V" + TriVertex.Value);
            if (PosPA != null) strings.Add("[" + PosPA + "]");
            if (AnglePA != null) strings.Add("[" + AnglePA + "]");
            return String.Join(" ", strings);
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
                case PositionAngleTypeEnum.Ghost:
                    return false;
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.XOffset);
                case PositionAngleTypeEnum.ObjHome:
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
                case PositionAngleTypeEnum.Hybrid:
                    return PosPA.SetX(value);
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
                case PositionAngleTypeEnum.Ghost:
                    return false;
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.YOffset);
                case PositionAngleTypeEnum.ObjHome:
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
                case PositionAngleTypeEnum.Hybrid:
                    return PosPA.SetY(value);
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
                case PositionAngleTypeEnum.Ghost:
                    return false;
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ZOffset);
                case PositionAngleTypeEnum.ObjHome:
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
                case PositionAngleTypeEnum.Hybrid:
                    return PosPA.SetZ(value);
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
                case PositionAngleTypeEnum.Ghost:
                    return false;
                case PositionAngleTypeEnum.Obj:
                    bool success = true;
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawMovingOffset);
                    return success;
                case PositionAngleTypeEnum.ObjHome:
                    return false;
                case PositionAngleTypeEnum.Tri:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return AnglePA.SetAngle(value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetValues(double? x = null, double? y = null, double? z = null, double? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= SetX(x.Value);
            if (y.HasValue) success &= SetX(y.Value);
            if (z.HasValue) success &= SetX(z.Value);
            if (angle.HasValue) success &= SetAngle(angle.Value);
            return success;
        }






        public static double GetDistance(PositionAngle p1, PositionAngle p2)
        {
            return MoreMath.GetDistanceBetween(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z);
        }

        public static double GetHDistance(PositionAngle p1, PositionAngle p2)
        {
            return MoreMath.GetDistanceBetween(p1.X, p1.Z, p2.X, p2.Z);
        }

        public static double GetXDistance(PositionAngle p1, PositionAngle p2)
        {
            return p2.X - p1.X;
        }

        public static double GetYDistance(PositionAngle p1, PositionAngle p2)
        {
            return p2.Y - p1.Y;
        }

        public static double GetZDistance(PositionAngle p1, PositionAngle p2)
        {
            return p2.Z - p1.Z;
        }

        public static double GetFDistance(PositionAngle p1, PositionAngle p2)
        {
            double hDist = MoreMath.GetDistanceBetween(p1.X, p1.Z, p2.X, p2.Z);
            double angle = MoreMath.AngleTo_AngleUnits(p1.X, p1.Z, p2.X, p2.Z);
            (double sidewaysDist, double forwardsDist) =
                MoreMath.GetComponentsFromVectorRelatively(hDist, angle, p1.Angle);
            return forwardsDist;
        }

        public static double GetSDistance(PositionAngle p1, PositionAngle p2)
        {
            double hDist = MoreMath.GetDistanceBetween(p1.X, p1.Z, p2.X, p2.Z);
            double angle = MoreMath.AngleTo_AngleUnits(p1.X, p1.Z, p2.X, p2.Z);
            (double sidewaysDist, double forwardsDist) =
                MoreMath.GetComponentsFromVectorRelatively(hDist, angle, p1.Angle);
            return sidewaysDist;
        }

        private static double AngleTo(double x1, double z1, double x2, double z2, bool inGameAngle, bool truncate)
        {
            double angleTo = inGameAngle
                ? InGameTrigUtilities.InGameAngleTo((float)x1, (float)z1, (float)x2, (float)z2)
                : MoreMath.AngleTo_AngleUnits(x1, z1, x2, z2);
            if (truncate) angleTo = MoreMath.NormalizeAngleTruncated(angleTo);
            return angleTo;
        }

        public static double GetAngleTo(PositionAngle p1, PositionAngle p2, bool? inGameAngleNullable, bool truncate)
        {
            bool inGameAngle = inGameAngleNullable ?? SavedSettingsConfig.UseInGameTrigForAngleLogic;
            return AngleTo(p1.X, p1.Z, p2.X, p2.Z, inGameAngle, truncate);
        }

        public static double GetDAngleTo(PositionAngle p1, PositionAngle p2, bool? inGameAngleNullable, bool truncate)
        {
            bool inGameAngle = inGameAngleNullable ?? SavedSettingsConfig.UseInGameTrigForAngleLogic;
            double angleTo = AngleTo(p1.X, p1.Z, p2.X, p2.Z, inGameAngle, truncate);
            double angle = truncate ? MoreMath.NormalizeAngleTruncated(p1.Angle) : p1.Angle;
            double angleDiff = angle - angleTo;
            return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
        }

        public static double GetAngleDifference(PositionAngle p1, PositionAngle p2, bool truncate)
        {
            double angle1 = truncate ? MoreMath.NormalizeAngleTruncated(p1.Angle) : p1.Angle;
            double angle2 = truncate ? MoreMath.NormalizeAngleTruncated(p2.Angle) : p2.Angle;
            double angleDiff = angle1 - angle2;
            return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
        }

        public static bool SetDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            (double x, double y, double z) = MoreMath.ExtrapolateLine3D(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, distance);
            return p2.SetValues(x: x, y: y, z: z);
        }

        public static bool SetHDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetHDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetHDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            (double x, double z) = MoreMath.ExtrapolateLine2D(p1.X, p1.Z, p2.X, p2.Z, distance);
            return p2.SetValues(x: x, z: z);
        }

        public static bool SetXDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetXDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetXDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            double x = p1.X + distance;
            return p2.SetValues(x: x);
        }

        public static bool SetYDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetYDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetYDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            double y = p1.Y + distance;
            return p2.SetValues(y: y);
        }

        public static bool SetZDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetZDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetZDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            double z = p1.Z + distance;
            return p2.SetValues(z: z);
        }

        public static bool SetFDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetFDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetFDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            (double x, double z) =
                MoreMath.GetRelativelyOffsettedPosition(
                    p1.X, p1.Z, p1.Angle, p2.X, p2.Z, null, distance);
            return p2.SetValues(x: x, z: z);
        }

        public static bool SetSDistance(PositionAngle p1, PositionAngle p2, object distance)
        {
            double? distanceDouble = ParsingUtilities.ParseDoubleNullable(distance);
            if (!distanceDouble.HasValue) return false;
            return SetSDistance(p1, p2, distanceDouble.Value);
        }

        public static bool SetSDistance(PositionAngle p1, PositionAngle p2, double distance)
        {
            (double x, double z) =
                MoreMath.GetRelativelyOffsettedPosition(
                    p1.X, p1.Z, p1.Angle, p2.X, p2.Z, distance, null);
            return p2.SetValues(x: x, z: z);
        }

        public static bool SetAngleTo(PositionAngle p1, PositionAngle p2, object angle)
        {
            double? angleNullable = ParsingUtilities.ParseDoubleNullable(angle);
            if (!angleNullable.HasValue) return false;
            return SetAngleTo(p1, p2, angleNullable.Value);
        }

        public static bool SetAngleTo(PositionAngle p1, PositionAngle p2, double angle)
        {
            (double x, double z) =
                MoreMath.RotatePointAboutPointToAngle(
                    p2.X, p2.Z, p1.X, p1.Z, angle);
            return p2.SetValues(x: x, z: z);
        }

        public static bool SetDAngleTo(PositionAngle p1, PositionAngle p2, object angleDiff)
        {
            double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(angleDiff);
            if (!angleDiffNullable.HasValue) return false;
            return SetDAngleTo(p1, p2, angleDiffNullable.Value);
        }

        public static bool SetDAngleTo(PositionAngle p1, PositionAngle p2, double angleDiff)
        {
            double currentAngle = MoreMath.AngleTo_AngleUnits(p1.X, p1.Z, p2.X, p2.Z);
            double newAngle = currentAngle + angleDiff;
            return p1.SetValues(angle: newAngle);
        }

        public static bool SetAngleDifference(PositionAngle p1, PositionAngle p2, object angleDiff)
        {
            double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(angleDiff);
            if (!angleDiffNullable.HasValue) return false;
            return SetAngleDifference(p1, p2, angleDiffNullable.Value);
        }

        public static bool SetAngleDifference(PositionAngle p1, PositionAngle p2, double angleDiff)
        {
            double newAngle = p2.Angle + angleDiff;
            return p1.SetValues(angle: newAngle);
        }

    }
}
