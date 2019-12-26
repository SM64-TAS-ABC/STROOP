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
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public abstract class Map3LineObject : Map3Object
    {
        public Map3LineObject()
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            if (OutlineWidth == 0) return;

            List<(float x, float z)> vertices = GetVertices();
            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, OpacityByte);
            GL.LineWidth(OutlineWidth);
            GL.Begin(PrimitiveType.Lines);
            foreach ((float x, float z) in veriticesForControl)
            {
                GL.Vertex2(x, z);
            }
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            if (OutlineWidth == 0) return;

            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            List<(float x, float z)> vertexList = GetVertices();

            Map4Vertex[] vertexArrayForEdges =
                vertexList.ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, marioY, vertex.z), OutlineColor)).ToArray();

            int buffer = GL.GenBuffer();
            GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArrayForEdges.Length * Map4Vertex.Size),
                vertexArrayForEdges, BufferUsageHint.DynamicDraw);
            GL.LineWidth(OutlineWidth);
            Config.Map4Graphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Lines, 0, vertexArrayForEdges.Length);
            GL.DeleteBuffer(buffer);
        }

        protected abstract List<(float x, float z)> GetVertices();

        public override Map3DrawType GetDrawType()
        {
            return Map3DrawType.Perspective;
        }
    }
}
