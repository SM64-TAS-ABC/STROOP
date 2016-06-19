using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SM64_Diagnostic.Structs
{
    public class MapObject
    {
        public Image Image;
        public PointF Location;
        public int TextureId;

        public MapObject(Image image, PointF location)
        {
            Image = image;
            Location = location;
        }
    }
}
