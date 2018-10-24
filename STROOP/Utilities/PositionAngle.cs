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

        public static Dictionary<uint, (double, double, double, double)> Schedule =
            new Dictionary<uint, (double, double, double, double)>();

        private enum PositionAngleTypeEnum
        {
            Custom,
            Mario,
            Holp,
            Camera,
            CameraFocus,
            CamHackCamera,
            CamHackFocus,
            Obj,
            ObjHome,
            ObjGfx,
            ObjScale,
            Tri,
            Wall,
            Floor,
            Ceiling,
            Schedule,
            Hybrid,
        }

        private bool ShouldHaveAddress(PositionAngleTypeEnum posAngleType)
        {
            return posAngleType == PositionAngleTypeEnum.Obj ||
                posAngleType == PositionAngleTypeEnum.ObjHome ||
                posAngleType == PositionAngleTypeEnum.ObjGfx ||
                posAngleType == PositionAngleTypeEnum.ObjScale ||
                posAngleType == PositionAngleTypeEnum.Tri;
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

            bool shouldHaveAddress = ShouldHaveAddress(posAngleType);
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveTriVertex =
                posAngleType == PositionAngleTypeEnum.Tri ||
                posAngleType == PositionAngleTypeEnum.Wall ||
                posAngleType == PositionAngleTypeEnum.Floor ||
                posAngleType == PositionAngleTypeEnum.Ceiling;
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
        public static PositionAngle CameraFocus = new PositionAngle(PositionAngleTypeEnum.CameraFocus);
        public static PositionAngle CamHackCamera = new PositionAngle(PositionAngleTypeEnum.CamHackCamera);
        public static PositionAngle CamHackFocus = new PositionAngle(PositionAngleTypeEnum.CamHackFocus);
        public static PositionAngle Scheduler = new PositionAngle(PositionAngleTypeEnum.Schedule);

        public static PositionAngle Obj(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.Obj, address);
        public static PositionAngle ObjHome(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjHome, address);
        public static PositionAngle MarioObj() => Obj(Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress));
        public static PositionAngle ObjGfx(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjGfx, address);
        public static PositionAngle Ghost() =>
            ObjGfx(Config.ObjectSlotsManager.GetLoadedObjectsWithName("Mario Ghost")
                .ConvertAll(objectDataModel => objectDataModel.Address).FirstOrDefault());
        public static PositionAngle ObjScale(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjScale, address);
        public static PositionAngle Tri(uint address, int triVertex) =>
            new PositionAngle(PositionAngleTypeEnum.Tri, address, triVertex);
        public static PositionAngle Wall(int triVertex) =>
            new PositionAngle(PositionAngleTypeEnum.Wall, null, triVertex);
        public static PositionAngle Floor(int triVertex) =>
            new PositionAngle(PositionAngleTypeEnum.Floor, null, triVertex);
        public static PositionAngle Ceiling(int triVertex) =>
            new PositionAngle(PositionAngleTypeEnum.Ceiling, null, triVertex);
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
            else if (parts.Count == 1 && (parts[0] == "camfocus" || parts[0] == "camerafocus"))
            {
                return CameraFocus;
            }
            else if (parts.Count == 1 && (parts[0] == "camhackcam" || parts[0] == "camhackcamera"))
            {
                return CamHackCamera;
            }
            else if (parts.Count == 1 && parts[0] == "camhackfocus")
            {
                return CamHackFocus;
            }
            else if (parts.Count == 1 && parts[0] == "ghost")
            {
                return Ghost();
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
            else if (parts.Count == 2 && 
                (parts[0] == "objgfx" || parts[0] == "objectgfx" || parts[0] == "objgraphics" || parts[0] == "objectgraphics"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return ObjGfx(address.Value);
            }
            else if (parts.Count == 2 && (parts[0] == "objscale" || parts[0] == "objectscale"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return ObjScale(address.Value);
            }
            else if (parts.Count == 3 && (parts[0] == "tri" || parts[0] == "triangle"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[2]);
                if (!triVertex.HasValue || triVertex.Value < 0 || triVertex.Value > 4) return null;
                // 0 = closest vertex
                // 1 = vertex 1
                // 2 = vertex 2
                // 3 = vertex 3
                // 4 = point on triangle
                return Tri(address.Value, triVertex.Value);
            }
            else if (parts.Count == 2 && parts[0] == "wall")
            {
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!triVertex.HasValue || triVertex.Value < 0 || triVertex.Value > 4) return null;
                return Wall(triVertex.Value);
            }
            else if (parts.Count == 2 && parts[0] == "floor")
            {
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!triVertex.HasValue || triVertex.Value < 0 || triVertex.Value > 4) return null;
                return Floor(triVertex.Value);
            }
            else if (parts.Count == 2 && parts[0] == "ceiling")
            {
                int? triVertex = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!triVertex.HasValue || triVertex.Value < 0 || triVertex.Value > 4) return null;
                return Ceiling(triVertex.Value);
            }
            else if (parts.Count == 1 && parts[0] == "schedule")
            {
                return Scheduler;
            }

            return null;
        }

        public override string ToString()
        {
            List<object> parts = new List<object>();
            parts.Add(PosAngleType);
            if (Address.HasValue) parts.Add(HexUtilities.FormatValue(Address.Value, 8));
            if (TriVertex.HasValue) parts.Add(TriVertex.Value);
            if (PosPA != null) parts.Add("[" + PosPA + "]");
            if (AnglePA != null) parts.Add("[" + AnglePA + "]");
            return String.Join(" ", parts);
        }






        public double X
        {
            get
            {
                if (ShouldHaveAddress(PosAngleType) && Address == 0) return Double.NaN;
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomX;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpXOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.XOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusXOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    case PositionAngleTypeEnum.CamHackFocus:
                        return Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.XOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeXOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.GraphicsXOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ScaleWidthOffset);
                    case PositionAngleTypeEnum.Tri:
                        return GetTriangleVertexComponent(Address.Value, TriVertex.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Wall:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), TriVertex.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), TriVertex.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), TriVertex.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item1;
                        return Double.NaN;
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
                if (ShouldHaveAddress(PosAngleType) && Address == 0) return Double.NaN;
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomY;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpYOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.YOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusYOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    case PositionAngleTypeEnum.CamHackFocus:
                        return Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeYOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.GraphicsYOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ScaleHeightOffset);
                    case PositionAngleTypeEnum.Tri:
                        return GetTriangleVertexComponent(Address.Value, TriVertex.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Wall:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), TriVertex.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), TriVertex.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), TriVertex.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item2;
                        return Double.NaN;
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
                if (ShouldHaveAddress(PosAngleType) && Address == 0) return Double.NaN;
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomZ;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpZOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.ZOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusZOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    case PositionAngleTypeEnum.CamHackFocus:
                        return Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ZOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeZOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.GraphicsZOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ScaleDepthOffset);
                    case PositionAngleTypeEnum.Tri:
                        return GetTriangleVertexComponent(Address.Value, TriVertex.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Wall:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), TriVertex.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), TriVertex.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), TriVertex.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item3;
                        return Double.NaN;
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
                if (ShouldHaveAddress(PosAngleType) && Address == 0) return Double.NaN;
                switch (PosAngleType)
                {
                    case PositionAngleTypeEnum.Custom:
                        return SpecialConfig.CustomAngle;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Double.NaN;
                    case PositionAngleTypeEnum.CamHackCamera:
                        return CamHackUtilities.GetCamHackYawFacing();
                    case PositionAngleTypeEnum.CamHackFocus:
                        return CamHackUtilities.GetCamHackYawFacing();
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetUInt16(Address.Value + ObjectConfig.YawFacingOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Double.NaN;
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetUInt16(Address.Value + ObjectConfig.GraphicsYawOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Tri:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Wall:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Floor:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Ceiling:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item4;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return AnglePA.Angle;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private static double GetTriangleVertexComponent(uint address, int triVertex, Coordinate coordinate)
        {
            if (address == 0) return Double.NaN;
            switch (triVertex)
            {
                case 0:
                    int closestVertex = new TriangleDataModel(address).GetClosestVertex(
                        SpecialConfig.SelfX, SpecialConfig.SelfY, SpecialConfig.SelfZ);
                    return GetTriangleVertexComponent(address, closestVertex, coordinate);
                case 1:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.X1);
                        case Coordinate.Y:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.Y1);
                        case Coordinate.Z:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.Z1);
                    }
                    break;
                case 2:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.X2);
                        case Coordinate.Y:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.Y2);
                        case Coordinate.Z:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.Z2);
                    }
                    break;
                case 3:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.X3);
                        case Coordinate.Y:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.Y3);
                        case Coordinate.Z:
                            return Config.Stream.GetInt16(address + TriangleOffsetsConfig.Z3);
                    }
                    break;
                case 4:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return SpecialConfig.SelfX;
                        case Coordinate.Y:
                            return new TriangleDataModel(address).GetHeightOnTriangle(SpecialConfig.SelfX, SpecialConfig.SelfZ);
                        case Coordinate.Z:
                            return SpecialConfig.SelfZ;
                    }
                    break;
            }
            throw new ArgumentOutOfRangeException();
        }




        public bool SetX(double value)
        {
            if (ShouldHaveAddress(PosAngleType) && Address == 0) return false;
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
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.XOffset);
                case PositionAngleTypeEnum.CameraFocus:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.FocusXOffset);
                case PositionAngleTypeEnum.CamHackCamera:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                case PositionAngleTypeEnum.CamHackFocus:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.XOffset);
                case PositionAngleTypeEnum.ObjHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeXOffset);
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.GraphicsXOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ScaleWidthOffset);
                case PositionAngleTypeEnum.Tri:
                    return SetTriangleVertexComponent((short)value, Address.Value, TriVertex.Value, Coordinate.X);
                case PositionAngleTypeEnum.Wall:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), TriVertex.Value, Coordinate.X);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), TriVertex.Value, Coordinate.X);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), TriVertex.Value, Coordinate.X);
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosPA.SetX(value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetY(double value)
        {
            if (ShouldHaveAddress(PosAngleType) && Address == 0) return false;
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
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.YOffset);
                case PositionAngleTypeEnum.CameraFocus:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.FocusYOffset);
                case PositionAngleTypeEnum.CamHackCamera:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                case PositionAngleTypeEnum.CamHackFocus:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.YOffset);
                case PositionAngleTypeEnum.ObjHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeYOffset);
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.GraphicsYOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ScaleHeightOffset);
                case PositionAngleTypeEnum.Tri:
                    return SetTriangleVertexComponent((short)value, Address.Value, TriVertex.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Wall:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), TriVertex.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), TriVertex.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), TriVertex.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosPA.SetY(value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetZ(double value)
        {
            if (ShouldHaveAddress(PosAngleType) && Address == 0) return false;
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
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.ZOffset);
                case PositionAngleTypeEnum.CameraFocus:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.FocusZOffset);
                case PositionAngleTypeEnum.CamHackCamera:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                case PositionAngleTypeEnum.CamHackFocus:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ZOffset);
                case PositionAngleTypeEnum.ObjHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeZOffset);
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.GraphicsZOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ScaleDepthOffset);
                case PositionAngleTypeEnum.Tri:
                    return SetTriangleVertexComponent((short)value, Address.Value, TriVertex.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Wall:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), TriVertex.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), TriVertex.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), TriVertex.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosPA.SetZ(value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool SetAngle(double value)
        {
            if (ShouldHaveAddress(PosAngleType) && Address == 0) return false;
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
                    return Config.Stream.SetValue(valueUShort, CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                case PositionAngleTypeEnum.CameraFocus:
                    return false;
                case PositionAngleTypeEnum.CamHackCamera:
                    return false;
                case PositionAngleTypeEnum.CamHackFocus:
                    return false;
                case PositionAngleTypeEnum.Obj:
                    bool success = true;
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawMovingOffset);
                    return success;
                case PositionAngleTypeEnum.ObjHome:
                    return false;
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.GraphicsYawOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return false;
                case PositionAngleTypeEnum.Tri:
                    return false;
                case PositionAngleTypeEnum.Wall:
                    return false;
                case PositionAngleTypeEnum.Floor:
                    return false;
                case PositionAngleTypeEnum.Ceiling:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return AnglePA.SetAngle(value);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool SetTriangleVertexComponent(short value, uint address, int triVertex, Coordinate coordinate)
        {
            if (address == 0) return false;
            switch (triVertex)
            {
                case 0:
                    int closestVertex = new TriangleDataModel(address).GetClosestVertex(
                        SpecialConfig.SelfX, SpecialConfig.SelfY, SpecialConfig.SelfZ);
                    return SetTriangleVertexComponent(value, address, closestVertex, coordinate);
                case 1:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.X1);
                        case Coordinate.Y:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.Y1);
                        case Coordinate.Z:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.Z1);
                    }
                    break;
                case 2:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.X2);
                        case Coordinate.Y:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.Y2);
                        case Coordinate.Z:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.Z2);
                    }
                    break;
                case 3:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.X3);
                        case Coordinate.Y:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.Y3);
                        case Coordinate.Z:
                            return Config.Stream.SetValue(value, address + TriangleOffsetsConfig.Z3);
                    }
                    break;
                case 4:
                    return false;
            }
            throw new ArgumentOutOfRangeException();
        }

        public bool SetValues(double? x = null, double? y = null, double? z = null, double? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= SetX(x.Value);
            if (y.HasValue) success &= SetY(y.Value);
            if (z.HasValue) success &= SetZ(z.Value);
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





        private static bool GetToggle()
        {
            return KeyboardUtilities.IsCtrlHeld();
        }

        public static bool SetDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                (double x, double y, double z) = MoreMath.ExtrapolateLine3D(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, distance);
                return p2.SetValues(x: x, y: y, z: z);
            }
            else
            {
                (double x, double y, double z) = MoreMath.ExtrapolateLine3D(p2.X, p2.Y, p2.Z, p1.X, p1.Y, p1.Z, distance);
                return p1.SetValues(x: x, y: y, z: z);
            }
        }

        public static bool SetHDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                (double x, double z) = MoreMath.ExtrapolateLine2D(p1.X, p1.Z, p2.X, p2.Z, distance);
                return p2.SetValues(x: x, z: z);
            }
            else
            {
                (double x, double z) = MoreMath.ExtrapolateLine2D(p2.X, p2.Z, p1.X, p1.Z, distance);
                return p1.SetValues(x: x, z: z);
            }
        }

        public static bool SetXDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                double x = p1.X + distance;
                return p2.SetValues(x: x);
            }
            else
            {
                double x = p2.X - distance;
                return p1.SetValues(x: x);
            }
        }

        public static bool SetYDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                double y = p1.Y + distance;
                return p2.SetValues(y: y);
            }
            else
            {
                double y = p2.Y - distance;
                return p1.SetValues(y: y);
            }
        }

        public static bool SetZDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                double z = p1.Z + distance;
                return p2.SetValues(z: z);
            }
            else
            {
                double z = p2.Z - distance;
                return p1.SetValues(z: z);
            }
        }

        public static bool SetFDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                (double x, double z) =
                    MoreMath.GetRelativelyOffsettedPosition(
                        p1.X, p1.Z, p1.Angle, p2.X, p2.Z, null, distance);
                return p2.SetValues(x: x, z: z);
            }
            else
            {
                (double x, double z) =
                    MoreMath.GetRelativelyOffsettedPosition(
                        p2.X, p2.Z, p1.Angle, p1.X, p1.Z, null, -1 * distance);
                return p1.SetValues(x: x, z: z);
            }
        }

        public static bool SetSDistance(PositionAngle p1, PositionAngle p2, double distance, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                (double x, double z) =
                    MoreMath.GetRelativelyOffsettedPosition(
                        p1.X, p1.Z, p1.Angle, p2.X, p2.Z, distance, null);
                return p2.SetValues(x: x, z: z);            }
            else
            {
                (double x, double z) =
                    MoreMath.GetRelativelyOffsettedPosition(
                        p2.X, p2.Z, p1.Angle, p1.X, p1.Z, -1 * distance, null);
                return p1.SetValues(x: x, z: z);
            }
        }

        public static bool SetAngleTo(PositionAngle p1, PositionAngle p2, double angle, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                (double x, double z) = 
                    MoreMath.RotatePointAboutPointToAngle(
                        p2.X, p2.Z, p1.X, p1.Z, angle);
                return p2.SetValues(x: x, z: z);
            }
            else
            {
                (double x, double z) =
                    MoreMath.RotatePointAboutPointToAngle(
                        p1.X, p1.Z, p2.X, p2.Z, MoreMath.ReverseAngle(angle));
                return p1.SetValues(x: x, z: z);
            }
        }

        public static bool SetDAngleTo(PositionAngle p1, PositionAngle p2, double angleDiff, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                double currentAngle = MoreMath.AngleTo_AngleUnits(p1.X, p1.Z, p2.X, p2.Z);
                double newAngle = currentAngle + angleDiff;
                return p1.SetValues(angle: newAngle);
            }
            else
            {
                double newAngle = p1.Angle - angleDiff;
                (double x, double z) =
                    MoreMath.RotatePointAboutPointToAngle(
                        p2.X, p2.Z, p1.X, p1.Z, newAngle);
                return p2.SetValues(x: x, z: z);
            }
        }

        public static bool SetAngleDifference(PositionAngle p1, PositionAngle p2, double angleDiff, bool? toggleNullable = null)
        {
            bool toggle = toggleNullable ?? GetToggle();
            if (!toggle)
            {
                double newAngle = p2.Angle + angleDiff;
                return p1.SetValues(angle: newAngle);
            }
            else
            {
                double newAngle = p1.Angle - angleDiff;
                return p2.SetValues(angle: newAngle);
            }
        }

    }
}
