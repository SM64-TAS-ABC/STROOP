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
using System.Drawing.Imaging;
using STROOP.Map.Map3D;
using OpenTK.Graphics;

namespace STROOP.Map
{
    public abstract class MapObjectQuad : MapObject
    {
        public MapObjectQuad()
            : base()
        {
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float y, float z, Color color, bool isHovered)>> quadList = GetQuadList(hoverData);
            List<List<(float x, float z, Color color, bool isHovered)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z, UseRelativeCoordinates);
                        return (x, z, vertex.color, vertex.isHovered);
                    }));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Begin(PrimitiveType.Quads);
            foreach (List<(float x, float z, Color color, bool isHovered)> quad in quadListForControl)
            {
                foreach ((float x, float z, Color color, bool isHovered) in quad)
                {
                    byte opacityByte = isHovered ? MapUtilities.GetHoverOpacityByte() : OpacityByte;
                    GL.Color4(color.R, color.G, color.B, opacityByte);
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (List<(float x, float z, Color color, bool isHovered)> quad in quadListForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z, Color color, bool isHovered) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float y, float z, Color color, bool isHovered)>> quadList = GetQuadList3D() ?? GetQuadList(hoverData);
            List<List<(float x, float z, Color color, bool isHovered)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z, UseRelativeCoordinates);
                        return (x, z, vertex.color, vertex.isHovered);
                    }));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Begin(PrimitiveType.Quads);
            foreach (List<(float x, float z, Color color, bool isHovered)> quad in quadListForControl)
            {
                foreach ((float x, float z, Color color, bool isHovered) in quad)
                {
                    byte opacityByte = isHovered ? MapUtilities.GetHoverOpacityByte() : OpacityByte;
                    GL.Color4(color.R, color.G, color.B, opacityByte);
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (List<(float x, float z, Color color, bool isHovered)> quad in quadListForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z, Color color, bool isHovered) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            List<List<(float x, float y, float z, Color color, bool isHovered)>> quadList = GetQuadList3D() ?? GetQuadList(null);

            List<Map3DVertex[]> vertexArrayForSurfaces = quadList.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), new Color4(vertex.color.R, vertex.color.G, vertex.color.B, OpacityByte))).ToArray());
            List<Map3DVertex[]> vertexArrayForEdges = quadList.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), LineColor)).ToArray());

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            vertexArrayForSurfaces.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });

            if (LineWidth != 0)
            {
                vertexArrayForEdges.ForEach(vertexes =>
                {
                    int buffer = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(LineWidth);
                    Config.Map3DGraphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer);
                });
            }
        }

        protected abstract List<List<(float x, float y, float z, Color color, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData);

        protected virtual List<List<(float x, float y, float z, Color color, bool isHovered)>> GetQuadList3D()
        {
            return null;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
