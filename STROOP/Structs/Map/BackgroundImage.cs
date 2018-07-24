using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace STROOP.Structs
{
    public struct BackgroundImage : IComparable
    {
        public string Name;
        public Bitmap Image;

        public static bool operator ==(BackgroundImage a, BackgroundImage b)
        {
            return (a.Name == b.Name && a.Image == b.Image);
        }

        public static bool operator !=(BackgroundImage a, BackgroundImage b)
        {
            return !(a == b);
        }
        
        public override bool Equals(object other)
        {
            if (!(other is BackgroundImage))
                return false;

            return (this == (BackgroundImage) other);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode() * 127 + Image.GetHashCode() * 31;
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is BackgroundImage)) return -1;
            BackgroundImage other = (BackgroundImage)obj;
            return Name.CompareTo(other.Name);
        }
    }
}
