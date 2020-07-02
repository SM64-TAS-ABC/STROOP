using STROOP.Controls;
using STROOP.Forms;
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

namespace STROOP.Utilities
{
    public static class TestUtilities
    {
        public static void Update()
        {
            //UpdateRacingPenguinWaypoints2();
            //Config.SetDebugText(DictionaryUtilities.GetString(Config.ObjectSlotsManager.MarkedSlotsAddressesDictionary));
        }

        public static void TestSomething()
        {

        }

        public static void TestSomethingElse()
        {
            CalculatorMain.TestBobomb3();
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public static void SearchForBadWallTriangles()
        {
            List<TriangleDataModel> wallTris = TriangleUtilities.GetLevelTriangles().FindAll(tri => tri.IsWall());
            wallTris = new List<TriangleDataModel>() { new TriangleDataModel(0x801A47C0) };

            List<TriangleDataModel> badWallTris = new List<TriangleDataModel>();
            foreach (TriangleDataModel wallTri in wallTris)
            {
                (float x1, float z1, float x2, float z2, bool xProjection) = MapUtilities.Get2DWallDataFromTri(wallTri).Value;
               
                float angle = (float)MoreMath.AngleTo_Radians(x1, z1, x2, z2);
                float projectionDist = 50 / (float)Math.Abs(xProjection ? Math.Cos(angle) : Math.Sin(angle));
                List<(float x, float z)> points = new List<(float x, float z)>();
                Action<float, float> addPoint = (float xAdd, float zAdd) =>
                {
                    points.AddRange(new List<(float x, float z)>()
                    {
                        (x1, z1),
                        (x1 + xAdd, z1 + zAdd),
                        (x2 + xAdd, z2 + zAdd),
                        (x2, z2),
                    });
                };
                if (xProjection)
                {
                    addPoint(projectionDist, 0);
                    addPoint(-1 * projectionDist, 0);
                }
                else
                {
                    addPoint(0, projectionDist);
                    addPoint(0, -1 * projectionDist);
                }

                short xMin = (short)points.Min(p => p.x);
                short xMax = (short)points.Max(p => p.x);
                short zMin = (short)points.Min(p => p.z);
                short zMax = (short)points.Max(p => p.z);

                List<(int x, int z)> hitboxCells = CellUtilities.GetCells(xMin, xMax, zMin, zMax);
                List<(int x, int z)> triCells = CellUtilities.GetCells(wallTri);
                if (hitboxCells.Any(cell => !triCells.Contains(cell)))
                {
                    badWallTris.Add(wallTri);
                }
            }

            List<string> addresses = badWallTris.ConvertAll(tri => HexUtilities.FormatValue(tri.Address));
            InfoForm.ShowValue(string.Join(",", addresses));
        }

        public static void SearchForRamStart()
        {
            byte[] memory = Config.Stream.ReadAllMemory();
            InfoForm.ShowValue(memory.Length);
        }

        public static void GetFirstBytes()
        {
            byte[] byteArray = Config.Stream.ReadRam(0x80000000, 100, EndiannessType.Little, false);
            List<byte> byteList = byteArray.ToList();
            string output = string.Join(" ", byteList);
            InfoForm.ShowValue(output);
        }

        public static void LookForBytes()
        {
            byte[] byteArray = Config.Stream.ReadRam(0x80000000, 100, EndiannessType.Little, false);
            List<byte> byteList = byteArray.ToList();
            string output = string.Join(" ", byteList);
            InfoForm.ShowValue(output);
        }

        public static void GetAllInGameAngles()
        {
            List<int> inGameAngles = new List<int>();
            List<int> allAngles = Enumerable.Range(0, 65536).ToList();
            foreach (int angle in allAngles)
            {
                (double x, double z) = MoreMath.GetComponentsFromVector(1, angle);
                int inGameAngle = InGameTrigUtilities.InGameAngleTo(x, z);
                if (!inGameAngles.Contains(inGameAngle))
                {
                    inGameAngles.Add(inGameAngle);
                }
            }
            InfoForm.ShowValue(string.Join(",", inGameAngles));
        }

        public static void TestWarpNodes()
        {
            List<string> lines = new List<string>();
            uint address = WatchVariableSpecialUtilities.GetWarpNodesAddress();
            while (address != 0)
            {
                byte id = Config.Stream.GetByte(address + 0x0);
                byte destLevel = Config.Stream.GetByte(address + 0x1);
                byte destArea = Config.Stream.GetByte(address + 0x2);
                byte destNode = Config.Stream.GetByte(address + 0x3);
                uint obj = Config.Stream.GetUInt32(address + 0x4);
                uint next = Config.Stream.GetUInt32(address + 0x8);
                string line = id + " " + destLevel + " " + destArea + " " + destNode + " " + HexUtilities.FormatValue(obj) + " " + HexUtilities.FormatValue(next);
                lines.Add(line);
                address = next;
            }
            InfoForm.ShowValue(string.Join("\r\n", lines));
        }

        public static void TestLllFloorGaps()
        {
            Config.Print("START");
            List<(int x, int z)> gaps = new List<(int x, int z)>();
            int y = 2721;
            for (int x = -787; x <= 583; x++)
            {
                Config.Print("x = " + x);
                for (int z = -2942; z <= -2473; z++)
                {
                    TriangleDataModel tri = TriangleUtilities.FindFloor(x, y, z);
                    if (tri == null) gaps.Add((x, z));
                }
            }
            foreach ((int x, int z) in gaps)
            {
                Config.Print("{0}.5, {1}.5", x, z);
            }
            Config.Print("END");
        }

        private static void UpdateMoneybagHome()
        {
            uint coinAddress = 0x8034D2A8;

            Config.Stream.Suspend();
            ObjectDataModel obj = Config.ObjectSlotsManager.GetLoadedObjectsWithName("Moneybag").FirstOrDefault();
            if (obj != null)
            {
                uint moneybagAddress = obj.Address;
                float homeX = Config.Stream.GetSingle(moneybagAddress + ObjectConfig.HomeXOffset);
                float homeY = Config.Stream.GetSingle(moneybagAddress + ObjectConfig.HomeYOffset);
                float homeZ = Config.Stream.GetSingle(moneybagAddress + ObjectConfig.HomeZOffset);
                Config.Stream.SetValue(homeX, coinAddress + ObjectConfig.XOffset);
                Config.Stream.SetValue(homeY, coinAddress + ObjectConfig.YOffset);
                Config.Stream.SetValue(homeZ, coinAddress + ObjectConfig.ZOffset);
            }
            Config.Stream.Resume();
        }

        private static void UpdateScuttlebugHome()
        {
            uint coinAddress = 0x8034F188;
            uint scuttlebugAddress = 0x80344B48;

            Config.Stream.Suspend();
            float homeX = Config.Stream.GetSingle(scuttlebugAddress + ObjectConfig.HomeXOffset);
            float homeY = Config.Stream.GetSingle(scuttlebugAddress + ObjectConfig.HomeYOffset);
            float homeZ = Config.Stream.GetSingle(scuttlebugAddress + ObjectConfig.HomeZOffset);
            Config.Stream.SetValue(homeX, coinAddress + ObjectConfig.XOffset);
            Config.Stream.SetValue(homeY, coinAddress + ObjectConfig.YOffset);
            Config.Stream.SetValue(homeZ, coinAddress + ObjectConfig.ZOffset);
            Config.Stream.Resume();
        }

        private static List<int> _eel2Waypoints = new List<int>()
        {
              5381,     0,  2758,
              5803, -3130,  3036,
              4876, -3045,  2706,
              4313, -3246,  2736,
              3792, -3413,  3668,
              4971, -3130,  3688,
              5392, -3130,  3326,
              6341, -3130,  2450,
              9431, -3130,  1400,
        };

        private static List<uint> _eel2Addresses = new List<uint>()
        {
            0x8035AAC8,0x8035F6C8,0x8035E168,0x80357DA8,0x80359A28,0x8035CC08,0x80357428,0x80359C88,0x80341248
        };

        private static void UpdateEel2Waypoints()
        {
            uint eelAddress = 0x8034EF28;
            uint waypointAddress = Config.Stream.GetUInt32(eelAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);
            //if (waypointAddress == 0x80108824)
            //{
            //    waypointX = 5300;
            //    waypointY = -3800;
            //    waypointZ = 1200;
            //}

            foreach (uint address in _eel2Addresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetEel2Waypoints()
        {
            for (int i = 0; i < _eel2Addresses.Count; i++)
            {
                Config.Stream.SetValue((float)_eel2Waypoints[3 * i + 0], _eel2Addresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_eel2Waypoints[3 * i + 1], _eel2Addresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_eel2Waypoints[3 * i + 2], _eel2Addresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<int> _eel1Waypoints = new List<int>()
        {
              5300, -3800,  1200,
              3700, -3600,  1700,
              3400, -3400,  3500,
              3900, -3600,  4400,
              5300, -3800,  4400,
              6200, -4000,  2700,
        };

        private static List<uint> _eel1Addresses = new List<uint>()
        {
            0x80341248,0x80340FE8,0x80340D88,0x80340B28,0x803408C8,0x80340668
        };

        private static void UpdateEel1Waypoints()
        {
            uint eelAddress = 0x8034E808;
            uint waypointAddress = Config.Stream.GetUInt32(eelAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);
            if (waypointAddress == 0x80108824)
            {
                waypointX = 5300;
                waypointY = -3800;
                waypointZ = 1200;
            }

            foreach (uint address in _eel1Addresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetEel1Waypoints()
        {
            for (int i = 0; i < _eel1Addresses.Count; i++)
            {
                Config.Stream.SetValue((float)_eel1Waypoints[3 * i + 0], _eel1Addresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_eel1Waypoints[3 * i + 1], _eel1Addresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_eel1Waypoints[3 * i + 2], _eel1Addresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<int> _snowmanWaypoints = new List<int>()
        {
              2501,  2662,  -975,
              2533,  2560,  -800,
              2566,  2300,  -500,
              2600,  1884,   733,
              2466,  1647,  1835,
              2000,  1483,  2233,
               766,  1321,  2400,
              -872,  1190,  2033,
             -3072,  1097,  1300,
             -3805,   882,  -366,
             -3758,   819, -1513,
             -3145,   786, -2426,
             -1658,   546, -2853,
              -138,   300, -3000,
              1966,  -192, -2800,
              3066,  -456, -2800,
              3933,  -461, -2999,
              4266,  -188, -3433,
              3901,  -402, -3800,
              3133,  -579, -3866,
              2033,  -855, -3800,
               766, -1073, -3633,
             -1100, -1142, -3744,
             -2318, -1188, -3658,
             -3318, -1228, -3375,
             -4010, -1267, -2802,
             -4470, -1368, -2151,
             -4679, -1358, -1321,
             -4770, -1333,  -648,
             -4847, -1351,    40,
        };

        private static List<uint> _snowmanAddresses = new List<uint>()
        {
            0x803454C8,0x803448E8,0x80344688,0x80344428,0x803441C8,0x80343F68,
            0x80343AA8,0x80343848,0x8034BAE8,0x8034A0C8,0x8034D768,0x80349E68,
            0x8034A328,0x8034ACA8,0x8034DE88,0x8034BFA8,0x80349748,0x8034D9C8,
            0x8034E0E8,0x8034B628,0x803499A8,0x8034B168,0x8034D048,0x8034C928,
            0x8034A588,0x8034C468,0x8034B888,0x8034DC28,0x8034D2A8,0x8034BD48,
        };

        public static void UpdateSnowmanWaypoints()
        {
            uint bowlingBallAddress = 0x803467C8;
            uint waypointAddress = Config.Stream.GetUInt32(bowlingBallAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _snowmanAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetSnowmanWaypoints()
        {
            for (int i = 0; i < _snowmanAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_snowmanWaypoints[3 * i + 0], _snowmanAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_snowmanWaypoints[3 * i + 1], _snowmanAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_snowmanWaypoints[3 * i + 2], _snowmanAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<int> _bobUpperBowlingBallWaypoints = new List<int>()
        {
              1535,  3839, -5561,
              1514,  3804, -5886,
              1927,  3827, -6232,
              2717,  3715, -6740,
              3113,  3668, -6918,
              3503,  3638, -6783,
              4863,  3354, -5954,
              5081,  3221, -5754,
              5118,  3209, -5481,
              5147,  3185, -3712,
              5016,  3149, -3370,
              4609,  3137, -3118,
              3075,  2909, -2345,
              2784,  1634, -2237,
              1926,  1505, -1139,
               517,   773,  -438,
             -1275,   179,   -83,
             -2089,     5,   -24,
        };

        private static List<uint> _bobUpperBowlingBallAddresses = new List<uint>()
        {
            0x80345268,0x80345008,0x80344DA8,0x80344B48,0x803448E8,0x80344688,
            0x80344428,0x803441C8,0x80342548,0x80353668,0x80353B28,0x80342088,
            0x8034E348,0x80352F48,0x80353D88,0x80353408,0x803538C8,0x80354248,
        };

        public static void UpdateBobUpperWaypoints()
        {
            uint bowlingBallAddress = 0x80355A08;
            uint waypointAddress = Config.Stream.GetUInt32(bowlingBallAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _bobUpperBowlingBallAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetBobUpperWaypoints()
        {
            for (int i = 0; i < _bobUpperBowlingBallAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_bobUpperBowlingBallWaypoints[3 * i + 0], _bobUpperBowlingBallAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_bobUpperBowlingBallWaypoints[3 * i + 1], _bobUpperBowlingBallAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_bobUpperBowlingBallWaypoints[3 * i + 2], _bobUpperBowlingBallAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<int> _bobLowerBowlingBallWaypoints = new List<int>()
        {
               524,  2825, -5400,
               399,  2597, -5725,
               499,  2567, -5975,
               699,  2556, -6150,
               949,  2548, -6250,
              1549,  2525, -6600,
              2575,  2482, -7125,
              2975,  2466, -7425,
              3275,  2433, -7450,
              3800,  2337, -6950,
              4125,  2279, -6775,
              5310,  2119, -6500,
              5635,  2062, -6340,
              6010,  2004, -5730,
              5955,  1987, -5270,
              5540,  1947, -4330,
              5549,  1933, -4060,
              6014,  1906, -3198,
              5740,  1876, -2651,
              5273,  1840, -2467,
              3983,  1728, -2218,
              3640,  1682, -2072,
              3395,  1683, -1501,
              3211,  1676, -1190,
              2961,  1665,  -920,
               654,   640,  -758,
             -1618,     0,  -939,
        };

        private static List<uint> _bobLowerBowlingBallAddresses = new List<uint>()
        {
            0x80357DA8,0x80356128,0x80356388,0x80357688,0x803565E8,0x80354BC8,
            0x80344688,0x803448E8,0x80344B48,0x80344DA8,0x80345008,0x80345268,
            0x80358988,0x80352F48,0x803531A8,0x80353D88,0x803427A8,0x80353668,
            0x80341E28,0x8034F648,0x80353FE8,0x80354968,0x8035AAC8,0x8035A608,
            0x8035A3A8,0x8035A148,0x80359EE8,
        };

        public static void UpdateBobLowerWaypoints()
        {
            uint bowlingBallAddress = 0x80353B28;
            uint waypointAddress = Config.Stream.GetUInt32(bowlingBallAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _bobLowerBowlingBallAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetBobLowerWaypoints()
        {
            for (int i = 0; i < _bobLowerBowlingBallAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_bobLowerBowlingBallWaypoints[3 * i + 0], _bobLowerBowlingBallAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_bobLowerBowlingBallWaypoints[3 * i + 1], _bobLowerBowlingBallAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_bobLowerBowlingBallWaypoints[3 * i + 2], _bobLowerBowlingBallAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<int> _ttmWaypoints = new List<int>()
        {
             -1541,   514, -2125,
              -843,   410, -2302,
              -792,   357, -3132,
              -211,   189, -3153,
               474,   -31, -2635,
               944,  -136, -3098,
              1391,  -157, -3484,
              1999,  -378, -3257,
              2475,  -600, -2692,
              3445,  -952, -2115,
              3926,  -984,  -681,
              4281, -3326,   460,
        };

        private static List<uint> _ttmAddresses = new List<uint>()
        {
            0x80342EC8,0x80342C68,0x80342A08,0x803427A8,0x80342548,0x803422E8,
            0x80342088,0x8034E808,0x80353B28,0x803506E8,0x80350BA8,0x80354708,
        };

        public static void UpdateTtmTinyWaypoints()
        {
            uint bowlingBallAddress = 0x80356388;
            uint waypointAddress = Config.Stream.GetUInt32(bowlingBallAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _ttmAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetTtmWaypoints()
        {
            for (int i = 0; i < _ttmAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_ttmWaypoints[3 * i + 0], _ttmAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_ttmWaypoints[3 * i + 1], _ttmAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_ttmWaypoints[3 * i + 2], _ttmAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<short> _thiHugeBowlingBallWaypoints = new List<short>()
        {
            -4786,101,-2166,
            -5000,81,-2753,
            -5040,33,-3846,
            -4966,38,-4966,
            -4013,-259,-4893,
            -2573,-1019,-4780,
            -1053,-1399,-4806,
            760,-1637,-4833,
            2866,-2047,-4886,
            3386,-6546,-4833,
        };

        private static List<short> _thiTinyBowlingBallWaypoints = new List<short>()
        {
            -1476,29,-680,
            -1492,14,-1072,
            -1500,3,-1331,
            -1374,-17,-1527,
            -1178,-83,-1496,
            -292,-424,-1425,
            250,-491,-1433,
            862,-613,-1449,
            1058,-1960,-1449,
        };

        private static List<uint> _thiHugeAddresses = new List<uint>()
        {
            0x80346C88,0x8034F8A8,0x8034ECC8,0x8034E0E8,0x80342C68,0x803454C8,0x8034DC28,0x8034EF28,0x8034FB08,0x8034FD68
        };

        private static List<uint> _thiTinyAddresses = new List<uint>()
        {
            0x80342A08,0x8034F648,0x8033D948,0x803414A8,0x8034E808,0x80346A28,0x80347868,0x8034A7E8,0x80341BC8
        };

        public static void UpdateThiTinyWaypoints()
        {
            uint bowlingBallAddress = 0x80341E28;
            uint waypointAddress = Config.Stream.GetUInt32(bowlingBallAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _thiTinyAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 2 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetThiTinyWaypoints()
        {
            for (int i = 0; i < _thiTinyAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_thiTinyBowlingBallWaypoints[3 * i + 0], _thiTinyAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_thiTinyBowlingBallWaypoints[3 * i + 1], _thiTinyAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_thiTinyBowlingBallWaypoints[3 * i + 2], _thiTinyAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        public static void UpdateThiHugeWaypoints()
        {
            uint bowlingBallAddress = 0x8034D9C8;
            uint waypointAddress = Config.Stream.GetUInt32(bowlingBallAddress + 0x100);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _thiHugeAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static void SetThiHugeWaypoints()
        {
            for (int i = 0; i < _thiHugeAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_thiHugeBowlingBallWaypoints[3 * i + 0], _thiHugeAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_thiHugeBowlingBallWaypoints[3 * i + 1], _thiHugeAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_thiHugeBowlingBallWaypoints[3 * i + 2], _thiHugeAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static void ConvertHexList()
        {
            List<uint> output = ParsingUtilities.ParseHexList(Clipboard.GetText());
            List<short> output2 = output.ConvertAll(o => (short)o);
            InfoForm.ShowValue(string.Join(",", output2));
        }

        private static List<uint> kleptoAddresses = new List<uint>() { 0x80349E68, 0x80348DC8, 0x8034D2A8 };
        private static List<float> kleptoData = new List<float>()
        {
            2200.0f, 1250.0f, -2820.0f,
            -6200.0f, 1250.0f, -2800.0f,
            -6200.0f, 1250.0f, 1150.0f,
        };

        public static void UpdateKleptoWaypoints()
        {
            uint kleptoAddress = 0x803454C8;
            short destination = Config.Stream.GetInt16(kleptoAddress + 0x1AC);
            for (int i = 0; i < kleptoAddresses.Count; i++)
            {
                float scale = i == destination ? 4 : 1;
                uint address = kleptoAddresses[i];

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        public static void UpdateYoshiWaypoints()
        {
            uint yoshiAddress = 0x80344428;
            List<uint> redCoinAddresses = new List<uint>()
            {
                0x8034C208,0x8034D9C8,0x8034CB88,0x8034CDE8
            };

            float homeX = Config.Stream.GetSingle(yoshiAddress + ObjectConfig.HomeXOffset);
            float homeY = Config.Stream.GetSingle(yoshiAddress + ObjectConfig.HomeYOffset);
            float homeZ = Config.Stream.GetSingle(yoshiAddress + ObjectConfig.HomeZOffset);

            foreach (uint address in redCoinAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == homeX && redCoinY == homeY && redCoinZ == homeZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static List<int> _mipsData = new List<int>()
        {
             -1831, -1177, -1178,
             -1810, -1177,   284,
             -2210, -1192,   715,
             -3505, -1279,   715,
             -3968, -1279,   -31,
             -4021, -1381, -1242,
             -3674, -1379,  -962,
             -3813, -1279,   -41,
             -3628, -1279,   755,
             -2210, -1192,   715,
             -1810, -1177,   284,
             -1842, -1177, -1078,
             -1604, -1177, -1445,
             -1463, -1210, -2231,
             -1515, -1279, -3094,
             -2019, -1279, -3077,
             -2559, -1279, -3043,
             -2957, -1279, -2747,
             -3031, -1262, -1947,
             -2846, -1262, -1321,
             -3005, -1197, -1874,
             -2967, -1279, -2582,
             -2559, -1279, -3043,
             -1984, -1262, -3068,
             -1432, -1262, -3038,
             -1387, -1254, -2541,
             -1541, -1177, -1446,
              -894, -1223, -1421,
              -306, -1279, -1601,
              -192, -1279, -2196,
              -187, -1279, -1662,
              -805, -1238, -1406,
             -1549, -1177, -1446,
             -1092, -1279, -3188,
              -593, -1279, -3175,
              -200, -1279, -2940,
              -216, -1279, -2139,
              -214, -1279, -2432,
              -160, -1283, -2900,
              -640, -1283, -3220,
             -1469, -1279, -3108,
        };

        private static List<uint> _mipsAddresses = new List<uint>()
        {
            0x803408C8,0x803435E8,0x80346308,0x80344B48,0x80343AA8,0x8033FCE8,
            0x80345008,0x80340D88,0x803401A8,0x80340408,0x80340B28,0x8033FA88,
            0x8033FF48,0x80344DA8,0x80343848,0x80340668,0x80347608,0x80347AC8,
            0x80346EE8,0x80346A28,0x803473A8,0x803486A8,0x80346C88,0x80347148,
            0x80348448,0x80347F88,0x80345268,0x80348908,0x80347868,0x80348B68,
            0x803481E8,0x80347D28,0x80348DC8,0x80349028,0x80349288,0x803494E8,
            0x80349748,0x803499A8,0x80349C08,0x80349E68,0x8034A0C8,
        };

        public static void UpdateMipsWaypoints()
        {
            uint mipsAddress = 0x80341968;
            int action = Config.Stream.GetInt32(mipsAddress + 0x14C);
            uint waypointAddress = Config.Stream.GetUInt32(mipsAddress + ObjectConfig.WaypointOffset);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            Dictionary<uint, float> sizes = new Dictionary<uint, float>();

            bool anyAreEnlarged = false;
            foreach (uint address in _mipsAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                if (isCurrent) anyAreEnlarged = true;
                float scale = isCurrent ? 3 : 1;
                sizes[address] = scale;
            }

            if (!anyAreEnlarged && action != 0)
            {
                uint wpAddress = Config.Stream.GetUInt32(mipsAddress + 0xFC);
                short x = Config.Stream.GetInt16(wpAddress + 0x2);
                short y = Config.Stream.GetInt16(wpAddress + 0x4);
                short z = Config.Stream.GetInt16(wpAddress + 0x6);
                foreach (uint address in _mipsAddresses)
                {
                    float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                    float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                    float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                    if (redCoinX == x && redCoinY == y && redCoinZ == z)
                    {
                        sizes[address] = 3;
                    }
                }
            }

            Config.Stream.Suspend();
            foreach (uint address in sizes.Keys)
            {
                Config.Stream.SetValue(sizes[address], address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(sizes[address], address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(sizes[address], address + ObjectConfig.ScaleDepthOffset);
            }
            Config.Stream.Resume();
        }

        public static void SetMipsWaypoints()
        {
            for (int i = 0; i < _mipsAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_mipsData[3 * i + 0], _mipsAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_mipsData[3 * i + 1], _mipsAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_mipsData[3 * i + 2], _mipsAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<int> _racingPenguinData = new List<int>()
        {
            -4762,  6660, -6143,
            -4133,  6455, -6100,
            -2000,  6100, -5944,
            -1200,  6033, -5833,
             1022,  5611, -6033,
             3833,  5033, -6233,
             6055,  4598, -5766,
             6677,  4462, -4877,
             6277,  4417, -3344,
             4788,  4280, -1844,
             2211,  4086,  -555,
              522,  3687,  -222,
             -724,  3443,  -466,
            -1350,  3302, -1288,
            -1255,  3039, -3000,
            -2233,  2785, -4533,
            -3288,  2622, -4820,
            -4266,  2480, -4555,
            -4900,  2333, -3944,
            -5066,  2175, -2977,
            -4833,  2018, -1999,
            -4122,  1866, -1366,
            -3200,  1736, -1088,
             -222,  1027, -1200,
             1333,   761, -1733,
             2488,   562, -2944,
             2977,   361, -4988,
             3754,   329, -5689,
             5805,    86, -5980,
             6566,  -449, -4133,
             6689, -1119,  -888,
             6688, -2127,  1200,
             6666, -2573,  3555,
             6600, -2667,  4333,
             6366, -2832,  5722,
             5844, -3021,  6355,
             2955, -3394,  6255,
             1788, -3512,  5988,
              -89, -3720,  5188,
             -732, -3910,  4144,
             -722, -4095,  2688,
            -1333, -4198,  1255,
            -2377, -4302,   788,
            -4500, -4684,   277,
            -5466, -4790,    11,
            -6044, -4860,  -333,
            -6388, -5079, -1155,
            -6510, -5389, -2666,
            -6476, -5555, -3622,
            -6488, -5684, -4777,
            -6488, -5829, -6088,
            -6507, -5841, -6400,
        };

        private static List<uint> _racingPenguinAddresses2 = new List<uint>()
        {
            0x80343F68,0x803441C8,0x80340FE8,0x80341248,0x803414A8,0x80346308,
            0x803467C8,0x80346A28,0x80341968,0x80349288,0x803499A8,0x80341708,
            0x803494E8,0x8034A588,0x80349748,0x80349E68,0x8034A328,0x8034F8A8,
            0x8034FB08,0x8034FD68,0x8034FFC8,0x80350228,0x80350488,0x803506E8,
            0x80350948,0x80350BA8,0x80350E08,0x80345008,0x80351068,0x803512C8,
            0x80351528,0x80351788,0x803519E8,0x80351C48,0x80351EA8,0x80352108,
            0x80352368,0x803525C8,0x80352828,0x80352A88,0x80352CE8,0x80352F48,
            0x803531A8,0x80353408,0x80353668,0x803538C8,0x80353B28,0x80353D88,
            0x80353FE8,0x80354248,0x803544A8,0x80354708,0x80354968
        };

        public static void UpdateRacingPenguinWaypoints2()
        {
            uint objAddress = 0x80347868;
            uint waypointAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.WaypointOffset);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _racingPenguinAddresses2)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static List<uint> _racingPenguinAddresses = new List<uint>()
        {
            0x803506E8,0x80344428,0x80340B28,0x80340D88,0x803454C8,0x803494E8,
            0x8034B168,0x8034EA68,0x8034D768,0x8034A7E8,0x80340FE8,0x8034E5A8,
            0x80346C88,0x80345008,0x8034B888,0x80343AA8,0x80341708,0x80349E68,
            0x8034F8A8,0x80341968,0x8034ECC8,0x80349028,0x80349C08,0x80350BA8,
            0x8034D048,0x8034EF28,0x80346A28,0x8034AF08,0x803414A8,0x80346EE8,
            0x8034E348,0x80351EA8,0x803525C8,0x80341248,0x80352A88,0x80345BE8,
            0x80351788,0x8034E808,0x8034B628,0x803441C8,0x80343848,0x80343F68,
            0x80350E08,0x80352F48,0x803519E8,0x80351528,0x80353408,0x80350228,
            0x80353B28,0x8034FD68,0x80353668,0x80352CE8,
        };

        public static void UpdateRacingPenguinWaypoints()
        {
            uint objAddress = 0x80347868;
            uint waypointAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.WaypointOffset);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in _racingPenguinAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        public static void SetRacingPenguinWaypoints()
        {
            for (int i = 0; i < _racingPenguinAddresses.Count; i++)
            {
                Config.Stream.SetValue((float)_racingPenguinData[3 * i + 0], _racingPenguinAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)_racingPenguinData[3 * i + 1], _racingPenguinAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)_racingPenguinData[3 * i + 2], _racingPenguinAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        private static List<uint> ktq2WaypointAddresses = new List<uint>()
        {
            0x8034ECC8,0x80347AC8,0x8034E0E8,0x80346568,0x8034FB08,0x8034ACA8,
            0x8034FD68,0x8034A7E8,0x80349288,0x80344688,0x80347148,0x8034DE88,
        };

        public static void SetKtq2Waypoints()
        {
            for (int i = 0; i <= TableConfig.KoopaTheQuick2Waypoints.GetMaxIndex(); i++)
            {
                Config.Stream.SetValue((float)TableConfig.KoopaTheQuick2Waypoints.GetWaypoint(i).x, ktq2WaypointAddresses[i] + ObjectConfig.XOffset);
                Config.Stream.SetValue((float)TableConfig.KoopaTheQuick2Waypoints.GetWaypoint(i).y, ktq2WaypointAddresses[i] + ObjectConfig.YOffset);
                Config.Stream.SetValue((float)TableConfig.KoopaTheQuick2Waypoints.GetWaypoint(i).z, ktq2WaypointAddresses[i] + ObjectConfig.ZOffset);
            }
        }

        public static void Update4Ktq2Waypoints()
        {
            uint ktqAddress = 0x803460A8;
            uint waypointAddress = Config.Stream.GetUInt32(ktqAddress + ObjectConfig.WaypointOffset);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in ktq2WaypointAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        private static List<uint> ktq1WaypointAddresses = new List<uint>()
        {
            0x803441C8,0x80344428,0x80344688,0x803448E8,0x80344B48,0x80344DA8,
            0x80345008,0x80345268,0x80341E28,0x803538C8,0x80353408,0x803544A8,
            0x80342548,0x803422E8,0x80353FE8,0x8034E5A8,0x80354708,0x80353B28,
            0x803427A8,0x80342088,0x80353D88,0x8034F8A8,0x80354248,0x80353668,
            0x80354968,0x80354BC8,0x80354E28,0x80355088,0x803552E8,0x80355548,
            0x803557A8,0x80355A08,0x80355C68,0x80355EC8,0x80356128,0x80356388,
        };

        public static void Update3Ktq1Waypoints()
        {
            uint ktqAddress = 0x8034E0E8;
            uint waypointAddress = Config.Stream.GetUInt32(ktqAddress + ObjectConfig.WaypointOffset);
            short waypointX = Config.Stream.GetInt16(waypointAddress + 0xA);
            short waypointY = Config.Stream.GetInt16(waypointAddress + 0xC);
            short waypointZ = Config.Stream.GetInt16(waypointAddress + 0xE);

            foreach (uint address in ktq1WaypointAddresses)
            {
                float redCoinX = Config.Stream.GetSingle(address + ObjectConfig.XOffset);
                float redCoinY = Config.Stream.GetSingle(address + ObjectConfig.YOffset);
                float redCoinZ = Config.Stream.GetSingle(address + ObjectConfig.ZOffset);
                bool isCurrent = redCoinX == waypointX && redCoinY == waypointY && redCoinZ == waypointZ;
                float scale = isCurrent ? 4 : 1;

                Config.Stream.Suspend();
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleWidthOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleHeightOffset);
                Config.Stream.SetValue(scale, address + ObjectConfig.ScaleDepthOffset);
                Config.Stream.Resume();
            }
        }

        public static void TestSomething29()
        {
            float x1 = -2105.274658203125f;
            float z1 = 856.53839111328125f;
            float x2 = -2105.279541015625f;
            float z2 = 856.53619384765625f;

            float y = -1916f;

            TriangleDataModel wallTri1 = new TriangleDataModel(0x801A8FC0);
            TriangleDataModel wallTri2 = new TriangleDataModel(0x801A8FF0);
            List<TriangleDataModel> wallTris = new List<TriangleDataModel>() { wallTri1, wallTri2 };

            float xMin = Math.Min(x1, x2);
            float zMin = Math.Min(z1, z2);
            float xMax = Math.Max(x1, x2);
            float zMax = Math.Max(z1, z2);

            HashSet<(float x, float z)> points = new HashSet<(float x, float z)>();
            HashSet<(float x, float z)> disps = new HashSet<(float x, float z)>();
            for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
            {
                for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
                {
                    points.Add((x, z));
                    (float dispX, float dispZ) = WallDisplacementCalculator.HandleWallDisplacement(x, y, z, wallTris, 50, 0);
                    if (!disps.Contains((dispX, dispZ)))
                    {
                        disps.Add((dispX, dispZ));
                    }
                }
            }
            Config.Print("POINTS");
            foreach ((float x, float z) in points)
            {
                Config.Print("{0},{1}", (double)x, (double)z);
            }
            Config.Print("DISPS");
            foreach ((float x, float z) in disps)
            {
                Config.Print("{0},{1}", (double)x, (double)z);
            }
        }

        public static void Update2()
        {
            uint marioObj = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
            int animationTimerValue = Config.Stream.GetInt16(marioObj + MarioObjectConfig.AnimationTimerOffset);
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            (float holpX, float holpY, float holpZ) = HolpCalculator.GetHolpForStanding(animationTimerValue, marioX, marioY, marioZ, marioAngle);
            SpecialConfig.CustomX = holpX;
            SpecialConfig.CustomY = holpY;
            SpecialConfig.CustomZ = holpZ;
        }

        public static void TestSomething28()
        {
            (float x, float y, float z) = (-89.9566192626953f, 2253f, 7003f);
            uint wallAddress1 = 0x801A6110;
            uint wallAddress2 = 0x801A60E0;
            TriangleDataModel tri1 = new TriangleDataModel(wallAddress1);
            TriangleDataModel tri2 = new TriangleDataModel(wallAddress2);
            List<TriangleDataModel> tris = new List<TriangleDataModel>() { tri1, tri2 };
            int numCollisions = WallDisplacementCalculator.GetNumWallCollisions(x, y, z, tris, 50, 60);
            Config.SetDebugText("numCollisions = " + numCollisions);
        }

        public static void TestSomething27()
        {
            uint wallAddress1 = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            uint wallAddress2 = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
            TriangleDataModel tri1 = new TriangleDataModel(wallAddress1);
            TriangleDataModel tri2 = new TriangleDataModel(wallAddress2);
            List<TriangleDataModel> tris = new List<TriangleDataModel>() { tri1, tri2 };

            bool containsVertex(TriangleDataModel tri, int x0, int y0, int z0)
            {
                if (tri.X1 == x0 && tri.Y1 == y0 && tri.Z1 == z0) return true;
                if (tri.X2 == x0 && tri.Y2 == y0 && tri.Z2 == z0) return true;
                if (tri.X3 == x0 && tri.Y3 == y0 && tri.Z3 == z0) return true;
                return false;
            }

            List<(int x, int y, int z)> vertices = new List<(int x, int y, int z)>();
            if (containsVertex(tri2, tri1.X1, tri1.Y1, tri1.Z1)) vertices.Add((tri1.X1, tri1.Y1, tri1.Z1));
            if (containsVertex(tri2, tri1.X2, tri1.Y2, tri1.Z2)) vertices.Add((tri1.X2, tri1.Y2, tri1.Z2));
            if (containsVertex(tri2, tri1.X3, tri1.Y3, tri1.Z3)) vertices.Add((tri1.X3, tri1.Y3, tri1.Z3));
            if (vertices.Count < 2) return;

            (int x1, int y1, int z1) = vertices[0];
            (int x2, int y2, int z2) = vertices[1];

            int sign;
            int start;
            int end;
            bool wIsX;
            if (x1 != x2)
            {
                wIsX = true;
                start = x1;
                end = x2;
                sign = Math.Sign(x2 - x1);
            }
            else if (z1 != z2)
            {
                wIsX = false;
                start = z1;
                end = z2;
                sign = Math.Sign(z2 - z1);
            }
            else
            {
                return;
            }

            List<(double x, double y, double z)> zeroCollisionPoints = new List<(double x, double y, double z)>();

            int[] numCollisions = new int[3];
            for (float w = start; sign > 0 ? w <= end : w >= end; w += sign * Math.Max(MoreMath.GetFloatInterval(w), 0.00001f))
            {
                float proportion = (w - start) / (end - start);
                float x = wIsX ? w : x1 + proportion * (x2 - x1);
                float y = y1 + proportion * (y2 - y1) - 150;
                float z = !wIsX ? w : z1 + proportion * (z2 - z1);
                int count = WallDisplacementCalculator.GetNumWallCollisions(x, y, z, tris, 50, 150);
                numCollisions[count]++;
                if (count == 0)
                {
                    zeroCollisionPoints.Add((x, y, z));
                }
            }

            InfoForm.ShowValue(string.Join("\r\n", zeroCollisionPoints));

            Config.SetDebugText(string.Format("[0]={0}, [1]={1}, [2]={2}", numCollisions[0], numCollisions[1], numCollisions[2]));
        }

        public static void TestSomething26()
        {
            InGameFunctionCall.WriteInGameFunctionCall(0x8024975C, 9);
        }

        public static void TestSomething25()
        {
            List<double> doubleList = new List<double>()
            {
                2,
                5.94287109375,
                13.38134765625,
                26.20458984375,
                44.599609375,
                62.783203125,
                71.255859375,
                73.255859375,
                75.255859375,
                77.255859375,
                79.255859375,
            };

            int startX = 6765;
            int startZ = 4152;

            foreach (double dist in doubleList)
            {
                Dictionary<(int, int), int> dictionary = new Dictionary<(int, int), int>();

                for (int angle = 0; angle <= 16384; angle++)
                {
                    (double x, double z) = MoreMath.AddVectorToPoint(dist, angle, startX, startZ);
                    int xInt = (int)x;
                    int zInt = (int)z;

                    if (!dictionary.ContainsKey((xInt, zInt)))
                    {
                        dictionary[(xInt, zInt)] = 0;
                    }
                    dictionary[(xInt, zInt)]++;
                }

                foreach (KeyValuePair<(int, int), int> pairs in dictionary.ToList())
                {
                    Config.Print("{0}: ({1},{2}) => {3}", dist, pairs.Key.Item1, pairs.Key.Item2, pairs.Value);
                }
            }
        }

        public static void TestSomething24()
        {
            int range = 100;
            int range2 = 10000;

            List<int> goodAngles = new List<int>() { 27408, 27424, 27440, 27456, 27472 };

            for (int i = 0; i < range; i++)
            {
                int initialIndex = 305 + i;
                int initialAngle = TableConfig.PendulumSwings.GetPendulumAmplitude(initialIndex);
                TtcPendulum2 pendulum = new TtcPendulum2(new TtcRng(0), -1 * Math.Sign(initialAngle), initialAngle, 0, 13, 0);
                for (int j = 0; j < range2; j++)
                {
                    pendulum.Update();
                    int angle = pendulum._angle;
                    int angleTruncated = MoreMath.NormalizeAngleTruncated(angle);
                    string index = pendulum.GetSwingIndexExtended();
                    if (goodAngles.Contains(angleTruncated))
                    {
                        Config.Print(
                            "i={0}, j={1}, initialIndex={2}, initialAngle={3}, angle={4}, angleTruncated={5}, index={6}",
                            i, j, initialIndex, initialAngle, angle, angleTruncated, index);
                    }
                }
            }
        }

        public static void TestSomething23()
        {
            List<string> outputList = TtcMain.FindDualPendulumManipulation(50).ToList().ConvertAll(pair =>
            {
                return pair.Key + ":\t[" + string.Join(",", pair.Value) + "]";
            });
            string output = String.Join("\r\n", outputList);
            InfoForm.ShowValue(output);
        }

        public static void TestSomething22()
        {
            TtcMain.FindIdealCogManipulation();
        }

        public static void TestSomething21()
        {
            TtcSimulation simulation = new TtcSimulation(new TtcSaveState());
            Clipboard.SetText(simulation.ToString());
        }

        public static void TestSomething20()
        {
            TtcSimulation simulation = new TtcSimulation(new TtcSaveState());
            for (int i = 0; i <= 100; i++)
            {
                Config.Print("{0}\t{1}\t{2}", i, simulation.GetSaveStateString(), simulation);
                simulation.SimulateNumFrames(1);
            }
        }

        public static void TestSomething19()
        {
            List<uint> addresses = TtcUtilities.GetObjectAddresses();
            for (int i = 0; i < addresses.Count; i++)
            {
                Config.Print(i + "\t" + HexUtilities.FormatValue(addresses[i]));
            }
        }

        public static void Update1()
        {
            uint marioObj = Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
            short animation = Config.Stream.GetInt16(marioObj + 0x40);
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

            (float x, float y, float z) = HolpCalculator.GetHolpForWalking(animation, marioX, marioY, marioZ, marioAngle);
            SpecialConfig.CustomX = x;
            SpecialConfig.CustomY = y;
            SpecialConfig.CustomZ = z;
        }

        public static void TestSomething18()
        {
            List<int> initialAngles = new List<int>() { 22976, 22592, 22512 };

            int range = 10;
            List<int> extendedAngles = new List<int>();
            for (int i = -1 * range; i <= range; i++)
            {
                if (i == 0) continue;
                foreach (int initialAngle in initialAngles)
                {
                    int angle = i * 65536 + initialAngle;
                    extendedAngles.Add(angle);
                }
            }

            List<int> allAngles = new List<int>();
            foreach (int extendedAngle in extendedAngles)
            {
                for (int i = 0; i < 16; i++)
                {
                    int angle = extendedAngle + i;
                    allAngles.Add(angle);
                }
            }

            string output = "";
            foreach (int angle in allAngles)
            {
                string index = TableConfig.PendulumSwings.GetPendulumSwingIndexExtended(angle);
                string index1, index2;
                int hyphenIndex = index.LastIndexOf('-');
                if (hyphenIndex == -1)
                {
                    index1 = "0";
                    index2 = "0";
                }
                else
                {
                    index1 = index.Substring(0, hyphenIndex);
                    index2 = index.Substring(hyphenIndex + 1);
                }
                output += angle + "\t" + index + "\t" + index1 + "\t" + index2 + "\r\n";
            }
            InfoForm.ShowValue(output);
        }

        public static void TestSomething17()
        {
            List<int> initialAngles = new List<int>()
            {
                88527,
                88052,
                88055,
                153595,
                153599,
                -174096,
                -108082,
                -42929,
                219202,
                -43016,
                154049,
                284746,
                -42940,
                284670,
                88519,
                -567228,
                -174005,
                -174014,
                -42555,
                88516,
                153675,
                -42545,
                547279,
                -42942,
                88141,
                -173627,
                88143,
                350284,
                547274,
                219121,
                677959,
                481349,
                -108476,
                -42939,
                153585,
                -43017,
                -305162,
                -108558,
                219125,
            };
            List<int> initialAccMags = new List<int>() { 13, 42 };

            string output = "";
            foreach (int initialAngle in initialAngles)
            {
                foreach (int initialAccMag in initialAccMags)
                {
                    int initialAccDir = initialAngle > 0 ? -1 : 1;
                    TtcPendulum2 pendulum = new TtcPendulum2(new TtcRng(0), initialAccDir, initialAngle, 0, initialAccMag, 0);
                    int startTimer = 35192;
                    for (int i = 0; i < 2000; i++)
                    {
                        int timer = startTimer + i;
                        int accelerationDirection = pendulum._accelerationDirection;
                        int accelerationMagnitude = pendulum._accelerationMagnitude;
                        int angularVelocity = pendulum._angularVelocity;
                        int angle = pendulum._angle;
                        int amplitude = (int)WatchVariableSpecialUtilities.GetPendulumAmplitude(
                            accelerationDirection, accelerationMagnitude, angularVelocity, angle);
                        string index = TableConfig.PendulumSwings.GetPendulumSwingIndexExtended(amplitude);

                        bool success = MoreMath.NormalizeAngleTruncated(angle) == 30416;
                        if (success)
                        {
                            output += String.Format(
                                "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\r\n",
                                i,
                                timer,
                                initialAngle,
                                accelerationDirection,
                                accelerationMagnitude,
                                angularVelocity,
                                angle,
                                amplitude,
                                index);
                        }

                        pendulum.Update();
                    }
                }
            }
            InfoForm.ShowValue(output);
        }

        public static void TestSomething16()
        {
            int angle1 = 21163;
            int angle2 = 25973;

            TtcPendulum pendulum = new TtcPendulum(new TtcRng2(), 1, -56745, 0, 42, 0);
            int startTimer = 35192;
            for (int i = 0; i < 20000; i++)
            {
                int timer = startTimer + i;
                int accelerationDirection = pendulum._accelerationDirection;
                int accelerationMagnitude = pendulum._accelerationMagnitude;
                int angularVelocity = pendulum._angularVelocity;
                int angle = pendulum._angle;
                int amplitude = (int)WatchVariableSpecialUtilities.GetPendulumAmplitude(
                    accelerationDirection, accelerationMagnitude, angularVelocity, angle);
                string index = TableConfig.PendulumSwings.GetPendulumSwingIndexExtended(amplitude);

                string success = "";
                int angleUshort = (int)MoreMath.NormalizeAngleDouble(angle);
                if (angleUshort > angle1 && angleUshort < angle2 &&
                    TableConfig.PendulumVertexes.HasVertexWithY(angle, -2434))
                {
                    success = "*******************************";
                }

                Config.Print(
                    "[{0}] [{1}]: {2}, {3}, {4}, {5} | {6}, {7} {8}",
                    i,
                    timer,
                    accelerationDirection,
                    accelerationMagnitude,
                    angularVelocity,
                    angle,
                    amplitude,
                    index,
                    success);

                pendulum.Update();
            }
        }

        public static void TestSomething15()
        {
            string clipboard = Clipboard.GetText();
            List<string> lines = clipboard.Split('\n').ToList();
            List<string> output = new List<string>();
            foreach (string line in lines)
            {
                List<string> parts = ParsingUtilities.ParseStringList(line);
                if (parts.Count == 0) continue;

                if (parts.Contains("SOUND_ARG_LOAD"))
                {
                    int index = parts.IndexOf("SOUND_ARG_LOAD");
                    string part1 = parts[index + 1];
                    string part2 = parts[index + 2];
                    string part3 = parts[index + 3];
                    string value = "0x" + part1 + part2 + part3.Substring(2);
                    output.Add(value + ",");
                }
                else
                {
                    string value = parts[parts.Count - 1];
                    output.Add(value + ",");
                }
            }
            InfoForm.ShowValue(string.Join("\r\n", output));
        }

        public static void TestSomething14()
        {
            /*
            uint setSound = RomVersionConfig.Switch(0x8031EB00, 0x8031DC78);
            uint soundArg = RomVersionConfig.Switch(0x803331F0, 0x803320E0); // = default stereo panning?
            uint starCollectSound = 0x701EFF81; // or any other
            InGameFunctionCall.WriteInGameFunctionCall(setSound, starCollectSound, soundArg);
            */

            /*
            uint fileSelectMusic = 31; // or any other
            uint setMusic = RomVersionConfig.Switch(0x80320544, 0x8031F690);
            InGameFunctionCall.WriteInGameFunctionCall(setMusic, 0, fileSelectMusic, 0);
            */

            uint spawnStar = RomVersionConfig.SwitchMap(0x802AB558, 0x802AACE4);
            InGameFunctionCall.WriteInGameFunctionCall(spawnStar, 6);
        }

        public static void TestSomething13()
        {
            uint absoluteAddress = (uint)SpecialConfig.CustomX;
            uint relativeAddress = TypeUtilities.GetRelativeAddressFromAbsoluteAddress(absoluteAddress, 4);
            InfoForm.ShowValue(HexUtilities.FormatValue(relativeAddress));
        }

        public static void TestSomething12()
        {
            uint triAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            if (triAddress == 0) return;
            TriangleDataModel triDataModel = new TriangleDataModel(triAddress);
            TriangleShape triShape =
                new TriangleShape(
                    triDataModel.X1, triDataModel.Y1, triDataModel.Z1,
                    triDataModel.X2, triDataModel.Y2, triDataModel.Z2,
                    triDataModel.X3, triDataModel.Y3, triDataModel.Z3);
            /*
            InfoForm.ShowValue(
                String.Format(
                    "NormX:{0}, NormY:{1}, NormZ:{2}, NormOffset:{3}",
                    triShape.NormX, triShape.NormY, triShape.NormZ, triShape.NormOffset));
            */

            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            InfoForm.ShowValue(triShape.GetY(marioX, marioZ));
        }

        public static void TestSomething10()
        {
            HashSet<int> seenAmplitudes = new HashSet<int>();
            Queue<PendulumSwing> queue = new Queue<PendulumSwing>();

            int startingAmplitude = -43470; // index 315
            PendulumSwing startingPendulumSwing = new PendulumSwing(startingAmplitude, 0, null);
            queue.Enqueue(startingPendulumSwing);
            seenAmplitudes.Add(startingPendulumSwing.Amplitude);

            while (queue.Count > 0)
            {
                PendulumSwing dequeue = queue.Dequeue();
                List<PendulumSwing> successors = dequeue.GetSuccessors();
                foreach (PendulumSwing pendulumSwing in successors)
                {
                    if (pendulumSwing.Amplitude == -57330)
                    {
                        InfoForm.ShowValue(pendulumSwing);
                        return;
                    }
                    if (seenAmplitudes.Contains(pendulumSwing.Amplitude)) continue;
                    queue.Enqueue(pendulumSwing);
                    seenAmplitudes.Add(pendulumSwing.Amplitude);
                }
            }
        }

        public class PendulumSwing
        {
            public readonly int Amplitude;
            public readonly int Acceleration;
            public readonly PendulumSwing Predecessor;

            public PendulumSwing(int amplitude, int acceleration, PendulumSwing predecessor)
            {
                Amplitude = amplitude;
                Acceleration = acceleration;
                Predecessor = predecessor;
            }

            public List<PendulumSwing> GetSuccessors()
            {
                return new List<PendulumSwing>()
                {
                    new PendulumSwing((int)WatchVariableSpecialUtilities.GetPendulumAmplitude(Amplitude, 13), 13, this),
                    new PendulumSwing((int)WatchVariableSpecialUtilities.GetPendulumAmplitude(Amplitude, 42), 42, this),
                };
            }

            public override string ToString()
            {
                string predecessorString = Predecessor?.ToString() ?? "";
                return predecessorString + " =>" + Acceleration + "=> " + Amplitude;
            }
        }

        public static void AddGraphicsTriangleVerticesToTriangleTab()
        {
            int numVertices = 3;
            if (KeyboardUtilities.IsCtrlHeld()) numVertices = 4;
            if (KeyboardUtilities.IsNumberHeld()) numVertices = KeyboardUtilities.GetCurrentlyInputtedNumber().Value;

            uint triangleAddress = Config.TriangleManager.TriangleAddress;
            if (triangleAddress == 0) return;
            TriangleDataModel triangle = new TriangleDataModel(triangleAddress);
            List<List<short>> triangleVertices = new List<List<short>>()
            {
                new List<short>() { triangle.X1, triangle.Y1, triangle.Z1 },
                new List<short>() { triangle.X2, triangle.Y2, triangle.Z2 },
                new List<short>() { triangle.X3, triangle.Y3, triangle.Z3 },
            };

            int structSize = numVertices * 0x10;
            uint ramStart = 0x80000000;

            for (uint baseAddress = ramStart; baseAddress < ramStart + Config.RamSize - structSize; baseAddress += 2)
            {
                List<uint> addresses = new List<uint>();
                List<string> names = new List<string>();
                List<List<short>> vertices = new List<List<short>>();

                for (int i = 0; i < numVertices; i++)
                {
                    List<short> vertex = new List<short>();
                    for (int j = 0; j < 3; j++)
                    {
                        uint offset = (uint)(i * 0x10 + j * 0x02);
                        uint address = baseAddress + offset;
                        short value = Config.Stream.GetInt16(address);
                        string component = j == 0 ? "x" : j == 1 ? "y" : "z";
                        string name = "v" + (i + 1) + component;

                        addresses.Add(address);
                        names.Add(name);
                        vertex.Add(value);
                    }
                    vertices.Add(vertex);
                }

                List<List<List<short>>> vertexSubsets = ControlUtilities.GetSubsets(vertices, 3);
                foreach (List<List<short>> vertexSubset in vertexSubsets)
                {
                    if (AreVerticesEqual(triangleVertices, vertexSubset))
                    {
                        List<WatchVariableControlPrecursor> precursors = new List<WatchVariableControlPrecursor>();
                        for (int i = 0; i < addresses.Count; i++)
                        {
                            WatchVariable watchVar = new WatchVariable(
                                memoryTypeName: "short",
                                specialType: null,
                                baseAddressType: BaseAddressTypeEnum.Relative,
                                offsetUS: null,
                                offsetJP: null,
                                offsetSH: null,
                                offsetEU: null,
                                offsetDefault: addresses[i],
                                mask: null,
                                shift: null,
                                handleMapping: true);
                            WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                                name: names[i],
                                watchVar: watchVar,
                                subclass: WatchVariableSubclass.Number,
                                backgroundColor: null,
                                displayType: null,
                                roundingLimit: null,
                                useHex: null,
                                invertBool: null,
                                isYaw: null,
                                coordinate: null,
                                groupList: new List<VariableGroup>() { VariableGroup.Custom });
                            precursors.Add(precursor);
                        }

                        Config.TriangleManager.AddVariables(
                            precursors.ConvertAll(precursor => precursor.CreateWatchVariableControl()));
                    }
                }
            }
        }
        
        public static bool AreVerticesEqual(List<List<short>> vertices1, List<List<short>> vertices2)
        {
            List<short> v1_0 = vertices1[0];
            List<short> v1_1 = vertices1[1];
            List<short> v1_2 = vertices1[2];
            List<short> v2_0 = vertices2[0];
            List<short> v2_1 = vertices2[1];
            List<short> v2_2 = vertices2[2];
            return
                Enumerable.SequenceEqual(v1_0, v2_0) && Enumerable.SequenceEqual(v1_1, v2_1) && Enumerable.SequenceEqual(v1_2, v2_2) ||
                Enumerable.SequenceEqual(v1_0, v2_0) && Enumerable.SequenceEqual(v1_1, v2_2) && Enumerable.SequenceEqual(v1_2, v2_1) ||
                Enumerable.SequenceEqual(v1_0, v2_1) && Enumerable.SequenceEqual(v1_1, v2_0) && Enumerable.SequenceEqual(v1_2, v2_2) ||
                Enumerable.SequenceEqual(v1_0, v2_1) && Enumerable.SequenceEqual(v1_1, v2_2) && Enumerable.SequenceEqual(v1_2, v2_0) ||
                Enumerable.SequenceEqual(v1_0, v2_2) && Enumerable.SequenceEqual(v1_1, v2_0) && Enumerable.SequenceEqual(v1_2, v2_1) ||
                Enumerable.SequenceEqual(v1_0, v2_2) && Enumerable.SequenceEqual(v1_1, v2_1) && Enumerable.SequenceEqual(v1_2, v2_0);
        }

        public static void TestSomething6()
        {
            List<List<int>> dustFrameLists = TtcMain.FindIdealPendulumManipulation(0x8033E788, 5);
            List<string> outputList = dustFrameLists.ConvertAll(dustFrameList => "[" + String.Join(", ", dustFrameList) + "]");
            string output = String.Join("\r\n", outputList);
            Config.Print(output);
        }

        public static void TestSomething7()
        {
            MoveBoxes(false);
        }

        public static void MoveBoxes(bool upwards)
        {
            List<double> marioPositions = upwards ? marioPositions1 : marioPositions2;
            int yDiff = upwards ? 78 : -100;

            List<double> qSteps = new List<double>();
            for (int i = 0; i < marioPositions.Count - 1; i++)
            {
                double x1 = marioPositions[i];
                double x2 = marioPositions[i + 1];
                double diff = x2 - x1;
                qSteps.Add(x1 + diff * 1 / 4.0);
                qSteps.Add(x1 + diff * 2 / 4.0);
                qSteps.Add(x1 + diff * 3 / 4.0);
                qSteps.Add(x1 + diff * 4 / 4.0);
            }
            List<int> qStepsTruncated = qSteps.ConvertAll(qStep => (int)Math.Truncate(qStep));
            List<ObjectDataModel> objects = Config.ObjectSlotsManager.SelectedObjects;
            if (objects.Count == 0) return;

            objects.Sort((obj1, obj2) =>
            {
                string label1 = Config.ObjectSlotsManager.GetSlotLabelFromObject(obj1);
                string label2 = Config.ObjectSlotsManager.GetSlotLabelFromObject(obj2);
                int pos1 = ParsingUtilities.ParseInt(label1);
                int pos2 = ParsingUtilities.ParseInt(label2);
                return pos1 - pos2;
            });

            int initialX = (int)objects[0].X;
            int initialY = (int)objects[0].Y;
            int initialZ = (int)objects[0].Z;
            int xOffset = initialX - qStepsTruncated[0];
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].X = qStepsTruncated[i] + xOffset;
                objects[i].Y = initialY + i * yDiff;
                objects[i].Z = initialZ;
            }
        }

        public static void TestSomething4()
        {
            TtcMain.TtcMainMethod();
        }

        public static void TestSomething2()
        {
            List<string> output = new List<string>();
            for (int i = 0; i < 65536; i++)
            {
                float trig2 = InGameTrigUtilities.InGameCosine(i);
            }
            InfoForm.ShowValue(String.Join("\r\n", output));
        }

        public static List<double> marioPositions1 = new List<double>()
        {
            -5625.607422,-5598.169922,-5570.271484,-5541.921875,-5513.130859,-5483.910156,-5454.267578,
            -5424.214844,-5393.761719,-5362.916016,-5331.6875,-5300.085938,-5268.119141,-5235.794922,
            -5204.470703,-5172.775391,-5140.716797,-5109.658203,-5078.222656,-5046.417969,-5014.251953,
            -4983.085938,-4951.544922,-4919.638672,-4887.373047,-4856.107422,-4824.46875,-4792.466797,
            -4761.464844,-4730.083984,-4698.332031,-4666.21875,-4635.105469,-4603.617188,-4571.759766,
            -4539.542969,-4508.326172,-4476.736328,-4444.78125,-4412.46875,-4381.15625,-4349.472656,
            -4317.425781,-4286.378906,-4254.953125,-4223.158203,-4191.003906,-4159.849609,-4128.318359,
            -4096.421875,-4064.166016,-4032.910156,-4001.28125,-3969.288086,-3936.939453,-3905.59082,
            -3873.871094,-3841.789063,-3810.707031,-3779.248047,-3747.419922,-3715.232422,-3684.044922,
            -3652.482422,-3620.553711,-3588.267578,-3556.981445,-3525.323242,-3493.300781,-3462.27832,
            -3430.87793,-3399.107422,-3366.975586,-3335.84375,-3304.335938,-3272.460938,-3240.227539,
            -3208.994141,-3177.386719,-3145.415039,-3113.086914,-3081.758789,-3050.058594,-3017.996094,
            -2986.933594,-2955.493164,-2923.683594,-2891.513672,-2860.34375,-2828.798828,-2796.887695,
            -2764.619141,-2733.350586,-2701.708984,-2669.703125,-2638.697266,-2638.697266,
        };

        public static List<double> marioPositions2 = new List<double>()
        {
            -5575.169922,-5547.271484,-5518.921875,-5490.130859,-5460.910156,-5431.267578,
            -5401.214844,-5370.761719,-5339.916016,-5308.6875,-5277.085938,-5245.119141,-5212.794922,
            -5181.470703,-5149.775391,-5117.716797,-5086.658203,-5055.222656,-5023.417969,-4991.251953,
            -4960.085938,-4928.544922,-4896.638672,-4864.373047,-4833.107422,-4801.46875,-4769.466797,
            -4738.464844,-4707.083984,-4675.332031,-4643.21875,-4612.105469,-4580.617188,-4548.759766,
            -4516.542969,-4485.326172,-4453.736328,-4421.78125,-4389.46875,-4358.15625,-4326.472656,
            -4294.425781,-4263.378906,-4231.953125,-4200.158203,-4168.003906,-4136.849609,-4105.318359,
            -4073.421143,-4041.165283,-4009.909424,-3978.280518,-3946.287354,-3913.938721,-3882.590088,
            -3850.870361,-3818.78833,-3787.706299,-3756.247314,-3724.419189,-3692.231689,-3661.044189,
            -3629.481689,-3597.552979,-3565.266846,-3533.980713,-3502.32251,-3470.300049,-3439.277588,
        };

        public static void TestScuttlebugDrops()
        {
            List<int> startingVerticalSpeeds = new List<int>();
            for (int i = 16; i >= -76; i -= 4)
            {
                startingVerticalSpeeds.Add(i);
            }
            startingVerticalSpeeds.Add(-78);

            List<List<int>> diffListList = new List<List<int>>();
            foreach (int startingVerticalSpeed in startingVerticalSpeeds)
            {
                int vs1 = startingVerticalSpeed;
                int vs2 = 16;
                int y1 = 0;
                int y2 = 0;

                List<int> diffList = new List<int>();
                for (int i = 0; i < 25; i++)
                {
                    int diff = y2 - y1;
                    diffList.Add(diff);
                    y1 += vs1;
                    y2 += vs2;
                    vs1 = Math.Max(vs1 - 4, -78);
                    vs2 = Math.Max(vs2 - 4, -78);
                }
                diffListList.Add(diffList);
            }

            string output = "";
            foreach (List<int> diffList in diffListList)
            {
                foreach (int diff in diffList)
                {
                    output += diff;
                    output += "\t";
                }
                output += "\n";
            }
            InfoForm.ShowValue(output);
        }

        public static void TestSomething11()
        {
            /*
            double normX = -20;
            double normY = -58;
            double normZ = 50;
            double normOffset = 750;
            */

            /*
            double normX = 0;
            double normY = -50;
            double normZ = 40;
            double normOffset = 440;
            */

            double normX = 0;
            double normY = -25;
            double normZ = 100;
            double normOffset = 600;

            Func<double, double, double> getY =
                (double x, double z) => TriangleDataModel.GetHeightOnTriangle(x, z, normX, normY, normZ, normOffset);

            List<int[]> unitCoordinates = new List<int[]>();
            for (int x = 1; x <= 6; x++)
            {
                for (int y = 1; y <= 6; y++)
                {
                    unitCoordinates.Add(new int[] { x, y });
                }
            }

            foreach (int[] coord in unitCoordinates)
            {
                int x = coord[0];
                int z = coord[1] * -1;

                int x1 = x;
                int z1 = z;
                int x2 = x;
                int z2 = z - 1;
                int x3 = x + 1;
                int z3 = z - 1;
                int x4 = x + 1;
                int z4 = z;

                double y1 = getY(x1, z1);
                double y2 = getY(x1, z1);
                double y3 = getY(x1, z1);
                double y4 = getY(x1, z1);

                Config.Print("{0},{1}[1]\t{2}\t{3}\t{4}", x, z, x1, -1 * z1, y1);
                Config.Print("{0},{1}[2]\t{2}\t{3}\t{4}", x, z, x2, -1 * z2, y2);
                Config.Print("{0},{1}[3]\t{2}\t{3}\t{4}", x, z, x3, -1 * z3, y3);
                Config.Print("{0},{1}[4]\t{2}\t{3}\t{4}", x, z, x4, -1 * z4, y4);
            }
        }

        private static List<int[]> unitCoordinates_ = new List<int[]>()
        {
            new int[] {5,13},
            new int[] {5,12},
            new int[] {6,12},
            new int[] {7,12},
            new int[] {5,11},
            new int[] {6,11},
            new int[] {7,11},
            new int[] {8,11},
            new int[] {9,11},
            new int[] {10,11},
            new int[] {4,10},
            new int[] {5,10},
            new int[] {6,10},
            new int[] {7,10},
            new int[] {8,10},
            new int[] {9,10},
            new int[] {4,9},
            new int[] {5,9},
            new int[] {6,9},
            new int[] {7,9},
            new int[] {8,9},
            new int[] {9,9},
            new int[] {3,8},
            new int[] {4,8},
            new int[] {5,8},
            new int[] {6,8},
            new int[] {7,8},
            new int[] {8,8},
            new int[] {3,7},
            new int[] {4,7},
            new int[] {5,7},
            new int[] {6,7},
            new int[] {7,7},
            new int[] {8,7},
            new int[] {3,6},
            new int[] {4,6},
            new int[] {5,6},
            new int[] {6,6},
            new int[] {7,6},
            new int[] {8,6},
            new int[] {2,5},
            new int[] {3,5},
            new int[] {4,5},
            new int[] {5,5},
            new int[] {6,5},
            new int[] {7,5},
            new int[] {2,4},
            new int[] {3,4},
            new int[] {4,4},
            new int[] {5,4},
            new int[] {6,4},
            new int[] {7,4},
            new int[] {1,3},
            new int[] {2,3},
            new int[] {3,3},
            new int[] {4,3},
            new int[] {5,3},
            new int[] {6,3},
            new int[] {4,2},
            new int[] {5,2},
            new int[] {6,2},
            new int[] {6,1},
        };
    }
} 
