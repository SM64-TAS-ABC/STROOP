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
                0,
                0,
                0,
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
                0,
                0,
                0,
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
                0,
                0,
                0,
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
                0,
                0,
                0,
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
                0,
                0,
                0,
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
                        0,
                        0,
                        0,
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
            successPositions.Sort((a, b) => Math.Sign(a.Item1 - b.Item1));
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
                0, 0, 0,
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

        public static void TestCombined()
        {
            float marioX = -918.42724609375f;
            float marioY = -2434f;
            float marioZ = -1730.48791503906f;
            float marioXSpeed = 1.16657888889313f;
            float marioYSpeed = 0f;
            float marioZSpeed = 5.46906852722168f;
            float marioHSpeed = 5.59210300445557f;
            ushort marioAngle = 2206;
            ushort cameraAngle = 4132;

            float objX = -897.566040039063f;
            float objZ = -1632.68811035156f;

            int inputX = -14;
            int inputY = -48;

            MarioState marioState = new MarioState(
                marioX, marioY, marioZ,
                marioXSpeed, marioYSpeed, marioZSpeed, marioHSpeed,
                0, 0, 0,
                marioAngle, cameraAngle, null, null, 0);
            Input input = new Input(inputX, inputY);

            // walking
            MarioState afterWalkingTemp = GroundMovementCalculator.ApplyInput(marioState, input);
            MarioState afterWalking = afterWalkingTemp.WithPosition(marioState.X, marioState.Y, marioState.Z);

            // displacement
            (float afterDisplacementX, float afterDisplacementZ) =
                ObjectCalculator.GetObjectDisplacement(
                    afterWalking.X, afterWalking.Z, 37, afterWalking.MarioAngle,
                    objX, objZ, 65 * 1.2f, -5);

            // relative position
            (float relX, float relY, float relZ) = ObjectCalculator.GetRelativePosition(
                afterDisplacementX, afterWalking.Y, afterDisplacementZ,
                afterWalking.MarioAngle, 0, 60, 100);

            MarioState finalState = new MarioState(
                afterDisplacementX, afterWalking.Y, afterDisplacementZ,
                afterWalking.XSpeed, afterWalking.YSpeed, afterWalking.ZSpeed, afterWalking.HSpeed,
                afterWalking.SlidingSpeedX, afterWalking.SlidingSpeedZ, afterWalking.SlidingAngle,
                afterWalking.MarioAngle, cameraAngle, null, null, 0);
            Config.Print(finalState);
            Config.Print("{0},{1},{2}", (double)relX, (double)relY, (double)relZ);
        }

        public static void TestMovementTowardsSpot()
        {
            float startX = -1323.72937011719f;
            float startY = -2434f;
            float startZ = -1579.7392578125f;
            float startXSpeed = 2.64395904541016f;
            float startYSpeed = 0f;
            float startZSpeed = -11.6073894500732f;
            float startHSpeed = 11.9047050476074f;
            ushort startAngle = 30442;
            List<ushort> cameraAngles = new List<ushort>()
            {
                7997,
                8089,
                8185,
                8276,
                8364,
                8454,
                8546,
                8640,
                8704,
                8983,
                9007,
                9007,
                9007,
                9050,
                9138,
                9225,
                9249,
                9249,
                9249,
                9249,
                9249,
                9249,
                9249,
            };
            int INDEX_START = 0;

            float objStartX = -1301.52001953125f;
            float objStartZ = -1677.24182128906f;

            int inputX = 127;
            int inputY = 87;

            MarioState marioState = new MarioState(
                startX, startY, startZ,
                startXSpeed, startYSpeed, startZSpeed, startHSpeed,
                0, 0, 0,
                startAngle, cameraAngles[INDEX_START], null, null, 0);
            MarioBobombState marioBobombState = new MarioBobombState(
                marioState, objStartX, objStartZ);

            Input input = new Input(inputX, inputY);

            MarioBobombState prevMarioBobombState = null;
            for (int i = INDEX_START + 1; i < 9; i++)
            {
                ushort nextCameraAngle = cameraAngles[i];
                prevMarioBobombState = marioBobombState;
                marioBobombState = ApplyInputToMarioBobombState(marioBobombState, input, nextCameraAngle);
            }
            Config.Print(marioBobombState);

            MarioState m = prevMarioBobombState.MarioState;
            (float holpX, float holpY, float holpZ) = HolpCalculator.GetHolp(58, m.X, m.Y, m.Z, m.MarioAngle);

            MarioState m2 = marioBobombState.MarioState;
            float marioX = m2.X;
            float marioY = m2.Y;
            float marioZ = m2.Z;
            ushort marioAngle = m2.MarioAngle;
            float marioRadius = 37;

            float bobombX = holpX;
            float bobombY = holpY;
            float bobombZ = holpZ;

            float padding = -5;

            for (int i = 1; i <= 4; i++)
            {
                if (i == 2)
                {
                    ushort bobombAngle = m.MarioAngle;
                    float delX = 5 * InGameTrigUtilities.InGameSine(bobombAngle);
                    float delZ = 5 * InGameTrigUtilities.InGameCosine(bobombAngle);
                    bobombX += delX;
                    bobombZ += delZ;
                }
                float bobombRadius = 65 * (1f + 0.2f * i);
                (marioX, marioZ) = ObjectCalculator.GetObjectDisplacement(
                    marioX, marioZ, marioRadius, 0, bobombX, bobombZ, bobombRadius, padding);
                Config.Print("{0}: ({1},{2})", i, (double)marioX, (double)marioZ);
            }
        }

        public static bool IsInSortedPositions(
            List<(float, float)> positions, (float, float) position)
        {
            return IsInSortedPositions(positions, position, 0, positions.Count - 1);
        }

        public static bool IsInSortedPositions(
            List<(float, float)> positions, (float, float) position, int startIndex, int endIndex)
        {
            if (startIndex > endIndex) return false;

            int midIndex = (startIndex + endIndex) / 2;
            (float midValue1, float midValue2) = positions[midIndex];
            if (position.Item1 > midValue1)
            {
                return IsInSortedPositions(positions, position, midIndex + 1, endIndex);
            }
            else if (position.Item1 < midValue1)
            {
                return IsInSortedPositions(positions, position, startIndex, midIndex - 1);
            }
            else
            {
                return position.Item2 == midValue2;
            }
        }

        public static float IsInSortedPositions2(
            List<(float, float)> positions, (float, float) position)
        {
            return IsInSortedPositions2(positions, position, 0, positions.Count - 1);
        }

        public static float IsInSortedPositions2(
            List<(float, float)> positions, (float, float) position, int startIndex, int endIndex)
        {
            if (startIndex > endIndex) return float.MaxValue;

            int midIndex = (startIndex + endIndex) / 2;
            (float midValue1, float midValue2) = positions[midIndex];
            if (position.Item1 > midValue1)
            {
                return IsInSortedPositions2(positions, position, midIndex + 1, endIndex);
            }
            else if (position.Item1 < midValue1)
            {
                return IsInSortedPositions2(positions, position, startIndex, midIndex - 1);
            }
            else
            {
                return Math.Abs(position.Item2 - midValue2);
            }
        }

        public static List<List<int>> GetAngleDiffsList(int length, int mid, int range)
        {
            List<int> angleDiffs = new List<int>();
            for (int i = -1 * range; i <= range; i++)
            {
                int angleDiff = mid + i * 16;
                angleDiffs.Add(angleDiff);
            }

            List<List<int>> output = new List<List<int>>();
            GetAngleDiffsListRecursion(output, new List<int>(), length, angleDiffs);
            return output;
        }

        public static void GetAngleDiffsListRecursion(
            List<List<int>> output, List<int> state, int length, List<int> values)
        {
            if (state.Count == length)
            {
                List<int> temp = new List<int>(state);
                output.Add(temp);
                return;
            }

            foreach (int value in values)
            {
                state.Add(value);
                GetAngleDiffsListRecursion(output, state, length, values);
                state.RemoveAt(state.Count - 1);
            }
        }

        public static void TestBruteForceMovingToSpot()
        {
            Config.Print("START BRUTE FORCE");
            List<(float, float)> successPositions = GetSuccessFloatPositions();

            bool boolValue = IsInSortedPositions(
                successPositions, (-1379.0001f, 0f));

            List<List<int>> angleDiffsList = GetAngleDiffsList(7, 96, 5);
            angleDiffsList.ForEach(list => list.Add(0));
            // List<int> angleDiffs = new List<int>() { 89, 92, 96, 91, 88, 90, 92, 2048 };

            //float minDiff = float.MaxValue;
            for (int i = 0; i < angleDiffsList.Count; i++)
            {
                List<int> angleDiffs = angleDiffsList[i];
                (float x, float z) = MoveIntoSpot(angleDiffs);
                float diff = IsInSortedPositions2(successPositions, (x, z));
                if (diff < 0.0002f)
                {
                    Config.Print("{0}: [{1}] ({2},{3})", i, (double)diff, (double)x, (double)z);
                    MoveIntoSpot(angleDiffs, true);
                    Config.Print();
                }
            }
            Config.Print("END BRUTE FORCE");
        }

        public static (float x, float z) MoveIntoSpot(List<int> angleDiffs, bool print = false)
        {
            float startX = -1323.72937011719f;
            float startY = -2434f;
            float startZ = -1579.7392578125f;
            float startXSpeed = 2.64395904541016f;
            float startYSpeed = 0f;
            float startZSpeed = -11.6073894500732f;
            float startHSpeed = 11.9047050476074f;
            ushort startAngle = 30442;

            float objStartX = -1301.52001953125f;
            float objStartZ = -1677.24182128906f;

            MarioState marioState = new MarioState(
                startX, startY, startZ,
                startXSpeed, startYSpeed, startZSpeed, startHSpeed,
                0, 0, 0,
                startAngle, 0, null, null, 0);
            MarioBobombState marioBobombState = new MarioBobombState(
                marioState, objStartX, objStartZ);

            MarioBobombState prevMarioBobombState = null;
            for (int i = 0; i < 8; i++)
            {
                prevMarioBobombState = marioBobombState;
                marioBobombState = ApplyInputToMarioBobombState(marioBobombState, angleDiffs[i]);
                if (print)
                {
                    //Config.Print((43226 + i) + ": " + marioBobombState);
                    Config.Print(
                        "{0} {1} {2} {3} {4}",
                        (43227 + i),
                        (double)marioBobombState.MarioState.X,
                        (double)marioBobombState.MarioState.Y,
                        (double)marioBobombState.MarioState.Z,
                        (double)marioBobombState.MarioState.MarioAngle);
                }
            }
            //Config.Print(marioBobombState);

            MarioState m = prevMarioBobombState.MarioState;
            (float holpX, float holpY, float holpZ) = HolpCalculator.GetHolp(58, m.X, m.Y, m.Z, m.MarioAngle);

            MarioState m2 = marioBobombState.MarioState;
            float marioX = m2.X;
            float marioY = m2.Y;
            float marioZ = m2.Z;
            ushort marioAngle = m2.MarioAngle;
            float marioRadius = 37;

            float bobombX = holpX;
            float bobombY = holpY;
            float bobombZ = holpZ;

            float padding = -5;

            for (int i = 1; i <= 4; i++)
            {
                if (i == 2)
                {
                    ushort bobombAngle = m.MarioAngle;
                    float delX = 5 * InGameTrigUtilities.InGameSine(bobombAngle);
                    float delZ = 5 * InGameTrigUtilities.InGameCosine(bobombAngle);
                    bobombX += delX;
                    bobombZ += delZ;
                }
                float bobombRadius = 65 * (1f + 0.2f * i);
                (marioX, marioZ) = ObjectCalculator.GetObjectDisplacement(
                    marioX, marioZ, marioRadius, 0, bobombX, bobombZ, bobombRadius, padding);
                //Config.Print("{0}: ({1},{2})", i, (double)marioX, (double)marioZ);

                if (print)
                {
                    if (i == 1)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            Config.Print(
                                "{0} {1} {2} {3} {4}",
                                43235 + j,
                                (double)marioX,
                                (double)m2.Y,
                                (double)marioZ,
                                (double)m2.MarioAngle);
                        }
                    }
                    else
                    {
                        Config.Print(
                            "{0} {1} {2} {3} {4}",
                            43237 + i,
                            (double)marioX,
                            (double)m2.Y,
                            (double)marioZ,
                            (double)m2.MarioAngle);
                    }
                }
            }

            return (marioX, marioZ);
        }

        public class MarioBobombState
        {
            public readonly MarioState MarioState;
            public readonly float ObjX;
            public readonly float ObjZ;

            public MarioBobombState(MarioState marioState, float objX, float objZ)
            {
                MarioState = marioState;
                ObjX = objX;
                ObjZ = objZ;
            }

            public override string ToString()
            {
                return String.Format("{0} obj=({1},{2})", MarioState, (double)ObjX, (double)ObjZ);
            }
        }

        public static MarioBobombState ApplyInputToMarioBobombState(
            MarioBobombState initialState, Input input, ushort nextCameraAngle)
        {
            // get vars
            MarioState marioState = initialState.MarioState;
            float objX = initialState.ObjX;
            float objZ = initialState.ObjZ;

            // walking
            MarioState afterWalkingTemp = GroundMovementCalculator.ApplyInput(marioState, input);
            // doesn't move due to ceiling
            MarioState afterWalking = afterWalkingTemp.WithPosition(marioState.X, marioState.Y, marioState.Z); 

            // displacement
            (float afterDisplacementX, float afterDisplacementZ) =
                ObjectCalculator.GetObjectDisplacement(
                    afterWalking.X, afterWalking.Z, 37, afterWalking.MarioAngle,
                    objX, objZ, 65 * 1.2f, -5);

            // relative position
            (float relX, float relY, float relZ) = ObjectCalculator.GetRelativePosition(
                afterDisplacementX, afterWalking.Y, afterDisplacementZ,
                afterWalking.MarioAngle, 0, 60, 100);

            MarioState finalMarioState = new MarioState(
                afterDisplacementX, afterWalking.Y, afterDisplacementZ,
                afterWalking.XSpeed, afterWalking.YSpeed, afterWalking.ZSpeed, afterWalking.HSpeed,
                afterWalking.SlidingSpeedX, afterWalking.SlidingSpeedZ, afterWalking.SlidingAngle,
                afterWalking.MarioAngle, nextCameraAngle, null, null, 0);
            MarioBobombState finalMarioBobombState = new MarioBobombState(finalMarioState, relX, relZ);
            return finalMarioBobombState;
        }

        public static MarioBobombState ApplyInputToMarioBobombState(
            MarioBobombState initialState, int angleDiff)
        {
            // get vars
            MarioState marioState = initialState.MarioState;
            float objX = initialState.ObjX;
            float objZ = initialState.ObjZ;

            // walking
            MarioState afterWalkingTemp = GroundMovementCalculator.ApplyInput(marioState, angleDiff);
            // doesn't move due to ceiling
            MarioState afterWalking = afterWalkingTemp.WithPosition(marioState.X, marioState.Y, marioState.Z);

            // displacement
            (float afterDisplacementX, float afterDisplacementZ) =
                ObjectCalculator.GetObjectDisplacement(
                    afterWalking.X, afterWalking.Z, 37, afterWalking.MarioAngle,
                    objX, objZ, 65 * 1.2f, -5);

            // relative position
            (float relX, float relY, float relZ) = ObjectCalculator.GetRelativePosition(
                afterDisplacementX, afterWalking.Y, afterDisplacementZ,
                afterWalking.MarioAngle, 0, 60, 100);

            MarioState finalMarioState = new MarioState(
                afterDisplacementX, afterWalking.Y, afterDisplacementZ,
                afterWalking.XSpeed, afterWalking.YSpeed, afterWalking.ZSpeed, afterWalking.HSpeed,
                afterWalking.SlidingSpeedX, afterWalking.SlidingSpeedZ, afterWalking.SlidingAngle,
                afterWalking.MarioAngle, 0, null, null, 0);
            MarioBobombState finalMarioBobombState = new MarioBobombState(finalMarioState, relX, relZ);
            return finalMarioBobombState;
        }

        public static void CalculateTylerChallenge()
        {
            float startX = 0f;
            float startY = 251.947235107422f;
            float startZ = -12.3211631774902f;
            float startXSpeed = 0f;
            float startYSpeed = 7.99412536621094f;
            float startZSpeed = 2.85620307922363f;
            float startHSpeed = 2.85620307922363f;

            ushort marioAngle = 0;
            ushort cameraAngle = 32768;

            MarioState startState = new MarioState(
                startX,
                startY,
                startZ,
                startXSpeed,
                startYSpeed,
                startZSpeed,
                startHSpeed,
                0,
                0,
                0,
                marioAngle,
                cameraAngle,
                null,
                null,
                0);

            int lastIndex = -1;
            List<Input> inputs = CalculatorUtilities.GetInputRange(0, 0, -65, 65);
            float bestDiff = 1;
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

                    if (state.Index != lastIndex)
                    {
                        if (state.Index == 5) return;
                        lastIndex = state.Index;
                        Config.Print("Now at index " + lastIndex);
                    }

                    float diff = (float)MoreMath.GetDistanceBetween(state.X, state.Z, 0, 0);

                    if (diff < bestDiff)
                    {
                        bestDiff = diff;
                        Config.Print("New best diff of " + diff);
                        Config.Print(state.GetLineage());
                        Config.Print();
                    }

                    alreadySeen.Add(state);
                    queue.Enqueue(state);
                }
            }
            Config.Print("FAILED");
        }
    }
}
