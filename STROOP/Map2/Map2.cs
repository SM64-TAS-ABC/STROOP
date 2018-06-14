using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace STROOP.Map2
{
    public struct Map2
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

        public static bool operator ==(Map2 a, Map2 b)
        {
            return (a.ImagePath == b.ImagePath && a.Area == b.Area && a.Level == b.Level && a.Y == b.Y
                && a.LoadingPoint == b.LoadingPoint && a.MissionLayout == b.MissionLayout);
        }

        public static bool operator !=(Map2 a, Map2 b)
        {
            return !(a == b);
        }

        public override bool Equals(object other)
        {
            if (!(other is Map2))
                return false;

            return (this == (Map2)other);
        }

        public override int GetHashCode()
        {
            return ImagePath.GetHashCode() * 127 + Level.GetHashCode() * 31 + Area.GetHashCode() * 17 + Y.GetHashCode()
                + 257 * MissionLayout.GetHashCode() + 67 * LoadingPoint.GetHashCode();
        }
    }
}
