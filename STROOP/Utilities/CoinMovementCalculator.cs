using STROOP.Models;
using STROOP.Structs.Configurations;
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
            float lakituX = -1332.55432128906f;

            float lakituYCenter = -2657.80029296875f;
            float lakituYRadius = 200;
            float lakituYEntries = 150;

            float lakituZCenter = 4721.93310546875f;
            float lakituZRadius = 200;
            float lakituZEntries = 150;

            float lakituYMin = lakituYCenter - lakituYRadius;
            float lakituYMax = lakituYCenter + lakituYRadius;
            float lakituYInc = (lakituYMax - lakituYMin) / lakituYEntries;

            float lakituZMin = lakituZCenter - lakituZRadius;
            float lakituZMax = lakituZCenter + lakituZRadius;
            float lakituZInc = (lakituZMax - lakituZMin) / lakituZEntries;

            Config.Print("START");
            for (float lakituY = lakituYMin; lakituY <= lakituYMax; lakituY += lakituYInc)
            {
                for (float lakituZ = lakituZMin; lakituZ <= lakituZMax; lakituZ += lakituZInc)
                {
                    for (int rngIndex = 0; rngIndex < 65114; rngIndex++)
                    {
                        int numCoinsPassingThroughFloor =
                            CalculateNumCoinsPassingThroughFloor(
                                lakituX, lakituY, lakituZ, rngIndex);
                        if (numCoinsPassingThroughFloor > 2)
                        {
                            Config.Print(
                                "{0} coins for rng index {1}, Ly = {2}, Lz = {3}",
                                numCoinsPassingThroughFloor, rngIndex, lakituY, lakituZ);
                        }
                    }
                }
            }
            Config.Print("END");
        }

        public static int CalculateNumCoinsPassingThroughFloor(
            float lakituX, float lakituY, float lakituZ, int rngIndex)
        {
            int numCoinsPassingThroughFloor = 0;

            List<CoinState> prevStates = GetCoinStates(rngIndex, lakituX, lakituY, lakituZ);
            List<CoinState> states;
            for (int i = 0; i < 50; i++)
            {
                states = prevStates.ConvertAll(prevState => prevState.GetNextState());
                for (int j = 0; j < states.Count; j++)
                {
                    CoinState prevState = prevStates[j];
                    CoinState state = states[j];
                    bool justEnteredWater = GetBelowWater(state) && !GetBelowWater(prevState);
                    bool passedThroughFloor =
                        GetFloorRelativity(prevState) == FloorRelativity.ABOVE &&
                        GetFloorRelativity(state) == FloorRelativity.BELOW;
                    if (justEnteredWater && passedThroughFloor)
                    {
                        numCoinsPassingThroughFloor++;
                    }
                }
                prevStates = states;
            }

            return numCoinsPassingThroughFloor;
        }

        public enum FloorRelativity { ABOVE, INSIDE, BELOW };

        public static List<CoinState> GetCoinStates(int rngIndex, float x, float y, float z)
        {
            int offset = 91;
            CoinObject lakitu = CoinObject.Lakitu;
            List<CoinTrajectory> trajectories = lakitu.CalculateCoinTrajectories(rngIndex + offset);
            List<CoinState> states = trajectories.ConvertAll(trajectory =>
            {
                (double xSpeed, double zSpeed) = MoreMath.GetComponentsFromVector(
                    trajectory.HSpeed, MoreMath.TruncateToMultipleOf16(trajectory.Angle));
                return new CoinState(
                    x, y, z,
                    (float)xSpeed, (float)trajectory.VSpeed, (float)zSpeed,
                    (float)trajectory.HSpeed, trajectory.Angle);
            });
            return states;
        }

        public static bool JustEnteredWater(CoinState coinState)
        {
            int waterLevel = -3071;
            if (coinState.Y > waterLevel) return false;
            float prevHeight = coinState.Y - coinState.YSpeed;
            if (prevHeight < waterLevel) return false;
            return true;
        }

        public static bool GetBelowWater(CoinState coinState)
        {
            int waterLevel = -3071;
            return coinState.Y < waterLevel;
        }

        public static FloorRelativity GetFloorRelativity(CoinState coinState)
        {
            float heightOnTriangle = GetHeightOnTriangle(coinState.X, coinState.Z);
            float heightDiff = coinState.Y - heightOnTriangle;
            if (heightDiff > 0) return FloorRelativity.ABOVE;
            if (heightDiff < -78) return FloorRelativity.BELOW;
            return FloorRelativity.INSIDE;
        }

        public static float GetHeightOnTriangle(float x, float z)
        {
            float normX = -0.0339056774973869f;
            float normY = 0.919187009334564f;
            float normZ = 0.392358928918839f;
            float normOffset = 969.59228515625f;

            x = (int)x;
            z = (int)z;

            return (float)TriangleDataModel.GetHeightOnTriangle(
                x, z, normX, normY, normZ, normOffset);
        }

        public class CoinState
        {
            public readonly float X;
            public readonly float Y;
            public readonly float Z;
            public readonly float XSpeed;
            public readonly float YSpeed;
            public readonly float ZSpeed;
            public readonly float HSpeed;
            public readonly ushort Angle;

            public CoinState(
                float x, float y, float z,
                float xSpeed, float ySpeed, float zSpeed,
                float hSpeed, ushort angle)
            {
                X = x;
                Y = y;
                Z = z;
                XSpeed = xSpeed;
                YSpeed = ySpeed;
                ZSpeed = zSpeed;
                HSpeed = hSpeed;
                Angle = angle;
            }

            public CoinState GetNextState()
            {
                float newXSpeed = (float)MoreMath.MoveNumberTowards(XSpeed, 0, 0.001f * XSpeed * XSpeed);
                float newZSpeed = (float)MoreMath.MoveNumberTowards(ZSpeed, 0, 0.001f * ZSpeed * ZSpeed);
                float newHSpeed = (float)Math.Sqrt(newXSpeed * newXSpeed + newZSpeed * newZSpeed);
                float newYSpeed = Math.Max(-78, YSpeed - 4);

                float newX = X + newXSpeed;
                float newZ = Z + newZSpeed;
                float newY = Y + newYSpeed;

                ushort newAngle = Angle;

                (double newXSpeed2, double newZSpeed2) = MoreMath.GetComponentsFromVector(
                    newHSpeed, MoreMath.TruncateToMultipleOf16(newAngle));
                newXSpeed = (float) newXSpeed2;
                newZSpeed = (float) newZSpeed2;

                return new CoinState(
                    newX, newY, newZ,
                    newXSpeed, newYSpeed, newZSpeed,
                    newHSpeed, newAngle);
            }

            public override string ToString()
            {
                return String.Format(
                    "pos=({0},{1},{2}) vel=({3},{4},{5}) hspd={6} angle={7}",
                    X, Y, Z, XSpeed, YSpeed, ZSpeed, HSpeed, Angle);
            }
        }
    }
}
