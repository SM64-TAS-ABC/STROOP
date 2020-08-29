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
using System.Windows.Forms;
using STROOP.Models;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public abstract class MapWallObject : MapTriangleObject
    {
        private bool _showArrows;
        private float? _relativeHeight;
        private float? _absoluteHeight;

        ToolStripMenuItem _itemShowArrows;

        public MapWallObject()
            : base()
        {
            Size = 50;
            Opacity = 0.5;
            Color = Color.Green;

            _showArrows = false;
            _relativeHeight = null;
            _absoluteHeight = null;
        }

        public override void DrawOn2DControl()
        {
            float marioHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float? height = _relativeHeight.HasValue ? marioHeight - _relativeHeight.Value : (float?)null;
            height = height ?? _absoluteHeight;

            List<(float x1, float z1, float x2, float z2, bool xProjection, double pushAngle)> wallData = GetTrianglesWithinDist()
                .ConvertAll(tri => MapUtilities.Get2DWallDataFromTri(tri, height))
                .FindAll(wallDataNullable => wallDataNullable.HasValue)
                .ConvertAll(wallDataNullable => wallDataNullable.Value);

            foreach ((float x1, float z1, float x2, float z2, bool xProjection, double pushAngle) in wallData)
            {
                float angle = (float)MoreMath.AngleTo_Radians(x1, z1, x2, z2);
                float projectionDist = Size / (float)Math.Abs(xProjection ? Math.Cos(angle) : Math.Sin(angle));
                List<List<(float x, float z)>> quads = new List<List<(float x, float z)>>();
                Action<float, float> addQuad = (float xAdd, float zAdd) =>
                {
                    quads.Add(new List<(float x, float z)>()
                    {
                        (x1, z1),
                        (x1 + xAdd, z1 + zAdd),
                        (x2 + xAdd, z2 + zAdd),
                        (x2, z2),
                    });
                };
                if (xProjection)
                {
                    addQuad(projectionDist, 0);
                    addQuad(-1 * projectionDist, 0);
                }
                else
                {
                    addQuad(0, projectionDist);
                    addQuad(0, -1 * projectionDist);
                }

                List<List<(float x, float z)>> quadsForControl =
                    quads.ConvertAll(quad => quad.ConvertAll(
                        vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z)));

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw quad
                GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
                GL.Begin(PrimitiveType.Quads);
                foreach (List<(float x, float z)> quad in quadsForControl)
                {
                    foreach ((float x, float z) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                }
                GL.End();

                if (_showArrows)
                {
                    double totalDistance = MoreMath.GetDistanceBetween(x1, z1, x2, z2);
                    List<double> markDistances = new List<double>();
                    if (totalDistance < 100)
                    {
                        markDistances.Add(totalDistance / 2);
                    }
                    else
                    {
                        double firstDistance = 25;
                        double lastDistance = totalDistance - 25;
                        double distanceDiff = lastDistance - firstDistance;
                        int numMarks = (int)Math.Truncate(distanceDiff / 50) + 1;
                        int numBetweens = numMarks - 1;
                        double betweenDistance = distanceDiff / numBetweens;
                        for (int i = 0; i < numMarks; i++)
                        {
                            markDistances.Add(firstDistance + i * betweenDistance);
                        }
                    }

                    List<(float x, float z)> markPoints = new List<(float x, float z)>();
                    foreach (double dist in markDistances)
                    {
                        double portion = dist / totalDistance;
                        (double x, double z) pointOnMidpoint = (x1 + portion * (x2 - x1), z1 + portion * (z2 - z1));
                        (double x, double z) pointOnSide1 = xProjection ?
                            (pointOnMidpoint.x - projectionDist / 2, pointOnMidpoint.z) :
                            (pointOnMidpoint.x, pointOnMidpoint.z - projectionDist / 2);
                        (double x, double z) pointOnSide2 = xProjection ?
                            (pointOnMidpoint.x + projectionDist / 2, pointOnMidpoint.z) :
                            (pointOnMidpoint.x, pointOnMidpoint.z + projectionDist / 2);
                        markPoints.Add(((float x, float z))pointOnSide1);
                        markPoints.Add(((float x, float z))pointOnSide2);
                    }

                    double angleUp = pushAngle;
                    double angleDown = pushAngle + 32768;
                    double angleLeft = pushAngle + 16384;
                    double angleRight = pushAngle - 16384;
                    double angleUpLeft = pushAngle + 8192;
                    double angleUpRight = pushAngle - 8192;
                    double angleDownLeft = pushAngle + 24576;
                    double angleDownRight = pushAngle - 24576;

                    double arrowBaseLength = 0.4 * Math.Min(Size, 50);
                    double arrowSideLength = 0.2 * Math.Min(Size, 50);

                    List<List<(float x, float z)>> arrowPoints = markPoints.ConvertAll(midPoint =>
                    {
                        (float x, float z) frontPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength, angleUp, midPoint.x, midPoint.z);
                        (float x, float z) leftOuterPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2 + arrowSideLength, angleLeft, midPoint.x, midPoint.z);
                        (float x, float z) leftInnerPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2, angleLeft, midPoint.x, midPoint.z);
                        (float x, float z) rightOuterPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2 + arrowSideLength, angleRight, midPoint.x, midPoint.z);
                        (float x, float z) rightInnerPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2, angleRight, midPoint.x, midPoint.z);
                        (float x, float z) backLeftPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength, angleDown, leftInnerPoint.x, leftInnerPoint.z);
                        (float x, float z) backRightPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength, angleDown, rightInnerPoint.x, rightInnerPoint.z);

                        return new List<(float x, float z)>()
                        {
                            frontPoint,
                            leftOuterPoint,
                            leftInnerPoint,
                            backLeftPoint,
                            backRightPoint,
                            rightInnerPoint,
                            rightOuterPoint,
                        };
                    });

                    List<List<(float x, float z)>> arrowsForControl =
                        arrowPoints.ConvertAll(arrow => arrow.ConvertAll(
                            vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z)));

                    // Draw arrow
                    Color arrowColor = Color.Darken(0.5);
                    GL.Color4(arrowColor.R, arrowColor.G, arrowColor.B, OpacityByte);
                    foreach (List<(float x, float z)> arrow in arrowsForControl)
                    {
                        GL.Begin(PrimitiveType.Polygon);
                        foreach ((float x, float z) in arrow)
                        {
                            GL.Vertex2(x, z);
                        }
                        GL.End();
                    }
                }

                // Draw outline
                if (OutlineWidth != 0)
                {
                    GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                    GL.LineWidth(OutlineWidth);
                    foreach (List<(float x, float z)> quad in quadsForControl)
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
        }

        protected List<ToolStripMenuItem> GetWallToolStripMenuItems()
        {
            _itemShowArrows = new ToolStripMenuItem("Show Arrows");
            _itemShowArrows.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    wallChangeShowArrows: true, wallNewShowArrows: !_showArrows);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemSetRelativeHeight = new ToolStripMenuItem("Set Relative Height");
            itemSetRelativeHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter relative height of wall hitbox compared to wall triangle.");
                float? relativeHeightNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!relativeHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    wallChangeRelativeHeight: true, wallNewRelativeHeight: relativeHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearRelativeHeight = new ToolStripMenuItem("Clear Relative Height");
            itemClearRelativeHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    wallChangeRelativeHeight: true, wallNewRelativeHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemSetAbsoluteHeight = new ToolStripMenuItem("Set Absolute Height");
            itemSetAbsoluteHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the height at which you want to see the wall triangles.");
                float? absoluteHeightNullable =
                    text == "" ?
                    Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!absoluteHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    wallChangeAbsoluteHeight: true, wallNewAbsoluteHeight: absoluteHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearAbsoluteHeight = new ToolStripMenuItem("Clear Absolute Height");
            itemClearAbsoluteHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    wallChangeAbsoluteHeight: true, wallNewAbsoluteHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemShowArrows,
                itemSetRelativeHeight,
                itemClearRelativeHeight,
                itemSetAbsoluteHeight,
                itemClearAbsoluteHeight,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.WallChangeShowArrows)
            {
                _showArrows = settings.WallNewShowArrows;
                _itemShowArrows.Checked = settings.WallNewShowArrows;
            }

            if (settings.WallChangeRelativeHeight)
            {
                _relativeHeight = settings.WallNewRelativeHeight;
            }

            if (settings.WallChangeAbsoluteHeight)
            {
                _absoluteHeight = settings.WallNewAbsoluteHeight;
            }
        }

        public override void DrawOn3DControl()
        {
            float relativeHeight = _relativeHeight ?? 0;
            List<TriangleDataModel> tris = GetTrianglesWithinDist();

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

            List<Map3DVertex[]> vertexArrayForSurfaces = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), Color4)).ToArray());
            List<Map3DVertex[]> vertexArrayForEdges = allSurfaces.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), OutlineColor)).ToArray());

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
