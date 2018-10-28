using STROOP.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class CoinMovementCalculator
    {
        public static void CalculateMovement()
        {
            int waterLevel = -3071;
            float normX = -0.0339056774973869f;
            float normY = 0.919187009334564f;
            float normZ = 0.392358928918839f;
            float normOffset = 969.59228515625f;



        }

        public class CoinState
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly ushort Angle;
            public readonly float VSpeed;
            public readonly float HSpeed;

            public CoinState(
                float x, float y, float z,
                ushort angle, float vSpeed, float hSpeed)
            {
                X = x;
                Y = y;
                Z = z;
                Angle = angle;
                VSpeed = vSpeed;
                HSpeed = hSpeed;
            }

            public CoinState GetNextState()
            {
                return null;
            }
        }
    }
}
