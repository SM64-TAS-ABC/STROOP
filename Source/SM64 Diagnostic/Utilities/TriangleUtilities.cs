using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Utilities
{
    public static class TriangleUtilities
    {
        public static List<TriangleStruct> GetLevelTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(Config.Triangle.TriangleListPointerAddress);
            int numLevelTriangles = Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress);
            return GetTrianglesInRange(triangleListAddress, numLevelTriangles);
        }

        public static List<uint> GetLevelTriangleAddresses()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(Config.Triangle.TriangleListPointerAddress);
            int numLevelTriangles = Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress);
            return GetTriangleAddressesInRange(triangleListAddress, numLevelTriangles);
        }

        public static List<TriangleStruct> GetObjectTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(Config.Triangle.TriangleListPointerAddress);
            int numTotalTriangles = Config.Stream.GetInt32(Config.Triangle.TotalTriangleCountAddress);
            int numLevelTriangles = Config.Stream.GetInt32(Config.Triangle.LevelTriangleCountAddress);

            uint objectTriangleListAddress = triangleListAddress + (uint)(numLevelTriangles * Config.Triangle.TriangleStructSize);
            int numObjectTriangles = numTotalTriangles - numLevelTriangles;

            return GetTrianglesInRange(objectTriangleListAddress, numObjectTriangles);
        }

        public static List<TriangleStruct> GetAllTriangles()
        {
            uint triangleListAddress = Config.Stream.GetUInt32(Config.Triangle.TriangleListPointerAddress);
            int numTotalTriangles = Config.Stream.GetInt32(Config.Triangle.TotalTriangleCountAddress);
            return GetTrianglesInRange(triangleListAddress, numTotalTriangles);
        }

        public static List<TriangleStruct> GetTrianglesInRange(uint startAddress, int numTriangles)
        {
            List<TriangleStruct> triangleList = new List<TriangleStruct>();
            for (int i = 0; i < numTriangles; i++)
            {
                uint address = startAddress + (uint)(i * Config.Triangle.TriangleStructSize);
                TriangleStruct triangle = new TriangleStruct(address);
                triangleList.Add(triangle);
            }
            return triangleList;
        }

        public static List<uint> GetTriangleAddressesInRange(uint startAddress, int numTriangles)
        {
            List<uint> triangleAddressList = new List<uint>();
            for (int i = 0; i < numTriangles; i++)
            {
                uint address = startAddress + (uint)(i * Config.Triangle.TriangleStructSize);
                triangleAddressList.Add(address);
            }
            return triangleAddressList;
        }

        public static void ShowTriangles(List<TriangleStruct> triangleList)
        {
            TriangleInfoForm triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetTriangles(triangleList);
            triangleInfoForm.ShowDialog();
        }

        public static void NeutralizeTriangles(TriangleClassification? classification = null)
        {
            List<uint> triangleAddresses = GetLevelTriangleAddresses();
            triangleAddresses.ForEach(address =>
            {
                float ynorm = Config.Stream.GetSingle(address + Config.TriangleOffsets.NormY);
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
                float ynorm = Config.Stream.GetSingle(address + Config.TriangleOffsets.NormY);
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
    }
} 
