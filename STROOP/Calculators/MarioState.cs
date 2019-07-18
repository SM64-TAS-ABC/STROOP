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
    public class MarioState
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

        public MutableMarioState GetMutableMarioState(Input input)
        {
            return new MutableMarioState(
                X, Y, Z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                MarioAngle, CameraAngle, input);
        }

        public override string ToString()
        {
            return String.Format(
                "pos=({0},{1},{2}) spd=({3},{4},{5}) hspd={6} angle={7}",
                (double)X, (double)Y, (double)Z,
                (double)XSpeed, (double)YSpeed, (double)ZSpeed, (double)HSpeed, MarioAngle);
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
                return PreviousState.GetLineage() + "\r\n" + ToStringWithInput();
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

        public MarioState WithPosition(float x, float y, float z)
        {
            return new MarioState(
                x, y, z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                MarioAngle, CameraAngle,
                PreviousState, LastInput, Index);
        }
    }
}
