using STROOP.Map;
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
        private readonly string Text;
        private double? ThisX;
        private double? ThisY;
        private double? ThisZ;
        private double? ThisAngle;
        private double? OffsetDist;
        private double? OffsetAngle;
        private bool? OffsetAngleRelative;
        private readonly PositionAngle PosAngle1;
        private readonly PositionAngle PosAngle2;
        private readonly List<Func<double>> Getters;
        private readonly List<Func<double, bool>> Setters;

        public static Dictionary<uint, (double, double, double, double, List<double>)> Schedule =
            new Dictionary<uint, (double, double, double, double, List<double>)>();
        public static int ScheduleOffset = 0;

        private static uint GetScheduleIndex()
        {
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
            return ParsingUtilities.ParseUIntRoundingCapping(globalTimer + ScheduleOffset);
        }

        private enum PositionAngleTypeEnum
        {
            Custom,
            Custom2,
            Mario,
            Holp,
            Camera,
            CameraFocus,
            CameraGoal,
            CameraGoalFocus,
            CamHackCamera,
            CamHackFocus,
            MapCamera,
            MapFocus,
            Obj,
            ObjHome,
            ObjGfx,
            ObjScale,
            Selected,
            First,
            Last,
            FirstHome,
            LastHome,
            GoombaProjection,
            BullyPivot,
            KoopaTheQuick,
            PyramidNormal,
            PyramidNormalTarget,
            Ghost,
            Tri,
            ObjTri,
            Wall,
            Floor,
            Ceiling,
            Snow,
            QFrame,
            GFrame,
            MarioProjection,
            RolloutPeak,
            PreviousPositions,
            NextPositions,
            Schedule,
            Hybrid,
            Offset,
            YOffset,
            Trunc,
            Pos,
            Ang,
            Functions,
            Self,
            Point,
            Self2,
            Point2,
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
                PosAngleType == PositionAngleTypeEnum.BullyPivot ||
                PosAngleType == PositionAngleTypeEnum.PyramidNormal ||
                PosAngleType == PositionAngleTypeEnum.PyramidNormalTarget ||
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

        private bool ShouldHaveText(PositionAngleTypeEnum posAngleType)
        {
            return posAngleType == PositionAngleTypeEnum.First ||
                posAngleType == PositionAngleTypeEnum.Last ||
                posAngleType == PositionAngleTypeEnum.FirstHome ||
                posAngleType == PositionAngleTypeEnum.LastHome;
        }

        private PositionAngle(
            PositionAngleTypeEnum posAngleType,
            uint? address = null,
            int? index = null,
            int? index2 = null,
            double? frame = null,
            string text = null,
            double? thisX = null,
            double? thisY = null,
            double? thisZ = null,
            double? thisAngle = null,
            double? offsetDist = null,
            double? offsetAngle = null,
            bool? offsetAngleRelative = null,
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
            Text = text;
            ThisX = thisX;
            ThisY = thisY;
            ThisZ = thisZ;
            ThisAngle = thisAngle;
            OffsetDist = offsetDist;
            OffsetAngle = offsetAngle;
            OffsetAngleRelative = offsetAngleRelative;
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

            bool shouldHaveText = ShouldHaveText(posAngleType);
            if ((text != null) != shouldHaveText)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveThisX = PosAngleType == PositionAngleTypeEnum.Pos;
            if (thisX.HasValue != shouldHaveThisX)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveThisY = PosAngleType == PositionAngleTypeEnum.Pos;
            if (thisY.HasValue != shouldHaveThisY)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveThisZ = PosAngleType == PositionAngleTypeEnum.Pos;
            if (thisZ.HasValue != shouldHaveThisZ)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveThisAngle =
                PosAngleType == PositionAngleTypeEnum.Pos ||
                PosAngleType == PositionAngleTypeEnum.Ang;
            if (thisAngle.HasValue != shouldHaveThisAngle)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveOffsetDist =
                PosAngleType == PositionAngleTypeEnum.Offset ||
                PosAngleType == PositionAngleTypeEnum.YOffset;
            if (offsetDist.HasValue != shouldHaveOffsetDist)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveOffsetAngle = PosAngleType == PositionAngleTypeEnum.Offset;
            if (offsetAngle.HasValue != shouldHaveOffsetAngle)
                throw new ArgumentOutOfRangeException();

            bool shouldHaveOffsetAngleRelative = PosAngleType == PositionAngleTypeEnum.Offset;
            if (offsetAngleRelative.HasValue != shouldHaveOffsetAngleRelative)
                throw new ArgumentOutOfRangeException();

            bool shouldHavePosAngle1 =
                PosAngleType == PositionAngleTypeEnum.Hybrid ||
                PosAngleType == PositionAngleTypeEnum.Offset ||
                PosAngleType == PositionAngleTypeEnum.YOffset ||
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
        public static PositionAngle KoopaTheQuick = new PositionAngle(PositionAngleTypeEnum.KoopaTheQuick);
        public static PositionAngle Ghost = new PositionAngle(PositionAngleTypeEnum.Ghost);
        public static PositionAngle Camera = new PositionAngle(PositionAngleTypeEnum.Camera);
        public static PositionAngle CameraFocus = new PositionAngle(PositionAngleTypeEnum.CameraFocus);
        public static PositionAngle CameraGoal = new PositionAngle(PositionAngleTypeEnum.CameraGoal);
        public static PositionAngle CameraGoalFocus = new PositionAngle(PositionAngleTypeEnum.CameraGoalFocus);
        public static PositionAngle CamHackCamera = new PositionAngle(PositionAngleTypeEnum.CamHackCamera);
        public static PositionAngle CamHackFocus = new PositionAngle(PositionAngleTypeEnum.CamHackFocus);
        public static PositionAngle MapCamera = new PositionAngle(PositionAngleTypeEnum.MapCamera);
        public static PositionAngle MapFocus = new PositionAngle(PositionAngleTypeEnum.MapFocus);
        public static PositionAngle MarioProjection = new PositionAngle(PositionAngleTypeEnum.MarioProjection);
        public static PositionAngle RolloutPeak = new PositionAngle(PositionAngleTypeEnum.RolloutPeak);
        public static PositionAngle PreviousPositions = new PositionAngle(PositionAngleTypeEnum.PreviousPositions);
        public static PositionAngle NextPositions = new PositionAngle(PositionAngleTypeEnum.NextPositions);
        public static PositionAngle Scheduler = new PositionAngle(PositionAngleTypeEnum.Schedule);
        public static PositionAngle Self = new PositionAngle(PositionAngleTypeEnum.Self);
        public static PositionAngle Point = new PositionAngle(PositionAngleTypeEnum.Point);
        public static PositionAngle Self2 = new PositionAngle(PositionAngleTypeEnum.Self2);
        public static PositionAngle Point2 = new PositionAngle(PositionAngleTypeEnum.Point2);
        public static PositionAngle None = new PositionAngle(PositionAngleTypeEnum.None);

        public static PositionAngle Obj(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.Obj, address: address);
        public static PositionAngle ObjHome(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjHome, address: address);
        public static PositionAngle MarioObj() => Obj(Config.Stream.GetUInt(MarioObjectConfig.PointerAddress));
        public static PositionAngle ObjGfx(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjGfx, address: address);
        public static PositionAngle ObjScale(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.ObjScale, address: address);
        public static PositionAngle First(string text) =>
            new PositionAngle(PositionAngleTypeEnum.First, text: text);
        public static PositionAngle Last(string text) =>
            new PositionAngle(PositionAngleTypeEnum.Last, text: text);
        public static PositionAngle FirstHome(string text) =>
            new PositionAngle(PositionAngleTypeEnum.FirstHome, text: text);
        public static PositionAngle LastHome(string text) =>
            new PositionAngle(PositionAngleTypeEnum.LastHome, text: text);
        public static PositionAngle GoombaProjection(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.GoombaProjection, address: address);
        public static PositionAngle BullyPivot(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.BullyPivot, address: address);
        public static PositionAngle PyramidNormal(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.PyramidNormal, address: address);
        public static PositionAngle PyramidNormalTarget(uint address) =>
            new PositionAngle(PositionAngleTypeEnum.PyramidNormalTarget, address: address);
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
        public static PositionAngle Offset(double dist, double angle, bool relative, PositionAngle posAngle) =>
            new PositionAngle(PositionAngleTypeEnum.Offset, offsetDist: dist, offsetAngle: angle, offsetAngleRelative: relative, posAngle1: posAngle);
        public static PositionAngle YOffset(double dist, PositionAngle posAngle) =>
            new PositionAngle(PositionAngleTypeEnum.YOffset, offsetDist: dist, posAngle1: posAngle);
        public static PositionAngle Trunc(PositionAngle posAngle) =>
            new PositionAngle(PositionAngleTypeEnum.Trunc, posAngle1: posAngle);
        public static PositionAngle Functions(List<Func<double>> getters, List<Func<double, bool>> setters) =>
            new PositionAngle(PositionAngleTypeEnum.Functions, getters: getters, setters: setters);
        public static PositionAngle Pos(double x, double y, double z, double angle = double.NaN) =>
            new PositionAngle(PositionAngleTypeEnum.Pos, thisX: x, thisY: y, thisZ: z, thisAngle: angle);
        public static PositionAngle Ang(double angle) =>
            new PositionAngle(PositionAngleTypeEnum.Ang, thisAngle: angle);

        public static PositionAngle FromString(string stringValue)
        {
            if (stringValue == null || stringValue == "") return null;
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
            else if (parts.Count == 1 && (parts[0] == "camgoal" || parts[0] == "cameragoal"))
            {
                return CameraGoal;
            }
            else if (parts.Count == 1 && (parts[0] == "camgoalfocus" || parts[0] == "cameragoalfocus"))
            {
                return CameraGoalFocus;
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
            else if (parts.Count >= 2 && parts[0] == "first")
            {
                return First(string.Join(" ", parts.Skip(1)));
            }
            else if (parts.Count >= 2 && parts[0] == "last")
            {
                return Last(string.Join(" ", parts.Skip(1)));
            }
            else if (parts.Count >= 2 && parts[0] == "firsthome")
            {
                return FirstHome(string.Join(" ", parts.Skip(1)));
            }
            else if (parts.Count >= 2 && parts[0] == "lasthome")
            {
                return LastHome(string.Join(" ", parts.Skip(1)));
            }
            else if (parts.Count == 2 && parts[0] == "goombaprojection")
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return GoombaProjection(address.Value);
            }
            else if (parts.Count == 2 && parts[0] == "bullypivot")
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return BullyPivot(address.Value);
            }
            else if (parts.Count == 2 && parts[0] == "pyramidnormal")
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return PyramidNormal(address.Value);
            }
            else if (parts.Count == 2 && parts[0] == "pyramidnormaltarget")
            {
                uint? address = ParsingUtilities.ParseHexNullable(parts[1]);
                if (!address.HasValue) return null;
                return PyramidNormalTarget(address.Value);
            }
            else if (parts.Count == 1 && parts[0] == "koopathequick")
            {
                return KoopaTheQuick;
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
                if (!index.HasValue || index.Value < 1 || index.Value > 7) return null;
                // 1 = vertex 1
                // 2 = vertex 2
                // 3 = vertex 3
                // 4 = vertex closest to Mario
                // 5 = vertex closest to Self
                // 6 = point on triangle under Mario
                // 7 = point on triangle under Self
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
            else if (parts.Count == 1 && parts[0] == "marioprojection")
            {
                return MarioProjection;
            }
            else if (parts.Count == 1 && parts[0] == "rolloutpeak")
            {
                return RolloutPeak;
            }
            else if (parts.Count == 1 && parts[0] == "previouspositions")
            {
                return PreviousPositions;
            }
            else if (parts.Count == 1 && parts[0] == "nextpositions")
            {
                return NextPositions;
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
            else if (parts.Count == 1 && parts[0] == "self2")
            {
                return Self2;
            }
            else if (parts.Count == 1 && parts[0] == "point2")
            {
                return Point2;
            }
            else if (parts.Count >= 1 && (parts[0] == "pos" || parts[0] == "position"))
            {
                double x = parts.Count >= 2 ? ParsingUtilities.ParseDoubleNullable(parts[1]) ?? double.NaN : double.NaN;
                double y = parts.Count >= 3 ? ParsingUtilities.ParseDoubleNullable(parts[2]) ?? double.NaN : double.NaN;
                double z = parts.Count >= 4 ? ParsingUtilities.ParseDoubleNullable(parts[3]) ?? double.NaN : double.NaN;
                double angle = parts.Count >= 5 ? ParsingUtilities.ParseDoubleNullable(parts[4]) ?? double.NaN : double.NaN;
                return Pos(x, y, z, angle);
            }
            else if (parts.Count == 2 && (parts[0] == "ang" || parts[0] == "angle"))
            {
                double angle = ParsingUtilities.ParseDoubleNullable(parts[1]) ?? double.NaN;
                return Ang(angle);
            }
            else if (parts[0] == "offset")
            {
                double dist = ParsingUtilities.ParseDouble(parts[1]);
                double angle = ParsingUtilities.ParseDouble(parts[2]);
                bool relative = ParsingUtilities.ParseBool(parts[3]);
                int indexStart = stringValue.IndexOf("[");
                int indexEnd = stringValue.LastIndexOf("]");
                string substring = stringValue.Substring(indexStart + 1, indexEnd - indexStart - 1);
                PositionAngle posAngle = FromString(substring);
                return Offset(dist, angle, relative, posAngle);
            }
            else if (parts[0] == "yoffset")
            {
                double dist = ParsingUtilities.ParseDouble(parts[1]);
                int indexStart = stringValue.IndexOf("[");
                int indexEnd = stringValue.LastIndexOf("]");
                string substring = stringValue.Substring(indexStart + 1, indexEnd - indexStart - 1);
                PositionAngle posAngle = FromString(substring);
                return YOffset(dist, posAngle);
            }
            else if (parts.Count == 1 && parts[0] == "schedule")
            {
                return Scheduler;
            }
            else if (parts.Count == 1 && parts[0] == "none")
            {
                return None;
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
            if (Text != null) parts.Add(Text);
            if (ThisX.HasValue) parts.Add(ThisX.Value);
            if (ThisY.HasValue) parts.Add(ThisY.Value);
            if (ThisZ.HasValue) parts.Add(ThisZ.Value);
            if (ThisAngle.HasValue) parts.Add(ThisAngle.Value);
            if (OffsetDist.HasValue) parts.Add(OffsetDist.Value);
            if (OffsetAngle.HasValue) parts.Add(OffsetAngle.Value);
            if (OffsetAngleRelative.HasValue) parts.Add(OffsetAngleRelative.Value);
            if (PosAngle1 != null) parts.Add("[" + PosAngle1 + "]");
            if (PosAngle2 != null) parts.Add("[" + PosAngle2 + "]");
            return string.Join(" ", parts);
        }

        public string GetMapName()
        {
            switch (PosAngleType)
            {
                case PositionAngleTypeEnum.Obj:
                    return GetMapNameForObject(Address.Value);
                case PositionAngleTypeEnum.ObjHome:
                    return "Home for " + GetMapNameForObject(Address.Value);
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

        public bool IsObjectDependent()
        {
            return PosAngleType == PositionAngleTypeEnum.Obj ||
                PosAngleType == PositionAngleTypeEnum.ObjHome ||
                PosAngleType == PositionAngleTypeEnum.ObjGfx ||
                PosAngleType == PositionAngleTypeEnum.ObjScale ||
                PosAngleType == PositionAngleTypeEnum.GoombaProjection ||
                PosAngleType == PositionAngleTypeEnum.BullyPivot ||
                PosAngleType == PositionAngleTypeEnum.ObjTri;
        }

        public bool IsObjectOrMario()
        {
            return PosAngleType == PositionAngleTypeEnum.Obj ||
                PosAngleType == PositionAngleTypeEnum.Mario;
        }

        public uint GetObjAddress()
        {
            if (IsObjectDependent())
            {
                return Address.Value;
            }
            if (PosAngleType == PositionAngleTypeEnum.Mario)
            {
                return Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
            }
            throw new ArgumentOutOfRangeException();
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
                return Index == 5 || Index == 7;
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
                        return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HolpXOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.XOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.FocusXOffset);
                    case PositionAngleTypeEnum.CameraGoal:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.GoalXOffset);
                    case PositionAngleTypeEnum.CameraGoalFocus:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.GoalFocusXOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    case PositionAngleTypeEnum.CamHackFocus:
                        return Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    case PositionAngleTypeEnum.MapCamera:
                        return MapConfig.Map3DCameraX;
                    case PositionAngleTypeEnum.MapFocus:
                        return MapConfig.Map3DFocusX;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.XOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.HomeXOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.GraphicsXOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.ScaleWidthOffset);
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
                    }
                    case PositionAngleTypeEnum.First:
                        return GetObjectValue(Text, true, CoordinateAngle.X);
                    case PositionAngleTypeEnum.Last:
                        return GetObjectValue(Text, false, CoordinateAngle.X);
                    case PositionAngleTypeEnum.FirstHome:
                        return GetObjectValue(Text, true, CoordinateAngle.X, home: true);
                    case PositionAngleTypeEnum.LastHome:
                        return GetObjectValue(Text, false, CoordinateAngle.X, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return GetGoombaProjection(Address.Value).x;
                    case PositionAngleTypeEnum.BullyPivot:
                        return GetBullyPivot(Address.Value, Coordinate.X);
                    case PositionAngleTypeEnum.PyramidNormal:
                        return GetPyramidNormal(Address.Value, Coordinate.X);
                    case PositionAngleTypeEnum.PyramidNormalTarget:
                        return GetPyramidNormalTarget(Address.Value, Coordinate.X);
                    case PositionAngleTypeEnum.KoopaTheQuick:
                        return PlushUtilities.GetX();
                    case PositionAngleTypeEnum.Ghost:
                        return Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.XOffset);
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
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.Snow:
                        return GetSnowComponent(Index.Value, Coordinate.X);
                    case PositionAngleTypeEnum.QFrame:
                        return GetQFrameComponent(Frame.Value, Coordinate.X);
                    case PositionAngleTypeEnum.GFrame:
                        return GetGFrameComponent(Frame.Value, Coordinate.X);
                    case PositionAngleTypeEnum.MarioProjection:
                        return GetMarioProjection(Coordinate.X);
                    case PositionAngleTypeEnum.RolloutPeak:
                        return GetRolloutPeakComponent(CoordinateAngle.X);
                    case PositionAngleTypeEnum.PreviousPositions:
                        return GetPreviousPositionsComponent(Coordinate.X);
                    case PositionAngleTypeEnum.NextPositions:
                        return GetNextPositionsComponent(Coordinate.X);
                    case PositionAngleTypeEnum.Schedule:
                        uint scheduleIndex = GetScheduleIndex();
                        if (Schedule.ContainsKey(scheduleIndex)) return Schedule[scheduleIndex].Item1;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle1.X;
                    case PositionAngleTypeEnum.Functions:
                        return Getters[0]();
                    case PositionAngleTypeEnum.Pos:
                        return ThisX.Value;
                    case PositionAngleTypeEnum.Ang:
                        return double.NaN;
                    case PositionAngleTypeEnum.Offset:
                        return GetOffset(Coordinate.X);
                    case PositionAngleTypeEnum.YOffset:
                        return PosAngle1.X;
                    case PositionAngleTypeEnum.Trunc:
                        return (int)PosAngle1.X;
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.X;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.X;
                    case PositionAngleTypeEnum.Self2:
                        return SpecialConfig.Self2PA.X;
                    case PositionAngleTypeEnum.Point2:
                        return SpecialConfig.Point2PA.X;
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
                        return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HolpYOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.YOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.FocusYOffset);
                    case PositionAngleTypeEnum.CameraGoal:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.GoalYOffset);
                    case PositionAngleTypeEnum.CameraGoalFocus:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.GoalFocusYOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    case PositionAngleTypeEnum.CamHackFocus:
                        return Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                    case PositionAngleTypeEnum.MapCamera:
                        return MapConfig.Map3DCameraY;
                    case PositionAngleTypeEnum.MapFocus:
                        return MapConfig.Map3DFocusY;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.HomeYOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.GraphicsYOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.ScaleHeightOffset);
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
                    }
                    case PositionAngleTypeEnum.First:
                        return GetObjectValue(Text, true, CoordinateAngle.Y);
                    case PositionAngleTypeEnum.Last:
                        return GetObjectValue(Text, false, CoordinateAngle.Y);
                    case PositionAngleTypeEnum.FirstHome:
                        return GetObjectValue(Text, true, CoordinateAngle.Y, home: true);
                    case PositionAngleTypeEnum.LastHome:
                        return GetObjectValue(Text, false, CoordinateAngle.Y, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.BullyPivot:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.YOffset);
                    case PositionAngleTypeEnum.PyramidNormal:
                        return GetPyramidNormal(Address.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.PyramidNormalTarget:
                        return GetPyramidNormalTarget(Address.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.KoopaTheQuick:
                        return PlushUtilities.GetY();
                    case PositionAngleTypeEnum.Ghost:
                        return Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YOffset);
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
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.Snow:
                        return GetSnowComponent(Index.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.QFrame:
                        return GetQFrameComponent(Frame.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.GFrame:
                        return GetGFrameComponent(Frame.Value, Coordinate.Y);
                    case PositionAngleTypeEnum.MarioProjection:
                        return GetMarioProjection(Coordinate.Y);
                    case PositionAngleTypeEnum.RolloutPeak:
                        return GetRolloutPeakComponent(CoordinateAngle.Y);
                    case PositionAngleTypeEnum.PreviousPositions:
                        return GetPreviousPositionsComponent(Coordinate.Y);
                    case PositionAngleTypeEnum.NextPositions:
                        return GetNextPositionsComponent(Coordinate.Y);
                    case PositionAngleTypeEnum.Schedule:
                        uint scheduleIndex = GetScheduleIndex();
                        if (Schedule.ContainsKey(scheduleIndex)) return Schedule[scheduleIndex].Item2;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle1.Y;
                    case PositionAngleTypeEnum.Functions:
                        return Getters[1]();
                    case PositionAngleTypeEnum.Pos:
                        return ThisY.Value;
                    case PositionAngleTypeEnum.Ang:
                        return double.NaN;
                    case PositionAngleTypeEnum.Offset:
                        return GetOffset(Coordinate.Y);
                    case PositionAngleTypeEnum.YOffset:
                        return PosAngle1.Y + OffsetDist.Value;
                    case PositionAngleTypeEnum.Trunc:
                        return (int)PosAngle1.Y;
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.Y;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.Y;
                    case PositionAngleTypeEnum.Self2:
                        return SpecialConfig.Self2PA.Y;
                    case PositionAngleTypeEnum.Point2:
                        return SpecialConfig.Point2PA.Y;
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
                        return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HolpZOffset);
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.ZOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.FocusZOffset);
                    case PositionAngleTypeEnum.CameraGoal:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.GoalZOffset);
                    case PositionAngleTypeEnum.CameraGoalFocus:
                        return Config.Stream.GetFloat(CameraConfig.StructAddress + CameraConfig.GoalFocusZOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    case PositionAngleTypeEnum.CamHackFocus:
                        return Config.Stream.GetFloat(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    case PositionAngleTypeEnum.MapCamera:
                        return MapConfig.Map3DCameraZ;
                    case PositionAngleTypeEnum.MapFocus:
                        return MapConfig.Map3DFocusZ;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.ZOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.HomeZOffset);
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.GraphicsZOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Config.Stream.GetFloat(Address.Value + ObjectConfig.ScaleDepthOffset);
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);
                    }
                    case PositionAngleTypeEnum.First:
                        return GetObjectValue(Text, true, CoordinateAngle.Z);
                    case PositionAngleTypeEnum.Last:
                        return GetObjectValue(Text, false, CoordinateAngle.Z);
                    case PositionAngleTypeEnum.FirstHome:
                        return GetObjectValue(Text, true, CoordinateAngle.Z, home: true);
                    case PositionAngleTypeEnum.LastHome:
                        return GetObjectValue(Text, false, CoordinateAngle.Z, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return GetGoombaProjection(Address.Value).z;
                    case PositionAngleTypeEnum.BullyPivot:
                        return GetBullyPivot(Address.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.PyramidNormal:
                        return GetPyramidNormal(Address.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.PyramidNormalTarget:
                        return GetPyramidNormalTarget(Address.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.KoopaTheQuick:
                        return PlushUtilities.GetZ();
                    case PositionAngleTypeEnum.Ghost:
                        return Config.Stream.GetFloat(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.ZOffset);
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
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Floor:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Ceiling:
                        return GetTriangleVertexComponent(
                            Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.Snow:
                        return GetSnowComponent(Index.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.QFrame:
                        return GetQFrameComponent(Frame.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.GFrame:
                        return GetGFrameComponent(Frame.Value, Coordinate.Z);
                    case PositionAngleTypeEnum.MarioProjection:
                        return GetMarioProjection(Coordinate.Z);
                    case PositionAngleTypeEnum.RolloutPeak:
                        return GetRolloutPeakComponent(CoordinateAngle.Z);
                    case PositionAngleTypeEnum.PreviousPositions:
                        return GetPreviousPositionsComponent(Coordinate.Z);
                    case PositionAngleTypeEnum.NextPositions:
                        return GetNextPositionsComponent(Coordinate.Z);
                    case PositionAngleTypeEnum.Schedule:
                        uint scheduleIndex = GetScheduleIndex();
                        if (Schedule.ContainsKey(scheduleIndex)) return Schedule[scheduleIndex].Item3;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle1.Z;
                    case PositionAngleTypeEnum.Functions:
                        return Getters[2]();
                    case PositionAngleTypeEnum.Pos:
                        return ThisZ.Value;
                    case PositionAngleTypeEnum.Ang:
                        return double.NaN;
                    case PositionAngleTypeEnum.Offset:
                        return GetOffset(Coordinate.Z);
                    case PositionAngleTypeEnum.YOffset:
                        return PosAngle1.Z;
                    case PositionAngleTypeEnum.Trunc:
                        return (int)PosAngle1.Z;
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.Z;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.Z;
                    case PositionAngleTypeEnum.Self2:
                        return SpecialConfig.Self2PA.Z;
                    case PositionAngleTypeEnum.Point2:
                        return SpecialConfig.Point2PA.Z;
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
                        return Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.Holp:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Camera:
                        return Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CameraFocus:
                        return Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CameraGoal:
                        return Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CameraGoalFocus:
                        return Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.CamHackCamera:
                        return CamHackUtilities.GetCamHackYawFacing();
                    case PositionAngleTypeEnum.CamHackFocus:
                        return CamHackUtilities.GetCamHackYawFacing();
                    case PositionAngleTypeEnum.MapCamera:
                        return MapConfig.Map3DCameraYaw;
                    case PositionAngleTypeEnum.MapFocus:
                        return MapConfig.Map3DCameraYaw;
                    case PositionAngleTypeEnum.Obj:
                        return Config.Stream.GetUShort(Address.Value + ObjectConfig.YawFacingOffset);
                    case PositionAngleTypeEnum.ObjHome:
                        return Double.NaN;
                    case PositionAngleTypeEnum.ObjGfx:
                        return Config.Stream.GetUShort(Address.Value + ObjectConfig.GraphicsYawOffset);
                    case PositionAngleTypeEnum.ObjScale:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Selected:
                    {
                        List<uint> objAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
                        if (objAddresses.Count == 0) return Double.NaN;
                        uint objAddress = objAddresses[0];
                        return Config.Stream.GetUShort(objAddress + ObjectConfig.YawFacingOffset);
                    }
                    case PositionAngleTypeEnum.First:
                        return GetObjectValue(Text, true, CoordinateAngle.Angle);
                    case PositionAngleTypeEnum.Last:
                        return GetObjectValue(Text, false, CoordinateAngle.Angle);
                    case PositionAngleTypeEnum.FirstHome:
                        return GetObjectValue(Text, true, CoordinateAngle.Angle, home: true);
                    case PositionAngleTypeEnum.LastHome:
                        return GetObjectValue(Text, false, CoordinateAngle.Angle, home: true);
                    case PositionAngleTypeEnum.GoombaProjection:
                        return MoreMath.NormalizeAngleUshort(Config.Stream.GetInt(Address.Value + ObjectConfig.GoombaTargetAngleOffset));
                    case PositionAngleTypeEnum.BullyPivot:
                        return double.NaN;
                    case PositionAngleTypeEnum.PyramidNormal:
                        return double.NaN;
                    case PositionAngleTypeEnum.PyramidNormalTarget:
                        return double.NaN;
                    case PositionAngleTypeEnum.KoopaTheQuick:
                        return PlushUtilities.GetAngle();
                    case PositionAngleTypeEnum.Ghost:
                        return Config.Stream.GetUShort(GhostHackConfig.CurrentGhostStruct + GhostHackConfig.YawFacingOffset);
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
                    case PositionAngleTypeEnum.MarioProjection:
                        return Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    case PositionAngleTypeEnum.RolloutPeak:
                        return GetRolloutPeakComponent(CoordinateAngle.Angle);
                    case PositionAngleTypeEnum.PreviousPositions:
                        return Double.NaN;
                    case PositionAngleTypeEnum.NextPositions:
                        return Double.NaN;
                    case PositionAngleTypeEnum.Schedule:
                        uint scheduleIndex = GetScheduleIndex();
                        if (Schedule.ContainsKey(scheduleIndex)) return Schedule[scheduleIndex].Item4;
                        return Double.NaN;
                    case PositionAngleTypeEnum.Hybrid:
                        return PosAngle2.Angle;
                    case PositionAngleTypeEnum.Functions:
                        if (Getters.Count >= 4) return Getters[3]();
                        return Double.NaN;
                    case PositionAngleTypeEnum.Pos:
                        return ThisAngle.Value;
                    case PositionAngleTypeEnum.Ang:
                        return ThisAngle.Value;
                    case PositionAngleTypeEnum.Offset:
                        return 0;
                    case PositionAngleTypeEnum.YOffset:
                        return PosAngle1.Angle;
                    case PositionAngleTypeEnum.Trunc:
                        return MoreMath.NormalizeAngleTruncated(PosAngle1.Angle);
                    case PositionAngleTypeEnum.Self:
                        return SpecialConfig.SelfPA.Angle;
                    case PositionAngleTypeEnum.Point:
                        return SpecialConfig.PointPA.Angle;
                    case PositionAngleTypeEnum.Self2:
                        return SpecialConfig.Self2PA.Angle;
                    case PositionAngleTypeEnum.Point2:
                        return SpecialConfig.Point2PA.Angle;
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
            uint scheduleIndex = GetScheduleIndex();
            if (!Schedule.ContainsKey(scheduleIndex)) return Double.NaN;
            List<double> doubleList = Schedule[scheduleIndex].Item5;
            if (index < 0 || index >= doubleList.Count) return Double.NaN;
            return doubleList[index];
        }

        private static double GetObjectValue(string name, bool first, CoordinateAngle coordAngle, bool home = false, bool gfx = false)
        {
            List<ObjectDataModel> objs = Config.ObjectSlotsManager.GetLoadedObjectsWithName(name);
            ObjectDataModel obj = first ? objs.FirstOrDefault() : objs.LastOrDefault();
            uint? objAddress = obj?.Address;
            if (!objAddress.HasValue) return Double.NaN;
            switch (coordAngle)
            {
                case CoordinateAngle.X:
                    uint xOffset = home ? ObjectConfig.HomeXOffset : gfx ? ObjectConfig.GraphicsXOffset : ObjectConfig.XOffset;
                    return Config.Stream.GetFloat(objAddress.Value + xOffset);
                case CoordinateAngle.Y:
                    uint yOffset = home ? ObjectConfig.HomeYOffset : gfx ? ObjectConfig.GraphicsYOffset : ObjectConfig.YOffset;
                    return Config.Stream.GetFloat(objAddress.Value + yOffset);
                case CoordinateAngle.Z:
                    uint zOffset = home ? ObjectConfig.HomeZOffset : gfx ? ObjectConfig.GraphicsZOffset : ObjectConfig.ZOffset;
                    return Config.Stream.GetFloat(objAddress.Value + zOffset);
                case CoordinateAngle.Angle:
                    if (home) return Double.NaN;
                    if (gfx) return Config.Stream.GetUShort(objAddress.Value + ObjectConfig.GraphicsYawOffset);
                    return Config.Stream.GetUShort(objAddress.Value + ObjectConfig.YawFacingOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static (double x, double z) GetGoombaProjection(uint address)
        {
            double startX = Config.Stream.GetFloat(address + ObjectConfig.XOffset);
            double startZ = Config.Stream.GetFloat(address + ObjectConfig.ZOffset);
            double hSpeed = Config.Stream.GetFloat(address + ObjectConfig.HSpeedOffset);
            int countdown = Config.Stream.GetInt(address + ObjectConfig.GoombaCountdownOffset);
            ushort targetAngle = MoreMath.NormalizeAngleUshort(Config.Stream.GetInt(address + ObjectConfig.GoombaTargetAngleOffset));
            return MoreMath.AddVectorToPoint(hSpeed * countdown, targetAngle, startX, startZ);
        }

        public static double GetBullyPivot(uint address, Coordinate coord)
        {
            float x = Config.Stream.GetFloat(address + ObjectConfig.XOffset);
            float z = Config.Stream.GetFloat(address + ObjectConfig.ZOffset);
            ushort yaw = Config.Stream.GetUShort(address + ObjectConfig.YawMovingOffset);
            float hSpeed = Config.Stream.GetFloat(address + ObjectConfig.HSpeedOffset);

            if (yaw % 2 == 0)
            {
                return coord == Coordinate.X ? x : z;
            }
            else
            {
                ushort truncated = MoreMath.NormalizeAngleTruncated(yaw + 32768);
                (double nextX, double nextZ) = MoreMath.AddVectorToPoint(hSpeed, truncated, x, z);
                return coord == Coordinate.X ? nextX : nextZ;
            }
        }

        public static double GetPyramidNormal(uint address, Coordinate coord)
        {
            float posX = Config.Stream.GetFloat(address + ObjectConfig.XOffset);
            float posY = Config.Stream.GetFloat(address + ObjectConfig.YOffset);
            float posZ = Config.Stream.GetFloat(address + ObjectConfig.ZOffset);

            float normalX = Config.Stream.GetFloat(address + ObjectConfig.PyramidPlatformNormalXOffset);
            float normalY = Config.Stream.GetFloat(address + ObjectConfig.PyramidPlatformNormalYOffset);
            float normalZ = Config.Stream.GetFloat(address + ObjectConfig.PyramidPlatformNormalZOffset);

            if (coord == Coordinate.X)
            {
                return posX + 500 * normalX;
            }
            if (coord == Coordinate.Y)
            {
                return posY + 500 * normalY;
            }
            if (coord == Coordinate.Z)
            {
                return posZ + 500 * normalZ;
            }

            throw new ArgumentOutOfRangeException(coord.ToString());
        }

        public static double GetPyramidNormalTarget(uint address, Coordinate coord)
        {
            return 0;
        }

        private static double GetTriangleVertexComponent(uint address, int index, Coordinate coordinate)
        {
            if (address == 0) return Double.NaN;
            switch (index)
            {
                case 1:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return TriangleOffsetsConfig.GetX1(address);
                        case Coordinate.Y:
                            return TriangleOffsetsConfig.GetY1(address);
                        case Coordinate.Z:
                            return TriangleOffsetsConfig.GetZ1(address);
                    }
                    break;
                case 2:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return TriangleOffsetsConfig.GetX2(address);
                        case Coordinate.Y:
                            return TriangleOffsetsConfig.GetY2(address);
                        case Coordinate.Z:
                            return TriangleOffsetsConfig.GetZ2(address);
                    }
                    break;
                case 3:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return TriangleOffsetsConfig.GetX3(address);
                        case Coordinate.Y:
                            return TriangleOffsetsConfig.GetY3(address);
                        case Coordinate.Z:
                            return TriangleOffsetsConfig.GetZ3(address);
                    }
                    break;
                case 4:
                    int closestVertexToMario = TriangleDataModel.CreateLazy(address).GetClosestVertex(
                        Mario.X, Mario.Y, Mario.Z);
                    return GetTriangleVertexComponent(address, closestVertexToMario, coordinate);
                case 5:
                    int closestVertexToSelf = TriangleDataModel.CreateLazy(address).GetClosestVertex(
                        SpecialConfig.SelfPA.X, SpecialConfig.SelfPA.Y, SpecialConfig.SelfPA.Z);
                    return GetTriangleVertexComponent(address, closestVertexToSelf, coordinate);
                case 6:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return Mario.X;
                        case Coordinate.Y:
                            return TriangleDataModel.CreateLazy(address).GetHeightOnTriangle(Mario.X, Mario.Z);
                        case Coordinate.Z:
                            return Mario.Z;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                case 7:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return SpecialConfig.SelfPA.X;
                        case Coordinate.Y:
                            return TriangleDataModel.CreateLazy(address).GetHeightOnTriangle(SpecialConfig.SelfPA.X, SpecialConfig.SelfPA.Z);
                        case Coordinate.Z:
                            return SpecialConfig.SelfPA.Z;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            throw new ArgumentOutOfRangeException();
        }

        private static double GetSnowComponent(int index, Coordinate coordinate)
        {
            short numSnowParticles = Config.Stream.GetShort(SnowConfig.CounterAddress);
            if (index < 0 || index >= numSnowParticles) return Double.NaN;
            uint snowStart = Config.Stream.GetUInt(SnowConfig.SnowArrayPointerAddress);
            uint structOffset = (uint)index * SnowConfig.ParticleStructSize;
            switch (coordinate)
            {
                case Coordinate.X:
                    return Config.Stream.GetInt(snowStart + structOffset + SnowConfig.XOffset);
                case Coordinate.Y:
                    return Config.Stream.GetInt(snowStart + structOffset + SnowConfig.YOffset);
                case Coordinate.Z:
                    return Config.Stream.GetInt(snowStart + structOffset + SnowConfig.ZOffset);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static double GetQFrameComponent(double frame, Coordinate coordinate)
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort angle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

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
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
            double frame = gFrame - globalTimer;
            return GetQFrameComponent(frame, coordinate);
        }

        private static double GetMarioProjection(Coordinate coordinate)
        {
            if (coordinate == Coordinate.Y)
            {
                return Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            }

            // 5.3 => 4.3 + 3.3 + 2.3 + 1.3 + 0.3

            ushort angle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            double angleTruncated = MoreMath.TruncateToMultipleOf16(angle);
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float hSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);

            double sign = Math.Sign(hSpeed);
            double hSpeedInt = (int)hSpeed;
            double hSpeedFraction = hSpeed - hSpeedInt;
            double hSpeedIntAbs = Math.Abs(hSpeedInt);
            double hSpeedFractionAbs = Math.Abs(hSpeedFraction);
            double distance = hSpeedIntAbs * (hSpeedIntAbs - 1) / 2 + hSpeedIntAbs * hSpeedFractionAbs;
            double signedDistance = sign * distance;

            (double x, double z) = MoreMath.AddVectorToPoint(signedDistance, angleTruncated, marioX, marioZ);
            return coordinate == Coordinate.X ? x : z;
        }

        private static double GetRolloutPeakComponent(CoordinateAngle coordAngle)
        {
            UpdateRolloutPeak();

            switch (coordAngle)
            {
                case CoordinateAngle.X:
                    return RolloutPeakX;
                case CoordinateAngle.Y:
                    return RolloutPeakY;
                case CoordinateAngle.Z:
                    return RolloutPeakZ;
                case CoordinateAngle.Angle:
                    return RolloutPeakAngle;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static float RolloutPeakX = 0;
        private static float RolloutPeakY = 0;
        private static float RolloutPeakZ = 0;
        private static ushort RolloutPeakAngle = 0;

        private static void UpdateRolloutPeak()
        {
            uint marioAction = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.ActionOffset);
            float marioYSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
            if (marioAction == 0x010008A6 && marioYSpeed == -2)
            {
                RolloutPeakX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
                RolloutPeakY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                RolloutPeakZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
                RolloutPeakAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            }
        }

        private static double GetPreviousPositionsComponent(Coordinate coordinate)
        {
            List<MapObjectPreviousPositions> prevPositionsObjs = new List<MapObjectPreviousPositions>();
            foreach (MapTracker mapTracker in Config.MapGui.flowLayoutPanelMapTrackers.Controls)
            {
                prevPositionsObjs.AddRange(mapTracker.GetMapPathObjectsOfType<MapObjectPreviousPositions>());
            }
            if (prevPositionsObjs.Count == 0) return double.NaN;
            MapObjectPreviousPositions prevPositions = prevPositionsObjs[0];
            (float x, float y, float z) = prevPositions.GetMidpoint();
            switch (coordinate)
            {
                case Coordinate.X:
                    return x;
                case Coordinate.Y:
                    return y;
                case Coordinate.Z:
                    return z;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static double GetNextPositionsComponent(Coordinate coordinate)
        {
            List<MapObjectNextPositions> nextPositionsObjs = new List<MapObjectNextPositions>();
            foreach (MapTracker mapTracker in Config.MapGui.flowLayoutPanelMapTrackers.Controls)
            {
                nextPositionsObjs.AddRange(mapTracker.GetMapPathObjectsOfType<MapObjectNextPositions>());
            }
            if (nextPositionsObjs.Count == 0) return double.NaN;
            MapObjectNextPositions nextPositions = nextPositionsObjs[0];
            (float x, float y, float z) = nextPositions.GetMidpoint();
            switch (coordinate)
            {
                case Coordinate.X:
                    return x;
                case Coordinate.Y:
                    return y;
                case Coordinate.Z:
                    return z;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private double GetOffset(Coordinate coordinate)
        {
            if (coordinate == Coordinate.Y) return PosAngle1.Y;
            double angle = OffsetAngleRelative.Value ? PosAngle1.Angle + OffsetAngle.Value : OffsetAngle.Value;
            double angleTruncated = MoreMath.NormalizeAngleTruncated(angle);
            var point = MoreMath.AddVectorToPoint(OffsetDist.Value, angleTruncated, PosAngle1.X, PosAngle1.Z);
            return coordinate == Coordinate.X ? point.x : point.z;
        }

        public double GetOffsetDist()
        {
            return OffsetDist.Value;
        }

        public double GetOffsetAngle()
        {
            return OffsetAngle.Value;
        }

        public PositionAngle GetBasePositionAngle()
        {
            return PosAngle1;
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
                case PositionAngleTypeEnum.CameraGoal:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.GoalXOffset);
                case PositionAngleTypeEnum.CameraGoalFocus:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.GoalFocusXOffset);
                case PositionAngleTypeEnum.CamHackCamera:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                case PositionAngleTypeEnum.CamHackFocus:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                case PositionAngleTypeEnum.MapCamera:
                    MapConfig.Map3DCameraX = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    MapConfig.Map3DFocusX = (float)value;
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
                case PositionAngleTypeEnum.First:
                    return SetObjectValue(value, Text, true, CoordinateAngle.X);
                case PositionAngleTypeEnum.Last:
                    return SetObjectValue(value, Text, false, CoordinateAngle.X);
                case PositionAngleTypeEnum.FirstHome:
                    return SetObjectValue(value, Text, true, CoordinateAngle.X, home: true);
                case PositionAngleTypeEnum.LastHome:
                    return SetObjectValue(value, Text, false, CoordinateAngle.X, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.BullyPivot:
                    return false;
                case PositionAngleTypeEnum.PyramidNormal:
                    return false;
                case PositionAngleTypeEnum.PyramidNormalTarget:
                    return false;
                case PositionAngleTypeEnum.KoopaTheQuick:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return false;
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
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.Snow:
                    return SetSnowComponent((int)value, Index.Value, Coordinate.X);
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.MarioProjection:
                    return false;
                case PositionAngleTypeEnum.RolloutPeak:
                    return false;
                case PositionAngleTypeEnum.PreviousPositions:
                    return false;
                case PositionAngleTypeEnum.NextPositions:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle1.SetX(value);
                case PositionAngleTypeEnum.Functions:
                    return Setters[0](value);
                case PositionAngleTypeEnum.Pos:
                    ThisX = value;
                    return true;
                case PositionAngleTypeEnum.Ang:
                    return false;
                case PositionAngleTypeEnum.Offset:
                    return SetOffset(value, Coordinate.X);
                case PositionAngleTypeEnum.YOffset:
                    return false;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetX(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetX(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetX(value);
                case PositionAngleTypeEnum.Self2:
                    return SpecialConfig.Self2PA.SetX(value);
                case PositionAngleTypeEnum.Point2:
                    return SpecialConfig.Point2PA.SetX(value);
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
                case PositionAngleTypeEnum.CameraGoal:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.GoalYOffset);
                case PositionAngleTypeEnum.CameraGoalFocus:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.GoalFocusYOffset);
                case PositionAngleTypeEnum.CamHackCamera:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                case PositionAngleTypeEnum.CamHackFocus:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                case PositionAngleTypeEnum.MapCamera:
                    MapConfig.Map3DCameraY = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    MapConfig.Map3DFocusY = (float)value;
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
                case PositionAngleTypeEnum.First:
                    return SetObjectValue(value, Text, true, CoordinateAngle.Y);
                case PositionAngleTypeEnum.Last:
                    return SetObjectValue(value, Text, false, CoordinateAngle.Y);
                case PositionAngleTypeEnum.FirstHome:
                    return SetObjectValue(value, Text, true, CoordinateAngle.Y, home: true);
                case PositionAngleTypeEnum.LastHome:
                    return SetObjectValue(value, Text, false, CoordinateAngle.Y, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.BullyPivot:
                    return false;
                case PositionAngleTypeEnum.PyramidNormal:
                    return false;
                case PositionAngleTypeEnum.PyramidNormalTarget:
                    return false;
                case PositionAngleTypeEnum.KoopaTheQuick:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return false;
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
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.Snow:
                    return SetSnowComponent((int)value, Index.Value, Coordinate.Y);
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.MarioProjection:
                    return false;
                case PositionAngleTypeEnum.RolloutPeak:
                    return false;
                case PositionAngleTypeEnum.PreviousPositions:
                    return false;
                case PositionAngleTypeEnum.NextPositions:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle1.SetY(value);
                case PositionAngleTypeEnum.Functions:
                    return Setters[1](value);
                case PositionAngleTypeEnum.Pos:
                    ThisY = value;
                    return true;
                case PositionAngleTypeEnum.Ang:
                    return false;
                case PositionAngleTypeEnum.Offset:
                    return SetOffset(value, Coordinate.Y);
                case PositionAngleTypeEnum.YOffset:
                    return false;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetY(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetY(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetY(value);
                case PositionAngleTypeEnum.Self2:
                    return SpecialConfig.Self2PA.SetY(value);
                case PositionAngleTypeEnum.Point2:
                    return SpecialConfig.Point2PA.SetY(value);
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
                case PositionAngleTypeEnum.CameraGoal:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.GoalZOffset);
                case PositionAngleTypeEnum.CameraGoalFocus:
                    return Config.Stream.SetValue((float)value, CameraConfig.StructAddress + CameraConfig.GoalFocusZOffset);
                case PositionAngleTypeEnum.CamHackCamera:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                case PositionAngleTypeEnum.CamHackFocus:
                    return Config.Stream.SetValue((float)value, CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                case PositionAngleTypeEnum.MapCamera:
                    MapConfig.Map3DCameraZ = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    MapConfig.Map3DFocusZ = (float)value;
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
                case PositionAngleTypeEnum.First:
                    return SetObjectValue(value, Text, true, CoordinateAngle.Z);
                case PositionAngleTypeEnum.Last:
                    return SetObjectValue(value, Text, false, CoordinateAngle.Z);
                case PositionAngleTypeEnum.FirstHome:
                    return SetObjectValue(value, Text, true, CoordinateAngle.Z, home: true);
                case PositionAngleTypeEnum.LastHome:
                    return SetObjectValue(value, Text, false, CoordinateAngle.Z, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.BullyPivot:
                    return false;
                case PositionAngleTypeEnum.PyramidNormal:
                    return false;
                case PositionAngleTypeEnum.PyramidNormalTarget:
                    return false;
                case PositionAngleTypeEnum.KoopaTheQuick:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return false;
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
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset), Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Floor:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset), Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Ceiling:
                    return SetTriangleVertexComponent(
                        (short)value, Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset), Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.Snow:
                    return SetSnowComponent((int)value, Index.Value, Coordinate.Z);
                case PositionAngleTypeEnum.QFrame:
                    return false;
                case PositionAngleTypeEnum.GFrame:
                    return false;
                case PositionAngleTypeEnum.MarioProjection:
                    return false;
                case PositionAngleTypeEnum.RolloutPeak:
                    return false;
                case PositionAngleTypeEnum.PreviousPositions:
                    return false;
                case PositionAngleTypeEnum.NextPositions:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle1.SetZ(value);
                case PositionAngleTypeEnum.Functions:
                    return Setters[2](value);
                case PositionAngleTypeEnum.Pos:
                    ThisZ = value;
                    return true;
                case PositionAngleTypeEnum.Ang:
                    return false;
                case PositionAngleTypeEnum.Offset:
                    return SetOffset(value, Coordinate.Z);
                case PositionAngleTypeEnum.YOffset:
                    return false;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetZ(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetZ(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetZ(value);
                case PositionAngleTypeEnum.Self2:
                    return SpecialConfig.Self2PA.SetZ(value);
                case PositionAngleTypeEnum.Point2:
                    return SpecialConfig.Point2PA.SetZ(value);
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
                case PositionAngleTypeEnum.CameraGoal:
                    return false;
                case PositionAngleTypeEnum.CameraGoalFocus:
                    return false;
                case PositionAngleTypeEnum.CamHackCamera:
                    return false;
                case PositionAngleTypeEnum.CamHackFocus:
                    return false;
                case PositionAngleTypeEnum.MapCamera:
                    MapConfig.Map3DCameraYaw = (float)value;
                    return true;
                case PositionAngleTypeEnum.MapFocus:
                    MapConfig.Map3DCameraYaw = (float)value;
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
                case PositionAngleTypeEnum.First:
                    return SetObjectValue(value, Text, true, CoordinateAngle.Angle);
                case PositionAngleTypeEnum.Last:
                    return SetObjectValue(value, Text, false, CoordinateAngle.Angle);
                case PositionAngleTypeEnum.FirstHome:
                    return SetObjectValue(value, Text, true, CoordinateAngle.Angle, home: true);
                case PositionAngleTypeEnum.LastHome:
                    return SetObjectValue(value, Text, false, CoordinateAngle.Angle, home: true);
                case PositionAngleTypeEnum.GoombaProjection:
                    return false;
                case PositionAngleTypeEnum.BullyPivot:
                    return false;
                case PositionAngleTypeEnum.PyramidNormal:
                    return false;
                case PositionAngleTypeEnum.PyramidNormalTarget:
                    return false;
                case PositionAngleTypeEnum.KoopaTheQuick:
                    return false;
                case PositionAngleTypeEnum.Ghost:
                    return false;
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
                case PositionAngleTypeEnum.MarioProjection:
                    return false;
                case PositionAngleTypeEnum.RolloutPeak:
                    return false;
                case PositionAngleTypeEnum.PreviousPositions:
                    return false;
                case PositionAngleTypeEnum.NextPositions:
                    return false;
                case PositionAngleTypeEnum.Schedule:
                    return false;
                case PositionAngleTypeEnum.Hybrid:
                    return PosAngle2.SetAngle(value);
                case PositionAngleTypeEnum.Functions:
                    if (Setters.Count >= 4) return Setters[3](value);
                    return false;
                case PositionAngleTypeEnum.Pos:
                    ThisAngle = value;
                    return true;
                case PositionAngleTypeEnum.Ang:
                    ThisAngle = value;
                    return true;
                case PositionAngleTypeEnum.Offset:
                    return false;
                case PositionAngleTypeEnum.YOffset:
                    return false;
                case PositionAngleTypeEnum.Trunc:
                    return PosAngle1.SetAngle(value);
                case PositionAngleTypeEnum.Self:
                    return SpecialConfig.SelfPA.SetAngle(value);
                case PositionAngleTypeEnum.Point:
                    return SpecialConfig.PointPA.SetAngle(value);
                case PositionAngleTypeEnum.Self2:
                    return SpecialConfig.Self2PA.SetAngle(value);
                case PositionAngleTypeEnum.Point2:
                    return SpecialConfig.Point2PA.SetAngle(value);
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
                uint marioObjRef = Config.Stream.GetUInt(MarioObjectConfig.PointerAddress);
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

        private static bool SetObjectValue(double value, string name, bool first, CoordinateAngle coordAngle, bool home = false, bool gfx = false)
        {
            List<ObjectDataModel> objs = Config.ObjectSlotsManager.GetLoadedObjectsWithName(name);
            ObjectDataModel obj = first ? objs.FirstOrDefault() : objs.LastOrDefault();
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
                case 1:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return TriangleOffsetsConfig.SetX1(value, address);
                        case Coordinate.Y:
                            return TriangleOffsetsConfig.SetY1(value, address);
                        case Coordinate.Z:
                            return TriangleOffsetsConfig.SetZ1(value, address);
                    }
                    break;
                case 2:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return TriangleOffsetsConfig.SetX2(value, address);
                        case Coordinate.Y:
                            return TriangleOffsetsConfig.SetY2(value, address);
                        case Coordinate.Z:
                            return TriangleOffsetsConfig.SetZ2(value, address);
                    }
                    break;
                case 3:
                    switch (coordinate)
                    {
                        case Coordinate.X:
                            return TriangleOffsetsConfig.SetX3(value, address);
                        case Coordinate.Y:
                            return TriangleOffsetsConfig.SetY3(value, address);
                        case Coordinate.Z:
                            return TriangleOffsetsConfig.SetZ3(value, address);
                    }
                    break;
                case 4:
                    int closestVertexToMario = TriangleDataModel.CreateLazy(address).GetClosestVertex(
                        Mario.X, Mario.Y, Mario.Z);
                    return SetTriangleVertexComponent(value, address, closestVertexToMario, coordinate);
                case 5:
                    int closestVertexToSelf = TriangleDataModel.CreateLazy(address).GetClosestVertex(
                        SpecialConfig.SelfPA.X, SpecialConfig.SelfPA.Y, SpecialConfig.SelfPA.Z);
                    return SetTriangleVertexComponent(value, address, closestVertexToSelf, coordinate);
                case 6:
                    return false;
                case 7:
                    return false;
            }
            throw new ArgumentOutOfRangeException();
        }

        private static bool SetSnowComponent(int value, int index, Coordinate coordinate)
        {
            short numSnowParticles = Config.Stream.GetShort(SnowConfig.CounterAddress);
            if (index < 0 || index > numSnowParticles) return false;
            uint snowStart = Config.Stream.GetUInt(SnowConfig.SnowArrayPointerAddress);
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

        public static (double x, double y, double z) GetMidPoint(PositionAngle p1, PositionAngle p2)
        {
            double x = (p1.X + p2.X) / 2;
            double y = (p1.Y + p2.Y) / 2;
            double z = (p1.Z + p2.Z) / 2;
            return (x, y, z);
        }





        public static bool SetDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetHDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetXDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetYDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetZDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetFDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetSDistance(PositionAngle p1, PositionAngle p2, double distance, bool toggle)
        {
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

        public static bool SetAngleTo(PositionAngle p1, PositionAngle p2, double angle, bool toggle)
        {
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

        public static bool SetDAngleTo(PositionAngle p1, PositionAngle p2, double angleDiff, bool toggle)
        {
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

        public static bool SetAngleDifference(PositionAngle p1, PositionAngle p2, double angleDiff, bool toggle)
        {
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

        private bool SetOffset(double value, Coordinate coordinate)
        {
            if (coordinate == Coordinate.Y) return false;

            double newX = coordinate == Coordinate.X ? value : X;
            double newZ = coordinate == Coordinate.Z ? value : Z;

            double dist = MoreMath.GetDistanceBetween(PosAngle1.X, PosAngle1.Z, newX, newZ);
            double angle = MoreMath.AngleTo_AngleUnits(PosAngle1.X, PosAngle1.Z, newX, newZ);
            if (OffsetAngleRelative.Value)
            {
                angle -= PosAngle1.Angle;
            }

            OffsetDist = dist;
            OffsetAngle = angle;
            return true;
        }

        public void SetOffsetDist(double value)
        {
            OffsetDist = value;
        }

        public void SetOffsetAngle(double value)
        {
            OffsetAngle = value;
        }

        public void SetOffsetAngleRelative(bool value)
        {
            OffsetAngleRelative = value;
        }
    }
}
