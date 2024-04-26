﻿using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
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

        private Dictionary<short, WaypointReference> _waypointDictionary;
        private Dictionary<WaypointReference, double> _distanceDictionary;
        private Dictionary<WaypointReference, WaypointReference?> _previousWaypointDictionary;
        private Dictionary<WaypointReference, WaypointReference?> _nextWaypointDictionary;
        private int _maxIndex = -1;

        public WaypointTable(IEnumerable<WaypointReference> waypoints)
        {
            _maxIndex = waypoints.Max(wp => wp.Index);

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

            _previousWaypointDictionary = new Dictionary<WaypointReference, WaypointReference?>();
            _nextWaypointDictionary = new Dictionary<WaypointReference, WaypointReference?>();
            previousWaypoint = null;
            foreach (KeyValuePair<short, WaypointReference> entry in _waypointDictionary)
            {
                WaypointReference currentWaypoint = entry.Value;
                _previousWaypointDictionary[currentWaypoint] = previousWaypoint;
                if (previousWaypoint != null)
                {
                    _nextWaypointDictionary[previousWaypoint.Value] = currentWaypoint;
                }
                previousWaypoint = currentWaypoint;
            }
            _nextWaypointDictionary[previousWaypoint.Value] = null;
        }

        public double GetProgress(uint objAddress)
        {
            uint waypointAddress = Config.Stream.GetUInt(objAddress + ObjectConfig.WaypointOffset);
            if (waypointAddress == 0) return 0;
            short prevWaypointIndex = Config.Stream.GetShort(waypointAddress + WaypointConfig.IndexOffset);

            if (!_waypointDictionary.ContainsKey(prevWaypointIndex)) return 0;
            WaypointReference previousWaypoint = _waypointDictionary[prevWaypointIndex];

            if (!_nextWaypointDictionary.ContainsKey(previousWaypoint)) return 0;
            WaypointReference? nullableNextWaypoint = _nextWaypointDictionary[previousWaypoint];
            WaypointReference nextWaypoint;
            if (nullableNextWaypoint == null)
            {
                if (!_previousWaypointDictionary.ContainsKey(previousWaypoint)) return 0;
                WaypointReference? nullablePreviousPreviousWaypoint = _previousWaypointDictionary[previousWaypoint];
                if (nullablePreviousPreviousWaypoint == null) return 0;
                nextWaypoint = previousWaypoint;
                previousWaypoint = nullablePreviousPreviousWaypoint.Value;
            }
            else
            {
                nextWaypoint = nullableNextWaypoint.Value;
            }

            float objX = Config.Stream.GetFloat(objAddress + ObjectConfig.XOffset);
            float objY = Config.Stream.GetFloat(objAddress + ObjectConfig.YOffset);
            float objZ = Config.Stream.GetFloat(objAddress + ObjectConfig.ZOffset);

            if (!_distanceDictionary.ContainsKey(previousWaypoint)) return 0;
            double previousDistance = _distanceDictionary[previousWaypoint];

            double planeDistance = MoreMath.GetPlaneDistanceBetweenPoints(
                objX, objY, objZ,
                previousWaypoint.X, previousWaypoint.Y, previousWaypoint.Z,
                nextWaypoint.X, nextWaypoint.Y, nextWaypoint.Z);
            double totalDistance = previousDistance + planeDistance;

            return totalDistance;
        }

        public (int x, int y, int z) GetWaypoint(int index)
        {
            WaypointReference wp = _waypointDictionary[(short)index];
            return (wp.X, wp.Y, wp.Z);
        }

        public int GetMaxIndex()
        {
            return _maxIndex;
        }
    }
}
