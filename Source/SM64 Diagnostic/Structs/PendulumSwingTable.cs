using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
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

        Dictionary<int, PendulumSwingReference> _table = new Dictionary<int, PendulumSwingReference>();

        public PendulumSwingTable()
        {
        }

        public void Add(PendulumSwingReference pendulumSwingRef)
        {
            _table.Add(pendulumSwingRef.Amplitude, pendulumSwingRef);
        }

        public int? GetPendulumSwingIndex(int amplitude)
        {
            if (_table.ContainsKey(amplitude))
                return _table[amplitude].Index;

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
    }
}
