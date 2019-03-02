using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
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
        private readonly static WatchVariableSpecialDictionary _dictionary;

        static WatchVariableSpecialUtilities()
        {
            _dictionary = new WatchVariableSpecialDictionary();
            AddLiteralEntriesToDictionary();
            AddGeneratedEntriesToDictionary();
        }

        public static (Func<uint, object> getter, Func<object, uint, bool> setter)
            CreateGetterSetterFunctions(string specialType)
        {
            if (_dictionary.ContainsKey(specialType))
                return _dictionary.Get(specialType);
            else
                throw new ArgumentOutOfRangeException();
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
                    (uint address) => PositionAngle.Obj(address),
                    (uint address) => PositionAngle.ObjHome(address),
                    (uint address) => PositionAngle.Ghost(),
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
                    "Obj",
                    "ObjHome",
                    "Ghost",
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
                    List<Func<PositionAngle, PositionAngle, double, bool>> distSetters =
                        new List<Func<PositionAngle, PositionAngle, double, bool>>()
                        {
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetXDistance(p1, p2, dist),
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetYDistance(p1, p2, dist),
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetZDistance(p1, p2, dist),
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetHDistance(p1, p2, dist),
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetDistance(p1, p2, dist),
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetFDistance(p1, p2, dist),
                            (PositionAngle p1, PositionAngle p2, double dist) => PositionAngle.SetSDistance(p1, p2, dist),
                        };

                    for (int k = 0; k < distTypes.Count; k++)
                    {
                        string distType = distTypes[k];
                        Func<PositionAngle, PositionAngle, double> getter = distGetters[k];
                        Func<PositionAngle, PositionAngle, double, bool> setter = distSetters[k];

                        _dictionary.Add(String.Format("{0}Dist{1}To{2}", distType, string1, string2),
                            ((uint address) =>
                            {
                                return getter(func1(address), func2(address));
                            },
                            (double dist, uint address) =>
                            {
                                return setter(func1(address), func2(address), dist);
                            }));
                    }

                    _dictionary.Add(String.Format("Angle{0}To{1}", string1, string2),
                        ((uint address) =>
                        {
                            return PositionAngle.GetAngleTo(func1(address), func2(address), null, false);
                        },
                        (double angle, uint address) =>
                        {
                            return PositionAngle.SetAngleTo(func1(address), func2(address), angle);
                        }));

                    _dictionary.Add(String.Format("DAngle{0}To{1}", string1, string2),
                        ((uint address) =>
                        {
                            return PositionAngle.GetDAngleTo(func1(address), func2(address), null, false);
                        },
                        (double angleDiff, uint address) =>
                        {
                            return PositionAngle.SetDAngleTo(func1(address), func2(address), angleDiff);
                        }));

                    _dictionary.Add(String.Format("AngleDiff{0}To{1}", string1, string2),
                        ((uint address) =>
                        {
                            return PositionAngle.GetAngleDifference(func1(address), func2(address), false);
                        },
                        (double angleDiff, uint address) =>
                        {
                            return PositionAngle.SetAngleDifference(func1(address), func2(address), angleDiff);
                        }));
                }
            }
        }

        public static void AddLiteralEntriesToDictionary()
        {
            // Object vars

            _dictionary.Add("MarioHitboxAwayFromObject",
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
                (double hitboxDistAway, uint objAddress) =>
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
                    double distAway = hitboxDistAway + mObjHitboxRadius + objHitboxRadius;

                    (double newMarioX, double newMarioZ) =
                        MoreMath.ExtrapolateLine2D(objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                    return BoolUtilities.Combine(
                        marioPos.SetValues(x: newMarioX, z: newMarioZ),
                        PositionAngle.MarioObj().SetValues(x: newMarioX, z: newMarioZ));
                }));

            _dictionary.Add("MarioHitboxAboveObject",
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
                (double hitboxDistAbove, uint objAddress) =>
                {
                    uint marioObjRef = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                    float mObjY = Config.Stream.GetSingle(marioObjRef + ObjectConfig.YOffset);
                    float mObjHitboxDownOffset = Config.Stream.GetSingle(marioObjRef + ObjectConfig.HitboxDownOffset);

                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    float objHitboxHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxHeight);
                    float objHitboxDownOffset = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxDownOffset);
                    float objHitboxTop = objY + objHitboxHeight - objHitboxDownOffset;

                    double newMarioY = objHitboxTop + mObjHitboxDownOffset + hitboxDistAbove;
                    return BoolUtilities.Combine(
                        PositionAngle.Mario.SetY(newMarioY),
                        PositionAngle.MarioObj().SetY(newMarioY));
                }));

            _dictionary.Add("MarioHitboxBelowObject",
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
                (double hitboxDistBelow, uint objAddress) =>
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

                    double newMarioY = objHitboxBottom - (mObjHitboxTop - mObjY) - hitboxDistBelow;
                    return BoolUtilities.Combine(
                        PositionAngle.Mario.SetY(newMarioY),
                        PositionAngle.MarioObj().SetY(newMarioY));
                }));

            _dictionary.Add("MarioHitboxOverlapsObject",
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
                    return overlap ? 1 : 0;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioPunchAngleAway",
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
                (double angleAway, uint objAddress) =>
                {
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
                }));

            _dictionary.Add("ObjectRngCallsPerFrame",
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
                DEFAULT_SETTER));

            _dictionary.Add("ObjectProcessGroup",
                ((uint processGroupUint) =>
                {
                    sbyte processGroupByte = processGroupUint == uint.MaxValue ? (sbyte)(-1) : (sbyte)processGroupUint;
                    return processGroupByte;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectProcessGroupDescription",
                ((uint processGroupUint) =>
                {
                    return ProcessGroupUtilities.GetProcessGroupDescription(processGroupUint);
                },
                DEFAULT_SETTER));

            // Object specific vars - Pendulum

            _dictionary.Add("PendulumCountdown",
                ((uint objAddress) =>
                {
                    int pendulumCountdown = GetPendulumCountdown(objAddress);
                    return pendulumCountdown;
                },
                DEFAULT_SETTER));

            _dictionary.Add("PendulumAmplitude",
                ((uint objAddress) =>
                {
                    float pendulumAmplitude = GetPendulumAmplitude(objAddress);
                    return pendulumAmplitude;
                },
                (double amplitude, uint objAddress) =>
                {
                    float accelerationDirection = amplitude > 0 ? -1 : 1;

                    bool success = true;
                    success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                    success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                    success &= Config.Stream.SetValue((float)amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                    return success;
                }));

            _dictionary.Add("PendulumSwingIndex",
                ((uint objAddress) =>
                {
                    float pendulumAmplitudeFloat = GetPendulumAmplitude(objAddress);
                    int? pendulumAmplitudeIntNullable = ParsingUtilities.ParseIntNullable(pendulumAmplitudeFloat);
                    if (!pendulumAmplitudeIntNullable.HasValue) return Double.NaN;
                    int pendulumAmplitudeInt = pendulumAmplitudeIntNullable.Value;
                    return TableConfig.PendulumSwings.GetPendulumSwingIndexExtended(pendulumAmplitudeInt);
                },
                (int index, uint objAddress) =>
                {
                    float amplitude = TableConfig.PendulumSwings.GetPendulumAmplitude(index);
                    float accelerationDirection = amplitude > 0 ? -1 : 1;

                    bool success = true;
                    success &= Config.Stream.SetValue(accelerationDirection, objAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
                    success &= Config.Stream.SetValue(0f, objAddress + ObjectConfig.PendulumAngularVelocityOffset);
                    success &= Config.Stream.SetValue(amplitude, objAddress + ObjectConfig.PendulumAngleOffset);
                    return success;
                }));

            // Object specific vars - Cog

            _dictionary.Add("CogCountdown",
                ((uint objAddress) =>
                {
                    int cogCountdown = GetCogNumFramesInRotation(objAddress);
                    return cogCountdown;
                },
                DEFAULT_SETTER));

            _dictionary.Add("CogEndingYaw",
                ((uint objAddress) =>
                {
                    ushort cogEndingYaw = GetCogEndingYaw(objAddress);
                    return cogEndingYaw;
                },
                DEFAULT_SETTER));

            _dictionary.Add("CogRotationIndex",
                ((uint objAddress) =>
                {
                    ushort yawFacing = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                    double rotationIndex = CogUtilities.GetRotationIndex(yawFacing) ?? Double.NaN;
                    return rotationIndex;
                },
                DEFAULT_SETTER));

            // Object specific vars - Waypoint

            _dictionary.Add("ObjectDotProductToWaypoint",
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return dotProduct;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectDistanceToWaypointPlane",
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return distToWaypointPlane;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectDistanceToWaypoint",
                ((uint objAddress) =>
                {
                    (double dotProduct, double distToWaypointPlane, double distToWaypoint) =
                        GetWaypointSpecialVars(objAddress);
                    return distToWaypoint;
                },
                DEFAULT_SETTER));

            // Object specific vars - Racing Penguin

            _dictionary.Add("RacingPenguinEffortTarget",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return effortTarget;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinEffortChange",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return effortChange;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinMinHSpeed",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return minHSpeed;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinHSpeedTarget",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    return hSpeedTarget;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinDiffHSpeedTarget",
                ((uint objAddress) =>
                {
                    (double effortTarget, double effortChange, double minHSpeed, double hSpeedTarget) =
                        GetRacingPenguinSpecialVars(objAddress);
                    float hSpeed = Config.Stream.GetSingle(objAddress + ObjectConfig.HSpeedOffset);
                    double hSpeedDiff = hSpeed - hSpeedTarget;
                    return hSpeedDiff;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RacingPenguinProgress",
                ((uint objAddress) =>
                {
                    double progress = TableConfig.RacingPenguinWaypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER));

            // Object specific vars - Koopa the Quick

            _dictionary.Add("KoopaTheQuickHSpeedTarget",
                ((uint objAddress) =>
                {
                    (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                    return hSpeedTarget;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuickHSpeedChange",
                ((uint objAddress) =>
                {
                    (double hSpeedTarget, double hSpeedChange) = GetKoopaTheQuickSpecialVars(objAddress);
                    return hSpeedChange;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuick1Progress",
                ((uint objAddress) =>
                {
                    double progress = TableConfig.KoopaTheQuick1Waypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER));

            _dictionary.Add("KoopaTheQuick2Progress",
                ((uint objAddress) =>
                {
                    double progress = TableConfig.KoopaTheQuick2Waypoints.GetProgress(objAddress);
                    return progress;
                },
                DEFAULT_SETTER));

            // Object specific vars - Fly Guy

            _dictionary.Add("FlyGuyZone",
                ((uint objAddress) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double heightDiff = marioY - objY;
                    if (heightDiff < -400) return "Low";
                    if (heightDiff > -200) return "High";
                    return "Medium";
                },
                DEFAULT_SETTER));

            _dictionary.Add("FlyGuyRelativeHeight",
                ((uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double relativeHeight = TableConfig.FlyGuyData.GetRelativeHeight(oscillationTimer);
                    return relativeHeight;
                },
                DEFAULT_SETTER));

            _dictionary.Add("FlyGuyMinHeight",
                ((uint objAddress) =>
                {
                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double minHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, objY);
                    return minHeight;
                },
                (double newMinHeight, uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    float oldHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double oldMinHeight = TableConfig.FlyGuyData.GetMinHeight(oscillationTimer, oldHeight);
                    double heightDiff = newMinHeight - oldMinHeight;
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("FlyGuyMaxHeight",
                ((uint objAddress) =>
                {
                    float objY = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    double maxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, objY);
                    return maxHeight;
                },
                (double newMaxHeight, uint objAddress) =>
                {
                    int oscillationTimer = Config.Stream.GetInt32(objAddress + ObjectConfig.FlyGuyOscillationTimerOffset);
                    float oldHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double oldMaxHeight = TableConfig.FlyGuyData.GetMaxHeight(oscillationTimer, oldHeight);
                    double heightDiff = newMaxHeight - oldMaxHeight;
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("FlyGuyActivationDistanceDiff",
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    double distDiff = dist - 4000;
                    return distDiff;
                },
                (double distDiff, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double distAway = distDiff + 4000;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                }));

            // Object specific vars - Bob-omb

            _dictionary.Add("BobombBloatSize",
                ((uint objAddress) =>
                {
                    float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                    float bloatSize = (hitboxRadius - 65) / 13;
                    return bloatSize;
                },
                (float bloatSize, uint objAddress) =>
                {
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
                }));

            _dictionary.Add("BobombRadius",
                ((uint objAddress) =>
                {
                    float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                    float radius = hitboxRadius + 32;
                    return radius;
                },
                (float radius, uint objAddress) =>
                {
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
                }));

            _dictionary.Add("BobombSpaceBetween",
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
                (double spaceBetween, uint objAddress) =>
                {
                    float hitboxRadius = Config.Stream.GetSingle(objAddress + ObjectConfig.HitboxRadius);
                    float radius = hitboxRadius + 32;
                    double distAway = spaceBetween + radius;

                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    (double newMarioX, double newMarioZ) =
                        MoreMath.ExtrapolateLine2D(
                            objPos.X, objPos.Z, marioPos.X, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            // Object specific vars - Chuckya

            _dictionary.Add("ChuckyaAngleMod1024",
                ((uint objAddress) =>
                {
                    ushort angle = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawMovingOffset);
                    int mod = angle % 1024;
                    return mod;
                },
                DEFAULT_SETTER));

            // Object specific vars - Scuttlebug

            _dictionary.Add("ScuttlebugDeltaAngleToTarget",
                ((uint objAddress) =>
                {
                    ushort facingAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                    ushort targetAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                    int angleDiff = facingAngle - targetAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint objAddress) =>
                {
                    ushort targetAngle = Config.Stream.GetUInt16(objAddress + ObjectConfig.ScuttlebugTargetAngleOffset);
                    double newObjAngleDouble = targetAngle + angleDiff;
                    ushort newObjAngleUShort = MoreMath.NormalizeAngleUshort(newObjAngleDouble);
                    return PositionAngle.Obj(objAddress).SetAngle(newObjAngleUShort);
                }));

            // Object specific vars - Goomba Triplet Spawner

            _dictionary.Add("GoombaTripletLoadingDistanceDiff",
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    double distDiff = dist - 3000;
                    return distDiff;
                },
                (double distDiff, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double distAway = distDiff + 3000;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                }));

            _dictionary.Add("GoombaTripletUnloadingDistanceDiff",
                ((uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double dist = MoreMath.GetDistanceBetween(
                        marioPos.X, marioPos.Y, marioPos.Z, objPos.X, objPos.Y, objPos.Z);
                    double distDiff = dist - 4000;
                    return distDiff;
                },
                (double distDiff, uint objAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle objPos = PositionAngle.Obj(objAddress);
                    double distAway = distDiff + 4000;
                    (double newMarioX, double newMarioY, double newMarioZ) =
                        MoreMath.ExtrapolateLine3D(
                            objPos.X, objPos.Y, objPos.Z, marioPos.X, marioPos.Y, marioPos.Z, distAway);
                    return marioPos.SetValues(x: newMarioX, y: newMarioY, z: newMarioZ);
                }));

            _dictionary.Add("BitfsPlatformGroupMinHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    return BitfsPlatformGroupTable.GetMinHeight(timer, height);
                },
                (double newMinHeight, uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double oldMinHeight = BitfsPlatformGroupTable.GetMinHeight(timer, height);
                    double heightDiff = newMinHeight - oldMinHeight;
                    float oldHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("BitfsPlatformGroupMaxHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    return BitfsPlatformGroupTable.GetMaxHeight(timer, height);
                },
                (double newMaxHeight, uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double oldMaxHeight = BitfsPlatformGroupTable.GetMaxHeight(timer, height);
                    double heightDiff = newMaxHeight - oldMaxHeight;
                    float oldHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    double newHeight = oldHeight + heightDiff;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            _dictionary.Add("BitfsPlatformGroupRelativeHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    return BitfsPlatformGroupTable.GetRelativeHeightFromMin(timer);
                },
                DEFAULT_SETTER));

            _dictionary.Add("BitfsPlatformGroupDisplacedHeight",
                ((uint objAddress) =>
                {
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float height = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                    float homeHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                    return BitfsPlatformGroupTable.GetDisplacedHeight(timer, height, homeHeight);
                },
                (double displacedHeight, uint objAddress) =>
                {
                    float homeHeight = Config.Stream.GetSingle(objAddress + ObjectConfig.HomeYOffset);
                    double newMaxHeight = homeHeight + displacedHeight;
                    int timer = Config.Stream.GetInt32(objAddress + ObjectConfig.BitfsPlatformGroupTimerOffset);
                    float relativeHeightFromMax = BitfsPlatformGroupTable.GetRelativeHeightFromMax(timer);
                    double newHeight = newMaxHeight + relativeHeightFromMax;
                    return Config.Stream.SetValue((float)newHeight, objAddress + ObjectConfig.YOffset);
                }));

            // Mario vars

            _dictionary.Add("RotationDisplacementX",
                ((uint dummy) =>
                {
                    return GetRotationDisplacement().ToTuple().Item1;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RotationDisplacementY",
                ((uint dummy) =>
                {
                    return GetRotationDisplacement().ToTuple().Item2;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RotationDisplacementZ",
                ((uint dummy) =>
                {
                    return GetRotationDisplacement().ToTuple().Item3;
                },
                DEFAULT_SETTER));

            _dictionary.Add("DeFactoSpeed",
                ((uint dummy) =>
                {
                    return GetMarioDeFactoSpeed();
                },
                (double newDefactoSpeed, uint dummy) =>
                {
                    double newHSpeed = newDefactoSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("SlidingSpeed",
                ((uint dummy) =>
                {
                    return GetMarioSlidingSpeed();
                },
                (double newHSlidingSpeed, uint dummy) =>
                {
                    float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    if (xSlidingSpeed == 0 && zSlidingSpeed == 0) xSlidingSpeed = 1;
                    double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                    double multiplier = newHSlidingSpeed / hSlidingSpeed;
                    double newXSlidingSpeed = xSlidingSpeed * multiplier;
                    double newZSlidingSpeed = zSlidingSpeed * multiplier;

                    bool success = true;
                    success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    return success;
                }));

            _dictionary.Add("SlidingAngle",
                ((uint dummy) =>
                {
                    float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    double slidingAngle = MoreMath.AngleTo_AngleUnits(xSlidingSpeed, zSlidingSpeed);
                    return slidingAngle;
                },
                (double newHSlidingAngle, uint dummy) =>
                {
                    float xSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    float zSlidingSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);

                    (double newXSlidingSpeed, double newZSlidingSpeed) =
                        MoreMath.GetComponentsFromVector(hSlidingSpeed, newHSlidingAngle);

                    bool success = true;
                    success &= Config.Stream.SetValue((float)newXSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                    success &= Config.Stream.SetValue((float)newZSlidingSpeed, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
                    return success;
                }));

            _dictionary.Add("BobombTrajectoryFramesToPoint",
                ((uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = SpecialConfig.PointY - holpPos.Y;
                    double frames = GetObjectTrajectoryYDistToFrames(yDist);
                    return frames;
                },
                (double frames, uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = GetObjectTrajectoryFramesToYDist(frames);
                    double hDist = Math.Abs(GetBobombTrajectoryFramesToHDist(frames));
                    double newY = SpecialConfig.PointY - yDist;
                    (double newX, double newZ) = MoreMath.AddVectorToPoint(
                        hDist,
                        MoreMath.ReverseAngle(SpecialConfig.PointAngle),
                        SpecialConfig.PointX,
                        SpecialConfig.PointZ);
                    return PositionAngle.Holp.SetValues(x: newX, y: newY, z: newZ);
                }));

            _dictionary.Add("CorkBoxTrajectoryFramesToPoint",
                ((uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = SpecialConfig.PointY - holpPos.Y;
                    double frames = GetObjectTrajectoryYDistToFrames(yDist);
                    return frames;
                },
                (double frames, uint dummy) =>
                {
                    PositionAngle holpPos = PositionAngle.Holp;
                    double yDist = GetObjectTrajectoryFramesToYDist(frames);
                    double hDist = Math.Abs(GetCorkBoxTrajectoryFramesToHDist(frames));
                    double newY = SpecialConfig.PointY - yDist;
                    (double newX, double newZ) = MoreMath.AddVectorToPoint(
                        hDist,
                        MoreMath.ReverseAngle(SpecialConfig.PointAngle),
                        SpecialConfig.PointX,
                        SpecialConfig.PointZ);
                    return PositionAngle.Holp.SetValues(x: newX, y: newY, z: newZ);
                }));

            _dictionary.Add("TrajectoryRemainingHeight",
                ((uint dummy) =>
                {
                    float vSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                    double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    return remainingHeight;
                },
                (double newRemainingHeight, uint dummy) =>
                {
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                    return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                }));

            _dictionary.Add("TrajectoryPeakHeight",
                ((uint dummy) =>
                {
                    float vSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                    double remainingHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double peakHeight = marioY + remainingHeight;
                    return peakHeight;
                },
                (double newPeakHeight, uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newRemainingHeight = newPeakHeight - marioY;
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newRemainingHeight);
                    return Config.Stream.SetValue((float)initialVSpeed, MarioConfig.StructAddress + MarioConfig.VSpeedOffset);
                }));

            _dictionary.Add("DoubleJumpVerticalSpeed",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                    return vSpeed;
                },
                (double newVSpeed, uint dummy) =>
                {
                    double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(newVSpeed);
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("DoubleJumpHeight",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                    double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    return doubleJumpHeight;
                },
                (double newHeight, uint dummy) =>
                {
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                    double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("DoubleJumpPeakHeight",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    double vSpeed = ConvertDoubleJumpHSpeedToVSpeed(hSpeed);
                    double doubleJumpHeight = ComputeHeightChangeFromInitialVerticalSpeed(vSpeed);
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double doubleJumpPeakHeight = marioY + doubleJumpHeight;
                    return doubleJumpPeakHeight;
                },
                (double newPeakHeight, uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newHeight = newPeakHeight - marioY;
                    double initialVSpeed = ComputeInitialVerticalSpeedFromHeightChange(newHeight);
                    double newHSpeed = ConvertDoubleJumpVSpeedToHSpeed(initialVSpeed);
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("MovementX",
                ((uint dummy) =>
                {
                    float endX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x10);
                    float startX = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x1C);
                    float movementX = endX - startX;
                    return movementX;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MovementY",
                ((uint dummy) =>
                {
                    float endY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x14);
                    float startY = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x20);
                    float movementY = endY - startY;
                    return movementY;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MovementZ",
                ((uint dummy) =>
                {
                    float endZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x18);
                    float startZ = Config.Stream.GetSingle(MiscConfig.HackedAreaAddress + 0x24);
                    float movementZ = endZ - startZ;
                    return movementZ;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MovementForwards",
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
                DEFAULT_SETTER));

            _dictionary.Add("MovementSideways",
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
                DEFAULT_SETTER));

            _dictionary.Add("MovementHorizontal",
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
                DEFAULT_SETTER));

            _dictionary.Add("MovementTotal",
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
                DEFAULT_SETTER));

            _dictionary.Add("MovementAngle",
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
                DEFAULT_SETTER));

            _dictionary.Add("QFrameCountEstimate",
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
                DEFAULT_SETTER));

            _dictionary.Add("DeltaYawIntendedFacing",
                ((uint dummy) =>
                {
                    return GetDeltaYawIntendedFacing();
                },
                DEFAULT_SETTER));

            _dictionary.Add("DeltaYawIntendedBackwards",
                ((uint dummy) =>
                {
                    return GetDeltaYawIntendedBackwards();
                },
                DEFAULT_SETTER));

            _dictionary.Add("FallHeight",
                ((uint dummy) =>
                {
                    float peakHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                    float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float fallHeight = peakHeight - floorY;
                    return fallHeight;
                },
                (double fallHeight, uint dummy) =>
                {
                    float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    double newPeakHeight = floorY + fallHeight;
                    return Config.Stream.SetValue((float)newPeakHeight, MarioConfig.StructAddress + MarioConfig.PeakHeightOffset);
                }));

            _dictionary.Add("WalkingDistance",
                ((uint dummy) =>
                {
                    float hSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                    float remainder = hSpeed % 1;
                    int numFrames = (int)Math.Abs(Math.Truncate(hSpeed)) + 1;
                    float sum = (hSpeed + remainder) * numFrames / 2;
                    float distance = sum - hSpeed;
                    return distance;
                },
                DEFAULT_SETTER));

            _dictionary.Add("WalkingDistanceDifferenceMarioToPoint",
                ((uint dummy) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    PositionAngle pointPos = SpecialConfig.PointPA;
                    float walkingDistance = (float)_dictionary.Get("WalkingDistance").Item1(0);
                    double diff = walkingDistance - PositionAngle.GetHDistance(marioPos, pointPos);
                    return diff;
                },
                DEFAULT_SETTER));
            
            // HUD vars

            _dictionary.Add("HudTimeText",
                ((uint dummy) =>
                {
                    ushort time = Config.Stream.GetUInt16(MarioConfig.StructAddress + HudConfig.TimeOffset);
                    int totalDeciSeconds = time / 3;
                    int deciSecondComponent = totalDeciSeconds % 10;
                    int secondComponent = (totalDeciSeconds / 10) % 60;
                    int minuteComponent = (totalDeciSeconds / 600);
                    return minuteComponent + "'" + secondComponent.ToString("D2") + "\"" + deciSecondComponent;
                },
                (string timerString, uint dummy) =>
                {
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
                }));

            // Triangle vars

            _dictionary.Add("Classification",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    return triStruct.Classification.ToString();
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleTypeDescription",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    return triStruct.Description;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleSlipperiness",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    return triStruct.Slipperiness;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleSlipperinessDescription",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    return triStruct.SlipperinessDescription;
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleExertion",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    return triStruct.Exertion ? 1 : 0;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertex",
                ((uint triAddress) =>
                {
                    return "V" + GetClosestTriangleVertexIndex(triAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexX",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).X;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexY",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).Y;
                },
                DEFAULT_SETTER));

            _dictionary.Add("ClosestVertexZ",
                ((uint triAddress) =>
                {
                    return GetClosestTriangleVertexPosition(triAddress).Z;
                },
                DEFAULT_SETTER));

            _dictionary.Add("Steepness",
                ((uint triAddress) =>
                {
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double steepness = MoreMath.RadiansToAngleUnits(Math.Acos(triStruct.NormY));
                    return steepness;
                },
                DEFAULT_SETTER));

            _dictionary.Add("UpHillAngle",
                ((uint triAddress) =>
                {

                    return GetTriangleUphillAngle(triAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("DownHillAngle",
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.ReverseAngle(uphillAngle);
                },
                DEFAULT_SETTER));

            _dictionary.Add("LeftHillAngle",
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.RotateAngleCCW(uphillAngle, 16384);
                },
                DEFAULT_SETTER));

            _dictionary.Add("RightHillAngle",
                ((uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    return MoreMath.RotateAngleCW(uphillAngle, 16384);
                },
                DEFAULT_SETTER));

            _dictionary.Add("UpHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double angleDiff = marioAngle - uphillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double newMarioAngleDouble = uphillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DownHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                    double angleDiff = marioAngle - downhillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                    double newMarioAngleDouble = downhillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("LeftHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                    double angleDiff = marioAngle - lefthillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                    double newMarioAngleDouble = lefthillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("RightHillDeltaAngle",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                    double angleDiff = marioAngle - righthillAngle;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                    double newMarioAngleDouble = righthillAngle + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("HillStatus",
                ((uint triAddress) =>
                {
                    ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    if (Double.IsNaN(uphillAngle)) return "No Hill";
                    double angleDiff = marioAngle - uphillAngle;
                    angleDiff = MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                    bool uphill = angleDiff >= -16384 && angleDiff <= 16384;
                    return uphill ? "Uphill" : "Downhill";
                },
                DEFAULT_SETTER));

            _dictionary.Add("WallKickAngleAway",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double angleDiff = marioPos.Angle - uphillAngle;
                    int angleDiffShort = MoreMath.NormalizeAngleShort(angleDiff);
                    int angleDiffAbs = Math.Abs(angleDiffShort);
                    int angleAway = angleDiffAbs - 8192;
                    return angleAway;
                },
                (double angleAway, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    double uphillAngle = GetTriangleUphillAngle(triAddress);
                    double oldAngleDiff = marioPos.Angle - uphillAngle;
                    int oldAngleDiffShort = MoreMath.NormalizeAngleShort(oldAngleDiff);
                    int signMultiplier = oldAngleDiffShort >= 0 ? 1 : -1;

                    double angleDiffAbs = angleAway + 8192;
                    double angleDiff = angleDiffAbs * signMultiplier;
                    double marioAngleDouble = uphillAngle + angleDiff;
                    ushort marioAngleUShort = MoreMath.NormalizeAngleUshort(marioAngleDouble);

                    return Config.Stream.SetValue(marioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DistanceAboveFloor",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float distAboveFloor = marioY - floorY;
                    return distAboveFloor;
                },
                (double distAbove, uint dummy) =>
                {
                    float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    double newMarioY = floorY + distAbove;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("DistanceBelowCeiling",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float ceilingY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                    float distBelowCeiling = ceilingY - marioY;
                    return distBelowCeiling;
                },
                (double distBelow, uint dummy) =>
                {
                    float ceilingY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.CeilingYOffset);
                    double newMarioY = ceilingY - distBelow;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("NormalDistAway",
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
                (double distAway, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);

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
                }));

            _dictionary.Add("VerticalDistAway",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double verticalDistAway =
                        marioPos.Y + (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return verticalDistAway;
                },
                (double distAbove, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double newMarioY = distAbove - (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("HeightOnTriangle",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double heightOnTriangle = triStruct.GetHeightOnTriangle(marioPos.X, marioPos.Z);
                    return heightOnTriangle;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedUphill",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, true, false);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedUphillAtAngle",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, true, true);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedDownhill",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, false, false);
                },
                DEFAULT_SETTER));

            _dictionary.Add("MaxHSpeedDownhillAtAngle",
                ((uint triAddress) =>
                {
                    return GetMaxHorizontalSpeedOnTriangle(triAddress, false, true);
                },
                DEFAULT_SETTER));

            _dictionary.Add("TriangleCells",
                ((uint triAddress) =>
                {
                    TriangleDataModel tri = new TriangleDataModel(triAddress);
                    short minCellX = lower_cell_index(tri.GetMinX());
                    short maxCellX = upper_cell_index(tri.GetMaxX());
                    short minCellZ = lower_cell_index(tri.GetMinZ());
                    short maxCellZ = upper_cell_index(tri.GetMaxZ());
                    return string.Format("X:{0}-{1},Z:{2}-{3}",
                        minCellX, maxCellX, minCellZ, maxCellZ);
                },
                DEFAULT_SETTER));

            _dictionary.Add("ObjectTriCount",
                ((uint dummy) =>
                {
                    int totalTriangleCount = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
                    int levelTriangleCount = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);
                    int objectTriangleCount = totalTriangleCount - levelTriangleCount;
                    return objectTriangleCount;
                },
                DEFAULT_SETTER));

            _dictionary.Add("CurrentTriangleIndex",
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
                (int index, uint triAddress) =>
                {
                    uint triangleListStartAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
                    uint structSize = TriangleConfig.TriangleStructSize;
                    uint newTriAddress = (uint)(triangleListStartAddress + index * structSize);
                    Config.TriangleManager.SetCustomTriangleAddress(newTriAddress);
                    return true;
                }));

            _dictionary.Add("CurrentTriangleAddress",
                ((uint triAddress) =>
                {
                    return triAddress;
                },
                (uint address, uint triAddress) =>
                {
                    Config.TriangleManager.SetCustomTriangleAddress(address);
                    return true;
                }));

            _dictionary.Add("ObjectNodeCount",
                ((uint dummy) =>
                {
                    int totalNodeCount = Config.Stream.GetInt32(TriangleConfig.TotalNodeCountAddress);
                    int levelNodeCount = Config.Stream.GetInt32(TriangleConfig.LevelNodeCountAddress);
                    int objectNodeCount = totalNodeCount - levelNodeCount;
                    return objectNodeCount;
                },
                DEFAULT_SETTER));

            _dictionary.Add("DistanceToLine12",
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
                (double dist, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 1, 2);

                    double missingDist = dist - signedDistToLine12;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newMarioX = marioPos.X + xDiff;
                    double newMarioZ = marioPos.Z + zDiff;
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("DistanceToLine23",
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
                (double dist, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 2, 3);

                    double missingDist = dist - signedDistToLine23;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newMarioX = marioPos.X + xDiff;
                    double newMarioZ = marioPos.Z + zDiff;
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("DistanceToLine31",
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
                (double dist, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                        marioPos.X, marioPos.Z,
                        triStruct.X1, triStruct.Z1,
                        triStruct.X2, triStruct.Z2,
                        triStruct.X3, triStruct.Z3, 3, 1);

                    double missingDist = dist - signedDistToLine31;
                    double lineAngle = MoreMath.AngleTo_AngleUnits(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    bool floorTri = MoreMath.IsPointLeftOfLine(triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double inwardAngle = floorTri ? MoreMath.RotateAngleCCW(lineAngle, 16384) : MoreMath.RotateAngleCW(lineAngle, 16384);

                    (double xDiff, double zDiff) = MoreMath.GetComponentsFromVector(missingDist, inwardAngle);
                    double newMarioX = marioPos.X + xDiff;
                    double newMarioZ = marioPos.Z + zDiff;
                    return marioPos.SetValues(x: newMarioX, z: newMarioZ);
                }));

            _dictionary.Add("DeltaAngleLine12",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double angleDiff = marioPos.Angle - angleV1ToV2;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);;
                    double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                    double newMarioAngleDouble = angleV1ToV2 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine21",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                    double angleDiff = marioPos.Angle - angleV2ToV1;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                    double newMarioAngleDouble = angleV2ToV1 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine23",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    double angleDiff = marioPos.Angle - angleV2ToV3;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                    double newMarioAngleDouble = angleV2ToV3 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine32",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                    double angleDiff = marioPos.Angle - angleV3ToV2;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                    double newMarioAngleDouble = angleV3ToV2 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine31",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    double angleDiff = marioPos.Angle - angleV3ToV1;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                    double newMarioAngleDouble = angleV3ToV1 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            _dictionary.Add("DeltaAngleLine13",
                ((uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                    double angleDiff = marioPos.Angle - angleV1ToV3;
                    return MoreMath.NormalizeAngleDoubleSigned(angleDiff);
                },
                (double angleDiff, uint triAddress) =>
                {
                    PositionAngle marioPos = PositionAngle.Mario;
                    TriangleDataModel triStruct = Config.TriangleManager.GetTriangleStruct(triAddress);
                    double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                        triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                    double newMarioAngleDouble = angleV1ToV3 + angleDiff;
                    ushort newMarioAngleUShort = MoreMath.NormalizeAngleUshort(newMarioAngleDouble);
                    return Config.Stream.SetValue(
                        newMarioAngleUShort, MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
                }));

            // File vars

            _dictionary.Add("StarsInFile",
                ((uint fileAddress) =>
                {
                    return Config.FileManager.CalculateNumStars(fileAddress);
                },
                DEFAULT_SETTER));

            _dictionary.Add("FileChecksumCalculated",
                ((uint fileAddress) =>
                {
                    return Config.FileManager.GetChecksum(fileAddress);
                },
                DEFAULT_SETTER));

            // Main Save vars

            _dictionary.Add("MainSaveChecksumCalculated",
                ((uint mainSaveAddress) =>
                {
                    return Config.MainSaveManager.GetChecksum(mainSaveAddress);
                },
                DEFAULT_SETTER));

            // Action vars

            _dictionary.Add("ActionDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetActionName();
                },
                DEFAULT_SETTER));

            _dictionary.Add("PrevActionDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetPrevActionName();
                },
                DEFAULT_SETTER));

            _dictionary.Add("ActionGroupDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioActions.GetGroupName();
                },
                DEFAULT_SETTER));

            _dictionary.Add("AnimationDescription",
                ((uint dummy) =>
                {
                    return TableConfig.MarioAnimations.GetAnimationName();
                },
                DEFAULT_SETTER));

            // Water vars

            _dictionary.Add("WaterAboveMedian",
                ((uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    short waterLevelMedian = Config.Stream.GetInt16(MiscConfig.WaterLevelMedianAddress);
                    double waterAboveMedian = waterLevel - waterLevelMedian;
                    return waterAboveMedian;
                },
                DEFAULT_SETTER));

            _dictionary.Add("MarioAboveWater",
                ((uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float marioAboveWater = marioY - waterLevel;
                    return marioAboveWater;
                },
                (double goalMarioAboveWater, uint dummy) =>
                {
                    short waterLevel = Config.Stream.GetInt16(MarioConfig.StructAddress + MarioConfig.WaterLevelOffset);
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double goalMarioY = waterLevel + goalMarioAboveWater;
                    return Config.Stream.SetValue((float)goalMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            // PU vars

            _dictionary.Add("MarioXQpuIndex",
                ((uint dummy) =>
                {
                    float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    int puXIndex = PuUtilities.GetPuIndex(marioX);
                    double qpuXIndex = puXIndex / 4d;
                    return qpuXIndex;
                },
                (double newQpuXIndex, uint dummy) =>
                {
                    int newPuXIndex = (int)Math.Round(newQpuXIndex * 4);
                    float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                    return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                }));

            _dictionary.Add("MarioYQpuIndex",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    int puYIndex = PuUtilities.GetPuIndex(marioY);
                    double qpuYIndex = puYIndex / 4d;
                    return qpuYIndex;
                },
                (double newQpuYIndex, uint dummy) =>
                {
                    int newPuYIndex = (int)Math.Round(newQpuYIndex * 4);
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("MarioZQpuIndex",
                ((uint dummy) =>
                {
                    float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    int puZIndex = PuUtilities.GetPuIndex(marioZ);
                    double qpuZIndex = puZIndex / 4d;
                    return qpuZIndex;
                },
                (double newQpuZIndex, uint dummy) =>
                {
                    int newPuZIndex = (int)Math.Round(newQpuZIndex * 4);
                    float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                    return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                }));

            _dictionary.Add("MarioXPuIndex",
                ((uint dummy) =>
                {
                    float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    int puXIndex = PuUtilities.GetPuIndex(marioX);
                    return puXIndex;
                },
                (int newPuXIndex, uint dummy) =>
                {
                    float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    double newMarioX = PuUtilities.GetCoordinateInPu(marioX, newPuXIndex);
                    return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                }));

            _dictionary.Add("MarioYPuIndex",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    int puYIndex = PuUtilities.GetPuIndex(marioY);
                    return puYIndex;
                },
                (int newPuYIndex, uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double newMarioY = PuUtilities.GetCoordinateInPu(marioY, newPuYIndex);
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("MarioZPuIndex",
                ((uint dummy) =>
                {
                    float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    int puZIndex = PuUtilities.GetPuIndex(marioZ);
                    return puZIndex;
                },
                (int newPuZIndex, uint dummy) =>
                {
                    float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    double newMarioZ = PuUtilities.GetCoordinateInPu(marioZ, newPuZIndex);
                    return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                }));

            _dictionary.Add("MarioXPuRelative",
                ((uint dummy) =>
                {
                    float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    double relX = PuUtilities.GetRelativeCoordinate(marioX);
                    return relX;
                },
                (double newRelX, uint dummy) =>
                {
                    float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                    int puXIndex = PuUtilities.GetPuIndex(marioX);
                    double newMarioX = PuUtilities.GetCoordinateInPu(newRelX, puXIndex);
                    return Config.Stream.SetValue((float)newMarioX, MarioConfig.StructAddress + MarioConfig.XOffset);
                }));

            _dictionary.Add("MarioYPuRelative",
                ((uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    double relY = PuUtilities.GetRelativeCoordinate(marioY);
                    return relY;
                },
                (double newRelY, uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    int puYIndex = PuUtilities.GetPuIndex(marioY);
                    double newMarioY = PuUtilities.GetCoordinateInPu(newRelY, puYIndex);
                    return Config.Stream.SetValue((float)newMarioY, MarioConfig.StructAddress + MarioConfig.YOffset);
                }));

            _dictionary.Add("MarioZPuRelative",
                ((uint dummy) =>
                {
                    float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    double relZ = PuUtilities.GetRelativeCoordinate(marioZ);
                    return relZ;
                },
                (double newRelZ, uint dummy) =>
                {
                    float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
                    int puZIndex = PuUtilities.GetPuIndex(marioZ);
                    double newMarioZ = PuUtilities.GetCoordinateInPu(newRelZ, puZIndex);
                    return Config.Stream.SetValue((float)newMarioZ, MarioConfig.StructAddress + MarioConfig.ZOffset);
                }));

            _dictionary.Add("DeFactoMultiplier",
                ((uint dummy) =>
                {
                    return GetDeFactoMultiplier();
                },
                (double newDeFactoMultiplier, uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float distAboveFloor = marioY - floorY;
                    if (distAboveFloor != 0) return false;

                    uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    if (floorTri == 0) return false;
                    return Config.Stream.SetValue((float)newDeFactoMultiplier, floorTri + TriangleOffsetsConfig.NormY);
                }));

            _dictionary.Add("SyncingSpeed",
                ((uint dummy) =>
                {
                    return GetSyncingSpeed();
                },
                (double newSyncingSpeed, uint dummy) =>
                {
                    float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float floorY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
                    float distAboveFloor = marioY - floorY;
                    if (distAboveFloor != 0) return false;

                    uint floorTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    if (floorTri == 0) return false;
                    double newYnorm = PuUtilities.QpuSpeed / newSyncingSpeed * SpecialConfig.PuHypotenuse;
                    return Config.Stream.SetValue((float)newYnorm, floorTri + TriangleOffsetsConfig.NormY);
                }));

            _dictionary.Add("QpuSpeed",
                ((uint dummy) =>
                {
                    return GetQpuSpeed();
                },
                (double newQpuSpeed, uint dummy) =>
                {
                    double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("PuSpeed",
                ((uint dummy) =>
                {
                    double puSpeed = GetQpuSpeed() * 4;
                    return puSpeed;
                },
                (double newPuSpeed, uint dummy) =>
                {
                    double newQpuSpeed = newPuSpeed / 4;
                    double newHSpeed = newQpuSpeed * GetSyncingSpeed();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("QpuSpeedComponent",
                ((uint dummy) =>
                {
                    return Math.Round(GetQpuSpeed());
                },
                (int newQpuSpeedComp, uint dummy) =>
                {
                    double relativeSpeed = GetRelativePuSpeed();
                    double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("PuSpeedComponent",
                ((uint dummy) =>
                {
                    return Math.Round(GetQpuSpeed() * 4);
                },
                (int newPuSpeedComp, uint dummy) =>
                {
                    double newQpuSpeedComp = newPuSpeedComp / 4d;
                    double relativeSpeed = GetRelativePuSpeed();
                    double newHSpeed = newQpuSpeedComp * GetSyncingSpeed() + relativeSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("RelativeSpeed",
                ((uint dummy) =>
                {
                    return GetRelativePuSpeed();
                },
                (double newRelativeSpeed, uint dummy) =>
                {
                    double puSpeed = GetQpuSpeed() * 4;
                    double puSpeedRounded = Math.Round(puSpeed);
                    double newHSpeed = (puSpeedRounded / 4) * GetSyncingSpeed() + newRelativeSpeed / GetDeFactoMultiplier();
                    return Config.Stream.SetValue((float)newHSpeed, MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
                }));

            _dictionary.Add("Qs1RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(1 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, true, true);
                }));

            _dictionary.Add("Qs1RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(1 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, false, true);
                }));

            _dictionary.Add("Qs1RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(1 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, true, false);
                }));

            _dictionary.Add("Qs1RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(1 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 1 / 4d, false, false);
                }));

            _dictionary.Add("Qs2RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(2 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, true, true);
                }));

            _dictionary.Add("Qs2RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(2 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, false, true);
                }));

            _dictionary.Add("Qs2RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(2 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, true, false);
                }));

            _dictionary.Add("Qs2RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(2 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 2 / 4d, false, false);
                }));

            _dictionary.Add("Qs3RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(3 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, true, true);
                }));

            _dictionary.Add("Qs3RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(3 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, false, true);
                }));

            _dictionary.Add("Qs3RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(3 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, true, false);
                }));

            _dictionary.Add("Qs3RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(3 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 3 / 4d, false, false);
                }));

            _dictionary.Add("Qs4RelativeXSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(4 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, true, true);
                }));

            _dictionary.Add("Qs4RelativeZSpeed",
                ((uint dummy) =>
                {
                    return GetQsRelativeSpeed(4 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, false, true);
                }));

            _dictionary.Add("Qs4RelativeIntendedNextX",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(4 / 4d, true);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, true, false);
                }));

            _dictionary.Add("Qs4RelativeIntendedNextZ",
                ((uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(4 / 4d, false);
                },
                (double newValue, uint dummy) =>
                {
                    return GetQsRelativeIntendedNextComponent(newValue, 4 / 4d, false, false);
                }));

            _dictionary.Add("PuParams",
                ((uint dummy) =>
                {
                    return "(" + SpecialConfig.PuParam1 + "," + SpecialConfig.PuParam2 + ")";
                },
                (string puParamsString, uint dummy) =>
                {
                    List<string> stringList = ParsingUtilities.ParseStringList(puParamsString);
                    List<int?> intList = stringList.ConvertAll(
                        stringVal => ParsingUtilities.ParseIntNullable(stringVal));
                    if (intList.Count == 1) intList.Insert(0, 0);
                    if (intList.Count != 2 || intList.Exists(intValue => !intValue.HasValue)) return false;
                    SpecialConfig.PuParam1 = intList[0].Value;
                    SpecialConfig.PuParam2 = intList[1].Value;
                    return true;
                }));

            // Misc vars

            _dictionary.Add("GlobalTimerMod64",
                ((uint dummy) =>
                {
                    uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
                    return globalTimer % 64;
                },
                DEFAULT_SETTER));

            _dictionary.Add("RngIndex",
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                    return RngIndexer.GetRngIndex(rngValue);
                },
                (int rngIndex, uint dummy) =>
                {
                    ushort rngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                }));

            _dictionary.Add("RngIndexMod4",
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                    int rngIndex = RngIndexer.GetRngIndex();
                    return rngIndex % 4;
                },
                DEFAULT_SETTER));

            _dictionary.Add("LastCoinRngIndex",
                ((uint coinAddress) =>
                {
                    ushort coinRngValue = Config.Stream.GetUInt16(coinAddress + ObjectConfig.YawMovingOffset);
                    int coinRngIndex = RngIndexer.GetRngIndex(coinRngValue);
                    return coinRngIndex;
                },
                (int rngIndex, uint coinAddress) =>
                {
                    ushort coinRngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(coinRngValue, coinAddress + ObjectConfig.YawMovingOffset);
                }));

            _dictionary.Add("LastCoinRngIndexDiff",
                ((uint coinAddress) =>
                {
                    ushort coinRngValue = Config.Stream.GetUInt16(coinAddress + ObjectConfig.YawMovingOffset);
                    int coinRngIndex = RngIndexer.GetRngIndex(coinRngValue);
                    int rngIndexDiff = coinRngIndex - SpecialConfig.GoalRngIndex;
                    return rngIndexDiff;
                },
                (int rngIndexDiff, uint coinAddress) =>
                {
                    int coinRngIndex = SpecialConfig.GoalRngIndex + rngIndexDiff;
                    ushort coinRngValue = RngIndexer.GetRngValue(coinRngIndex);
                    return Config.Stream.SetValue(coinRngValue, coinAddress + ObjectConfig.YawMovingOffset);
                }));
            
            _dictionary.Add("GoalRngValue",
                ((uint dummy) =>
                {
                    return SpecialConfig.GoalRngValue;
                },
                (ushort goalRngValue, uint coinAddress) =>
                {
                    SpecialConfig.GoalRngValue = goalRngValue;
                    return true;
                }));

            _dictionary.Add("GoalRngIndex",
                ((uint dummy) =>
                {
                    return SpecialConfig.GoalRngIndex;
                },
                (ushort goalRngIndex, uint coinAddress) =>
                {
                    SpecialConfig.GoalRngIndex = goalRngIndex;
                    return true;
                }));

            _dictionary.Add("GoalRngIndexDiff",
                ((uint dummy) =>
                {
                    ushort rngValue = Config.Stream.GetUInt16(MiscConfig.RngAddress);
                    int rngIndex = RngIndexer.GetRngIndex(rngValue);
                    int rngIndexDiff = rngIndex - SpecialConfig.GoalRngIndex;
                    return rngIndexDiff;
                },
                (int rngIndexDiff, uint dummy) =>
                {
                    int rngIndex = SpecialConfig.GoalRngIndex + rngIndexDiff;
                    ushort rngValue = RngIndexer.GetRngValue(rngIndex);
                    return Config.Stream.SetValue(rngValue, MiscConfig.RngAddress);
                }));

            _dictionary.Add("RngCallsPerFrame",
                ((uint dummy) =>
                {
                    ushort preRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0C);
                    ushort currentRng = Config.Stream.GetUInt16(MiscConfig.HackedAreaAddress + 0x0E);
                    int rngDiff = RngIndexer.GetRngIndexDiff(preRng, currentRng);
                    return rngDiff;
                },
                DEFAULT_SETTER));

            _dictionary.Add("NumberOfLoadedObjects",
                ((uint dummy) =>
                {
                    return DataModels.ObjectProcessor.ActiveObjectCount;
                },
                DEFAULT_SETTER));

            _dictionary.Add("PlayTime",
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
                DEFAULT_SETTER));

            _dictionary.Add("DemoCounterDescription",
                ((uint dummy) =>
                {
                    return DemoCounterUtilities.GetDemoCounterDescription();
                },
                (string description, uint dummy) =>
                {
                    short? demoCounterNullable = DemoCounterUtilities.GetDemoCounter(description);
                    if (!demoCounterNullable.HasValue) return false;
                    return Config.Stream.SetValue(demoCounterNullable.Value, MiscConfig.DemoCounterAddress);
                }
            ));

            _dictionary.Add("TtcSpeedSettingDescription",
                ((uint dummy) =>
                {
                    return TtcSpeedSettingUtilities.GetTtcSpeedSettingDescription();
                },
                (string description, uint dummy) =>
                {
                    short? ttcSpeedSettingNullable = TtcSpeedSettingUtilities.GetTtcSpeedSetting(description);
                    if (!ttcSpeedSettingNullable.HasValue) return false;
                    return Config.Stream.SetValue(ttcSpeedSettingNullable.Value, MiscConfig.TtcSpeedSettingAddress);
                }));

            _dictionary.Add("TtcSaveState",
                ((uint dummy) =>
                {
                    return new TtcSaveState().ToString();
                },
                DEFAULT_SETTER));            

            // Area vars

            _dictionary.Add("CurrentAreaIndexMario",
                ((uint dummy) =>
                {
                    uint currentAreaMario = Config.Stream.GetUInt32(
                        MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                    double currentAreaIndexMario = AreaUtilities.GetAreaIndex(currentAreaMario) ?? Double.NaN;
                    return currentAreaIndexMario;
                },
                (int currentAreaIndexMario, uint dummy) =>
                {
                    if (currentAreaIndexMario < 0 || currentAreaIndexMario >= 8) return false;
                    uint currentAreaAddressMario = AreaUtilities.GetAreaAddress(currentAreaIndexMario);
                    return Config.Stream.SetValue(
                        currentAreaAddressMario, MarioConfig.StructAddress + MarioConfig.AreaPointerOffset);
                }));

            _dictionary.Add("CurrentAreaIndex",
                ((uint dummy) =>
                {
                    uint currentArea = Config.Stream.GetUInt32(AreaConfig.CurrentAreaPointerAddress);
                    double currentAreaIndex = AreaUtilities.GetAreaIndex(currentArea) ?? Double.NaN;
                    return currentAreaIndex;
                },
                (int currentAreaIndex, uint dummy) =>
                {
                    if (currentAreaIndex < 0 || currentAreaIndex >= 8) return false;
                    uint currentAreaAddress = AreaUtilities.GetAreaAddress(currentAreaIndex);
                    return Config.Stream.SetValue(currentAreaAddress, AreaConfig.CurrentAreaPointerAddress);
                }));

            _dictionary.Add("AreaTerrainDescription",
                ((uint dummy) =>
                {
                    short terrainType = Config.Stream.GetInt16(
                        Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                    string terrainDescription = AreaUtilities.GetTerrainDescription(terrainType);
                    return terrainDescription;
                },
                (short terrainType, uint dummy) =>
                {
                    return Config.Stream.SetValue(
                        terrainType, Config.AreaManager.SelectedAreaAddress + AreaConfig.TerrainTypeOffset);
                }));

            // Custom point

            _dictionary.Add("SelfPosType",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfPosPA.ToString();
                },
                (PositionAngle posAngle, uint dummy) =>
                {
                    SpecialConfig.SelfPosPA = posAngle;
                    return true;
                }));

            _dictionary.Add("SelfX",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfX;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.SelfPosPA.SetX(doubleValue);
                }));

            _dictionary.Add("SelfY",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfY;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.SelfPosPA.SetY(doubleValue);
                }));

            _dictionary.Add("SelfZ",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfZ;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.SelfPosPA.SetZ(doubleValue);
                }));

            _dictionary.Add("SelfAngleType",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfAnglePA.ToString();
                },
                (PositionAngle posAngle, uint dummy) =>
                {
                    SpecialConfig.SelfAnglePA = posAngle;
                    return true;
                }));

            _dictionary.Add("SelfAngle",
                ((uint dummy) =>
                {
                    return SpecialConfig.SelfAngle;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.SelfAnglePA.SetAngle(doubleValue);
                }));

            _dictionary.Add("PointPosType",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointPosPA.ToString();
                },
                (PositionAngle posAngle, uint dummy) =>
                {
                    SpecialConfig.PointPosPA = posAngle;
                    return true;
                }));

            _dictionary.Add("PointX",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointX;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.PointPosPA.SetX(doubleValue);
                }));

            _dictionary.Add("PointY",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointY;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.PointPosPA.SetY(doubleValue);
                }));

            _dictionary.Add("PointZ",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointZ;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.PointPosPA.SetZ(doubleValue);
                }));

            _dictionary.Add("PointAngleType",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointAnglePA.ToString();
                },
                (PositionAngle posAngle, uint dummy) =>
                {
                    SpecialConfig.PointAnglePA = posAngle;
                    return true;
                }));

            _dictionary.Add("PointAngle",
                ((uint dummy) =>
                {
                    return SpecialConfig.PointAngle;
                },
                (double doubleValue, uint dummy) =>
                {
                    return SpecialConfig.PointPosPA.SetAngle(doubleValue);
                }));

            // Mupen vars

            _dictionary.Add("MupenLag",
                ((uint objAddress) =>
                {
                    if (!MupenUtilities.IsUsingMupen()) return Double.NaN;
                    int lag = MupenUtilities.GetLagCount() + SpecialConfig.MupenLagOffset;
                    return lag;
                },
                (string stringValue, uint dummy) =>
                {
                    if (!MupenUtilities.IsUsingMupen()) return false;

                    if (stringValue.ToLower() == "x")
                    {
                        SpecialConfig.MupenLagOffset = 0;
                        return true;
                    }

                    int? newLagNullable = ParsingUtilities.ParseIntNullable(stringValue);
                    if (!newLagNullable.HasValue) return false;
                    int newLag = newLagNullable.Value;
                    int newLagOffset = newLag - MupenUtilities.GetLagCount();
                    SpecialConfig.MupenLagOffset = newLagOffset;
                    return true;
                }));
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

        public static int GetPendulumCountdown(uint pendulumAddress)
        {
            // Get pendulum variables
            float accelerationDirection = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
            float accelerationMagnitude = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAccelerationMagnitudeOffset);
            float angularVelocity = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAngularVelocityOffset);
            float angle = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAngleOffset);
            int waitingTimer = Config.Stream.GetInt32(pendulumAddress + ObjectConfig.PendulumWaitingTimerOffset);
            return GetPendulumCountdown(accelerationDirection, accelerationMagnitude, angularVelocity, angle, waitingTimer);
        }

        public static int GetPendulumCountdown(
             float accelerationDirection, float accelerationMagnitude, float angularVelocity, float angle, int waitingTimer)
        {
            return GetPendulumVars(accelerationDirection, accelerationMagnitude, angularVelocity, angle).ToTuple().Item2 + waitingTimer;
        }

        public static float GetPendulumAmplitude(uint pendulumAddress)
        {
            // Get pendulum variables
            float accelerationDirection = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAccelerationDirectionOffset);
            float accelerationMagnitude = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAccelerationMagnitudeOffset);
            float angularVelocity = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAngularVelocityOffset);
            float angle = Config.Stream.GetSingle(pendulumAddress + ObjectConfig.PendulumAngleOffset);
            return GetPendulumAmplitude(accelerationDirection, accelerationMagnitude, angularVelocity, angle);
        }

        public static float GetPendulumAmplitude(
            float accelerationDirection, float accelerationMagnitude, float angularVelocity, float angle)
        {
            return GetPendulumVars(accelerationDirection, accelerationMagnitude, angularVelocity, angle).ToTuple().Item1;
        }

        public static float GetPendulumAmplitude(float angle, float accelerationMagnitude)
        {
            float accelerationDirection = -1 * Math.Sign(angle);
            float angularVelocity = 0;
            return GetPendulumAmplitude(accelerationDirection, accelerationMagnitude, angularVelocity, angle);
        }

        public static (float amplitude, int countdown) GetPendulumVars(
            float accelerationDirection, float accelerationMagnitude, float angularVelocity, float angle)
        {
            // Get pendulum variables
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
            int totalDuration = speedUpDuration + slowDownDuration;
            float totalDistance = speedUpDistance + slowDownDistance;
            float amplitude = angle + totalDistance;
            return (amplitude, totalDuration);
        }

        public static int GetCogNumFramesInRotation(uint cogAddress)
        {
            ushort yawFacing = Config.Stream.GetUInt16(cogAddress + ObjectConfig.YawFacingOffset);
            int currentYawVel = (int)Config.Stream.GetSingle(cogAddress + ObjectConfig.CogCurrentYawVelocity);
            int targetYawVel = (int)Config.Stream.GetSingle(cogAddress + ObjectConfig.CogTargetYawVelocity);
            return GetCogNumFramesInRotation(yawFacing, currentYawVel, targetYawVel);
        }

        public static int GetCogNumFramesInRotation(ushort yawFacing, int currentYawVel, int targetYawVel)
        {
            int diff = Math.Abs(targetYawVel - currentYawVel);
            int numFrames = diff / 50;
            if (numFrames == 0) numFrames = 1;
            return numFrames;
        }

        public static ushort GetCogEndingYaw(uint cogAddress)
        {
            ushort yawFacing = Config.Stream.GetUInt16(cogAddress + ObjectConfig.YawFacingOffset);
            int currentYawVel = (int)Config.Stream.GetSingle(cogAddress + ObjectConfig.CogCurrentYawVelocity);
            int targetYawVel = (int)Config.Stream.GetSingle(cogAddress + ObjectConfig.CogTargetYawVelocity);
            return GetCogEndingYaw(yawFacing, currentYawVel, targetYawVel);
        }

        public static ushort GetCogEndingYaw(ushort yawFacing, int currentYawVel, int targetYawVel)
        {
            int numFrames = GetCogNumFramesInRotation(yawFacing, currentYawVel, targetYawVel);
            int remainingRotation = (currentYawVel + targetYawVel) * (numFrames + 1) / 2 - currentYawVel;
            int endingYaw = yawFacing + remainingRotation;
            return MoreMath.NormalizeAngleUshort(endingYaw);
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
        
        private static bool GetQsRelativeIntendedNextComponent(double newValue, double numFrames, bool xComp, bool relativePosition)
        {
            float currentX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float currentZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float currentComp = xComp ? currentX : currentZ;
            (double intendedX, double intendedZ) = GetIntendedNextPosition(numFrames);
            double intendedComp = xComp ? intendedX : intendedZ;
            int intendedPuCompIndex = PuUtilities.GetPuIndex(intendedComp);
            double newRelativeComp = relativePosition ? currentComp + newValue : newValue;
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

        public static short GetDeltaYawIntendedBackwards()
        {
            ushort marioYawFacing = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioYawIntended = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.IntendedYawOffset);
            return MoreMath.GetDeltaAngleTruncated(marioYawFacing + 32768, marioYawIntended);
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

        // Rotation methods

        private static (float x, float y, float z) GetRotationDisplacement()
        {
            uint stoodOnObject = Config.Stream.GetUInt32(MarioConfig.StoodOnObjectPointerAddress);
            if (stoodOnObject == 0)
            {
                return (0, 0, 0);
            }

            float[] currentObjectPos = new float[]
            {
                Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset),
                Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset),
                Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset),
            };

            float[] platformPos = new float[]
            {
                Config.Stream.GetSingle(stoodOnObject + ObjectConfig.XOffset),
                Config.Stream.GetSingle(stoodOnObject + ObjectConfig.YOffset),
                Config.Stream.GetSingle(stoodOnObject + ObjectConfig.ZOffset),
            };

            float[] currentObjectOffset = new float[]
            {
                currentObjectPos[0] - platformPos[0],
                currentObjectPos[1] - platformPos[1],
                currentObjectPos[2] - platformPos[2],
            };

            short[] platformAngularVelocity = new short[]
            {
                (short)Config.Stream.GetInt32(stoodOnObject + ObjectConfig.PitchVelocityOffset),
                (short)Config.Stream.GetInt32(stoodOnObject + ObjectConfig.YawVelocityOffset),
                (short)Config.Stream.GetInt32(stoodOnObject + ObjectConfig.RollVelocityOffset),
            };

            short[] platformFacingAngle = new short[]
            {
                Config.Stream.GetInt16(stoodOnObject + ObjectConfig.PitchFacingOffset),
                Config.Stream.GetInt16(stoodOnObject + ObjectConfig.YawFacingOffset),
                Config.Stream.GetInt16(stoodOnObject + ObjectConfig.RollFacingOffset),
            };

            short[] rotation = new short[]
            {
                (short)(platformFacingAngle[0] - platformAngularVelocity[0]),
                (short)(platformFacingAngle[1] - platformAngularVelocity[1]),
                (short)(platformFacingAngle[2] - platformAngularVelocity[2]),
            };

            float[,] displaceMatrix = new float[4,4];
            float[] relativeOffset = new float[3];
            float[] newObjectOffset = new float[3];

            mtxf_rotate_zxy_and_translate(displaceMatrix, currentObjectOffset, rotation);
            linear_mtxf_transpose_mul_vec3f(displaceMatrix, relativeOffset, currentObjectOffset);

            rotation[0] = platformFacingAngle[0];
            rotation[1] = platformFacingAngle[1];
            rotation[2] = platformFacingAngle[2];

            mtxf_rotate_zxy_and_translate(displaceMatrix, currentObjectOffset, rotation);
            linear_mtxf_transpose_mul_vec3f(displaceMatrix, newObjectOffset, relativeOffset);

            float[] netDisplacement = new float[]
            {
                newObjectOffset[0] - currentObjectOffset[0],
                newObjectOffset[1] - currentObjectOffset[1],
                newObjectOffset[2] - currentObjectOffset[2],
            };

            return (netDisplacement[0], netDisplacement[1], netDisplacement[2]);
        }

        private static void mtxf_rotate_zxy_and_translate(float[,] dest, float[] translate, short[] rotate)
        {
            float sx = InGameTrigUtilities.InGameSine(rotate[0]);
            float cx = InGameTrigUtilities.InGameCosine(rotate[0]);

            float sy = InGameTrigUtilities.InGameSine(rotate[1]);
            float cy = InGameTrigUtilities.InGameCosine(rotate[1]);

            float sz = InGameTrigUtilities.InGameSine(rotate[2]);
            float cz = InGameTrigUtilities.InGameCosine(rotate[2]);

            dest[0,0] = cy * cz + sx * sy * sz;
            dest[1,0] = -cy * sz + sx * sy * cz;
            dest[2,0] = cx * sy;
            dest[3,0] = translate[0];

            dest[0,1] = cx * sz;
            dest[1,1] = cx * cz;
            dest[2,1] = -sx;
            dest[3,1] = translate[1];

            dest[0,2] = -sy * cz + sx * cy * sz;
            dest[1,2] = sy * sz + sx * cy * cz;
            dest[2,2] = cx * cy;
            dest[3,2] = translate[2];

            dest[0,3] = dest[1,3] = dest[2,3] = 0.0f;
            dest[3,3] = 1.0f;
        }

        private static void linear_mtxf_transpose_mul_vec3f(float[,] m, float[] dst, float[] v)
        {
            for (int i = 0; i < 3; i++)
            {
                dst[i] = m[i,0] * v[0] + m[i,1] * v[1] + m[i,2] * v[2];
            }
        }

        // Triangle methods

        static short lower_cell_index(short t)
        {
            short index;

            // Move from range [-0x2000, 0x2000) to [0, 0x4000)
            t += 0x2000;
            if (t < 0)
                t = 0;

            // [0, 16)
            index = (short)(t / 0x400);

            // Include extra cell if close to boundary
            if (t % 0x400 < 50)
                index -= 1;

            if (index < 0)
                index = 0;

            // Potentially > 15, but since the upper index is <= 15, not exploitable
            return index;
        }

        static short upper_cell_index(short t)
        {
            short index;

            // Move from range [-0x2000, 0x2000) to [0, 0x4000)
            t += 0x2000;
            if (t < 0)
                t = 0;

            // [0, 16)
            index = (short)(t / 0x400);

            // Include extra cell if close to boundary
            if (t % 0x400 > 0x400 - 50)
                index += 1;

            if (index > 15)
                index = 15;

            // Potentially < 0, but since lower index is >= 0, not exploitable
            return index;
        }
    }
}