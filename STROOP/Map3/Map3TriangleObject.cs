using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public abstract class Map3TriangleObject : Map3Object
    {
        public Map3TriangleObject(Map3Graphics graphics)
            : base(graphics)
        {
        }

        protected void DrawTriangle(List<(int x, int z)> vertices, Color color)
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(color);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(vertices[0].x, vertices[0].z);
            GL.Vertex2(vertices[1].x, vertices[1].z);
            GL.Vertex2(vertices[2].x, vertices[2].z);
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void Dispose()
        {
        }
    }
}
