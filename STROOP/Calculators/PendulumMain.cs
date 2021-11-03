using Accord.Collections;
using STROOP.Forms;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Ttc;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class PendulumMain
    {
        public static void Test()
        {
            TtcFloatPendulum initialPendulum = new TtcFloatPendulum(null);
            for (int i = 0; i <= 288; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(i % 2 == 0);
                Config.Print(i + ": " + (int)initialPendulum._angle + " " + swungThroughZero);
            }
            for (int i = 0; true; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(true);
                Config.Print(i + ": " + (int)initialPendulum._angle + " " + swungThroughZero);
                if (initialPendulum._angle == 33578192) break;
            }

            HashSet<TtcFloatPendulum> alreadySeen = new HashSet<TtcFloatPendulum>();
            PriorityQueue<TtcFloatPendulum> queue = new PriorityQueue<TtcFloatPendulum>(capacity: 2_000_000);
            alreadySeen.Add(initialPendulum);
            queue.Enqueue(initialPendulum, initialPendulum._fitness);

            while (queue.Count > 0)
            {
                TtcFloatPendulum dequeue = queue.Dequeue().Value;

                TtcFloatPendulum fast = (TtcFloatPendulum)dequeue.Clone(null);
                bool swungThroughZeroFast = fast.PerformSwing(true);
                fast._predecessor = dequeue;
                fast._fitness = swungThroughZeroFast ? GetFitness(fast._angle) : dequeue._fitness + 1;
                fast._pathLength = dequeue._pathLength + 1;
                if (swungThroughZeroFast)
                {
                    Config.Print("swungThroughZeroFast {0} {1} {2} {3}", fast._angle, fast._fitness, fast._pathLength, queue.Count);
                }
                if (Math.Abs(fast._angle) > int.MaxValue)
                {
                    Config.Print(GetLineage(fast));
                    return;
                }

                TtcFloatPendulum slow = (TtcFloatPendulum)dequeue.Clone(null);
                bool swungThroughZeroSlow = slow.PerformSwing(false);
                slow._predecessor = dequeue;
                slow._fitness = swungThroughZeroSlow ? GetFitness(slow._angle) : dequeue._fitness + 1;
                slow._pathLength = dequeue._pathLength + 1;
                if (swungThroughZeroSlow)
                {
                    Config.Print("swungThroughZeroSlow {0} {1} {2} {3}", slow._angle, slow._fitness, slow._pathLength, queue.Count);
                }
                if (Math.Abs(slow._angle) > int.MaxValue)
                {
                    Config.Print(GetLineage(slow));
                    return;
                }

                if (!alreadySeen.Contains(fast))
                {
                    alreadySeen.Add(fast);
                    queue.Enqueue(fast, fast._fitness);
                }

                if (!alreadySeen.Contains(slow))
                {
                    alreadySeen.Add(slow);
                    queue.Enqueue(slow, slow._fitness);
                }

                MaybePruneQueue(queue);
            }
        }

        public static void Test2()
        {
            TtcFloatPendulum initialPendulum = new TtcFloatPendulum(null);
            for (int i = 0; i <= 288; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(i % 2 == 0);
                Config.Print(i + ": " + StringUtilities.FormatIntegerWithSign((int)initialPendulum._angle) + " " + swungThroughZero);
            }
            for (int i = 0; true; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(true);
                Config.Print(i + ": " + StringUtilities.FormatIntegerWithSign((int)initialPendulum._angle) + " " + swungThroughZero);
                if (initialPendulum._angle == 33578192) break;
            }

            int numSaved = 5;
            PendulumPossibility initialPossibility = new PendulumPossibility(initialPendulum, new List<bool>(), null);
            List<PendulumPossibility> bestPossibilities = new List<PendulumPossibility>() { initialPossibility };
            for (int i = 0; true; i++)
            {
                List<PendulumPossibility> nextPossilbiites = bestPossibilities.ConvertAll(
                    possibility => GetNextPendulumPossibilities(possibility)).SelectMany(list => list).ToList();
                bestPossibilities.Clear();
                bestPossibilities.AddRange(GetBestPossibilities(nextPossilbiites, numSaved));

                if (Math.Abs(bestPossibilities[0].Pendulum._angle) > int.MaxValue) break;

                for (int j = 0; j < numSaved; j++)
                {
                    float angle = bestPossibilities[j].Pendulum._angle;
                    Config.Print("{0}: {1} {2}", i, StringUtilities.FormatIntegerWithSign((int)angle), angle / int.MaxValue);
                }
            }
            List<bool> totalBoolPermutation = GetTotalBoolPermutation(bestPossibilities[0]);
            Config.Print(string.Join("\r\n", totalBoolPermutation));
        }

        public static void Test3()
        {
            TtcFloatPendulum initialPendulum = new TtcFloatPendulum(null);
            for (int i = 0; i <= 288; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(i % 2 == 0);
                Config.Print(i + ": " + StringUtilities.FormatIntegerWithSign((int)initialPendulum._angle) + " " + swungThroughZero);
            }
            for (int i = 0; true; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(true);
                Config.Print(i + ": " + StringUtilities.FormatIntegerWithSign((int)initialPendulum._angle) + " " + swungThroughZero);
                if (initialPendulum._angle == 33578192) break;
            }

            TtcFloatPendulum testPendulum = (TtcFloatPendulum)initialPendulum.Clone(null);
            TtcFloatPendulum controlPendulum = (TtcFloatPendulum)initialPendulum.Clone(null);

            List<int> exceptions = new List<int>() { 1013 };
            for (int i = 0; i < Solution.Count; i++)
            {
                bool b = Solution[i];
                testPendulum.PerformSwing(b);

                bool b2 = !exceptions.Contains(i);
                controlPendulum.PerformSwing(b2);
                
                Config.Print(i + ": " + StringUtilities.FormatIntegerWithSign((int)testPendulum._angle) + " " + StringUtilities.FormatIntegerWithSign((int)controlPendulum._angle));
            }
        }

        public static List<List<bool>> BoolPermuations = GetBools(10);

        public static List<PendulumPossibility> GetNextPendulumPossibilities(PendulumPossibility possibility)
        {
            List<PendulumPossibility> output = new List<PendulumPossibility>();
            foreach (List<bool> boolPermuation in BoolPermuations)
            {
                TtcFloatPendulum pendulum = (TtcFloatPendulum)possibility.Pendulum.Clone(null);
                foreach (bool b in boolPermuation)
                {
                    pendulum.PerformSwing(b);
                }
                PendulumPossibility nextPossibility = new PendulumPossibility(pendulum, boolPermuation, possibility);
                output.Add(nextPossibility);
            }
            return output;
        }

        public static List<PendulumPossibility> GetBestPossibilities(List<PendulumPossibility> possibilities, int count)
        {
            possibilities = Enumerable.OrderBy(possibilities, possiblity => Math.Abs(possiblity.Pendulum._angle)).ToList();
            possibilities.Reverse();
            return possibilities.Take(count).ToList();
        }

        public static List<bool> GetTotalBoolPermutation(PendulumPossibility possibility)
        {
            List<List<bool>> permutations = new List<List<bool>>();
            while (possibility != null)
            {
                permutations.Insert(0, possibility.BoolPermutation);
                possibility = possibility.Predecessor;
            }
            return permutations.SelectMany(list => list).ToList();
        }

        public class PendulumPossibility
        {
            public readonly TtcFloatPendulum Pendulum;
            public readonly List<bool> BoolPermutation;
            public readonly PendulumPossibility Predecessor;

            public PendulumPossibility(
                TtcFloatPendulum pendulum,
                List<bool> boolPermutation,
                PendulumPossibility predecessor)
            {
                Pendulum = pendulum;
                BoolPermutation = boolPermutation;
                Predecessor = predecessor;
            }
        }

        public static List<List<bool>> GetBools(int count)
        {
            return GetBools(count, new List<List<bool>>() { new List<bool>() });
        }

        public static List<List<bool>> GetBools(int count, List<List<bool>> list)
        {
            if (list[0].Count == count) return list;

            List<List<bool>> temp = new List<List<bool>>();
            foreach (List<bool> l in list)
            {
                temp.Add(l);
            }
            list.Clear();
            foreach (List<bool> l in temp)
            {
                List<bool> trueList = new List<bool>(l);
                List<bool> falseList = new List<bool>(l);
                trueList.Add(true);
                falseList.Add(false);
                list.Add(trueList);
                list.Add(falseList);
            }
            return GetBools(count, list);
        }

        public static int GetFitness(float angle)
        {
            float baseAngle = 3.373777E+07f;
            float ratio = Math.Abs(angle / baseAngle) - 1;
            return (int)(ratio * -500);
        }

        public static string GetLineage(TtcFloatPendulum pendulum)
        {
            List<string> lines = new List<string>();
            while (pendulum != null)
            {
                lines.Insert(0, string.Format(
                    "{0}\t{1}\t{2}",
                    pendulum._accelerationMagnitude, (int)pendulum._angle, pendulum));
                pendulum = pendulum._predecessor;
            }
            return string.Join("\r\n", lines);
        }

        public static void MaybePruneQueue(PriorityQueue<TtcFloatPendulum> queue)
        {
            if (queue.Count < 1_000_000) return;
            Config.Print("PRUNING");

            List<PriorityQueueNode<TtcFloatPendulum>> saved =
                new List<PriorityQueueNode<TtcFloatPendulum>>();
            for (int i = 0; i < 100_000; i++)
            {
                PriorityQueueNode<TtcFloatPendulum> node = queue.Dequeue();
                saved.Add(node);
            }

            queue.Clear();
            foreach (var node in saved)
            {
                queue.Enqueue(node.Value, node.Priority);
            }
        }

        public static List<bool> Solution = CreateSolution();

        public static List<bool> CreateSolution()
        {
            List<bool> output = new List<bool>();
            for (int i = 0; i < 8860; i++)
            {
                output.Add(true);
            }
            output[725] = false;
            output[3999] = false;
            output[5723] = false;
            output[6183] = false;
            output[7966] = false;
            return output;
        }
    }
}
