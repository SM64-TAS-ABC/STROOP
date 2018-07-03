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
            int earliestDustFrame = 229234;
            int dustFrameRange = 25;
            int numFramesToBruteForce = 1500;

            List<List<int>> dustFrameLists = GetDustFrameLists(earliestDustFrame, dustFrameRange);
            int counter = 0;
            foreach (List<int> dustFrames in dustFrameLists)
            {
                counter++;
                if (counter % 100 == 0)
                    StringUtilities.WriteLine("counter = {0} / {1}", counter, dustFrameLists.Count);

                TtcSimulation simulation = new TtcSimulation(dustFrames);
                int? idealCogConfigurationFrame = simulation.LookForIdealCogConfiguration(numFramesToBruteForce);
                if (idealCogConfigurationFrame.HasValue)
                {
                    string dustFramesString = "[" + String.Join(", ", dustFrames) + "]";
                    StringUtilities.WriteLine(dustFramesString + " => " + idealCogConfigurationFrame.Value);
                    StringUtilities.WriteLine("Success");
                    return;
                }
            }
            StringUtilities.WriteLine("Failure");
        }

        private static List<List<int>> GetDustFrameLists(int earliestDustFrame, int dustFrameRange)
        {
            List<List<int>> dustFrameLists = new List<List<int>>();
            AddDustFrameListRecursion(new bool[dustFrameRange], 0, dustFrameLists, earliestDustFrame);
            return dustFrameLists;
        }

        private static void AddDustFrameListRecursion(bool[] bools, int index, List<List<int>> dustFrameLists, int earliestDustFrame)
        {
            if (index == bools.Length)
            {
                dustFrameLists.Add(ConvertBoolsToDustFrames(bools, earliestDustFrame));
                return;
            }
            bools[index] = false;
            AddDustFrameListRecursion(bools, index + 1, dustFrameLists, earliestDustFrame);
            if (index == 0 || bools[index - 1] == false)
            {
                bools[index] = true;
                AddDustFrameListRecursion(bools, index + 1, dustFrameLists, earliestDustFrame);
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
