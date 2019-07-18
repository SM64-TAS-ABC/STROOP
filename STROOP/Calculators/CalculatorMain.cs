using STROOP.Forms;
using STROOP.Managers;
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
    public static class CalculatorMain
    {
        public static void CalculateMovementForBitsHolp()
        {
            float startX = 435.913696289063f;
            float startY = 4474f;
            float startZ = -1854.50500488281f;
            float startXSpeed = -16.1702556610107f;
            float startYSpeed = -75f;
            float startZSpeed = -17.676326751709f;
            float startHSpeed = 23.8997459411621f;

            ushort marioAngle = 39780;
            ushort cameraAngle = 16384;

            float goalX = 392.857605f;
            float goalY = 4249f;
            float goalZ = -1901.016846f;

            int xInput = -56;
            int zInput = -31;
            int xRadius = 10;
            int zRadius = 10;

            MarioState startState = new MarioState(
                            startX,
                            startY,
                            startZ,
                            startXSpeed,
                            startYSpeed,
                            startZSpeed,
                            startHSpeed,
                            marioAngle,
                            cameraAngle,
                            null,
                            null,
                            0);

            int lastIndex = -1;
            List<Input> inputs = CalculatorUtilities.GetInputRange(xInput - xRadius, xInput + xRadius, zInput - zRadius, zInput + zRadius);
            float bestDiff = float.MaxValue;
            MarioState bestState = null;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => AirMovementCalculator.ApplyInput(dequeue, input));
                foreach (MarioState state in nextStates)
                {
                    if (alreadySeen.Contains(state)) continue;
                    if (state.Index > 3) continue;

                    if (state.Index != lastIndex)
                    {
                        lastIndex = state.Index;
                        System.Diagnostics.Trace.WriteLine("Now at index " + lastIndex);
                    }

                    if (state.Index == 3)
                    {
                        float diff = (float)MoreMath.GetDistanceBetween(state.X, state.Z, goalX, goalZ);

                        if (diff < bestDiff)
                        {
                            bestDiff = diff;
                            bestState = state;
                            System.Diagnostics.Trace.WriteLine("Diff of " + bestDiff + " is: " + bestState.GetLineage());
                        }
                    }

                    alreadySeen.Add(state);
                    queue.Enqueue(state);
                }
            }
            System.Diagnostics.Trace.WriteLine("Done");
        }

        public static void CalculateMovementForWfHolp()
        {
            float startX = 310.128448486328f;
            float startY = 4384f;
            float startZ = -1721.65405273438f;
            float startXSpeed = 15.5246114730835f;
            float startYSpeed = -24f;
            float startZSpeed = -12.4710474014282f;
            float startHSpeed = 19.8780212402344f;

            ushort marioAngle = 24066;

            Dictionary<int, ushort> cameraAngles =
                new Dictionary<int, ushort>()
                {
                    //[0] = 32707,
                    [0] = 32768,
                    [1] = 32839,
                    [2] = 32900,
                    [3] = 32972,
                    [4] = 33063,
                    [5] = 33135,
                    [6] = 33216,
                };

            float goalX = 374.529907226563f;
            float goalY = 4264f;
            float goalZ = -1773.07543945313f;

            int xInput = -45;
            int zInput = -27;
            int xRadius = 5;
            int zRadius = 5;

            MarioState startState = new MarioState(
                startX,
                startY,
                startZ,
                startXSpeed,
                startYSpeed,
                startZSpeed,
                startHSpeed,
                marioAngle,
                cameraAngles[0],
                null,
                null,
                0);

            int lastIndex = -1;
            List<Input> inputs = CalculatorUtilities.GetInputRange(xInput - xRadius, xInput + xRadius, zInput - zRadius, zInput + zRadius);
            float bestDiff = float.MaxValue;
            MarioState bestState = null;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => AirMovementCalculator.ApplyInput(dequeue, input));
                nextStates = nextStates.ConvertAll(state => state.WithCameraAngle(cameraAngles[state.Index]));
                foreach (MarioState state in nextStates)
                {
                    if (alreadySeen.Contains(state)) continue;
                    if (state.Index > 4) continue;

                    if (state.Index != lastIndex)
                    {
                        lastIndex = state.Index;
                        System.Diagnostics.Trace.WriteLine("Now at index " + lastIndex);
                    }

                    if (state.Index == 4)
                    {
                        float diff = (float)MoreMath.GetDistanceBetween(state.X, state.Z, goalX, goalZ);

                        if (diff < bestDiff)
                        {
                            bestDiff = diff;
                            bestState = state;
                            System.Diagnostics.Trace.WriteLine("Diff of " + bestDiff + " is: " + bestState.GetLineage());
                        }
                    }

                    alreadySeen.Add(state);
                    queue.Enqueue(state);
                }
            }
            System.Diagnostics.Trace.WriteLine("Done");
        }

        public static void CalculateMovementForBully()
        {
            /*
            float startX = -6842.04736328125f;
            float startY = 2358;
            float startZ = -506.698120117188f;
            float startXSpeed = -34.6734008789063f;
            float startYSpeed = -74;
            float startZSpeed = 0;
            float startHSpeed = 34.6734008789063f;
            */

            float startX = -8172.14892578125f;
            float startY = -47.4696655273438f;
            float startZ = -507.290283203125f;
            float startXSpeed = -3.33430767059326f;
            float startYSpeed = -75;
            float startZSpeed = 0;
            float startHSpeed = 3.33430767059326f;

            float goalX = -8171.970703125f;
            float goalZ = -507.2902832031f;

            ushort marioAngle = 49152;
            ushort cameraAngle = 32768;

            MarioState startState = new MarioState(
                startX,
                startY,
                startZ,
                startXSpeed,
                startYSpeed,
                startZSpeed,
                startHSpeed,
                marioAngle,
                cameraAngle,
                null,
                null,
                0);

            int lastIndex = -1;
            List<Input> inputs = CalculatorUtilities.GetInputRange(-70, 70, 0, 0);
            float bestDiff = float.MaxValue;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => AirMovementCalculator.ApplyInput(dequeue, input));
                foreach (MarioState state in nextStates)
                {
                    if (alreadySeen.Contains(state)) continue;

                    float threshold = 10f / (state.Index * state.Index);
                    if (state.Index != lastIndex)
                    {
                        lastIndex = state.Index;
                        System.Diagnostics.Trace.WriteLine("Now at index " + lastIndex + " with threshold " + threshold);
                    }

                    float diff = (float)MoreMath.GetDistanceBetween(state.X, state.Z, goalX, goalZ);
                    if (diff > threshold) continue;

                    if (diff < bestDiff)
                    {
                        bestDiff = diff;
                        System.Diagnostics.Trace.WriteLine("New best diff of " + diff);
                    }
                    //System.Diagnostics.Trace.WriteLine(diff + " < " + threshold + " at index " + state.Index);

                    if (diff == 0 && Math.Abs(state.HSpeed) < 0.2)
                    {
                        System.Diagnostics.Trace.WriteLine("");
                        System.Diagnostics.Trace.WriteLine(state.GetLineage());
                        return;
                    }

                    alreadySeen.Add(state);
                    queue.Enqueue(state);
                }
            }
            System.Diagnostics.Trace.WriteLine("FAILED");
        }

        public static void CalculateMovementForWallGap()
        {
            float startX = -258.926910400391f;
            float startY = 2373f;
            float startZ = 5770.876953125f;
            float startXSpeed = 30.5356960296631f;
            float startYSpeed = -10f;
            float startZSpeed = 0f;
            float startHSpeed = 30.5356960296631f;

            float goalX = -89.956619262695313f;

            int listLength = 1000;

            List<float> floats = new List<float>();
            List<int> counts = new List<int>();
            float f = goalX;
            for (int i = 0; i < listLength; i++)
            {
                floats.Add(f);
                f += 0.00001f;
                counts.Add(0);
            }

            ushort marioAngle = 16384;
            ushort cameraAngle = 49152;

            int inputRadius = 8;

            MarioState startState = new MarioState(
                startX,
                startY,
                startZ,
                startXSpeed,
                startYSpeed,
                startZSpeed,
                startHSpeed,
                marioAngle,
                cameraAngle,
                null,
                null,
                0);

            int lastIndex = -1;
            List<Input> inputs = CalculatorUtilities.GetInputRange(0, 0, -38 - inputRadius, -38 + inputRadius);

            float bestDiff = float.MaxValue;
            MarioState bestState = null;

            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => AirMovementCalculator.ApplyInput(dequeue, input));
                foreach (MarioState state in nextStates)
                {
                    if (alreadySeen.Contains(state)) continue;

                    if (state.Index > lastIndex)
                    {
                        lastIndex = state.Index;
                        Config.Print("Now at index " + state.Index + " with queue size " + queue.Count);
                        /*
                        if (queue.Count > 100000)
                        {
                            Config.Print("Commence pruning");
                            List<MarioState> states = queue.ToList();
                            queue.Clear();
                            Random random = new Random();
                            while (queue.Count < 100000)
                            {
                                int index = random.Next(0, states.Count);
                                MarioState enqueue = states[index];
                                states.RemoveAt(index);
                                queue.Enqueue(enqueue);
                            }
                            Config.Print("Now at index " + state.Index + " with queue size " + queue.Count);
                        }
                        */
                    }

                    int numFramesRemaining = ((int)state.YSpeed + 34) / 4;
                    float expectedX = AirMovementCalculator.ComputePosition(state.X, state.XSpeed, numFramesRemaining);
                    float expectedDiff = Math.Abs(expectedX - goalX);
                    float threshold = (float)Math.Pow(2, numFramesRemaining) * 2;
                    if (expectedDiff > threshold) continue;

                    if (state.YSpeed == -34)
                    {
                        float diff = Math.Abs(state.X - goalX);
                        if (diff <= bestDiff / 1.1f || diff == 0)
                        {
                            bestDiff = diff;
                            bestState = state;
                            Config.Print("New best diff of " + diff + " with state:\r\n" + state.GetLineage());
                        }

                        for (int i = 0; i < floats.Count; i++)
                        {
                            if (state.X == floats[i]) counts[i]++;
                        }
                    }
                    else
                    {
                        queue.Enqueue(state);
                        alreadySeen.Add(state);
                    }
                }
            }
            Config.Print("END");
            for (int i = 0; i < floats.Count; i++)
            {
                Config.Print(i + "\t" + counts[i] + "\t" + floats[i]);
            }
        }

        public static void CalculateMovementForTtmHolp()
        {
            float startX = 1094.12268066406f;
            float startY = -476.171997070313f;
            float startZ = -3675.9716796875f;
            float startXSpeed = -6.70571994781494f;
            float startYSpeed = -52f;
            float startZSpeed = -0.628647029399872f;
            float startHSpeed = -6.70173645019531f;

            ushort marioAngle = 16455;

            Dictionary<int, ushort> cameraAngles =
                new Dictionary<int, ushort>()
                {
                    [0] = 28563,
                    [1] = 28552,
                    [2] = 28548,
                    [3] = 28533,
                    [4] = 28524,
                    [5] = 28514,
                    [6] = 28500,
                };

            float goalX = 1060.860229f;
            float goalY = -5001.017029f;
            float goalZ = -3678.57666f;

            int xInput = 56;
            int zInput = 22;
            int xRadius = 5;
            int zRadius = 5;

            MarioState startState = new MarioState(
                startX,
                startY,
                startZ,
                startXSpeed,
                startYSpeed,
                startZSpeed,
                startHSpeed,
                marioAngle,
                cameraAngles[0],
                null,
                null,
                0);

            int lastIndex = -1;
            List<Input> inputs = CalculatorUtilities.GetInputRange(xInput - xRadius, xInput + xRadius, zInput - zRadius, zInput + zRadius);
            float bestDiff = float.MaxValue;
            MarioState bestState = null;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => AirMovementCalculator.ApplyInput(dequeue, input));
                nextStates = nextStates.ConvertAll(state => state.WithCameraAngle(cameraAngles[state.Index]));
                foreach (MarioState state in nextStates)
                {
                    if (alreadySeen.Contains(state)) continue;
                    if (state.Index > 4) continue;

                    if (state.Index != lastIndex)
                    {
                        lastIndex = state.Index;
                        System.Diagnostics.Trace.WriteLine("Now at index " + lastIndex);
                    }

                    if (state.Index == 4)
                    {
                        float diff = (float)MoreMath.GetDistanceBetween(state.X, state.Z, goalX, goalZ);

                        if (diff > 1 ? diff < bestDiff * 0.5 : diff < bestDiff)
                        {
                            bestDiff = diff;
                            bestState = state;
                            System.Diagnostics.Trace.WriteLine("Diff of " + bestDiff + " is: " + bestState.GetLineage());
                        }
                    }

                    alreadySeen.Add(state);
                    queue.Enqueue(state);
                }
            }
            System.Diagnostics.Trace.WriteLine("Done");
        }

        public static List<(float, float)> GetSuccessFloatPositions()
        {
            // initial
            float startX = -1378.91674804688f;
            float startY = -2434f;
            float startZ = -1423.35168457031f;
            float startXSpeed = 0f;
            float startYSpeed = 20f;
            float startZSpeed = 0f;
            float startHSpeed = 0f;

            // after all 4 q steps (no wall displacement)
            float endX = -1376.13940429688f;
            float endY = -2414f;
            float endZ = -1423.66223144531f;
            float endXSpeed = 2.7774920463562f;
            float endYSpeed = 16f;
            float endZSpeed = -0.310500144958496f;
            float endHSpeed = -1.45670866966248f;

            // after 1 q step (no wall displacement)
            float qstepX = -1378.22241210938f;
            float qstepY = -2429f;
            float qstepZ = -1423.42932128906f;
            float qstepXSpeed = 2.7774920463562f;
            float qstepYSpeed = -4f;
            float qstepZSpeed = -0.310500144958496f;
            float qstepHSpeed = -1.45670866966248f;

            // after 1 q step and wall displacement
            float displacedX = -1307.73107910156f;
            float displacedY = -2429f;
            float displacedZ = -1353.11071777344f;
            float displacedXSpeed = 0f;
            float displacedYSpeed = -4f;
            float displacedZSpeed = 0f;
            float displacedHSpeed = 0f;

            // closest starting position that works
            float closestX = -1378.91381835938f;
            float closestY = -2434f;
            float closestZ = -1423.34875488281f;
            float closestXSpeed = -3.67686033248901f;
            float closestYSpeed = 0f;
            float closestZSpeed = -4.74138116836548f;
            float closestHSpeed = 6f;

            // farthest starting position that is within range (doesn't work)
            float farthestX = -1379.22241210938f;
            float farthestY = -2434f;
            float farthestZ = -1423.65734863281f;
            float farthestXSpeed = 0f;
            float farthestYSpeed = 0f;
            float farthestZSpeed = 0f;
            float farthestHSpeed = 0f;

            ushort marioAngle = 39655;
            ushort cameraAngle = 7142;

            TriangleDataModel tri = new TriangleDataModel(0x8015F910);

            List<(float, float)> successPositions = new List<(float, float)>();
            int numAttempts = 0;
            int numSuccesses = 0;
            for (float lineX = closestX, lineZ = closestZ; lineX >= farthestX; lineX -= 0.0001f, lineZ -= 0.0001f)
            {
                List<float> pointXs = new List<float>();

                float temp = lineX;
                pointXs.Add(temp);

                temp = lineX;
                for (int i = 0; i < 10; i++)
                {
                    temp -= 0.0001f;
                    pointXs.Add(temp);
                }

                temp = lineX;
                for (int i = 0; i < 10; i++)
                {
                    temp += 0.0001f;
                    pointXs.Add(temp);
                }

                float pointZ = lineZ;
                foreach (float pointX in pointXs)
                {
                    MarioState pointState = new MarioState(
                        pointX,
                        startY,
                        pointZ,
                        startXSpeed,
                        startYSpeed,
                        startZSpeed,
                        startHSpeed,
                        marioAngle,
                        cameraAngle,
                        null,
                        null,
                        0);
                    Input input = new Input(32, -124);
                    MarioState movedState = AirMovementCalculator.ApplyInput(pointState, input, 1);
                    (float dispX, float dispZ) = WallDisplacementCalculator.HandleWallDisplacement(
                        movedState.X, movedState.Y, movedState.Z, tri, 50, 150);
                    bool match = dispX == displacedX && dispZ == displacedZ;

                    if (match)
                    {
                        successPositions.Add((pointX, pointZ));
                        /*
                        Config.Print(
                            "({0},{1}) => ({2},{3}) match={4}",
                            (double)pointX, (double)pointZ, (double)dispX, (double)dispZ, match);
                        */
                    }
                    
                    numAttempts++;
                    if (match) numSuccesses++;
                }
            }

            /*
            Config.Print("numAttempts = " + numAttempts);
            Config.Print("numSuccesses = " + numSuccesses);
            */
            return successPositions;
        }

        public static void TestWalkingCode()
        {
            float startX = -7390.01953125f;
            float startY = -3153f;
            float startZ = 3936.21435546875f;
            float startXSpeed = 7.88103151321411f;
            float startYSpeed = 0f;
            float startZSpeed = -15.0203580856323f;
            float startHSpeed = 16.9623641967773f;
            ushort startMarioAngle = 27738;
            ushort startCameraAngle = 0;

            MarioState marioState = new MarioState(
                startX, startY, startZ,
                startXSpeed, startYSpeed, startZSpeed, startHSpeed,
                startMarioAngle, startCameraAngle, null, null, 0);
            Input input = new Input(23, 26);

            for (int i = 0; i < 10; i++)
            {
                Config.Print(i + ": " + marioState);
                marioState = GroundMovementCalculator.ApplyInput(marioState, input);
            }
        }

        public static void TestGetRelativePosition()
        {
            float marioX = -1431.61889648438f;
            float marioY = -4003f;
            float marioZ = -1318.10009765625f;
            ushort marioAngle = 53906;

            (float x, float y, float z) = ObjectCalculator.GetRelativePosition(
                marioX, marioY, marioZ, marioAngle, 0, 60, 100);

            Config.Print("{0},{1},{2}", (double)x, (double)y, (double)z);
        }

        public static void TestGetObjectDisplacement()
        {
            float marioX = -1462.44079589844f;
            float marioY = -4003f;
            float marioZ = -1196.89099121094f;
            float marioRadius = 37;

            float bobombX = -1538.07922363281f;
            float bobombY = -4003f;
            float bobombZ = -1257.61840820313f;
            float bobombRadius = 65 * 1.2f;

            float padding = -5;

            (float x, float z) = ObjectCalculator.GetObjectDisplacement(
                marioX, marioZ, marioRadius, 0, bobombX, bobombZ, bobombRadius, padding);
            Config.Print("{0},{1}", (double)x, (double)z);
        }
    }
}
