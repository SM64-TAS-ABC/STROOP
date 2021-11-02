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
                Config.Print(i + ": " + initialPendulum._angle + " " + swungThroughZero);
            }
            for (int i = 0; true; i++)
            {
                bool swungThroughZero = initialPendulum.PerformSwing(true);
                Config.Print(i + ": " + initialPendulum._angle + " " + swungThroughZero);
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
                if (Math.Abs(fast._angle) > 2E31)
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
                if (Math.Abs(slow._angle) > 2E31)
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
    }
}
