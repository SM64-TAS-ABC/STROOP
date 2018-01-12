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
        private readonly static Func<uint, string> DEFAULT_GETTER = (uint address) => "UNIMPLEMENTED";
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
                    /*
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
                    */
                    break;

                case "MarioVerticalDistanceToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float objY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                        float yDist = marioY - objY;
                        return yDist.ToString();
                    };
                    /*
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
                    */
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
                    break;

                case "MarioVerticalDistanceToObjectHome":
                    getterFunction = (uint objAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float homeY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.HomeYOffset);
                        float yDist = marioY - homeY;
                        return yDist.ToString();
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
                    break;

                case "VerticalObjectDistanceToHome":
                    getterFunction = (uint objAddress) =>
                    {
                        float objY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                        float homeY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.HomeYOffset);
                        float yDist = objY - homeY;
                        return yDist.ToString();
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

                case "AngleMarioToObject":
                    getterFunction = (uint objAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        Position objPos = GetObjectPosition(objAddress);
                        double angleToObject = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, objPos.X, objPos.Z);
                        return MoreMath.NormalizeAngleDouble(angleToObject).ToString();
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
                    break;

                case "MarioHitboxAwayFromObject":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "MarioHitboxAboveObject":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "MarioHitboxBelowObject":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "MarioHitboxOverlapsObject":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "ObjectRngCallsPerFrame":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Pendulum

                case "PendulumAmplitude":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "PendulumSwingIndex":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Waypoint

                case "ObjectDotProductToWaypoint":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "ObjectDistanceToWaypointPlane":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "ObjectDistanceToWaypoint":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Racing Penguin

                case "RacingPenguinEffortTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinEffortChange":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinMinHSpeed":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinHSpeedTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinDiffHSpeedTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinProgress":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinProgressDiff":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "RacingPenguinProgressDiffDelta":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Koopa the Quick

                case "KoopaTheQuickHSpeedTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "KoopaTheQuickHSpeedChange":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "KoopaTheQuick1Progress":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "KoopaTheQuick2Progress":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Fly Guy

                case "FlyGuyZone":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "FlyGuyRelativeHeight":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "FlyGuyNextHeightDiff":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "FlyGuyMinHeight":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "FlyGuyMaxHeight":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Bob-omb

                case "BobombBloatSize":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "BobombRadius":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "BobombSpaceBetween":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Scuttlebug

                case "ScuttlebugDeltaAngleToTarget":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                // Object specific vars - Ghost

                case "MarioGhostVerticalDistance":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "MarioGhostHorizontalDistance":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "MarioGhostForwardsDistance":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;

                case "MarioGhostSidewaysDistance":
                    getterFunction = (uint objAddress) =>
                    {
                        return "UNIMP2";
                    };
                    break;
                    
                // Mario vars

                case "DeFactoSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        uint floorTri = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
                        float yNorm = floorTri == 0 ? 1 : Config.Stream.GetSingle(floorTri + Config.TriangleOffsets.NormY);
                        float hSpeed = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);

                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.FloorYOffset);
                        float distAboveFloor = marioY - floorY;

                        float defactoSpeed = distAboveFloor == 0 ? hSpeed * yNorm : hSpeed;
                        return defactoSpeed.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        double? defactoSpeedNullable = ParsingUtilities.ParseDoubleNullable(stringValue);
                        if (!defactoSpeedNullable.HasValue) return false;
                        double defactoSpeed = defactoSpeedNullable.Value;

                        uint floorTri = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
                        float yNorm = floorTri == 0 ? 1 : Config.Stream.GetSingle(floorTri + Config.TriangleOffsets.NormY);
                        float hSpeed = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);

                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.FloorYOffset);
                        float distAboveFloor = marioY - floorY;

                        float newHSpeed = distAboveFloor == 0 ? (float)defactoSpeed / yNorm : hSpeed;
                        return Config.Stream.SetValue(newHSpeed, Config.Mario.StructAddress + Config.Mario.HSpeedOffset);
                    };
                    break;

                case "SlidingSpeed":
                    getterFunction = (uint dummy) =>
                    {
                        float xSlidingSpeed = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedXOffset);
                        float zSlidingSpeed = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedZOffset);
                        double hSlidingSpeed = MoreMath.GetHypotenuse(xSlidingSpeed, zSlidingSpeed);
                        return hSlidingSpeed.ToString();
                    };
                    break;

                case "MovementX":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        return movementX.ToString();
                    };
                    break;

                case "MovementY":
                    getterFunction = (uint dummy) =>
                    {
                        float endY = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        return movementY.ToString();
                    };
                    break;

                case "MovementZ":
                    getterFunction = (uint dummy) =>
                    {
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        return movementZ.ToString();
                    };
                    break;

                case "MovementForwards":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        (double movementSideways, double movementForwards) =
                            MoreMath.GetComponentsFromVectorRelatively(movementHorizontal, movementAngle, marioAngle);
                        return movementForwards.ToString();
                    };
                    break;

                case "MovementSideways":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        (double movementSideways, double movementForwards) =
                            MoreMath.GetComponentsFromVectorRelatively(movementHorizontal, movementAngle, marioAngle);
                        return movementSideways.ToString();
                    };
                    break;

                case "MovementHorizontal":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementHorizontal = MoreMath.GetHypotenuse(movementX, movementZ);
                        return movementHorizontal.ToString();
                    };
                    break;

                case "MovementTotal":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endY = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementTotal = MoreMath.GetHypotenuse(movementX, movementY, movementZ);
                        return movementTotal.ToString();
                    };
                    break;

                case "MovementAngle":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        double movementAngle = MoreMath.AngleTo_AngleUnits(movementX, movementZ);
                        return movementAngle.ToString();
                    };
                    break;

                case "QFrameCountEstimate":
                    getterFunction = (uint dummy) =>
                    {
                        float endX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x10);
                        float startX = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x1C);
                        float movementX = endX - startX;
                        float endY = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x14);
                        float startY = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x20);
                        float movementY = endY - startY;
                        float endZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x18);
                        float startZ = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x24);
                        float movementZ = endZ - startZ;
                        float oldHSpeed = Config.Stream.GetSingle(Config.HackedAreaAddress + 0x28);
                        double qframes = Math.Abs(Math.Round(Math.Sqrt(movementX * movementX + movementZ * movementZ) / (oldHSpeed / 4)));
                        if (qframes > 4) qframes = double.NaN;
                        return qframes.ToString();
                    };
                    break;

                case "DeltaYawIntendedFacing":
                    getterFunction = (uint dummy) =>
                    {
                        ushort marioYawFacing = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        ushort marioYawFacingTruncated = MoreMath.NormalizeAngleTruncated(marioYawFacing);
                        ushort marioYawIntended = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawIntendedOffset);
                        ushort marioYawIntendedTruncated = MoreMath.NormalizeAngleTruncated(marioYawIntended);
                        int deltaYaw = marioYawIntendedTruncated - marioYawFacingTruncated;
                        return MoreMath.NormalizeAngleDoubleSigned(deltaYaw).ToString();
                    };
                    break;

                case "FallHeight":
                    getterFunction = (uint dummy) =>
                    {
                        float peakHeight = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset);
                        float floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.FloorYOffset);
                        float fallHeight = peakHeight - floorY;
                        return fallHeight.ToString();
                    };
                    break;
                    
                // HUD vars

                case "HudTimeText":
                    getterFunction = (uint dummy) =>
                    {
                        ushort time = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Hud.TimeOffset);
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
                        return Config.Stream.SetValue(timeUShort, Config.Mario.StructAddress + Config.Hud.TimeOffset);
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
                        return SetCameraPosition(newCameraX, newCameraY, newCameraZ);
                    };
                    break;

                // Triangle vars

                case "Classification":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
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
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
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
                        ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double angleDiff = marioAngle - uphillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "DownHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double downhillAngle = MoreMath.ReverseAngle(uphillAngle);
                        double angleDiff = marioAngle - downhillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "LeftHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double lefthillAngle = MoreMath.RotateAngleCCW(uphillAngle, 16384);
                        double angleDiff = marioAngle - lefthillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "RightHillDeltaAngle":
                    getterFunction = (uint triAddress) =>
                    {
                        ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
                        double uphillAngle = GetTriangleUphillAngle(triAddress);
                        double righthillAngle = MoreMath.RotateAngleCW(uphillAngle, 16384);
                        double angleDiff = marioAngle - righthillAngle;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "DistanceAboveFloor":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float floorY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.FloorYOffset);
                        float distAboveFloor = marioY - floorY;
                        return distAboveFloor.ToString();
                    };
                    break;

                case "DistanceBelowCeiling":
                    getterFunction = (uint dummy) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float ceilingY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.CeilingYOffset);
                        float distBelowCeiling = ceilingY - marioY;
                        return distBelowCeiling.ToString();
                    };
                    break;

                case "NormalDistAway":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double normalDistAway =
                            marioPos.X * triStruct.NormX +
                            marioPos.Y * triStruct.NormY +
                            marioPos.Z * triStruct.NormZ +
                            triStruct.NormOffset;
                        return normalDistAway.ToString();
                    };
                    break;

                case "VerticalDistAway":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double verticalDistAway =
                            marioPos.Y + (marioPos.X * triStruct.NormX + marioPos.Z * triStruct.NormZ + triStruct.NormOffset) / triStruct.NormY;
                        return verticalDistAway.ToString();
                    };
                    break;

                case "HeightOnSlope":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double heightOnTriangle =
                            (-marioPos.X * triStruct.NormX - marioPos.Z * triStruct.NormZ - triStruct.NormOffset) / triStruct.NormY;
                        return heightOnTriangle.ToString();
                    };
                    break;

                case "ObjectTriCount":
                    getterFunction = (uint dummy) =>
                    {
                        int totalTriangleCount = Config.Stream.GetInt32(Config.Triangle.TotalTriangleCountAddress);
                        int levelTriangleCount = Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress);
                        int objectTriangleCount = totalTriangleCount - levelTriangleCount;
                        return objectTriangleCount.ToString();
                    };
                    break;

                case "ObjectNodeCount":
                    getterFunction = (uint dummy) =>
                    {
                        int totalNodeCount = Config.Stream.GetInt32(Config.Triangle.TotalNodeCountAddress);
                        int levelNodeCount = Config.Stream.GetInt32(Config.Triangle.LevelNodeCountAddress);
                        int objectNodeCount = totalNodeCount - levelNodeCount;
                        return objectNodeCount.ToString();
                    };
                    break;

                case "DistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double distToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X1, triStruct.Y1, triStruct.Z1);
                        return distToV1.ToString();
                    };
                    break;

                case "XDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double xDistToV1 = marioX - triStruct.X1;
                        return xDistToV1.ToString();
                    };
                    break;

                case "YDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double yDistToV1 = marioY - triStruct.Y1;
                        return yDistToV1.ToString();
                    };
                    break;

                case "ZDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double zDistToV1 = marioZ - triStruct.Z1;
                        return zDistToV1.ToString();
                    };
                    break;

                case "HDistanceToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double hDistToV1 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        return hDistToV1.ToString();
                    };
                    break;

                case "DistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double distToV2 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X2, triStruct.Y2, triStruct.Z2);
                        return distToV2.ToString();
                    };
                    break;

                case "XDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double xDistToV2 = marioX - triStruct.X2;
                        return xDistToV2.ToString();
                    };
                    break;

                case "YDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double yDistToV2 = marioY - triStruct.Y2;
                        return yDistToV2.ToString();
                    };
                    break;

                case "ZDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double zDistToV2 = marioZ - triStruct.Z2;
                        return zDistToV2.ToString();
                    };
                    break;

                case "HDistanceToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double hDistToV2 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        return hDistToV2.ToString();
                    };
                    break;

                case "DistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double distToV3 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Y, marioPos.Z, triStruct.X3, triStruct.Y3, triStruct.Z3);
                        return distToV3.ToString();
                    };
                    break;

                case "XDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double xDistToV3 = marioX - triStruct.X3;
                        return xDistToV3.ToString();
                    };
                    break;

                case "YDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double yDistToV3 = marioY - triStruct.Y3;
                        return yDistToV3.ToString();
                    };
                    break;

                case "ZDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        float marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double zDistToV3 = marioZ - triStruct.Z3;
                        return zDistToV3.ToString();
                    };
                    break;

                case "HDistanceToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double hDistToV3 = MoreMath.GetDistanceBetween(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        return hDistToV3.ToString();
                    };
                    break;

                case "DistanceToLine12":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double signedDistToLine12 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 1, 2);
                        return signedDistToLine12.ToString();
                    };
                    break;

                case "DistanceToLine23":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double signedDistToLine23 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 2, 3);
                        return signedDistToLine23.ToString();
                    };
                    break;

                case "DistanceToLine13":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double signedDistToLine31 = MoreMath.GetSignedDistanceFromPointToLine(
                            marioPos.X, marioPos.Z,
                            triStruct.X1, triStruct.Z1,
                            triStruct.X2, triStruct.Z2,
                            triStruct.X3, triStruct.Z3, 3, 1);
                        return signedDistToLine31.ToString();
                    };
                    break;

                case "AngleMarioToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        return angleToV1.ToString();
                    };
                    break;

                case "DeltaAngleMarioToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleToV1 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X1, triStruct.Z1);
                        double angleDiff = marioPos.Angle.Value - angleToV1;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "AngleV1ToMario":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV1ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, marioPos.X, marioPos.Z);
                        return angleV1ToMario.ToString();
                    };
                    break;

                case "AngleMarioToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleToV2 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        return angleToV2.ToString();
                    };
                    break;

                case "DeltaAngleMarioToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleToV2 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X2, triStruct.Z2);
                        double angleDiff = marioPos.Angle.Value - angleToV2;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "AngleV2ToMario":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV2ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, marioPos.X, marioPos.Z);
                        return angleV2ToMario.ToString();
                    };
                    break;

                case "AngleMarioToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleToV3 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        return angleToV3.ToString();
                    };
                    break;

                case "DeltaAngleMarioToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleToV3 = MoreMath.AngleTo_AngleUnits(
                            marioPos.X, marioPos.Z, triStruct.X3, triStruct.Z3);
                        double angleDiff = marioPos.Angle.Value - angleToV3;
                        return MoreMath.NormalizeAngleDoubleSigned(angleDiff).ToString();
                    };
                    break;

                case "AngleV3ToMario":
                    getterFunction = (uint triAddress) =>
                    {
                        Position marioPos = GetMarioPosition();
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV3ToMario = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, marioPos.X, marioPos.Z);
                        return angleV3ToMario.ToString();
                    };
                    break;

                case "AngleV1ToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV1ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X2, triStruct.Z2);
                        return angleV1ToV2.ToString();
                    };
                    break;

                case "AngleV2ToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV2ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X1, triStruct.Z1);
                        return angleV2ToV1.ToString();
                    };
                    break;

                case "AngleV2ToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV2ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X2, triStruct.Z2, triStruct.X3, triStruct.Z3);
                        return angleV2ToV3.ToString();
                    };
                    break;

                case "AngleV3ToV2":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV3ToV2 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X2, triStruct.Z2);
                        return angleV3ToV2.ToString();
                    };
                    break;

                case "AngleV1ToV3":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV1ToV3 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X1, triStruct.Z1, triStruct.X3, triStruct.Z3);
                        return angleV1ToV3.ToString();
                    };
                    break;

                case "AngleV3ToV1":
                    getterFunction = (uint triAddress) =>
                    {
                        TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
                        double angleV3ToV1 = MoreMath.AngleTo_AngleUnits(
                            triStruct.X3, triStruct.Z3, triStruct.X1, triStruct.Z1);
                        return angleV3ToV1.ToString();
                    };
                    break;

                // Action vars

                case "ActionDescription":
                    getterFunction = (uint dummy) =>
                    {
                        uint action = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset);
                        string actionDescription = Config.MarioActions.GetActionName(action);
                        return actionDescription;
                    };
                    break;

                case "PrevActionDescription":
                    getterFunction = (uint dummy) =>
                    {
                        uint prevAction = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.PrevActionOffset);
                        string prevActionDescription = Config.MarioActions.GetActionName(prevAction);
                        return prevActionDescription;
                    };
                    break;

                case "MarioAnimationDescription":
                    getterFunction = (uint dummy) =>
                    {
                        uint marioObjRef = Config.Stream.GetUInt32(Config.Mario.ObjectReferenceAddress);
                        short animation = Config.Stream.GetInt16(marioObjRef + Config.Mario.ObjectAnimationOffset);
                        string animationDescription = Config.MarioAnimations.GetAnimationName(animation);
                        return animationDescription;
                    };
                    break;

                // Water vars

                case "WaterAboveMedian":
                    getterFunction = (uint dummy) =>
                    {
                        short waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + Config.Mario.WaterLevelOffset);
                        short waterLevelMedian = Config.Stream.GetInt16(Config.WaterLevelMedianAddress);
                        double waterAboveMedian = waterLevel - waterLevelMedian;
                        return waterAboveMedian.ToString();
                    };
                    break;

                case "MarioAboveWater":
                    getterFunction = (uint dummy) =>
                    {
                        short waterLevel = Config.Stream.GetInt16(Config.Mario.StructAddress + Config.Mario.WaterLevelOffset);
                        float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
                        float marioAboveWater = marioY - waterLevel;
                        return marioAboveWater.ToString();
                    };
                    setterFunction = (string stringValue, uint dummy) =>
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
                    getterFunction = (uint dummy) =>
                    {
                        ushort rngValue = Config.Stream.GetUInt16(Config.RngAddress);
                        string rngIndexString = RngIndexer.GetRngIndexString(rngValue);
                        return rngIndexString;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
                    {
                        int? index = ParsingUtilities.ParseIntNullable(stringValue);
                        if (!index.HasValue) return false;
                        ushort rngValue = RngIndexer.GetRngValue(index.Value);
                        return Config.Stream.SetValue(rngValue, Config.RngAddress);
                    };
                    break;

                case "RngCallsPerFrame":
                    getterFunction = (uint dummy) =>
                    {
                        ushort preRng = Config.Stream.GetUInt16(Config.HackedAreaAddress + 0x0C);
                        ushort currentRng = Config.Stream.GetUInt16(Config.HackedAreaAddress + 0x0E);
                        int rngDiff = RngIndexer.GetRngIndexDiff(preRng, currentRng);
                        return rngDiff.ToString();
                    };
                    break;

                case "NumberOfLoadedObjects":
                    getterFunction = (uint dummy) =>
                    {
                        int numberOfLoadedObjects = ObjectSlotsManager.Instance.ActiveObjectCount;
                        return numberOfLoadedObjects.ToString();
                    };
                    break;

                // Area vars

                case "CurrentAreaIndexMario":
                    getterFunction = (uint dummy) =>
                    {
                        uint currentAreaMario = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
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
                        return Config.Stream.SetValue(currentAreaAddressMario, Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
                    };
                    break;

                case "CurrentAreaIndex":
                    getterFunction = (uint dummy) =>
                    {
                        uint currentArea = Config.Stream.GetUInt32(Config.Area.CurrentAreaPointerAddress);
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
                        return Config.Stream.SetValue(currentAreaAddress, Config.Area.CurrentAreaPointerAddress);
                    };
                    break;

                case "AreaTerrainDescription":
                    getterFunction = (uint dummy) =>
                    {
                        short terrainType = Config.Stream.GetInt16(AreaManager.Instance.SelectedAreaAddress + Config.Area.TerrainTypeOffset);
                        string terrainDescription = AreaUtilities.GetTerrainDescription(terrainType);
                        return terrainDescription;
                    };
                    setterFunction = (string stringValue, uint dummy) =>
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
            float marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            float marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            float marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
            ushort marioAngle = Config.Stream.GetUInt16(Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            return new Position(marioX, marioY, marioZ, marioAngle);
        }

        private static bool SetMarioPosition(double? x, double? y, double? z, ushort? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, Config.Mario.StructAddress + Config.Mario.XOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, Config.Mario.StructAddress + Config.Mario.YOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, Config.Mario.StructAddress + Config.Mario.ZOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue((ushort)angle.Value, Config.Mario.StructAddress + Config.Mario.YawFacingOffset);
            return success;
        }

        private static Position GetObjectPosition(uint objAddress)
        {
            float objX = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectXOffset);
            float objY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
            float objZ = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectZOffset);
            ushort objAngle = Config.Stream.GetUInt16(objAddress + Config.ObjectSlots.YawFacingOffset);
            return new Position(objX, objY, objZ, objAngle);
        }

        private static bool SetObjectPosition(uint objAddress, double? x, double? y, double? z, ushort? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, objAddress + Config.ObjectSlots.ObjectXOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, objAddress + Config.ObjectSlots.ObjectYOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, objAddress + Config.ObjectSlots.ObjectZOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue((ushort)angle.Value, objAddress + Config.ObjectSlots.YawFacingOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue((ushort)angle.Value, objAddress + Config.ObjectSlots.YawMovingOffset);
            return success;
        }

        private static Position GetObjectHomePosition(uint objAddress)
        {
            float objX = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.HomeXOffset);
            float objY = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.HomeYOffset);
            float objZ = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.HomeZOffset);
            return new Position(objX, objY, objZ);
        }

        private static Position GetCameraPosition()
        {
            float cameraX = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.XOffset);
            float cameraY = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.YOffset);
            float cameraZ = Config.Stream.GetSingle(Config.Camera.CameraStructAddress + Config.Camera.ZOffset);
            ushort cameraAngle = Config.Stream.GetUInt16(Config.Camera.CameraStructAddress + Config.Camera.YawFacingOffset);
            return new Position(cameraX, cameraY, cameraZ, cameraAngle);
        }

        private static bool SetCameraPosition(double? x, double? y, double? z, ushort? angle = null)
        {
            bool success = true;
            if (x.HasValue) success &= Config.Stream.SetValue((float)x.Value, Config.Camera.CameraStructAddress + Config.Camera.XOffset);
            if (y.HasValue) success &= Config.Stream.SetValue((float)y.Value, Config.Camera.CameraStructAddress + Config.Camera.YOffset);
            if (z.HasValue) success &= Config.Stream.SetValue((float)z.Value, Config.Camera.CameraStructAddress + Config.Camera.ZOffset);
            if (angle.HasValue) success &= Config.Stream.SetValue((ushort)angle.Value, Config.Camera.CameraStructAddress + Config.Camera.YawFacingOffset);
            return success;
        }

        // Triangle utilitiy methods

        private static int GetClosestTriangleVertexIndex(uint triAddress)
        {
            Position marioPos = GetMarioPosition();
            TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
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
            TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
            if (closestTriangleVertexIndex == 1) return new Position(triStruct.X1, triStruct.Y1, triStruct.Z1);
            if (closestTriangleVertexIndex == 2) return new Position(triStruct.X2, triStruct.Y2, triStruct.Z2);
            if (closestTriangleVertexIndex == 3) return new Position(triStruct.X3, triStruct.Y3, triStruct.Z3);
            throw new ArgumentOutOfRangeException();
        }

        private static double GetTriangleUphillAngle(uint triAddress)
        {
            TriangleStruct triStruct = TriangleManager.Instance.GetTriangleStruct(triAddress);
            double uphillAngleRadians = Math.PI + Math.Atan2(triStruct.NormX, triStruct.NormZ);
            if (triStruct.NormX == 0 && triStruct.NormZ == 0) uphillAngleRadians = double.NaN;
            if (triStruct.IsCeiling()) uphillAngleRadians += Math.PI;
            return MoreMath.RadiansToAngleUnits(uphillAngleRadians);
        }
    }
}