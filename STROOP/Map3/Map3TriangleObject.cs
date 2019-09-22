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
        public Map3TriangleObject()
            : base()
        {
        }

        protected void DrawTriangle(List<(int x, int z)> vertices)
        {
            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Triangles);
            GL.Vertex2(veriticesForControl[0].x, veriticesForControl[0].z);
            GL.Vertex2(veriticesForControl[1].x, veriticesForControl[1].z);
            GL.Vertex2(veriticesForControl[2].x, veriticesForControl[2].z);
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void Dispose()
        {
        }
    }
}
