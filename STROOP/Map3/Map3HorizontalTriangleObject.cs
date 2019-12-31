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
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public abstract class Map3HorizontalTriangleObject : Map3TriangleObject
    {
        public Map3HorizontalTriangleObject()
            : base()
        {
        }

        public override void DrawOn2DControl()
        {
            if (ShowTriUnits)
            {
                DrawOn2DControlWithUnits();
            }
            else
            {
                DrawOn2DControlWithoutUnits();
            }
        }

        private void DrawOn2DControlWithoutUnits()
        {
            List<List<(float x, float y, float z)>> vertexLists = GetVertexLists();
            List<List<(float x, float y, float z)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.y, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Triangles);
            foreach (List<(float x, float y, float z)> vertexList in vertexListsForControl)
            {
                foreach ((float x, float y, float z) in vertexList)
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
                foreach (List<(float x, float y, float z)> vertexList in vertexListsForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float y, float z) in vertexList)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        private void DrawOn2DControlWithUnits()
        {
            List<List<(float x, float y, float z)>> triVertexLists = GetVertexLists();
            List<(int x, int z)> unitPoints = triVertexLists.ConvertAll(vertexList =>
            {
                if (vertexList.Count == 0) return new List<(int x, int z)>();

                int xMin = (int)Math.Max(vertexList.Min(vertex => vertex.x), Config.Map3Graphics.MapViewXMin - 1);
                int xMax = (int)Math.Min(vertexList.Max(vertex => vertex.x), Config.Map3Graphics.MapViewXMax + 1);
                int zMin = (int)Math.Max(vertexList.Min(vertex => vertex.z), Config.Map3Graphics.MapViewZMin - 1);
                int zMax = (int)Math.Min(vertexList.Max(vertex => vertex.z), Config.Map3Graphics.MapViewZMax + 1);

                List<(int x, int z)> points = new List<(int x, int z)>();
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        if (MoreMath.IsPointInsideTriangle(
                            x, z,
                            vertexList[0].x, vertexList[0].z,
                            vertexList[1].x, vertexList[1].z,
                            vertexList[2].x, vertexList[2].z))
                        {
                            points.Add((x, z));
                        }
                    }
                }
                return points;
            }).SelectMany(points => points).Distinct().ToList();

            List<List<(float x, float y, float z)>> quadList = Map3Utilities.ConvertUnitPointsToQuads(unitPoints);
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

        public override void DrawOn3DControl()
        {
            List<List<(float x, float y, float z)>> topSurfaces = GetVertexLists();

            List<List<(float x, float y, float z)>> bottomSurfaces =
                topSurfaces.ConvertAll(topSurface =>
                    topSurface.ConvertAll(vertex =>
                        OffsetVertex(vertex, 0, -1 * Size, 0)));

            List<List<(float x, float y, float z)>> GetSideSurfaces(int index1, int index2) =>
                topSurfaces.ConvertAll(topSurface =>
                    new List<(float x, float y, float z)>()
                    {
                        topSurface[index1],
                        topSurface[index2],
                        OffsetVertex(topSurface[index2], 0, -1 * Size, 0),
                        OffsetVertex(topSurface[index1], 0, -1 * Size, 0),
                    });
            List<List<(float x, float y, float z)>> side1Surfaces = GetSideSurfaces(0, 1);
            List<List<(float x, float y, float z)>> side2Surfaces = GetSideSurfaces(1, 2);
            List<List<(float x, float y, float z)>> side3Surfaces = GetSideSurfaces(2, 0);

            List<List<(float x, float y, float z)>> allSurfaces =
                topSurfaces
                .Concat(bottomSurfaces)
                .Concat(side1Surfaces)
                .Concat(side2Surfaces)
                .Concat(side3Surfaces)
                .ToList();

            List<Map4Vertex[]> vertexArrayForSurfaces = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), Color4)).ToArray());
            List<Map4Vertex[]> vertexArrayForEdges = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), OutlineColor)).ToArray());

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map4Camera.Matrix;
            GL.UniformMatrix4(Config.Map4Graphics.GLUniformView, false, ref viewMatrix);

            vertexArrayForSurfaces.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, Map4GraphicsUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });

            if (OutlineWidth != 0)
            {
                vertexArrayForEdges.ForEach(vertexes =>
                {
                    int buffer = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, Map4GraphicsUtilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(OutlineWidth);
                    Config.Map4Graphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer);
                });
            }
        }
    }
}
