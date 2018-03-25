using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;

namespace STROOP.Structs
{
    public static class WatchVariableSpecialUtilities
    {
        private readonly static Func<uint, string> DEFAULT_GETTER = (uint address) => "NOT IMPL";
        private readonly static Func<string, uint, bool> DEFAULT_SETTER = (string value, uint address) => false;

        public static (Func<uint, string> getter, Func<string, uint, bool> setter) CreateGetterSetterFunctions(string specialType)
        {
            Func<uint, string> getterFunction = DEFAULT_GETTER;
            Func<string, uint, bool> setterFunction = DEFAULT_SETTER;

            switch (specialType)
            {
                // Object generic vars

                case "MarioDistanceToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                        return dist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "MarioHorizontalDistanceToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        return hDist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? distAway = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAway.HasValue) return false;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway.Value);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "MarioVerticalDistanceToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float yDist = marioY - objY;
                        return yDist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        double? distAbove = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAbove.HasValue) return false;
                        double newMarioY = objY + distAbove.Value;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "MarioDistanceToObjectHome":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, homePos.X, homePos.Y, homePos.Z);
                        return dist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                homePos.X, homePos.Y, homePos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "MarioHorizontalDistanceToObjectHome":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, homePos.X, homePos.Z);
                        return hDist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAway = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAway.HasValue) return false;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(homePos.X, homePos.Z, marioPos.X, marioPos.Z, distAway.Value);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "MarioVerticalDistanceToObjectHome":
                    getterFunction = (uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        float yDist = marioY - homeY;
                        return yDist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        double? distAbove = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAbove.HasValue) return false;
                        double newMarioY = homeY + distAbove.Value;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "ObjectDistanceToHome":
                    getterFunction = (uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double dist = MoreMath.GetDistanceBetween(
                            objPos.X, objPos.Y, objPos.Z, homePos.X, homePos.Y, homePos.Z);
                        return dist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newObjX, double newObjY, double newObjZ) =
                            MoreMath.ExtrapolateLine3D(
                                homePos.X, homePos.Y, homePos.Z, objPos.X, objPos.Y, objPos.Z, distAway);
                        return SetObjectPosition(objAddress, newObjX, newObjY, newObjZ);
                    };
                    break;

                case "HorizontalObjectDistanceToHome":
                    getterFunction = (uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        return hDist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? distAway = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAway.HasValue) return false;
                        (double newObjX, double newObjZ) =
                            MoreMath.ExtrapolateLineHorizontally(homePos.X, homePos.Z, objPos.X, objPos.Z, distAway.Value);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    };
                    break;

                case "VerticalObjectDistanceToHome":
                    getterFunction = (uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        float yDist = objY - homeY;
                        return yDist.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        float homeY = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                        double? distAbove = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAbove.HasValue) return false;
                        double newObjY = homeY + distAbove.Value;
                        return Config.Stream.SetValue((float)newObjY, objAddress + ObjectConfig.YOffset);
                    };
                    break;

                case "AngleObjectToMario":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToMario).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newObjX, double newObjZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                objPos.X, objPos.Z, marioPos.X, marioPos.Z, angle);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    };
                    break;

                case "DeltaAngleObjectToMario":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double angleDiff = objPos.Angle.Value - angleToMario;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double newObjAngleDouble = angleToMario + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    };
                    break;

                case "InGameAngleObjectToMario":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = MoreMath.InGameAngleTo(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToMario).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newObjX, double newObjZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                objPos.X, objPos.Z, marioPos.X, marioPos.Z, angle);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    };
                    break;

                case "InGameDeltaAngleObjectToMario":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToMario = MoreMath.InGameAngleTo(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double angleDiff = objPos.Angle.Value - angleToMario;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToMario = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z);
                        double newObjAngleDouble = angleToMario + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    };
                    break;

                case "AngleMarioToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToObject = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToObject).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, objPos.X, objPos.Z, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DeltaAngleMarioToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToObject = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        double angleDiff = marioPos.Angle.Value - angleToObject;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToObj = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        double newMarioAngleDouble = angleToObj + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "AngleObjectToHome":
                    getterFunction = (uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double angleToHome = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToHome).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newObjX, double newObjZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                objPos.X, objPos.Z, homePos.X, homePos.Z, angle);
                        return SetObjectPosition(objAddress, newObjX, null, newObjZ);
                    };
                    break;

                case "DeltaAngleObjectToHome":
                    getterFunction = (uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double angleToHome = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        double angleDiff = objPos.Angle.Value - angleToHome;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToHome = MoreMath.AngleTo_AngleUnits(
                            objPos.X, objPos.Z, homePos.X, homePos.Z);
                        double newObjAngleDouble = angleToHome + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    };
                    break;

                case "AngleHomeToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double angleHomeToObject = MoreMath.AngleTo_AngleUnits(
                            homePos.X, homePos.Z, objPos.X, objPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleHomeToObject).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        Position objPos = GetObjectPosition(objAddress);
                        Position homePos = GetObjectHomePosition(objAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newHomeX, double newHomeZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                homePos.X, homePos.Z, objPos.X, objPos.Z, angle);
                        return SetObjectHomePosition(objAddress, newHomeX, null, newHomeZ);
                    };
                    break;

                case "MarioHitboxAwayFromObject":
                    getterFunction = (uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjX = Config.Stream.GetSingle(marioObjRef + ObjectConfig.XOffset);
                        float mObjZ = Config.Stream.GetSingle(marioObjRef + ObjectConfig.ZOffset);
                        float mObjHitboxRadius = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxRadius);

                        float objX = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                        float objZ = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                        float objHitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);

                        double marioHitboxAwayFromObject = MoreMath.GetDistanceBetween(mObjX, mObjZ, objX, objZ) - mObjHitboxRadius - objHitboxRadius;
                        return marioHitboxAwayFromObject.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
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
                        double? hitboxDistAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hitboxDistAwayNullable.HasValue) return false;
                        double hitboxDistAway = hitboxDistAwayNullable.Value;
                        double distAway = hitboxDistAway + mObjHitboxRadius + objHitboxRadius;

                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                        return SetMarioPositionAndMarioObjectPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "MarioHitboxAboveObject":
                    getterFunction = (uint objAddress) =>
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
                        return marioHitboxAboveObject.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                        float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                        float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);

                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                        float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                        float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                        double? hitboxDistAboveNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hitboxDistAboveNullable.HasValue) return false;
                        double hitboxDistAbove = hitboxDistAboveNullable.Value;
                        double newMarioY = objHitboxTop + mObjHitboxDownOffset + hitboxDistAbove;
                        return SetMarioPositionAndMarioObjectPosition(null, newMarioY, null);
                    };
                    break;

                case "MarioHitboxBelowObject":
                    getterFunction = (uint objAddress) =>
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
                        return marioHitboxBelowObject.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
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

                        double? hitboxDistBelowNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hitboxDistBelowNullable.HasValue) return false;
                        double hitboxDistBelow = hitboxDistBelowNullable.Value;
                        double newMarioY = objHitboxBottom - (mObjHitboxTop - mObjY) - hitboxDistBelow;
                        return SetMarioPositionAndMarioObjectPosition(null, newMarioY, null);
                    };
                    break;

                case "MarioHitboxOverlapsObject":
                    getterFunction = (uint objAddress) =>
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
                    };
                    break;

                case "ObjectRngCallsPerFrame":
                    getterFunction = (uint objAddress) =>
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
                        return numOfCalls.ToString();
                    };
                    break;

                // Object specific vars - Pendulum

                case "PendulumAmplitude":
                    getterFunction = (uint objAddress) =>
                    {
                        float pendulumAmplitude = GetPendulumAmplitude(objAddress);
                        return pendulumAmplitude.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        double? amplitudeNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!amplitudeNullable.HasValue) return false;
                        double amplitude = amplitudeNullable.Value;
                        float accelerationDirection = amplitude > 0 ? -1 : 1;

                        bool success = true;
                        success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                        success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                        success &= Config.Stream.SetValue((float)amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                        return success;
                    };
                    break;

                case "PendulumSwingIndex":
                    getterFunction = (uint objAddress) =>
                    {
                        string badValue = "Unknown Index";
                        float pendulumAmplitudeFloat = GetPendulumAmplitude(objAddress);
                        int? pendulumAmplitudeIntNullable = ParsingUtilities.ParseIntNullable(pendulumAmplitudeFloat);
                        if (!pendulumAmplitudeIntNullable.HasValue) return badValue;
                        int pendulumAmplitudeInt = pendulumAmplitudeIntNullable.Value;
                        int? pendulumSwingIndexNullable = TableConfig.PendulumSwings.GetPendulumSwingIndex(pendulumAmplitudeInt);
                        if (!pendulumSwingIndexNullable.HasValue) return badValue;
                        int pendulumSwingIndex = pendulumSwingIndexNullable.Value;
                        return pendulumSwingIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        int? indexNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!indexNullable.HasValue) return false;
                        int index = indexNullable.Value;
                        float amplitude = TableConfig.PendulumSwings.GetPendulumAmplitude(index);
                        float accelerationDirection = amplitude > 0 ? -1 : 1;

                        bool success = true;
                        success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                        success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                        success &= Config.Stream.SetValue(amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                        return success;
                    };
                    break;

                // Object specific vars - Waypoint

                case "ObjectDotProductToWaypoint":
                    getterFunction = (uint objAddress) =>
                    {
                        (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                            GetWaypointSpecialVars(objAddress);
                        return dotProduct.ToString();
                    };
                    break;

                case "ObjectDistanceToWaypointPlane":
                    getterFunction = (uint objAddress) =>
                    {
                        (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                            GetWaypointSpecialVars(objAddress);
                        return distToWaypointPlane.ToString();
                    };
                    break;

                case "ObjectDistanceToWaypoint":
                    getterFunction = (uint objAddress) =>
                    {
                        (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                            GetWaypointSpecialVars(objAddress);
                        return distToWaypoint.ToString();
                    };
                    break;

                // Object specific vars - Racing Penguin

                case "RacingPenguinEffortTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return effortTarget.ToString();
                    };
                    break;

                case "RacingPenguinEffortChange":
                    getterFunction = (uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return effortChange.ToString();
                    };
                    break;

                case "RacingPenguinMinHSpeed":
                    getterFunction = (uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return minHSpeed.ToString();
                    };
                    break;

                case "RacingPenguinHSpeedTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        return hSpeedTarget.ToString();
                    };
                    break;

                case "RacingPenguinDiffHSpeedTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                            GetRacingPenguinSpecialVars(objAddress);
                        float hSpeed = Config.Stream.GetSingle(objAddress + ObjectConfig.HSpeedOffset);
                        double hSpeedDiff = hSpeed - hSpeedTarget;
                        return hSpeedDiff.ToString();
                    };
                    break;

                case "RacingPenguinProgress":
                    getterFunction = (uint objAddress) =>
                    {
                        double progress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                        return progress.ToString();
                    };
                    break;
                    /*
                case "RacingPenguinProgressDiff":
                    getterFunction = (uint objAddress) =>
                    {
                        Dictionary<int, TestingManager.VarState> dictionary = Config.TestingManager.VarStateDictionary;
                        var currentTimer = Config.Stream.GetInt32(Config.SwitchRomVersion(0x803493DC, 0x803463EC));
                        if (!dictionary.ContainsKey(currentTimer))
                        {
                            return double.NaN.ToString();
                        }
                        TestingManager.VarState varState = dictionary[currentTimer];
                        if (!(varState is TestingManager.VarStatePenguin))
                        {
                            return double.NaN.ToString();
                        }
                        TestingManager.VarStatePenguin varStatePenguin = varState as TestingManager.VarStatePenguin;
                        double varStateProgress = varStatePenguin.Progress;
                        double currentProgress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                        double progressDiff = currentProgress - varStateProgress;
                        return progressDiff.ToString();
                    };
                    break;
                    */
                    /*
                case "RacingPenguinProgressDiffDelta":
                    getterFunction = (uint objAddress) =>
                    {
                        TestingManager testingManager = TestingManager.Instance;
                        Dictionary<int, TestingManager.VarState> dictionary = testingManager.VarStateDictionary;
                        var currentTimer = Config.Stream.GetInt32(Config.SwitchRomVersion(0x803493DC, 0x803463EC));
                        if (!dictionary.ContainsKey(currentTimer))
                        {
                            newText = "N/A";
                            break;
                        }
                        TestingManager.VarState varState = dictionary[currentTimer];
                        if (!(varState is TestingManager.VarStatePenguin))
                        {
                            newText = "N/A";
                            break;
                        }
                        TestingManager.VarStatePenguin varStatePenguin = varState as TestingManager.VarStatePenguin;
                        double varStateProgress = varStatePenguin.Progress;

                        double currentProgress = Config.RacingPenguinWaypoints.GetProgress(objAddress);
                        double progressDiff = currentProgress - varStateProgress;

                        if (currentTimer != _racingPenguinCurrentTimer)
                        {
                            _racingPenguinPreviousTimer = _racingPenguinCurrentTimer;
                            _racingPenguinPreviousProgressDiff = _racingPenguinCurrentProgressDiff;
                            _racingPenguinCurrentTimer = currentTimer;
                            _racingPenguinCurrentProgressDiff = progressDiff;
                        }

                        newText = Math.Round(_racingPenguinCurrentProgressDiff - _racingPenguinPreviousProgressDiff, 3).ToString();
                        break;
                    };
                    break;
                    */

                // Object specific vars - Koopa the Quick

                case "KoopaTheQuickHSpeedTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                        return hSpeedTarget.ToString();
                    };
                    break;

                case "KoopaTheQuickHSpeedChange":
                    getterFunction = (uint objAddress) =>
                    {
                        (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                        return hSpeedChange.ToString();
                    };
                    break;

                case "KoopaTheQuick1Progress":
                    getterFunction = (uint objAddress) =>
                    {
                        double progress = TableConfig.KoopaTheQuick1Waypoints.GetProgress(objAddress);
                        return progress.ToString();
                    };
                    break;

                case "KoopaTheQuick2Progress":
                    getterFunction = (uint objAddress) =>
                    {
                        double progress = TableConfig.KoopaTheQuick2Waypoints.GetProgress(objAddress);
                        return progress.ToString();
                    };
                    break;

                // Object specific vars - Fly Guy

                case "FlyGuyZone":
                    getterFunction = (uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        double heightDiff = marioY - objY;
                        if (heightDiff < -400) return "Low";
                        if (heightDiff > -200) return "High";
                        return "Medium";
                    };
                    break;

                case "FlyGuyRelativeHeight":
                    getterFunction = (uint objAddress) =>
                    {
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double relativeHeight = TableConfig.FlyGuyData.GetRelativeHeight(oscillationTimer);
                        return relativeHeight.ToString();
                    };
                    break;

                case "FlyGuyNextHeightDiff":
                    getterFunction = (uint objAddress) =>
                    {
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double nextRelativeHeight = TableConfig.FlyGuyData.GetNextHeightDiff(oscillationTimer);
                        return nextRelativeHeight.ToString();
                    };
                    break;

                case "FlyGuyMinHeight":
                    getterFunction = (uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double minHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, objY);
                        return minHeight.ToString();
                    };
                    break;

                case "FlyGuyMaxHeight":
                    getterFunction = (uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                        int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                        double maxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, objY);
                        return maxHeight.ToString();
                    };
                    break;

                // Object specific vars - Bob-omb

                case "BobombBloatSize":
                    getterFunction = (uint objAddress) =>
                    {
                        float scale = Config.Stream.GetSingle(objAddress + ObjectConfig.ScaleWidthOffset);
                        float bloatSize = (scale - 1) * 5;
                        return bloatSize.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        double? bloatSizeNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!bloatSizeNullable.HasValue) return false;
                        double bloatSize = bloatSizeNullable.Value;
                        float scale = (float)(bloatSize / 5 + 1);

                        bool success = true;
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleWidthOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleHeightOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleDepthOffset);
                        return success;
                    };
                    break;

                case "BobombRadius":
                    getterFunction = (uint objAddress) =>
                    {
                        float scale = Config.Stream.GetSingle(objAddress + ObjectConfig.ScaleWidthOffset);
                        float radius = 32 + scale * 65;
                        return radius.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        double? radiusNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!radiusNullable.HasValue) return false;
                        double radius = radiusNullable.Value;
                        float scale = (float)((radius -32) / 65);

                        bool success = true;
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleWidthOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleHeightOffset);
                        success &= Config.Stream.SetValue(scale, objAddress + ObjectConfig.ScaleDepthOffset);
                        return success;
                    };
                    break;

                case "BobombSpaceBetween":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double hDist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        float scale = Config.Stream.GetSingle(objAddress + ObjectConfig.ScaleWidthOffset);
                        float radius = 32 + scale * 65;
                        double spaceBetween = hDist - radius;
                        return spaceBetween.ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        double? spaceBetweenNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!spaceBetweenNullable.HasValue) return false;
                        double spaceBetween = spaceBetweenNullable.Value;
                        float scale = Config.Stream.GetSingle(objAddress + ObjectConfig.ScaleWidthOffset);
                        float radius = 32 + scale * 65;
                        double distAway = spaceBetween + radius;

                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                // Object specific vars - Scuttlebug

                case "ScuttlebugDeltaAngleToTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        ushort facingAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                        ushort targetAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                        int angleDiff = facingAngle - targetAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint objAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        ushort targetAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                        double newObjAngleDouble = targetAngle + angleDiff;
                        ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                        return SetObjectPosition(objAddress, null, null, null, newObjAngleUShort);
                    };
                    break;

                // Object specific vars - Ghost

                case "MarioGhostVerticalDistance":
                    getterFunction = (uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float ghostY = Config.Stream.GetSingle(objAddress + ObjectConfig.GraphicsYOffset);
                        float yDiff = marioY - ghostY;
                        return yDiff.ToString();
                    };
                    break;

                case "MarioGhostHorizontalDistance":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position ghostPos = GetObjectGraphicsPosition(objAddress);
                        double hDistToGhost = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, ghostPos.X, ghostPos.Z);
                        return hDistToGhost.ToString();
                    };
                    break;

                case "MarioGhostForwardsDistance":
                    getterFunction = (uint objAddress) =>
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
                        return movementForwards.ToString();
                    };
                    break;

                case "MarioGhostSidewaysDistance":
                    getterFunction = (uint objAddress) =>
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
                        return movementSideways.ToString();
                    };
                    break;
                    
                // Mario vars

                case "DeFactoSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetMarioDeFactoSpeed().ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newDefactoSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newDefactoSpeedNullable.HasValue) return false;
                        double newDefactoSpeed = newDefactoSpeedNullable.Value;
                        double newHSpeed = newDefactoSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "SlidingSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetMarioSlidingSpeed().ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        if (xSlidingSpeed == 0 && zSlidingSpeed == 0) xSlidingSpeed = 1;
                        double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                        double? newHSlidingSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newHSlidingSpeedNullable.HasValue) return false;
                        double newHSlidingSpeed = newHSlidingSpeedNullable.Value;

                        double multiplier = newHSlidingSpeed / hSlidingSpeed;
                        double newXSlidingSpeed = xSlidingSpeed * multiplier;
                        double newZSlidingSpeed = zSlidingSpeed * multiplier;

                        bool success = true;
                        success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        return success;
                    };
                    break;

                case "SlidingAngle":
                    getterFunction = (uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        double slidingAngle = MoreMath.AngleTo_AngleUnits(xSlidingSpeed, zSlidingSpeed);
                        return slidingAngle.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                        double? newHSlidingAngleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newHSlidingAngleNullable.HasValue) return false;
                        double newHSlidingAngle = newHSlidingAngleNullable.Value;
                        (double newXSlidingSpeed, double newZSlidingSpeed) =
                            MoreMath.GetComponentsFromVector(hSlidingSpeed, newHSlidingAngle);

                        bool success = true;
                        success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                        success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                        return success;
                    };
                    break;

                case "TrajectoryRemainingHeight":
                    getterFunction = (uint dummy) =>
                    {
                        float vSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                        double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        return remainingHeight.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newRemainingHeightNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newRemainingHeightNullable.HasValue) return false;
                        double newRemainingHeight = newRemainingHeightNullable.Value;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                        return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                    };
                    break;

                case "TrajectoryPeakHeight":
                    getterFunction = (uint dummy) =>
                    {
                        float vSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                        double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double peakHeight = marioY + remainingHeight;
                        return peakHeight.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newPeakHeightNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newPeakHeightNullable.HasValue) return false;
                        double newPeakHeight = newPeakHeightNullable.Value;
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newRemainingHeight = newPeakHeight - marioY;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                        return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                    };
                    break;

                case "DoubleJumpVerticalSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                        double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                        return vSpeed.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newVSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newVSpeedNullable.HasValue) return false;
                        double newVSpeed = newVSpeedNullable.Value;
                        double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(newVSpeed);
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "DoubleJumpHeight":
                    getterFunction = (uint dummy) =>
                    {
                        float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                        double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                        double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        return doubleJumpHeight.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newHeightNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newHeightNullable.HasValue) return false;
                        double newHeight = newHeightNullable.Value;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                        double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "DoubleJumpPeakHeight":
                    getterFunction = (uint dummy) =>
                    {
                        float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                        double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                        double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double doubleJumpPeakHeight = marioY + doubleJumpHeight;
                        return doubleJumpPeakHeight.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newPeakHeightNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newPeakHeightNullable.HasValue) return false;
                        double newPeakHeight = newPeakHeightNullable.Value;
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newHeight = newPeakHeight - marioY;
                        double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                        double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "MovementX":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        return movementX.ToString();
                    };
                    break;

                case "MovementY":
                    getterFunction = (uint dummy) =>
                    {
                        float endY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        return movementY.ToString();
                    };
                    break;

                case "MovementZ":
                    getterFunction = (uint dummy) =>
                    {
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        return movementZ.ToString();
                    };
                    break;

                case "MovementForwards":
                    getterFunction = (uint dummy) =>
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
                        return movementForwards.ToString();
                    };
                    break;

                case "MovementSideways":
                    getterFunction = (uint dummy) =>
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
                        return movementSideways.ToString();
                    };
                    break;

                case "MovementHorizontal":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        return movementHorizontal.ToString();
                    };
                    break;

                case "MovementTotal":
                    getterFunction = (uint dummy) =>
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
                        return movementTotal.ToString();
                    };
                    break;

                case "MovementAngle":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        return movementAngle.ToString();
                    };
                    break;

                case "QFrameCountEstimate":
                    getterFunction = (uint dummy) =>
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
                        return qframes.ToString();
                    };
                    break;

                case "DeltaYawIntendedFacing":
                    getterFunction = (uint dummy) =>
                    {
                        return GetDeltaYawIntendedFacing().ToString();
                    };
                    break;

                case "FallHeight":
                    getterFunction = (uint dummy) =>
                    {
                        float peakHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float fallHeight = peakHeight - floorY;
                        return fallHeight.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? fallHeightNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!fallHeightNullable.HasValue) return false;
                        double fallHeight = fallHeightNullable.Value;

                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        double newPeakHeight = floorY + fallHeight;
                        return Config.Stream.SetValue((float)newPeakHeight, MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                    };
                    break;
                    
                // HUD vars

                case "HudTimeText":
                    getterFunction = (uint dummy) =>
                    {
                        ushort time = Config.Stream.GetUInt16(MarioConfig.StructAddress + HudConfig.TimeOffset);
                        int totalDeciSeconds = time / 3;
                        int deciSecondComponent = totalDeciSeconds % 10;
                        int secondComponent = (totalDeciSeconds / 10) % 60;
                        int minuteComponent = (totalDeciSeconds / 600);
                        return minuteComponent + "'" + secondComponent.ToString("D2") + "\"" + deciSecondComponent;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        if (stringValue == null) return false;
                        if (stringValue.Length == 0) stringValue = "0" + stringValue;
                        if (stringValue.Length == 1) stringValue = "\"" + stringValue;
                        if (stringValue.Length == 2) stringValue = "0" + stringValue;
                        if (stringValue.Length == 3) stringValue = "0" + stringValue;
                        if (stringValue.Length == 4) stringValue = "'" + stringValue;
                        if (stringValue.Length == 5) stringValue = "0" + stringValue;

                        string minuteComponentString = stringValue.Substring(0, stringValue.Length - 5);
                        string leftMarker = stringValue.Substring(stringValue.Length - 5, 1);
                        string secondComponentString = stringValue.Substring(stringValue.Length - 4, 2);
                        string rightMarker = stringValue.Substring(stringValue.Length - 2, 1);
                        string deciSecondComponentString = stringValue.Substring(stringValue.Length - 1, 1);

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
                    };
                    break;

                // Camera vars

                case "CameraDistanceToMario":
                    getterFunction = (uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position cameraPos = GetCameraPosition();
                        double dist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, cameraPos.X, cameraPos.Y, cameraPos.Z);
                        return dist.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position cameraPos = GetCameraPosition();
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newCameraX, double newCameraY, double newCameraZ) =
                            MoreMath.ExtrapolateLine3D(
                                marioPos.X, marioPos.Y, marioPos.Z, cameraPos.X, cameraPos.Y, cameraPos.Z, distAway);
                        SetCameraPosition(newCameraX, newCameraY, newCameraZ);
                        return true;
                    };
                    break;

                // Triangle vars

                case "Classification":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        return triStruct.Classification.ToString();
                    };
                    break;

                case "ClosestVertex":
                    getterFunction = (uint triAddress) =>
                    {
                        return "V" + GetClosestTriangleVertexIndex(triAddress);
                    };
                    break;

                case "ClosestVertexX":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetClosestTriangleVertexPosition(triAddress).X.ToString();
                    };
                    break;

                case "ClosestVertexY":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetClosestTriangleVertexPosition(triAddress).Y.ToString();
                    };
                    break;

                case "ClosestVertexZ":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetClosestTriangleVertexPosition(triAddress).Z.ToString();
                    };
                    break;

                case "Steepness":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double steepness = MoreMath.RadiansToAngleUnits(Math.Acos(triStruct.NormY));
                        return steepness.ToString();
                    };
                    break;

                case "UpHillAngle":
                    getterFunction = (uint triAddress) =>
                    {

                        return GetTriangleUphillAngle(triAddress).ToString();
                    };
                    break;

                case "DownHillAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        return MoreMath.ReverseAngle(uphillAngle).ToString();
                    };
                    break;

                case "LeftHillAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        return MoreMath.RotateAngleCCW(uphillAngle, 16384).ToString();
                    };
                    break;

                case "RightHillAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        return MoreMath.RotateAngleCW(uphillAngle, 16384).ToString();
                    };
                    break;

                case "UpHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double angleDiff = marioAngle - uphillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double newMarioAngleDouble = uphillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "DownHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                        double angleDiff = marioAngle - downhillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                        double newMarioAngleDouble = downhillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "LeftHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                        double angleDiff = marioAngle - lefthillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                        double newMarioAngleDouble = lefthillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "RightHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                        double angleDiff = marioAngle - righthillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                        double newMarioAngleDouble = righthillAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "HillStatus":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double angleDiff = marioAngle - uphillAngle;
                        angleDiff = MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                        bool uphill = angleDiff >= -16384 && angleDiff <= 16384;
                        return uphill ? "Uphill" : "Downhill";
                    };
                    break;

                case "DistanceAboveFloor":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        return distAboveFloor.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        double? distAboveNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAboveNullable.HasValue) return false;
                        double distAbove = distAboveNullable.Value;
                        double newMarioY = floorY + distAbove;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "DistanceBelowCeiling":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float ceilingY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                        float distBelowCeiling = ceilingY - marioY;
                        return distBelowCeiling.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        float ceilingY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                        double? distBelowNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distBelowNullable.HasValue) return false;
                        double distBelow = distBelowNullable.Value;
                        double newMarioY = ceilingY - distBelow;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "NormalDistAway":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double normalDistAway =
                            marioPos.X * triStruct.NormX +
                            marioPos.Y * triStruct.NormY +
                            marioPos.Z * triStruct.NormZ +
                            triStruct.NormOffset;
                        return normalDistAway.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
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
                    };
                    break;

                case "VerticalDistAway":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double verticalDistAway =
                            marioPos.Y + (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                        return verticalDistAway.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAboveNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAboveNullable.HasValue) return false;
                        double distAbove = distAboveNullable.Value;
                        double newMarioY = distAbove - (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "HeightOnSlope":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double heightOnTriangle =
                            (-marioPos.X * triStruct.NormX - marioPos.Z * triStruct.NormZ - triStruct.NormOffset) / triStruct.NormY;
                        return heightOnTriangle.ToString();
                    };
                    break;

                case "MaxHorizontalSpeedUphill":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, true, false).ToString();
                    };
                    break;

                case "MaxHorizontalSpeedUphillAtAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, true, true).ToString();
                    };
                    break;

                case "MaxHorizontalSpeedDownhill":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, false, false).ToString();
                    };
                    break;

                case "MaxHorizontalSpeedDownhillAtAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        return GetMaxHorizontalSpeedOnTriangle(triAddress, false, true).ToString();
                    };
                    break;
                    
                case "ObjectTriCount":
                    getterFunction = (uint dummy) =>
                    {
                        int totalTriangleCount = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
                        int levelTriangleCount = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);
                        int objectTriangleCount = totalTriangleCount - levelTriangleCount;
                        return objectTriangleCount.ToString();
                    };
                    break;

                case "ObjectNodeCount":
                    getterFunction = (uint dummy) =>
                    {
                        int totalNodeCount = Config.Stream.GetInt32(TriangleConfig.TotalNodeCountAddress);
                        int levelNodeCount = Config.Stream.GetInt32(TriangleConfig.LevelNodeCountAddress);
                        int objectNodeCount = totalNodeCount - levelNodeCount;
                        return objectNodeCount.ToString();
                    };
                    break;

                case "XDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double xDistToV1 = marioX - triStruct.X1;
                        return xDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = triStruct.X1 + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "YDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double yDistToV1 = marioY - triStruct.Y1;
                        return yDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = triStruct.Y1 + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "ZDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double zDistToV1 = marioZ - triStruct.Z1;
                        return zDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = triStruct.Z1 + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "HDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double hDistToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        return hDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(triStruct.X1, triStruct.Z1, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double distToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X1, triStruct.Y1, triStruct.Z1);
                        return distToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                triStruct.X1, triStruct.Y1, triStruct.Z1, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "XDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double xDistToV2 = marioX - triStruct.X2;
                        return xDistToV2.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = triStruct.X2 + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "YDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double yDistToV2 = marioY - triStruct.Y2;
                        return yDistToV2.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = triStruct.Y2 + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "ZDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double zDistToV2 = marioZ - triStruct.Z2;
                        return zDistToV2.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = triStruct.Z2 + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "HDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double hDistToV2 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        return hDistToV2.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(triStruct.X2, triStruct.Z2, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double distToV2 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X2, triStruct.Y2, triStruct.Z2);
                        return distToV2.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                triStruct.X2, triStruct.Y2, triStruct.Z2, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "XDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double xDistToV3 = marioX - triStruct.X3;
                        return xDistToV3.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = triStruct.X3 + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "YDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double yDistToV3 = marioY - triStruct.Y3;
                        return yDistToV3.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = triStruct.Y3 + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "ZDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double zDistToV3 = marioZ - triStruct.Z3;
                        return zDistToV3.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = triStruct.Z3 + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "HDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double hDistToV3 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        return hDistToV3.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(triStruct.X3, triStruct.Z3, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double distToV3 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X3, triStruct.Y3, triStruct.Z3);
                        return distToV3.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                triStruct.X3, triStruct.Y3, triStruct.Z3, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "DistanceToLine12":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 1, 2);
                        return signedDistToLine12.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 1, 2);

                        double? distNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
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
                    };
                    break;

                case "DistanceToLine23":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 2, 3);
                        return signedDistToLine23.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 2, 3);

                        double? distNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
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
                    };
                    break;

                case "DistanceToLine31":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 3, 1);
                        return signedDistToLine31.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 3, 1);

                        double? distNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
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
                    };
                    break;

                case "AngleMarioToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        return angleToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DeltaAngleMarioToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "AngleV1ToMario":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, marioPos.X, marioPos.Z);
                        return angleV1ToMario.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "AngleMarioToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV2 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        return angleToV2.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DeltaAngleMarioToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV2 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "AngleV2ToMario":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, marioPos.X, marioPos.Z);
                        return angleV2ToMario.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "AngleMarioToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV3 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        return angleToV3.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DeltaAngleMarioToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleToV3 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "AngleV3ToMario":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, marioPos.X, marioPos.Z);
                        return angleV3ToMario.ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "AngleV1ToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        return angleV1ToV2.ToString();
                    };
                    break;

                case "AngleV2ToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        return angleV2ToV1.ToString();
                    };
                    break;

                case "AngleV2ToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        return angleV2ToV3.ToString();
                    };
                    break;

                case "AngleV3ToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        return angleV3ToV2.ToString();
                    };
                    break;

                case "AngleV1ToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        return angleV1ToV3.ToString();
                    };
                    break;

                case "AngleV3ToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        return angleV3ToV1.ToString();
                    };
                    break;

                case "DeltaAngleLine12":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleV1ToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        double newMarioAngleDouble = angleV1ToV2 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "DeltaAngleLine21":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleV2ToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        double newMarioAngleDouble = angleV2ToV1 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "DeltaAngleLine23":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleV2ToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        double newMarioAngleDouble = angleV2ToV3 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "DeltaAngleLine32":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleV3ToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        double newMarioAngleDouble = angleV3ToV2 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "DeltaAngleLine31":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleV3ToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        double newMarioAngleDouble = angleV3ToV1 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "DeltaAngleLine13":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleV1ToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        double newMarioAngleDouble = angleV1ToV3 + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                // Action vars

                case "ActionDescription":
                    getterFunction = (uint dummy) =>
                    {
                        return TableConfig.MarioActions.GetActionName();
                    };
                    break;

                case "PrevActionDescription":
                    getterFunction = (uint dummy) =>
                    {
                        return TableConfig.MarioActions.GetPrevActionName();
                    };
                    break;

                case "MarioAnimationDescription":
                    getterFunction = (uint dummy) =>
                    {
                        return TableConfig.MarioAnimations.GetAnimationName();
                    };
                    break;

                // Water vars

                case "WaterAboveMedian":
                    getterFunction = (uint dummy) =>
                    {
                        short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                        short waterLevelMedian = Config.Stream.GetInt16(MiscConfig.WaterLevelMedianAddress);
                        double waterAboveMedian = waterLevel - waterLevelMedian;
                        return waterAboveMedian.ToString();
                    };
                    break;

                case "MarioAboveWater":
                    getterFunction = (uint dummy) =>
                    {
                        short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float marioAboveWater = marioY - waterLevel;
                        return marioAboveWater.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!doubleValueNullable.HasValue) return false;
                        double goalMarioAboveWater = doubleValueNullable.Value;
                        short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double goalMarioY = waterLevel + goalMarioAboveWater;
                        return Config.Stream.SetValue((float)goalMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                // PU vars

                case "MarioXQpuIndex":
                    getterFunction = (uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        int puXIndex = PuUtilities.GetPuIndex(marioX);
                        double qpuXIndex = puXIndex / 4d;
                        return qpuXIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newQpuXIndexNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newQpuXIndexNullable.HasValue) return false;
                        double newQpuXIndex = newQpuXIndexNullable.Value;
                        int newPuXIndex = (int)Math.Round(newQpuXIndex * 4);
                        
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "MarioYQpuIndex":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        int puYIndex = PuUtilities.GetPuIndex(marioY);
                        double qpuYIndex = puYIndex / 4d;
                        return qpuYIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newQpuYIndexNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newQpuYIndexNullable.HasValue) return false;
                        double newQpuYIndex = newQpuYIndexNullable.Value;
                        int newPuYIndex = (int)Math.Round(newQpuYIndex * 4);

                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "MarioZQpuIndex":
                    getterFunction = (uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        int puZIndex = PuUtilities.GetPuIndex(marioZ);
                        double qpuZIndex = puZIndex / 4d;
                        return qpuZIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newQpuZIndexNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newQpuZIndexNullable.HasValue) return false;
                        double newQpuZIndex = newQpuZIndexNullable.Value;
                        int newPuZIndex = (int)Math.Round(newQpuZIndex * 4);

                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "MarioXPuIndex":
                    getterFunction = (uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        int puXIndex = PuUtilities.GetPuIndex(marioX);
                        return puXIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? newPuXIndexNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!newPuXIndexNullable.HasValue) return false;
                        int newPuXIndex = newPuXIndexNullable.Value;

                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "MarioYPuIndex":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        int puYIndex = PuUtilities.GetPuIndex(marioY);
                        return puYIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? newPuYIndexNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!newPuYIndexNullable.HasValue) return false;
                        int newPuYIndex = newPuYIndexNullable.Value;

                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "MarioZPuIndex":
                    getterFunction = (uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        int puZIndex = PuUtilities.GetPuIndex(marioZ);
                        return puZIndex.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? newPuZIndexNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!newPuZIndexNullable.HasValue) return false;
                        int newPuZIndex = newPuZIndexNullable.Value;

                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "MarioXPuRelative":
                    getterFunction = (uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double relX = PuUtilities.GetRelativeCoordinate(marioX);
                        return relX.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newRelXNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newRelXNullable.HasValue) return false;
                        double newRelX = newRelXNullable.Value;

                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        int puXIndex = PuUtilities.GetPuIndex(marioX);
                        double newMarioX = PuUtilities.GetCoordinateInPu(newRelX, puXIndex);
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "MarioYPuRelative":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double relY = PuUtilities.GetRelativeCoordinate(marioY);
                        return relY.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newRelYNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newRelYNullable.HasValue) return false;
                        double newRelY = newRelYNullable.Value;

                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        int puYIndex = PuUtilities.GetPuIndex(marioY);
                        double newMarioY = PuUtilities.GetCoordinateInPu(newRelY, puYIndex);
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "MarioZPuRelative":
                    getterFunction = (uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double relZ = PuUtilities.GetRelativeCoordinate(marioZ);
                        return relZ.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newRelZNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newRelZNullable.HasValue) return false;
                        double newRelZ = newRelZNullable.Value;

                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        int puZIndex = PuUtilities.GetPuIndex(marioZ);
                        double newMarioZ = PuUtilities.GetCoordinateInPu(newRelZ, puZIndex);
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "DeFactoMultiplier":
                    getterFunction = (uint dummy) =>
                    {
                        return GetDeFactoMultiplier().ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        if (distAboveFloor != 0) return false;

                        double? newDeFactoMultiplierNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newDeFactoMultiplierNullable.HasValue) return false;
                        double newDeFactoMultiplier = newDeFactoMultiplierNullable.Value;

                        uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        if (floorTri == 0) return false;
                        return Config.Stream.SetValue((float)newDeFactoMultiplier, floorTri + TriangleOffsetsConfig.NormY);
                    };
                    break;

                case "SyncingSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetSyncingSpeed().ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        if (distAboveFloor != 0) return false;

                        double? newSyncingSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newSyncingSpeedNullable.HasValue) return false;
                        double newSyncingSpeed = newSyncingSpeedNullable.Value;

                        uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        if (floorTri == 0) return false;
                        double newYnorm = PuUtilities.QpuSpeed / newSyncingSpeed * SpecialConfig.PuHypotenuse;
                        return Config.Stream.SetValue((float)newYnorm, floorTri + TriangleOffsetsConfig.NormY);
                    };
                    break;

                case "QpuSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQpuSpeed().ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newQpuSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newQpuSpeedNullable.HasValue) return false;
                        double newQpuSpeed = newQpuSpeedNullable.Value;
                        double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "PuSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        double puSpeed = GetQpuSpeed() * 4;
                        return puSpeed.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newPuSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newPuSpeedNullable.HasValue) return false;
                        double newPuSpeed = newPuSpeedNullable.Value;
                        double newQpuSpeed = newPuSpeed / 4;
                        double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "QpuSpeedComponent":
                    getterFunction = (uint dummy) =>
                    {
                        return Math.Round(GetQpuSpeed()).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? newQpuSpeedCompNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!newQpuSpeedCompNullable.HasValue) return false;
                        int newQpuSpeedComp = newQpuSpeedCompNullable.Value;

                        double relativeSpeed = GetRelativePuSpeed();
                        double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "PuSpeedComponent":
                    getterFunction = (uint dummy) =>
                    {
                        return Math.Round(GetQpuSpeed() * 4).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? newPuSpeedCompNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!newPuSpeedCompNullable.HasValue) return false;
                        int newPuSpeedComp = newPuSpeedCompNullable.Value;
                        
                        double newQpuSpeedComp = newPuSpeedComp / 4d;
                        double relativeSpeed = GetRelativePuSpeed();
                        double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "RelativeSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetRelativePuSpeed().ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newRelativeSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newRelativeSpeedNullable.HasValue) return false;
                        double newRelativeSpeed = newRelativeSpeedNullable.Value;

                        double puSpeed = GetQpuSpeed() * 4;
                        double puSpeedRounded = Math.Round(puSpeed);
                        double newHSpeed = (puSpeedRounded / 4) * GetSyncingSpeed() + newRelativeSpeed / GetDeFactoMultiplier();
                        return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    };
                    break;

                case "Qs1RelativeXSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(1 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 1 / 4d, true, true);
                    };
                    break;

                case "Qs1RelativeZSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(1 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 1 / 4d, false, true);
                    };
                    break;

                case "Qs1RelativeIntendedNextX":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(1 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 1 / 4d, true, false);
                    };
                    break;

                case "Qs1RelativeIntendedNextZ":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(1 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 1 / 4d, false, false);
                    };
                    break;

                case "Qs2RelativeXSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(2 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 2 / 4d, true, true);
                    };
                    break;

                case "Qs2RelativeZSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(2 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 2 / 4d, false, true);
                    };
                    break;

                case "Qs2RelativeIntendedNextX":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(2 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 2 / 4d, true, false);
                    };
                    break;

                case "Qs2RelativeIntendedNextZ":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(2 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 2 / 4d, false, false);
                    };
                    break;

                case "Qs3RelativeXSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(3 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 3 / 4d, true, true);
                    };
                    break;

                case "Qs3RelativeZSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(3 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 3 / 4d, false, true);
                    };
                    break;

                case "Qs3RelativeIntendedNextX":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(3 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 3 / 4d, true, false);
                    };
                    break;

                case "Qs3RelativeIntendedNextZ":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(3 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 3 / 4d, false, false);
                    };
                    break;

                case "Qs4RelativeXSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(4 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 4 / 4d, true, true);
                    };
                    break;

                case "Qs4RelativeZSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeSpeed(4 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 4 / 4d, false, true);
                    };
                    break;

                case "Qs4RelativeIntendedNextX":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(4 / 4d, true).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 4 / 4d, true, false);
                    };
                    break;

                case "Qs4RelativeIntendedNextZ":
                    getterFunction = (uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(4 / 4d, false).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        return GetQsRelativeIntendedNextComponent(stringValue, 4 / 4d, false, false);
                    };
                    break;

                case "PuParams":
                    getterFunction = (uint dummy) =>
                    {
                        return "(" + SpecialConfig.PuParam1 + "," + SpecialConfig.PuParam2 + ")";
                    };
                    setterFunction = (string puParams, uint dummy) =>
                    {
                        List<string> stringList = ParsingUtilities.ParseStringList(puParams);
                        List<int?> intList = stringList.ConvertAll(
                            stringValue => ParsingUtilities.ParseIntNullable(stringValue));
                        if (intList.Count == 1) intList.Insert(0, 0);
                        if (intList.Count != 2 || intList.Exists(intValue => !intValue.HasValue)) return false;
                        SpecialConfig.PuParam1 = intList[0].Value;
                        SpecialConfig.PuParam2 = intList[1].Value;
                        return true;
                    };
                    break;

                // Misc vars

                case "RngIndex":
                    getterFunction = (uint dummy) =>
                    {
                        ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                        string rngIndexString = RngIndexer.GetRngIndexString(rngValue);
                        return rngIndexString;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? index = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!index.HasValue) return false;
                        ushort rngValue = RngIndexer.GetRngValue(index.Value);
                        return Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                    };
                    break;

                case "RngCallsPerFrame":
                    getterFunction = (uint dummy) =>
                    {
                        ushort preRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0C);
                        ushort currentRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0E);
                        int rngDiff = RngIndexer.GetRngIndexDiff(preRng, currentRng);
                        return rngDiff.ToString();
                    };
                    break;

                case "NumberOfLoadedObjects":
                    getterFunction = (uint dummy) =>
                    {
                        return $"{DataModels.ObjectProcessor.ActiveObjectCount}";
                    };
                    break;

                // Area vars

                case "CurrentAreaIndexMario":
                    getterFunction = (uint dummy) =>
                    {
                        uint currentAreaMario = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                        string currentAreaIndexMario = AreaUtilities.GetAreaIndexString(currentAreaMario);
                        return currentAreaIndexMario;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? intValueNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!intValueNullable.HasValue) return false;
                        int currentAreaIndexMario = intValueNullable.Value;
                        if (currentAreaIndexMario < 0 || currentAreaIndexMario >= 8) return false;
                        uint currentAreaAddressMario = AreaUtilities.GetAreaAddress(currentAreaIndexMario);
                        return Config.Stream.SetValue(currentAreaAddressMario, MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                    };
                    break;

                case "CurrentAreaIndex":
                    getterFunction = (uint dummy) =>
                    {
                        uint currentArea = Config.Stream.GetUInt32(AreaConfig.CurrentAreaPointerAddress);
                        string currentAreaIndex = AreaUtilities.GetAreaIndexString(currentArea);
                        return currentAreaIndex;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? intValueNullable = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!intValueNullable.HasValue) return false;
                        int currentAreaIndex = intValueNullable.Value;
                        if (currentAreaIndex < 0 || currentAreaIndex >= 8) return false;
                        uint currentAreaAddress = AreaUtilities.GetAreaAddress(currentAreaIndex);
                        return Config.Stream.SetValue(currentAreaAddress, AreaConfig.CurrentAreaPointerAddress);
                    };
                    break;

                case "AreaTerrainDescription":
                    getterFunction = (uint dummy) =>
                    {
                        short terrainType = Config.Stream.GetInt16(Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                        string terrainDescription = AreaUtilities.GetTerrainDescription(terrainType);
                        return terrainDescription;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        short? terrainTypeNullable = AreaUtilities.GetTerrainType(stringValue);
                        if (!terrainTypeNullable.HasValue) return false;
                        short terrainType = terrainTypeNullable.Value;
                        return Config.Stream.SetValue(terrainType, Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                    };
                    break;

                // Custom point

                case "PointX":
                    getterFunction = (uint dummy) =>
                    {
                        return SpecialConfig.PointX.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newValueNullable.HasValue) return false;
                        double newValue = newValueNullable.Value;
                        SpecialConfig.PointX = newValue;
                        return true;
                    };
                    break;

                case "PointY":
                    getterFunction = (uint dummy) =>
                    {
                        return SpecialConfig.PointY.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newValueNullable.HasValue) return false;
                        double newValue = newValueNullable.Value;
                        SpecialConfig.PointY = newValue;
                        return true;
                    };
                    break;

                case "PointZ":
                    getterFunction = (uint dummy) =>
                    {
                        return SpecialConfig.PointZ.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newValueNullable.HasValue) return false;
                        double newValue = newValueNullable.Value;
                        SpecialConfig.PointZ = newValue;
                        return true;
                    };
                    break;

                case "PointAngle":
                    getterFunction = (uint dummy) =>
                    {
                        return SpecialConfig.PointAngle.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? newValueNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!newValueNullable.HasValue) return false;
                        double newValue = newValueNullable.Value;
                        SpecialConfig.PointAngle = newValue;
                        return true;
                    };
                    break;

                case "XDistanceToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                        double xDistToV1 = marioX - SpecialConfig.PointX;
                        return xDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? xDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!xDistNullable.HasValue) return false;
                        double xDist = xDistNullable.Value;
                        double newMarioX = SpecialConfig.PointX + xDist;
                        return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                    };
                    break;

                case "YDistanceToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                        double yDistToV1 = marioY - SpecialConfig.PointY;
                        return yDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? yDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!yDistNullable.HasValue) return false;
                        double yDist = yDistNullable.Value;
                        double newMarioY = SpecialConfig.PointY + yDist;
                        return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                    };
                    break;

                case "ZDistanceToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                        double zDistToV1 = marioZ - SpecialConfig.PointZ;
                        return zDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? zDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!zDistNullable.HasValue) return false;
                        double zDist = zDistNullable.Value;
                        double newMarioZ = SpecialConfig.PointZ + zDist;
                        return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                    };
                    break;

                case "HDistanceToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double hDistToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ);
                        return hDistToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double? hDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!hDistNullable.HasValue) return false;
                        double hDist = hDistNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.ExtrapolateLineHorizontally(
                                SpecialConfig.PointX, SpecialConfig.PointZ, marioPos.X, marioPos.Z, hDist);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DistanceToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double distToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z,
                            SpecialConfig.PointX, SpecialConfig.PointY, SpecialConfig.PointZ);
                        return distToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double? distAwayNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!distAwayNullable.HasValue) return false;
                        double distAway = distAwayNullable.Value;
                        (double newMarioX, double newMarioY, double newMarioZ) =
                            MoreMath.ExtrapolateLine3D(
                                SpecialConfig.PointX, SpecialConfig.PointY, SpecialConfig.PointZ,
                                marioPos.X, marioPos.Y, marioPos.Z, distAway);
                        return SetMarioPosition(newMarioX, newMarioY, newMarioZ);
                    };
                    break;

                case "AngleMarioToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ);
                        return angleToV1.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = angleNullable.Value;
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DeltaAngleMarioToPoint":
                    getterFunction = (uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ);
                        double angleDiff = marioPos.Angle.Value - angleToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double angleToVertex = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ);
                        double newMarioAngleDouble = angleToVertex + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "AnglePointToMario":
                    getterFunction = (uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double angleV1ToMario = MoreMath.AngleTo_AngleUnits(
                            SpecialConfig.PointX, SpecialConfig.PointZ, marioPos.X, marioPos.Z);
                        return angleV1ToMario.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double? angleNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleNullable.HasValue) return false;
                        double angle = MoreMath.ReverseAngle(angleNullable.Value);
                        (double newMarioX, double newMarioZ) =
                            MoreMath.RotatePointAboutPointToAngle(
                                marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ, angle);
                        return SetMarioPosition(newMarioX, null, newMarioZ);
                    };
                    break;

                case "DeltaAngleMarioToAngle":
                    getterFunction = (uint dummy) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                        double angleDiff = marioAngle - SpecialConfig.PointAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? angleDiffNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!angleDiffNullable.HasValue) return false;
                        double angleDiff = angleDiffNullable.Value;
                        double newMarioAngleDouble = SpecialConfig.PointAngle + angleDiff;
                        ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                        return Config.Stream.SetValue(
                            newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    };
                    break;

                case "FDistanceToPoint":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double hdist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ);
                        double angleFromPoint = MoreMath.AngleTo_AngleUnits(
                            SpecialConfig.PointX, SpecialConfig.PointZ, marioPos.X, marioPos.Z);
                        (double sidewaysDist, double forwardsDist) =
                            MoreMath.GetComponentsFromVectorRelatively(
                                hdist, angleFromPoint, SpecialConfig.PointAngle);
                        return forwardsDist.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? forwardsDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!forwardsDistNullable.HasValue) return false;
                        double forwardsDist = forwardsDistNullable.Value;
                        Position marioPos = GetMarioPosition();
                        (double newPointX, double newPointZ) =
                            MoreMath.GetRelativelyOffsettedPosition(
                                SpecialConfig.PointX, SpecialConfig.PointZ, SpecialConfig.PointAngle,
                                marioPos.X, marioPos.Z, null, forwardsDist);
                        return SetMarioPosition(newPointX, null, newPointZ);
                    };
                    break;

                case "SDistanceToPoint":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        double hdist = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, SpecialConfig.PointX, SpecialConfig.PointZ);
                        double angleFromPoint = MoreMath.AngleTo_AngleUnits(
                            SpecialConfig.PointX, SpecialConfig.PointZ, marioPos.X, marioPos.Z);
                        (double sidewaysDist, double forwardsDist) =
                            MoreMath.GetComponentsFromVectorRelatively(
                                hdist, angleFromPoint, SpecialConfig.PointAngle);
                        return sidewaysDist.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? sidewaysDistNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!sidewaysDistNullable.HasValue) return false;
                        double sidewaysDist = sidewaysDistNullable.Value;
                        Position marioPos = GetMarioPosition();
                        (double newPointX, double newPointZ) =
                            MoreMath.GetRelativelyOffsettedPosition(
                                SpecialConfig.PointX, SpecialConfig.PointZ, SpecialConfig.PointAngle,
                                marioPos.X, marioPos.Z, sidewaysDist, null);
                        return SetMarioPosition(newPointX, null, newPointZ);
                    };
                    break;

                default:
                    break;
            }

            return (getterFunction, setterFunction);
        }

        // Position logic

        private struct Position
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
            double uphillAngle = 32768 + MoreMath.InGameAngleTo(triStruct.NormX, triStruct.NormZ);
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

        private static double GetDeFactoMultiplier()
        {
            uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            float yNorm = floorTri == 0 ? 1 : Config.Stream.GetSingle(floorTri + TriangleOffsetsConfig.NormY);

            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            float distAboveFloor = marioY - floorY;

            float defactoMultiplier = distAboveFloor == 0 ? yNorm : 1;
            return defactoMultiplier;
        }

        public static double GetMarioDeFactoSpeed()
        {
            float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            double defactoSpeed = hSpeed * GetDeFactoMultiplier();
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
        
        private static bool GetQsRelativeIntendedNextComponent(string stringValue, double numFrames, bool xComp, bool relativePosition)
        {
            double? newInputNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
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
            ushort marioYawFacingTruncated = MoreMath.NormalizeAngleTruncated(marioYawFacing);
            ushort marioYawIntended = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
            ushort marioYawIntendedTruncated = MoreMath.NormalizeAngleTruncated(marioYawIntended);
            int deltaYaw = marioYawIntendedTruncated - marioYawFacingTruncated;
            return MoreMath.NormalizeAngleShort(deltaYaw);
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