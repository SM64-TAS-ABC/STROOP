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
    public static class TtcMain
    {

        private static readonly string testString4050 = "987B550000000A000000550000000A000000190000000A0000007D0000000A000000050000000A0000002D0000000A0000007D000000050000007D000000050000000100000003EAFFFF4AFFFFFF0D000000000000000100000003EAFFFF4AFFFFFF0D000000000000000100000003EAFFFF4AFFFFFF0D000000000000000100000003EAFFFF4AFFFFFF0D0000000000000000000000CEFFFFFFCEFFFFFF5A0000003100000001000000000000000000000000000000320000000100000000000000000000000000000032000000010000000000000000000000000000003200000001000000000000000000000000000000320000003700000014000000000000003200000037000000000000000000000032000000370000003200000000000000320000003700000064000000000000003200000037000000000000000000000032000000370000000A0000000000000032000000370000000000000000000000320000003700000000000000000000003200000037000000000000000000000032000000370000001E0000000000000032000000370000000A000000000000003200000037000000140000000000000032000000AA370000C2010000200300007E1D0000EE0200005802000046BA00005E01000050FBFFFFC6DF0000D4FEFFFFE0FCFFFF807000003200000038FFFFFF8250000012FDFFFFB0040000FA120000A2FEFFFFE8030000B9FFFFFFF7FFFFFF01000000460000003100000010C700000A00000000000000BCFBFFFF0000000032000000BCFB00001E000000BCFB0000BCFBFFFFC90000000A000000781E0000010000007800000012000000A8E40000FFFFFFFF1E0000001200000030DF0000FFFFFFFF780000003100000060220000010000005A0000003100000030DF0000FFFFFFFF3C00000031000000A8E40000FFFFFFFF3C00000012000000781E0000010000007800000012000000781E0000010000005A0000001200000030DF0000FFFFFFFF3C0000003100000060220000010000003C0000003100000060220000010000003C0000003100000030DF0000FFFFFFFF3C0000003100000030DF0000FFFFFFFF5A0000003100000030DF0000FFFFFFFF3C0000003100000068E600001E00000068E6000034F3FFFF6B0000001B00000060DF0000320000009CD9000034F3FFFFA70000000A00000034F300003200000034F3000034F3FFFF2F0000002C00000094FC0000320000000000000034F3FFFF8A0000000D00000094FC00000A00000000000000CC0C00002F0000000D00000068E600003200000068E6000034F3FFFF2F0000001B000000BF02000006000000010000005A00000031000000DEFAFFFFFAFFFFFFFFFFFFFF5A0000003100000050E2000050FBFFFFE803000002000000000000000000000000000000320000000300000000000000000000000000000032000000CF19000000000000100000000100000008000000020000003A590000020000006F2B000000000000010000000000000001000000";

        // from game
        public static void TtcMainMethod()
        {
            TtcSimulation simulation = new TtcSimulation();
            string result = simulation.GetObjectsString(4100);
            Config.Print(result);
        }

        // from save state
        public static void TtcMainMethod4()
        {
            TtcSimulation simulation = new TtcSimulation(testString4050);
            string result = simulation.GetObjectsString(50);
            Config.Print(result);
        }

        // from start
        public static void TtcMainMethod5()
        {
            TtcSimulation simulation = new TtcSimulation(0, 4000);
            string result = simulation.GetObjectsString(4100);
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

        public static void PrintIdealPendulumManipulation(uint pendulumAddress, int numIterations, bool useForm)
        {
            List<List<int>> dustFrameLists = TtcMain.FindIdealPendulumManipulation(pendulumAddress, numIterations);
            List<string> outputList = dustFrameLists.ConvertAll(dustFrameList => "[" + String.Join(", ", dustFrameList) + "]");
            string output = String.Join("\r\n", outputList);
            if (useForm)
            {
                InfoForm.ShowValue("Pendulum Manipulation", "Dust Frames", output);
            }
            else
            {
                Config.Print(output);
            }
        }

        public static List<List<int>> FindIdealPendulumManipulation(uint pendulumAddress, int numIterations)
        {
            TtcSaveState currentSaveState = new TtcSaveState();
            int currentStartFrame = MupenUtilities.GetFrameCount();

            List<List<int>> dustFrameLists = new List<List<int>>();
            for (int i = 0; i < numIterations; i++)
            {
                (bool success, TtcSaveState saveState, int relativeEndFrame, List<int> relativeDustFrames) =
                    FindIdealPendulumManipulation(pendulumAddress, currentSaveState);
                if (!success) break;

                List<int> absoluteDustFrames = relativeDustFrames.ConvertAll(rel => rel + currentStartFrame - 2);
                dustFrameLists.Add(absoluteDustFrames);

                currentSaveState = saveState;
                currentStartFrame += relativeEndFrame;
            }
            return dustFrameLists;
        }

        public static (bool success, TtcSaveState savestate, int endFrame, List<int> dustFrames)
            FindIdealPendulumManipulation(uint pendulumAddress, TtcSaveState saveState)
        {
            List<List<int>> dustFrameLists = GetDustFrameLists(2, 25, 25);
            foreach (List<int> dustFrames in dustFrameLists)
            {
                TtcSimulation simulation = new TtcSimulation(saveState);
                simulation.AddDustFrames(dustFrames);
                (bool success, TtcSaveState savestate, int endFrame) = simulation.FindIdealPendulumManipulation(pendulumAddress);
                if (success)
                {
                    return (success, savestate, endFrame, dustFrames);
                }
            }
            return (false, null, 0, null);
        }

        public static List<int> FindIdealPendulumManipulation(uint pendulumAddress)
        {
            List<List<int>> dustFrameLists = GetDustFrameLists(MupenUtilities.GetFrameCount() + 2, 25, 25);
            foreach (List<int> dustFrames in dustFrameLists)
            {
                TtcSimulation simulation = new TtcSimulation(dustFrames);
                bool success = simulation.FindIdealPendulumManipulation(pendulumAddress).ToTuple().Item1;
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
