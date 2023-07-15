using STROOP.Structs.Configurations;
using STROOP.Utilities;
using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;

namespace STROOP.Models
{
    public abstract class TriangleDataModel
    {
        public abstract uint Address { get; }

        public abstract short SurfaceType { get; }
        public abstract byte ExertionForceIndex { get; }
        public abstract byte ExertionAngle { get; }
        public abstract byte Flags { get; }
        public abstract byte Room { get; }

        public abstract short YMinMinus5 { get; }
        public abstract short YMaxPlus5 { get; }

        public abstract short X1 { get; }
        public abstract short Y1 { get; }
        public abstract short Z1 { get; }
        public abstract short X2 { get; }
        public abstract short Y2 { get; }
        public abstract short Z2 { get; }
        public abstract short X3 { get; }
        public abstract short Y3 { get; }
        public abstract short Z3 { get; }

        public abstract float NormX { get; }
        public abstract float NormY { get; }
        public abstract float NormZ { get; }
        public abstract float NormOffset { get; }

        public abstract uint AssociatedObject { get; }

        public abstract TriangleClassification Classification { get; }

        public abstract bool XProjection { get; }
        public abstract bool BelongsToObject { get; }
        public abstract bool NoCamCollision { get; }

        public abstract string Description { get; }
        public abstract short Slipperiness { get; }
        public abstract string SlipperinessDescription { get; }
        public abstract double FrictionMultiplier { get; }
        public abstract double SlopeAccel { get; }
        public abstract double SlopeDecelValue { get; }
        public abstract bool Exertion { get; }

        public abstract List<object> FieldValueList { get; }

        public readonly static List<string> FieldNameList =
            new List<string> {
                "Address",
                "Classification",
                "SurfaceType",
                "Description",
                "Slipperiness",
                "SlipperinessDescription",
                "Exertion",
                "ExertionForceIndex",
                "ExertionAngle",
                "Flags",
                "XProjection",
                "BelongsToObject",
                "NoCamCollision",
                "Room",
                "YMin-5",
                "YMax+5",
                "X1",
                "Y1",
                "Z1",
                "X2",
                "Y2",
                "Z2",
                "X3",
                "Y3",
                "Z3",
                "NormX",
                "NormY",
                "NormZ",
                "NormOffset",
                "AssociatedObject",
            };

        private static Dictionary<uint, TriangleDataModel> _cache = new Dictionary<uint, TriangleDataModel>();

        public static void ClearCache()
        {
            _cache.Clear();
        }

        public static TriangleDataModel CreateFull(uint triangleAddress)
        {
            if (!_cache.ContainsKey(triangleAddress))
            {
                TriangleDataModel tri = new TriangleDataModelFull(triangleAddress);
                _cache[triangleAddress] = tri;
            }
            return _cache[triangleAddress];
        }

        public static TriangleDataModel CreateLazy(uint triangleAddress)
        {
            if (!_cache.ContainsKey(triangleAddress))
            {
                TriangleDataModel tri = new TriangleDataModelLazy(triangleAddress);
                _cache[triangleAddress] = tri;
            }
            return _cache[triangleAddress];
        }

        public static TriangleDataModel CreateCustom((int, int, int) p1, (int, int, int) p2, (int, int, int) p3)
        {
            return new TriangleDataModelCustom(p1.Item1, p1.Item2, p1.Item3, p2.Item1, p2.Item2, p2.Item3, p3.Item1, p3.Item2, p3.Item3);
        }

