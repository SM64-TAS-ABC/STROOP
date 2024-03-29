﻿using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
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
        private int _numLevelTriangles;
        private List<TriangleDataModel>[,,] _staticTris;
        private List<TriangleDataModel>[,,] _dynamicTris;
        private List<(int y, int xMin, int xMax, int zMin, int zMax)> _waterLevels;

        public CellSnapshot()
        {
            _numLevelTriangles = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);
            _staticTris = GetTrianglesInPartition(true);
            _dynamicTris = GetTrianglesInPartition(false);
            _waterLevels = WaterUtilities.GetWaterLevels();
        }

        private List<TriangleDataModel>[,,] GetTrianglesInPartition(bool staticPartition)
        {
            List<TriangleDataModel>[,,] tris = new List<TriangleDataModel>[16, 16, 3];
            for (int z = 0; z < 16; z++)
            {
                for (int x = 0; x < 16; x++)
                {
                    tris[z, x, 0] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Floor).ConvertAll(triAddress => TriangleDataModel.CreateLazy(triAddress));
                    tris[z, x, 1] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Ceiling).ConvertAll(triAddress => TriangleDataModel.CreateLazy(triAddress));
                    tris[z, x, 2] = CellUtilities.GetTriangleAddressesInCell(x, z, staticPartition, TriangleClassification.Wall).ConvertAll(triAddress => TriangleDataModel.CreateLazy(triAddress));
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

        public (TriangleDataModel, float) FindCeilingAndY(float floatX, float floatY, float floatZ)
        {
            TriangleDataModel tri = FindCeiling(floatX, floatY, floatZ);
            if (tri == null) return (tri, 20_000);
            float y = tri.GetTruncatedHeightOnTriangle(floatX, floatZ);
            return (tri, y);
        }

        public TriangleDataModel FindCeiling(float floatX, float floatY, float floatZ)
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

        private TriangleDataModel FindCeilingFromList(short shortX, short shortY, short shortZ, int cellX, int cellZ, bool isStaticParition)
        {
            List<TriangleDataModel>[,,] tris = isStaticParition ? _staticTris : _dynamicTris;
            List<TriangleDataModel> ceilingTris = tris[cellZ, cellX, 1];

            foreach (TriangleDataModel tri in ceilingTris)
            {
                bool isLegitimateTriangle = tri.NormX != 0 || tri.NormY != 0 || tri.NormZ != 0;
                if (isLegitimateTriangle && tri.IsPointInsideAndBelowTriangle(shortX, shortY, shortZ)) return tri;
            }

            return null;
        }

        public int GetWaterAtPos(float x, float z)
        {
            foreach (var w in _waterLevels)
            {
                if (x > w.xMin && x < w.xMax && z > w.zMin && z < w.zMax)
                {
                    return w.y;
                }
            }
            return -11000;
        }

        public void Update()
        {
            int numLevelTriangles = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);
            if (_numLevelTriangles != numLevelTriangles)
            {
                _numLevelTriangles = numLevelTriangles;
                _staticTris = GetTrianglesInPartition(true);
                _waterLevels = WaterUtilities.GetWaterLevels();
            }
            _dynamicTris = GetTrianglesInPartition(false);
        }
    }
}
