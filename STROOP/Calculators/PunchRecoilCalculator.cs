using STROOP.Controls;
using STROOP.Forms;
using STROOP.M64;
using STROOP.Managers;
using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Utilities
{
    public static class PunchRecoilCalculator
    {
        public static readonly int ShaftSideForwards1Index = 50;
        public static readonly int ShaftSideForwards2Index = 48;
        public static readonly int ShaftSideBackwardsIndex = 44;
        public static readonly int ShaftTopIndex = 46;
        public static readonly int HeadFloor1Index = 2;
        public static readonly int HeadFloor2Index = 3;
        public static readonly int HeadWall1Index = 8;
        public static readonly int HeadWall2Index = 35;

        public static Dictionary<(ushort angle, int triIndex), TriangleDataModel> Dictionary;

        public static void Test()
        {
            Config.Print("START");
            FindWallOverlaps();
            Config.Print("DONE");
        }

        public static void FindWallOverlaps()
        {
            SetUpDictionary();
            List<DropDownPoint> dropDownPoints = new List<DropDownPoint>();
            foreach (int angle in MaybeAngles)
            {
                TriangleDataModel ShaftSideForwards1Tri = GetDictionaryValue(angle, ShaftSideForwards1Index);
                TriangleDataModel ShaftSideForwards2Tri = GetDictionaryValue(angle, ShaftSideForwards2Index);
                TriangleDataModel ShaftSideBackwardsTri = GetDictionaryValue(angle, ShaftSideBackwardsIndex);
                TriangleDataModel ShaftTopTri = GetDictionaryValue(angle, ShaftTopIndex);
                TriangleDataModel HeadFloor1Tri = GetDictionaryValue(angle, HeadFloor1Index);
                TriangleDataModel HeadFloor2Tri = GetDictionaryValue(angle, HeadFloor2Index);

                int xMin = Math.Min(ShaftSideForwards1Tri.GetMinX(), ShaftSideForwards2Tri.GetMinX()) - 1;
                int xMax = Math.Max(ShaftSideForwards1Tri.GetMaxX(), ShaftSideForwards2Tri.GetMaxX()) + 1;
                int zMin = Math.Min(ShaftSideForwards1Tri.GetMinZ(), ShaftSideForwards2Tri.GetMinZ()) - 1;
                int zMax = Math.Max(ShaftSideForwards1Tri.GetMaxZ(), ShaftSideForwards2Tri.GetMaxZ()) + 1;
                List<(int x, float y, int z)> ShaftSideForwardsTriPoints = new List<(int x, float y, int z)>();
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        float? y1 = ShaftSideForwards1Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(x, z);
                        float? y2 = ShaftSideForwards2Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(x, z);
                        float? topY = ShaftTopTri.GetTruncatedHeightOnTriangleIfInsideTriangle(x, z);
                        if (y1.HasValue && !topY.HasValue)
                        {
                            ShaftSideForwardsTriPoints.Add((x, y1.Value, z));
                        }
                        if (y2.HasValue && !topY.HasValue)
                        {
                            ShaftSideForwardsTriPoints.Add((x, y2.Value, z));
                        }
                    }
                }

                foreach (var point in ShaftSideForwardsTriPoints)
                {
                    int dropDownX = point.x;
                    int dropDownZ = point.z - 1;
                    float? y1 = HeadFloor1Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(dropDownX, dropDownZ);
                    float? y2 = HeadFloor2Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(dropDownX, dropDownZ);
                    if (y1.HasValue && point.y - y1.Value <= 100)
                    {
                        dropDownPoints.Add(new DropDownPoint(angle, dropDownX, y1.Value, dropDownZ));
                    }
                    if (y2.HasValue && point.y - y2.Value <= 100)
                    {
                        dropDownPoints.Add(new DropDownPoint(angle, dropDownX, y2.Value, dropDownZ));
                    }
                }
            }

            for (int angle = 0; angle >= -16384; angle -= 16)
            {
                TriangleDataModel HeadWall1Tri = GetDictionaryValue(angle, HeadWall1Index);
                TriangleDataModel HeadWall2Tri = GetDictionaryValue(angle, HeadWall2Index);
                foreach (var point in dropDownPoints)
                {
                    float x = point.X;
                    float y = point.Y;
                    float z1 = point.Z;
                    float z2 = point.Z - 0.999f;
                    int countZ1Tri1 = WallDisplacementCalculator.GetNumWallCollisions(x, y, z1, new List<TriangleDataModel>() { HeadWall1Tri }, 50, 60);
                    int countZ1Tri2 = WallDisplacementCalculator.GetNumWallCollisions(x, y, z1, new List<TriangleDataModel>() { HeadWall2Tri }, 50, 60);
                    int countZ2Tri1 = WallDisplacementCalculator.GetNumWallCollisions(x, y, z2, new List<TriangleDataModel>() { HeadWall1Tri }, 50, 60);
                    int countZ2Tri2 = WallDisplacementCalculator.GetNumWallCollisions(x, y, z2, new List<TriangleDataModel>() { HeadWall2Tri }, 50, 60);
                    if (countZ1Tri1 + countZ1Tri2 > 0 &&
                        countZ2Tri1 + countZ2Tri2 > 0 &&
                        (countZ1Tri1 + "" + countZ1Tri2) != (countZ2Tri1 + "" + countZ2Tri2))
                    {
                        for (float z = z1; z >= z2; z = MoreMath.GetPreviousFloat(z))
                        {
                            int count = WallDisplacementCalculator.GetNumWallCollisions(x, y, z, new List<TriangleDataModel>() { HeadWall1Tri, HeadWall2Tri }, 50, 60);
                            if (count != 1)
                            {
                                Config.Print(
                                    "{0}=>{1}, {2} {3} {4}, {5}",
                                    point.Angle, angle, x, (double)y, z, count);
                            }
                        }
                    }
                }
            }
        }

        public class DropDownPoint
        {
            public readonly int Angle;
            public readonly int X;
            public readonly float Y;
            public readonly int Z;

            public DropDownPoint(int angle, int x, float y, int z)
            {
                Angle = angle;
                X = x;
                Y = y;
                Z = z;
            }
        }

        public static void FindGoodAngles()
        {
            SetUpDictionary();
            for (int angle = -4000; angle >= -11000; angle -= 16)
            {
                TriangleDataModel ShaftSideForwards1Tri = GetDictionaryValue(angle, ShaftSideForwards1Index);
                TriangleDataModel ShaftSideForwards2Tri = GetDictionaryValue(angle, ShaftSideForwards2Index);
                TriangleDataModel ShaftSideBackwardsTri = GetDictionaryValue(angle, ShaftSideBackwardsIndex);
                TriangleDataModel ShaftTopTri = GetDictionaryValue(angle, ShaftTopIndex);
                TriangleDataModel HeadFloor1Tri = GetDictionaryValue(angle, HeadFloor1Index);
                TriangleDataModel HeadFloor2Tri = GetDictionaryValue(angle, HeadFloor2Index);

                bool goodClassifications =
                    (ShaftSideForwards1Tri.Classification == TriangleClassification.Floor ||
                        ShaftSideForwards2Tri.Classification == TriangleClassification.Floor) &&
                    //ShaftSideBackwardsTri.Classification != TriangleClassification.Wall &&
                    HeadFloor1Tri.Classification == TriangleClassification.Floor &&
                    HeadFloor2Tri.Classification == TriangleClassification.Floor;
                if (!goodClassifications) continue;

                Config.Print(angle);
            }
        }

        public static void SetUpDictionary()
        {
            string filePath = DialogUtilities.GetFilePath(FileType.Text);
            if (filePath == null) return;
            List<string> fileLines = DialogUtilities.ReadFileLines(filePath);
            Dictionary = new Dictionary<(ushort angle, int triIndex), TriangleDataModel>();
            foreach (string line in fileLines)
            {
                List<string> parts = ParsingUtilities.ParseStringList(line);
                ushort angle = MoreMath.NormalizeAngleUshort(ParsingUtilities.ParseInt(parts[0]));
                uint address = ParsingUtilities.ParseHex(parts[1]);
                int triIndex = (int)((address - 0x8016AFB0) / 0x30); // 54 indexes
                int x1 = ParsingUtilities.ParseInt(parts[2]);
                int y1 = ParsingUtilities.ParseInt(parts[3]);
                int z1 = ParsingUtilities.ParseInt(parts[4]);
                int x2 = ParsingUtilities.ParseInt(parts[5]);
                int y2 = ParsingUtilities.ParseInt(parts[6]);
                int z2 = ParsingUtilities.ParseInt(parts[7]);
                int x3 = ParsingUtilities.ParseInt(parts[8]);
                int y3 = ParsingUtilities.ParseInt(parts[9]);
                int z3 = ParsingUtilities.ParseInt(parts[10]);
                TriangleDataModel tri = new TriangleDataModel(x1, y1, z1, x2, y2, z2, x3, y3, z3);
                Dictionary[(angle, triIndex)] = tri;
            }
        }

        public static TriangleDataModel GetDictionaryValue(int angle, int triIndex)
        {
            ushort truncated = MoreMath.NormalizeAngleTruncated(angle);
            return Dictionary[(truncated, triIndex)];
        }

        public static string GetTriPoints(TriangleDataModel tri)
        {
            return string.Format(
                "{0} {1} {2} {3} {4} {5} {6} {7} {8}",
                tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);
        }

        public static List<int> GoodAngles = new List<int>()
        {
            -6960,
            -6976,
            -6992,
            -7056,
            -7072,
            -7088,
            -7104,
            -7552,
            -8112,
            -8128,
            -8144,
            -8160,
            -8176,
            -8192,
            -8208,
            -8224,
            -8240,
            -8256,
            -8272,
            -9184,
            -9200,
            -9216,
            -9232,
            -9248,
            -9264,
            -9296,
            -10080,
            -10160,
            -10176,
            -10192,
            -10224,
            -10240,
            -10672,
        };

        public static List<int> MaybeAngles = new List<int>()
        {
            -5600,
            -5616,
            -5632,
            -5648,
            -5680,
            -5696,
            -5712,
            -5728,
            -5760,
            -6256,
            -6448,
            -6560,
            -6672,
            -6816,
            -6928,
            -6960,
            -6976,
            -6992,
            -7008,
            -7024,
            -7040,
            -7056,
            -7072,
            -7088,
            -7104,
            -7280,
            -7488,
            -7520,
            -7552,
            -7888,
            -7984,
            -8016,
            -8048,
            -8080,
            -8112,
            -8128,
            -8144,
            -8160,
            -8176,
            -8192,
            -8208,
            -8224,
            -8240,
            -8256,
            -8272,
            -8304,
            -8640,
            -8672,
            -8704,
            -8880,
            -8912,
            -9056,
            -9088,
            -9184,
            -9200,
            -9216,
            -9232,
            -9248,
            -9264,
            -9280,
            -9296,
            -9344,
            -9568,
            -9680,
            -9760,
            -9840,
            -10080,
            -10160,
            -10176,
            -10192,
            -10208,
            -10224,
            -10240,
            -10256,
            -10496,
            -10544,
            -10672,
        };
    }
}