        public static TriangleDataModel CreateCustom(int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
        {
            return new TriangleDataModelCustom(x1, y1, z1, x2, y2, z2, x3, y3, z3);
        }

        public override string ToString()
        {
            return String.Join("\t", FieldValueList);
        }

        public static string GetFieldNameString()
        {
            return String.Join("\t", FieldNameList);
        }

        public bool IsWall()
        {
            return Classification == TriangleClassification.Wall;
        }

        public bool IsFloor()
        {
            return Classification == TriangleClassification.Floor;
        }

        public bool IsCeiling()
        {
            return Classification == TriangleClassification.Ceiling;
        }

        public short GetMinX()
        {
            return Math.Min(X1, Math.Min(X2, X3));
        }

        public short GetMaxX()
        {
            return Math.Max(X1, Math.Max(X2, X3));
        }

        public short GetMinY()
        {
            return Math.Min(Y1, Math.Min(Y2, Y3));
        }

        public short GetMaxY()
        {
            return Math.Max(Y1, Math.Max(Y2, Y3));
        }

        public short GetMinZ()
        {
            return Math.Min(Z1, Math.Min(Z2, Z3));
        }

        public short GetMaxZ()
        {
            return Math.Max(Z1, Math.Max(Z2, Z3));
        }

        public int GetRangeX()
        {
            return GetMaxX() - GetMinX();
        }

        public int GetRangeY()
        {
            return GetMaxY() - GetMinY();
        }

        public int GetRangeZ()
        {
            return GetMaxZ() - GetMinZ();
        }

        public double GetMidpointX()
        {
            return (GetMinX() + GetMaxX()) / 2.0;
        }

        public double GetMidpointY()
        {
            return (GetMinY() + GetMaxY()) / 2.0;
        }

        public double GetMidpointZ()
        {
            return (GetMinZ() + GetMaxZ()) / 2.0;
        }

        public (int x, int y, int z) GetP1()
        {
            return (X1, Y1, Z1);
        }

        public (int x, int y, int z) GetP2()
        {
            return (X2, Y2, Z2);
        }

        public (int x, int y, int z) GetP3()
        {
            return (X3, Y3, Z3);
        }

        public List<(float x, float z)> Get2DVertices()
        {
            return new List<(float, float)>()
            {
                (X1, Z1), (X2, Z2), (X3, Z3)
            };
        }

        public List<(float x, float y, float z)> Get3DVertices()
        {
            return new List<(float, float, float)>()
            {
                (X1, Y1, Z1), (X2, Y2, Z2), (X3, Y3, Z3)
            };
        }

        public List<(float x, float y, float z, TriangleDataModel tri)> Get3DVerticesWithTri()
        {
            return new List<(float, float, float, TriangleDataModel)>()
            {
                (X1, Y1, Z1, this), (X2, Y2, Z2, this), (X3, Y3, Z3, this)
            };
        }

        public double GetDistToMidpoint()
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            return MoreMath.GetDistanceBetween(marioX, marioY, marioZ, GetMidpointX(), GetMidpointY(), GetMidpointZ());
        }

        public int GetClosestVertex()
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            return GetClosestVertex(marioX, marioY, marioZ);
        }

        public int GetClosestVertex(double x, double y, double z)
        {
            double dist1 = MoreMath.GetDistanceBetween(X1, Y1, Z1, x, y, z);
            double dist2 = MoreMath.GetDistanceBetween(X2, Y2, Z2, x, y, z);
            double dist3 = MoreMath.GetDistanceBetween(X3, Y3, Z3, x, y, z);

            if (dist1 <= dist2 && dist1 <= dist3) return 1;
            if (dist2 <= dist3) return 2;
            return 3;
        }

        public double GetHeightOnTriangle(double x, double z)
        {
            return -(x * NormX + NormZ * z + NormOffset) / NormY;
        }

        public float GetTruncatedHeightOnTriangle(double doubleX, double doubleZ)
        {
            short x = (short)doubleX;
            short z = (short)doubleZ;
            if (SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                int modX = ((x % 4) + 4) % 4;
                int modZ = ((z % 4) + 4) % 4;
                x = (short)(x - modX);
                z = (short)(z - modZ);
            }
            return -(x * NormX + NormZ * z + NormOffset) / NormY;
        }

        public static double GetHeightOnTriangle(
            double x, double z, double normX, double normY, double normZ, double normOffset)
        {
            return (-x * normX - z * normZ - normOffset) / normY;
        }

        public bool IsPointInsideAndAboveTriangle(double doubleX, double doubleY, double doubleZ)
        {
            short shortX = (short)doubleX;
            short shortY = (short)doubleY;
            short shortZ = (short)doubleZ;

            if (!IsPointInsideTriangle(shortX, shortZ)) return false;
            
            double heightOnTriangle = GetHeightOnTriangle(shortX, shortZ, NormX, NormY, NormZ, NormOffset);
            if (shortY < heightOnTriangle - 78) return false;

            return true;
        }

