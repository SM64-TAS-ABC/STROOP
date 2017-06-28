using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class MoreMath
    {
        public static double DistanceTo(float x1, float y1, float z1, float x2, float y2, float z2)
        {
            float dx, dy, dz;
            dx = x1 - x2;
            dy = y1 - y2;
            dz = z1 - z2;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }

        public static double DistanceTo(float x1, float y1, float x2, float y2)
        {
            float dx, dy;
            dx = x1 - x2;
            dy = y1 - y2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double AngleTo(float xFrom, float yFrom, float xTo, float yTo)
        {
            float dx, dy;
            dx = xTo - xFrom;
            dy = yTo - yFrom;
            return Math.Atan2(dy, dx);
        }

        public static (double radius, double theta, double phi) EulerToSphericalRadians(double x, double y, double z)
        {
            double radius = Math.Sqrt(x * x + y * y + z * z);
            double theta = Math.Atan2(x, z);
            double phi = radius == 0 ? 0 : Math.Acos(y / radius);
            return (radius, theta, phi);
        }

        public static (double radius, double theta, double phi) EulerToSphericalAngleUnits(double x, double y, double z)
        {
            double radius, thetaRadians, phiRadians;
            (radius, thetaRadians, phiRadians) = EulerToSphericalRadians(x, y, z);
            double thetaAngleUnits = RadiansToAngleUnits(thetaRadians);
            double phiAngleUnits = RadiansToAngleUnits(phiRadians);
            return (radius, thetaAngleUnits, phiAngleUnits);
        }

        public static (double x, double y, double z) SphericalToEulerRadians(double radius, double theta, double phi)
        {
            double x = radius * Math.Sin(theta) * Math.Sin(phi);
            double y = radius * Math.Cos(phi);
            double z = radius * Math.Cos(theta) * Math.Sin(phi);
            return (x, y, z);
        }

        public static (double x, double y, double z) SphericalToEulerAngleUnits(double radius, double thetaAngleUnits, double phiAngleUnits)
        {
            double thetaRadians = AngleUnitsToRadians(thetaAngleUnits);
            double phiRadians = AngleUnitsToRadians(phiAngleUnits);
            return SphericalToEulerRadians(radius, thetaRadians, phiRadians);
        }

        public static double RadiansToAngleUnits(double radians)
        {
            double angleUnits = radians / (2 * Math.PI) * 65536;
            return NonnegativeModulus(angleUnits, 65536);
        }

        public static double AngleUnitsToRadians(double angleUnits)
        {
            double radians = angleUnits / 65536 * (2 * Math.PI);
            return NonnegativeModulus(radians, 2 * Math.PI);
        }

        public static (double x, double y, double z) OffsetSpherically(
            double x, double y, double z, double radiusChange, double thetaChangeAngleUnits, double phiChangeAngleUnits)
        {
            double oldRadius, oldTheta, oldPhi;
            (oldRadius, oldTheta, oldPhi) = EulerToSphericalAngleUnits(x, y, z);

            double newRadius = Math.Max(oldRadius + radiusChange, 0);
            double newTheta = NonnegativeModulus(oldTheta + thetaChangeAngleUnits, 65536);
            double newPhi = OffsetAngleUnitsCappedAt32768(oldPhi, phiChangeAngleUnits);

            return SphericalToEulerAngleUnits(newRadius, newTheta, newPhi);
        }

        public static (double x, double y, double z) OffsetSphericallyAboutPivot(
            double x, double y, double z, double radiusChange, double thetaChangeAngleUnits, double phiChangeAngleUnits,
            double pivotX, double pivotY, double pivotZ)
        {
            double oldRelX = x - pivotX;
            double oldRelY = y - pivotY;
            double oldRelZ = z - pivotZ;

            double newRelX, newRelY, newRelZ;
            (newRelX, newRelY, newRelZ) =
                OffsetSpherically(oldRelX, oldRelY, oldRelZ, radiusChange, thetaChangeAngleUnits, phiChangeAngleUnits);

            return (newRelX + pivotX, newRelY + pivotY, newRelZ + pivotZ);
        }

        public static double OffsetAngleUnitsCapped(double angleUnits, double change)
        {
            angleUnits = NonnegativeModulus(angleUnits, 65536);
            angleUnits = Clamp(angleUnits + change, 0, 65536);
            angleUnits = NonnegativeModulus(angleUnits, 65536);
            return angleUnits;
        }

        public static double OffsetAngleUnitsCappedAt32768(double angleUnits, double change)
        {
            angleUnits = NonnegativeModulus(angleUnits, 65536);
            angleUnits = Clamp(angleUnits + change, 0, 32768);
            return angleUnits;
        }

        public static double NonnegativeModulus(double value, double modulus)
        {
            value %= modulus;
            if (value < 0) value += modulus;
            return value;
        }

        public static double Clamp(double value, double min, double max)
        {
            return Math.Min(Math.Max(value, min), max);
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
            return (ushort)RadiansToAngleUnits(uphillRadians);
        }
    }
}