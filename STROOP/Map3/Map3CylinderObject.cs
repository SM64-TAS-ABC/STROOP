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
    public abstract class Map3CylinderObject : Map3CircleObject
    {
        public Map3CylinderObject()
            : base()
        {
        }

        protected override (float centerX, float centerZ, float radius) Get2DDimensions()
        {
            (float centerX, float centerZ, float radius, float minY, float maxY) = Get3DDimensions();
            return (centerX, centerZ, radius);
        }

        protected abstract (float centerX, float centerZ, float radius, float minY, float maxY) Get3DDimensions();

        public override void DrawOn3DControl()
        {
            (float centerX, float centerZ, float radius, float minY, float maxY) = Get3DDimensions();

            Map4Vertex[] GetBaseVertices(float height)
            {
                List<(float x, float y, float z)> points = Enumerable.Range(0, NUM_POINTS).ToList()
                    .ConvertAll(index => (index / (float)NUM_POINTS) * 65536)
                    .ConvertAll(angle =>
                    {
                        (float x, float z) = ((float, float))MoreMath.AddVectorToPoint(radius, angle, centerX, centerZ);
                        return (x, height, z);
                    });
                return points.ConvertAll(
                    vertex => new Map4Vertex(new Vector3(
                        vertex.x, vertex.y, vertex.z), Color4)).ToArray();
            }
            List<Map4Vertex[]> vertexArrayForSurfaces = new List<Map4Vertex[]>()
            {
                GetBaseVertices(maxY),
                GetBaseVertices(minY),
            };

            vertexArrayForSurfaces.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });




            /*
            float relativeHeight = _relativeHeight ?? 0;
            List<TriangleDataModel> tris = GetTriangles();

            List<List<(float x, float y, float z)>> centerSurfaces =
                tris.ConvertAll(tri => tri.Get3DVertices()
                    .ConvertAll(vertex => OffsetVertex(vertex, 0, relativeHeight, 0)));

            List<List<(float x, float y, float z)>> GetFrontOrBackSurfaces(bool front) =>
                tris.ConvertAll(tri =>
                {
                    bool xProjection = tri.XProjection;
                    float angle = (float)Math.Atan2(tri.NormX, tri.NormZ);
                    float projectionMag = Size / (float)Math.Abs(xProjection ? Math.Sin(angle) : Math.Cos(angle));
                    float projectionDist = front ? projectionMag : -1 * projectionMag;
                    float xOffset = xProjection ? projectionDist : 0;
                    float yOffset = relativeHeight;
                    float zOffset = xProjection ? 0 : projectionDist;
                    return tri.Get3DVertices().ConvertAll(vertex =>
                    {
                        return OffsetVertex(vertex, xOffset, yOffset, zOffset);
                    });
                });
            List<List<(float x, float y, float z)>> frontSurfaces = GetFrontOrBackSurfaces(true);
            List<List<(float x, float y, float z)>> backSurfaces = GetFrontOrBackSurfaces(false);

            List<List<(float x, float y, float z)>> GetSideSurfaces(int index1, int index2) =>
                tris.ConvertAll(tri =>
                {
                    bool xProjection = tri.XProjection;
                    float angle = (float)Math.Atan2(tri.NormX, tri.NormZ);
                    float projectionMag = Size / (float)Math.Abs(xProjection ? Math.Sin(angle) : Math.Cos(angle));
                    float xOffsetMag = xProjection ? projectionMag : 0;
                    float zOffsetMag = xProjection ? 0 : projectionMag;
                    List<(float x, float y, float z)> vertices = tri.Get3DVertices();
                    return new List<(float x, float y, float z)>()
                    {
                        OffsetVertex(vertices[index1], xOffsetMag, relativeHeight, zOffsetMag),
                        OffsetVertex(vertices[index2], xOffsetMag, relativeHeight, zOffsetMag),
                        OffsetVertex(vertices[index2], -1 * xOffsetMag, relativeHeight, -1 * zOffsetMag),
                        OffsetVertex(vertices[index1], -1 * xOffsetMag, relativeHeight, -1 * zOffsetMag),
                    };
                });
            List<List<(float x, float y, float z)>> side1Surfaces = GetSideSurfaces(0, 1);
            List<List<(float x, float y, float z)>> side2Surfaces = GetSideSurfaces(1, 2);
            List<List<(float x, float y, float z)>> side3Surfaces = GetSideSurfaces(2, 0);

            List<List<(float x, float y, float z)>> allSurfaces =
                centerSurfaces
                .Concat(frontSurfaces)
                .Concat(backSurfaces)
                .Concat(side1Surfaces)
                .Concat(side2Surfaces)
                .Concat(side3Surfaces)
                .ToList();

            List<Map4Vertex[]> vertexArray1 = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), Color4)).ToArray());
            List<Map4Vertex[]> vertexArray2 = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), OutlineColor)).ToArray());

            vertexArray1.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });

            if (OutlineWidth != 0)
            {
                vertexArray2.ForEach(vertexes =>
                {
                    int buffer = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(OutlineWidth);
                    Config.Map4Graphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer);
                });
            }
            */
        }
    }
}
