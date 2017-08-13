using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class FlyGuyDataTable
    {
        private static readonly int CycleSize = 64;

        public FlyGuyDataTable()
        {
            double[] relativeHeightOffsets = new double[CycleSize];
            double[] nextHeightDiffs = new double[CycleSize];

            for (int i = 0; i < CycleSize; i++)
            {

            }
        }

        public double GetNextHeightDiff(int oscillationTimer)
        {
            return 1;
        }

        public double GetHeightMin(int oscillationTimer)
        {
            return 2;
        }

        public double GetHeightMax(int oscillationTimer)
        {
            return 3;
        }
    }
}
