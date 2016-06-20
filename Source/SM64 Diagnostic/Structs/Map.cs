using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Structs
{
    public struct Map
    {
        public string ImagePath;
        public byte Level;
        public byte Area;
        public RectangleF Coordinates;
        public float Y;
        public string Name;
        public string SubName;

        public static bool operator ==(Map a, Map b)
        {
            return (a.ImagePath == b.ImagePath && a.Area == b.Area && a.Level == b.Level && a.Y == b.Y );
        }

        public static bool operator !=(Map a, Map b)
        {
            return !(a == b);
        }
        
        public override bool Equals(object other)
        {
            if (!(other is Map))
                return false;

            return (this == (Map) other);
        }

        public override int GetHashCode()
        {
            return ImagePath.GetHashCode() * 127 + Level.GetHashCode() * 31 + Area.GetHashCode() * 17 + Y.GetHashCode(); 
        }
    }
}
