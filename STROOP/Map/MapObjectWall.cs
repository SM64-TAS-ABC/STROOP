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
    public abstract class MapObjectWall : MapObjectTriangle
    {
        private float? _relativeHeight;
        private float? _absoluteHeight;

        private ToolStripMenuItem _itemSetRelativeHeight;
        private ToolStripMenuItem _itemSetAbsoluteHeight;

        private static readonly string SET_RELATIVE_HEIGHT_TEXT = "Set Relative Height";
        private static readonly string SET_ABSOLUTE_HEIGHT_TEXT = "Set Absolute Height";

        public MapObjectWall()
            : base()
        {
            Size = 50;
            Opacity = 0.5;
            Color = Color.Green;

            _relativeHeight = null;
            _absoluteHeight = null;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            float marioHeight = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float? height = _relativeHeight.HasValue ? marioHeight - _relativeHeight.Value : _absoluteHeight;

            List<TriangleMapData> wallDataList = GetFilteredTriangles()
                .ConvertAll(tri => MapUtilities.Get2DWallDataFromTri(tri, height))
                .FindAll(wallDataNullable => wallDataNullable != null);

            foreach (TriangleMapData wallData in wallDataList)
            {
                float angle = (float)MoreMath.AngleTo_Radians(wallData.X1, wallData.Z1, wallData.X2, wallData.Z2);
                float projectionDist = Size / (float)Math.Abs(wallData.Tri.XProjection ? Math.Cos(angle) : Math.Sin(angle));
                List<List<(float x, float z)>> quads = new List<List<(float x, float z)>>();
                void addQuad(float xAdd, float zAdd)
                {
                    quads.Add(new List<(float x, float z)>()
                    {
                        (wallData.X1, wallData.Z1),
                        (wallData.X1 + xAdd, wallData.Z1 + zAdd),
                        (wallData.X2 + xAdd, wallData.Z2 + zAdd),
                        (wallData.X2, wallData.Z2),
                    });
                };
                if (wallData.Tri.XProjection)
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
                        vertex => MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z)));

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw quad
                byte opacityByte = OpacityByte;
                if (this == hoverData?.MapObject && hoverData?.Tri == wallData.Tri && !hoverData.Index.HasValue)
                {
                    opacityByte = MapUtilities.GetHoverOpacityByte();
                }
                GL.Color4(Color.R, Color.G, Color.B, opacityByte);
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
                    double totalDistance = MoreMath.GetDistanceBetween(
                        wallData.X1, wallData.Z1, wallData.X2, wallData.Z2);
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
                        int numMarks = (int)Math.Truncate(distanceDiff / 50 + 0.25) + 1;
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
                        (double x, double z) pointOnMidpoint =
                            (wallData.X1 + portion * (wallData.X2 - wallData.X1), wallData.Z1 + portion * (wallData.Z2 - wallData.Z1));
                        (double x, double z) pointOnSide1 = wallData.Tri.XProjection ?
                            (pointOnMidpoint.x - projectionDist / 2, pointOnMidpoint.z) :
                            (pointOnMidpoint.x, pointOnMidpoint.z - projectionDist / 2);
                        (double x, double z) pointOnSide2 = wallData.Tri.XProjection ?
                            (pointOnMidpoint.x + projectionDist / 2, pointOnMidpoint.z) :
                            (pointOnMidpoint.x, pointOnMidpoint.z + projectionDist / 2);
                        markPoints.Add(((float x, float z))pointOnSide1);
                        markPoints.Add(((float x, float z))pointOnSide2);
                    }

                    markPoints = markPoints.FindAll(p => MapUtilities.IsInVisibleSpace(p.x, p.z, 200));

                    double pushAngle = wallData.Tri.GetPushAngle();
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
                            vertex => MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z)));

                    // Draw arrow
                    Color arrowColor = Color.Darken(0.5);
                    GL.Color4(arrowColor.R, arrowColor.G, arrowColor.B, opacityByte);
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
                if (LineWidth != 0)
                {
                    GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                    GL.LineWidth(LineWidth);
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

                if (_customImage != null)
                {
                    for (int i = 0; i < quadsForControl.Count; i++)
                    {
                        var quad = quadsForControl[i];
                        for (int j = 0; j < quad.Count; j++)
                        {
                            var vertex = quad[j];
                            PointF point = new PointF(vertex.x, vertex.z);
                            SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, _iconSize, Scales);
                            double opacity = 1;
                            if (this == hoverData?.MapObject && hoverData?.Tri == wallData.Tri && i == hoverData?.Index && j == hoverData?.Index2)
                            {
                                opacity = MapUtilities.GetHoverOpacity();
                            }
                            MapUtilities.DrawTexture(_customImageTex.Value, point, size, 0, opacity);
                        }
                    }
                }

                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        public override float GetWallRelativeHeightForOrthographicView()
        {
            return _relativeHeight ?? 0;
        }

        protected List<ToolStripMenuItem> GetWallToolStripMenuItems()
        {
            _itemSetRelativeHeight = new ToolStripMenuItem(SET_RELATIVE_HEIGHT_TEXT);
            _itemSetRelativeHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter relative height of wall hitbox compared to wall triangle.");
                float? relativeHeightNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!relativeHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeWallRelativeHeight: true, newWallRelativeHeight: relativeHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearRelativeHeight = new ToolStripMenuItem("Clear Relative Height");
            itemClearRelativeHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeWallRelativeHeight: true, newWallRelativeHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemSetAbsoluteHeight = new ToolStripMenuItem(SET_ABSOLUTE_HEIGHT_TEXT);
            _itemSetAbsoluteHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the height at which you want to see the wall triangles.");
                float? absoluteHeightNullable =
                    text == "" ?
                    Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!absoluteHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeWallAbsoluteHeight: true, newWallAbsoluteHeight: absoluteHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearAbsoluteHeight = new ToolStripMenuItem("Clear Absolute Height");
            itemClearAbsoluteHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeWallAbsoluteHeight: true, newWallAbsoluteHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemSetRelativeHeight,
                itemClearRelativeHeight,
                _itemSetAbsoluteHeight,
                itemClearAbsoluteHeight,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeWallRelativeHeight)
            {
                _relativeHeight = settings.NewWallRelativeHeight;
                string suffix = _relativeHeight.HasValue ? string.Format(" ({0})", _relativeHeight.Value) : "";
                _itemSetRelativeHeight.Text = SET_RELATIVE_HEIGHT_TEXT + suffix;
            }

            if (settings.ChangeWallAbsoluteHeight)
            {
                _absoluteHeight = settings.NewWallAbsoluteHeight;
                string suffix = _absoluteHeight.HasValue ? string.Format(" ({0})", _absoluteHeight.Value) : "";
                _itemSetAbsoluteHeight.Text = SET_ABSOLUTE_HEIGHT_TEXT + suffix;
            }
        }

        public override void DrawOn3DControl()
        {
            float relativeHeight = _relativeHeight ?? 0;
            List<TriangleDataModel> tris = GetFilteredTriangles();

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

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            float marioHeight = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float? height = _relativeHeight.HasValue ? marioHeight - _relativeHeight.Value : _absoluteHeight;

            List<TriangleMapData> wallDataList = GetFilteredTriangles()
                .ConvertAll(tri => MapUtilities.Get2DWallDataFromTri(tri, height))
                .FindAll(wallDataNullable => wallDataNullable != null);

            for (int i = wallDataList.Count - 1; i >= 0; i--)
            {
                TriangleMapData wallData = wallDataList[i];

                float angle = (float)MoreMath.AngleTo_Radians(wallData.X1, wallData.Z1, wallData.X2, wallData.Z2);
                float projectionDist = Size / (float)Math.Abs(wallData.Tri.XProjection ? Math.Cos(angle) : Math.Sin(angle));
                List<List<(float x, float z)>> quads = new List<List<(float x, float z)>>();
                void addQuad(float xAdd, float zAdd)
                {
                    quads.Add(new List<(float x, float z)>()
                    {
                        (wallData.X1, wallData.Z1),
                        (wallData.X1 + xAdd, wallData.Z1 + zAdd),
                        (wallData.X2 + xAdd, wallData.Z2 + zAdd),
                        (wallData.X2, wallData.Z2),
                    });
                };
                if (wallData.Tri.XProjection)
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
                        vertex => MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z)));

                for (int j = 0; j < quadsForControl.Count; j++)
                {
                    List<(float x, float z)> quadForControl = quadsForControl[j];
                    if (_customImage != null)
                    {
                        for (int k = 0; k < quadForControl.Count; k++)
                        {
                            var vertex = quadForControl[k];
                            double dist = MoreMath.GetDistanceBetween(vertex.x, vertex.z, relPos.X, relPos.Y);
                            double radius = Scales ? _iconSize * Config.CurrentMapGraphics.MapViewScaleValue : _iconSize;
                            if (dist <= radius)
                            {
                                (float x, float z) = MapUtilities.ConvertCoordsForInGame(vertex.x, vertex.z);
                                return new MapObjectHoverData(this, x, 0, z, tri: wallData.Tri, index: j, index2: k);
                            }
                        }
                    }
                    if (MapUtilities.IsWithinShapeForControl(quadForControl, relPos.X, relPos.Y))
                    {
                        return new MapObjectHoverData(
                            this, wallData.Tri.GetMidpointX(), wallData.Tri.GetMidpointY(), wallData.Tri.GetMidpointZ(), tri: wallData.Tri);
                    }
                }
            }

            return null;
        }
    }
}
