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
            if (!_table.ContainsKey(amplitude))
                return null;

            return _table[amplitude].Index;
        }
    }
}
