using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace STROOP.Structs
{
    public static class WatchVariableSpecialUtilities
    {
        private readonly static Func<uint, object> DEFAULT_GETTER = (uint address) => Double.NaN;
        private readonly static Func<object, uint, bool> DEFAULT_SETTER = (object value, uint address) => false;

        private static Dictionary<string, (Func<uint, object>, Func<object, uint, bool>)> _dictionary;

        static WatchVariableSpecialUtilities()
        {
            CreateDictionary();
        }

        public static (Func<uint, object> getter, Func<object, uint, bool> setter) CreateGetterSetterFunctions(string specialType)
        {
            if (_dictionary.ContainsKey(specialType))
                return _dictionary[specialType];
            else
                return WatchVariableSpecialUtilities2.CreateGetterSetterFunctionsUsingSwitchStatement(specialType);
        }

        public static void CreateDictionary()
        {

            _dictionary = new Dictionary<string, (Func<uint, object>, Func<object, uint, bool>)>()
            {
                // Object generic vars

                ["MarioDistanceToObject"] =
                ((uint objAddress) =>
                {
                    Position marioPos = GetMarioPosition();
                    Position objPos = GetObjectPosition(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    return dist;
                }, (object objectValue, uint objAddress) =>
                {
                    Position marioPos = GetMarioPosition();
                    Position objPos = GetObjectPosition(objAddress);
                    double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!distAwayNullable.HasValue) return false;
                    double distAway = distAwayNullable.Value;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                }),

                ["MarioHDistanceToObject"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        return hDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? distAway = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAway.HasValue) return false;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway.Value);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["MarioXDistanceToObject"] =
                    ((uint objAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float xDist = marioX - objX;
                        return xDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float? xDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!xDist.HasValue) return false;
                        float newMarioX = objX + xDist.Value;
                        return Config.Stream.SetValue(newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["MarioYDistanceToObject"] =
                    ((uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float yDist = marioY - objY;
                        return yDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float? distAbove = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!distAbove.HasValue) return false;
                        float newMarioY = objY + distAbove.Value;
                        return Config.Stream.SetValue(newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["MarioZDistanceToObject"] =
                    ((uint objAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float zDist = marioZ - objZ;
                        return zDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float? zDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!zDist.HasValue) return false;
                        float newMarioZ = objZ + zDist.Value;
                        return Config.Stream.SetValue(newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["MarioDistanceToObjectHome"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, homePos.X, homePos.Y, homePos.Z);
                        return dist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                homePos.X, homePos.Y, homePos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["MarioHDistanceToObjectHome"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, homePos.X, homePos.Z);
                        return hDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAway = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAway.HasValue) return false;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(homePos.X, homePos.Z, marioPos.X, marioPos.Z, distAway.Value);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["MarioXDistanceToObjectHome"] =
                    ((uint objAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        float homeX = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeXOffset);
                        float xDist = marioX - homeX;
                        return xDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float homeX = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeXOffset);
                        float? xDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!xDist.HasValue) return false;
                        float newMarioX = homeX + xDist.Value;
                        return Config.Stream.SetValue(newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["MarioYDistanceToObjectHome"] =
                    ((uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        float yDist = marioY - homeY;
                        return yDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        float? yDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!yDist.HasValue) return false;
                        float newMarioY = homeY + yDist.Value;
                        return Config.Stream.SetValue(newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["MarioZDistanceToObjectHome"] =
                    ((uint objAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        float homeZ = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeZOffset);
                        float zDist = marioZ - homeZ;
                        return zDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float homeZ = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeZOffset);
                        float? zDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!zDist.HasValue) return false;
                        float newMarioZ = homeZ + zDist.Value;
                        return Config.Stream.SetValue(newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["ObjectDistanceToHome"] =
                    ((uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            objPos.X, objPos.Y, objPos.Z, homePos.X, homePos.Y, homePos.Z);
                        return dist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newObjX, double newObjY, double newObjZ) =
                            MoreMath.ExtrapolateLine3D(
                                homePos.X, homePos.Y, homePos.Z, objPos.X, objPos.Y, objPos.Z, distAway);
                        return SetObjectPosition(objAddress, newObjX, newObjY, newObjZ);
                    }),

                ["ObjectHDistanceToHome"] =
                    ((uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        return hDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAway = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAway.HasValue) return false;
                        (double newObjX, double newObjZ) =
                            MoreMath.ExtrapolateLine2D(homePos.X, homePos.Z, objPos.X, objPos.Z, distAway.Value);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    }),

                ["ObjectXDistanceToHome"] =
                    ((uint objAddress) =>
                    {
                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float homeX = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeXOffset);
                        float xDist = objX - homeX;
                        return xDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float homeX = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeXOffset);
                        float? xDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!xDist.HasValue) return false;
                        float newObjX = homeX + xDist.Value;
                        return Config.Stream.SetValue(newObjX, objAddress + ObjectConfig.XOffset);
                    }),

                ["ObjectYDistanceToHome"] =
                    ((uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        float yDist = objY - homeY;
                        return yDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        float? yDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!yDist.HasValue) return false;
                        float newObjY = homeY + yDist.Value;
                        return Config.Stream.SetValue(newObjY, objAddress + ObjectConfig.YOffset);
                    }),

                ["ObjectZDistanceToHome"] =
                    ((uint objAddress) =>
                    {
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float homeZ = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeZOffset);
                        float zDist = objZ - homeZ;
                        return zDist;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float homeZ = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeZOffset);
                        float? zDist = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!zDist.HasValue) return false;
                        float newObjZ = homeZ + zDist.Value;
                        return Config.Stream.SetValue(newObjZ, objAddress + ObjectConfig.ZOffset);
                    }),

                ["AngleObjectToMario"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToMario);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newObjX, double newObjZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                objPos.X, objPos.Z, marioPos.X, marioPos.Z, angle);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    }),

                ["DeltaAngleObjectToMario"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double angleDiff = objPos.Angle.Value - angleToMario;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double newObjAngleDouble = angleToMario + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    }),

                ["InGameAngleObjectToMario"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = InGameTrigUtilities.InGameAngleTo(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToMario);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newObjX, double newObjZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                objPos.X, objPos.Z, marioPos.X, marioPos.Z, angle);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    }),

                ["InGameDeltaAngleObjectToMario"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = InGameTrigUtilities.InGameAngleTo(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double angleDiff = objPos.Angle.Value - angleToMario;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double newObjAngleDouble = angleToMario + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    }),

                ["AngleMarioToObject"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToObject = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToObject);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, objPos.X, objPos.Z, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DeltaAngleMarioToObject"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToObject = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        double angleDiff = marioPos.Angle.Value - angleToObject;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToObj = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        double newMarioAngleDouble = angleToObj + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["AngleObjectToHome"] =
                    ((uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double angleToHome = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToHome);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newObjX, double newObjZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                objPos.X, objPos.Z, homePos.X, homePos.Z, angle);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    }),

                ["DeltaAngleObjectToHome"] =
                    ((uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double angleToHome = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        double angleDiff = objPos.Angle.Value - angleToHome;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToHome = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        double newObjAngleDouble = angleToHome + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    }),

                ["AngleHomeToObject"] =
                    ((uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double angleHomeToObject = MoreMath.AngleTo_AngleUnits(
                            homePos.X, homePos.Z, objPos.X, objPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleHomeToObject);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newHomeX, double newHomeZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                homePos.X, homePos.Z, objPos.X, objPos.Z, angle);
                        return SetObjectHomePosition(objAddress, newHomeX, null, newHomeZ);
                    }),

                ["MarioHitboxAwayFromObject"] =
                    ((uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjX = Config.Stream.GetSingle(marioObjRef + ObjectConfig.XOffset);
                        float mObjZ = Config.Stream.GetSingle(marioObjRef + ObjectConfig.ZOffset);
                        float mObjHitboxRadius = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxRadius);

                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float objHitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);

                        double marioHitboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHitboxRadius - objHitboxRadius;
                        return marioHitboxAwayFromObject;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjX = Config.Stream.GetSingle(marioObjRef + ObjectConfig.XOffset);
                        float mObjZ = Config.Stream.GetSingle(marioObjRef + ObjectConfig.ZOffset);
                        float mObjHitboxRadius = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxRadius);

                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float objHitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);

                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? hitboxDistAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!hitboxDistAwayNullable.HasValue) return false;
                        double hitboxDistAway = hitboxDistAwayNullable.Value;
                        double distAway = hitboxDistAway + mObjHitboxRadius + objHitboxRadius;

                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                        return SetMarioPositionAndMarioObjectPosition(newMarioX, null, newMarioZ);
                    }),

                ["MarioHitboxAboveObject"] =
                    ((uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                        float mObjHitboxHeight = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxHeight);
                        float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);
                        float mObjHitboxBottom = mObjY - mObjHitboxDownOffset;

                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                        float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                        float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                        double marioHitboxAboveObject = mObjHitboxBottom - objHitboxTop;
                        return marioHitboxAboveObject;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                        float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);

                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                        float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                        float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                        double? hitboxDistAboveNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!hitboxDistAboveNullable.HasValue) return false;
                        double hitboxDistAbove = hitboxDistAboveNullable.Value;
                        double newMarioY = objHitboxTop + mObjHitboxDownOffset + hitboxDistAbove;
                        return SetMarioPositionAndMarioObjectPosition(null, newMarioY, null);
                    }),

                ["MarioHitboxBelowObject"] =
                    ((uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                        float mObjHitboxHeight = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxHeight);
                        float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);
                        float mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                        float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                        float objHitboxBottom = objY - objHitboxDownOffset;

                        double marioHitboxBelowObject = objHitboxBottom - mObjHitboxTop;
                        return marioHitboxBelowObject;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                        float mObjHitboxHeight = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxHeight);
                        float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);
                        float mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                        float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                        float objHitboxBottom = objY - objHitboxDownOffset;

                        double? hitboxDistBelowNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!hitboxDistBelowNullable.HasValue) return false;
                        double hitboxDistBelow = hitboxDistBelowNullable.Value;
                        double newMarioY = objHitboxBottom - (mObjHitboxTop - mObjY) - hitboxDistBelow;
                        return SetMarioPositionAndMarioObjectPosition(null, newMarioY, null);
                    }),

                ["MarioHitboxOverlapsObject"] =
                    ((uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjX = Config.Stream.GetSingle(marioObjRef + ObjectConfig.XOffset);
                        float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                        float mObjZ = Config.Stream.GetSingle(marioObjRef + ObjectConfig.ZOffset);
                        float mObjHitboxRadius = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxRadius);
                        float mObjHitboxHeight = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxHeight);
                        float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);
                        float mObjHitboxBottom = mObjY - mObjHitboxDownOffset;
                        float mObjHitboxTop = mObjY + mObjHitboxHeight - mObjHitboxDownOffset;

                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float objHitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                        float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                        float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                        float objHitboxBottom = objY - objHitboxDownOffset;
                        float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                        double marioHitboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHitboxRadius - objHitboxRadius;
                        double marioHitboxAboveObject = mObjHitboxBottom - objHitboxTop;
                        double marioHitboxBelowObject = objHitboxBottom - mObjHitboxTop;

                        bool overlap = marioHitboxAwayFromObject < 0 && marioHitboxAboveObject <= 0 && marioHitboxBelowObject <= 0;
                        return overlap ? "1" : "0";
                    }),

                ["MarioPunchAngleAway"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        ushort angleToObj = InGameTrigUtilities.InGameAngleTo(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        int angleDiff = marioPos.Angle.Value - angleToObj;
                        int angleDiffShort = MoreMath.NormalizeAngleShort(angleDiff);
                        int angleDiffAbs = Math.Abs(angleDiffShort);
                        int angleAway = angleDiffAbs - 0x2AAA;
                        return angleAway;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        double? angleAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleAwayNullable.HasValue) return false;
                        double angleAway = angleAwayNullable.Value;

                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        ushort angleToObj = InGameTrigUtilities.InGameAngleTo(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        int oldAngleDiff = marioPos.Angle.Value - angleToObj;
                        int oldAngleDiffShort = MoreMath.NormalizeAngleShort(oldAngleDiff);
                        int signMultiplier = oldAngleDiffShort >= 0 ? 1 : -1;

                        double angleDiffAbs = angleAway + 0x2AAA;
                        double angleDiff = angleDiffAbs * signMultiplier;
                        double marioAngleDouble = angleToObj + angleDiff;
                        ushort marioAngleUShort = MoreMath.NormalizeAngleUshort(marioAngleDouble);

                        return Config.Stream.SetValue(marioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["ObjectRngCallsPerFrame"] =
                    ((uint objAddress) =>
                    {
                        uint numberOfRngObjs = Config.Stream.GetUInt32(MiscConfig.HackedAreaAddress);
                        int numOfCalls = 0;
                        for (int i = 0; i < Math.Min(numberOfRngObjs, ObjectSlotsConfig.MaxSlots); i++)
                        {
                            uint rngStructAdd = (uint)(MiscConfig.HackedAreaAddress + 0x30 + 0x08 * i);
                            uint address = Config.Stream.GetUInt32(rngStructAdd + 0x04);
                            if (address != objAddress) continue;
                            ushort preRng = Config.Stream.GetUInt16(rngStructAdd + 0x00);
                            ushort postRng = Config.Stream.GetUInt16(rngStructAdd + 0x02);
                            numOfCalls = RngIndexer.GetRngIndexDiff(preRng, postRng);
                            break;
                        }
                        return numOfCalls;
                    }),

                // Object specific vars - Pendulum

                ["PendulumAmplitude"] =
                    ((uint objAddress) =>
                    {
                        float pendulumAmplitude = GetPendulumAmplitude(objAddress);
                        return pendulumAmplitude;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        double? amplitudeNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!amplitudeNullable.HasValue) return false;
                        double amplitude = amplitudeNullable.Value;
                        float accelerationDirection = amplitude > 0 ? -1 : 1;

                        bool success = true;
                        success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                        success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                        success &= Config.Stream.SetValue((float)amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                        return success;
                    }),

                ["PendulumSwingIndex"] =
                    ((uint objAddress) =>
                    {
                        float pendulumAmplitudeFloat = GetPendulumAmplitude(objAddress);
                        int? pendulumAmplitudeIntNullable = ParsingUtilities.ParseIntNullable(pendulumAmplitudeFloat);
                        if (!pendulumAmplitudeIntNullable.HasValue) return Double.NaN;
                        int pendulumAmplitudeInt = pendulumAmplitudeIntNullable.Value;
                        int? pendulumSwingIndexNullable = TableConfig.PendulumSwings.GetPendulumSwingIndex(pendulumAmplitudeInt);
                        if (!pendulumSwingIndexNullable.HasValue) return Double.NaN;
                        int pendulumSwingIndex = pendulumSwingIndexNullable.Value;
                        return pendulumSwingIndex;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        int? indexNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!indexNullable.HasValue) return false;
                        int index = indexNullable.Value;
                        float amplitude = TableConfig.PendulumSwings.GetPendulumAmplitude(index);
                        float accelerationDirection = amplitude > 0 ? -1 : 1;

                        bool success = true;
                        success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                        success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                        success &= Config.Stream.SetValue(amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                        return success;
                    }),

                // Object specific vars - Waypoint

                ["ObjectDotProductToWaypoint"] =
                    ((uint objAddress) =>
                    {
                        (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                            GetWaypointSpecialVars(objAddress);
                        return dotProduct;
                    }),

                ["ObjectDistanceToWaypointPlane"] =
                    ((uint objAddress) =>
                    {
                        (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                            GetWaypointSpecialVars(objAddress);
                        return distToWaypointPlane;
                    }),

                ["ObjectDistanceToWaypoint"] =
                    ((uint objAddress) =>
                    {
                        (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                            GetWaypointSpecialVars(objAddress);
                        return distToWaypoint;
                    }),

                // Object specific vars - Racing Penguin

                ["RacingPenguinEffortTarget"] =
                    ((uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return effortTarget;
                    }),

                ["RacingPenguinEffortChange"] =
                    ((uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return effortChange;
                    }),

                ["RacingPenguinMinHSpeed"] =
                    ((uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return minHSpeed;
                    }),

                ["RacingPenguinHSpeedTarget"] =
                    ((uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return hSpeedTarget;
                    }),

                ["RacingPenguinDiffHSpeedTarget"] =
                    ((uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        float hSpeed = Config.Stream.GetSingle(objAddress + ObjectConfig.HSpeedOffset);
                        double hSpeedDiff = hSpeed - hSpeedTarget;
                        return hSpeedDiff;
                    }),

                ["RacingPenguinProgress"] =
                    ((uint objAddress) =>
                    {
                        double progress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                        return progress;
                    }),

                ["RacingPenguinProgressDiff"] =
                    ((uint objAddress) =>
                    {
                        Dictionary<int, TestingManager.VarState> dictionary = Config.TestingManager.VarStateDictionary;
                        var currentTimer = Config.Stream.GetInt32(Config.SwitchRomVersion(0x803493DC, 0x803463EC));
                        if (!dictionary.ContainsKey(currentTimer))
                        {
                            return double.NaN;
                        }
                        TestingManager.VarState varState = dictionary[currentTimer];
                        if (!(varState is TestingManager.VarStatePenguin))
                        {
                            return double.NaN;
                        }
                        TestingManager.VarStatePenguin varStatePenguin = varState as TestingManager.VarStatePenguin;
                        double varStateProgress = varStatePenguin.Progress;
                        double currentProgress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                        double progressDiff = currentProgress - varStateProgress;
                        return progressDiff;
                    }),

                // Object specific vars - Koopa the Quick

                ["KoopaTheQuickHSpeedTarget"] =
                    ((uint objAddress) =>
                    {
                        (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                        return hSpeedTarget;
                    }),

                ["KoopaTheQuickHSpeedChange"] =
                    ((uint objAddress) =>
                    {
                        (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                        return hSpeedChange;
                    }),

                ["KoopaTheQuick1Progress"] =
                    ((uint objAddress) =>
                    {
                        double progress = TableConfig.KoopaTheQuick1Waypoints.GetProgress(objAddress);
                        return progress;
                    }),

                ["KoopaTheQuick2Progress"] =
                    ((uint objAddress) =>
                    {
                        double progress = TableConfig.KoopaTheQuick2Waypoints.GetProgress(objAddress);
                        return progress;
                    }),

                // Object specific vars - Fly Guy

                ["FlyGuyZone"] =
                    ((uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        double heightDiff = marioY - objY;
                        if (heightDiff < -400) return "Low";
                        if (heightDiff > -200) return "High";
                        return "Medium";
                    }),

                ["FlyGuyRelativeHeight"] =
                    ((uint objAddress) =>
                    {
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double relativeHeight = TableConfig.FlyGuyData.GetRelativeHeight(oscillationTimer);
                        return relativeHeight;
                    }),

                ["FlyGuyNextHeightDiff"] =
                    ((uint objAddress) =>
                    {
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double nextRelativeHeight = TableConfig.FlyGuyData.GetNextHeightDiff(oscillationTimer);
                        return nextRelativeHeight;
                    }),

                ["FlyGuyMinHeight"] =
                    ((uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double minHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, objY);
                        return minHeight;
                    }),

                ["FlyGuyMaxHeight"] =
                    ((uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double maxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, objY);
                        return maxHeight;
                    }),

                // Object specific vars - Bob-omb

                ["BobombBloatSize"] =
                    ((uint objAddress) =>
                    {
                        float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                        float bloatSize = (hitboxRadius - 65) / 13;
                        return bloatSize;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float? bloatSizeNullable = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!bloatSizeNullable.HasValue) return false;
                        float bloatSize = bloatSizeNullable.Value;
                        float hitboxRadius = bloatSize * 13 + 65;
                        float hitboxHeight = bloatSize * 22.6f + 113;
                        float scale = bloatSize / 5 + 1;

                        bool success = true;
                        success &= Config.Stream.SetValue(hitboxRadius, objAddress + ObjectConfig.HitboxRadius);
                        success &= Config.Stream.SetValue(hitboxHeight, objAddress + ObjectConfig.HitboxHeight);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleWidthOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleHeightOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleDepthOffset);
                        return success;
                    }),

                ["BobombRadius"] =
                    ((uint objAddress) =>
                    {
                        float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                        float radius = hitboxRadius + 32;
                        return radius;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        float? radiusNullable = ParsingUtilities.ParseFloatNullable(objectValue);
                        if (!radiusNullable.HasValue) return false;
                        float radius = radiusNullable.Value;
                        float bloatSize = (radius - 97) / 13;
                        float hitboxRadius = bloatSize * 13 + 65;
                        float hitboxHeight = bloatSize * 22.6f + 113;
                        float scale = bloatSize / 5 + 1;

                        bool success = true;
                        success &= Config.Stream.SetValue(hitboxRadius, objAddress + ObjectConfig.HitboxRadius);
                        success &= Config.Stream.SetValue(hitboxHeight, objAddress + ObjectConfig.HitboxHeight);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleWidthOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleHeightOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleDepthOffset);
                        return success;
                    }),

                ["BobombSpaceBetween"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                        float radius = hitboxRadius + 32;
                        double spaceBetween = hDist - radius;
                        return spaceBetween;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        double? spaceBetweenNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!spaceBetweenNullable.HasValue) return false;
                        double spaceBetween = spaceBetweenNullable.Value;
                        float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                        float radius = hitboxRadius + 32;
                        double distAway = spaceBetween + radius;

                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(
                                objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                // Object specific vars - Scuttlebug

                ["ScuttlebugDeltaAngleToTarget"] =
                    ((uint objAddress) =>
                    {
                        ushort facingAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                        ushort targetAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                        int angleDiff = facingAngle - targetAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        ushort targetAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                        double newObjAngleDouble = targetAngle + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    }),

                // Object specific vars - Goomba Triplet Spawner

                ["GoombaTripletLoadingDistanceDiff"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                        double distDiff = dist - 3000;
                        return distDiff;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? distDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distDiffNullable.HasValue) return false;
                        double distDiff = distDiffNullable.Value;
                        double distAway = distDiff + 3000;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["GoombaTripletUnloadingDistanceDiff"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                        double distDiff = dist - 4000;
                        return distDiff;
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? distDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distDiffNullable.HasValue) return false;
                        double distDiff = distDiffNullable.Value;
                        double distAway = distDiff + 4000;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["BitfsPlatformGroupMinHeight"] =
                    ((uint objAddress) =>
                    {
                        int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                        float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        return BitfsPlatformGroupTable.GetMinHeight(timer, height);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        return false;
                    }),

                ["BitfsPlatformGroupMaxHeight"] =
                    ((uint objAddress) =>
                    {
                        int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                        float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        return BitfsPlatformGroupTable.GetMaxHeight(timer, height);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        return false;
                    }),

                ["BitfsPlatformGroupRelativeHeight"] =
                    ((uint objAddress) =>
                    {
                        int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                        return BitfsPlatformGroupTable.GetRelativeHeightFromMin(timer);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        return false;
                    }),

                ["BitfsPlatformGroupDisplacedHeight"] =
                    ((uint objAddress) =>
                    {
                        int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                        float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float homeHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        return BitfsPlatformGroupTable.GetDisplacedHeight(timer, height, homeHeight);
                    },
                    (object objectValue, uint objAddress) =>
                    {
                        return false;
                    }),

                // Object specific vars - Ghost

                ["MarioGhostVerticalDistance"] =
                    ((uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float ghostY = Config.Stream.GetSingle(objAddress + ObjectConfig.GraphicsYOffset);
                        float yDiff = marioY - ghostY;
                        return yDiff;
                    }),

                ["MarioGhostHorizontalDistance"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position ghostPos = GetObjectGraphicsPosition(objAddress);
                        double hDistToGhost = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, ghostPos.X, ghostPos.Z);
                        return hDistToGhost;
                    }),

                ["MarioGhostForwardsDistance"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position ghostPos = GetObjectGraphicsPosition(objAddress);
                        double hDistToGhost = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, ghostPos.X, ghostPos.Z);
                        double angleFromGhost = MoreMath.AngleTo_AngleUnits(
                            ghostPos.X, ghostPos.Z, marioPos.X, marioPos.Z);
                        (double movementSideways, double movementForwards) =
                            MoreMath.GetComponentsFromVectorRelatively(
                                hDistToGhost, angleFromGhost, marioPos.Angle.Value);
                        return movementForwards;
                    }),

                ["MarioGhostSidewaysDistance"] =
                    ((uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position ghostPos = GetObjectGraphicsPosition(objAddress);
                        double hDistToGhost = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, ghostPos.X, ghostPos.Z);
                        double angleFromGhost = MoreMath.AngleTo_AngleUnits(
                            ghostPos.X, ghostPos.Z, marioPos.X, marioPos.Z);
                        (double movementSideways, double movementForwards) =
                            MoreMath.GetComponentsFromVectorRelatively(
                                hDistToGhost, angleFromGhost, marioPos.Angle.Value);
                        return movementSideways;
                    }),

                // Mario vars

                ["DeFactoSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetMarioDeFactoSpeed();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newDefactoSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newDefactoSpeedNullable.HasValue) return false;
                        double newDefactoSpeed = newDefactoSpeedNullable.Value;
                        double newHSpeed = newDefactoSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["SlidingSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetMarioSlidingSpeed();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        if (xSlidingSpeed == 0 && zSlidingSpeed == 0) xSlidingSpeed = 1;
                        double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                        double? newHSlidingSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newHSlidingSpeedNullable.HasValue) return false;
                        double newHSlidingSpeed = newHSlidingSpeedNullable.Value;

                        double multiplier = newHSlidingSpeed / hSlidingSpeed;
                        double newXSlidingSpeed = xSlidingSpeed * multiplier;
                        double newZSlidingSpeed = zSlidingSpeed * multiplier;

                        bool success = true;
                        success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        return success;
                    }),

                ["SlidingAngle"] =
                    ((uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        double slidingAngle = MoreMath.AngleTo_AngleUnits(xSlidingSpeed, zSlidingSpeed);
                        return slidingAngle;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                        double? newHSlidingAngleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newHSlidingAngleNullable.HasValue) return false;
                        double newHSlidingAngle = newHSlidingAngleNullable.Value;
                        (double newXSlidingSpeed, double newZSlidingSpeed) =
                            MoreMath.GetComponentsFromVector(hSlidingSpeed, newHSlidingAngle);

                        bool success = true;
                        success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        return success;
                    }),

                ["TrajectoryRemainingHeight"] =
                    ((uint dummy) =>
                    {
                        float vSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                        double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        return remainingHeight;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newRemainingHeightNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newRemainingHeightNullable.HasValue) return false;
                        double newRemainingHeight = newRemainingHeightNullable.Value;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                        return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                    }),

                ["TrajectoryPeakHeight"] =
                    ((uint dummy) =>
                    {
                        float vSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                        double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double peakHeight = marioY + remainingHeight;
                        return peakHeight;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newPeakHeightNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newPeakHeightNullable.HasValue) return false;
                        double newPeakHeight = newPeakHeightNullable.Value;
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newRemainingHeight = newPeakHeight - marioY;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                        return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                    }),

                ["DoubleJumpVerticalSpeed"] =
                    ((uint dummy) =>
                    {
                        float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                        double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                        return vSpeed;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newVSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newVSpeedNullable.HasValue) return false;
                        double newVSpeed = newVSpeedNullable.Value;
                        double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(newVSpeed);
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["DoubleJumpHeight"] =
                    ((uint dummy) =>
                    {
                        float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                        double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                        double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        return doubleJumpHeight;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newHeightNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newHeightNullable.HasValue) return false;
                        double newHeight = newHeightNullable.Value;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                        double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["DoubleJumpPeakHeight"] =
                    ((uint dummy) =>
                    {
                        float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                        double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                        double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double doubleJumpPeakHeight = marioY + doubleJumpHeight;
                        return doubleJumpPeakHeight;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newPeakHeightNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newPeakHeightNullable.HasValue) return false;
                        double newPeakHeight = newPeakHeightNullable.Value;
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newHeight = newPeakHeight - marioY;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                        double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["MovementX"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        return movementX;
                    }),

                ["MovementY"] =
                    ((uint dummy) =>
                    {
                        float endY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        return movementY;
                    }),

                ["MovementZ"] =
                    ((uint dummy) =>
                    {
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        return movementZ;
                    }),

                ["MovementForwards"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        (double movementSideways, double movementForwards) =
                            MoreMath.GetComponentsFromVectorRelatively(movementHorizontal, movementAngle, marioAngle);
                        return movementForwards;
                    }),

                ["MovementSideways"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        (double movementSideways, double movementForwards) =
                            MoreMath.GetComponentsFromVectorRelatively(movementHorizontal, movementAngle, marioAngle);
                        return movementSideways;
                    }),

                ["MovementHorizontal"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        return movementHorizontal;
                    }),

                ["MovementTotal"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementTotal = MoreMath.GetHypotenuse(movementX, movementY, movementZ);
                        return movementTotal;
                    }),

                ["MovementAngle"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        return movementAngle;
                    }),

                ["QFrameCountEstimate"] =
                    ((uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        float oldHSpeed = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x28);
                        double qframes = Math.Abs(Math.Round(Math.Sqrt(movementX * movementX + movementZ * movementZ) / (oldHSpeed / 4)));
                        if (qframes > 4) qframes = double.NaN;
                        return qframes;
                    }),

                ["DeltaYawIntendedFacing"] =
                    ((uint dummy) =>
                    {
                        return GetDeltaYawIntendedFacing();
                    }),

                ["FallHeight"] =
                    ((uint dummy) =>
                    {
                        float peakHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float fallHeight = peakHeight - floorY;
                        return fallHeight;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? fallHeightNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!fallHeightNullable.HasValue) return false;
                        double fallHeight = fallHeightNullable.Value;

                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        double newPeakHeight = floorY + fallHeight;
                        return Config.Stream.SetValue((float)newPeakHeight, MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                    }),

                ["MarioDistanceToHolp"] =
                    ((uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position holpPos = GetHolpPosition();
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, holpPos.X, holpPos.Y, holpPos.Z);
                        return dist;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position holpPos = GetHolpPosition();
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newHolpX, double newHolpY, double newHolpZ) =
                            MoreMath.ExtrapolateLine3D(
                                marioPos.X, marioPos.Y, marioPos.Z, holpPos.X, holpPos.Y, holpPos.Z, distAway);
                        return SetHolpPosition(newHolpX, newHolpY, newHolpZ);
                    }),

                ["MarioHDistanceToHolp"] =
                    ((uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position holpPos = GetHolpPosition();
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, holpPos.X, holpPos.Z);
                        return hDist;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position holpPos = GetHolpPosition();
                        double? distAway = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAway.HasValue) return false;
                        (double newHolpX, double newHolpZ) =
                            MoreMath.ExtrapolateLine2D(
                                marioPos.X, marioPos.Z, holpPos.X, holpPos.Z, distAway.Value);
                        return SetHolpPosition(newHolpX, null, newHolpZ);
                    }),

                // HUD vars

                ["HudTimeText"] =
                    ((uint dummy) =>
                    {
                        ushort time = Config.Stream.GetUInt16(MarioConfig.StructAddress + HudConfig.TimeOffset);
                        int totalDeciSeconds = time / 3;
                        int deciSecondComponent = totalDeciSeconds % 10;
                        int secondComponent = (totalDeciSeconds / 10) % 60;
                        int minuteComponent = (totalDeciSeconds / 600);
                        return minuteComponent + "'" + secondComponent.ToString("D2") + "\"" + deciSecondComponent;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        string timerString = objectValue.ToString();
                        if (timerString == null) return false;
                        if (timerString.Length == 0) timerString = "0" + timerString;
                        if (timerString.Length == 1) timerString = "\"" + timerString;
                        if (timerString.Length == 2) timerString = "0" + timerString;
                        if (timerString.Length == 3) timerString = "0" + timerString;
                        if (timerString.Length == 4) timerString = "'" + timerString;
                        if (timerString.Length == 5) timerString = "0" + timerString;

                        string minuteComponentString = timerString.Substring(0, timerString.Length - 5);
                        string leftMarker = timerString.Substring(timerString.Length - 5, 1);
                        string secondComponentString = timerString.Substring(timerString.Length - 4, 2);
                        string rightMarker = timerString.Substring(timerString.Length - 2, 1);
                        string deciSecondComponentString = timerString.Substring(timerString.Length - 1, 1);

                        if (leftMarker != "\"" && leftMarker != "'" && leftMarker != ".") return false;
                        if (rightMarker != "\"" && rightMarker != "'" && rightMarker != ".") return false;

                        int? minuteComponentNullable = ParsingUtilities.ParseIntNullable(minuteComponentString);
                        int? secondComponentNullable = ParsingUtilities.ParseIntNullable(secondComponentString);
                        int? deciSecondComponentNullable = ParsingUtilities.ParseIntNullable(deciSecondComponentString);

                        if (!minuteComponentNullable.HasValue ||
                            !secondComponentNullable.HasValue ||
                            !deciSecondComponentNullable.HasValue) return false;

                        int totalDeciSeconds =
                            deciSecondComponentNullable.Value +
                            secondComponentNullable.Value * 10 +
                            minuteComponentNullable.Value * 600;

                        int time = totalDeciSeconds * 3;
                        ushort timeUShort = ParsingUtilities.ParseUShortRoundingCapping(time);
                        return Config.Stream.SetValue(timeUShort, MarioConfig.StructAddress + HudConfig.TimeOffset);
                    }),

                // Camera vars

                ["CameraDistanceToMario"] =
                    ((uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position cameraPos = GetCameraPosition();
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, cameraPos.X, cameraPos.Y, cameraPos.Z);
                        return dist;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position cameraPos = GetCameraPosition();
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newCameraX, double newCameraY, double newCameraZ) =
                            MoreMath.ExtrapolateLine3D(
                                marioPos.X, marioPos.Y, marioPos.Z, cameraPos.X, cameraPos.Y, cameraPos.Z, distAway);
                        SetCameraPosition(newCameraX, newCameraY, newCameraZ);
                        return true;
                    }),

                // Triangle vars

                ["Classification"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        return triStruct.Classification.ToString();
                    }),

                ["ClosestVertex"] =
                    ((uint triAddress) =>
                    {
                        return "V" + GetClosestTriangleVertexIndex(triAddress);
                    }),

                ["ClosestVertexX"] =
                    ((uint triAddress) =>
                    {
                        return GetClosestTriangleVertexPosition(triAddress).X;
                    }),

                ["ClosestVertexY"] =
                    ((uint triAddress) =>
                    {
                        return GetClosestTriangleVertexPosition(triAddress).Y;
                    }),

                ["ClosestVertexZ"] =
                    ((uint triAddress) =>
                    {
                        return GetClosestTriangleVertexPosition(triAddress).Z;
                    }),

                ["Steepness"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double steepness = MoreMath.RadiansToAngleUnits(Math.Acos(triStruct.NormY));
                        return steepness;
                    }),

                ["UpHillAngle"] =
                    ((uint triAddress) =>
                    {

                        return GetTriangleUphillAngle(triAddress);
                    }),

                ["DownHillAngle"] =
                    ((uint triAddress) =>
                    {
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        return MoreMath.ReverseAngle(uphillAngle);
                    }),

                ["LeftHillAngle"] =
                    ((uint triAddress) =>
                    {
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        return MoreMath.RotateAngleCCW(uphillAngle, 16384);
                    }),

                ["RightHillAngle"] =
                    ((uint triAddress) =>
                    {
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        return MoreMath.RotateAngleCW(uphillAngle, 16384);
                    }),

                ["UpHillDeltaAngle"] =
                    ((uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double angleDiff = marioAngle - uphillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double newMarioAngleDouble = uphillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["DownHillDeltaAngle"] =
                    ((uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                        double angleDiff = marioAngle - downhillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                        double newMarioAngleDouble = downhillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["LeftHillDeltaAngle"] =
                    ((uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                        double angleDiff = marioAngle - lefthillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                        double newMarioAngleDouble = lefthillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["RightHillDeltaAngle"] =
                    ((uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                        double angleDiff = marioAngle - righthillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                        double newMarioAngleDouble = righthillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["HillStatus"] =
                    ((uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        if (Double.IsNaN(uphillAngle)) return Double.NaN.ToString();
                        double angleDiff = marioAngle - uphillAngle;
                        angleDiff = MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                        bool uphill = angleDiff >= -16384 && angleDiff <= 16384;
                        return uphill ? "Uphill" : "Downhill";
                    }),

                ["DistanceAboveFloor"] =
                    ((uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        return distAboveFloor;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        double? distAboveNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAboveNullable.HasValue) return false;
                        double distAbove = distAboveNullable.Value;
                        double newMarioY = floorY + distAbove;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["DistanceBelowCeiling"] =
                    ((uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float ceilingY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                        float distBelowCeiling = ceilingY - marioY;
                        return distBelowCeiling;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        float ceilingY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                        double? distBelowNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distBelowNullable.HasValue) return false;
                        double distBelow = distBelowNullable.Value;
                        double newMarioY = ceilingY - distBelow;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["NormalDistAway"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double normalDistAway =
                            marioPos.X * triStruct.NormX +
                            marioPos.Y * triStruct.NormY +
                            marioPos.Z * triStruct.NormZ +
                            triStruct.NormOffset;
                        return normalDistAway;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;

                        double missingDist = distAway -
                            marioPos.X * triStruct.NormX -
                            marioPos.Y * triStruct.NormY -
                            marioPos.Z * triStruct.NormZ -
                            triStruct.NormOffset;

                        double xDiff = missingDist * triStruct.NormX;
                        double yDiff = missingDist * triStruct.NormY;
                        double zDiff = missingDist * triStruct.NormZ;

                        double newMarioX = marioPos.X + xDiff;
                        double newMarioY = marioPos.Y + yDiff;
                        double newMarioZ = marioPos.Z + zDiff;

                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["VerticalDistAway"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double verticalDistAway =
                            marioPos.Y + (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                        return verticalDistAway;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAboveNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAboveNullable.HasValue) return false;
                        double distAbove = distAboveNullable.Value;
                        double newMarioY = distAbove - (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["HeightOnSlope"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double heightOnTriangle =
                            (-marioPos.X * triStruct.NormX - marioPos.Z * triStruct.NormZ - triStruct.NormOffset) / triStruct.NormY;
                        return heightOnTriangle;
                    }),

                ["MaxHSpeedUphill"] =
                    ((uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, true, false);
                    }),

                ["MaxHSpeedUphillAtAngle"] =
                    ((uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, true, true);
                    }),

                ["MaxHSpeedDownhill"] =
                    ((uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, false, false);
                    }),

                ["MaxHSpeedDownhillAtAngle"] =
                    ((uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, false, true);
                    }),

                ["ObjectTriCount"] =
                    ((uint dummy) =>
                    {
                        int totalTriangleCount = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
                        int levelTriangleCount = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);
                        int objectTriangleCount = totalTriangleCount - levelTriangleCount;
                        return objectTriangleCount;
                    }),

                ["CurrentTriangleIndex"] =
                    ((uint triAddress) =>
                    {
                        uint triangleListStartAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
                        uint structSize = TriangleConfig.TriangleStructSize;
                        int addressDiff = triAddress >= triangleListStartAddress
                            ? (int)(triAddress - triangleListStartAddress)
                            : (int)(-1 * (triangleListStartAddress - triAddress));
                        int indexGuess = (int)(addressDiff / structSize);
                        if (triangleListStartAddress + indexGuess * structSize == triAddress) return indexGuess;
                        return Double.NaN;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        int? indexNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!indexNullable.HasValue) return false;
                        int index = indexNullable.Value;

                        uint triangleListStartAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
                        uint structSize = TriangleConfig.TriangleStructSize;
                        uint newTriAddress = (uint)(triangleListStartAddress + index * structSize);
                        Config.TriangleManager.SetCustomTriangleAddress(newTriAddress);
                        return true;
                    }),

                ["CurrentTriangleAddress"] =
                    ((uint triAddress) =>
                    {
                        return triAddress;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        uint? addressNullable = ParsingUtilities.ParseUIntNullable(objectValue);
                        if (!addressNullable.HasValue) return false;
                        uint address = addressNullable.Value;

                        Config.TriangleManager.SetCustomTriangleAddress(address);
                        return true;
                    }),

                ["ObjectNodeCount"] =
                    ((uint dummy) =>
                    {
                        int totalNodeCount = Config.Stream.GetInt32(TriangleConfig.TotalNodeCountAddress);
                        int levelNodeCount = Config.Stream.GetInt32(TriangleConfig.LevelNodeCountAddress);
                        int objectNodeCount = totalNodeCount - levelNodeCount;
                        return objectNodeCount;
                    }),

                ["XDistanceToV1"] =
                    ((uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double xDistToV1 = marioX - triStruct.X1;
                        return xDistToV1;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = triStruct.X1 + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["YDistanceToV1"] =
                    ((uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double yDistToV1 = marioY - triStruct.Y1;
                        return yDistToV1;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = triStruct.Y1 + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["ZDistanceToV1"] =
                    ((uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double zDistToV1 = marioZ - triStruct.Z1;
                        return zDistToV1;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = triStruct.Z1 + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["HDistanceToV1"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double hDistToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        return hDistToV1;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(triStruct.X1, triStruct.Z1, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DistanceToV1"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double distToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X1, triStruct.Y1, triStruct.Z1);
                        return distToV1;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                triStruct.X1, triStruct.Y1, triStruct.Z1, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["XDistanceToV2"] =
                    ((uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double xDistToV2 = marioX - triStruct.X2;
                        return xDistToV2;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = triStruct.X2 + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["YDistanceToV2"] =
                    ((uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double yDistToV2 = marioY - triStruct.Y2;
                        return yDistToV2;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = triStruct.Y2 + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["ZDistanceToV2"] =
                    ((uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double zDistToV2 = marioZ - triStruct.Z2;
                        return zDistToV2;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = triStruct.Z2 + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["HDistanceToV2"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double hDistToV2 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        return hDistToV2;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(triStruct.X2, triStruct.Z2, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DistanceToV2"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double distToV2 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X2, triStruct.Y2, triStruct.Z2);
                        return distToV2;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                triStruct.X2, triStruct.Y2, triStruct.Z2, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["XDistanceToV3"] =
                    ((uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double xDistToV3 = marioX - triStruct.X3;
                        return xDistToV3;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = triStruct.X3 + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["YDistanceToV3"] =
                    ((uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double yDistToV3 = marioY - triStruct.Y3;
                        return yDistToV3;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = triStruct.Y3 + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["ZDistanceToV3"] =
                    ((uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double zDistToV3 = marioZ - triStruct.Z3;
                        return zDistToV3;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = triStruct.Z3 + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["HDistanceToV3"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double hDistToV3 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        return hDistToV3;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLine2D(triStruct.X3, triStruct.Z3, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DistanceToV3"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double distToV3 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X3, triStruct.Y3, triStruct.Z3);
                        return distToV3;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                triStruct.X3, triStruct.Y3, triStruct.Z3, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    }),

                ["DistanceToLine12"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 1, 2);
                        return signedDistToLine12;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 1, 2);

                        double? distNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distNullable.HasValue) return false;
                        double dist = distNullable.Value;
                        double missingDist = dist - signedDistToLine12;

                        double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                        (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                        double newMarioX = marioPos.X + xDiff;
                        double newMarioZ = marioPos.Z + zDiff;
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DistanceToLine23"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 2, 3);
                        return signedDistToLine23;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 2, 3);

                        double? distNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distNullable.HasValue) return false;
                        double dist = distNullable.Value;
                        double missingDist = dist - signedDistToLine23;

                        double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                        (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                        double newMarioX = marioPos.X + xDiff;
                        double newMarioZ = marioPos.Z + zDiff;
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DistanceToLine31"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 3, 1);
                        return signedDistToLine31;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 3, 1);

                        double? distNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!distNullable.HasValue) return false;
                        double dist = distNullable.Value;
                        double missingDist = dist - signedDistToLine31;

                        double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                        (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                        double newMarioX = marioPos.X + xDiff;
                        double newMarioZ = marioPos.Z + zDiff;
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["AngleMarioToV1"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        return angleToV1;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DeltaAngleMarioToV1"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["AngleV1ToMario"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, marioPos.X, marioPos.Z);
                        return angleV1ToMario;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["AngleMarioToV2"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV2 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        return angleToV2;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DeltaAngleMarioToV2"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV2 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["AngleV2ToMario"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, marioPos.X, marioPos.Z);
                        return angleV2ToMario;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["AngleMarioToV3"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV3 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        return angleToV3;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["DeltaAngleMarioToV3"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV3 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["AngleV3ToMario"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, marioPos.X, marioPos.Z);
                        return angleV3ToMario;
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    }),

                ["AngleV1ToV2"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        return angleV1ToV2;
                    }),

                ["AngleV2ToV1"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        return angleV2ToV1;
                    }),

                ["AngleV2ToV3"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        return angleV2ToV3;
                    }),

                ["AngleV3ToV2"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        return angleV3ToV2;
                    }),

                ["AngleV1ToV3"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        return angleV1ToV3;
                    }),

                ["AngleV3ToV1"] =
                    ((uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        return angleV3ToV1;
                    }),

                ["DeltaAngleLine12"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleV1ToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double newMarioAngleDouble = angleV1ToV2 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["DeltaAngleLine21"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleV2ToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        double newMarioAngleDouble = angleV2ToV1 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["DeltaAngleLine23"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleV2ToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        double newMarioAngleDouble = angleV2ToV3 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["DeltaAngleLine32"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleV3ToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        double newMarioAngleDouble = angleV3ToV2 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["DeltaAngleLine31"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleV3ToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        double newMarioAngleDouble = angleV3ToV1 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                ["DeltaAngleLine13"] =
                    ((uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleV1ToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    },
                    (object objectValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        double newMarioAngleDouble = angleV1ToV3 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    }),

                // File vars

                ["StarsInFile"] =
                    ((uint fileAddress) =>
                    {
                        return Config.FileManager.CalculateNumStars(fileAddress);
                    }),

                ["ChecksumCalculated"] =
                    ((uint fileAddress) =>
                    {
                        return Config.FileManager.GetChecksum(fileAddress);
                    }),

                // Action vars

                ["ActionDescription"] =
                    ((uint dummy) =>
                    {
                        return TableConfig.MarioActions.GetActionName();
                    }),

                ["PrevActionDescription"] =
                    ((uint dummy) =>
                    {
                        return TableConfig.MarioActions.GetPrevActionName();
                    }),

                ["MarioAnimationDescription"] =
                    ((uint dummy) =>
                    {
                        return TableConfig.MarioAnimations.GetAnimationName();
                    }),

                // Water vars

                ["WaterAboveMedian"] =
                    ((uint dummy) =>
                    {
                        short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                        short waterLevelMedian = Config.Stream.GetInt16(MiscConfig.WaterLevelMedianAddress);
                        double waterAboveMedian = waterLevel - waterLevelMedian;
                        return waterAboveMedian;
                    }),

                ["MarioAboveWater"] =
                    ((uint dummy) =>
                    {
                        short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float marioAboveWater = marioY - waterLevel;
                        return marioAboveWater;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double goalMarioAboveWater = doubleValueNullable.Value;
                        short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double goalMarioY = waterLevel + goalMarioAboveWater;
                        return Config.Stream.SetValue((float)goalMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                // PU vars

                ["MarioXQpuIndex"] =
                    ((uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        int puXIndex = PuUtilities.GetPuIndex(marioX);
                        double qpuXIndex = puXIndex / 4d;
                        return qpuXIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newQpuXIndexNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newQpuXIndexNullable.HasValue) return false;
                        double newQpuXIndex = newQpuXIndexNullable.Value;
                        int newPuXIndex = (int)Math.Round(newQpuXIndex * 4);

                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["MarioYQpuIndex"] =
                    ((uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        int puYIndex = PuUtilities.GetPuIndex(marioY);
                        double qpuYIndex = puYIndex / 4d;
                        return qpuYIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newQpuYIndexNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newQpuYIndexNullable.HasValue) return false;
                        double newQpuYIndex = newQpuYIndexNullable.Value;
                        int newPuYIndex = (int)Math.Round(newQpuYIndex * 4);

                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["MarioZQpuIndex"] =
                    ((uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        int puZIndex = PuUtilities.GetPuIndex(marioZ);
                        double qpuZIndex = puZIndex / 4d;
                        return qpuZIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newQpuZIndexNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newQpuZIndexNullable.HasValue) return false;
                        double newQpuZIndex = newQpuZIndexNullable.Value;
                        int newPuZIndex = (int)Math.Round(newQpuZIndex * 4);

                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["MarioXPuIndex"] =
                    ((uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        int puXIndex = PuUtilities.GetPuIndex(marioX);
                        return puXIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? newPuXIndexNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!newPuXIndexNullable.HasValue) return false;
                        int newPuXIndex = newPuXIndexNullable.Value;

                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["MarioYPuIndex"] =
                    ((uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        int puYIndex = PuUtilities.GetPuIndex(marioY);
                        return puYIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? newPuYIndexNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!newPuYIndexNullable.HasValue) return false;
                        int newPuYIndex = newPuYIndexNullable.Value;

                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["MarioZPuIndex"] =
                    ((uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        int puZIndex = PuUtilities.GetPuIndex(marioZ);
                        return puZIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? newPuZIndexNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!newPuZIndexNullable.HasValue) return false;
                        int newPuZIndex = newPuZIndexNullable.Value;

                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["MarioXPuRelative"] =
                    ((uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double relX = PuUtilities.GetRelativeCoordinate(marioX);
                        return relX;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newRelXNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newRelXNullable.HasValue) return false;
                        double newRelX = newRelXNullable.Value;

                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        int puXIndex = PuUtilities.GetPuIndex(marioX);
                        double newMarioX = PuUtilities.GetCoordinateInPu(newRelX, puXIndex);
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    }),

                ["MarioYPuRelative"] =
                    ((uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double relY = PuUtilities.GetRelativeCoordinate(marioY);
                        return relY;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newRelYNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newRelYNullable.HasValue) return false;
                        double newRelY = newRelYNullable.Value;

                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        int puYIndex = PuUtilities.GetPuIndex(marioY);
                        double newMarioY = PuUtilities.GetCoordinateInPu(newRelY, puYIndex);
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    }),

                ["MarioZPuRelative"] =
                    ((uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double relZ = PuUtilities.GetRelativeCoordinate(marioZ);
                        return relZ;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newRelZNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newRelZNullable.HasValue) return false;
                        double newRelZ = newRelZNullable.Value;

                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        int puZIndex = PuUtilities.GetPuIndex(marioZ);
                        double newMarioZ = PuUtilities.GetCoordinateInPu(newRelZ, puZIndex);
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    }),

                ["DeFactoMultiplier"] =
                    ((uint dummy) =>
                    {
                        return GetDeFactoMultiplier();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        if (distAboveFloor != 0) return false;

                        double? newDeFactoMultiplierNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newDeFactoMultiplierNullable.HasValue) return false;
                        double newDeFactoMultiplier = newDeFactoMultiplierNullable.Value;

                        uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        if (floorTri == 0) return false;
                        return Config.Stream.SetValue((float)newDeFactoMultiplier, floorTri + TriangleOffsetsConfig.NormY);
                    }),

                ["SyncingSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetSyncingSpeed();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        if (distAboveFloor != 0) return false;

                        double? newSyncingSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newSyncingSpeedNullable.HasValue) return false;
                        double newSyncingSpeed = newSyncingSpeedNullable.Value;

                        uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        if (floorTri == 0) return false;
                        double newYnorm = PuUtilities.QpuSpeed / newSyncingSpeed * SpecialConfig.PuHypotenuse;
                        return Config.Stream.SetValue((float)newYnorm, floorTri + TriangleOffsetsConfig.NormY);
                    }),

                ["QpuSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQpuSpeed();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newQpuSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newQpuSpeedNullable.HasValue) return false;
                        double newQpuSpeed = newQpuSpeedNullable.Value;
                        double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["PuSpeed"] =
                    ((uint dummy) =>
                    {
                        double puSpeed = GetQpuSpeed() * 4;
                        return puSpeed;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newPuSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newPuSpeedNullable.HasValue) return false;
                        double newPuSpeed = newPuSpeedNullable.Value;
                        double newQpuSpeed = newPuSpeed / 4;
                        double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["QpuSpeedComponent"] =
                    ((uint dummy) =>
                    {
                        return Math.Round(GetQpuSpeed());
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? newQpuSpeedCompNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!newQpuSpeedCompNullable.HasValue) return false;
                        int newQpuSpeedComp = newQpuSpeedCompNullable.Value;

                        double relativeSpeed = GetRelativePuSpeed();
                        double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["PuSpeedComponent"] =
                    ((uint dummy) =>
                    {
                        return Math.Round(GetQpuSpeed() * 4);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? newPuSpeedCompNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!newPuSpeedCompNullable.HasValue) return false;
                        int newPuSpeedComp = newPuSpeedCompNullable.Value;

                        double newQpuSpeedComp = newPuSpeedComp / 4d;
                        double relativeSpeed = GetRelativePuSpeed();
                        double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["RelativeSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetRelativePuSpeed();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? newRelativeSpeedNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!newRelativeSpeedNullable.HasValue) return false;
                        double newRelativeSpeed = newRelativeSpeedNullable.Value;

                        double puSpeed = GetQpuSpeed() * 4;
                        double puSpeedRounded = Math.Round(puSpeed);
                        double newHSpeed = (puSpeedRounded / 4) * GetSyncingSpeed() + newRelativeSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    }),

                ["Qs1RelativeXSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(1 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, true, true);
                    }),

                ["Qs1RelativeZSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(1 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, false, true);
                    }),

                ["Qs1RelativeIntendedNextX"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(1 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, true, false);
                    }),

                ["Qs1RelativeIntendedNextZ"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(1 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, false, false);
                    }),

                ["Qs2RelativeXSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(2 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, true, true);
                    }),

                ["Qs2RelativeZSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(2 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, false, true);
                    }),

                ["Qs2RelativeIntendedNextX"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(2 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, true, false);
                    }),

                ["Qs2RelativeIntendedNextZ"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(2 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, false, false);
                    }),

                ["Qs3RelativeXSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(3 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, true, true);
                    }),

                ["Qs3RelativeZSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(3 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, false, true);
                    }),

                ["Qs3RelativeIntendedNextX"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(3 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, true, false);
                    }),

                ["Qs3RelativeIntendedNextZ"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(3 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, false, false);
                    }),

                ["Qs4RelativeXSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(4 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, true, true);
                    }),

                ["Qs4RelativeZSpeed"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeSpeed(4 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, false, true);
                    }),

                ["Qs4RelativeIntendedNextX"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(4 / 4d, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, true, false);
                    }),

                ["Qs4RelativeIntendedNextZ"] =
                    ((uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(4 / 4d, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, false, false);
                    }),

                ["PuParams"] =
                    ((uint dummy) =>
                    {
                        return "(" + SpecialConfig.PuParam1 + "," + SpecialConfig.PuParam2 + ")";
                    },
                    (object objectValue, uint dummy) =>
                    {
                        List<string> stringList = ParsingUtilities.ParseStringList(objectValue.ToString());
                        List<int?> intList = stringList.ConvertAll(
                            stringVal => ParsingUtilities.ParseIntNullable(stringVal));
                        if (intList.Count == 1) intList.Insert(0, 0);
                        if (intList.Count != 2 || intList.Exists(intValue => !intValue.HasValue)) return false;
                        SpecialConfig.PuParam1 = intList[0].Value;
                        SpecialConfig.PuParam2 = intList[1].Value;
                        return true;
                    }),

                // Misc vars

                ["RngIndex"] =
                    ((uint dummy) =>
                    {
                        ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                        string rngIndexString = RngIndexer.GetRngIndexString(rngValue);
                        return rngIndexString;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? index = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!index.HasValue) return false;
                        ushort rngValue = RngIndexer.GetRngValue(index.Value);
                        return Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                    }),

                ["RngIndexMod4"] =
                    ((uint dummy) =>
                    {
                        ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                        int rngIndex = RngIndexer.GetRngIndex();
                        return rngIndex % 4;
                    }),

                ["RngCallsPerFrame"] =
                    ((uint dummy) =>
                    {
                        ushort preRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0C);
                        ushort currentRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0E);
                        int rngDiff = RngIndexer.GetRngIndexDiff(preRng, currentRng);
                        return rngDiff;
                    }),

                ["NumberOfLoadedObjects"] =
                    ((uint dummy) =>
                    {
                        return DataModels.ObjectProcessor.ActiveObjectCount;
                    }),

                ["PlayTime"] =
                    ((uint dummy) =>
                    {
                        uint frameConst = 30;
                        uint secondConst = 60;
                        uint minuteConst = 60;
                        uint hourConst = 24;
                        uint dayConst = 365;

                        uint totalFrames = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                        uint totalSeconds = totalFrames / frameConst;
                        uint totalMinutes = totalSeconds / secondConst;
                        uint totalHours = totalMinutes / minuteConst;
                        uint totalDays = totalHours / hourConst;
                        uint totalYears = totalDays / dayConst;

                        uint frames = totalFrames % frameConst;
                        uint seconds = totalSeconds % secondConst;
                        uint minutes = totalMinutes % minuteConst;
                        uint hours = totalHours % hourConst;
                        uint days = totalDays % dayConst;
                        uint years = totalYears;

                        List<uint> values = new List<uint> { years, days, hours, minutes, seconds, frames };
                        int firstNonZeroIndex = values.FindIndex(value => value != 0);
                        if (firstNonZeroIndex == -1) firstNonZeroIndex = values.Count - 1;
                        int numValuesToShow = values.Count - firstNonZeroIndex;

                        StringBuilder builder = new StringBuilder();
                        if (numValuesToShow >= 6) builder.Append(years + "y ");
                        if (numValuesToShow >= 5) builder.Append(days + "d ");
                        if (numValuesToShow >= 4) builder.Append(hours + "h ");
                        if (numValuesToShow >= 3) builder.Append(minutes + "m ");
                        if (numValuesToShow >= 2) builder.Append(seconds + "s ");
                        if (numValuesToShow >= 1) builder.Append(String.Format("{0:D2}", frames) + "f");
                        return builder.ToString();
                    }),

                ["TtcSpeedSettingDescription"] =
                    ((uint dummy) =>
                    {
                        return TtcSpeedSettingUtilities.GetTtcSpeedSettingDescription();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        short? ttcSpeedSettingNullable = TtcSpeedSettingUtilities.GetTtcSpeedSetting(objectValue.ToString());
                        if (!ttcSpeedSettingNullable.HasValue) return false;
                        short ttcSpeedSetting = ttcSpeedSettingNullable.Value;
                        return Config.Stream.SetValue(ttcSpeedSetting, MiscConfig.TtcSpeedSettingAddress);
                    }),

                // Area vars

                ["CurrentAreaIndexMario"] =
                    ((uint dummy) =>
                    {
                        uint currentAreaMario = Config.Stream.GetUInt32(
                            MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                        double currentAreaIndexMario = AreaUtilities.GetAreaIndex(currentAreaMario) ?? Double.NaN;
                        return currentAreaIndexMario;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? intValueNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!intValueNullable.HasValue) return false;
                        int currentAreaIndexMario = intValueNullable.Value;
                        if (currentAreaIndexMario < 0 || currentAreaIndexMario >= 8) return false;
                        uint currentAreaAddressMario = AreaUtilities.GetAreaAddress(currentAreaIndexMario);
                        return Config.Stream.SetValue(
                            currentAreaAddressMario, MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                    }),

                ["CurrentAreaIndex"] =
                    ((uint dummy) =>
                    {
                        uint currentArea = Config.Stream.GetUInt32(AreaConfig.CurrentAreaPointerAddress);
                        double currentAreaIndex = AreaUtilities.GetAreaIndex(currentArea) ?? Double.NaN;
                        return currentAreaIndex;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        int? intValueNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!intValueNullable.HasValue) return false;
                        int currentAreaIndex = intValueNullable.Value;
                        if (currentAreaIndex < 0 || currentAreaIndex >= 8) return false;
                        uint currentAreaAddress = AreaUtilities.GetAreaAddress(currentAreaIndex);
                        return Config.Stream.SetValue(currentAreaAddress, AreaConfig.CurrentAreaPointerAddress);
                    }),

                ["AreaTerrainDescription"] =
                    ((uint dummy) =>
                    {
                        short terrainType = Config.Stream.GetInt16(
                            Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                        string terrainDescription = AreaUtilities.GetTerrainDescription(terrainType);
                        return terrainDescription;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        short? terrainTypeNullable = AreaUtilities.GetTerrainType(objectValue.ToString());
                        if (!terrainTypeNullable.HasValue) return false;
                        short terrainType = terrainTypeNullable.Value;
                        return Config.Stream.SetValue(
                            terrainType, Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                    }),

                // Custom point

                ["SelfPosType"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.SelfPosPA.ToString();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        PositionAngle posAngle = PositionAngle.FromString(objectValue.ToString());
                        if (posAngle == null) return false;
                        SpecialConfig.SelfPosPA = posAngle;
                        return true;
                    }),

                ["SelfX"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.SelfX;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.SelfPosPA.SetX(doubleValue);
                    }),

                ["SelfY"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.SelfY;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.SelfPosPA.SetY(doubleValue);
                    }),

                ["SelfZ"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.SelfZ;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.SelfPosPA.SetZ(doubleValue);
                    }),

                ["SelfAngleType"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.SelfAnglePA.ToString();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        PositionAngle posAngle = PositionAngle.FromString(objectValue.ToString());
                        if (posAngle == null) return false;
                        SpecialConfig.SelfAnglePA = posAngle;
                        return true;
                    }),

                ["SelfAngle"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.SelfAngle;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.SelfAnglePA.SetAngle(doubleValue);
                    }),

                ["PointPosType"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.PointPosPA.ToString();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        PositionAngle posAngle = PositionAngle.FromString(objectValue.ToString());
                        if (posAngle == null) return false;
                        SpecialConfig.PointPosPA = posAngle;
                        return true;
                    }),

                ["PointX"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.PointX;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.PointPosPA.SetX(doubleValue);
                    }),

                ["PointY"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.PointY;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.PointPosPA.SetY(doubleValue);
                    }),

                ["PointZ"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.PointZ;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.PointPosPA.SetZ(doubleValue);
                    }),

                ["PointAngleType"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.PointAnglePA.ToString();
                    },
                    (object objectValue, uint dummy) =>
                    {
                        PositionAngle posAngle = PositionAngle.FromString(objectValue.ToString());
                        if (posAngle == null) return false;
                        SpecialConfig.PointAnglePA = posAngle;
                        return true;
                    }),

                ["PointAngle"] =
                    ((uint dummy) =>
                    {
                        return SpecialConfig.PointAngle;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double doubleValue = doubleValueNullable.Value;
                        return SpecialConfig.PointPosPA.SetAngle(doubleValue);
                    }),

                ["XDistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetXDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetXDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["YDistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetYDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetYDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["ZDistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetZDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetZDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["HDistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetHDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetHDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["DistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["AngleSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetAngleTo(SpecialConfig.SelfPA, SpecialConfig.PointPA, false, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetAngleTo(SpecialConfig.SelfPA, SpecialConfig.PointPA, objectValue);
                    }),

                ["DeltaAngleSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetDAngleTo(SpecialConfig.SelfPA, SpecialConfig.PointPA, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetDAngleTo(SpecialConfig.SelfPA, SpecialConfig.PointPA, objectValue);
                    }),

                ["AnglePointToSelf"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetAngleTo(SpecialConfig.PointPA, SpecialConfig.SelfPA, false, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetAngleTo(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["DeltaAnglePointToSelf"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetDAngleTo(SpecialConfig.PointPA, SpecialConfig.SelfPA, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetDAngleTo(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["DeltaAngleSelfToAngle"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetAngleDifference(SpecialConfig.SelfPA, SpecialConfig.PointPA, false);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetAngleDifference(SpecialConfig.SelfPA, SpecialConfig.PointPA, objectValue);
                    }),

                ["DeltaAngleSelfToAngleTruncated"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetAngleDifference(SpecialConfig.SelfPA, SpecialConfig.PointPA, true);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetAngleDifference(SpecialConfig.SelfPA, SpecialConfig.PointPA, objectValue);
                    }),

                ["FDistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetFDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetFDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                ["SDistanceSelfToPoint"] =
                    ((uint dummy) =>
                    {
                        return PositionAngle.GetSDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA);
                    },
                    (object objectValue, uint dummy) =>
                    {
                        return PositionAngle.SetSDistance(SpecialConfig.PointPA, SpecialConfig.SelfPA, objectValue);
                    }),

                // Mupen vars

                ["MupenLag"] =
                    ((uint objAddress) =>
                    {
                        if (!MupenUtilities.IsUsingMupen()) return Double.NaN;
                        int lag = MupenUtilities.GetLagCount() + SpecialConfig.MupenLagOffset;
                        return lag;
                    },
                    (object objectValue, uint dummy) =>
                    {
                        if (!MupenUtilities.IsUsingMupen()) return false;

                        if (objectValue.ToString().ToLower() == "x")
                        {
                            SpecialConfig.MupenLagOffset = 0;
                            return true;
                        }

                        int? newLagNullable = ParsingUtilities.ParseIntNullable(objectValue);
                        if (!newLagNullable.HasValue) return false;
                        int newLag = newLagNullable.Value;
                        int newLagOffset = newLag - MupenUtilities.GetLagCount();
                        SpecialConfig.MupenLagOffset = newLagOffset;
                        return true;
                    }),
            };
        }

        // Position logic

        public struct Position
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly ushort? Angle;

            public Position(float x, float y, float z, ushort? angle = null)
            {
                X = x;
                Y = y;
                Z = z;
                Angle = angle;
            }
        }

        private static Position GetMarioPosition()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            return new Position(marioX, marioY, marioZ, marioAngle);
        }

        private static bool SetMarioPosition(double? x, double? y, double? z, ushort? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, MarioConfig.StructAddress + MarioConfig.XOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, MarioConfig.StructAddress + MarioConfig.YOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, MarioConfig.StructAddress + MarioConfig.ZOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue(angle.Value, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            return success;
        }

        private static Position GetHolpPosition()
        {
            float holpX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpXOffset);
            float holpY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpYOffset);
            float holpZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HolpZOffset);
            return new Position(holpX, holpY, holpZ);
        }

        private static bool SetHolpPosition(double? x, double? y, double? z)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, MarioConfig.StructAddress + MarioConfig.HolpXOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, MarioConfig.StructAddress + MarioConfig.HolpYOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, MarioConfig.StructAddress + MarioConfig.HolpZOffset);
            return success;
        }

        private static bool SetMarioPositionAndMarioObjectPosition(double? x, double? y, double? z, ushort? angle = null)
        {
            uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
            bool success = true;
            success &= SetMarioPosition(x, y, z, angle);
            success &= SetObjectPosition(marioObjRef, x, y, z, angle);
            return success;
        }

        private static Position GetObjectPosition(uint objAddress)
        {
            float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
            float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
            float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
            ushort objAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
            return new Position(objX, objY, objZ, objAngle);
        }

        private static bool SetObjectPosition(uint objAddress, double? x, double? y, double? z, ushort? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, objAddress + ObjectConfig.XOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, objAddress + ObjectConfig.YOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, objAddress + ObjectConfig.ZOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue(angle.Value, objAddress + ObjectConfig.YawFacingOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue(angle.Value, objAddress + ObjectConfig.YawMovingOffset);
            return success;
        }

        private static Position GetObjectHomePosition(uint objAddress)
        {
            float homeX = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeXOffset);
            float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
            float homeZ = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeZOffset);
            return new Position(homeX, homeY, homeZ);
        }

        private static bool SetObjectHomePosition(uint objAddress, double? x, double? y, double? z)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, objAddress + ObjectConfig.HomeXOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, objAddress + ObjectConfig.HomeYOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, objAddress + ObjectConfig.HomeZOffset);
            return success;
        }

        private static Position GetObjectGraphicsPosition(uint objAddress)
        {
            float graphicsX = Config.Stream.GetSingle(objAddress + ObjectConfig.GraphicsXOffset);
            float graphicsY = Config.Stream.GetSingle(objAddress + ObjectConfig.GraphicsYOffset);
            float graphicsZ = Config.Stream.GetSingle(objAddress + ObjectConfig.GraphicsZOffset);
            ushort graphicsAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.GraphicsYawOffset);
            return new Position(graphicsX, graphicsY, graphicsZ, graphicsAngle);
        }

        private static Position GetCameraPosition()
        {
            return new Position(DataModels.Camera.X, DataModels.Camera.Y, DataModels.Camera.Z, DataModels.Camera.FacingYaw);
        }

        private static void SetCameraPosition(double? x, double? y, double? z, ushort? angle = null)
        {
            if (x.HasValue) DataModels.Camera.X = (float) x.Value;
            if (y.HasValue) DataModels.Camera.Y = (float) y.Value;
            if (z.HasValue) DataModels.Camera.Z = (float) z.Value;
            if (angle.HasValue) DataModels.Camera.FacingYaw = angle.Value;
        }

        private static bool SetSelfPosition(double? x, double? y, double? z)
        {
            bool success = true;
            if (x.HasValue) success &= SpecialConfig.SelfPosPA.SetX(x.Value);
            if (y.HasValue) success &= SpecialConfig.SelfPosPA.SetY(y.Value);
            if (z.HasValue) success &= SpecialConfig.SelfPosPA.SetZ(z.Value);
            return success;
        }

        // Triangle utilitiy methods

        public static int GetClosestTriangleVertexIndex(uint triAddress)
        {
            Position marioPos = GetMarioPosition();
            TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
            double distToV1 = MoreMath.GetDistanceBetween(
                marioPos.X, marioPos.Y, marioPos.Z, triStruct.X1, triStruct.Y1, triStruct.Z1);
            double distToV2 = MoreMath.GetDistanceBetween(
                marioPos.X, marioPos.Y, marioPos.Z, triStruct.X2, triStruct.Y2, triStruct.Z2);
            double distToV3 = MoreMath.GetDistanceBetween(
                marioPos.X, marioPos.Y, marioPos.Z, triStruct.X3, triStruct.Y3, triStruct.Z3);

            if (distToV1 <= distToV2 && distToV1 <= distToV3) return 1;
            else return distToV2 <= distToV3 ? 2 : 3;
        }

        private static Position GetClosestTriangleVertexPosition(uint triAddress)
        {
            int closestTriangleVertexIndex = GetClosestTriangleVertexIndex(triAddress);
            TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
            if (closestTriangleVertexIndex == 1) return new Position(triStruct.X1, triStruct.Y1, triStruct.Z1);
            if (closestTriangleVertexIndex == 2) return new Position(triStruct.X2, triStruct.Y2, triStruct.Z2);
            if (closestTriangleVertexIndex == 3) return new Position(triStruct.X3, triStruct.Y3, triStruct.Z3);
            throw new ArgumentOutOfRangeException();
        }

        private static double GetTriangleUphillAngleRadians(uint triAddress)
        {
            double angle = GetTriangleUphillAngle(triAddress);
            return MoreMath.AngleUnitsToRadians(angle);
        }

        private static double GetTriangleUphillAngle(uint triAddress)
        {
            TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
            double uphillAngle = 32768 + InGameTrigUtilities.InGameAngleTo(triStruct.NormX, triStruct.NormZ);
            if (triStruct.NormX == 0 && triStruct.NormZ == 0) uphillAngle = double.NaN;
            if (triStruct.IsCeiling()) uphillAngle += 32768;
            return MoreMath.NormalizeAngleDouble(uphillAngle);
        }

        private static double GetTriangleUphillAngleRadiansTrue(uint triAddress)
        {
            TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
            double uphillAngleRadians = Math.PI + Math.Atan2(triStruct.NormX, triStruct.NormZ);
            if (triStruct.NormX == 0 && triStruct.NormZ == 0) uphillAngleRadians = double.NaN;
            if (triStruct.IsCeiling()) uphillAngleRadians += Math.PI;
            return uphillAngleRadians;
        }

        private static double GetTriangleUphillAngleTrue(uint triAddress)
        {
            double uphillAngleRadians = GetTriangleUphillAngleRadiansTrue(triAddress);
            return MoreMath.RadiansToAngleUnits(uphillAngleRadians);
        }

        private static double GetMaxHorizontalSpeedOnTriangle(uint triAddress, bool uphill, bool atAngle)
        {
            TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
            double vDist = uphill ? 78 : 100;
            if (atAngle)
            {
                ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                double marioAngleRadians = MoreMath.AngleUnitsToRadians(marioAngle);
                double uphillAngleRadians = GetTriangleUphillAngleRadians(triAddress);
                double deltaAngle = marioAngleRadians - uphillAngleRadians;
                double multiplier = Math.Abs(Math.Cos(deltaAngle));
                vDist /= multiplier;
            }
            double steepnessRadians = Math.Acos(triStruct.NormY);
            double hDist = vDist / Math.Tan(steepnessRadians);
            double hSpeed = hDist * 4 / triStruct.NormY;
            return hSpeed;
        }

        // Mario special methods

        public static double GetMarioSlidingSpeed()
        {
            float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
            float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
            double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);
            return hSlidingSpeed;
        }

        // Object specific utilitiy methods

        private static (double dotProduct, double distToWaypointPlane, double distToWaypoint)
            GetWaypointSpecialVars(uint objAddress)
        {
            float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
            float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
            float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);

            uint prevWaypointAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.WaypointOffset);
            short prevWaypointIndex = Config.Stream.GetInt16(prevWaypointAddress + WaypointConfig.IndexOffset);
            short prevWaypointX = Config.Stream.GetInt16(prevWaypointAddress + WaypointConfig.XOffset);
            short prevWaypointY = Config.Stream.GetInt16(prevWaypointAddress + WaypointConfig.YOffset);
            short prevWaypointZ = Config.Stream.GetInt16(prevWaypointAddress + WaypointConfig.ZOffset);
            uint nextWaypointAddress = prevWaypointAddress + WaypointConfig.StructSize;
            short nextWaypointIndex = Config.Stream.GetInt16(nextWaypointAddress + WaypointConfig.IndexOffset);
            short nextWaypointX = Config.Stream.GetInt16(nextWaypointAddress + WaypointConfig.XOffset);
            short nextWaypointY = Config.Stream.GetInt16(nextWaypointAddress + WaypointConfig.YOffset);
            short nextWaypointZ = Config.Stream.GetInt16(nextWaypointAddress + WaypointConfig.ZOffset);

            float objToWaypointX = nextWaypointX - objX;
            float objToWaypointY = nextWaypointY - objY;
            float objToWaypointZ = nextWaypointZ - objZ;
            float prevToNextX = nextWaypointX - prevWaypointX;
            float prevToNextY = nextWaypointY - prevWaypointY;
            float prevToNextZ = nextWaypointZ - prevWaypointZ;

            double dotProduct = MoreMath.GetDotProduct(objToWaypointX, objToWaypointY, objToWaypointZ, prevToNextX, prevToNextY, prevToNextZ);
            double prevToNextDist = MoreMath.GetDistanceBetween(prevWaypointX, prevWaypointY, prevWaypointZ, nextWaypointX, nextWaypointY, nextWaypointZ);
            double distToWaypointPlane = dotProduct / prevToNextDist;
            double distToWaypoint = MoreMath.GetDistanceBetween(objX, objY, objZ, nextWaypointX, nextWaypointY, nextWaypointZ);

            return (dotProduct, distToWaypointPlane, distToWaypoint);
        }

        private static (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget)
            GetRacingPenguinSpecialVars(uint racingPenguinAddress)
        {
            double marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            double objectY = Config.Stream.GetSingle(racingPenguinAddress + ObjectConfig.YOffset);
            double heightDiff = marioY - objectY;

            uint prevWaypointAddress = Config.Stream.GetUInt32(racingPenguinAddress + ObjectConfig.WaypointOffset);
            short prevWaypointIndex = Config.Stream.GetInt16(prevWaypointAddress);
            double effort = Config.Stream.GetSingle(racingPenguinAddress + ObjectConfig.RacingPenguinEffortOffset);

            double effortTarget;
            double effortChange;
            double minHSpeed = 70;
            if (heightDiff > -100 || prevWaypointIndex >= 35)
            {
                if (prevWaypointIndex >= 35) minHSpeed = 60;
                effortTarget = -500;
                effortChange = 100;
            }
            else
            {
                effortTarget = 1000;
                effortChange = 30;
            }
            effort = MoreMath.MoveNumberTowards(effort, effortTarget, effortChange);

            double hSpeedTarget = (effort - heightDiff) * 0.1;
            hSpeedTarget = MoreMath.Clamp(hSpeedTarget, minHSpeed, 150);

            return (effortTarget, effortChange, minHSpeed, hSpeedTarget);
        }

        private static (double hSpeedTarget, double hSpeedChange)
            GetKoopaTheQuickSpecialVars(uint koopaTheQuickAddress)
        {
            double hSpeedMultiplier = Config.Stream.GetSingle(koopaTheQuickAddress + ObjectConfig.KoopaTheQuickHSpeedMultiplierOffset);
            short pitchToWaypointAngleUnits = Config.Stream.GetInt16(koopaTheQuickAddress + ObjectConfig.PitchToWaypointOffset);
            double pitchToWaypointRadians = MoreMath.AngleUnitsToRadians(pitchToWaypointAngleUnits);

            double hSpeedTarget = hSpeedMultiplier * (Math.Sin(pitchToWaypointRadians) + 1) * 6;
            double hSpeedChange = hSpeedMultiplier * 0.1;

            return (hSpeedTarget, hSpeedChange);
        }

        private static float GetPendulumAmplitude(uint pendulumAddress)
        {
            // Get pendulum variables
            float accelerationDirection = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
            float accelerationMagnitude = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAccelerationMagnitudeOffset);
            float angularVelocity = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAngularVelocityOffset);
            float angle = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAngleOffset);
            float acceleration = accelerationDirection * accelerationMagnitude;

            // Calculate one frame forwards to see if pendulum is speeding up or slowing down
            float nextAccelerationDirection = accelerationDirection;
            if (angle > 0) nextAccelerationDirection = -1;
            if (angle < 0) nextAccelerationDirection = 1;
            float nextAcceleration = nextAccelerationDirection * accelerationMagnitude;
            float nextAngularVelocity = angularVelocity + nextAcceleration;
            float nextAngle = angle + nextAngularVelocity;
            bool speedingUp = Math.Abs(nextAngularVelocity) > Math.Abs(angularVelocity);

            // Calculate duration of speeding up phase
            float inflectionAngle = angle;
            float inflectionAngularVelocity = nextAngularVelocity;
            float speedUpDistance = 0;
            int speedUpDuration = 0;

            if (speedingUp)
            {
                // d = t * v + t(t-1)/2 * a
                // d = tv + (t^2)a/2-ta/2
                // d = t(v-a/2) + (t^2)a/2
                // 0 = (t^2)a/2 + t(v-a/2) + -d
                // t = (-B +- sqrt(B^2 - 4AC)) / (2A)
                float tentativeSlowDownStartAngle = nextAccelerationDirection;
                float tentativeSpeedUpDistance = tentativeSlowDownStartAngle - angle;
                float A = nextAcceleration / 2;
                float B = nextAngularVelocity - nextAcceleration / 2;
                float C = -1 * tentativeSpeedUpDistance;
                double tentativeSpeedUpDuration = (-B + nextAccelerationDirection * Math.Sqrt(B * B - 4 * A * C)) / (2 * A);
                speedUpDuration = (int)Math.Ceiling(tentativeSpeedUpDuration);

                // d = t * v + t(t-1)/2 * a
                speedUpDistance = speedUpDuration * nextAngularVelocity + speedUpDuration * (speedUpDuration - 1) / 2 * nextAcceleration;
                inflectionAngle = angle + speedUpDistance;

                // v_f = v_i + t * a
                inflectionAngularVelocity = nextAngularVelocity + (speedUpDuration - 2) * nextAcceleration;
            }

            // Calculate duration of slowing down phase

            // v_f = v_i + t * a
            // 0 = v_i + t * a
            // t = v_i / a
            int slowDownDuration = (int)Math.Abs(inflectionAngularVelocity / accelerationMagnitude);

            // d = t * (v_i + v_f)/2
            // d = t * (v_i + 0)/2
            // d = t * v_i/2
            float slowDownDistance = (slowDownDuration + 1) * inflectionAngularVelocity / 2;

            // Combine the results from the speeding up phase and the slowing down phase
            float totalDistance = speedUpDistance + slowDownDistance;
            float amplitude = angle + totalDistance;
            return amplitude;
        }

        // PU methods

        private static float GetDeFactoMultiplier()
        {
            uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            float yNorm = floorTri == 0 ? 1 : Config.Stream.GetSingle(floorTri + TriangleOffsetsConfig.NormY);

            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            float distAboveFloor = marioY - floorY;

            float defactoMultiplier = distAboveFloor == 0 ? yNorm : 1;
            return defactoMultiplier;
        }

        public static float GetMarioDeFactoSpeed()
        {
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            float defactoSpeed = hSpeed * GetDeFactoMultiplier();
            return defactoSpeed;
        }

        public static double GetSyncingSpeed()
        {
            return PuUtilities.QpuSpeed / GetDeFactoMultiplier() * SpecialConfig.PuHypotenuse;
        }

        public static double GetQpuSpeed()
        {
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            return hSpeed / GetSyncingSpeed();
        }

        public static double GetRelativePuSpeed()
        {
            double puSpeed = GetQpuSpeed() * 4;
            double puSpeedRounded = Math.Round(puSpeed);
            double relativeSpeed = (puSpeed - puSpeedRounded) / 4 * GetSyncingSpeed() * GetDeFactoMultiplier();
            return relativeSpeed;
        }

        public static (double x, double z) GetIntendedNextPosition(double numFrames)
        {
            double deFactoSpeed = GetMarioDeFactoSpeed();
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioAngleTruncated = MoreMath.NormalizeAngleTruncated(marioAngle);
            (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(deFactoSpeed * numFrames, marioAngleTruncated);

            float currentX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float currentZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            return (currentX + xDiff, currentZ + zDiff);
        }

        private static double GetQsRelativeSpeed(double numFrames, bool xComp)
        {
            uint compOffset = xComp ? MarioConfig.XOffset : MarioConfig.ZOffset;
            float currentComp = Config.Stream.GetSingle(MarioConfig.StructAddress + compOffset);
            double relCurrentComp = PuUtilities.GetRelativeCoordinate(currentComp);
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            double relIntendedComp = PuUtilities.GetRelativeCoordinate(intendedComp);
            double compDiff = relIntendedComp - relCurrentComp;
            return compDiff;
        }
        
        private static double GetQsRelativeIntendedNextComponent(double numFrames, bool xComp)
        {
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            double relIntendedComp = PuUtilities.GetRelativeCoordinate(intendedComp);
            return relIntendedComp;
        }
        
        private static bool GetQsRelativeIntendedNextComponent(object objectValue, double numFrames, bool xComp, bool relativePosition)
        {
            double? newInputNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
            if (!newInputNullable.HasValue) return false;
            double newInput = newInputNullable.Value;

            float currentX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float currentZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float currentComp = xComp ? currentX : currentZ;
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            int intendedPuCompIndex = PuUtilities.GetPuIndex(intendedComp);
            double newRelativeComp = relativePosition ? currentComp + newInput : newInput;
            double newIntendedComp = PuUtilities.GetCoordinateInPu(newRelativeComp, intendedPuCompIndex);

            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            double intendedXComp = xComp ? newIntendedComp : intendedX;
            double intendedZComp = xComp ? intendedZ : newIntendedComp;
            (double newDeFactoSpeed, double newAngle) =
                MoreMath.GetVectorFromCoordinates(
                    currentX, currentZ, intendedXComp, intendedZComp, hSpeed >= 0);
            double newHSpeed = newDeFactoSpeed / GetDeFactoMultiplier() / numFrames;
            ushort newAngleRounded = MoreMath.NormalizeAngleUshort(newAngle);

            bool success = true;
            success &= Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            success &= Config.Stream.SetValue(newAngleRounded, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            return success;
        }

        // Angle methods

        public static short GetDeltaYawIntendedFacing()
        {
            ushort marioYawFacing = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioYawIntended = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
            return MoreMath.GetDeltaAngleTruncated(marioYawFacing, marioYawIntended);
        }

        // Mario trajectory methods

        public static double ConvertDoubleJumpHSpeedToVSpeed(double hSpeed)
        {
            return (hSpeed / 4) + 52;
        }

        public static double ConvertDoubleJumpVSpeedToHSpeed(double vSpeed)
        {
            return (vSpeed - 52) * 4;
        }

        public static double ComputeHeightChangeFromInitialVerticalSpeed(double initialVSpeed)
        {
            int numFrames = (int) Math.Ceiling(initialVSpeed / 4);
            double finalVSpeed = initialVSpeed - (numFrames - 1) * 4;
            double heightChange = numFrames * (initialVSpeed + finalVSpeed) / 2;
            return heightChange;
        }

        public static double ComputeInitialVerticalSpeedFromHeightChange(double heightChange)
        {
            int numFrames = (int) Math.Ceiling((-2 + Math.Sqrt(4 + 8 * heightChange)) / 4);
            double triangleConstant = 2 * numFrames * (numFrames - 1);
            double initialSpeed = (heightChange + triangleConstant) / numFrames;
            return initialSpeed;
        }
    }
}