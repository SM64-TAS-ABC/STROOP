using OpenTK.Graphics.OpenGL;
using STROOP.Controls.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Map2
{
    public class TriangleMap2Object : Map2BaseObject
    {

        public float X1, X2, X3;
        public float Z1, Z2, Z3;

        public float RelX1 { get => (float)PuUtilities.GetRelativeCoordinate(X1); }
        public float RelX2 { get => (float)PuUtilities.GetRelativeCoordinate(X2); }
        public float RelX3 { get => (float)PuUtilities.GetRelativeCoordinate(X3); }
        public float RelZ1 { get => (float)PuUtilities.GetRelativeCoordinate(Z1); }
        public float RelZ2 { get => (float)PuUtilities.GetRelativeCoordinate(Z2); }
        public float RelZ3 { get => (float)PuUtilities.GetRelativeCoordinate(Z3); }

        public float Y;
        public bool Show;
        public Color Color;

        public PointF P1OnControl, P2OnControl, P3OnControl;


        public TriangleMap2Object(Color color, int depth = 0)
        {
            Depth = depth;
            Color = color;
        }

        public override void DrawOnControl(Map2Graphics graphics)
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(Color);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(P1OnControl.X, P1OnControl.Y);
            GL.Vertex2(P2OnControl.X, P2OnControl.Y);
            GL.Vertex2(P3OnControl.X, P3OnControl.Y);
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void Load(Map2Graphics graphics)
        {
        }

        public override double GetDepthScore()
        {
            return Y + Depth * 65536d;
        }

        public void Update(TriangleDataModel tri)
        {
            X1 = tri.X1;
            Z1 = tri.Z1;
            X2 = tri.X2;
            Z2 = tri.Z2;
            X3 = tri.X3;
            Z3 = tri.Z3;
            Y = (tri.Y1 + tri.Y2 + tri.Y3) / 3f;
        }

        public void Update(TriangleShape tri)
        {
            X1 = (float)tri.X1;
            Z1 = (float)tri.Z1;
            X2 = (float)tri.X2;
            Z2 = (float)tri.Z2;
            X3 = (float)tri.X3;
            Z3 = (float)tri.Z3;
            Y = (float)(tri.Y1 + tri.Y2 + tri.Y3) / 3f;
        }
    }
}
