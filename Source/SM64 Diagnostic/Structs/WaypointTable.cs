using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
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

        Dictionary<short, WaypointReference> _waypointDictionary;
        Dictionary<WaypointReference, double> _distanceDictionary;

        public WaypointTable(List<WaypointReference> waypoints)
        {
            _waypointDictionary = new Dictionary<short, WaypointReference>();
            foreach (WaypointReference waypointRef in waypoints)
            {
                _waypointDictionary.Add(waypointRef.Index, waypointRef);
            }

            _distanceDictionary = new Dictionary<WaypointReference, double>();
            WaypointReference? previousWaypoint = null;
            foreach (KeyValuePair<short, WaypointReference> entry in _waypointDictionary)
            {
                WaypointReference currentWaypoint = entry.Value;
                if (previousWaypoint == null)
                {
                    _distanceDictionary[currentWaypoint] = 0;
                }
                else
                {
                    double previousDistance = _distanceDictionary[previousWaypoint.Value];
                    double deltaDistance = MoreMath.GetDistanceBetween(
                        previousWaypoint.Value.X, previousWaypoint.Value.Y, previousWaypoint.Value.Z,
                        currentWaypoint.X, currentWaypoint.Y, currentWaypoint.Z);
                    double currentDistance = previousDistance + deltaDistance;
                    _distanceDictionary[currentWaypoint] = currentDistance;
                }
                previousWaypoint = currentWaypoint;
            }
        }

        public double GetProgress(uint objAddress)
        {
            uint waypointAddress = Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.WaypointOffset);
            if (waypointAddress == 0) return 0;
            short prevWaypointIndex = Config.Stream.GetInt16(waypointAddress + Config.Waypoint.IndexOffset);
            if (!_waypointDictionary.ContainsKey(prevWaypointIndex)) return 0;
            WaypointReference waypoint = _waypointDictionary[prevWaypointIndex];
            if (!_distanceDictionary.ContainsKey(waypoint)) return 0;
            double distance = _distanceDictionary[waypoint];
            return distance;
        }
    }
}
