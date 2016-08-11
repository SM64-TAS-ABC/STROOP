using OpenTK.Graphics.OpenGL;
using SM64_Diagnostic.Controls.Map;
using SM64_Diagnostic.ManagerClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM64_Diagnostic.Structs
{
    public class TriangleMapObject : MapBaseObject
    {

        public float X1, X2, X3;
        public float Z1, Z2, Z3;
        public float Y;
        public bool Show;


        public PointF P1OnControl, P2OnControl, P3OnControl;


        public TriangleMapObject(int depth = 0)
        {
            Depth = depth;
        }

        public override void DrawOnControl(MapGraphics graphics)
        {
            GL.LoadIdentity();
            GL.Begin(PrimitiveType.Triangles);
            GL.Color4(1, 0, 0, 0.5);
            GL.Vertex3(P1OnControl.X, 0, P1OnControl.Y);
            GL.Vertex3(P2OnControl.X, 0, P2OnControl.Y);
            GL.Vertex3(P3OnControl.X, 0, P3OnControl.Y);
            GL.End();
        }

        public override void Load(MapGraphics graphics)
        {
        }

        public override void Dispose()
        {
        }

        public override double GetDepthScore()
        {
            return Y + Depth * 65536d;
        }
    }
}
