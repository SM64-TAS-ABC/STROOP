using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class CellSnapshot
    {
        private readonly List<TriangleDataModel>[,,] _staticTris;
        private readonly List<TriangleDataModel>[,,] _dynamicTris;

        public CellSnapshot()
        {
            _staticTris = GetTrianglesInPartition(true);
            _dynamicTris = GetTrianglesInPartition(false);
        }

        private List<TriangleDataModel>[,,] GetTrianglesInPartition(bool staticPartition)
        {
            List<TriangleDataModel>[,,] tris = new List<TriangleDataModel>[16, 16, 3];
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    tris[z, x, 0] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Floor).ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
                    tris[z, x, 1] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Ceiling).ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
                    tris[z, x, 2] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Wall).ConvertAll(triAddress => TriangleDataModel.Create(triAddress));
                }
            }
            return tris;
        }

        private int GetTypeFromClassification(TriangleClassification classification)
        {
            switch (classification)
            {
                case TriangleClassification.Wall:
                    return 2;
                case TriangleClassification.Floor:
                    return 0;
                case TriangleClassification.Ceiling:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public List<TriangleDataModel> GetTrianglesInCell(int cellX, int cellZ, bool staticPartition, TriangleClassification classification)
        {
            List<TriangleDataModel>[,,] tris = staticPartition ? _staticTris : _dynamicTris;
            int type = GetTypeFromClassification(classification);
            return tris[cellZ, cellX, type];
        }

        public List<TriangleDataModel> GetTrianglesAtPosition(float x, float z, bool staticPartition, TriangleClassification classification)
        {
            (int cellX, int cellZ) = CellUtilities.GetCell(x, z);
            return GetTrianglesInCell(cellX, cellZ, staticPartition, classification);
        }

        public (TriangleDataModel, float) FindFloorAndY(float floatX, float floatY, float floatZ)
        {
            TriangleDataModel tri = FindFloor(floatX, floatY, floatZ);
            if (tri == null) return (tri, -11000);
            float y = tri.GetTruncatedHeightOnTriangle(floatX, floatZ);
            return (tri, y);
        }

        public TriangleDataModel FindFloor(float floatX, float floatY, float floatZ)
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

        private TriangleDataModel FindFloorFromList(short shortX, short shortY, short shortZ, int cellX, int cellZ, bool isStaticParition)
        {
            List<TriangleDataModel>[,,] tris = isStaticParition ? _staticTris : _dynamicTris;
            List<TriangleDataModel> floorTris = tris[cellZ, cellX, 0];
            
            foreach (TriangleDataModel tri in floorTris)
            {
                bool isLegitimateTriangle = tri.NormX != 0 || tri.NormY != 0 || tri.NormZ != 0;
                if (isLegitimateTriangle && tri.IsPointInsideAndAboveTriangle(shortX, shortY, shortZ)) return tri;
            }

            return null;
        }
    }
}
