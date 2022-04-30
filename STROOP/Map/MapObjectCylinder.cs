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
    public abstract class MapObjectCylinder : MapObjectCircle
    {
        public MapObjectCylinder()
            : base()
        {
        }

        protected override List<(float centerX, float centerZ, float radius)> Get2DDimensions()
        {
            return Get3DDimensions().ConvertAll(dimension => (dimension.centerX, dimension.centerZ, dimension.radius));
        }

        protected abstract List<(float centerX, float centerZ, float radius, float minY, float maxY)> Get3DDimensions();

        protected override List<(float x, float y, float z)> GetPoints()
        {
            return Get3DDimensions().ConvertAll(d => (d.centerX, d.minY, d.centerZ));
        }

        private List<List<(float x, float z)>> GetOrthographicDimensionsForControl()
        {
            List<List<(float x, float y, float z)>> vertexLists = Get3DDimensions().ConvertAll(dimension =>
            {
                if (_useCrossSection)
                {
                    double dist = MoreMath.GetDistanceBetween(
                        Config.CurrentMapGraphics.MapViewCenterXValue,
                        Config.CurrentMapGraphics.MapViewCenterZValue,
                        dimension.centerX,
                        dimension.centerZ);
                    double angle = MoreMath.AngleTo_AngleUnits(
                        Config.CurrentMapGraphics.MapViewCenterXValue,
                        Config.CurrentMapGraphics.MapViewCenterZValue,
                        dimension.centerX,
                        dimension.centerZ);
                    (double sidewaysDist, double forwardsDist) = 
                        MoreMath.GetComponentsFromVectorRelatively(
                            dist, angle, Config.CurrentMapGraphics.MapViewYawValue);
                    if (forwardsDist > dimension.radius || forwardsDist < -1 * dimension.radius)
                    {
                        return null;
                    }
                    (double pointX, double pointZ) = MoreMath.AddVectorToPoint(
                        -1 * forwardsDist,
                        Config.CurrentMapGraphics.MapViewYawValue,
                        dimension.centerX,
                        dimension.centerZ);
                    double legDist = Math.Sqrt(dimension.radius * dimension.radius - forwardsDist * forwardsDist);
                    (float leftX, float leftZ) = ((float, float))MoreMath.AddVectorToPoint(
                        legDist, Config.CurrentMapGraphics.MapViewYawValue + 16384, pointX, pointZ);
                    (float rightX, float rightZ) = ((float, float))MoreMath.AddVectorToPoint(
                        legDist, Config.CurrentMapGraphics.MapViewYawValue - 16384, pointX, pointZ);
                    return new List<(float x, float y, float z)>()
                    {
                        (leftX, dimension.minY, leftZ),
                        (rightX, dimension.minY, rightZ),
                        (rightX, dimension.maxY, rightZ),
                        (leftX, dimension.maxY, leftZ),
                    };
                }
                switch (Config.CurrentMapGraphics.MapViewYawValue)
                {
                    case 0:
                    case 32768:
                        return new List<(float x, float y, float z)>()
                        {
                            (dimension.centerX - dimension.radius, dimension.minY, dimension.centerZ),
                            (dimension.centerX + dimension.radius, dimension.minY, dimension.centerZ),
                            (dimension.centerX + dimension.radius, dimension.maxY, dimension.centerZ),
                            (dimension.centerX - dimension.radius, dimension.maxY, dimension.centerZ),
                        };
                    case 16384:
                    case 49152:
                        return new List<(float x, float y, float z)>()
                        {
                            (dimension.centerX, dimension.minY, dimension.centerZ - dimension.radius),
                            (dimension.centerX, dimension.minY, dimension.centerZ + dimension.radius),
                            (dimension.centerX, dimension.maxY, dimension.centerZ + dimension.radius),
                            (dimension.centerX, dimension.maxY, dimension.centerZ - dimension.radius),
                        };
                    default:
                        double sideAngle = MoreMath.RotateAngleCW(Config.CurrentMapGraphics.MapViewYawValue, 16384);
                        (float sideDiffX, float sideDiffZ) = ((float, float))MoreMath.GetComponentsFromVector(dimension.radius, sideAngle);
                        return new List<(float x, float y, float z)>()
                        {
                            (dimension.centerX - sideDiffX, dimension.minY, dimension.centerZ - sideDiffZ),
                            (dimension.centerX + sideDiffX, dimension.minY, dimension.centerZ + sideDiffZ),
                            (dimension.centerX + sideDiffX, dimension.maxY, dimension.centerZ + sideDiffZ),
                            (dimension.centerX - sideDiffX, dimension.maxY, dimension.centerZ - sideDiffZ),
                        };
                }
            }).FindAll(list => list != null);

            List<List<(float x, float z)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex => MapUtilities.ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z, UseRelativeCoordinates)));

            return vertexListsForControl;
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float z)>> vertexListsForControl = GetOrthographicDimensionsForControl();

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            for (int i = 0; i < vertexListsForControl.Count; i++)
            {
                List<(float x, float z)> vertexList = vertexListsForControl[i];

                GL.Begin(PrimitiveType.Polygon);
                foreach ((float x, float z) in vertexList)
                {
                    byte opacityByte = OpacityByte;
                    if (this == hoverData?.MapObject && i == hoverData?.Index)
                    {
                        opacityByte = MapUtilities.GetHoverOpacityByte();
                    }
                    GL.Color4(Color.R, Color.G, Color.B, opacityByte);
                    GL.Vertex2(x, z);
                }
                GL.End();

                // Draw outline
                if (LineWidth != 0)
                {
                    GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                    GL.LineWidth(LineWidth);
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
                    List<(float x, float y, float z)> points3D = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                        .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 65536)
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
                    GetBaseVertices(maxY, LineColor),
                    GetBaseVertices(minY, LineColor),
                };

                List<(float x, float z)> points2D = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                    .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 65536)
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
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            List<List<(float x, float z)>> dimensionList = GetOrthographicDimensionsForControl();
            for (int i = dimensionList.Count - 1; i >= 0; i--)
            {
                List<(float x, float z)> dimension = dimensionList[i];
                if (MapUtilities.IsWithinShapeForControl(dimension, relPos.X, relPos.Y) || forceCursorPosition)
                {
                    var inGameDimensionList = GetPoints();
                    var inGameDimension = inGameDimensionList[i];
                    return new MapObjectHoverData(this, inGameDimension.x, inGameDimension.y, inGameDimension.z, index: i);
                }
            }
            return null;
        }
    }
}
