using STROOP.Forms;
using STROOP.Managers;
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
    public static class MovementCalculator
    {
        /*
        def computeAirSpeed(mario): # 8026a554
            perpSpeed = 0
            if windyAirSpeed(mario):
                return
            if mario.action == MarioAction.longJump:
                maxSpeed = 48
            else:
                maxSpeed = 32
            mario.hspeed = approachHSpeed(mario.hspeed, 0, 0.35, 0.35)
            if mario.input[0]: # tilting analog stick
                mario.hspeed += (mario.analogTilt/32)*1.5*cosTable(mario.h_0x24 - mario.euler.yaw)
                perpSpeed = sinTable(mario.h_0x24 - mario.euler.yaw)*(mario.analogTilt/32)*10
            if mario.hspeed > maxSpeed:
                mario.hspeed -= 1
            if mario.hspeed < -16:
                mario.hspeed += 2
            mario.specialXSpeed = sinTable(mario.euler.yaw)*mario.hspeed
            mario.specialZSpeed = cosTable(mario.euler.yaw)*mario.hspeed
            mario.specialXSpeed += perpSpeed*sinTable(mario.euler.yaw + 0x4000)
            mario.specialZSpeed += perpSpeed*cosTable(mario.euler.yaw + 0x4000)
            mario.velocity.x = mario.specialXSpeed
            mario.velocity.z = mario.specialZSpeed

        def airMove(mario,a1): #80256940
            finalValue = 0
            mario.wallTri = None
            newPos = mario.pos #stored @ sp24
            for i in range(4):
                newPos += (mario.vel/4)
                value = checkAirQframe(mario,newPos,a1)
                if value != 0: # note that a 2 followed by 0s will give 2
                    finalValue = value
                if value  in [1,3,4,6]:
                    break
            if 0 <= mario.vel.y:
                mario.fallPeak = mario.pos.y
            sw(mario+0x14,fn8025167c(mario))
            if mario.action != 0x10880899:
                mario.determineGravity()
            applyWind(mario)
            mario.obj.pos = mario.pos.copy() #via copyVector
            setSignedShortVector(mario.obj+0x1a,0,short(mario+0x2e),0)
            return finalValue

        def approachHSpeed(speed,maxSpeed,a2,a3):   #8037A8B4
            # f12 current hspeed
            # f14 max hspeed
            # a2,a3 floats
            if speed < maxSpeed:
                return min(maxSpeed, speed+a2)
            else:
                return max(maxSpeed, speed-a3)
        */

        private class Input
        {
            public readonly int X;
            public readonly int Y;

            public Input(int x, int y)
            {
                X = x;
                Y = y;
            }

            public float GetScaledMagnitude()
            {
                return MoreMath.GetScaledInputMagnitude(X, Y, false);
            }

            public override string ToString()
            {
                return String.Format("({0},{1})", X, -1 * Y);
            }
        }

        private class MarioState
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly float XSpeed;
            public readonly float YSpeed;
            public readonly float ZSpeed;
            public readonly float HSpeed;
            public readonly ushort MarioAngle;
            public readonly ushort CameraAngle;

            public readonly MarioState PreviousState;
            public readonly Input LastInput;
            public readonly int Index;

            public MarioState(
                float x, float y, float z,
                float xSpeed, float ySpeed, float zSpeed, float hSpeed,
                ushort marioAngle, ushort cameraAngle,
                MarioState previousState, Input lastInput, int index)
            {
                X = x;
                Y = y;
                Z = z;
                XSpeed = xSpeed;
                YSpeed = ySpeed;
                ZSpeed = zSpeed;
                HSpeed = hSpeed;
                MarioAngle = marioAngle;
                CameraAngle = cameraAngle;

                PreviousState = previousState;
                LastInput = lastInput;
                Index = index;
            }

            public MarioState ApplyInput(Input input)
            {
                MarioState withHSpeed = ComputeAirHSpeed(this, input);
                MarioState moved = AirMove(withHSpeed);
                MarioState withYSpeed = ComputeAirYSpeed(moved);
                return withYSpeed;
            }

            public override string ToString()
            {
                return String.Format(
                    "pos=({0},{1},{2}) spd=({3},{4},{5}) hspd={6}",
                    (double)X, (double)Y, (double)Z,
                    (double)XSpeed, (double)YSpeed, (double)ZSpeed, (double)HSpeed);
            }

            public string ToStringWithInput()
            {
                string inputString = LastInput != null ? LastInput + " to " : "";
                return inputString + ToString();
            }

            private List<object> GetFields()
            {
                return new List<object>()
                {
                    X, Y, Z,
                    XSpeed, YSpeed, ZSpeed, HSpeed,
                    MarioAngle, CameraAngle,
                };
            }

            public override bool Equals(object obj)
            {
                if (!(obj is MarioState)) return false;
                MarioState other = obj as MarioState;
                return Enumerable.SequenceEqual(
                    GetFields(), other.GetFields());
            }

            public override int GetHashCode()
            {
                return GetFields().GetHashCode();
            }

            public string GetLineage()
            {
                if (PreviousState == null)
                {
                    return ToStringWithInput();
                }
                else
                {
                    return PreviousState.GetLineage() + "\n" + ToStringWithInput();
                }
            }

            public MarioState WithCameraAngle(ushort cameraAngle)
            {
                return new MarioState(
                    X, Y, Z,
                    XSpeed, YSpeed, ZSpeed, HSpeed,
                    MarioAngle, cameraAngle,
                    PreviousState, LastInput, Index);
            }
        }

        private static MarioState AirMove(MarioState initialState)
        {
            float newX = initialState.X;
            float newY = initialState.Y;
            float newZ = initialState.Z;

            for (int i = 0; i < 4; i++)
            {
                newX += initialState.XSpeed / 4;
                newY += initialState.YSpeed / 4;
                newZ += initialState.ZSpeed / 4;
            }

            return new MarioState(
                newX,
                newY,
                newZ,
                initialState.XSpeed,
                initialState.YSpeed,
                initialState.ZSpeed,
                initialState.HSpeed,
                initialState.MarioAngle,
                initialState.CameraAngle,
                initialState.PreviousState,
                initialState.LastInput,
                initialState.Index);
        }

        private static MarioState ComputeAirHSpeed(MarioState initialState, Input input)
        {
            bool longJump = false;
            int maxSpeed = longJump ? 48 : 32;

            ushort marioAngle = initialState.MarioAngle;
            ushort yawIntended = MoreMath.CalculateAngleFromInputs(input.X, input.Y, initialState.CameraAngle);
            int deltaAngleIntendedFacing = yawIntended - marioAngle;
            float inputScaledMagnitude = input.GetScaledMagnitude();

            float perpSpeed = 0;
            float newHSpeed = ApproachHSpeed(initialState.HSpeed, 0, 0.35f, 0.35f);
            if (inputScaledMagnitude > 0)
            {
                newHSpeed += (inputScaledMagnitude / 32) * 1.5f * InGameTrigUtilities.InGameCosine(deltaAngleIntendedFacing);
                perpSpeed = InGameTrigUtilities.InGameSine(deltaAngleIntendedFacing) * (inputScaledMagnitude / 32) * 10;
            }

            if (newHSpeed > maxSpeed) newHSpeed -= 1;
            if (newHSpeed < -16) newHSpeed += 2;

            float newSlidingXSpeed = InGameTrigUtilities.InGameSine(marioAngle) * newHSpeed;
            float newSlidingZSpeed = InGameTrigUtilities.InGameCosine(marioAngle) * newHSpeed;
            newSlidingXSpeed += perpSpeed * InGameTrigUtilities.InGameSine(marioAngle + 0x4000);
            newSlidingZSpeed += perpSpeed * InGameTrigUtilities.InGameCosine(marioAngle + 0x4000);
            float newXSpeed = newSlidingXSpeed;
            float newZSpeed = newSlidingZSpeed;

            return new MarioState(
                initialState.X,
                initialState.Y,
                initialState.Z,
                newXSpeed,
                initialState.YSpeed,
                newZSpeed,
                newHSpeed,
                initialState.MarioAngle,
                initialState.CameraAngle,
                initialState,
                input,
                initialState.Index + 1);
        }

        private static float ComputeAirHSpeed(float initialHSpeed)
        {
            int maxSpeed = 32;
            float newHSpeed = ApproachHSpeed(initialHSpeed, 0, 0.35f, 0.35f);
            if (newHSpeed > maxSpeed) newHSpeed -= 1;
            if (newHSpeed < -16) newHSpeed += 2;
            return newHSpeed;
        }

        private static float ComputePosition(float position, float hSpeed, int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                hSpeed = ComputeAirHSpeed(hSpeed);
                position += hSpeed;
            }
            return position;
        }

        private static MarioState ComputeAirYSpeed(MarioState initialState)
        {
            float newYSpeed = Math.Max(initialState.YSpeed - 4, -75);
            return new MarioState(
                initialState.X,
                initialState.Y,
                initialState.Z,
                initialState.XSpeed,
                newYSpeed,
                initialState.ZSpeed,
                initialState.HSpeed,
                initialState.MarioAngle,
                initialState.CameraAngle,
                initialState.PreviousState,
                initialState.LastInput,
                initialState.Index);
        }

        private static float ApproachHSpeed(float speed, float maxSpeed, float increase, float decrease)
        {
            if (speed < maxSpeed)
                return Math.Min(maxSpeed, speed + increase);
            else
                return Math.Max(maxSpeed, speed - decrease);
        }

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
            List<Input> inputs = GetInputRange(xInput - xRadius, xInput + xRadius, zInput - zRadius, zInput + zRadius);
            float bestDiff = float.MaxValue;
            MarioState bestState = null;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => dequeue.ApplyInput(input));
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
            List<Input> inputs = GetInputRange(xInput - xRadius, xInput + xRadius, zInput - zRadius, zInput + zRadius);
            float bestDiff = float.MaxValue;
            MarioState bestState = null;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => dequeue.ApplyInput(input));
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
            List<Input> inputs = GetInputRange(-70, 70, 0, 0);
            float bestDiff = float.MaxValue;
            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => dequeue.ApplyInput(input));
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
            List<Input> inputs = GetInputRange(0, 0, -38 - inputRadius, -38 + inputRadius);

            float bestDiff = float.MaxValue;
            MarioState bestState = null;

            Queue<MarioState> queue = new Queue<MarioState>();
            HashSet<MarioState> alreadySeen = new HashSet<MarioState>();
            queue.Enqueue(startState);
            alreadySeen.Add(startState);

            while (queue.Count != 0)
            {
                MarioState dequeue = queue.Dequeue();
                List<MarioState> nextStates = inputs.ConvertAll(input => dequeue.ApplyInput(input));
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
                    float expectedX = ComputePosition(state.X, state.XSpeed, numFramesRemaining);
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

        private static List<Input> GetAllInputs()
        {
            return GetInputRange(-128, 127, -128, 127);
        }

        private static List<Input> GetInputRange(int minX, int maxX, int minZ, int maxZ)
        {
            List<Input> output = new List<Input>();
            for (int x = minX; x <= maxX; x++)
            {
                if (MoreMath.InputIsInDeadZone(x)) continue;
                for (int z = minZ; z <= maxZ; z++)
                {
                    if (MoreMath.InputIsInDeadZone(z)) continue;
                    output.Add(new Input(x, z));
                }
            }
            return output;
        }
    }
}
