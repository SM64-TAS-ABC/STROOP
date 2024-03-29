﻿using System;
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

        protected override List<(float centerX, float centerY, float centerZ, float radius, Color color)> Get2DDimensions()
        {
            return Get3DDimensions().ConvertAll(dimension => (dimension.centerX, dimension.minY, dimension.centerZ, dimension.radius, dimension.color));
        }

        protected abstract List<(float centerX, float centerZ, float radius, float minY, float maxY, Color color)> Get3DDimensions();

        protected override List<(float x, float y, float z)> GetPoints()
        {
            return Get3DDimensions().ConvertAll(d => (d.centerX, d.minY, d.centerZ));
        }

        private List<List<(float x, float z, Color color)>> GetOrthographicDimensionsForControl()
        {
            List<List<(float x, float y, float z, Color color)>> vertexLists = Get3DDimensions().ConvertAll(dimension =>
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
                    return new List<(float x, float y, float z, Color color)>()
                    {
                        (leftX, dimension.minY, leftZ, dimension.color),
                        (rightX, dimension.minY, rightZ, dimension.color),
                        (rightX, dimension.maxY, rightZ, dimension.color),
                        (leftX, dimension.maxY, leftZ, dimension.color),
                    };
                }
                switch (Config.CurrentMapGraphics.MapViewYawValue)
                {
                    case 0:
                    case 32768:
                        return new List<(float x, float y, float z, Color color)>()
                        {
                            (dimension.centerX - dimension.radius, dimension.minY, dimension.centerZ, dimension.color),
                            (dimension.centerX + dimension.radius, dimension.minY, dimension.centerZ, dimension.color),
                            (dimension.centerX + dimension.radius, dimension.maxY, dimension.centerZ, dimension.color),
                            (dimension.centerX - dimension.radius, dimension.maxY, dimension.centerZ, dimension.color),
                        };
                    case 16384:
                    case 49152:
                        return new List<(float x, float y, float z, Color color)>()
                        {
                            (dimension.centerX, dimension.minY, dimension.centerZ - dimension.radius, dimension.color),
                            (dimension.centerX, dimension.minY, dimension.centerZ + dimension.radius, dimension.color),
                            (dimension.centerX, dimension.maxY, dimension.centerZ + dimension.radius, dimension.color),
                            (dimension.centerX, dimension.maxY, dimension.centerZ - dimension.radius, dimension.color),
                        };
                    default:
                        double sideAngle = MoreMath.RotateAngleCW(Config.CurrentMapGraphics.MapViewYawValue, 16384);
                        (float sideDiffX, float sideDiffZ) = ((float, float))MoreMath.GetComponentsFromVector(dimension.radius, sideAngle);
                        return new List<(float x, float y, float z, Color color)>()
                        {
                            (dimension.centerX - sideDiffX, dimension.minY, dimension.centerZ - sideDiffZ, dimension.color),
                            (dimension.centerX + sideDiffX, dimension.minY, dimension.centerZ + sideDiffZ, dimension.color),
                            (dimension.centerX + sideDiffX, dimension.maxY, dimension.centerZ + sideDiffZ, dimension.color),
                            (dimension.centerX - sideDiffX, dimension.maxY, dimension.centerZ - sideDiffZ, dimension.color),
                        };
                }
            }).FindAll(list => list != null);

            List<List<(float x, float z, Color color)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z, UseRelativeCoordinates);
                        return (x, z, vertex.color);
                    }));

            return vertexListsForControl;
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float z, Color color)>> vertexListsForControl = GetOrthographicDimensionsForControl();

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            for (int i = 0; i < vertexListsForControl.Count; i++)
            {
                List<(float x, float z, Color color)> vertexList = vertexListsForControl[i];

                GL.Begin(PrimitiveType.Polygon);
                foreach ((float x, float z, Color color) in vertexList)
                {
                    byte opacityByte = OpacityByte;
                    if (this == hoverData?.MapObject && i == hoverData?.Index)
                    {
                        opacityByte = MapUtilities.GetHoverOpacityByte();
                    }
                    GL.Color4(color.R, color.G, color.B, opacityByte);
                    GL.Vertex2(x, z);
                }
                GL.End();

                // Draw outline
                if (LineWidth != 0)
                {
                    GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                    GL.LineWidth(LineWidth);
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z, Color color) in vertexList)
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
            List<(float centerX, float centerZ, float radius, float minY, float maxY, Color color)> dimensionList = Get3DDimensions();

            foreach ((float centerX, float centerZ, float radius, float minY, float maxY, Color color) in dimensionList)
            {
                Map3DVertex[] GetBaseVertices(float height, Color4 color2)
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
                            vertex.x, vertex.y, vertex.z), color2)).ToArray();
                }
                List<Map3DVertex[]> vertexArrayForBases = new List<Map3DVertex[]>()
                {
                    GetBaseVertices(maxY, new Color4(color.R, color.G, color.B, OpacityByte)),
                    GetBaseVertices(minY, new Color4(color.R, color.G, color.B, OpacityByte)),
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

            List<List<(float x, float z, Color color)>> dimensionList = GetOrthographicDimensionsForControl();
            for (int i = dimensionList.Count - 1; i >= 0; i--)
            {
                List<(float x, float z, Color color)> dimensionWithColor = dimensionList[i];
                List<(float x, float z)> dimension = dimensionWithColor.ConvertAll(d => (d.x, d.z));
                if (MapUtilities.IsWithinShapeForControl(dimension, relPos.X, relPos.Y, forceCursorPosition))
                {
                    var inGameDimensionList = GetPoints();
                    var inGameDimension = inGameDimensionList[i];
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Circle, inGameDimension.x, inGameDimension.y, inGameDimension.z, index: i);
                }
            }
            return null;
        }
    }
}
