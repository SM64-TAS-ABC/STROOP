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
            MarioAngle = marioAngle;
            IntendedAngle = MoreMath.CalculateAngleFromInputs(input.X, input.Y, cameraAngle);
            IntendedMagnitude = input.GetScaledMagnitude();
        }

        public MarioState GetMarioState(MarioState previousState, Input lastInput)
        {
            return new MarioState(
                X, Y, Z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                MarioAngle, previousState.CameraAngle,
                previousState, lastInput, previousState.Index + 1);
        }
    }
}
