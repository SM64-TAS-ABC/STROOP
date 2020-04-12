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
        private readonly uint? Address;
        private readonly int? Index;
        private readonly int? Index2;
        private readonly double? Frame;
        private double? ManualX;
        private double? ManualY;
        private double? ManualZ;
        private double? ManualAngle;
        private readonly PositionAngle PosAngle1;
        private readonly PositionAngle PosAngle2;
        private readonly List<Func<double>> Getters;
        private readonly List<Func<double, bool>> Setters;

        public static Dictionary<uint, (double, double, double, double, List<double>)> Schedule =
            new Dictionary<uint, (double, double, double, double, List<double>)>();

        private enum PositionAngleTypeEnum
        {
            Custom,
            Custom2,
            Mario,
            Holp,
            Camera,
            CameraFocus,
            CamHackCamera,
            CamHackFocus,
            MapCamera,
            MapFocus,
            Obj,
            ObjHome,
            ObjGfx,
            ObjScale,
            Selected,
            Moneybag,
            MoneybagHome,
            GoombaProjection,
            Ghost,
            Tri,
            ObjTri,
            Wall,
            Floor,
            Ceiling,
            Snow,
            QFrame,
            GFrame,
            Schedule,
            Hybrid,
            Trunc,
            Man,
            Functions,
            Self,
            Point,
            None,
        }

        public bool IsSelected
        {
            get => PosAngleType == PositionAngleTypeEnum.Selected;
        }

        private bool ShouldHaveAddress(PositionAngleTypeEnum posAngleType)
        {
            return posAngleType == PositionAngleTypeEnum.Obj ||
                posAngleType == PositionAngleTypeEnum.ObjHome ||
                posAngleType == PositionAngleTypeEnum.ObjGfx ||
                posAngleType == PositionAngleTypeEnum.ObjScale ||
                posAngleType == PositionAngleTypeEnum.GoombaProjection ||
                posAngleType == PositionAngleTypeEnum.Tri ||
                posAngleType == PositionAngleTypeEnum.ObjTri;
        }

        private bool ShouldHaveIndex(PositionAngleTypeEnum posAngleType)
        {
            return posAngleType == PositionAngleTypeEnum.Tri ||
                posAngleType == PositionAngleTypeEnum.ObjTri ||
                posAngleType == PositionAngleTypeEnum.Wall ||
                posAngleType == PositionAngleTypeEnum.Floor ||
                posAngleType == PositionAngleTypeEnum.Ceiling ||
                posAngleType == PositionAngleTypeEnum.Snow;
        }

        private bool ShouldHaveFrame(PositionAngleTypeEnum posAngleType)
        {
            return posAngleType == PositionAngleTypeEnum.QFrame ||
                posAngleType == PositionAngleTypeEnum.GFrame;
        }

        private PositionAngle(
            PositionAngleTypeEnum posAngleType,
            uint? address = null,
            int? index = null,
            int? index2 = null,
            double? frame = null,
            double? manualX = null,
            double? manualY = null,
            double? manualZ = null,
            double? manualAngle = null,
            PositionAngle posAngle1 = null,
            PositionAngle posAngle2 = null,
            List<Func<double>> getters = null,
            List<Func<double, bool>> setters = null)
        {
            PosAngleType = posAngleType;
            Address = address;
            Index = index;
            Index2 = index2;
            Frame = frame;
            ManualX = manualX;
            ManualY = manualY;
            ManualZ = manualZ;
            ManualAngle = manualAngle;
            PosAngle1 = posAngle1;
            PosAngle2 = posAngle2;
            Getters = getters;
            Setters = setters;

            bool shouldHaveAddress = ShouldHaveAddress(posAngleType);
            if (address.HasValue != shouldHaveAddress)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveIndex = ShouldHaveIndex(posAngleType);
            if (index.HasValue != shouldHaveIndex)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveIndex2 = PosAngleType == PositionAngleTypeEnum.ObjTri;
            if (index2.HasValue != shouldHaveIndex2)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveFrame = ShouldHaveFrame(posAngleType);
            if (frame.HasValue != shouldHaveFrame)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveManualX = PosAngleType == PositionAngleTypeEnum.Man;
            if (manualX.HasValue != shouldHaveManualX)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveManualY = PosAngleType == PositionAngleTypeEnum.Man;
            if (manualY.HasValue != shouldHaveManualY)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveManualZ = PosAngleType == PositionAngleTypeEnum.Man;
            if (manualZ.HasValue != shouldHaveManualZ)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveManualAngle = PosAngleType == PositionAngleTypeEnum.Man;
            if (manualAngle.HasValue != shouldHaveManualAngle)
                throw new ArgumentOutOfRangeException();

            bool shouldHavePosAngle1 =
                PosAngleType == PositionAngleTypeEnum.Hybrid ||
                PosAngleType == PositionAngleTypeEnum.Trunc;
            if ((posAngle1 != null) != shouldHavePosAngle1)
                throw new ArgumentOutOfRangeException();

            bool shouldHavePosAngle2 = PosAngleType == PositionAngleTypeEnum.Hybrid;
            if ((posAngle2 != null) != shouldHavePosAngle2)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveGetters = PosAngleType == PositionAngleTypeEnum.Functions;
            if ((getters != null) != shouldHaveGetters)
                throw new ArgumentOutOfRangeException();
            if (getters != null && (getters.Count < 3 || getters.Count > 4)) // optional angle getter
                throw new ArgumentOutOfRangeException();

            bool shouldHaveSetters = PosAngleType == PositionAngleTypeEnum.Functions;
            if ((setters != null) != shouldHaveSetters)
                throw new ArgumentOutOfRangeException();
            if (setters != null && (setters.Count < 3 || setters.Count > 4)) // optional angle setter
                throw new ArgumentOutOfRangeException();
        }

        public static PositionAngle Custom = new PositionAngle(PositionAngleTypeEnum.Custom);
        public static PositionAngle Custom2 = new PositionAngle(PositionAngleTypeEnum.Custom2);
        public static PositionAngle Mario = new PositionAngle(PositionAngleTypeEnum.Mario);
        public static PositionAngle Holp = new PositionAngle(PositionAngleTypeEnum.Holp);
        public static PositionAngle Selected = new PositionAngle(PositionAngleTypeEnum.Selected);
        public static PositionAngle Moneybag = new PositionAngle(PositionAngleTypeEnum.Moneybag);
        public static PositionAngle MoneybagHome = new PositionAngle(PositionAngleTypeEnum.MoneybagHome);
        public static PositionAngle Ghost = new PositionAngle(PositionAngleTypeEnum.Ghost);
        public static PositionAngle Camera = new PositionAngle(PositionAngleTypeEnum.Camera);
        public static PositionAngle CameraFocus = new PositionAngle(PositionAngleTypeEnum.CameraFocus);
        public static PositionAngle CamHackCamera = new PositionAngle(PositionAngleTypeEnum.CamHackCamera);
        public static PositionAngle CamHackFocus = new PositionAngle(PositionAngleTypeEnum.CamHackFocus);
        public static PositionAngle MapCamera = new PositionAngle(PositionAngleTypeEnum.MapCamera);
        public static PositionAngle MapFocus = new PositionAngle(PositionAngleTypeEnum.MapFocus);
        public static PositionAngle Scheduler = new PositionAngle(PositionAngleTypeEnum.Schedule);
        public static PositionAngle Self = new PositionAngle(PositionAngleTypeEnum.Self);
        public static PositionAngle Point = new PositionAngle(PositionAngleTypeEnum.Point);
        public static PositionAngle None = new PositionAngle(PositionAngleTypeEnum.None);

        public static PositionAngle Obj(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.Obj, address: address);
        public static PositionAngle ObjHome(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjHome, address: address);
        public static PositionAngle MarioObj() => Obj(Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress));
        public static PositionAngle ObjGfx(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjGfx, address: address);
        public static PositionAngle ObjScale(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjScale, address: address);
        public static PositionAngle GoombaProjection(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.GoombaProjection, address: address);
        public static PositionAngle Tri(uint address, int index) =>
            new PositionAngle(PositionAngleTypeEnum.Tri, address: address, index: index);
        public static PositionAngle ObjTri(uint address, int index, int index2) =>
            new PositionAngle(PositionAngleTypeEnum.ObjTri, address: address, index: index, index2: index2);
        public static PositionAngle Wall(int index) =>
            new PositionAngle(PositionAngleTypeEnum.Wall, index: index);
        public static PositionAngle Floor(int index) =>
            new PositionAngle(PositionAngleTypeEnum.Floor, index: index);
        public static PositionAngle Ceiling(int index) =>
            new PositionAngle(PositionAngleTypeEnum.Ceiling, index: index);
        public static PositionAngle Snow(int index) =>
            new PositionAngle(PositionAngleTypeEnum.Snow, index: index);
        public static PositionAngle QFrame(double frame) =>
            new PositionAngle(PositionAngleTypeEnum.QFrame, frame: frame);
        public static PositionAngle GFrame(double frame) =>
            new PositionAngle(PositionAngleTypeEnum.GFrame, frame: frame);
        public static PositionAngle Hybrid(PositionAngle posAngle1, PositionAngle posAngle2) =>
            new PositionAngle(PositionAngleTypeEnum.Hybrid, posAngle1: posAngle1, posAngle2: posAngle2);
        public static PositionAngle Trunc(PositionAngle posAngle) =>
            new PositionAngle(PositionAngleTypeEnum.Trunc, posAngle1: posAngle);
        public static PositionAngle Functions(List<Func<double>> getters, List<Func<double, bool>> setters) =>
            new PositionAngle(PositionAngleTypeEnum.Functions, getters: getters, setters: setters);
        public static PositionAngle Man(double x, double y, double z, double angle = double.NaN) =>
            new PositionAngle(PositionAngleTypeEnum.Man, manualX: x, manualY: y, manualZ: z, manualAngle: angle);

        public static PositionAngle FromString(string stringValue)
        {
            if (stringValue == null) return null;
            stringValue = stringValue.ToLower();
            List<string> parts = ParsingUtilities.ParseStringList(stringValue);

            if (parts.Count == 1 && parts[0] == "custom")
            {
                return Custom;
            }
            if (parts.Count == 1 && parts[0] == "custom2")
            {
                return Custom2;
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
            else if (parts.Count == 1 && (parts[0] == "mapcam" || parts[0] == "mapcamera"))
            {
                return MapCamera;
            }
            else if (parts.Count == 1 && parts[0] == "mapfocus")
            {
                return MapFocus;
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
            else if (parts.Count == 1 && parts[0] == "selected")
            {
                return Selected;
            }
            else if (parts.Count == 1 && parts[0] == "moneybag")
            {
                return Moneybag;
            }
            else if (parts.Count == 1 && parts[0] == "moneybaghome")
            {
                return MoneybagHome;
            }
            else if (parts.Count == 2 && parts[0] == "goombaprojection")
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return GoombaProjection(address.Value);
            }
            else if (parts.Count == 1 && parts[0] == "ghost")
            {
                return Ghost;
            }
            else if (parts.Count == 3 && (parts[0] == "tri" || parts[0] == "triangle"))
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                int? index = ParsingUtilities.ParseIntNullable(parts[2]);
                if (!index.HasValue || index.Value < 0 || index.Value > 4) return null;
                // 0 = closest vertex
                // 1 = vertex 1
                // 2 = vertex 2
                // 3 = vertex 3
                // 4 = point on triangle
                return Tri(address.Value, index.Value);
            }
            else if (parts.Count == 4 && parts[0] == "objtri")
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                int? index = ParsingUtilities.ParseIntNullable(parts[2]);
                if (!index.HasValue) return null;
                int? index2 = ParsingUtilities.ParseIntNullable(parts[3]);
                if (!index2.HasValue || index2.Value < 0 || index2.Value > 4) return null;
                return ObjTri(address.Value, index.Value, index2.Value);
            }
            else if (parts.Count == 2 && parts[0] == "wall")
            {
                int? index = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!index.HasValue || index.Value < 0 || index.Value > 4) return null;
                return Wall(index.Value);
            }
            else if (parts.Count == 2 && parts[0] == "floor")
            {
                int? index = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!index.HasValue || index.Value < 0 || index.Value > 4) return null;
                return Floor(index.Value);
            }
            else if (parts.Count == 2 && parts[0] == "ceiling")
            {
                int? index = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!index.HasValue || index.Value < 0 || index.Value > 4) return null;
                return Ceiling(index.Value);
            }
            else if (parts.Count == 2 && parts[0] == "snow")
            {
                int? index = ParsingUtilities.ParseIntNullable(parts[1]);
                if (!index.HasValue || index.Value < 0) return null;
                return Snow(index.Value);
            }
            else if (parts.Count == 2 && parts[0] == "qframe")
            {
                double? frame = ParsingUtilities.ParseDoubleNullable(parts[1]);
                if (!frame.HasValue) return null;
                return QFrame(frame.Value);
            }
            else if (parts.Count == 2 && parts[0] == "gframe")
            {
                double? frame = ParsingUtilities.ParseDoubleNullable(parts[1]);
                if (!frame.HasValue) return null;
                return GFrame(frame.Value);
            }
            else if (parts.Count >= 1 && parts[0] == "trunc")
            {
                PositionAngle posAngle = FromString(string.Join(" ", parts.Skip(1)));
                if (posAngle == null) return null;
                return Trunc(posAngle);
            }
            else if (parts.Count == 1 && parts[0] == "self")
            {
                return Self;
            }
            else if (parts.Count == 1 && parts[0] == "point")
            {
                return Point;
            }
            else if (parts.Count >= 1 && parts[0] == "man")
            {
                double x = parts.Count >= 2 ? ParsingUtilities.ParseDoubleNullable(parts[1]) ?? double.NaN : double.NaN;
                double y = parts.Count >= 3 ? ParsingUtilities.ParseDoubleNullable(parts[2]) ?? double.NaN : double.NaN;
                double z = parts.Count >= 4 ? ParsingUtilities.ParseDoubleNullable(parts[3]) ?? double.NaN : double.NaN;
                double angle = parts.Count >= 5 ? ParsingUtilities.ParseDoubleNullable(parts[4]) ?? double.NaN : double.NaN;
                return Man(x, y, z, angle);
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
            if (Index.HasValue) parts.Add(Index.Value);
            if (Index2.HasValue) parts.Add(Index2.Value);
            if (Frame.HasValue) parts.Add(Frame.Value);
            if (ManualX.HasValue) parts.Add(ManualX.Value);
            if (ManualY.HasValue) parts.Add(ManualY.Value);
            if (ManualZ.HasValue) parts.Add(ManualZ.Value);
            if (ManualAngle.HasValue) parts.Add(ManualAngle.Value);
            if (PosAngle1 != null) parts.Add("[" + PosAngle1 + "]");
            if (PosAngle2 != null) parts.Add("[" + PosAngle2 + "]");
            return string.Join(" ", parts);
        }

        public string GetMapName()
        {
            switch (PosAngleType)
            {
                case PositionAngleTypeEnum.Mario:
                    return "Mario";
                case PositionAngleTypeEnum.Holp:
                    return "HOLP";
                case PositionAngleTypeEnum.Camera:
                    return "Camera";
                case PositionAngleTypeEnum.Obj:
                    return GetMapNameForObject(Address.Value);
                case PositionAngleTypeEnum.ObjHome:
                    return "Home for " + GetMapNameForObject(Address.Value);
                case PositionAngleTypeEnum.Self:
                    return "Self";
                case PositionAngleTypeEnum.Point:
                    return "Point";
                default:
                    return ToString();
            }
        }

        public static string GetMapNameForObject(uint address)
        {
            ObjectDataModel obj = new ObjectDataModel(address, true);
            string objectName = Config.ObjectAssociations.GetObjectName(obj.BehaviorCriteria);
            string slotLabel = Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(address, true);
            return string.Format("[{0}] {1}", slotLabel, objectName);
        }

        public bool IsObject()
        {
            return PosAngleType == PositionAngleTypeEnum.Obj;
        }

        public bool IsObjectOrMario()
        {
            return PosAngleType == PositionAngleTypeEnum.Obj ||
                PosAngleType == PositionAngleTypeEnum.Mario;
        }

        public uint GetObjAddress()
        {
            switch (PosAngleType)
            {
                case PositionAngleTypeEnum.Obj:
                    return Address.Value;
                case PositionAngleTypeEnum.Mario:
                    return Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsSelfOrPoint()
        {
            return PosAngleType == PositionAngleTypeEnum.Self ||
                PosAngleType == PositionAngleTypeEnum.Point;
        }

        public bool DependsOnSelf()
        {
            if (PosAngleType == PositionAngleTypeEnum.Tri)
            {
                return Index == 0 || Index == 4;
            }
            return false;
        }

        public bool IsNone()
        {
            return PosAngleType == PositionAngleTypeEnum.None;
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
                    case PositionAngleTypeEnum.Custom2:
                        return SpecialConfig.Custom2X;
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
                    case PositionAngleTypeEnum.MapCamera:
                        return SpecialConfig.Map3DCameraX;
                    case PositionAngleTypeEnum.MapFocus:
                        return SpecialConfig.Map3DFocusX;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.XOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeXOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.GraphicsXOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ScaleWidthOffset);
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                    }
                    case PositionAngleTypeEnum.Moneybag:
                        return GetObjectValue("Moneybag", CoordinateAngle.X);
                    case PositionAngleTypeEnum.MoneybagHome:
                        return GetObjectValue("Moneybag", CoordinateAngle.X, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return GetGoombaProjection(Address.Value).x;
                    case PositionAngleTypeEnum.Ghost:
                        return GetObjectValue("Mario Ghost", CoordinateAngle.X, gfx: true);
                    case PositionAngleTypeEnum.Tri:
                        return GetTriangleVertexComponent(Address.Value, Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.ObjTri:
                        {
                            uint? triAddress = TriangleUtilities.GetTriangleAddressOfObjectTriangleIndex(Address.Value, Index.Value);
                            if (!triAddress.HasValue) return double.NaN;
                            return GetTriangleVertexComponent(triAddress.Value, Index2.Value, Coordinate.X);
                        }
                    case PositionAngleTypeEnum.Wall:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Snow:
                        return GetSnowComponent(Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.QFrame:
                        return GetQFrameComponent(Frame.Value, Coordinate.X);
                    case PositionAngleTypeEnum.GFrame:
                        return GetGFrameComponent(Frame.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item1;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle1.X;
                    case PositionAngleTypeEnum.Functions:
                        return Getters[0]();
                    case PositionAngleTypeEnum.Man:
                        return ManualX.Value;
                    case PositionAngleTypeEnum.Trunc:
                        return (int)PosAngle1.X;
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.X;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.X;
                    case PositionAngleTypeEnum.None:
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
                    case PositionAngleTypeEnum.Custom2:
                        return SpecialConfig.Custom2Y;
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
                    case PositionAngleTypeEnum.MapCamera:
                        return SpecialConfig.Map3DCameraY;
                    case PositionAngleTypeEnum.MapFocus:
                        return SpecialConfig.Map3DFocusY;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeYOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.GraphicsYOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ScaleHeightOffset);
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    }
                    case PositionAngleTypeEnum.Moneybag:
                        return GetObjectValue("Moneybag", CoordinateAngle.Y);
                    case PositionAngleTypeEnum.MoneybagHome:
                        return GetObjectValue("Moneybag", CoordinateAngle.Y, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.Ghost:
                        return GetObjectValue("Mario Ghost", CoordinateAngle.Y, gfx: true);
                    case PositionAngleTypeEnum.Tri:
                        return GetTriangleVertexComponent(Address.Value, Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.ObjTri:
                        {
                            uint? triAddress = TriangleUtilities.GetTriangleAddressOfObjectTriangleIndex(Address.Value, Index.Value);
                            if (!triAddress.HasValue) return double.NaN;
                            return GetTriangleVertexComponent(triAddress.Value, Index2.Value, Coordinate.Y);
                        }
                    case PositionAngleTypeEnum.Wall:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Snow:
                        return GetSnowComponent(Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.QFrame:
                        return GetQFrameComponent(Frame.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.GFrame:
                        return GetGFrameComponent(Frame.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item2;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle1.Y;
                    case PositionAngleTypeEnum.Functions:
                        return Getters[1]();
                    case PositionAngleTypeEnum.Man:
                        return ManualY.Value;
                    case PositionAngleTypeEnum.Trunc:
                        return (int)PosAngle1.Y;
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.Y;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.Y;
                    case PositionAngleTypeEnum.None:
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
                    case PositionAngleTypeEnum.Custom2:
                        return SpecialConfig.Custom2Z;
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
                    case PositionAngleTypeEnum.MapCamera:
                        return SpecialConfig.Map3DCameraZ;
                    case PositionAngleTypeEnum.MapFocus:
                        return SpecialConfig.Map3DFocusZ;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ZOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.HomeZOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.GraphicsZOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetSingle(Address.Value + ObjectConfig.ScaleDepthOffset);
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                    }
                    case PositionAngleTypeEnum.Moneybag:
                        return GetObjectValue("Moneybag", CoordinateAngle.Z);
                    case PositionAngleTypeEnum.MoneybagHome:
                        return GetObjectValue("Moneybag", CoordinateAngle.Z, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return GetGoombaProjection(Address.Value).z;
                    case PositionAngleTypeEnum.Ghost:
                        return GetObjectValue("Mario Ghost", CoordinateAngle.Z, gfx: true);
                    case PositionAngleTypeEnum.Tri:
                        return GetTriangleVertexComponent(Address.Value, Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.ObjTri:
                        {
                            uint? triAddress = TriangleUtilities.GetTriangleAddressOfObjectTriangleIndex(Address.Value, Index.Value);
                            if (!triAddress.HasValue) return double.NaN;
                            return GetTriangleVertexComponent(triAddress.Value, Index2.Value, Coordinate.Z);
                        }
                    case PositionAngleTypeEnum.Wall:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Snow:
                        return GetSnowComponent(Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.QFrame:
                        return GetQFrameComponent(Frame.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.GFrame:
                        return GetGFrameComponent(Frame.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item3;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle1.Z;
                    case PositionAngleTypeEnum.Functions:
                        return Getters[2]();
                    case PositionAngleTypeEnum.Man:
                        return ManualZ.Value;
                    case PositionAngleTypeEnum.Trunc:
                        return (int)PosAngle1.Z;
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.Z;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.Z;
                    case PositionAngleTypeEnum.None:
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
                    case PositionAngleTypeEnum.Custom2:
                        return SpecialConfig.Custom2Angle;
                    case PositionAngleTypeEnum.Mario:
                        return Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return CamHackUtilities.GetCamHackYawFacing();
                    case PositionAngleTypeEnum.CamHackFocus:
                        return CamHackUtilities.GetCamHackYawFacing();
                    case PositionAngleTypeEnum.MapCamera:
                        return SpecialConfig.Map3DCameraYaw;
                    case PositionAngleTypeEnum.MapFocus:
                        return SpecialConfig.Map3DCameraYaw;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetUInt16(Address.Value + ObjectConfig.YawFacingOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Double.NaN;
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetUInt16(Address.Value + ObjectConfig.GraphicsYawOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                    }
                    case PositionAngleTypeEnum.Moneybag:
                        return GetObjectValue("Moneybag", CoordinateAngle.Angle);
                    case PositionAngleTypeEnum.MoneybagHome:
                        return GetObjectValue("Moneybag", CoordinateAngle.Angle, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return MoreMath.NormalizeAngleUshort(Config.Stream.GetInt32(Address.Value + ObjectConfig.GoombaTargetAngleOffset));
                    case PositionAngleTypeEnum.Ghost:
                        return GetObjectValue("Mario Ghost", CoordinateAngle.Angle, gfx: true);
                    case PositionAngleTypeEnum.Tri:
                        return Double.NaN;
                    case PositionAngleTypeEnum.ObjTri:
                        return double.NaN;
                    case PositionAngleTypeEnum.Wall:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Floor:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Ceiling:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Snow:
                        return Double.NaN;
                    case PositionAngleTypeEnum.QFrame:
                        return Double.NaN;
                    case PositionAngleTypeEnum.GFrame:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Schedule:
                        uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        if (Schedule.ContainsKey(globalTimer)) return Schedule[globalTimer].Item4;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle2.Angle;
                    case PositionAngleTypeEnum.Functions:
                        if (Getters.Count >= 4) return Getters[3]();
                        return Double.NaN;
                    case PositionAngleTypeEnum.Man:
                        return ManualAngle.Value;
                    case PositionAngleTypeEnum.Trunc:
                        return MoreMath.NormalizeAngleTruncated(PosAngle1.Angle);
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.Angle;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.Angle;
                    case PositionAngleTypeEnum.None:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public (double x, double y, double z, double angle) GetValues()
        {
            return (X, Y, Z, Angle);
        }

        public double GetAdditionalValue(int index)
        {
            if (PosAngleType != PositionAngleTypeEnum.Schedule) return Double.NaN;
            uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
            if (!Schedule.ContainsKey(globalTimer)) return Double.NaN;
            List<double> doubleList = Schedule[globalTimer].Item5;
            if (index < 0 || index >= doubleList.Count) return Double.NaN;
            return doubleList[index];
        }

        private static double GetObjectValue(string name, CoordinateAngle coordAngle, bool home = false, bool gfx = false)
        {
            ObjectDataModel obj = Config.ObjectSlotsManager.GetLoadedObjectsWithName(name).LastOrDefault();
            uint? objAddress = obj?.Address;
            if (!objAddress.HasValue) return Double.NaN;
            switch (coordAngle)
            {
                case CoordinateAngle.X:
                    uint xOffset = home ? ObjectConfig.HomeXOffset : gfx ? ObjectConfig.GraphicsXOffset : ObjectConfig.XOffset;
                    return Config.Stream.GetSingle(objAddress.Value + xOffset);
                case CoordinateAngle.Y:
                    uint yOffset = home ? ObjectConfig.HomeYOffset : gfx ? ObjectConfig.GraphicsYOffset : ObjectConfig.YOffset;
                    return Config.Stream.GetSingle(objAddress.Value + yOffset);
                case CoordinateAngle.Z:
                    uint zOffset = home ? ObjectConfig.HomeZOffset : gfx ? ObjectConfig.GraphicsZOffset : ObjectConfig.ZOffset;
                    return Config.Stream.GetSingle(objAddress.Value + zOffset);
                case CoordinateAngle.Angle:
                    if (home) return Double.NaN;
                    if (gfx) return Config.Stream.GetUInt16(objAddress.Value + ObjectConfig.GraphicsYawOffset);
                    return Config.Stream.GetUInt16(objAddress.Value + ObjectConfig.YawFacingOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static (double x, double z) GetGoombaProjection(uint address)
        {
            double startX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
            double startZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
            double hSpeed = Config.Stream.GetSingle(address + ObjectConfig.HSpeedOffset);
            int countdown = Config.Stream.GetInt32(address + ObjectConfig.GoombaCountdownOffset);
            ushort targetAngle = MoreMath.NormalizeAngleUshort(Config.Stream.GetInt32(address + ObjectConfig.GoombaTargetAngleOffset));
            return MoreMath.AddVectorToPoint(hSpeed * countdown, targetAngle, startX, startZ);
        }

        private static double GetTriangleVertexComponent(uint address, int index, Coordinate coordinate)
        {
            if (address == 0) return Double.NaN;
            switch (index)
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

        private static double GetSnowComponent(int index, Coordinate coordinate)
        {
            short numSnowParticles = Config.Stream.GetInt16(SnowConfig.CounterAddress);
            if (index < 0 || index >= numSnowParticles) return Double.NaN;
            uint snowStart = Config.Stream.GetUInt32(SnowConfig.SnowArrayPointerAddress);
            uint structOffset = (uint)index * SnowConfig.ParticleStructSize;
            switch (coordinate)
            {
                case Coordinate.X:
                    return Config.Stream.GetInt32(snowStart + structOffset + SnowConfig.XOffset);
                case Coordinate.Y:
                    return Config.Stream.GetInt32(snowStart + structOffset + SnowConfig.YOffset);
                case Coordinate.Z:
                    return Config.Stream.GetInt32(snowStart + structOffset + SnowConfig.ZOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static double GetQFrameComponent(double frame, Coordinate coordinate)
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort angle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

            (double pointX, double pointZ) = MoreMath.AddVectorToPoint(hSpeed * frame, angle, marioX, marioZ);
            double pointY = marioY;

            switch (coordinate)
            {
                case Coordinate.X:
                    return pointX;
                case Coordinate.Y:
                    return pointY;
                case Coordinate.Z:
                    return pointZ;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static double GetGFrameComponent(double gFrame, Coordinate coordinate)
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort angle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);

            double frame = gFrame - globalTimer;
            (double pointX, double pointZ) = MoreMath.AddVectorToPoint(hSpeed * frame, angle, marioX, marioZ);
            double pointY = marioY;

            switch (coordinate)
            {
                case Coordinate.X:
                    return pointX;
                case Coordinate.Y:
                    return pointY;
                case Coordinate.Z:
                    return pointZ;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }




        public bool SetX(double value)
        {
            if (ShouldHaveAddress(PosAngleType) && Address == 0) return false;
            switch (PosAngleType)
            {
                case PositionAngleTypeEnum.Custom:
                    SpecialConfig.CustomX = value;
                    return true;
                case PositionAngleTypeEnum.Custom2:
                    SpecialConfig.Custom2X = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return SetMarioComponent((float)value, Coordinate.X);
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
                case PositionAngleTypeEnum.MapCamera:
                    SpecialConfig.Map3DCameraX = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    SpecialConfig.Map3DFocusX = (float)value;
                    return true;
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.XOffset);
                case PositionAngleTypeEnum.ObjHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeXOffset);
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.GraphicsXOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ScaleWidthOffset);
                case PositionAngleTypeEnum.Selected:
                {
                    List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                    if (objAddresses.Count == 0) return false;
                    uint objAddress = objAddresses[0];
                    return Config.Stream.SetValue((float)value, objAddress + ObjectConfig.XOffset);
                }
                case PositionAngleTypeEnum.Moneybag:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.X);
                case PositionAngleTypeEnum.MoneybagHome:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.X, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return SetObjectValue(value, "Mario Ghost", CoordinateAngle.X, gfx: true);
                case PositionAngleTypeEnum.Tri:
                    return SetTriangleVertexComponent((short)value, Address.Value, Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.ObjTri:
                    {
                        uint? triAddress = TriangleUtilities.GetTriangleAddressOfObjectTriangleIndex(Address.Value, Index.Value);
                        if (!triAddress.HasValue) return false;
                        return SetTriangleVertexComponent((short)value, triAddress.Value, Index2.Value, Coordinate.X);
                    }
                case PositionAngleTypeEnum.Wall:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.Snow:
                    return SetSnowComponent((int)value, Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle1.SetX(value);
                case PositionAngleTypeEnum.Functions:
                    return Setters[0](value);
                case PositionAngleTypeEnum.Man:
                    ManualX = value;
                    return true;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetX(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetX(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetX(value);
                case PositionAngleTypeEnum.None:
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
                case PositionAngleTypeEnum.Custom2:
                    SpecialConfig.Custom2Y = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return SetMarioComponent((float)value, Coordinate.Y);
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
                case PositionAngleTypeEnum.MapCamera:
                    SpecialConfig.Map3DCameraY = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    SpecialConfig.Map3DFocusY = (float)value;
                    return true;
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.YOffset);
                case PositionAngleTypeEnum.ObjHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeYOffset);
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.GraphicsYOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ScaleHeightOffset);
                case PositionAngleTypeEnum.Selected:
                {
                    List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                    if (objAddresses.Count == 0) return false;
                    uint objAddress = objAddresses[0];
                    return Config.Stream.SetValue((float)value, objAddress + ObjectConfig.YOffset);
                }
                case PositionAngleTypeEnum.Moneybag:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.Y);
                case PositionAngleTypeEnum.MoneybagHome:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.Y, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return SetObjectValue(value, "Mario Ghost", CoordinateAngle.Y, gfx: true);
                case PositionAngleTypeEnum.Tri:
                    return SetTriangleVertexComponent((short)value, Address.Value, Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.ObjTri:
                    {
                        uint? triAddress = TriangleUtilities.GetTriangleAddressOfObjectTriangleIndex(Address.Value, Index.Value);
                        if (!triAddress.HasValue) return false;
                        return SetTriangleVertexComponent((short)value, triAddress.Value, Index2.Value, Coordinate.Y);
                    }
                case PositionAngleTypeEnum.Wall:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Snow:
                    return SetSnowComponent((int)value, Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle1.SetY(value);
                case PositionAngleTypeEnum.Functions:
                    return Setters[1](value);
                case PositionAngleTypeEnum.Man:
                    ManualY = value;
                    return true;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetY(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetY(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetY(value);
                case PositionAngleTypeEnum.None:
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
                case PositionAngleTypeEnum.Custom2:
                    SpecialConfig.Custom2Z = value;
                    return true;
                case PositionAngleTypeEnum.Mario:
                    return SetMarioComponent((float)value, Coordinate.Z);
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
                case PositionAngleTypeEnum.MapCamera:
                    SpecialConfig.Map3DCameraZ = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    SpecialConfig.Map3DFocusZ = (float)value;
                    return true;
                case PositionAngleTypeEnum.Obj:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ZOffset);
                case PositionAngleTypeEnum.ObjHome:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.HomeZOffset);
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.GraphicsZOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return Config.Stream.SetValue((float)value, Address.Value + ObjectConfig.ScaleDepthOffset);
                case PositionAngleTypeEnum.Selected:
                {
                    List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                    if (objAddresses.Count == 0) return false;
                    uint objAddress = objAddresses[0];
                    return Config.Stream.SetValue((float)value, objAddress + ObjectConfig.ZOffset);
                }
                case PositionAngleTypeEnum.Moneybag:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.Z);
                case PositionAngleTypeEnum.MoneybagHome:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.Z, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return SetObjectValue(value, "Mario Ghost", CoordinateAngle.Z, gfx: true);
                case PositionAngleTypeEnum.Tri:
                    return SetTriangleVertexComponent((short)value, Address.Value, Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.ObjTri:
                    {
                        uint? triAddress = TriangleUtilities.GetTriangleAddressOfObjectTriangleIndex(Address.Value, Index.Value);
                        if (!triAddress.HasValue) return false;
                        return SetTriangleVertexComponent((short)value, triAddress.Value, Index2.Value, Coordinate.Z);
                    }
                case PositionAngleTypeEnum.Wall:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Snow:
                    return SetSnowComponent((int)value, Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle1.SetZ(value);
                case PositionAngleTypeEnum.Functions:
                    return Setters[2](value);
                case PositionAngleTypeEnum.Man:
                    ManualZ = value;
                    return true;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetZ(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetZ(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetZ(value);
                case PositionAngleTypeEnum.None:
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
                case PositionAngleTypeEnum.Custom2:
                    SpecialConfig.Custom2Angle = value;
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
                case PositionAngleTypeEnum.MapCamera:
                    SpecialConfig.Map3DCameraYaw = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    SpecialConfig.Map3DCameraYaw = (float)value;
                    return true;
                case PositionAngleTypeEnum.Obj:
                {
                    bool success = true;
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.YawMovingOffset);
                    return success;
                }
                case PositionAngleTypeEnum.ObjHome:
                    return false;
                case PositionAngleTypeEnum.ObjGfx:
                    return Config.Stream.SetValue(valueUShort, Address.Value + ObjectConfig.GraphicsYawOffset);
                case PositionAngleTypeEnum.ObjScale:
                    return false;
                case PositionAngleTypeEnum.Selected:
                {
                    List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                    if (objAddresses.Count == 0) return false;
                    uint objAddress = objAddresses[0];
                    bool success = true;
                    success &= Config.Stream.SetValue(valueUShort, objAddress + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(valueUShort, objAddress + ObjectConfig.YawMovingOffset);
                    return success;
                }
                case PositionAngleTypeEnum.Moneybag:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.Angle);
                case PositionAngleTypeEnum.MoneybagHome:
                    return SetObjectValue(value, "Moneybag", CoordinateAngle.Angle, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return SetObjectValue(value, "Mario Ghost", CoordinateAngle.Angle, gfx: true);
                case PositionAngleTypeEnum.Tri:
                    return false;
                case PositionAngleTypeEnum.ObjTri:
                    return false;
                case PositionAngleTypeEnum.Wall:
                    return false;
                case PositionAngleTypeEnum.Floor:
                    return false;
                case PositionAngleTypeEnum.Ceiling:
                    return false;
                case PositionAngleTypeEnum.Snow:
                    return false;
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle2.SetAngle(value);
                case PositionAngleTypeEnum.Functions:
                    if (Setters.Count >= 4) return Setters[3](value);
                    return false;
                case PositionAngleTypeEnum.Man:
                    ManualAngle = value;
                    return true;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetAngle(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetAngle(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetAngle(value);
                case PositionAngleTypeEnum.None:
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool SetMarioComponent(float value, Coordinate coordinate)
        {
            bool success = true;
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            switch (coordinate)
            {
                case Coordinate.X:
                    success &= Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.XOffset);
                    break;
                case Coordinate.Y:
                    success &= Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.YOffset);
                    break;
                case Coordinate.Z:
                    success &= Config.Stream.SetValue(value, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (KeyboardUtilities.IsAltHeld())
            {
                uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                switch (coordinate)
                {
                    case Coordinate.X:
                        success &= Config.Stream.SetValue(value, marioObjRef + ObjectConfig.GraphicsXOffset);
                        break;
                    case Coordinate.Y:
                        success &= Config.Stream.SetValue(value, marioObjRef + ObjectConfig.GraphicsYOffset);
                        break;
                    case Coordinate.Z:
                        success &= Config.Stream.SetValue(value, marioObjRef + ObjectConfig.GraphicsZOffset);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
            return success;
        }

        private static bool SetObjectValue(double value, string name, CoordinateAngle coordAngle, bool home = false, bool gfx = false)
        {
            ObjectDataModel obj = Config.ObjectSlotsManager.GetLoadedObjectsWithName(name).LastOrDefault();
            uint? objAddress = obj?.Address;
            if (!objAddress.HasValue) return false;
            switch (coordAngle)
            {
                case CoordinateAngle.X:
                    uint xOffset = home ? ObjectConfig.HomeXOffset : gfx ? ObjectConfig.GraphicsXOffset : ObjectConfig.XOffset;
                    return Config.Stream.SetValue((float)value, objAddress.Value + xOffset);
                case CoordinateAngle.Y:
                    uint yOffset = home ? ObjectConfig.HomeYOffset : gfx ? ObjectConfig.GraphicsYOffset : ObjectConfig.YOffset;
                    return Config.Stream.SetValue((float)value, objAddress.Value + yOffset);
                case CoordinateAngle.Z:
                    uint zOffset = home ? ObjectConfig.HomeZOffset : gfx ? ObjectConfig.GraphicsZOffset : ObjectConfig.ZOffset;
                    return Config.Stream.SetValue((float)value, objAddress.Value + zOffset);
                case CoordinateAngle.Angle:
                    if (home) return false;
                    if (gfx) return Config.Stream.SetValue(MoreMath.NormalizeAngleUshort(value), objAddress.Value + ObjectConfig.GraphicsYawOffset);
                    bool success = true;
                    success &= Config.Stream.SetValue(MoreMath.NormalizeAngleUshort(value), objAddress.Value + ObjectConfig.YawFacingOffset);
                    success &= Config.Stream.SetValue(MoreMath.NormalizeAngleUshort(value), objAddress.Value + ObjectConfig.YawMovingOffset);
                    return success;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static bool SetTriangleVertexComponent(short value, uint address, int index, Coordinate coordinate)
        {
            if (address == 0) return false;
            switch (index)
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

        private static bool SetSnowComponent(int value, int index, Coordinate coordinate)
        {
            short numSnowParticles = Config.Stream.GetInt16(SnowConfig.CounterAddress);
            if (index < 0 || index > numSnowParticles) return false;
            uint snowStart = Config.Stream.GetUInt32(SnowConfig.SnowArrayPointerAddress);
            uint structOffset = (uint)index * SnowConfig.ParticleStructSize;
            switch (coordinate)
            {
                case Coordinate.X:
                    return Config.Stream.SetValue(value, snowStart + structOffset + SnowConfig.XOffset);
                case Coordinate.Y:
                    return Config.Stream.SetValue(value, snowStart + structOffset + SnowConfig.YOffset);
                case Coordinate.Z:
                    return Config.Stream.SetValue(value, snowStart + structOffset + SnowConfig.ZOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                return p2.SetValues(x: x, z: z);
            }
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
