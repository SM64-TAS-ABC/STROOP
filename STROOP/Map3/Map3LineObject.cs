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
    public abstract class Map3LineObject : Map3Object
    {
        public Map3LineObject()
            : base()
        {
        }

        public override void DrawOnControl()
        {
            List<(float x, float z)> vertices = GetVertices();
            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.LineWidth(Size);
            GL.Begin(PrimitiveType.Lines);
            foreach ((float x, float z) in veriticesForControl)
            {
                GL.Vertex2(x, z);
            }
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        protected abstract List<(float x, float z)> GetVertices();
    }
}
