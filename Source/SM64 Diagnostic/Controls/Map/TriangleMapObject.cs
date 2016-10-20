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
        public Color Color;

        public PointF P1OnControl, P2OnControl, P3OnControl;


        public TriangleMapObject(Color color, int depth = 0)
        {
            Depth = depth;
            Color = color;
        }

        public override void DrawOnControl(MapGraphics graphics)
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(Color);
            GL.Begin(BeginMode.Triangles);
            GL.Vertex2(P1OnControl.X, P1OnControl.Y);
            GL.Vertex2(P2OnControl.X, P2OnControl.Y);
            GL.Vertex2(P3OnControl.X, P3OnControl.Y);
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void Load(MapGraphics graphics)
        {
        }

        public override double GetDepthScore()
        {
            return Y + Depth * 65536d;
        }
    }
}
