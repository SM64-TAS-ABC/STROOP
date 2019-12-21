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
    public abstract class Map3QuadObject : Map3Object
    {
        public Map3QuadObject()
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            List<List<(float x, float z)>> quadList = GetQuadList();
            List<List<(float x, float z)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Quads);
            foreach (List<(float x, float z)> quad in quadListForControl)
            {
                foreach ((float x, float z) in quad)
                {
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                foreach (List<(float x, float z)> quad in quadListForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        protected abstract List<List<(float x, float z)>> GetQuadList();
    }
}
