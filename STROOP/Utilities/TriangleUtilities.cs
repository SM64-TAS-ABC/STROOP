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

        public static void AnnihilateAllCeilings()
        {
            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();
            List<uint> ceilingAddresses = GetLevelTriangles()
                .FindAll(tri => tri.IsCeiling())
                .ConvertAll(tri => tri.Address);
            ButtonUtilities.AnnihilateTriangle(ceilingAddresses);
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
            int type = 0; // floor

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

        public static (TriangleDataModel, float) FindCeilingAndY(float floatX, float floatY, float floatZ)
        {
            TriangleDataModel tri = FindCeiling(floatX, floatY + 80, floatZ);
            if (tri == null) return (tri, 20000);
            float y = tri.GetTruncatedHeightOnTriangle(floatX, floatZ);
            return (tri, y);
        }

        public static TriangleDataModel FindCeiling(float floatX, float floatY, float floatZ)
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

            TriangleDataModel staticTri = FindCeilingFromList(shortX, shortY, shortZ, cellX, cellZ, true);
            TriangleDataModel dynamicTri = FindCeilingFromList(shortX, shortY, shortZ, cellX, cellZ, false);

            if (staticTri == null && dynamicTri == null) return null;
            if (staticTri == null) return dynamicTri;
            if (dynamicTri == null) return staticTri;

            double yOnStaticTri = staticTri.GetHeightOnTriangle(shortX, shortZ);
            double yOnDynamicTri = dynamicTri.GetHeightOnTriangle(shortX, shortZ);
            return yOnDynamicTri < yOnStaticTri ? dynamicTri : staticTri;
        }

        private static TriangleDataModel FindCeilingFromList(short shortX, short shortY, short shortZ, int cellX, int cellZ, bool isStaticParition)
        {
            uint partitionAddress = isStaticParition ? TriangleConfig.StaticTrianglePartitionAddress : TriangleConfig.DynamicTrianglePartitionAddress;
            int type = 1; // ceiling

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
                if (isLegitimateTriangle && tri.IsPointInsideAndBelowTriangle(shortX, shortY, shortZ)) return tri;
                address = Config.Stream.GetUInt32(address);
            }

            return null;
        }
    }
} 
