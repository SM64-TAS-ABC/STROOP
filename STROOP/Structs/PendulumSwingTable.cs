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

        Dictionary<int, string> _extendedAmplitudeDictionary = new Dictionary<int, string>();

        public PendulumSwingTable()
        {
        }

        public void Add(PendulumSwingReference pendulumSwingRef)
        {
            _amplitudeDictionary.Add(pendulumSwingRef.Amplitude, pendulumSwingRef);
            _indexDictionary.Add(pendulumSwingRef.Index, pendulumSwingRef);

            _extendedAmplitudeDictionary.Add(pendulumSwingRef.Amplitude, pendulumSwingRef.Index.ToString());
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

        public string GetPendulumSwingIndexExtended(int amplitude)
        {
            int? pendulumIndex = GetPendulumSwingIndex(amplitude);
            if (pendulumIndex.HasValue)
                return pendulumIndex.Value.ToString();

            if (_extendedAmplitudeDictionary.ContainsKey(amplitude))
                return _extendedAmplitudeDictionary[amplitude];

            return Double.NaN.ToString();
        }

        public void FillInExtended()
        {
            int range = 100;

            List<int> startingIndexes = new List<int>();
            for (int i = 0; i < range; i++)
            {
                startingIndexes.Add(289 + i);
            }
            for (int i = 0; i < range; i++)
            {
                startingIndexes.Add(-382 - i);
            }

            List<PendulumSwing> startingSwings = startingIndexes.ConvertAll(
                index => new PendulumSwing(GetPendulumAmplitude(index), 0, null, index, 0));
            Queue<PendulumSwing> queue = new Queue<PendulumSwing>();
            foreach (PendulumSwing swing in startingSwings)
            {
                List<PendulumSwing> successors = swing.GetSuccessors();
                successors.ForEach(successor => queue.Enqueue(successor));
            }

            while (queue.Count > 0)
            {
                PendulumSwing dequeue = queue.Dequeue();
                if (GetPendulumSwingIndexExtended(dequeue.Amplitude) != Double.NaN.ToString())
                    continue;
                if (dequeue.SecondaryIndex > range)
                    continue;

                string extendedIndex = dequeue.PrimaryIndex + "-" + dequeue.SecondaryIndex;
                _extendedAmplitudeDictionary[dequeue.Amplitude] = extendedIndex;

                List<PendulumSwing> successors = dequeue.GetSuccessors();
                successors.ForEach(successor => queue.Enqueue(successor));
            }
        }

        public class PendulumSwing
        {
            public readonly int Amplitude;
            public readonly int Acceleration;
            public readonly PendulumSwing Predecessor;
            public readonly int PrimaryIndex;
            public readonly int SecondaryIndex;

            public PendulumSwing(
                int amplitude,
                int acceleration,
                PendulumSwing predecessor,
                int primaryIndex,
                int secondaryIndex)
            {
                Amplitude = amplitude;
                Acceleration = acceleration;
                Predecessor = predecessor;
                PrimaryIndex = primaryIndex;
                SecondaryIndex = secondaryIndex;
            }

            public List<PendulumSwing> GetSuccessors()
            {
                return new List<PendulumSwing>()
                {
                    new PendulumSwing((int)WatchVariableSpecialUtilities.GetPendulumAmplitude(Amplitude, 13), 13, this, PrimaryIndex, SecondaryIndex + 1),
                    new PendulumSwing((int)WatchVariableSpecialUtilities.GetPendulumAmplitude(Amplitude, 42), 42, this, PrimaryIndex, SecondaryIndex + 1),
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
