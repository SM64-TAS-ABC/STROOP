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
using OpenTK.Graphics;
using System.Drawing;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public abstract class MapCylinderObject : MapCircleObject
    {
        public MapCylinderObject()
            : base()
        {
        }

        protected override List<(float centerX, float centerZ, float radius)> Get2DDimensions()
        {
            return Get3DDimensions().ConvertAll(dimension => (dimension.centerX, dimension.centerZ, dimension.radius));
        }

        protected abstract List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions();

        public override void DrawOn2DControlSideView()
        {
            List<List<(float x, float y, float z)>> vertexLists = Get3DDimensions().ConvertAll(dimension =>
            {
                switch (Config.MapGraphics.MapViewSideViewAngle)
                {
                    case MapGraphics.MapSideViewAngle.Angle0:
                    case MapGraphics.MapSideViewAngle.Angle32768:
                        return new List<(float x, float y, float z)>()
                        {
                            (dimension.centerX - dimension.radius, dimension.minY, dimension.centerZ),
                            (dimension.centerX + dimension.radius, dimension.minY, dimension.centerZ),
                            (dimension.centerX + dimension.radius, dimension.maxY, dimension.centerZ),
                            (dimension.centerX - dimension.radius, dimension.maxY, dimension.centerZ),
                        };
                    case MapGraphics.MapSideViewAngle.Angle16384:
                    case MapGraphics.MapSideViewAngle.Angle49152:
                        return new List<(float x, float y, float z)>()
                        {
                            (dimension.centerX, dimension.minY, dimension.centerZ - dimension.radius),
                            (dimension.centerX, dimension.minY, dimension.centerZ + dimension.radius),
                            (dimension.centerX, dimension.maxY, dimension.centerZ + dimension.radius),
                            (dimension.centerX, dimension.maxY, dimension.centerZ - dimension.radius),
                        };
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });

            List<List<(float x, float z)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex => MapUtilities.ConvertCoordsForControlSideView(vertex.x, vertex.y, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            foreach (List<(float x, float z)> vertexList in vertexListsForControl)
            {
                GL.Begin(PrimitiveType.Polygon);
                foreach ((float x, float z) in vertexList)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                foreach (List<(float x, float z)> vertexList in vertexListsForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in vertexList)
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
            List<(float centerX, float centerZ, float radius, float minY, float maxY)> dimensionList = Get3DDimensions();

            foreach ((float centerX, float centerZ, float radius, float minY, float maxY) in dimensionList)
            {
                Map3DVertex[] GetBaseVertices(float height, Color4 color)
                {
                    List<(float x, float y, float z)> points3D = Enumerable.Range(0, NUM_POINTS_2D).ToList()
                        .ConvertAll(index => (index / (float)NUM_POINTS_2D) * 65536)
                        .ConvertAll(angle =>
                        {
                            (float x, float z) = ((float, float))MoreMath.AddVectorToPoint(radius, angle, centerX, centerZ);
                            return (x, height, z);
                        });
                    return points3D.ConvertAll(
                        vertex => new Map3DVertex(new Vector3(
                            vertex.x, vertex.y, vertex.z), color)).ToArray();
                }
                List<Map3DVertex[]> vertexArrayForBases = new List<Map3DVertex[]>()
                {
                    GetBaseVertices(maxY, Color4),
                    GetBaseVertices(minY, Color4),
                };
                List<Map3DVertex[]> vertexArrayForEdges = new List<Map3DVertex[]>()
                {
                    GetBaseVertices(maxY, OutlineColor),
                    GetBaseVertices(minY, OutlineColor),
                };

                List<(float x, float z)> points2D = Enumerable.Range(0, NUM_POINTS_2D).ToList()
                    .ConvertAll(index => (index / (float)NUM_POINTS_2D) * 65536)
                    .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(radius, angle, centerX, centerZ));
                List<Map3DVertex[]> vertexArrayForCurve = new List<Map3DVertex[]>();
                for (int i = 0; i < points2D.Count; i++)
                {
                    (float x1, float z1) = points2D[i];
                    (float x2, float z2) = points2D[(i + 1) % points2D.Count];
                    vertexArrayForCurve.Add(new Map3DVertex[]
                    {
                        new Map3DVertex(new Vector3(x1, maxY, z1), Color4),
                        new Map3DVertex(new Vector3(x2, maxY, z2), Color4),
                        new Map3DVertex(new Vector3(x2, minY, z2), Color4),
                        new Map3DVertex(new Vector3(x1, minY, z1), Color4),
                    });
                }

                List<Map3DVertex[]> vertexArrayForSurfaces = vertexArrayForBases.Concat(vertexArrayForCurve).ToList();

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

                if (OutlineWidth != 0)
                {
                    vertexArrayForEdges.ForEach(vertexes =>
                    {
                        int buffer = GL.GenBuffer();
                        GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                        GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                        GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                        GL.LineWidth(OutlineWidth);
                        Config.Map3DGraphics.BindVertices();
                        GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                        GL.DeleteBuffer(buffer);
                    });
                }
            }
        }
    }
}
