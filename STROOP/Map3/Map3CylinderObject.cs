using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using STROOP.Map3.Map.Graphics;
using OpenTK.Graphics;
using System.Drawing;

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

            Map4Vertex[] GetBaseVertices(float height, Color4 color)
            {
                List<(float x, float y, float z)> points3D = Enumerable.Range(0, NUM_POINTS_2D).ToList()
                    .ConvertAll(index => (index / (float)NUM_POINTS_2D) * 65536)
                    .ConvertAll(angle =>
                    {
                        (float x, float z) = ((float, float))MoreMath.AddVectorToPoint(radius, angle, centerX, centerZ);
                        return (x, height, z);
                    });
                return points3D.ConvertAll(
                    vertex => new Map4Vertex(new Vector3(
                        vertex.x, vertex.y, vertex.z), color)).ToArray();
            }
            List<Map4Vertex[]> vertexArrayForBases = new List<Map4Vertex[]>()
            {
                GetBaseVertices(maxY, Color4),
                GetBaseVertices(minY, Color4),
            };
            List<Map4Vertex[]> vertexArrayForEdges = new List<Map4Vertex[]>()
            {
                GetBaseVertices(maxY, OutlineColor),
                GetBaseVertices(minY, OutlineColor),
            };

            List<(float x, float z)> points2D = Enumerable.Range(0, NUM_POINTS_2D).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS_2D) * 65536)
                .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(radius, angle, centerX, centerZ));
            List<Map4Vertex[]> vertexArrayForCurve = new List<Map4Vertex[]>();
            for (int i = 0; i < points2D.Count; i++)
            {
                (float x1, float z1) = points2D[i];
                (float x2, float z2) = points2D[(i + 1) % points2D.Count];
                vertexArrayForCurve.Add(new Map4Vertex[]
                {
                    new Map4Vertex(new Vector3(x1, maxY, z1), Color4),
                    new Map4Vertex(new Vector3(x2, maxY, z2), Color4),
                    new Map4Vertex(new Vector3(x2, minY, z2), Color4),
                    new Map4Vertex(new Vector3(x1, minY, z1), Color4),
                });
            }

            List<Map4Vertex[]> vertexArrayForSurfaces = vertexArrayForBases.Concat(vertexArrayForCurve).ToList();

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

            if (OutlineWidth != 0)
            {
                vertexArrayForEdges.ForEach(vertexes =>
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
        }
    }
}
