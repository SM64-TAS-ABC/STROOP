using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class OverflowJumpCalculator
    {
        public static List<TriangleDataModel> flatFloorTris;
        public static List<int> possibleAngles;

        public static void Test()
        {
            double startX = -2838;
            double startY = 1610.8349609375;
            double startZ = -42.8171844482422;
            List<double> initialHSpeeds = new List<double>()
            {
                -427922.90625, -641883.5, -962824.375, -1444235.625, -2166352.75, -3249528.25,
            };
            double goalX = 2806.789063;
            double goalY = 1033;
            double goalZ = -130.9819946;

            flatFloorTris = TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsFloor() && tri.NormY == 1 && tri.SurfaceType != 0x0A);
            possibleAngles = Enumerable.Range(0, 4096).ToList().ConvertAll(i => 16 * i);

            List<double> moreHSpeeds = initialHSpeeds.ConvertAll(
                hSpeed => GetSuccessiveHSpeeds(hSpeed)).SelectMany(list => list).ToList();
            initialHSpeeds.AddRange(moreHSpeeds);
            initialHSpeeds = ControlUtilities.Randomize(initialHSpeeds);

            Queue<MarioPuState> queue = new Queue<MarioPuState>();
            initialHSpeeds.ForEach(hSpeed => queue.Enqueue(
                new MarioPuState(startX, startY, startZ, hSpeed, 0, null, 0)));

            Config.Print("START");
            int lastIndex = -1;
            while (queue.Count > 0)
            {
                MarioPuState dequeue = queue.Dequeue();
                if (dequeue.Index > lastIndex)
                {
                    lastIndex = dequeue.Index;
                    Config.Print("Now on index " + lastIndex);
                }

                List<int> anglesToUse;
                if (dequeue.Index < 2)
                {
                    anglesToUse = possibleAngles;
                }
                else
                {
                    int angle = MoreMath.NormalizeAngleTruncated(MoreMath.AngleTo_AngleUnits(goalX, goalZ, dequeue.X, dequeue.Z));
                    anglesToUse = new List<int>() { angle - 16, angle, angle + 16 };
                }

                foreach (int angle in anglesToUse)
                {
                    MarioPuState state = TestOverflowJump(dequeue, angle);
                    if (state == null) continue;

                    double dist = MoreMath.GetDistanceBetween(state.X, state.Z, goalX, goalZ);
                    if (state.Index >= 2 && dist > Math.Abs(state.HSpeed)) continue;
                    if (dist < 1000 && state.Y == goalY)
                    {
                        Config.Print(dist);
                        Config.Print(state.GetLineage());
                    }

                    if (state.Index >= 3) continue;
                    List<MarioPuState> nextStates = GetSuccessiveStates(state);
                    nextStates.Insert(0, state);
                    nextStates = ControlUtilities.Randomize(nextStates);
                    nextStates.ForEach(s => queue.Enqueue(s));
                }
            }
            Config.Print("END");
        }

        public static MarioPuState TestOverflowJump(MarioPuState state, int angle)
        {
            double hSpeed = state.HSpeed * 0.8;
            double hDist = hSpeed / 4;
            double vSpeed = 42 + state.HSpeed / 4;
            double vDist = vSpeed / 4;

            (double x, double z) = MoreMath.AddVectorToPoint(hDist, angle, state.X, state.Z);
            double y = state.Y + vDist;

            double modX = MoreMath.MaybeNegativeModulus(x, 65536);
            double modY = MoreMath.MaybeNegativeModulus(y, 65536);
            double modZ = MoreMath.MaybeNegativeModulus(z, 65536);

            if (modX < -8192 || modX > 8192) return null;
            if (modZ < -8192 || modZ > 8192) return null;
            if (modY < -8192) return null;

            double? bestFloorY = null;
            foreach (TriangleDataModel tri in flatFloorTris)
            {
                if (tri.IsPointInsideAndAboveTriangle(modX, modY, modZ))
                {
                    double floorY = tri.GetHeightOnTriangle(modX, modZ);
                    if (bestFloorY == null || floorY > bestFloorY)
                    {
                        bestFloorY = floorY;
                    }
                }
            }

            if (bestFloorY.HasValue && bestFloorY.Value > state.Y - 3000)
            {
                return new MarioPuState(x, bestFloorY.Value, z, hSpeed, angle, state, state.Index + 1);
            }
            return null;
        }

        public class MarioPuState
        {
            public readonly double X;
            public readonly double Y;
            public readonly double Z;
            public readonly double HSpeed;
            public readonly double Angle;
            public readonly MarioPuState Predecessor;
            public readonly int Index;

            public MarioPuState(double x, double y, double z, double hSpeed, double angle, MarioPuState predecessor, int index)
            {
                X = x;
                Y = y;
                Z = z;
                HSpeed = hSpeed;
                Angle = angle;
                Predecessor = predecessor;
                Index = index;
            }

            public string GetLineage()
            {
                double deltaY = Predecessor == null ? 0 : Y - Predecessor.Y;
                string past = Predecessor == null ? "" : Predecessor.GetLineage();
                string current = ToString() + " " + deltaY + "\r\n";
                return past + current;
            }

            public override string ToString()
            {
                return string.Format(
                    "X={0} Y={1} Z={2} HSpeed={3} Angle={4}",
                    X, Y, Z, HSpeed, Angle);
            }
        }

        public static List<double> GetSuccessiveHSpeeds(double hSpeed)
        {
            List<double> output = new List<double>();
            for (int numJumps = 1; numJumps <= 3; numJumps++)
            {
                for (int numDusts = 0; numDusts <= numJumps * 3 + 3; numDusts++)
                {
                    double value = hSpeed * Math.Pow(0.8, numJumps) * Math.Pow(0.98, numDusts);
                    if (value > -200_000) continue;
                    output.Add(value);
                }
            }
            return output;
        }

        public static List<MarioPuState> GetSuccessiveStates(MarioPuState state)
        {
            List<double> hSpeeds = GetSuccessiveHSpeeds(state.HSpeed);
            return hSpeeds.ConvertAll(
                hSpeed => new MarioPuState(state.X, state.Y, state.Z, hSpeed, state.Angle, state, state.Index));
        }
    }
}
