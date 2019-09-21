using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using STROOP.Structs.Configurations;

namespace STROOP.Structs
{
    public class MapLayout : IComparable
    {
        public string ImagePath;
        public BackgroundImage? Background;

        public string Id;
        public byte Level;
        public byte Area;
        public ushort? LoadingPoint;
        public ushort? MissionLayout;
        public RectangleF Coordinates;
        public float Y;
        public string Name;
        public string SubName;
        
        public override bool Equals(object obj)
        {
            if (!(obj is MapLayout)) return false;
            MapLayout other = (MapLayout)obj;
            return Id == other.Id;
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

        private Bitmap _mapImage;
        public Bitmap MapImage
        {
            get
            {
                if (_mapImage != null) return _mapImage;
                var path = Path.Combine(Config.MapAssociations.MapImageFolderPath, ImagePath);
                using (Bitmap preLoad = Bitmap.FromFile(path) as Bitmap)
                {
                    int maxSize = 1080;
                    int largest = Math.Max(preLoad.Width, preLoad.Height);
                    float scale = 1;
                    if (largest > maxSize)
                        scale = largest / maxSize;

                    _mapImage = new Bitmap(preLoad, new Size((int)(preLoad.Width / scale), (int)(preLoad.Height / scale)));
                }
                return _mapImage;
            }
        }

        public Bitmap BackgroundImage
        {
            get
            {
                if (!Background.HasValue) return null;
                return Background.Value.Image;
            }
        }
    }
}
