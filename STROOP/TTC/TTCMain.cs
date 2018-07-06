using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Ttc
{
    /** This class creates all the RNGObjects and updates them
     *  for an arbitrary number of frames. Hence, this class is
     *  the hub of the project and where all the testing
     *  is done.
     */
    public class TtcMain
    {

        public static void TtcMainMethod()
        {
            int earliestDustFrame = 901 + 2 + 0;
            int dustFrameRange = 65;
            int maxDustFrames = 5;

            int numFramesMin = 120;
            int numFramesMax = 3000;

            List<List<int>> dustFrameLists = GetDustFrameLists(earliestDustFrame, dustFrameRange, maxDustFrames);
            int counter = 0;
            List<string> outputStrings = new List<string>();
            foreach (List<int> dustFrames in dustFrameLists)
            {
                counter++;
                if (counter % 1000 == 0)
                {
                    double percent = Math.Round(100d * counter / dustFrameLists.Count, 1);
                    string percentString = percent.ToString("N1");
                    Config.Print(
                        "counter = {0} / {1} ({2}%)",
                        counter, dustFrameLists.Count, percentString);
                }

                TtcSimulation simulation = new TtcSimulation(dustFrames);
                int? idealCogConfigurationFrame = simulation.FindIdealCogConfiguration(numFramesMin, numFramesMax);
                if (idealCogConfigurationFrame.HasValue)
                {
                    List<int> dustInputFrames = dustFrames.ConvertAll(dustFrame => dustFrame - 2);
                    string dustInputFramesString = "[" + String.Join(", ", dustInputFrames) + "]";
                    string outputString = dustInputFramesString + " => " + idealCogConfigurationFrame.Value;
                    outputStrings.Add(outputString);
                    Config.Print(outputString);
                }
            }
            Config.Print("In total, there were {0} successes:", outputStrings.Count);
            outputStrings.ForEach(output => Config.Print(output));
        }

        public static string Simulate(int endFrame, List<int> dustFrames)
        {
            TtcSimulation simulation = new TtcSimulation(dustFrames);
            return simulation.GetObjectsString(endFrame);
        }

        private static List<List<int>> GetDustFrameLists(int earliestDustFrame, int dustFrameRange, int maxDustFrames)
        {
            List<List<int>> dustFrameLists = new List<List<int>>();
            AddDustFrameListRecursion(new bool[dustFrameRange], 0, 0, maxDustFrames, dustFrameLists, earliestDustFrame);
            return dustFrameLists;
        }

        private static void AddDustFrameListRecursion(
            bool[] bools, int index, int numDustFrames, int maxDustFrames,
            List<List<int>> dustFrameLists, int earliestDustFrame)
        {
            // ending condition
            if (index == bools.Length)
            {
                dustFrameLists.Add(ConvertBoolsToDustFrames(bools, earliestDustFrame));
                return;
            }

            // false case
            bools[index] = false;
            AddDustFrameListRecursion(bools, index + 1, numDustFrames, maxDustFrames, dustFrameLists, earliestDustFrame);

            // true case
            bool precedingIsDust = index > 0 && bools[index - 1];
            bool lessThanMaxDusts = numDustFrames < maxDustFrames;
            if (!precedingIsDust && lessThanMaxDusts)
            {
                bools[index] = true;
                AddDustFrameListRecursion(bools, index + 1, numDustFrames + 1, maxDustFrames, dustFrameLists, earliestDustFrame);
            }
        }

        private static List<int> ConvertBoolsToDustFrames(bool[] bools, int earliestDustFrame)
        {
            List<int> dustFrames = new List<int>();
            for (int i = 0; i < bools.Length; i++)
            {
                if (bools[i]) dustFrames.Add(earliestDustFrame + i);
            }
            return dustFrames;
        }
    }

}
