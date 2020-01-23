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
    public class MutableMarioState
    {
        public float X;
        public float Y;
        public float Z;
        public float XSpeed;
        public float YSpeed;
        public float ZSpeed;
        public float HSpeed;
        public float SlidingSpeedX;
        public float SlidingSpeedZ;
        public ushort SlidingAngle;
        public ushort MarioAngle;
        public ushort IntendedAngle;
        public float IntendedMagnitude;

        public MutableMarioState(
            float x,
            float y,
            float z,
            float xSpeed,
            float ySpeed,
            float zSpeed,
            float hSpeed,
            float slidingSpeedX,
            float slidingSpeedZ,
            ushort slidingAngle,
            ushort marioAngle,
            ushort cameraAngle,
            Input input)
        {
            X = x;
            Y = y;
            Z = z;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            ZSpeed = zSpeed;
            HSpeed = hSpeed;
            SlidingSpeedX = slidingSpeedX;
            SlidingSpeedZ = slidingSpeedZ;
            SlidingAngle = slidingAngle;
            MarioAngle = marioAngle;
            IntendedAngle = MoreMath.CalculateAngleFromInputs(input.X, input.Y, cameraAngle);
            IntendedMagnitude = input.GetScaledMagnitude();
        }

        public MutableMarioState(
            float x,
            float y,
            float z,
            float xSpeed,
            float ySpeed,
            float zSpeed,
            float hSpeed,
            float slidingSpeedX,
            float slidingSpeedZ,
            ushort slidingAngle,
            ushort marioAngle,
            int angleDiff)
        {
            X = x;
            Y = y;
            Z = z;
            XSpeed = xSpeed;
            YSpeed = ySpeed;
            ZSpeed = zSpeed;
            HSpeed = hSpeed;
            SlidingSpeedX = slidingSpeedX;
            SlidingSpeedZ = slidingSpeedZ;
            SlidingAngle = slidingAngle;
            MarioAngle = marioAngle;
            IntendedAngle = MoreMath.NormalizeAngleUshort(marioAngle + angleDiff);
            IntendedMagnitude = 32;
        }

        public MarioState GetMarioState(MarioState previousState, Input lastInput)
        {
            return new MarioState(
                X, Y, Z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
                MarioAngle, previousState.CameraAngle,
                previousState, lastInput, previousState.Index + 1);
        }
    }
}
