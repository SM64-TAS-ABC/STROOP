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
                InfoForm.ShowValue(output, "Pendulum Manipulation", "Dust Frames");
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

        public static Dictionary<int, List<int>> FindDualPendulumManipulation(int numIterations)
        {
            TtcSaveState currentSaveState = new TtcSaveState();
            int currentStartFrame = MupenUtilities.GetFrameCount();


            Dictionary<int, List<int>> dustFrameLists = new Dictionary<int, List<int>>();
            for (int i = 0; i < numIterations; i++)
            {
                (bool success, TtcSaveState saveState, int relativeEndFrame, List<int> relativeDustFrames) =
                    FindDualPendulumManipulation(currentSaveState);
                if (!success) break;

                List<int> absoluteDustFrames = relativeDustFrames.ConvertAll(rel => rel + currentStartFrame - 2);
                dustFrameLists[currentStartFrame] = absoluteDustFrames;
                Config.Print(currentStartFrame + ":\t[" + string.Join(",", absoluteDustFrames) + "]");

                currentSaveState = saveState;
                currentStartFrame += relativeEndFrame;
            }
            return dustFrameLists;
        }

        public static (bool success, TtcSaveState savestate, int endFrame, List<int> dustFrames)
            FindDualPendulumManipulation(TtcSaveState saveState)
        {
            List<List<int>> dustFrameLists = GetDustFrameLists(2, 25, 25);
            foreach (List<int> dustFrames in dustFrameLists)
            {
                TtcSimulation simulation = new TtcSimulation(saveState);
                simulation.AddDustFrames(dustFrames);
                (bool success, TtcSaveState savestate, int endFrame) = simulation.FindDualPendulumManipulation();
                if (success)
                {
                    return (success, savestate, endFrame, dustFrames);
                }
            }
            return (false, null, 0, null);
        }

        public static void FindIdealHandManipulation()
        {
            HandManipulationProgress startingProgress =
                new HandManipulationProgress(
                    new TtcSaveState(), MupenUtilities.GetFrameCount(), new List<int>());

            Queue<HandManipulationProgress> queue = new Queue<HandManipulationProgress>();
            queue.Enqueue(startingProgress);

            while (queue.Count > 0)
            {
                HandManipulationProgress dequeue = queue.Dequeue();
                int handMovementFrame = dequeue.GetHandMovementFrame();
                if (handMovementFrame < 1000000) Config.Print(dequeue + " => " + handMovementFrame);

                List<HandManipulationProgress> successors = dequeue.GetSuccessors();
                successors.ForEach(successor => queue.Enqueue(successor));
            }
        }

        public class HandManipulationProgress
        {
            public readonly TtcSaveState SaveState;
            public readonly int Frame;
            public readonly List<int> DustFrames;

            public HandManipulationProgress(
                TtcSaveState saveState,
                int frame,
                List<int> dustFrames)
            {
                SaveState = saveState;
                Frame = frame;
                DustFrames = dustFrames;
            }

            public override string ToString()
            {
                return String.Join(",", DustFrames);
            }

            public int GetHandMovementFrame()
            {
                return TtcSimulation.FindHandMovement(SaveState, Frame);
            }

            public bool IsValid()
            {
                TtcSimulation simulation = new TtcSimulation(SaveState);
                bool isValid = simulation.GetClosePendulum()._accelerationMagnitude == 13;
                return isValid;
            }

            public List<HandManipulationProgress> GetSuccessors()
            {
                if (!IsValid()) return new List<HandManipulationProgress>();

                List<int> keyFrames = new TtcSimulation(
                    SaveState, Frame, new List<int>()).FindKeyHandFrames();
                int endingFrame = keyFrames[4];
                List<int> potentialDustFrames = new List<int>()
                {
                    keyFrames[0], keyFrames[1], keyFrames[2], keyFrames[3]
                };

                List<List<int>> dustFrameConfigurations =
                    ControlUtilities.GetSubsetsRanged<int>(potentialDustFrames, 0, 4);

                List<HandManipulationProgress> successors = new List<HandManipulationProgress>();
                foreach (List<int> dustFrameConfiguration in dustFrameConfigurations)
                {
                    TtcSimulation simulation = new TtcSimulation(SaveState, Frame, dustFrameConfiguration);
                    simulation.SimulateUntilFrame(endingFrame);
                    HandManipulationProgress progress = new HandManipulationProgress(
                        simulation.GetSaveState(), endingFrame, DustFrames.Concat(dustFrameConfiguration).ToList());
                    successors.Add(progress);
                }
                return successors;
            }
        }

        public static void FindIdealCogManipulation()
        {
            TtcSaveState saveState = new TtcSaveState();
            int startFrame = MupenUtilities.GetFrameCount();
            int earliestDustFrame = startFrame + 2;

            int dustFrameRange = 40;
            int maxDustFrames = 6;
            int minDustFrames = 0;

            int numFramesMin = 120;
            int numFramesMax = 7000;

            //int numFramesMin = 0;
            //int numFramesMax = 3000;

            List<List<int>> dustFrameLists = GetDustFrameLists(earliestDustFrame, dustFrameRange, maxDustFrames, minDustFrames);
            int counter = 0;
            List<string> outputStrings = new List<string>();
            Config.Print("Starting brute force...");
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

                TtcSimulation simulation = new TtcSimulation(saveState, startFrame, dustFrames);
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

        public static int FindHandMovement()
        {
            TtcSimulation simulation = new TtcSimulation();
            return simulation.FindHandMovement();
        }

        public static string Simulate(int endFrame, List<int> dustFrames = null)
        {
            TtcSimulation simulation = new TtcSimulation(dustFrames);
            return simulation.GetObjectsString(endFrame);
        }

        private static List<List<int>> GetDustFrameLists(int earliestDustFrame, int dustFrameRange, int maxDustFrames, int minDustFrames = 0)
        {
            List<List<int>> dustFrameLists = new List<List<int>>();
            for (int numDustFrames = minDustFrames; numDustFrames <= maxDustFrames; numDustFrames++)
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

        public static string FormatDustFrames(List<int> dustFrames)
        {
            List<int> dustFrameInputs = dustFrames.ConvertAll(dust => dust - 2);
            return "[" + string.Join(",", dustFrameInputs) + "]";
        }

        public static void FindIdealReentryManipulation()
        {
            TtcSaveState saveState = new TtcSaveState();
            int startingFrame = MupenUtilities.GetFrameCount();
            List<List<int>> dustFramesLists = GetDustFrameLists(startingFrame + 2, 25, 25);

            Config.Print("START FindIdealReentryManipulation");
            foreach (List<int> dustFrames in dustFramesLists)
            {
                TtcSimulation simulation = new TtcSimulation(saveState, startingFrame, dustFrames);
                simulation.FindIdealReentryManipulationGivenDustFrames(dustFrames);
            }
            Config.Print("END FindIdealReentryManipulation");
        }

        public static void TestReentryPhase1()
        {
            string savestateString = "000069000000130000007D00000090000000550000000E0000007D00000095000000550000006D00000055000000590000007D000000900000002D00000054000000FFFFFFFFAF12000016FFFFFF0D000000000000000100000067E4FFFFD6FFFFFF2A00000000000000FFFFFFFFFCA60000000000000D0000000000000001000000BBDBFFFF000000000D000000090000000000000032000000320000005A0000002B00000001000000000000000000000000000000B184000001000000000000000000000000000000B184000001000000000000000000000000000000B184000001000000000000000000000000000000B184000064000000000000000000000019000000010000004600000001000000270000006400000000000000030000000B0000000C0000000000000002000000210000006400000000000000000000003A000000370000000000000000000000130000006400000000000000030000003600000037000000000000000300000029000000010000001D00000001000000600000000C000000000000000300000001000000370000003C000000010000000000000064000000000000000300000007000000786D0000E0FCFFFF18FCFFFFC4DC0000A8FDFFFF50FBFFFFECBB000076FDFFFF00000000761C0000F401000018FCFFFF78820000CEFFFFFFB00400004E990000CEFFFFFF70FEFFFF60070000C201000070FEFFFF03010000F7FFFFFF010000001E0000001D00000090AB00000A00000090AB0000BCFBFFFF820000000A000000B84D000032000000B84D0000BCFBFFFF8F00000008000000D09B000001000000780000000900000028EB0000FFFFFFFF3C0000000C000000B8F8000001000000780000002C000000F8DE0000010000005A00000002000000C03A0000FFFFFFFF5A00000031000000C0210000FFFFFFFF5A0000001900000008510000FFFFFFFF780000007400000040CA0000010000005A0000003500000030740000010000003C0000002D00000078C7000001000000780000001A0000004095000001000000780000004B000000C87E000001000000780000003200000098C40000FFFFFFFF780000000E000000608F0000010000003C0000000F00000058DC00000A0000000CDC000034F3FFFFFE000000110000002C0F0000320000002C0F000034F3FFFF490000001F000000D439000032000000A035000034F3FFFF1A0000000C0000008067000032000000445C000034F3FFFF390000000300000030CC00003200000084C2000034F3FFFF7100000005000000F474000032000000F068000034F3FFFF5100000002000000CB02000006000000010000005A00000010000000EEFAFFFF0600000001000000B4000000840000000CBE0000580200000000000002000000000000000000000000000000B184000003000000000000000000000000000000B1840000B61900000000000019000000000000002700000002000000178A000002000000FF39000000000000000000000000000000000000";
            int startFrame = 41887;

            TtcSaveState savestate = new TtcSaveState(savestateString);
            TtcSimulation simulation = new TtcSimulation(savestate, startFrame, new List<int>());
            simulation.FindIdealReentryManipulationGivenFrame1(new List<int>(), startFrame);
        }

        public static void FindPendulumSyncingManipulation()
        {
            TtcSaveState saveState = new TtcSaveState();
            int startingFrame = MupenUtilities.GetFrameCount();
            List<List<int>> dustFramesLists = GetDustFrameLists(startingFrame + 2, 25, 25);

            Config.Print("START FindPendulumSyncingManipulation");
            foreach (List<int> dustFrames in dustFramesLists)
            {
                TtcSimulation simulation = new TtcSimulation(saveState, startingFrame, dustFrames);
                int? syncingFrame = simulation.FindPendulumSyncingManipulation();
                if (syncingFrame.HasValue)
                {
                    Config.Print(syncingFrame.Value + "\t" + FormatDustFrames(dustFrames));
                }
            }
            Config.Print("END FindPendulumSyncingManipulation");
        }

        public static void FindMovingBarManipulation()
        {
            TtcSaveState saveState = new TtcSaveState();
            int startingFrame = MupenUtilities.GetFrameCount();
            List<List<int>> dustFramesLists = GetDustFrameLists(startingFrame + 2, 25, 25);

            Config.Print("START FindMovingBarManipulation");
            foreach (List<int> dustFrames in dustFramesLists)
            {
                TtcSimulation simulation = new TtcSimulation(saveState, startingFrame, dustFrames);
                simulation.FindMovingBarManipulationGivenDustFrames(dustFrames);
            }
            Config.Print("END FindMovingBarManipulation");
        }
    }

}
