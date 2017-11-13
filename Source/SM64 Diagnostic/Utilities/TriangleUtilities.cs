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

        public static void ShowTriangles(List<TriangleStruct> triangleList)
        {
            TriangleInfoForm triangleInfoForm = new TriangleInfoForm();
            triangleInfoForm.SetTriangles(triangleList);
            triangleInfoForm.ShowDialog();
        }

        public static void NeutralizeAllTriangles()
        {

        }

        public static void DisableAllCamCollision()
        {

        }
    }
}
