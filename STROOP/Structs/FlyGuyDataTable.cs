using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class FlyGuyDataTable
    {
        private static readonly int CycleSize = 64;
        private static readonly int CycleStart = 48;
        private static readonly double Amplitude = 1.5;

        double[] relativeHeights;
        double[] prevHeightDiffs;
        double maxRelativeHeight;

        public FlyGuyDataTable()
        {
            relativeHeights = new double[CycleSize];
            prevHeightDiffs = new double[CycleSize];

            double prevHeightDiff = 0;
            double relativeHeight = 0;
            for (int i = 0; i < CycleSize; i++)
            {
                int cycleIndex = (CycleStart + i) % CycleSize;
                double radians = (cycleIndex / (double)CycleSize) * 2 * Math.PI;
                prevHeightDiff = Math.Cos(radians) * Amplitude;
                prevHeightDiffs[cycleIndex] = prevHeightDiff;
                relativeHeight += prevHeightDiff;
                relativeHeights[cycleIndex] = relativeHeight;
            }

            maxRelativeHeight = relativeHeights.Max();
        }

        public double GetRelativeHeight(int oscillationTimer)
        {
            oscillationTimer = NormalizeIndex(oscillationTimer);
            return relativeHeights[oscillationTimer];
        }

        public double GetPrevHeightDiff(int oscillationTimer)
        {
            oscillationTimer = NormalizeIndex(oscillationTimer);
            return prevHeightDiffs[oscillationTimer];
        }

        public double GetNextHeightDiff(int oscillationTimer)
        {
            oscillationTimer = NormalizeIndex(oscillationTimer + 1);
            return prevHeightDiffs[oscillationTimer];
        }

        public double GetMinHeight(int oscillationTimer, double currentHeight)
        {
            return currentHeight - GetRelativeHeight(oscillationTimer);
        }

        public double GetMaxHeight(int oscillationTimer, double currentHeight)
        {
            return GetMinHeight(oscillationTimer, currentHeight) + maxRelativeHeight;
        }

        private int NormalizeIndex(int index)
        {
            return MoreMath.NonNegativeModulus(index, CycleSize);
        }
    }
}
