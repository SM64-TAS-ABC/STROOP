using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public class PointTable
    {
        public struct PointReference
        {
            public int Index;
            public double X;
            public double Y;
            public double Z;

            public override int GetHashCode()
            {
                return Index;
            }
        }

        private Dictionary<int, PointReference> _pointDictionary;

        public PointTable(List<PointReference> pointRefs)
        {
            _pointDictionary = new Dictionary<int, PointReference>();
            foreach (PointReference pointRef in pointRefs)
            {
                _pointDictionary.Add(pointRef.Index, pointRef);
            }
        }

        public PointReference GetClosestPoint(double x, double y, double z)
        {
            PointReference closestPointRef = _pointDictionary[0];
            double closestDistance = Double.MaxValue;

            foreach (int index in _pointDictionary.Keys)
            {
                PointReference pointRef = _pointDictionary[index];
                double distance = MoreMath.GetDistanceBetween(
                    pointRef.X, pointRef.Y, pointRef.Z, x, y, z);
                if (distance < closestDistance)
                {
                    closestPointRef = pointRef;
                    closestDistance = distance;
                }
            }

            return closestPointRef;
        }

        public int GetClosestIndex(double x, double y, double z)
        {
            PointReference closestPointRef = GetClosestPoint(x, y, z);
            return closestPointRef.Index;
        }

        public double GetClosestDistance(double x, double y, double z)
        {
            PointReference closestPointRef = GetClosestPoint(x, y, z);
            return MoreMath.GetDistanceBetween(
                closestPointRef.X, closestPointRef.Y, closestPointRef.Z, x, y, z);
        }
    }
}
