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
        public PointF LocationOnContol;
        public bool Show;
        public float X;
        public float Y;
        public float Z;
        public bool IsActive;
        public float Rotation;
        public bool UsesRotation; 

        public int TextureId;
        public bool Draw;

        public MapObject(Image image, PointF location = new PointF())
        {
            Image = image;
            X = location.X;
            Y = location.Y;
        }
    }
}
