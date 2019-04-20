using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace STROOP.Utilities
{
    public static class FlyingUtilities
    {
        private class MarioFlyingState
        {
            public float Pos0;
            public float Pos1;
            public float Pos2;

            public float Vel0;
            public float Vel1;
            public float Vel2;
            public float ForwardVel;

            public float FaceAngle0;
            public float FaceAngle1;
            public float FaceAngle2;

            public float AngleVel0;
            public float AngleVel1;
            public float AngleVel2;

            private static float GetFloat(uint address)
            {
                return Config.Stream.GetSingle(MarioConfig.StructAddress + address);
            }

            private static float GetShort(uint address)
            {
                return Config.Stream.GetInt16(MarioConfig.StructAddress + address);
            }

            public MarioFlyingState()
            {
                Pos0 = GetFloat(0x3C);
                Pos1 = GetFloat(0x40);
                Pos2 = GetFloat(0x44);

                Vel0 = GetFloat(0x48);
                Vel1 = GetFloat(0x4C);
                Vel2 = GetFloat(0x50);
                ForwardVel = GetFloat(0x54);

                FaceAngle0 = GetShort(0x2C);
                FaceAngle1 = GetShort(0x2E);
                FaceAngle2 = GetShort(0x30);

                AngleVel0 = GetShort(0x32);
                AngleVel0 = GetShort(0x34);
                AngleVel0 = GetShort(0x36);
            }
        }


    }
}