        public bool IsPointInsideAndBelowTriangle(double doubleX, double doubleY, double doubleZ)
        {
            short shortX = (short)doubleX;
            short shortY = (short)doubleY;
            short shortZ = (short)doubleZ;

            if (!IsPointInsideTriangle(shortX, shortZ)) return false;

            double heightOnTriangle = GetHeightOnTriangle(shortX, shortZ, NormX, NormY, NormZ, NormOffset);
            if (shortY > heightOnTriangle + 78) return false;

            return true;
        }

        public bool IsPointInsideAndWithinTriangle(double doubleX, double doubleY, double doubleZ)
        {
            short shortX = (short)doubleX;
            short shortY = (short)doubleY;
            short shortZ = (short)doubleZ;

            if (!IsPointInsideTriangle(shortX, shortZ)) return false;

            double heightOnTriangle = GetHeightOnTriangle(shortX, shortZ, NormX, NormY, NormZ, NormOffset);
            if (shortY < heightOnTriangle - 78 || shortY > heightOnTriangle) return false;

            return true;
        }

        public bool IsPointInsideTriangle(double doubleX, double doubleZ, bool truncate)
        {
            if (truncate)
            {
                doubleX = (short)doubleX;
                doubleZ = (short)doubleZ;
            }
            return IsPointInsideTriangle(doubleX, doubleZ);
        }

        public double GetVerticalDistAwayFromTriangleHitbox(double doubleX, double doubleY, double doubleZ)
        {
            short shortX = (short)doubleX;
            short shortY = (short)doubleY;
            short shortZ = (short)doubleZ;

            //if (!IsPointInsideTriangle(shortX, shortZ)) return null;

            double heightOnTriangle = GetHeightOnTriangle(shortX, shortZ, NormX, NormY, NormZ, NormOffset);
            if (shortY < heightOnTriangle - 78) return shortY - (heightOnTriangle - 78);
            if (shortY > heightOnTriangle) return shortY - heightOnTriangle;

            return 0;
        }

        public float? GetTruncatedHeightOnTriangleIfInsideTriangle(double doubleX, double doubleZ)
        {
            short shortX = (short)doubleX;
            short shortZ = (short)doubleZ;
            if (!IsPointInsideTriangle(shortX, shortZ)) return null;
            return GetTruncatedHeightOnTriangle(doubleX, doubleZ);
        }

        public bool IsPointInsideTriangle(double pX, double pZ)
        {
            int x1 = X1;
            int z1 = Z1;
            int x2 = X2;
            int z2 = Z2;
            int x3 = X3;
            int z3 = Z3;

            if (SavedSettingsConfig.UseExtendedLevelBoundaries)
            {
                x1 = (int)ExtendedLevelBoundariesUtilities.Unconvert(x1, false);
                z1 = (int)ExtendedLevelBoundariesUtilities.Unconvert(z1, false);
                x2 = (int)ExtendedLevelBoundariesUtilities.Unconvert(x2, false);
                z2 = (int)ExtendedLevelBoundariesUtilities.Unconvert(z2, false);
                x3 = (int)ExtendedLevelBoundariesUtilities.Unconvert(x3, false);
                z3 = (int)ExtendedLevelBoundariesUtilities.Unconvert(z3, false);
            }

            return MoreMath.IsPointInsideTriangle(pX, pZ, x1, z1, x2, z2, x3, z3);
        }

        public bool IsTriWithinVerticalDistOfCenter(float? withinDistNullable, float centerY)
        {
            if (!withinDistNullable.HasValue) return true;
            float withinDist = withinDistNullable.Value;
            short minY = GetMinY();
            short maxY = GetMaxY();
            bool triTooFarDown = centerY - maxY > withinDist;
            bool triTooFarUp = minY - centerY > withinDist;
            return !triTooFarDown && !triTooFarUp;
        }

        public double GetPushAngle()
        {
            double uphillAngle = WatchVariableSpecialUtilities.GetTriangleUphillAngle(this);
            return MoreMath.ReverseAngle(uphillAngle);
        }
    }
}
