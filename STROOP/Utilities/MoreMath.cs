using OpenTK;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace STROOP.Utilities
{
    public static class MoreMath
    {
        public static int Sign(double value)
        {
            if (value == 0 || double.IsNaN(value)) return 0;
            return value > 0 ? 1 : -1;
        }

        public static int Min(params int[] values)
        {
            if (values.Length == 0) return 0;
            int min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min) min = values[i];
            }
            return min;
        }

        public static int Max(params int[] values)
        {
            if (values.Length == 0) return 0;
            int max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > max) max = values[i];
            }
            return max;
        }

        public static double Min(params double[] values)
        {
            if (values.Length == 0) return 0;
            double min = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] < min) min = values[i];
            }
            return min;
        }

        public static double Max(params double[] values)
        {
            if (values.Length == 0) return 0;
            double max = values[0];
            for (int i = 1; i < values.Length; i++)
            {
                if (values[i] > max) max = values[i];
            }
            return max;
        }

        public static double Average(params double[] values)
        {
            if (values.Length == 0) return 0;
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
            return sum / values.Length;
        }

        public static double GetHypotenuse(double x, double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        public static double GetHypotenuse(double x, double y, double z)
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static double GetDistanceBetween(double x1, double y1, double z1, double x2, double y2, double z2)
        {
            double dx, dy, dz;
            dx = x1 - x2;
            dy = y1 - y2;
            dz = z1 - z2;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double GetDistanceBetween(double x1, double z1, double x2, double z2)
        {
            double dx, dz;
            dx = x1 - x2;
            dz = z1 - z2;
            return Math.Sqrt(dx * dx + dz * dz);
        }

        public static (double xDist, double zDist) GetComponentsFromVector(double magnitude, double angle)
        {
            double radians = AngleUnitsToRadians(angle);
            double xComponent = Math.Sin(radians);
            double zComponent = Math.Cos(radians);
            return (magnitude * xComponent, magnitude * zComponent);
        }

        public static (double x, double z) AddVectorToPoint(
            double magnitude, double angle, double x, double z)
        {
            (double xDist, double zDist) = GetComponentsFromVector(magnitude, angle);
            return (x + xDist, z + zDist);
        }

        public static (double x, double y, double z) AddVectorToPointWithPitch(
            double magnitude, double angle, double pitch, double x, double y, double z, bool clamp)
        {
            (double x2, double z2) = AddVectorToPoint(magnitude, angle, x, z);
            return OffsetSphericallyAboutPivot(x2, y, z2, 0, 0, pitch, x, y, z, clamp);
        }

        public static (double sidewaysDist, double forwardsDist) GetComponentsFromVectorRelatively(
            double magnitude, double vectorAngle, double baseAngle)
        {
            double rotatedAngle = NormalizeAngleDouble(vectorAngle - baseAngle);
            (double xComponent, double zComponent) = GetComponentsFromVector(magnitude, rotatedAngle);
            return (-1 * xComponent, zComponent);
        }

        public static (double sidewaysDist, double forwardsDist) GetSidewaysAndForwardsDist(
            double x1, double z1, double x2, double z2, double baseAngle)
        {
            double hDist = GetDistanceBetween(x1, z1, x2, z2);
            double angle = AngleTo_AngleUnits(x1, z1, x2, z2);
            return GetComponentsFromVectorRelatively(hDist, angle, baseAngle);
        }

        public static (double xDist, double zDist) GetAbsoluteComponents(
            double sidewaysDist, double forwardsDist, double relativeAngle)
        {
            double relX = sidewaysDist;
            double relZ = -1 * forwardsDist;
            double relDist = GetHypotenuse(relX, relZ);
            double relAngle = AngleTo_AngleUnits(relX, relZ);
            double absAngle = relativeAngle + ReverseAngle(relAngle);
            return GetComponentsFromVector(relDist, absAngle);
        }

        public static (double newXPos, double newZPos) GetRelativelyOffsettedPosition(
            double baseX, double baseZ, double baseAngle, double pointX, double pointZ,
            double? goalSidewaysDistNullable, double? goalForwardsDistNullable)
        {
            double hdist = GetDistanceBetween(baseX, baseZ, pointX, pointZ);
            double angle = AngleTo_AngleUnits(baseX, baseZ, pointX, pointZ);
            (double currentSidewaysDist, double currentForwardsDist) =
                GetComponentsFromVectorRelatively(hdist, angle, baseAngle);

            double goalSidewaysDist = goalSidewaysDistNullable ?? currentSidewaysDist;
            double goalForwardsDist = goalForwardsDistNullable ?? currentForwardsDist;

            (double xDist, double zDist) = GetAbsoluteComponents(goalSidewaysDist, goalForwardsDist, baseAngle);
            return (baseX + xDist, baseZ + zDist);
        }

        public static (double magnitude, double angle) GetVectorFromComponents(double xDist, double zDist)
        {
            double magnitude = Math.Sqrt(xDist * xDist + zDist * zDist);
            double angle = AngleTo_AngleUnits(0, 0, xDist, zDist);
            return (magnitude, angle);
        }

        public static (double magnitude, double angle) GetVectorFromCoordinates(
            double xFrom, double zFrom, double xTo, double zTo, bool usePositiveMagnitude)
        {
            double xDist = xTo - xFrom;
            double zDist = zTo - zFrom;
            (double magnitude, double angle) = GetVectorFromComponents(xDist, zDist);
            double adjustedMagnitude = usePositiveMagnitude ? magnitude : -1 * magnitude;
            double adjustedAngle = usePositiveMagnitude ? angle : ReverseAngle(angle);
            return (adjustedMagnitude, adjustedAngle);
        }

        public static (double x, double y, double z) ScaleVector3D(
            double xComp, double yComp, double zComp, double finalDist)
        {
            double magnitude = GetHypotenuse(xComp, yComp, zComp);
            if (magnitude == 0) return (finalDist, 0, 0);
            double multiplier = finalDist / magnitude;
            return (xComp * multiplier, yComp * multiplier, zComp * multiplier);
        }

        public static (double x, double z) ScaleVector2D(
            double xComp, double zComp, double finalDist)
        {
            double magnitude = GetHypotenuse(xComp, zComp);
            if (magnitude == 0) return (finalDist, 0);
            double multiplier = finalDist / magnitude;
            return (xComp * multiplier, zComp * multiplier);
        }

        public static double ScaleVector1D(
            double xComp, double finalDist)
        {
            return xComp >= 0 ? finalDist : -1 * finalDist;
        }

        public static (double x, double y, double z) ExtrapolateLine3D(
            double p1X, double p1Y, double p1Z, double p2X, double p2Y, double p2Z, double finalDist)
        {
            double diffX = p2X - p1X;
            double diffY = p2Y - p1Y;
            double diffZ = p2Z - p1Z;
            (double scaledX, double scaledY, double scaledZ) = ScaleVector3D(diffX, diffY, diffZ, finalDist);
            return (p1X + scaledX, p1Y + scaledY, p1Z + scaledZ);
        }

        public static (double x, double z) ExtrapolateLine2D(
            double p1X, double p1Z, double p2X, double p2Z, double finalDist)
        {
            double diffX = p2X - p1X;
            double diffZ = p2Z - p1Z;
            (double scaledX, double scaledZ) = ScaleVector2D(diffX, diffZ, finalDist);
            return (p1X + scaledX, p1Z + scaledZ);
        }

        public static double GetPositionAlongLine(double p1X, double p1Y, double p2X, double p2Y, double x)
        {
            double slope = (p2Y - p1Y) / (p2X - p1X);
            return (x - p1X) * slope + p1Y;
        }

        /** p2 is pivot. */
        public static (double x, double z) RotatePointAboutPointToAngle(
            double p1X, double p1Z, double p2X, double p2Z, double finalAngle)
        {
            double dist = GetDistanceBetween(p1X, p1Z, p2X, p2Z);
            (double xDiff, double zDiff) = GetComponentsFromVector(dist, finalAngle);
            return (p2X + xDiff, p2Z + zDiff);
        }

        /** p2 is pivot. */
        public static (double x, double z) RotatePointAboutPointAnAngularDistance(
            double p1X, double p1Z, double p2X, double p2Z, double angularDistance)
        {
            double dist = GetDistanceBetween(p1X, p1Z, p2X, p2Z);
            double angle = AngleTo_AngleUnits(p1X, p1Z, p2X, p2Z);
            (double xDiff, double zDiff) = GetComponentsFromVector(dist, angle + angularDistance);
            return (p2X + xDiff, p2Z + zDiff);
        }

        public static double GetDistanceFromPointToLine(
            double pX, double pZ, double v1X, double v1Z, double v2X, double v2Z)
        {
            double numerator = Math.Abs((v2Z - v1Z) * pX - (v2X - v1X) * pZ + v2X * v1Z - v2Z * v1X);
            double denominator = GetDistanceBetween(v1X, v1Z, v2X, v2Z);
            return numerator / denominator;
        }

        public static double GetSignedDistanceFromPointToLine(
            double pX, double pZ, double v1X, double v1Z, double v2X, double v2Z, double v3X, double v3Z, int p1Index, int p2Index,
            TriangleClassification classification, bool? misalignmentOffsetNullable = null)
        {
            pX = PuUtilities.GetRelativeCoordinate(pX);
            pZ = PuUtilities.GetRelativeCoordinate(pZ);

            double[] vX = new double[] { v1X, v2X, v3X };
            double[] vZ = new double[] { v1Z, v2Z, v3Z };

            double p1X = vX[p1Index - 1];
            double p1Z = vZ[p1Index - 1];
            double p2X = vX[p2Index - 1];
            double p2Z = vZ[p2Index - 1];

            double dist = MoreMath.GetDistanceFromPointToLine(pX, pZ, p1X, p1Z, p2X, p2Z);
            bool leftOfLine = MoreMath.IsPointLeftOfLine(pX, pZ, p1X, p1Z, p2X, p2Z);
            bool floorTri = MoreMath.IsPointLeftOfLine(v3X, v3Z, v1X, v1Z, v2X, v2Z);
            bool onSideOfLineTowardsTri = floorTri == leftOfLine;
            double signedDist = dist * (onSideOfLineTowardsTri ? 1 : -1);

            bool misalignmentOffset = misalignmentOffsetNullable ?? SavedSettingsConfig.UseMisalignmentOffsetForDistanceToLine;
            if (misalignmentOffset && classification != TriangleClassification.Wall)
            {
                if (p1X == p2X)
                {
                    bool thirdPointOnLeft = p1Z >= p2Z == floorTri;
                    if ((thirdPointOnLeft && p1X >= 0) || (!thirdPointOnLeft && p1X <= 0))
                    {
                        signedDist += 1;
                    }
                }
                else if (p1Z == p2Z)
                {
                    bool thirdPointOnTop = p1X <= p2X == floorTri;
                    if ((thirdPointOnTop && p1Z >= 0) || (!thirdPointOnTop && p1Z <= 0))
                    {
                        signedDist += 1;
                    }
                }
            }

            return signedDist;
        }

        public static bool IsPointInsideTriangle(
            double pX, double pZ, double v1X, double v1Z, double v2X, double v2Z, double v3X, double v3Z)
        {
            bool leftOf12 = IsPointLeftOfLine(pX, pZ, v1X, v1Z, v2X, v2Z);
            bool leftOf23 = IsPointLeftOfLine(pX, pZ, v2X, v2Z, v3X, v3Z);
            bool leftOf31 = IsPointLeftOfLine(pX, pZ, v3X, v3Z, v1X, v1Z);

            bool rightOf12 = IsPointRightOfLine(pX, pZ, v1X, v1Z, v2X, v2Z);
            bool rightOf23 = IsPointRightOfLine(pX, pZ, v2X, v2Z, v3X, v3Z);
            bool rightOf31 = IsPointRightOfLine(pX, pZ, v3X, v3Z, v1X, v1Z);

            return (leftOf12 && leftOf23 && leftOf31) || (rightOf12 && rightOf23 && rightOf31);
        }

        public static bool IsPointLeftOfLine(
            double pX, double pZ, double v1X, double v1Z, double v2X, double v2Z)
        {
            return (v1Z - pZ) * (v2X - v1X) >= (v1X - pX) * (v2Z - v1Z);
        }

        public static bool IsPointRightOfLine(
            double pX, double pZ, double v1X, double v1Z, double v2X, double v2Z)
        {
            return (v1Z - pZ) * (v2X - v1X) <= (v1X - pX) * (v2Z - v1Z);
        }

        public static double GetPlaneDistanceBetweenPoints(
            double pointX, double pointY, double pointZ, double startX, double startY, double startZ, double endX, double endY, double endZ)
        {
            double startToPointX = pointX - startX;
            double startToPointY = pointY - startY;
            double startToPointZ = pointZ - startZ;
            double startToEndX = endX - startX;
            double startToEndY = endY - startY;
            double startToEndZ = endZ - startZ;

            double dotProduct = GetDotProduct(startToPointX, startToPointY, startToPointZ, startToEndX, startToEndY, startToEndZ);
            double prevToNextDist = GetDistanceBetween(startX, startY, startZ, endX, endY, endZ);
            double planeDistance = dotProduct / prevToNextDist;

            return planeDistance;
        }

        public static double ReflectValueAboutValue(double value, double pivot)
        {
            double diff = pivot - value;
            return pivot + diff;
        }

        public static double NormalizeAngleDouble(double angle)
        {
            return NonNegativeModulus(angle, 65536);
        }

        public static double NormalizeAngleDoubleSigned(double angle)
        {
            return MaybeNegativeModulus(angle, 65536);
        }

        public static ushort NormalizeAngleUshort(double angle)
        {
            double nonNegative = NormalizeAngleDouble(angle);
            return (ushort)(Math.Round(nonNegative) % 65536);
        }

        public static short NormalizeAngleShort(double angle)
        {
            ushort angleUshort = NormalizeAngleUshort(angle);
            short angleShort;
            if (angleUshort > 32767)
            {
                angleShort = (short)(angleUshort - 65536);
            }
            else
            {
                angleShort = (short)angleUshort;
            }
            return angleShort;
        }

        public static ushort NormalizeAngleTruncated(double angle)
        {
            angle = NormalizeAngleDouble(angle);
            ushort angleUshort = (ushort)angle;
            ushort angleTruncated = (ushort)(angleUshort - (angleUshort % 16));
            return angleTruncated;
        }

        public static double NormalizeAngleUsingType(double angle, Type type)
        {
            if (type == typeof(short)) return MaybeNegativeModulus(angle, 1.0 + short.MaxValue - short.MinValue);
            if (type == typeof(ushort)) return NonNegativeModulus(angle, 1.0 + ushort.MaxValue - ushort.MinValue);
            if (type == typeof(int)) return MaybeNegativeModulus(angle, 1.0 + int.MaxValue - int.MinValue);
            if (type == typeof(uint)) return NonNegativeModulus(angle, 1.0 + uint.MaxValue - uint.MinValue);
            throw new ArgumentOutOfRangeException("Cannot call NormalizeAngleUsingType with type " + type);
        }

        public static double NormalizeAngle45Degrees(double angle)
        {
            int divided = NormalizeAngleUshort(angle + 4096) / 8192;
            return divided * 8192;
        }

        public static double AngleTo_Radians(double xFrom, double zFrom, double xTo, double zTo)
        {
            return Math.Atan2(xTo - xFrom, zTo - zFrom);
        }

        public static double AngleTo_Radians(double xTo, double zTo)
        {
            return AngleTo_Radians(0, 0, xTo, zTo);
        }

        public static double AngleTo_AngleUnits(double xFrom, double zFrom, double xTo, double zTo)
        {
            return RadiansToAngleUnits(AngleTo_Radians(xFrom, zFrom, xTo, zTo));
        }

        public static double AngleTo_AngleUnits(double xTo, double zTo)
        {
            return AngleTo_AngleUnits(0, 0, xTo, zTo);
        }

        public static double? AngleTo_AngleUnitsNullable(double xTo, double zTo)
        {
            if (xTo == 0 && zTo == 0) return null;
            return AngleTo_AngleUnits(0, 0, xTo, zTo);
        }

        public static ushort AngleTo_AngleUnitsRounded(double xFrom, double zFrom, double xTo, double zTo)
        {
            return RadiansToAngleUnitsRounded(AngleTo_Radians(xFrom, zFrom, xTo, zTo));
        }

        public static ushort AngleTo_AngleUnitsRounded(double xTo, double zTo)
        {
            return AngleTo_AngleUnitsRounded(0, 0, xTo, zTo);
        }

        public static (double radius, double theta, double phi) EulerToSpherical_Radians(double x, double y, double z)
        {
            double radius = Math.Sqrt(x * x + y * y + z * z);
            double theta = Math.Atan2(x, z);
            double phi = radius == 0 ? 0 : Math.Asin(y / radius);
            return (radius, theta, phi);
        }

        public static (double radius, double theta, double phi) EulerToSpherical_AngleUnits(double x, double y, double z)
        {
            double radius, thetaRadians, phiRadians;
            (radius, thetaRadians, phiRadians) = EulerToSpherical_Radians(x, y, z);
            double thetaAngleUnits = RadiansToAngleUnits(thetaRadians);
            double phiAngleUnits = RadiansToAngleUnits(phiRadians);
            return (radius, thetaAngleUnits, phiAngleUnits);
        }

        public static (double x, double y, double z) SphericalToEuler_Radians(double radius, double theta, double phi)
        {
            double x = radius * Math.Sin(theta) * Math.Cos(phi);
            double y = radius * Math.Sin(phi);
            double z = radius * Math.Cos(theta) * Math.Cos(phi);
            return (x, y, z);
        }

        public static (double x, double y, double z) SphericalToEuler_AngleUnits(double radius, double thetaAngleUnits, double phiAngleUnits)
        {
            double thetaRadians = AngleUnitsToRadians(thetaAngleUnits);
            double phiRadians = AngleUnitsToRadians(phiAngleUnits);
            return SphericalToEuler_Radians(radius, thetaRadians, phiRadians);
        }

        public static (double radius, double theta, double height) EulerToCylindrical_Radians(double x, double y, double z)
        {
            double radius = Math.Sqrt(x * x + z * z);
            double theta = Math.Atan2(x, z);
            double height = y;
            return (radius, theta, height);
        }

        public static (double x, double y, double z) CylindricalToEuler_Radians(double radius, double theta, double height)
        {
            double x = radius * Math.Sin(theta);
            double y = height;
            double z = radius * Math.Cos(theta);
            return (x, y, z);
        }

        public static (double radius, double thetaAngleUnits, double height) EulerToCylindrical_AngleUnits(double x, double y, double z)
        {
            double radius, thetaRadians, height;
            (radius, thetaRadians, height) = EulerToCylindrical_Radians(x, y, z);
            double thetaAngleUnits = RadiansToAngleUnits(thetaRadians);
            return (radius, thetaAngleUnits, height);
        }

        public static (double x, double y, double z) CylindricalToEuler_AngleUnits(double radius, double thetaAngleUnits, double height)
        {
            double thetaRadians = AngleUnitsToRadians(thetaAngleUnits);
            return CylindricalToEuler_Radians(radius, thetaRadians, height);
        }

        public static (double radius, double thetaAngleUnits, double height) EulerToCylindricalAboutPivot(
            double x, double y, double z, double pivotX, double pivotY, double pivotZ)
        {
            return EulerToCylindrical_AngleUnits(x - pivotX, y - pivotY, z - pivotZ);
        }

        public static double GetPitch(double startX, double startY, double startZ, double endX, double endY, double endZ)
        {
            (double radius, double theta, double phi) = EulerToSpherical_AngleUnits(endX - startX, endY - startY, endZ - startZ);
            return phi;
        }

        public static double RadiansToAngleUnits(double radians)
        {
            double angleUnits = radians / (2 * Math.PI) * 65536;
            return NonNegativeModulus(angleUnits, 65536);
        }

        public static ushort RadiansToAngleUnitsRounded(double radians)
        {
            double angleUnits = radians / (2 * Math.PI) * 65536;
            double nonNegative = NonNegativeModulus(angleUnits, 65536);
            return (ushort)(Math.Round(nonNegative) % 65536);
        }

        public static double AngleUnitsToRadians(double angleUnits)
        {
            double radians = angleUnits / 65536 * (2 * Math.PI);
            return NonNegativeModulus(radians, 2 * Math.PI);
        }

        public static double AngleUnitsToDegrees(double angleUnits)
        {
            double radians = angleUnits / 65536 * 360;
            return NonNegativeModulus(radians, 360);
        }

        public static double RotateAngleCCW(double angleUnits, double rotationDiff)
        {
            return NormalizeAngleDouble(angleUnits + rotationDiff);
        }

        public static double RotateAngleCW(double angleUnits, double rotationDiff)
        {
            return RotateAngleCCW(angleUnits, -1 * rotationDiff);
        }

        public static double ReverseAngle(double angleUnits)
        {
            return RotateAngleCCW(angleUnits, 32768);
        }

        public static (double x, double y, double z) OffsetSpherically(
            double x, double y, double z, double radiusChange, double thetaChangeAngleUnits, double phiChangeAngleUnits, bool clamp)
        {
            double oldRadius, oldTheta, oldPhi;
            (oldRadius, oldTheta, oldPhi) = EulerToSpherical_AngleUnits(x, y, z);

            double newRadius = Math.Max(oldRadius + radiusChange, 0);
            double newTheta = NonNegativeModulus(oldTheta + thetaChangeAngleUnits, 65536);
            double newPhi = NormalizeAngleDoubleSigned(oldPhi) + phiChangeAngleUnits;
            if (clamp) newPhi = Clamp(newPhi, -16384, 16384);
         
            return SphericalToEuler_AngleUnits(newRadius, newTheta, newPhi);
        }

        public static (double x, double y, double z) OffsetSphericallyAboutPivot(
            double x, double y, double z, double radiusChange, double thetaChangeAngleUnits, double phiChangeAngleUnits,
            double pivotX, double pivotY, double pivotZ, bool clamp)
        {
            double oldRelX = x - pivotX;
            double oldRelY = y - pivotY;
            double oldRelZ = z - pivotZ;

            double newRelX, newRelY, newRelZ;
            (newRelX, newRelY, newRelZ) =
                OffsetSpherically(oldRelX, oldRelY, oldRelZ, radiusChange, thetaChangeAngleUnits, phiChangeAngleUnits, clamp);

            return (newRelX + pivotX, newRelY + pivotY, newRelZ + pivotZ);
        }

        public static double OffsetAngleUnitsCapped(double angleUnits, double change)
        {
            angleUnits = NonNegativeModulus(angleUnits, 65536);
            angleUnits = Clamp(angleUnits + change, 0, 65536);
            angleUnits = NonNegativeModulus(angleUnits, 65536);
            return angleUnits;
        }

        /** Gets the value in [0, modulus). */
        public static int NonNegativeModulus(int value, int modulus)
        {
            value %= modulus;
            if (value < 0) value += modulus;
            return value;
        }

        /** Gets the value in [0, modulus). */
        public static double NonNegativeModulus(double value, double modulus)
        {
            value %= modulus;
            if (value < 0) value += modulus;
            return value;
        }

        /** Gets the value in [-modulus/2, modulus/2). */
        public static double MaybeNegativeModulus(double value, double modulus)
        {
            value %= modulus;
            if (value < 0) value += modulus;
            if (value >= modulus / 2) value -= modulus;
            return value;
        }

        /** Rounds and then wraps the value to be in [-range/2, range/2) if signed or [0, range) if unsigned. */
        public static double GetIntegerInRangeWrapped(double value, double range, bool signed)
        {
            value = Math.Round(value, MidpointRounding.AwayFromZero);
            return signed ? MaybeNegativeModulus(value, range) : NonNegativeModulus(value, range);
        }

        /** Rounds and then caps the value to be in [-range/2, range/2) if signed or [0, range) if unsigned. */
        public static double GetIntegerInRangeCapped(double value, double range, bool signed)
        {
            value = Math.Round(value, MidpointRounding.AwayFromZero);
            double min = signed ? -1 * range / 2 : 0;
            double exclusiveMax = signed ? range / 2 : range;
            double inclusiveMax = exclusiveMax - 1;
            return Clamp(value, min, inclusiveMax);
        }

        public static double GetUnsignedAngleDifference(double angle1, double angle2)
        {
            return NonNegativeModulus(angle2 - angle1, 65536);
        }

        public static double GetAngleDifference(double angle1, double angle2)
        {
            return MaybeNegativeModulus(angle2 - angle1, 65536);
        }

        public static double GetAngleDistance(double angle1, double angle2)
        {
            return Math.Abs(GetAngleDifference(angle1, angle2));
        }

        public static bool IsAngleBetweenAngles(double angle, double angleMin, double angleMax)
        {
            double effectiveAngle = NonNegativeModulus(angle - angleMin, 65536);
            double effectiveRange = NonNegativeModulus(angleMax - angleMin, 65536);
            return effectiveAngle <= effectiveRange;
        }

        public static double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }

        public static double TruncateToMultipleOf16(double value)
        {
            double divided = value / 16;
            double truncated = Math.Floor(divided);
            double multipled = truncated * 16;
            return multipled;
        }

        public static string GetPercentString(double count, double total, int decimalPlaces)
        {
            double percent = Math.Round(100 * count / total, decimalPlaces);
            string percentString = percent.ToString("N" + decimalPlaces) + "%";
            return percentString;
        }

        public static (double scaledX, double scaledZ) ScaleValues(double xValue, double zValue)
        {
            double magnitude = Math.Max(Math.Abs(xValue), Math.Abs(zValue));
            double totalMagnitude = Math.Sqrt(xValue * xValue + zValue * zValue);
            double multiplier = totalMagnitude == 0 ? 1 : magnitude / totalMagnitude;
            return (xValue * multiplier, zValue * multiplier);
        }

        public static ushort getUphillAngle(double normX, double normY, double normZ)
        {
            var uphillRadians = Math.PI + Math.Atan2(normX, normZ);
            if (normY < -0.01)
                uphillRadians += Math.PI;
            if (normX == 0 && normZ == 0)
                uphillRadians = 0;
            return RadiansToAngleUnitsRounded(uphillRadians);
        }

        public static byte ApplyValueToMaskedByte(byte currentValue, byte mask, byte valueToSet)
        {
            byte maskedValueToSet = (byte)(valueToSet & mask);
            byte unmaskedCurrentValue = (byte)(currentValue & ~mask);
            byte newValue = (byte)(unmaskedCurrentValue | maskedValueToSet);
            return newValue;
        }

        public static byte ApplyValueToMaskedByte(byte currentValue, byte mask, bool useWholeMask)
        {
            return ApplyValueToMaskedByte(currentValue, mask, useWholeMask ? mask : (byte)0);
        }

        public static double RotateAngleTowards(double angle1, double angle2, double cap)
        {
            angle1 = NormalizeAngleDouble(angle1);
            angle2 = NormalizeAngleDouble(angle2);
            double angle12Diff = NormalizeAngleDouble(angle1 - angle2);
            double angle21Diff = NormalizeAngleDouble(angle2 - angle1);
            double rotationDiff = Math.Min(cap, Math.Min(angle12Diff, angle21Diff));
            bool angle1Less = angle21Diff <= angle12Diff;
            double newAngle = angle1 + (angle1Less ? 1 : -1) * rotationDiff;
            return NormalizeAngleDouble(newAngle);
        }

        public static double MoveNumberTowards(double start, double end, double cap)
        {
            bool startLessThanEnd = start < end;
            double diff = Math.Abs(end - start);
            double cappedDiff = Math.Min(diff, cap);
            double moved = start + (startLessThanEnd ? 1 : -1) * cappedDiff;
            return moved;
        }

        public static double GetDotProduct(double v1X, double v1Y, double v1Z, double v2X, double v2Y, double v2Z)
        {
            return v1X * v2X + v1Y * v2Y + v1Z * v2Z;
        }

        // Input angle stuff

        public static (float effectiveX, float effectiveY) GetEffectiveInput(int rawX, int rawY)
        {
            float effectiveX = rawX >= 8 ? rawX - 6 : rawX <= -8 ? rawX + 6 : 0;
            float effectiveY = rawY >= 8 ? rawY - 6 : rawY <= -8 ? rawY + 6 : 0;
            float hypotenuse = (float)Math.Sqrt(effectiveX * effectiveX + effectiveY * effectiveY);
            if (hypotenuse > 64)
            {
                effectiveX *= 64 / hypotenuse;
                effectiveY *= 64 / hypotenuse;
            }
            return (effectiveX, effectiveY);
        }

        public static float GetEffectiveInputMagnitudeUncapped(int rawX, int rawY)
        {
            int effectiveX = rawX >= 8 ? rawX - 6 : rawX <= -8 ? rawX + 6 : 0;
            int effectiveY = rawY >= 8 ? rawY - 6 : rawY <= -8 ? rawY + 6 : 0;
            return (float)Math.Sqrt(effectiveX * effectiveX + effectiveY * effectiveY);
        }

        public static float GetEffectiveInputMagnitude(int rawX, int rawY)
        {
            int effectiveX = rawX >= 8 ? rawX - 6 : rawX <= -8 ? rawX + 6 : 0;
            int effectiveY = rawY >= 8 ? rawY - 6 : rawY <= -8 ? rawY + 6 : 0;
            float hypotenuse = (float)Math.Sqrt(effectiveX * effectiveX + effectiveY * effectiveY);
            return Math.Min(hypotenuse, 64f);
        }

        public static float GetScaledInputMagnitude(int rawX, int rawY, bool squished)
        {
            float effectiveMagnitude = GetEffectiveInputMagnitude(rawX, rawY);
            float scaled = (effectiveMagnitude / 64f) * (effectiveMagnitude / 64f) * 64f;
            int divider = squished ? 8 : 2;
            return scaled / divider;
        }

        public static bool InputIsInDeadZone(int input)
        {
            return input > -8 && input < 8 && input != 0;
        }

        public static (int xInput, int yInput) CalculateInputsForAngle(ushort goalAngle, ushort cameraAngle)
        {
            double bestMagnitude = 0;
            int bestX = 0;
            int bestY = 0;

            ushort truncatedGoalAngle = NormalizeAngleTruncated(goalAngle);
            for (int x = -128; x <= 127; x++)
            {
                if (InputIsInDeadZone(x)) continue;
                for (int y = -128; y <= 127; y++)
                {
                    if (InputIsInDeadZone(y)) continue;
                    ushort inputAngle = CalculateAngleFromInputs(x, y, cameraAngle);
                    ushort truncatedInputAngle = NormalizeAngleTruncated(inputAngle);
                    if (truncatedInputAngle == truncatedGoalAngle)
                    {
                        double magnitude = GetEffectiveInputMagnitudeUncapped(x, y);
                        if (magnitude > bestMagnitude)
                        {
                            bestMagnitude = magnitude;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }
            }
            return (bestX, bestY);
        }

        public static (int xInput, int yInput) CalculateInputsForAngleOptimized(ushort goalAngle, ushort cameraAngle)
        {
            double bestMagnitude = 0;
            int bestX = 0;
            int bestY = 0;

            ushort truncatedGoalAngle = NormalizeAngleTruncated(goalAngle);
            ushort reversedCameraAngle = NormalizeAngleUshort(ReverseAngle(cameraAngle));
            ushort goalMarioAngle = NormalizeAngleUshort(goalAngle - reversedCameraAngle);
            double goalMarioAngleRadians = AngleUnitsToRadians(goalMarioAngle);

            bool useX;
            bool positiveA;
            bool positiveB;
            if (goalMarioAngle < 8192)
            {
                useX = false;
                positiveA = true;
                positiveB = false;
            }
            else if (goalMarioAngle < 16384)
            {
                useX = true;
                positiveA = false;
                positiveB = true;
            }
            else if (goalMarioAngle < 24576)
            {
                useX = true;
                positiveA = false;
                positiveB = false;
            }
            else if (goalMarioAngle < 32768)
            {
                useX = false;
                positiveA = false;
                positiveB = false;
            }
            else if (goalMarioAngle < 40960)
            {
                useX = false;
                positiveA = false;
                positiveB = true;
            }
            else if (goalMarioAngle < 49152)
            {
                useX = true;
                positiveA = true;
                positiveB = false;
            }
            else if (goalMarioAngle < 57344)
            {
                useX = true;
                positiveA = true;
                positiveB = true;
            }
            else
            {
                useX = false;
                positiveA = true;
                positiveB = true;
            }

            double ratio = useX ?
                    Math.Cos(goalMarioAngleRadians) / Math.Sin(goalMarioAngleRadians) :
                    Math.Sin(goalMarioAngleRadians) / Math.Cos(goalMarioAngleRadians);
            double ratioAbs = Math.Abs(ratio);
            int max = positiveA ? 121 : 122;

            for (int aMag = 8; aMag <= max; aMag++)
            {
                int a = aMag * (positiveA ? 1 : -1);
                int bMedianMag = (int)(aMag * ratioAbs);
                int bMedian = bMedianMag * (positiveB ? 1 : -1);

                int width = 1;
                for (int b = bMedian - width; b <= bMedian + width; b++)
                {
                    int xEffective = useX ? a : b;
                    int yEffective = useX ? b : a;

                    if (Math.Abs(xEffective) == 1 || Math.Abs(yEffective) == 1) continue;

                    int x = xEffective < 0 ? xEffective - 6 : xEffective > 0 ? xEffective + 6 : 0;
                    int y = yEffective < 0 ? yEffective - 6 : yEffective > 0 ? yEffective + 6 : 0;

                    ushort inputAngle = CalculateAngleFromInputs(x, y, cameraAngle);
                    ushort truncatedInputAngle = NormalizeAngleTruncated(inputAngle);
                    if (truncatedInputAngle == truncatedGoalAngle)
                    {
                        double magnitude = GetEffectiveInputMagnitudeUncapped(x, y);
                        if (magnitude > bestMagnitude)
                        {
                            bestMagnitude = magnitude;
                            bestX = x;
                            bestY = y;
                        }
                    }
                }
            }

            return (bestX, bestY);
        }

        public static ushort CalculateAngleFromInputs(int xInput, int yInput, ushort? cameraAngleNullable = null)
        {
            (float effectiveX, float effectiveY) = GetEffectiveInput(xInput, yInput);
            ushort marioAngle = InGameTrigUtilities.InGameATan(effectiveY, -effectiveX);
            ushort cameraAngleRaw = cameraAngleNullable ?? Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset);
            ushort cameraAngle = NormalizeAngleUshort(ReverseAngle(cameraAngleRaw));
            ushort summedAngle = NormalizeAngleUshort(marioAngle + cameraAngle);
            return summedAngle;
        }

        // Float stuff

        public static int GetFloatSign(float floatValue)
        {
            string bitString = GetBitString(floatValue);
            string signChar = bitString.Substring(0, 1);
            return signChar == "0" ? 1 : -1;
        }

        public static int GetFloatExponent(float floatValue)
        {
            string bitString = GetBitString(floatValue);
            string exponentString = bitString.Substring(1, 8);
            int byteValue = 0;
            for (int i = 0; i < 8; i++)
            {
                string bitChar = exponentString.Substring(8 - 1 - i, 1);
                bool bitBool = bitChar == "1";
                if (bitBool) byteValue = (byte)(byteValue | (1 << i));
            }
            int exponent = byteValue - 127;
            return exponent;
        }

        public static double GetFloatMantissa(float floatValue)
        {
            string bitString = GetBitString(floatValue);
            string exponentString = bitString.Substring(9, 23);
            double sum = 1;
            double multiplier = 1;
            for (int i = 0; i < 23; i++)
            {
                multiplier *= 0.5;
                string bitChar = exponentString.Substring(i, 1);
                bool bitBool = bitChar == "1";
                if (bitBool) sum += multiplier;
            }
            return sum;
        }

        public static string GetBitString(object value)
        {
            List<string> bitStrings = TypeUtilities.GetBytes(value).ToList().ConvertAll(b => GetBitString(b));
            bitStrings.Reverse();
            return String.Join("", bitStrings);
        }

        public static string GetBitString(byte b)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 7; i >= 0; i--)
            {
                bool bit = (b & (1 << i)) != 0;
                builder.Append(bit ? "1" : "0");
            }
            return builder.ToString();
        }

        /** relX = how much right, relY = how much up, relZ = how much towards the camera */
        public static (double x, double y, double z) TranslateRelatively(
            double yaw, double pitch, double roll, double relX, double relY, double relZ)
        {
            (double fx, double fy, double fz) = SphericalToEuler_AngleUnits(relZ, yaw, pitch);
            (double sx, double sy, double sz) = SphericalToEuler_AngleUnits(relX, yaw - 16384, 0);
            (double vx, double vy, double vz) = SphericalToEuler_AngleUnits(relY, yaw, pitch - 16384);
            return (fx + sx + vx, fy + sy + vy, fz + sz + vz);
        }

        public static float GetNextFloatInterval(float value)
        {
            value = Math.Abs(value);
            float interval = 262144;
            while (true)
            {
                float testValue = value + (interval / 2);
                if (value == testValue) return interval;
                interval /= 2;
            }
        }

        public static float GetPreviousFloatInterval(float value)
        {
            value = Math.Abs(value);
            float interval = 262144;
            while (true)
            {
                float testValue = value - (interval / 2);
                if (value == testValue) return interval;
                interval /= 2;
            }
        }

        public static float GetNextFloat(float value)
        {
            return value + GetNextFloatInterval(value);
        }

        public static float GetPreviousFloat(float value)
        {
            return value - GetPreviousFloatInterval(value);
        }

        public static float MoveFloat(float value, int num)
        {
            int iters = Math.Abs(num);
            for (int i = 0; i < iters; i++)
            {
                value = num > 0 ? GetNextFloat(value) : GetPreviousFloat(value);
            }
            return value;
        }

        public static float MoveFloatTowards(float value, float goal)
        {
            if (goal > value) return GetNextFloat(value);
            if (goal < value) return GetPreviousFloat(value);
            return value;
        }

        public static (double x, double y, double z, double t) GetPlaneLineIntersection(
            double planeX, double planeY, double planeZ, double planeYaw, double planePitch,
            double x1, double y1, double z1, double x2, double y2, double z2)
        {
            // Ax + By + Cz = D
            double yawRadians = AngleUnitsToRadians(planeYaw);
            double pitchRadians = AngleUnitsToRadians(planePitch);
            double A = Math.Sin(yawRadians) * Math.Cos(pitchRadians);
            double B = Math.Sin(pitchRadians);
            double C = Math.Cos(yawRadians) * Math.Cos(pitchRadians);
            double D = A * planeX + B * planeY + C * planeZ;

            // x = x1 + xDiff * t
            // y = y1 + yDiff * t
            // z = z1 + zDiff * t
            double xDiff = x2 - x1;
            double yDiff = y2 - y1;
            double zDiff = z2 - z1;

            // A * x + B * y + C * z = D
            // A * (x1 + xDiff * t) + B * (y1 + yDiff * t) + C * (z1 + zDiff * t) = D
            // A * x1 + A * xDiff * t + B * y1 + B * yDiff * t + C * z1 + C * zDiff * t = D
            // A * xDiff * t + B * yDiff * t + C * zDiff * t = D - (A * x1) - (B * y1) - (C * z1)
            // t * (A * xDiff + B * yDiff + C * zDiff) = D - (A * x1) - (B * y1) - (C * z1)
            // t = (D - (A * x1) - (B * y1) - (C * z1)) / (A * xDiff + B * yDiff + C * zDiff)
            double t = (D - (A * x1) - (B * y1) - (C * z1)) / (A * xDiff + B * yDiff + C * zDiff);

            return (x1 + xDiff * t, y1 + yDiff * t, z1 + zDiff * t, t);
        }

        public static (double x, double y, double z, double t) GetPlaneLineIntersection(
            double planeX, double planeY, double planeZ, double planeYaw, double planePitch,
            double x1, double y1, double z1, double lineYaw, double linePitch)
        {
            // Ax + By + Cz = D
            double yawRadians = AngleUnitsToRadians(planeYaw);
            double pitchRadians = AngleUnitsToRadians(planePitch);
            double A = Math.Sin(yawRadians) * Math.Cos(pitchRadians);
            double B = Math.Sin(pitchRadians);
            double C = Math.Cos(yawRadians) * Math.Cos(pitchRadians);
            double D = A * planeX + B * planeY + C * planeZ;

            // x = x1 + xDiff * t
            // y = y1 + yDiff * t
            // z = z1 + zDiff * t
            (double xDiff, double yDiff, double zDiff) = SphericalToEuler_AngleUnits(1, lineYaw, linePitch);

            // A * x + B * y + C * z = D
            // A * (x1 + xDiff * t) + B * (y1 + yDiff * t) + C * (z1 + zDiff * t) = D
            // A * x1 + A * xDiff * t + B * y1 + B * yDiff * t + C * z1 + C * zDiff * t = D
            // A * xDiff * t + B * yDiff * t + C * zDiff * t = D - (A * x1) - (B * y1) - (C * z1)
            // t * (A * xDiff + B * yDiff + C * zDiff) = D - (A * x1) - (B * y1) - (C * z1)
            // t = (D - (A * x1) - (B * y1) - (C * z1)) / (A * xDiff + B * yDiff + C * zDiff)
            double t = (D - (A * x1) - (B * y1) - (C * z1)) / (A * xDiff + B * yDiff + C * zDiff);

            return (x1 + xDiff * t, y1 + yDiff * t, z1 + zDiff * t, t);
        }

        public static (double x, double y, double z) GetPlanePointAtPoint(
            double planeX, double planeY, double planeZ, double planeYaw, double planePitch,
            double px, double py, double pz)
        {
            (double qx, double qz) = AddVectorToPoint(1, planeYaw, planeX, planeZ);
            (double rx, double ry, double rz, double t) = GetPlaneLineIntersection(
                px, py, pz, planeYaw, planePitch, planeX, planeY, planeZ, qx, planeY, qz);
            return (rx, ry, rz);
        }

        public static double GetPlaneDistanceToPoint(
            double planeX, double planeY, double planeZ, double planeYaw, double planePitch,
            double px, double py, double pz)
        {
            return Math.Abs(GetPlaneDistanceToPointSigned(
                planeX, planeY, planeZ, planeYaw, planePitch, px, py, pz));
        }

        public static double GetPlaneDistanceToPointSigned(
            double planeX, double planeY, double planeZ, double planeYaw, double planePitch,
            double px, double py, double pz)
        {
            // Ax + By + Cz = D
            double yawRadians = AngleUnitsToRadians(planeYaw);
            double pitchRadians = AngleUnitsToRadians(planePitch);
            double A = Math.Sin(yawRadians) * Math.Cos(pitchRadians);
            double B = Math.Sin(pitchRadians);
            double C = Math.Cos(yawRadians) * Math.Cos(pitchRadians);
            double D = A * planeX + B * planeY + C * planeZ;
            return A * px + B * py + C * pz - D;
        }

        public static (double x, double z) GetLineIntersectionAtCoordinate(
            double pointX, double pointZ, double lineAngle, double coord, bool isX)
        {
            if (isX)
            {
                double lineAngleRadians = AngleUnitsToRadians(-lineAngle + 16384);
                double dx = coord - pointX;
                double dz = Math.Tan(lineAngleRadians) * dx;
                return (coord, pointZ + dz);
            }
            else
            {
                double lineAngleRadians = AngleUnitsToRadians(lineAngle);
                double dz = coord - pointZ;
                double dx = Math.Tan(lineAngleRadians) * dz;
                return (pointX + dx, coord);
            }
        }
    }
}