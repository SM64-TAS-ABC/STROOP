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
        public readonly float SlidingSpeedX;
        public readonly float SlidingSpeedZ;
        public readonly ushort SlidingAngle;
        public readonly ushort MarioAngle;
        public readonly ushort CameraAngle;

        public readonly MarioState PreviousState;
        public readonly Input LastInput;
        public readonly int Index;

        public MarioState(
            float x, float y, float z,
            float xSpeed, float ySpeed, float zSpeed, float hSpeed,
            float slidingSpeedX, float slidingSpeedZ, ushort slidingAngle,
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
            SlidingSpeedX = slidingSpeedX;
            SlidingSpeedZ = slidingSpeedZ;
            SlidingAngle = slidingAngle;
            MarioAngle = marioAngle;
            CameraAngle = cameraAngle;

            PreviousState = previousState;
            LastInput = lastInput;
            Index = index;
        }

        public static MarioState CreateMarioState()
        {
            return new MarioState(
                x: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset),
                y: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset),
                z: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset),
                xSpeed: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset),
                ySpeed: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset),
                zSpeed: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset),
                hSpeed: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset),
                slidingSpeedX: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset),
                slidingSpeedZ: Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset),
                slidingAngle: Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.SlidingYawOffset),
                marioAngle: Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset),
                cameraAngle: Config.Stream.GetUShort(CameraConfig.StructAddress + CameraConfig.CentripetalAngleOffset),
                previousState: null,
                lastInput: null,
                index: 0);
        }

        public MutableMarioState GetMutableMarioState(Input input)
        {
            return new MutableMarioState(
                X, Y, Z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
                MarioAngle, CameraAngle, input);
        }

        public MutableMarioState GetMutableMarioState(int angleDiff)
        {
            return new MutableMarioState(
                X, Y, Z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
                MarioAngle, angleDiff);
        }

        public override string ToString()
        {
            return String.Format(
                "pos=({0},{1},{2}) spd=({3},{4},{5}) slide=({6},{7}) slideAngle={8} hspd={9} angle={10}",
                (double)X, (double)Y, (double)Z,
                (double)XSpeed, (double)YSpeed, (double)ZSpeed,
                (double)SlidingSpeedX, (double)SlidingSpeedZ, SlidingAngle,
                (double)HSpeed, MarioAngle);
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
                    SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
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
                SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
                MarioAngle, cameraAngle,
                PreviousState, LastInput, Index);
        }

        public MarioState WithPosition(float x, float y, float z)
        {
            return new MarioState(
                x, y, z,
                XSpeed, YSpeed, ZSpeed, HSpeed,
                SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
                MarioAngle, CameraAngle,
                PreviousState, LastInput, Index);
        }

        public MarioState WithDive()
        {
            return new MarioState(
                X, Y, Z,
                XSpeed, YSpeed, ZSpeed, HSpeed + 15,
                SlidingSpeedX, SlidingSpeedZ, SlidingAngle,
                MarioAngle, CameraAngle,
                PreviousState, LastInput, Index);
        }
    }
}
