using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapAggregatedPathObject : MapObject
    {
        public MapAggregatedPathObject(PositionAngle posAngle)
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            if (OutlineWidth == 0) return;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LineWidth(OutlineWidth);
            for (int i = 0; i < veriticesForControl.Count - 1; i++)
            {
                Color color = OutlineColor;
                if (false)
                {
                    int distFromEnd = veriticesForControl.Count - i - 2;
                    if (distFromEnd < Size)
                    {
                        color = ColorUtilities.InterpolateColor(
                            OutlineColor, Color, distFromEnd / (double)Size);
                    }
                    else
                    {
                        color = Color;
                    }
                }
                (float x1, float z1) = veriticesForControl[i];
                (float x2, float z2) = veriticesForControl[i + 1];
                GL.Color4(color.R, color.G, color.B, OpacityByte);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x1, z1);
                GL.Vertex2(x2, z2);
                GL.End();
            }
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            if (OutlineWidth == 0) return;

            List<(float x, float y, float z)> vertices = new List<(float x, float y, float z)>();
            List<Map3DVertex[]> vertexArrayList = new List<Map3DVertex[]>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Color color = OutlineColor;
                if (false)
                {
                    int distFromEnd = vertices.Count - i - 2;
                    if (distFromEnd < Size)
                    {
                        color = ColorUtilities.InterpolateColor(
                            OutlineColor, Color, distFromEnd / (double)Size);
                    }
                    else
                    {
                        color = Color;
                    }
                }
                (float x1, float y1, float z1) = vertices[i];
                (float x2, float y2, float z2) = vertices[i + 1];

                vertexArrayList.Add(new Map3DVertex[]
                {
                    new Map3DVertex(new Vector3(x1, y1, z1), color),
                    new Map3DVertex(new Vector3(x2, y2, z2), color),
                });
            }

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            vertexArrayList.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                GL.LineWidth(OutlineWidth);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Lines, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });
        }

        public override string GetName()
        {
            return "Aggregated Path";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.PathImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
