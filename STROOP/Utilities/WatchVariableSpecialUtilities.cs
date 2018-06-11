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
        private readonly static Dictionary<string, (Func<uint, object>, Func<object, uint, bool>)> _dictionary;

        static WatchVariableSpecialUtilities()
        {
            _dictionary = new Dictionary<string, (Func<uint, object>, Func<object, uint, bool>)>();
            AddLiteralEntriesToDictionary();
            AddGeneratedEntriesToDictionary();
        }

        public static (Func<uint, object> getter, Func<object, uint, bool> setter)
            CreateGetterSetterFunctions(string specialType)
        {
            if (_dictionary.ContainsKey(specialType))
                return _dictionary[specialType];
            else
                throw new ArgumentOutOfRangeException();
                return (DEFAULT_GETTER, DEFAULT_SETTER);
        }

        public static void AddGeneratedEntriesToDictionary()
        {
            List<Func<uint, PositionAngle>> posAngleFuncs =
                new List<Func<uint, PositionAngle>>()
                {
                    (uint address) => PositionAngle.Custom,
                    (uint address) => PositionAngle.Mario,
                    (uint address) => PositionAngle.Holp,
                    (uint address) => PositionAngle.Camera,
                    (uint address) => PositionAngle.Ghost,
                    (uint address) => PositionAngle.Obj(address),
                    (uint address) => PositionAngle.ObjHome(address),
                    (uint address) => PositionAngle.Tri(address, 1),
                    (uint address) => PositionAngle.Tri(address, 2),
                    (uint address) => PositionAngle.Tri(address, 3),
                    (uint address) => SpecialConfig.PointPA,
                    (uint address) => SpecialConfig.SelfPA,
                };

            List<string> posAngleStrings =
                new List<string>()
                {
                    "Custom",
                    "Mario",
                    "Holp",
                    "Camera",
                    "Ghost",
                    "Obj",
                    "ObjHome",
                    "TriV1",
                    "TriV2",
                    "TriV3",
                    "Point",
                    "Self",
                };

            for (int i = 0; i < posAngleFuncs.Count; i++)
            {
                Func<uint, PositionAngle> func1 = posAngleFuncs[i];
                string string1 = posAngleStrings[i];

                for (int j = 0; j < posAngleFuncs.Count; j++)
                {
                    if (j == i) continue;
                    Func<uint, PositionAngle> func2 = posAngleFuncs[j];
                    string string2 = posAngleStrings[j];

                    List<string> distTypes = new List<string>() { "X", "Y", "Z", "H", "", "F", "S" };
                    List<Func<PositionAngle, PositionAngle, double>> distGetters =
                        new List<Func<PositionAngle, PositionAngle, double>>()
                        {
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetXDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetYDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetZDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetHDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetFDistance(p1, p2),
                            (PositionAngle p1, PositionAngle p2) => PositionAngle.GetSDistance(p1, p2),
                        };
                    List<Func<PositionAngle, PositionAngle, object, bool>> distSetters =
                        new List<Func<PositionAngle, PositionAngle, object, bool>>()
                        {
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetXDistance(p1, p2, v),
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetYDistance(p1, p2, v),
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetZDistance(p1, p2, v),
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetHDistance(p1, p2, v),
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetDistance(p1, p2, v),
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetFDistance(p1, p2, v),
                            (PositionAngle p1, PositionAngle p2, object v) => PositionAngle.SetSDistance(p1, p2, v),
                        };

                    for (int k = 0; k < distTypes.Count; k++)
                    {
                        string distType = distTypes[k];
                        Func<PositionAngle, PositionAngle, double> getter = distGetters[k];
                        Func<PositionAngle, PositionAngle, object, bool> setter = distSetters[k];

                        _dictionary[String.Format("{0}Dist{1}To{2}", distType, string1, string2)] =
                            ((uint address) =>
                            {
                                return getter(func1(address), func2(address));
                            },
                            (object objectValue, uint address) =>
                            {
                                return setter(func1(address), func2(address), objectValue);
                            });
                    }

                    _dictionary[String.Format("Angle{0}To{1}", string1, string2)] =
                        ((uint address) =>
                        {
                            return PositionAngle.GetAngleTo(func1(address), func2(address), null, false);
                        },
                        (object objectValue, uint address) =>
                        {
                            return PositionAngle.SetAngleTo(func1(address), func2(address), objectValue);
                        });

                    _dictionary[String.Format("DAngle{0}To{1}", string1, string2)] =
                        ((uint address) =>
                        {
                            return PositionAngle.GetDAngleTo(func1(address), func2(address), null, false);
                        },
                        (object objectValue, uint address) =>
                        {
                            return PositionAngle.SetDAngleTo(func1(address), func2(address), objectValue);
                        });

                    _dictionary[String.Format("AngleDiff{0}To{1}", string1, string2)] =
                        ((uint address) =>
                        {
                            return PositionAngle.GetAngleDifference(func1(address), func2(address), false);
                        },
                        (object objectValue, uint address) =>
                        {
                            return PositionAngle.SetAngleDifference(func1(address), func2(address), objectValue);
                        });
                }
            }
        }

        public static void AddLiteralEntriesToDictionary()
        {
            _dictionary["MarioHitboxAwayFromObject"] =
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

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double? hitboxDistAwayNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!hitboxDistAwayNullable.HasValue) return false;
                    double hitboxDistAway = hitboxDistAwayNullable.Value;
                    double distAway = hitboxDistAway + mObjHitboxRadius + objHitboxRadius;

                    (double newMarioX, double newMarioZ) =
                        MoreMath.ExtrapolateLine2D(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                    return BoolUtilities.Combine(
                        marioPos.SetValues(x: newMarioX, z: newMarioZ),
                        PositionAngle.MarioObj().SetValues(x: newMarioX, z: newMarioZ));
                });

            _dictionary["MarioHitboxAboveObject"] =
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
                    return BoolUtilities.Combine(
                        PositionAngle.Mario.SetY(newMarioY),
                        PositionAngle.MarioObj().SetY(newMarioY));
                });

            _dictionary["MarioHitboxBelowObject"] =
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
                    return BoolUtilities.Combine(
                        PositionAngle.Mario.SetY(newMarioY),
                        PositionAngle.MarioObj().SetY(newMarioY));
                });

            _dictionary["MarioHitboxOverlapsObject"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["MarioPunchAngleAway"] =
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    ushort angleToObj = InGameTrigUtilities.InGameAngleTo(
                        marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                    double angleDiff = marioPos.Angle - angleToObj;
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

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    ushort angleToObj = InGameTrigUtilities.InGameAngleTo(
                        marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                    double oldAngleDiff = marioPos.Angle - angleToObj;
                    int oldAngleDiffShort = MoreMath.NormalizeAngleShort(oldAngleDiff);
                    int signMultiplier = oldAngleDiffShort >= 0 ? 1 : -1;

                    double angleDiffAbs = angleAway + 0x2AAA;
                    double angleDiff = angleDiffAbs * signMultiplier;
                    double marioAngleDouble = angleToObj + angleDiff;
                    ushort marioAngleUShort = MoreMath.NormalizeAngleUshort(marioAngleDouble);

                    return Config.Stream.SetValue(marioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                });

            _dictionary["ObjectRngCallsPerFrame"] =
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
                },
                DEFAULT_SETTER);

            // Object specific vars - Pendulum

            _dictionary["PendulumAmplitude"] =
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
                });

            _dictionary["PendulumSwingIndex"] =
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
                });

            // Object specific vars - Waypoint

            _dictionary["ObjectDotProductToWaypoint"] =
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return dotProduct;
                },
                DEFAULT_SETTER);

            _dictionary["ObjectDistanceToWaypointPlane"] =
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return distToWaypointPlane;
                },
                DEFAULT_SETTER);

            _dictionary["ObjectDistanceToWaypoint"] =
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return distToWaypoint;
                },
                DEFAULT_SETTER);

            // Object specific vars - Racing Penguin

            _dictionary["RacingPenguinEffortTarget"] =
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return effortTarget;
                },
                DEFAULT_SETTER);

            _dictionary["RacingPenguinEffortChange"] =
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return effortChange;
                },
                DEFAULT_SETTER);

            _dictionary["RacingPenguinMinHSpeed"] =
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return minHSpeed;
                },
                DEFAULT_SETTER);

            _dictionary["RacingPenguinHSpeedTarget"] =
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return hSpeedTarget;
                },
                DEFAULT_SETTER);

            _dictionary["RacingPenguinDiffHSpeedTarget"] =
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    float hSpeed = Config.Stream.GetSingle(objAddress + ObjectConfig.HSpeedOffset);
                    double hSpeedDiff = hSpeed - hSpeedTarget;
                    return hSpeedDiff;
                },
                DEFAULT_SETTER);

            _dictionary["RacingPenguinProgress"] =
                ((uint objAddress) =>
                {
                    double progress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER);

            // Object specific vars - Koopa the Quick

            _dictionary["KoopaTheQuickHSpeedTarget"] =
                ((uint objAddress) =>
                {
                    (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                    return hSpeedTarget;
                },
                DEFAULT_SETTER);

            _dictionary["KoopaTheQuickHSpeedChange"] =
                ((uint objAddress) =>
                {
                    (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                    return hSpeedChange;
                },
                DEFAULT_SETTER);

            _dictionary["KoopaTheQuick1Progress"] =
                ((uint objAddress) =>
                {
                    double progress = TableConfig.KoopaTheQuick1Waypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER);

            _dictionary["KoopaTheQuick2Progress"] =
                ((uint objAddress) =>
                {
                    double progress = TableConfig.KoopaTheQuick2Waypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER);

            // Object specific vars - Fly Guy

            _dictionary["FlyGuyZone"] =
                ((uint objAddress) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double heightDiff = marioY - objY;
                    if (heightDiff < -400) return "Low";
                    if (heightDiff > -200) return "High";
                    return "Medium";
                },
                DEFAULT_SETTER);

            _dictionary["FlyGuyRelativeHeight"] =
                ((uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double relativeHeight = TableConfig.FlyGuyData.GetRelativeHeight(oscillationTimer);
                    return relativeHeight;
                },
                DEFAULT_SETTER);

            _dictionary["FlyGuyNextHeightDiff"] =
                ((uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double nextRelativeHeight = TableConfig.FlyGuyData.GetNextHeightDiff(oscillationTimer);
                    return nextRelativeHeight;
                },
                DEFAULT_SETTER);

            _dictionary["FlyGuyMinHeight"] =
                ((uint objAddress) =>
                {
                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double minHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, objY);
                    return minHeight;
                },
                DEFAULT_SETTER);

            _dictionary["FlyGuyMaxHeight"] =
                ((uint objAddress) =>
                {
                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double maxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, objY);
                    return maxHeight;
                },
                DEFAULT_SETTER);

            // Object specific vars - Bob-omb

            _dictionary["BobombBloatSize"] =
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
                });

            _dictionary["BobombRadius"] =
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
                });

            _dictionary["BobombSpaceBetween"] =
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
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

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    (double newMarioX, double newMarioZ) =
                        MoreMath.ExtrapolateLine2D(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                });

            // Object specific vars - Scuttlebug

            _dictionary["ScuttlebugDeltaAngleToTarget"] =
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
                    return PositionAngle.Obj(objAddress).SetAngle(newObjAngleUShort);
                });

            // Object specific vars - Goomba Triplet Spawner

            _dictionary["GoombaTripletLoadingDistanceDiff"] =
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    double distDiff = dist - 3000;
                    return distDiff;
                },
                (object objectValue, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double? distDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!distDiffNullable.HasValue) return false;
                    double distDiff = distDiffNullable.Value;
                    double distAway = distDiff + 3000;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                });

            _dictionary["GoombaTripletUnloadingDistanceDiff"] =
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    double distDiff = dist - 4000;
                    return distDiff;
                },
                (object objectValue, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double? distDiffNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!distDiffNullable.HasValue) return false;
                    double distDiff = distDiffNullable.Value;
                    double distAway = distDiff + 4000;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                });

            _dictionary["BitfsPlatformGroupMinHeight"] =
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    return BitfsPlatformGroupTable.GetMinHeight(timer, height);
                },
                (object objectValue, uint objAddress) =>
                {
                    return false;
                });

            _dictionary["BitfsPlatformGroupMaxHeight"] =
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    return BitfsPlatformGroupTable.GetMaxHeight(timer, height);
                },
                (object objectValue, uint objAddress) =>
                {
                    return false;
                });

            _dictionary["BitfsPlatformGroupRelativeHeight"] =
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + 0xF4);
                    return BitfsPlatformGroupTable.GetRelativeHeightFromMin(timer);
                },
                (object objectValue, uint objAddress) =>
                {
                    return false;
                });

            _dictionary["BitfsPlatformGroupDisplacedHeight"] =
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
                });

            // Mario vars

            _dictionary["DeFactoSpeed"] =
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
                });

            _dictionary["SlidingSpeed"] =
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
                });

            _dictionary["SlidingAngle"] =
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
                });

            _dictionary["BobombTrajectoryFramesToPoint"] =
                ((uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = SpecialConfig.PointY - holpPos.Y;
                    double frames = GetObjectTrajectoryYDistToFrames(yDist);
                    return frames;
                },
                (object objectValue, uint dummy) =>
                {
                    double? framesNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!framesNullable.HasValue) return false;
                    double frames = framesNullable.Value;
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = GetObjectTrajectoryFramesToYDist(frames);
                    double hDist = Math.Abs(GetBobombTrajectoryFramesToHDist(frames));
                    double newY = SpecialConfig.PointY - yDist;
                    (double newX, double newZ) = MoreMath.ExtrapolateLine2D(
                        SpecialConfig.PointX, SpecialConfig.PointZ, holpPos.X, holpPos.Z, hDist);
                    return PositionAngle.Holp.SetValues(x: newX, y: newY, z: newZ);
                });

            _dictionary["CorkBoxTrajectoryFramesToPoint"] =
                ((uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = SpecialConfig.PointY - holpPos.Y;
                    double frames = GetObjectTrajectoryYDistToFrames(yDist);
                    return frames;
                },
                (object objectValue, uint dummy) =>
                {
                    double? framesNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!framesNullable.HasValue) return false;
                    double frames = framesNullable.Value;
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = GetObjectTrajectoryFramesToYDist(frames);
                    double hDist = Math.Abs(GetCorkBoxTrajectoryFramesToHDist(frames));
                    double newY = SpecialConfig.PointY - yDist;
                    (double newX, double newZ) = MoreMath.ExtrapolateLine2D(
                        SpecialConfig.PointX, SpecialConfig.PointZ, holpPos.X, holpPos.Z, hDist);
                    return PositionAngle.Holp.SetValues(x: newX, y: newY, z: newZ);
                });

            _dictionary["TrajectoryRemainingHeight"] =
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
                });

            _dictionary["TrajectoryPeakHeight"] =
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
                });

            _dictionary["DoubleJumpVerticalSpeed"] =
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
                });

            _dictionary["DoubleJumpHeight"] =
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
                });

            _dictionary["DoubleJumpPeakHeight"] =
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
                });

            _dictionary["MovementX"] =
                ((uint dummy) =>
                {
                    float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                    float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                    float movementX = endX - startX;
                    return movementX;
                },
                DEFAULT_SETTER);

            _dictionary["MovementY"] =
                ((uint dummy) =>
                {
                    float endY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x14);
                    float startY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x20);
                    float movementY = endY - startY;
                    return movementY;
                },
                DEFAULT_SETTER);

            _dictionary["MovementZ"] =
                ((uint dummy) =>
                {
                    float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                    float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                    float movementZ = endZ - startZ;
                    return movementZ;
                },
                DEFAULT_SETTER);

            _dictionary["MovementForwards"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["MovementSideways"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["MovementHorizontal"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["MovementTotal"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["MovementAngle"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["QFrameCountEstimate"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["DeltaYawIntendedFacing"] =
                ((uint dummy) =>
                {
                    return GetDeltaYawIntendedFacing();
                },
                DEFAULT_SETTER);

            _dictionary["FallHeight"] =
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
                });
            
            // HUD vars

            _dictionary["HudTimeText"] =
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
                });

            // Triangle vars

            _dictionary["Classification"] =
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    return triStruct.Classification.ToString();
                },
                DEFAULT_SETTER);

            _dictionary["ClosestVertex"] =
                ((uint triAddress) =>
                {
                    return "V" + GetClosestTriangleVertexIndex(triAddress);
                },
                DEFAULT_SETTER);

            _dictionary["ClosestVertexX"] =
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).X;
                },
                DEFAULT_SETTER);

            _dictionary["ClosestVertexY"] =
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).Y;
                },
                DEFAULT_SETTER);

            _dictionary["ClosestVertexZ"] =
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).Z;
                },
                DEFAULT_SETTER);

            _dictionary["Steepness"] =
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double steepness = MoreMath.RadiansToAngleUnits(Math.Acos(triStruct.NormY));
                    return steepness;
                },
                DEFAULT_SETTER);

            _dictionary["UpHillAngle"] =
                ((uint triAddress) =>
                {

                    return GetTriangleUphillAngle(triAddress);
                },
                DEFAULT_SETTER);

            _dictionary["DownHillAngle"] =
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.ReverseAngle(uphillAngle);
                },
                DEFAULT_SETTER);

            _dictionary["LeftHillAngle"] =
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.RotateAngleCCW(uphillAngle, 16384);
                },
                DEFAULT_SETTER);

            _dictionary["RightHillAngle"] =
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.RotateAngleCW(uphillAngle, 16384);
                },
                DEFAULT_SETTER);

            _dictionary["UpHillDeltaAngle"] =
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
                });

            _dictionary["DownHillDeltaAngle"] =
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
                });

            _dictionary["LeftHillDeltaAngle"] =
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
                });

            _dictionary["RightHillDeltaAngle"] =
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
                });

            _dictionary["HillStatus"] =
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    if (Double.IsNaN(uphillAngle)) return Double.NaN.ToString();
                    double angleDiff = marioAngle - uphillAngle;
                    angleDiff = MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    bool uphill = angleDiff >= -16384 && angleDiff <= 16384;
                    return uphill ? "Uphill" : "Downhill";
                },
                DEFAULT_SETTER);

            _dictionary["DistanceAboveFloor"] =
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
                });

            _dictionary["DistanceBelowCeiling"] =
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
                });

            _dictionary["NormalDistAway"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    PositionAngle marioPos = PositionAngle.Mario;
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

                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                });

            _dictionary["VerticalDistAway"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double verticalDistAway =
                        marioPos.Y + (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return verticalDistAway;
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double? distAboveNullable = ParsingUtilities.ParseDoubleNullable(objectValue);
                    if (!distAboveNullable.HasValue) return false;
                    double distAbove = distAboveNullable.Value;
                    double newMarioY = distAbove - (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                });

            _dictionary["HeightOnTriangle"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double heightOnTriangle =
                        (-marioPos.X * triStruct.NormX - marioPos.Z * triStruct.NormZ - triStruct.NormOffset) / triStruct.NormY;
                    return heightOnTriangle;
                },
                DEFAULT_SETTER);

            _dictionary["MaxHSpeedUphill"] =
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, true, false);
                },
                DEFAULT_SETTER);

            _dictionary["MaxHSpeedUphillAtAngle"] =
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, true, true);
                },
                DEFAULT_SETTER);

            _dictionary["MaxHSpeedDownhill"] =
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, false, false);
                },
                DEFAULT_SETTER);

            _dictionary["MaxHSpeedDownhillAtAngle"] =
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, false, true);
                },
                DEFAULT_SETTER);

            _dictionary["ObjectTriCount"] =
                ((uint dummy) =>
                {
                    int totalTriangleCount = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
                    int levelTriangleCount = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);
                    int objectTriangleCount = totalTriangleCount - levelTriangleCount;
                    return objectTriangleCount;
                },
                DEFAULT_SETTER);

            _dictionary["CurrentTriangleIndex"] =
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
                });

            _dictionary["CurrentTriangleAddress"] =
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
                });

            _dictionary["ObjectNodeCount"] =
                ((uint dummy) =>
                {
                    int totalNodeCount = Config.Stream.GetInt32(TriangleConfig.TotalNodeCountAddress);
                    int levelNodeCount = Config.Stream.GetInt32(TriangleConfig.LevelNodeCountAddress);
                    int objectNodeCount = totalNodeCount - levelNodeCount;
                    return objectNodeCount;
                },
                DEFAULT_SETTER);

            _dictionary["DistanceToLine12"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                });

            _dictionary["DistanceToLine23"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                });

            _dictionary["DistanceToLine31"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    PositionAngle marioPos = PositionAngle.Mario;
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
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                });

            _dictionary["DeltaAngleLine12"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double angleDiff = marioPos.Angle - angleV1ToV2;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                });

            _dictionary["DeltaAngleLine21"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                    double angleDiff = marioPos.Angle - angleV2ToV1;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                });

            _dictionary["DeltaAngleLine23"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    double angleDiff = marioPos.Angle - angleV2ToV3;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                });

            _dictionary["DeltaAngleLine32"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                    double angleDiff = marioPos.Angle - angleV3ToV2;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                });

            _dictionary["DeltaAngleLine31"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    double angleDiff = marioPos.Angle - angleV3ToV1;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                });

            _dictionary["DeltaAngleLine13"] =
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                    double angleDiff = marioPos.Angle - angleV1ToV3;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (object objectValue, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
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
                });

            // File vars

            _dictionary["StarsInFile"] =
                ((uint fileAddress) =>
                {
                    return Config.FileManager.CalculateNumStars(fileAddress);
                },
                DEFAULT_SETTER);

            _dictionary["ChecksumCalculated"] =
                ((uint fileAddress) =>
                {
                    return Config.FileManager.GetChecksum(fileAddress);
                },
                DEFAULT_SETTER);

            // Action vars

            _dictionary["ActionDescription"] =
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetActionName();
                },
                DEFAULT_SETTER);

            _dictionary["PrevActionDescription"] =
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetPrevActionName();
                },
                DEFAULT_SETTER);

            _dictionary["MarioAnimationDescription"] =
                ((uint dummy) =>
                {
                    return TableConfig.MarioAnimations.GetAnimationName();
                },
                DEFAULT_SETTER);

            // Water vars

            _dictionary["WaterAboveMedian"] =
                ((uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    short waterLevelMedian = Config.Stream.GetInt16(MiscConfig.WaterLevelMedianAddress);
                    double waterAboveMedian = waterLevel - waterLevelMedian;
                    return waterAboveMedian;
                },
                DEFAULT_SETTER);

            _dictionary["MarioAboveWater"] =
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
                });

            // PU vars

            _dictionary["MarioXQpuIndex"] =
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
                });

            _dictionary["MarioYQpuIndex"] =
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
                });

            _dictionary["MarioZQpuIndex"] =
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
                });

            _dictionary["MarioXPuIndex"] =
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
                });

            _dictionary["MarioYPuIndex"] =
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
                });

            _dictionary["MarioZPuIndex"] =
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
                });

            _dictionary["MarioXPuRelative"] =
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
                });

            _dictionary["MarioYPuRelative"] =
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
                });

            _dictionary["MarioZPuRelative"] =
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
                });

            _dictionary["DeFactoMultiplier"] =
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
                });

            _dictionary["SyncingSpeed"] =
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
                });

            _dictionary["QpuSpeed"] =
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
                });

            _dictionary["PuSpeed"] =
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
                });

            _dictionary["QpuSpeedComponent"] =
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
                });

            _dictionary["PuSpeedComponent"] =
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
                });

            _dictionary["RelativeSpeed"] =
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
                });

            _dictionary["Qs1RelativeXSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(1 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, true, true);
                });

            _dictionary["Qs1RelativeZSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(1 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, false, true);
                });

            _dictionary["Qs1RelativeIntendedNextX"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(1 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, true, false);
                });

            _dictionary["Qs1RelativeIntendedNextZ"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(1 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 1 / 4d, false, false);
                });

            _dictionary["Qs2RelativeXSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(2 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, true, true);
                });

            _dictionary["Qs2RelativeZSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(2 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, false, true);
                });

            _dictionary["Qs2RelativeIntendedNextX"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(2 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, true, false);
                });

            _dictionary["Qs2RelativeIntendedNextZ"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(2 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 2 / 4d, false, false);
                });

            _dictionary["Qs3RelativeXSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(3 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, true, true);
                });

            _dictionary["Qs3RelativeZSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(3 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, false, true);
                });

            _dictionary["Qs3RelativeIntendedNextX"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(3 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, true, false);
                });

            _dictionary["Qs3RelativeIntendedNextZ"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(3 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 3 / 4d, false, false);
                });

            _dictionary["Qs4RelativeXSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(4 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, true, true);
                });

            _dictionary["Qs4RelativeZSpeed"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(4 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, false, true);
                });

            _dictionary["Qs4RelativeIntendedNextX"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(4 / 4d, true);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, true, false);
                });

            _dictionary["Qs4RelativeIntendedNextZ"] =
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(4 / 4d, false);
                },
                (object objectValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(objectValue, 4 / 4d, false, false);
                });

            _dictionary["PuParams"] =
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
                });

            // Misc vars

            _dictionary["RngIndex"] =
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
                });

            _dictionary["RngIndexMod4"] =
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                    int rngIndex = RngIndexer.GetRngIndex();
                    return rngIndex % 4;
                },
                DEFAULT_SETTER);

            _dictionary["RngCallsPerFrame"] =
                ((uint dummy) =>
                {
                    ushort preRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0C);
                    ushort currentRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0E);
                    int rngDiff = RngIndexer.GetRngIndexDiff(preRng, currentRng);
                    return rngDiff;
                },
                DEFAULT_SETTER);

            _dictionary["NumberOfLoadedObjects"] =
                ((uint dummy) =>
                {
                    return DataModels.ObjectProcessor.ActiveObjectCount;
                },
                DEFAULT_SETTER);

            _dictionary["PlayTime"] =
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
                },
                DEFAULT_SETTER);

            _dictionary["TtcSpeedSettingDescription"] =
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
                });

            // Area vars

            _dictionary["CurrentAreaIndexMario"] =
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
                });

            _dictionary["CurrentAreaIndex"] =
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
                });

            _dictionary["AreaTerrainDescription"] =
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
                });

            // Custom point

            _dictionary["SelfPosType"] =
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
                });

            _dictionary["SelfX"] =
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
                });

            _dictionary["SelfY"] =
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
                });

            _dictionary["SelfZ"] =
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
                });

            _dictionary["SelfAngleType"] =
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
                });

            _dictionary["SelfAngle"] =
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
                });

            _dictionary["PointPosType"] =
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
                });

            _dictionary["PointX"] =
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
                });

            _dictionary["PointY"] =
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
                });

            _dictionary["PointZ"] =
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
                });

            _dictionary["PointAngleType"] =
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
                });

            _dictionary["PointAngle"] =
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
                });

            // Mupen vars

            _dictionary["MupenLag"] =
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
                });
        }

        // Triangle utilitiy methods

        public static int GetClosestTriangleVertexIndex(uint triAddress)
        {
            PositionAngle marioPos = PositionAngle.Mario;
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

        private static PositionAngle GetClosestTriangleVertexPosition(uint triAddress)
        {
            int closestTriangleVertexIndex = GetClosestTriangleVertexIndex(triAddress);
            TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
            if (closestTriangleVertexIndex == 1) return PositionAngle.Tri(triAddress, 1);
            if (closestTriangleVertexIndex == 2) return PositionAngle.Tri(triAddress, 2);
            if (closestTriangleVertexIndex == 3) return PositionAngle.Tri(triAddress, 3);
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

        private static double GetObjectTrajectoryFramesToYDist(double frames)
        {
            bool reflected = false;
            if (frames < 7.5)
            {
                frames = MoreMath.ReflectValueAboutValue(frames, 7.5);
                reflected = true;
            }
            double yDist;
            if (frames <= 38)
            {
                yDist = -1.25 * frames * frames + 18.75 * frames;
            }
            else
            {
                yDist = -75 * (frames - 38) - 1092.5;
            }
            if (reflected) yDist = MoreMath.ReflectValueAboutValue(yDist, 70.3125);
            return yDist;
        }

        private static double GetObjectTrajectoryYDistToFrames(double yDist)
        {
            bool reflected = false;
            if (yDist > 70.3125)
            {
                yDist = MoreMath.ReflectValueAboutValue(yDist, 70.3125);
                reflected = true;
            }
            double frames;
            if (yDist >= -1092.5)
            {
                double radicand = 351.5625 - 5 * yDist;
                frames = 7.5 + 0.4 * Math.Sqrt(radicand);
            }
            else
            {
                frames = (yDist + 1092.5) / -75 + 38;
            }
            if (reflected) frames = MoreMath.ReflectValueAboutValue(frames, 7.5);
            return frames;
        }

        private static double GetBobombTrajectoryFramesToHDist(double frames)
        {
            return 32 + frames * 25;
        }

        private static double GetBobombTrajectoryHDistToFrames(double hDist)
        {
            return (hDist - 32) / 25;
        }

        private static double GetCorkBoxTrajectoryFramesToHDist(double frames)
        {
            return 32 + frames * 40;
        }

        private static double GetCorkBoxTrajectoryHDistToFrames(double hDist)
        {
            return (hDist - 32) / 40;
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