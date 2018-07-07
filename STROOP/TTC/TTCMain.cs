using STROOP.Forms;
using STROOP.Structs;
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
            TtcSimulation simulation = new TtcSimulation(0, 4000);
            string result = simulation.GetObjectsString((int)GotoRetrieveConfig.GotoAboveOffset);
            Config.Print(result);
        }

        public static void TtcMainMethod3()
        {
            List<int> dustFrames = FindIdealPendulumManipulation(0x8033E788);
            if (dustFrames == null) return;
            List<int> dustInputFrames = dustFrames.ConvertAll(dustFrame => dustFrame - 2);
            string dustInputFramesString = "[" + String.Join(", ", dustInputFrames) + "]";
            Config.Print(dustInputFramesString);
        }

        public static List<int> FindIdealPendulumManipulation(uint pendulumAddress)
        {
            List<List<int>> dustFrameLists = GetDustFrameLists(MupenUtilities.GetFrameCount() + 2, 25, 25);
            foreach (List<int> dustFrames in dustFrameLists)
            {
                TtcSimulation simulation = new TtcSimulation(dustFrames);
                bool success = simulation.FindIdealPendulumManipulation(pendulumAddress);
                if (success)
                {
                    return dustFrames;
                }
            }
            return null;
        }

        public static void TtcMainMethod2()
        {
            int earliestDustFrame = 901 + 2 + 0;
            int dustFrameRange = 65;
            int maxDustFrames = 5;

            int numFramesMin = 120;
            int numFramesMax = 10000;

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

        public static string Simulate(int endFrame, List<int> dustFrames = null)
        {
            TtcSimulation simulation = new TtcSimulation(dustFrames);
            return simulation.GetObjectsString(endFrame);
        }

        private static List<List<int>> GetDustFrameLists(int earliestDustFrame, int dustFrameRange, int maxDustFrames)
        {
            List<List<int>> dustFrameLists = new List<List<int>>();
            for (int numDustFrames = 0; numDustFrames <= maxDustFrames; numDustFrames++)
            {
                AddDustFrameListRecursion(new bool[dustFrameRange], 0, 0, numDustFrames, dustFrameLists, earliestDustFrame);
            }
            return dustFrameLists;
        }

        private static void AddDustFrameListRecursion(
            bool[] bools, int index, int numDustFrames, int exactDustFrames,
            List<List<int>> dustFrameLists, int earliestDustFrame)
        {
            // ending condition
            if (index == bools.Length)
            {
                if (numDustFrames == exactDustFrames)
                {
                    dustFrameLists.Add(ConvertBoolsToDustFrames(bools, earliestDustFrame));
                }
                return;
            }

            // true case
            bool precedingIsDust = index > 0 && bools[index - 1];
            bool lessThanExactDusts = numDustFrames < exactDustFrames;
            if (!precedingIsDust && lessThanExactDusts)
            {
                bools[index] = true;
                AddDustFrameListRecursion(bools, index + 1, numDustFrames + 1, exactDustFrames, dustFrameLists, earliestDustFrame);
            }

            // false case
            int numRemainingBools = bools.Length - index;
            int dustFramesToGo = exactDustFrames - numDustFrames;
            bool canAffordFalse = numRemainingBools > dustFramesToGo;
            if (canAffordFalse)
            {
                bools[index] = false;
                AddDustFrameListRecursion(bools, index + 1, numDustFrames, exactDustFrames, dustFrameLists, earliestDustFrame);
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
