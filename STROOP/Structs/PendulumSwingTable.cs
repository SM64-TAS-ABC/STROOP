using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class PendulumSwingTable
    {
        public struct PendulumSwingReference
        {
            public int Amplitude;
            public int Index;

            public override int GetHashCode()
            {
                return Amplitude;
            }
        }

        Dictionary<int, PendulumSwingReference> _amplitudeDictionary = new Dictionary<int, PendulumSwingReference>();
        Dictionary<int, PendulumSwingReference> _indexDictionary = new Dictionary<int, PendulumSwingReference>();

        public PendulumSwingTable()
        {
        }

        public void Add(PendulumSwingReference pendulumSwingRef)
        {
            _amplitudeDictionary.Add(pendulumSwingRef.Amplitude, pendulumSwingRef);
            _indexDictionary.Add(pendulumSwingRef.Index, pendulumSwingRef);
        }

        public int? GetPendulumSwingIndex(int amplitude)
        {
            if (_amplitudeDictionary.ContainsKey(amplitude))
                return _amplitudeDictionary[amplitude].Index;

            // Short circuit this case, otherwise Math.Abs throws an exception
            if (amplitude == Int32.MinValue)
                return null;

            // Check for pendulum swings beyond the standard indexes
            int absAmplitude = Math.Abs(amplitude);
            int tenativeFrames = (int)((-21 + Math.Sqrt(441 + 84 * absAmplitude)) / 42);
            int tentativeAmplitude = tenativeFrames * (tenativeFrames + 1) * 21;
            if (absAmplitude == tentativeAmplitude && absAmplitude > 7182)
            {
                if ((amplitude > 0) == (tenativeFrames % 2 == 0)) // beyond forward indexes
                {
                    return tenativeFrames + 270;
                }
                else // beyond backward indexes
                {
                    return -1 * tenativeFrames - 363;
                }
            }

            return null;
        }

        public int GetPendulumAmplitude(int index)
        {
            if (_indexDictionary.ContainsKey(index)) return _indexDictionary[index].Amplitude;

            int beyondIndex = index > 288 ? index - 288 : -381 - index;
            int amplitudeMagnitude = 7182 + 777 * beyondIndex + 21 * (beyondIndex * beyondIndex);
            int sign = index % 2 == 0 ? 1 : -1;
            return amplitudeMagnitude * sign;
        }

        public void FillInExtras()
        {
            HashSet<int> seenAmplitudes = new HashSet<int>();
            Queue<PendulumSwing> queue = new Queue<PendulumSwing>();

            int startingAmplitude = -43470; // index 315
            PendulumSwing startingPendulumSwing = new PendulumSwing(startingAmplitude, 0, null);
            queue.Enqueue(startingPendulumSwing);
            seenAmplitudes.Add(startingPendulumSwing.Amplitude);

            while (queue.Count > 0)
            {
                PendulumSwing dequeue = queue.Dequeue();
                List<PendulumSwing> successors = dequeue.GetSuccessors();
                foreach (PendulumSwing pendulumSwing in successors)
                {
                    if (pendulumSwing.Amplitude == -57330)
                    {
                        return;
                    }
                    if (seenAmplitudes.Contains(pendulumSwing.Amplitude)) continue;
                    queue.Enqueue(pendulumSwing);
                    seenAmplitudes.Add(pendulumSwing.Amplitude);
                }
            }
        }

        public class PendulumSwing
        {
            public readonly int Amplitude;
            public readonly int Acceleration;
            public readonly PendulumSwing Predecessor;

            public PendulumSwing(int amplitude, int acceleration, PendulumSwing predecessor)
            {
                Amplitude = amplitude;
                Acceleration = acceleration;
                Predecessor = predecessor;
            }

            public List<PendulumSwing> GetSuccessors()
            {
                return new List<PendulumSwing>()
                {
                    new PendulumSwing((int)WatchVariableSpecialUtilities.GetPendulumAmplitude(Amplitude, 13), 13, this),
                    new PendulumSwing((int)WatchVariableSpecialUtilities.GetPendulumAmplitude(Amplitude, 42), 42, this),
                };
            }

            public override string ToString()
            {
                string predecessorString = Predecessor?.ToString() ?? "";
                return predecessorString + " =>" + Acceleration + "=> " + Amplitude;
            }
        }
    }
}
