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

        Dictionary<short, WaypointReference> _table = new Dictionary<short, WaypointReference>();

        public WaypointTable()
        {
        }

        public void Add(WaypointReference waypointRef)
        {
            _table.Add(waypointRef.Index, waypointRef);
        }
    }
}
