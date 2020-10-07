using STROOP.Forms;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Utilities
{
    public static class TriangleUtilities
    {
        public static List<TriangleDataModel> GetLevelTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
            int numLevelTriangles = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);
            return GetTrianglesInRange(triangleListAddress, numLevelTriangles);
        }

        public static List<uint> GetLevelTriangleAddresses()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
            int numLevelTriangles = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);
            return GetTriangleAddressesInRange(triangleListAddress, numLevelTriangles);
        }

        public static List<TriangleDataModel> GetObjectTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
            int numTotalTriangles = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
            int numLevelTriangles = Config.Stream.GetInt32(TriangleConfig.LevelTriangleCountAddress);

            uint objectTriangleListAddress = triangleListAddress + (uint)(numLevelTriangles * TriangleConfig.TriangleStructSize);
            int numObjectTriangles = numTotalTriangles - numLevelTriangles;

            return GetTrianglesInRange(objectTriangleListAddress, numObjectTriangles);
        }

        public static List<TriangleDataModel> GetObjectTrianglesForObject(uint objAddress)
        {
            return GetObjectTriangles().FindAll(tri => tri.AssociatedObject == objAddress);
        }

        public static uint? GetTriangleAddressOfObjectTriangleIndex(uint objAddress, int index)
        {
            List<TriangleDataModel> objTris = GetObjectTrianglesForObject(objAddress);
            if (index < 0 || index >= objTris.Count) return null;
            return objTris[index].Address;
        }

        public static List<TriangleDataModel> GetSelectedObjectTriangles()
        {
            List<TriangleDataModel> allObjectTriangles = GetObjectTriangles();
            List<uint> selectedAddresses = Config.ObjectSlotsManager.SelectedSlotsAddresses;
            List<TriangleDataModel> selectedObjectTriangles = allObjectTriangles.FindAll(
                tri => selectedAddresses.Contains(tri.AssociatedObject));
            return selectedObjectTriangles;
        }

        public static List<TriangleDataModel> GetAllTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
            int numTotalTriangles = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
            return GetTrianglesInRange(triangleListAddress, numTotalTriangles);
        }

        public static List<TriangleDataModel> GetTrianglesInRange(uint startAddress, int numTriangles)
        {
            return GetTriangleAddressesInRange(startAddress, numTriangles)
                .ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
        }

        public static List<uint> GetTriangleAddressesInRange(uint startAddress, int numTriangles)
        {
            if (numTriangles > 10000) numTriangles = 10000;
            List<uint> triangleAddressList = new List<uint>();
            for (int i = 0; i < numTriangles; i++)
            {
                uint address = startAddress + (uint)(i * TriangleConfig.TriangleStructSize);
                triangleAddressList.Add(address);
            }
            return triangleAddressList;
        }

        public static void ShowTriangles(List<TriangleDataModel> triangleList)
        {
            InfoForm infoForm = new InfoForm();
            infoForm.SetTriangles(triangleList);
            infoForm.Show();
        }

        public static void AnnihilateAllTrianglesButDeathBarriers()
        {
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                short type = Config.Stream.GetInt16(address + TriangleOffsetsConfig.SurfaceType);
                if (type != 0x0A)
                {
                    ButtonUtilities.AnnihilateTriangle(new List<uint>() { address });
                }
            });
            if (!streamAlreadySuspended) Config.Stream.Resume();
        }

        public static void NeutralizeTriangles(TriangleClassification? classification = null)
        {
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                float ynorm = Config.Stream.GetSingle(address + TriangleOffsetsConfig.NormY);
                TriangleClassification triClassification = CalculateClassification(ynorm);
                if (classification == null || classification == triClassification)
                {
                    ButtonUtilities.NeutralizeTriangle(new List<uint>() { address });
                }
            });
        }

        public static void NeutralizeTriangles(short surfaceType)
        {
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                short type = Config.Stream.GetInt16(address + TriangleOffsetsConfig.SurfaceType);
                if (type == surfaceType)
                {
                    ButtonUtilities.NeutralizeTriangle(new List<uint>() { address });
                }
            });
        }

        public static void NeutralizeSleeping()
        {
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                byte oldFlags = Config.Stream.GetByte(address + TriangleOffsetsConfig.Flags);
                byte newFlags = (byte)(oldFlags | TriangleOffsetsConfig.BelongsToObjectMask);
                Config.Stream.SetValue(newFlags, address + TriangleOffsetsConfig.Flags);
            });
        }

        public static void DisableCamCollision(TriangleClassification? classification = null)
        {
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                float ynorm = Config.Stream.GetSingle(address + TriangleOffsetsConfig.NormY);
                TriangleClassification triClassification = CalculateClassification(ynorm);
                if (classification == null || classification == triClassification)
                {
                    ButtonUtilities.DisableCamCollisionForTriangle(address);
                }
            });
        }

        public static TriangleClassification CalculateClassification(double yNorm)
        {
            if (yNorm > 0.01) return TriangleClassification.Floor;
            if (yNorm < -0.01) return TriangleClassification.Ceiling;
            return TriangleClassification.Wall;
        }

        public static void ConvertSurfaceTypes(TriangleClassificationExtended classification, short fromType, short toType)
        {
            GetLevelTriangles()
                .FindAll(tri => TrianglePassesClassification(tri, classification))
                .FindAll(tri => tri.SurfaceType == fromType)
                .ForEach(tri => Config.Stream.SetValue(toType, tri.Address + TriangleOffsetsConfig.SurfaceType));
        }

        private static bool TrianglePassesClassification(TriangleDataModel tri, TriangleClassificationExtended classification)
        {
            switch (classification)
            {
                case TriangleClassificationExtended.FloorTris:
                    return tri.Classification == TriangleClassification.Floor;
                case TriangleClassificationExtended.WallTris:
                    return tri.Classification == TriangleClassification.Wall;
                case TriangleClassificationExtended.CeilingTris:
                    return tri.Classification == TriangleClassification.Ceiling;
                case TriangleClassificationExtended.AllTris:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static List<TriangleShape> GetWallTriangleHitboxComponents(List<TriangleDataModel> wallTriangles)
        {
            List<((short, short), (short, short), bool)> vertexPairs =
                new List<((short, short), (short, short), bool)>();
            foreach (TriangleDataModel wallTriangle in wallTriangles)
            {
                List<(short, short)> vertices = new List<(short, short)>()
                {
                    (wallTriangle.X1, wallTriangle.Z1),
                    (wallTriangle.X2, wallTriangle.Z2),
                    (wallTriangle.X3, wallTriangle.Z3),
                };

                for (int i = 0; i < vertices.Count; i++)
                {
                    (short x1, short z1) = vertices[i];
                    (short x2, short z2) = vertices[(i + 1) % vertices.Count];
                    vertexPairs.Add(((x1, z1), (x2, z2), wallTriangle.XProjection));
                }
            }

            List<int> badVertexPairIndexes = new List<int>();
            for (int i = 0; i < vertexPairs.Count; i++)
            {
                ((short x1, short z1), (short x2, short z2), bool proj) = vertexPairs[i];
                if (x1 == x2 && z1 == z2) badVertexPairIndexes.Add(i);
            }
            for (int i = 0; i < vertexPairs.Count; i++)
            {
                for (int j = i + 1; j < vertexPairs.Count; j++)
                {
                    var vertexPair1 = vertexPairs[i];
                    var vertexPair2 = vertexPairs[j];
                    ((short vp1x1, short vp1z1), (short vp1x2, short vp1z2), bool vp1proj) = vertexPair1;
                    ((short vp2x1, short vp2z1), (short vp2x2, short vp2z2), bool vp2proj) = vertexPair2;
                    if ((vp1x1 == vp2x1 && vp1z1 == vp2z1 && vp1x2 == vp2x2 && vp1z2 == vp2z2) ||
                        (vp1x1 == vp2x2 && vp1z1 == vp2z2 && vp1x2 == vp2x1 && vp1z2 == vp2z1))
                    {
                        badVertexPairIndexes.Add(j);
                    }
                }
            }

            badVertexPairIndexes = badVertexPairIndexes.Distinct().ToList();
            badVertexPairIndexes.Sort();
            badVertexPairIndexes.Reverse();
            badVertexPairIndexes.ForEach(index => vertexPairs.RemoveAt(index));

            List<TriangleShape> triShapes = new List<TriangleShape>();
            foreach (var ((x1, z1), (x2, z2), proj) in vertexPairs)
            {
                double angle = MoreMath.AngleTo_AngleUnits(x1, z1, x2, z2);
                double projAngle = proj ? 16384 : 0;
                double projDist = 50 / Math.Sin(MoreMath.AngleUnitsToRadians(angle - projAngle));
                (double p1X, double p1Z) = MoreMath.AddVectorToPoint(projDist, projAngle, x1, z1);
                (double p2X, double p2Z) = MoreMath.AddVectorToPoint(-1 * projDist, projAngle, x1, z1);
                (double p3X, double p3Z) = MoreMath.AddVectorToPoint(projDist, projAngle, x2, z2);
                (double p4X, double p4Z) = MoreMath.AddVectorToPoint(-1 * projDist, projAngle, x2, z2);

                TriangleShape triShape1 = new TriangleShape(p1X, 0, p1Z, p2X, 0, p2Z, p3X, 0, p3Z);
                TriangleShape triShape2 = new TriangleShape(p2X, 0, p2Z, p3X, 0, p3Z, p4X, 0, p4Z);
                triShapes.Add(triShape1);
                triShapes.Add(triShape2);
            }
            return triShapes;
        }

        public static (List<TriangleShape> floors, List<TriangleShape> walls) GetWallFoorTrianglesForShape(
            int numSides, double shapeRadius, double shapeAngle, double shapeX, double shapeZ)
        {
            List<(double, double)> vertices = new List<(double, double)>();
            for (int i = 0; i < numSides; i++)
            {
                double angle = 65536d / numSides * i + shapeAngle;
                (double, double) vertex = MoreMath.AddVectorToPoint(shapeRadius, angle, shapeX, shapeZ);
                vertices.Add(vertex);
            }

            List<((double, double), (double, double), bool)> vertexPairs =
                new List<((double, double), (double, double), bool)>();
            for (int i = 0; i < vertices.Count; i++)
            {
                (double v1X, double v1Z) = vertices[i];
                (double v2X, double v2Z) = vertices[(i + 1) % vertices.Count];
                ushort angle = MoreMath.AngleTo_AngleUnitsRounded(v1X, v1Z, v2X, v2Z);
                bool xProj = (angle <= 8192) ||
                             (angle >= 24576 && angle <= 40960) ||
                             (angle >= 57344);
                vertexPairs.Add(((v1X, v1Z), (v2X, v2Z), xProj));
            }

            List<TriangleShape> wallTris = new List<TriangleShape>();
            foreach (var ((x1, z1), (x2, z2), proj) in vertexPairs)
            {
                double angle = MoreMath.AngleTo_AngleUnits(x1, z1, x2, z2);
                double projAngle = proj ? 16384 : 0;
                double projDist = 50 / Math.Sin(MoreMath.AngleUnitsToRadians(angle - projAngle));
                (double p1X, double p1Z) = MoreMath.AddVectorToPoint(projDist, projAngle, x1, z1);
                (double p2X, double p2Z) = MoreMath.AddVectorToPoint(-1 * projDist, projAngle, x1, z1);
                (double p3X, double p3Z) = MoreMath.AddVectorToPoint(projDist, projAngle, x2, z2);
                (double p4X, double p4Z) = MoreMath.AddVectorToPoint(-1 * projDist, projAngle, x2, z2);

                TriangleShape triShape1 = new TriangleShape(p1X, 0, p1Z, p2X, 0, p2Z, p3X, 0, p3Z);
                TriangleShape triShape2 = new TriangleShape(p2X, 0, p2Z, p3X, 0, p3Z, p4X, 0, p4Z);
                wallTris.Add(triShape1);
                wallTris.Add(triShape2);
            }

            List<TriangleShape> floorTris = new List<TriangleShape>();
            foreach (var ((x1, z1), (x2, z2), proj) in vertexPairs)
            {
                TriangleShape triShape = new TriangleShape(x1, 0, z1, x2, 0, z2, shapeX, 0, shapeZ);
                floorTris.Add(triShape);
            }

            return (floorTris, wallTris);
        }

        public static (float normX, float normY, float normZ, float normOffset) GetNorms(
            int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
        {
            float nx = (y2 - y1) * (z3 - z2) - (z2 - z1) * (y3 - y2);
            float ny = (z2 - z1) * (x3 - x2) - (x2 - x1) * (z3 - z2);
            float nz = (x2 - x1) * (y3 - y2) - (y2 - y1) * (x3 - x2);
            float mag = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz);

            mag = 1 / mag;
            nx *= mag;
            ny *= mag;
            nz *= mag;

            float originOffset = -(nx * x1 + ny * y1 + nz * z1);

            return (nx, ny, nz, originOffset);
        }

        public static (TriangleDataModel, float) FindFloorAndY(float floatX, float floatY, float floatZ)
        {
            TriangleDataModel tri = FindFloor(floatX, floatY, floatZ);
            if (tri == null) return (tri, -11000);
            float y = tri.GetTruncatedHeightOnTriangle(floatX, floatZ);
            return (tri, y);
        }

        public static TriangleDataModel FindFloor(float floatX, float floatY, float floatZ)
        {
            int LEVEL_BOUNDARY_MAX = 0x2000;
            int CELL_SIZE = 0x400;

            short shortX = (short)floatX;
            short shortY = (short)floatY;
            short shortZ = (short)floatZ;
            
            if (shortX <= -LEVEL_BOUNDARY_MAX || shortX >= LEVEL_BOUNDARY_MAX)
            {
                return null;
            }
            if (shortZ <= -LEVEL_BOUNDARY_MAX || shortZ >= LEVEL_BOUNDARY_MAX)
            {
                return null;
            }

            int cellX = ((shortX + LEVEL_BOUNDARY_MAX) / CELL_SIZE) & 0xF;
            int cellZ = ((shortZ + LEVEL_BOUNDARY_MAX) / CELL_SIZE) & 0xF;

            TriangleDataModel staticTri = FindFloorFromList(shortX, shortY, shortZ, cellX, cellZ, true);
            TriangleDataModel dynamicTri = FindFloorFromList(shortX, shortY, shortZ, cellX, cellZ, false);

            if (staticTri == null && dynamicTri == null) return null;
            if (staticTri == null) return dynamicTri;
            if (dynamicTri == null) return staticTri;

            double yOnStaticTri = staticTri.GetHeightOnTriangle(shortX, shortZ);
            double yOnDynamicTri = dynamicTri.GetHeightOnTriangle(shortX, shortZ);
            return yOnDynamicTri > yOnStaticTri ? dynamicTri : staticTri;
        }

        private static TriangleDataModel FindFloorFromList(short shortX, short shortY, short shortZ, int cellX, int cellZ, bool isStaticParition)
        {
            uint partitionAddress = isStaticParition ? TriangleConfig.StaticTrianglePartitionAddress : TriangleConfig.DynamicTrianglePartitionAddress;
            int type = 0;

            int typeSize = 2 * 4;
            int xSize = 3 * typeSize;
            int zSize = 16 * xSize;
            uint address = (uint)(partitionAddress + cellZ * zSize + cellX * xSize + type * typeSize);
            address = Config.Stream.GetUInt32(address);

            while (address != 0)
            {
                uint triAddress = Config.Stream.GetUInt32(address + 4);
                TriangleDataModel tri = TriangleDataModel.Create(triAddress);
                bool isLegitimateTriangle = tri.NormX != 0 || tri.NormY != 0 || tri.NormZ != 0;
                if (isLegitimateTriangle && tri.IsPointInsideAndAboveTriangle(shortX, shortY, shortZ)) return tri;
                address = Config.Stream.GetUInt32(address);
            }

            return null;
        }
    }
} 
