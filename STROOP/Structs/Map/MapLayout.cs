using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace STROOP.Structs
{
    public struct MapLayout : IComparable
    {
        public string ImagePath;
        public string BackgroundPath;
        public string Id;
        public byte Level;
        public byte Area;
        public ushort? LoadingPoint;
        public ushort? MissionLayout;
        public RectangleF Coordinates;
        public float Y;
        public string Name;
        public string SubName;

        public static bool operator ==(MapLayout a, MapLayout b)
        {
            return (a.ImagePath == b.ImagePath && a.Area == b.Area && a.Level == b.Level && a.Y == b.Y
                && a.LoadingPoint == b.LoadingPoint && a.MissionLayout == b.MissionLayout);
        }

        public static bool operator !=(MapLayout a, MapLayout b)
        {
            return !(a == b);
        }
        
        public override bool Equals(object other)
        {
            if (!(other is MapLayout))
                return false;

            return (this == (MapLayout) other);
        }

        public override int GetHashCode()
        {
            return ImagePath.GetHashCode() * 127 + Level.GetHashCode() * 31 + Area.GetHashCode() * 17 + Y.GetHashCode()
                + 257 * MissionLayout.GetHashCode() + 67 * LoadingPoint.GetHashCode(); 
        }

        public override string ToString()
        {
            string subNameString = SubName != null ? ": " + SubName : "";
            string yString = Y != float.MinValue ? String.Format(" (y ≥ {0})", Y) : "";
            return Name + subNameString + yString;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is MapLayout)) return -1;
            MapLayout other = (MapLayout)obj;
            return Id.CompareTo(other.Id);
        }
    }
}
