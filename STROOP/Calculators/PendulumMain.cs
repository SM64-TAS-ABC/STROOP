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
            Queue<TtcFloatPendulum> queue = new Queue<TtcFloatPendulum>();

            alreadySeen.Add(initialPendulum);
            queue.Enqueue(initialPendulum);

            while (queue.Count > 0)
            {
                TtcFloatPendulum dequeue = queue.Dequeue();

                TtcFloatPendulum fast = (TtcFloatPendulum)dequeue.Clone(null);
                fast._predecessor = dequeue;
                TtcFloatPendulum slow = (TtcFloatPendulum)dequeue.Clone(null);
                slow._predecessor = dequeue;

                bool swungThroughZeroFast = fast.PerformSwing(true);
                bool swungThroughZeroSlow = slow.PerformSwing(false);

                if (swungThroughZeroFast)
                {
                    queue.Clear();
                    queue.Enqueue(fast);
                    Config.Print("RESET " + fast._angle);
                }
                else
                {
                    if (!alreadySeen.Contains(fast))
                    {
                        alreadySeen.Add(fast);
                        queue.Enqueue(fast);
                    }

                    if (!alreadySeen.Contains(slow))
                    {
                        alreadySeen.Add(slow);
                        queue.Enqueue(slow);
                    }
                }
            }
        }
    }
}
