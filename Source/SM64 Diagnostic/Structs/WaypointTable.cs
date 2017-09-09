using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class WaypointTable
    {
        public struct WaypointReference
        {
            public short Index;
            public short X;
            public short Y;
            public short Z;

            public override int GetHashCode()
            {
                return Index;
            }
        }

        Dictionary<short, WaypointReference> _table;

        public WaypointTable(List<WaypointReference> waypoints)
        {
            _table = new Dictionary<short, WaypointReference>();
            foreach (WaypointReference waypointRef in waypoints)
            {
                _table.Add(waypointRef.Index, waypointRef);
            }
        }

        public double GetProgress(uint objAddress)
        {
            return 1;
        }
    }
}
