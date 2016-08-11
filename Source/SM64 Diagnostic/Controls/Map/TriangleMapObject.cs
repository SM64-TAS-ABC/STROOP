using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class TriangleMapObject
    {
        public bool Show;
        public float X1, X2, X3;
        public float Z1, Z2, Z3;
        public bool IsActive;
        public int Depth;

        public PointF P1OnControl, P2OnControl, P3OnControl;
        public uint DepthScore;
        public bool Draw;

        public TriangleMapObject(Image image, int depth = 0, PointF location = new PointF())
        {
            Image = image;
            X = location.X;
            Y = location.Y;
            Depth = depth;
        }
    }
}
