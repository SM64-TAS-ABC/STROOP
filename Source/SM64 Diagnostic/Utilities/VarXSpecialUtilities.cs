using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public static class VarXSpecialUtilities
    {
        private readonly static Func<List<string>> DEFAULT_GETTER = () => new List<string>() { "UNIMPLEMENTED" };
        private readonly static Func<string, bool> DEFAULT_SETTER = (string stringValue) => false;

        public static (Func<List<string>> getter, Func<string, bool> setter) CreateGetterSetterFunctions(string specialType)
        {
            Func<List<string>> getterFunction = DEFAULT_GETTER;
            Func<string, bool> setterFunction = DEFAULT_SETTER;

            switch (specialType)
            {
                case "MarioDistanceToObject":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return MoreMath.GetDistanceBetween(
                                marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z).ToString();
                        });
                    };
                    setterFunction = (string stringValue) =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        if (objPoses.Count == 0) return false;
                        Position objPos = objPoses[0];
                        double? distAway = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAway.HasValue) return false;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway.Value);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "MarioHorizontalDistanceToObject":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return MoreMath.GetDistanceBetween(
                                marioPos.X, marioPos.Z, objPos.X, objPos.Z).ToString();
                        });
                    };
                    setterFunction = (string stringValue) =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        if (objPoses.Count == 0) return false;
                        Position objPos = objPoses[0];
                        double? distAway = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAway.HasValue) return false;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway.Value);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "MarioVerticalDistanceToObject":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return (marioPos.Y - objPos.Y).ToString();
                        });
                    };
                    setterFunction = (string stringValue) =>
                    {
                        List<Position> objPoses = GetObjectPositions();
                        if (objPoses.Count == 0) return false;
                        Position objPos = objPoses[0];
                        double? distAbove = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAbove.HasValue) return false;
                        double newMarioY = objPos.Y + distAbove.Value;
                        return SetMarioPosition(null, newMarioY, null);
                    };
                    break;

                case "AngleObjectToMario":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return MoreMath.AngleTo_AngleUnits(objPos.X, objPos.Z, marioPos.X, marioPos.Z).ToString();
                        });
                    };
                    break;

                case "DeltaAngleObjectToMario":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return (objPos.Angle - MoreMath.AngleTo_AngleUnits(objPos.X, objPos.Z, marioPos.X, marioPos.Z)).ToString();
                        });
                    };
                    break;

                case "AngleMarioToObject":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return MoreMath.AngleTo_AngleUnits(marioPos.X, marioPos.Z, objPos.X, objPos.Z).ToString();
                        });
                    };
                    break;

                case "DeltaAngleMarioToObject":
                    getterFunction = () =>
                    {
                        Position marioPos = GetMarioPosition();
                        List<Position> objPoses = GetObjectPositions();
                        return objPoses.ConvertAll(objPos =>
                        {
                            return (marioPos.Angle - MoreMath.AngleTo_AngleUnits(marioPos.X, marioPos.Z, objPos.X, objPos.Z)).ToString();
                        });
                    };
                    break;

                case "ActionDescription":
                    getterFunction = () =>
                    {
                        uint action = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset);
                        string actionDescription = Config.MarioActions.GetActionName(action);
                        return CreateList(actionDescription);
                    };
                    break;

                case "PrevActionDescription":
                    getterFunction = () =>
                    {
                        uint prevAction = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.PrevActionOffset);
                        string actionDescription = Config.MarioActions.GetActionName(prevAction);
                        return CreateList(actionDescription);
                    };
                    break;

                case "MarioAnimationDescription":
                    getterFunction = () =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
                        short marioObjAnimation = Config.Stream.GetInt16(marioObjRef + Config.Mario.ObjectAnimationOffset);
                        string animationDescription = Config.MarioAnimations.GetAnimationName(marioObjAnimation);
                        return CreateList(animationDescription);
                    };
                    break;

                case "WaterAboveMedian":
                    getterFunction = () =>
                    {
                        short waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + Config.Mario.WaterLevelOffset);
                        short waterLevelMedian = Config.Stream.GetInt16(Config.WaterLevelMedianAddress);
                        double waterAboveMedian = waterLevel - waterLevelMedian;
                        return CreateList(waterAboveMedian);
                    };
                    break;

                case "MarioAboveWater":
                    getterFunction = () =>
                    {
                        short waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + Config.Mario.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float marioAboveWater = marioY - waterLevel;
                        return CreateList(marioAboveWater);
                    };
                    setterFunction = (string stringValue) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double goalMarioAboveWater = doubleValueNullable.Value;
                        short waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + Config.Mario.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        double goalMarioY = waterLevel + goalMarioAboveWater;
                        return Config.Stream.SetValue((float)goalMarioY, Config.Mario.StructAddress + Config.Mario.YOffset);
                    };
                    break;

                // Misc vars

                case "RngIndex":
                    getterFunction = () =>
                    {
                        ushort rngValue = Config.Stream.GetUInt16(Config.RngAddress);
                        string rngIndexString = RngIndexer.GetRngIndexString(rngValue);
                        return CreateList(rngIndexString);
                    };
                    setterFunction = (string stringValue) =>
                    {
                        int? index = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!index.HasValue) return false;
                        ushort rngValue = RngIndexer.GetRngValue(index.Value);
                        return Config.Stream.SetValue(rngValue, Config.RngAddress);
                    };
                    break;

                case "RngCallsPerFrame":
                    getterFunction = () =>
                    {
                        ushort preRng = Config.Stream.GetUInt16(Config.HackedAreaAddress + 0x0C);
                        ushort currentRng = Config.Stream.GetUInt16(Config.HackedAreaAddress + 0x0E);
                        int rngDiff = RngIndexer.GetRngIndexDiff(preRng, currentRng);
                        return CreateList(rngDiff);
                    };
                    break;

                case "NumberOfLoadedObjects":
                    getterFunction = () =>
                    {
                        int numberOfLoadedObjects = ObjectSlotsManager.Instance.ActiveObjectCount;
                        return CreateList(numberOfLoadedObjects);
                    };
                    break;

                // Area vars

                case "CurrentAreaIndexMario":
                    getterFunction = () =>
                    {
                        uint currentAreaMario = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
                        string currentAreaIndexMario = AreaUtilities.GetAreaIndexString(currentAreaMario);
                        return CreateList(currentAreaIndexMario);
                    };
                    setterFunction = (string stringValue) =>
                    {
                        int? intValueNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!intValueNullable.HasValue) return false;
                        int currentAreaIndexMario = intValueNullable.Value;
                        if (currentAreaIndexMario < 0 || currentAreaIndexMario >= 8) return false;
                        uint currentAreaAddressMario = AreaUtilities.GetAreaAddress(currentAreaIndexMario);
                        return Config.Stream.SetValue(currentAreaAddressMario, Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
                    };
                    break;

                case "CurrentAreaIndex":
                    getterFunction = () =>
                    {
                        uint currentArea = Config.Stream.GetUInt32(Config.Area.CurrentAreaPointerAddress);
                        string currentAreaIndex = AreaUtilities.GetAreaIndexString(currentArea);
                        return CreateList(currentAreaIndex);
                    };
                    setterFunction = (string stringValue) =>
                    {
                        int? intValueNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!intValueNullable.HasValue) return false;
                        int currentAreaIndex = intValueNullable.Value;
                        if (currentAreaIndex < 0 || currentAreaIndex >= 8) return false;
                        uint currentAreaAddress = AreaUtilities.GetAreaAddress(currentAreaIndex);
                        return Config.Stream.SetValue(currentAreaAddress, Config.Area.CurrentAreaPointerAddress);
                    };
                    break;

                case "AreaTerrainDescription":
                    getterFunction = () =>
                    {
                        short terrainType = Config.Stream.GetInt16(AreaManager.Instance.SelectedAreaAddress + Config.Area.TerrainTypeOffset);
                        string terrainDescription = AreaUtilities.GetTerrainDescription(terrainType);
                        return CreateList(terrainDescription);
                    };
                    setterFunction = (string stringValue) =>
                    {
                        short? terrainTypeNullable = AreaUtilities.GetTerrainType(stringValue);
                        if (!terrainTypeNullable.HasValue) return false;
                        short terrainType = terrainTypeNullable.Value;
                        return Config.Stream.SetValue(terrainType, AreaManager.Instance.SelectedAreaAddress + Config.Area.TerrainTypeOffset);
                    };
                    break;

                default:
                    break;
            }

            return (getterFunction, setterFunction);
        }

        private static List<string> CreateList(object value)
        {
            return new List<string>() { value.ToString() };
        }

        private static Position GetMarioPosition()
        {
            float marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
            ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            return new Position(marioX, marioY, marioZ, marioAngle);
        }

        private static bool SetMarioPosition(double? x, double? y, double? z)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, Config.Mario.StructAddress + Config.Mario.XOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, Config.Mario.StructAddress + Config.Mario.YOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, Config.Mario.StructAddress + Config.Mario.ZOffset);
            return success;
        }

        private static List<Position> GetObjectPositions()
        {
            return ObjectManager.Instance.CurrentAddresses.ConvertAll(objAddress =>
            {
                float objX = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectXOffset);
                float objY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                float objZ = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectZOffset);
                ushort objAngle = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset);
                return new Position(objX, objY, objZ, objAngle);
            });
        }

        private struct Position
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly ushort Angle;

            public Position(float x, float y, float z, ushort angle)
            {
                X = x;
                Y = y;
                Z = z;
                Angle = angle;
            }
        }
    }
}