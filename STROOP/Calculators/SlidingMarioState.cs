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
    public class SlidingMarioState
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
        public uint Action;
        public TriangleDataModel Floor;
        public float FloorHeight;
        public TriangleDataModel Wall;
        public short TerrainType;
        public Input Input;
        public ushort IntendedAngle;
        public float IntendedMagnitude;

        public SlidingMarioState(
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
            uint action,
            TriangleDataModel floor,
            float floorHeight,
            TriangleDataModel wall,
            short terrainType,
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
            Action = action;
            Floor = floor;
            FloorHeight = floorHeight;
            Wall = wall;
            TerrainType = terrainType;
            Input = input;
            IntendedAngle = MoreMath.CalculateAngleFromInputs(input.X, input.Y, cameraAngle);
            IntendedMagnitude = input.GetScaledMagnitude();
        }
    }
}
