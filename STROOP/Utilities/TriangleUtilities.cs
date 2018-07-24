﻿using STROOP.Forms;
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

        public static List<TriangleDataModel> GetAllTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(TriangleConfig.TriangleListPointerAddress);
            int numTotalTriangles = Config.Stream.GetInt32(TriangleConfig.TotalTriangleCountAddress);
            return GetTrianglesInRange(triangleListAddress, numTotalTriangles);
        }

        public static List<TriangleDataModel> GetTrianglesInRange(uint startAddress, int numTriangles)
        {
            List<TriangleDataModel> triangleList = new List<TriangleDataModel>();
            for (int i = 0; i < numTriangles; i++)
            {
                uint address = startAddress + (uint)(i * TriangleConfig.TriangleStructSize);
                TriangleDataModel triangle = new TriangleDataModel(address);
                triangleList.Add(triangle);
            }
            return triangleList;
        }

        public static List<uint> GetTriangleAddressesInRange(uint startAddress, int numTriangles)
        {
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

        public static void NeutralizeTriangles(TriangleClassification? classification = null)
        {
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                float ynorm = Config.Stream.GetSingle(address + TriangleOffsetsConfig.NormY);
                TriangleClassification triClassification = CalculateClassification(ynorm);
                if (classification == null || classification == triClassification)
                {
                    ButtonUtilities.NeutralizeTriangle(address);
                }
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
            foreach (var ((x1,z1),(x2,z2),proj) in vertexPairs)
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
    }
} 
