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
            return Name + ": " + SubName;
        }

        private List<object> GetFieldList()
        {
            return new List<object>() { Level, Area, LoadingPoint, MissionLayout, Y };
        }

        public int CompareTo(object obj)
        {
            if (!(obj is MapLayout)) return -1;
            MapLayout other = (MapLayout)obj;
            if (Level != other.Level) return Level.CompareTo(other.Level);
            if (Area != other.Area) return Area.CompareTo(other.Area);
            if (LoadingPoint != other.LoadingPoint)
            {
                if (LoadingPoint == null || other.LoadingPoint == null)
                {
                    return LoadingPoint == null ? -1 : 1;
                }
                else
                {
                    return LoadingPoint.Value.CompareTo(other.LoadingPoint.Value);
                }
            }
            if (MissionLayout != other.MissionLayout)
            {
                if (MissionLayout == null || other.MissionLayout == null)
                {
                    return MissionLayout == null ? -1 : 1;
                }
                else
                {
                    return MissionLayout.Value.CompareTo(other.MissionLayout.Value);
                }
            }
            if (Y != other.Y) return Y.CompareTo(other.Y);
            return 0;
        }
    }
}
