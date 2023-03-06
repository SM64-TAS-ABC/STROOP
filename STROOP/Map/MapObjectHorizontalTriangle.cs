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
using System.Windows.Forms;
using STROOP.Models;

namespace STROOP.Map
{
    public abstract class MapObjectHorizontalTriangle : MapObjectTriangle
    {
        private bool _showTriUnits;
        private bool _truncateBottomOfHitbox;
        private float? _minHeight;
        private float? _maxHeight;
        protected bool _distinguishSlidingClasses;
        protected bool _enableQuarterFrameLandings;

        private ToolStripMenuItem _itemShowTriUnits;
        private ToolStripMenuItem _itemTruncateBottomOfHitbox;
        private ToolStripMenuItem _itemSetMinHeight;
        private ToolStripMenuItem _itemSetMaxHeight;

        private static readonly string SET_MIN_HEIGHT_TEXT = "Set Min Height";
        private static readonly string SET_MAX_HEIGHT_TEXT = "Set Max Height";

        public MapObjectHorizontalTriangle()
            : base()
        {
            _showTriUnits = false;
            _minHeight = null;
            _maxHeight = null;
            _distinguishSlidingClasses = false;
            _enableQuarterFrameLandings = false;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float? minHeight, float? maxHeight, Color color)> drawData = GetDrawData();
            if (_showTriUnits && MapUtilities.IsAbleToShowUnitPrecision())
            {
                List<List<(float x, float z, Color color, TriangleDataModel tri)>> vertexListsForControl =
                    GetVertexListsForControlWithUnits(drawData);
                DrawVertexListsForControlWithUnits(vertexListsForControl, hoverData);
            }
            else
            {
                List<List<(float x, float z, Color color, TriangleDataModel tri)>> vertexListsForControl =
                    GetVertexListsForControlWithoutUnits(drawData);
                DrawVertexListsForControlWithoutUnits(vertexListsForControl, hoverData);
            }
        }

        private List<(float? minHeight, float? maxHeight, Color color)> GetDrawData()
        {
            if (_enableQuarterFrameLandings)
            {
                float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                float marioYSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
                List<(float y, float ySpeed)> steps = new List<(float y, float ySpeed)>();
                for (int i = 0; i < 100 && steps.Count < 10; i++)
                {
                    if (marioYSpeed < 0)
                    {
                        steps.Add((marioY, marioYSpeed));
                    }
                    marioY += marioYSpeed;
                    marioYSpeed = Math.Max(marioYSpeed - 4, -75);
                }

                List<(float yMin, float yMax)> yBounds = new List<(float yMin, float yMax)>();
                foreach ((float y, float ySpeed) in steps)
                {
                    float y0 = y + (0 / 4f) * ySpeed;
                    float y1 = y + (1 / 4f) * ySpeed;
                    float y2 = y + (2 / 4f) * ySpeed;
                    float y3 = y + (3 / 4f) * ySpeed;
                    float y4 = y + (4 / 4f) * ySpeed;
                    yBounds.Add((y1, y0));
                    yBounds.Add((y2, y1));
                    yBounds.Add((y3, y2));
                    yBounds.Add((y4, y3));
                }

                List<(float? minHeight, float? maxHeight, Color color)> drawData =
                    new List<(float? minHeight, float? maxHeight, Color color)>();
                for (int i = 0; i < yBounds.Count; i++)
                {
                    (float yMin, float yMax) = yBounds[i];
                    List<Color> colors = new List<Color>() { Color.Red, Color.Yellow, Color.Green, Color.Cyan };
                    Color color = colors[i % 4];
                    if (_showTriUnits && MapUtilities.IsAbleToShowUnitPrecision())
                    {
                        drawData.Add((yMin, yMax, color));
                    }
                    else
                    {
                        drawData.Add((yMin, MoreMath.GetPreviousFloat(yMax), color));
                    }
                }
                return drawData;
            }
            else
            {
                return new List<(float? minHeight, float? maxHeight, Color color)>()
                {
                    (_minHeight, _maxHeight, Color),
                };
            }
        }

        private List<List<(float x, float z, Color color, TriangleDataModel tri)>> GetVertexListsForControlWithoutUnits(
            List<(float? minHeight, float? maxHeight, Color color)> drawData)
        {
            return drawData.ConvertAll(data =>
            {
                List<List<(float x, float y, float z, TriangleDataModel tri)>> vertexLists =
                    GetVertexListsWithSplicing(data.minHeight, data.maxHeight);
                return vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                        vertex =>
                        {
                            Color color = data.color;
                            if (_distinguishSlidingClasses)
                            {
                                switch (vertex.tri.Slipperiness)
                                {
                                    case 0x13:
                                        color = Color.Red;
                                        break;
                                    case 0x14:
                                        color = Color.Yellow;
                                        break;
                                    case 0x00:
                                        color = Color.Green;
                                        break;
                                    case 0x15:
                                        color = Color.Cyan;
                                        break;
                                }
                            }
                            (float x, float z) = MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z, UseRelativeCoordinates);
                            return (x, z, color, vertex.tri);
                        }));
            }).SelectMany(list => list).ToList();
        }

        private void DrawVertexListsForControlWithoutUnits(
            List<List<(float x, float z, Color color, TriangleDataModel tri)>> vertexListsForControl, MapObjectHoverData hoverData)
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            for (int i = 0; i < vertexListsForControl.Count; i++)
            {
                var vertexList = vertexListsForControl[i];
                GL.Begin(PrimitiveType.Polygon);
                foreach (var vertex in vertexList)
                {
                    byte opacityByte = OpacityByte;
                    if (this == hoverData?.MapObject && vertex.tri == hoverData?.Tri && i == hoverData?.Index && !hoverData.Index2.HasValue)
                    {
                        opacityByte = MapUtilities.GetHoverOpacityByte();
                    }
                    GL.Color4(vertex.color.R, vertex.color.G, vertex.color.B, opacityByte);
                    GL.Vertex2(vertex.x, vertex.z);
                }
                GL.End();
            }

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (var vertexList in vertexListsForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach (var vertex in vertexList)
                    {
                        GL.Vertex2(vertex.x, vertex.z);
                    }
                    GL.End();
                }
            }

            if (_customImage != null)
            {
                for (int i = 0; i < vertexListsForControl.Count; i++)
                {
                    var vertexList = vertexListsForControl[i];
                    for (int j = 0; j < vertexList.Count; j++)
                    {
                        var vertex = vertexList[j];
                        PointF point = new PointF(vertex.x, vertex.z);
                        SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, _iconSize, Scales);
                        double opacity = 1;
                        if (this == hoverData?.MapObject && vertex.tri == hoverData?.Tri && i == hoverData?.Index && j == hoverData?.Index2)
                        {
                            opacity = MapUtilities.GetHoverOpacity();
                        }
                        MapUtilities.DrawTexture(_customImageTex.Value, point, size, 0, opacity);
                    }
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        private List<List<(float x, float z, Color color, TriangleDataModel tri)>> GetVertexListsForControlWithUnits(
            List<(float? minHeight, float? maxHeight, Color color)> drawData)
        {
            List<TriangleDataModel> tris = GetFilteredTriangles();
            List<(int x, int z, Color color, TriangleDataModel tri)> unitPoints =
                drawData.ConvertAll(data =>
                {
                    return tris.ConvertAll(tri =>
                    {
                        int xMin = (int)Math.Max(tri.GetMinX(), Config.CurrentMapGraphics.MapViewXMin - 1);
                        int xMax = (int)Math.Min(tri.GetMaxX(), Config.CurrentMapGraphics.MapViewXMax + 1);
                        int zMin = (int)Math.Max(tri.GetMinZ(), Config.CurrentMapGraphics.MapViewZMin - 1);
                        int zMax = (int)Math.Min(tri.GetMaxZ(), Config.CurrentMapGraphics.MapViewZMax + 1);

                        Color color = data.color;
                        if (_distinguishSlidingClasses)
                        {
                            switch (tri.Slipperiness)
                            {
                                case 0x13:
                                    color = Color.Red;
                                    break;
                                case 0x14:
                                    color = Color.Yellow;
                                    break;
                                case 0x00:
                                    color = Color.Green;
                                    break;
                                case 0x15:
                                    color = Color.Cyan;
                                    break;
                            }
                        }

                        List<(int x, int z, Color color, TriangleDataModel tri)> points =
                            new List<(int x, int z, Color color, TriangleDataModel tri)>();
                        for (int x = xMin; x <= xMax; x++)
                        {
                            for (int z = zMin; z <= zMax; z++)
                            {
                                float? y = tri.GetTruncatedHeightOnTriangleIfInsideTriangle(x, z);
                                if (y.HasValue &&
                                    (!data.minHeight.HasValue || y.Value >= data.minHeight.Value) &&
                                    (!data.maxHeight.HasValue || y.Value <= data.maxHeight.Value))
                                {
                                    points.Add((x, z, color, tri));
                                }
                            }
                        }
                        return points;
                    }).SelectMany(points => points).ToList();
                }).SelectMany(list => list).ToList();

            List<(int x, int z, Color color, TriangleDataModel tri)> unitPointsDistinct =
                new List<(int x, int z, Color color, TriangleDataModel tri)>();
            HashSet<(int x, int z)> alreadySeen = new HashSet<(int x, int z)>();
            foreach (var unitPoint in unitPoints)
            {
                if (alreadySeen.Contains((unitPoint.x, unitPoint.z))) continue;
                alreadySeen.Add((unitPoint.x, unitPoint.z));
                unitPointsDistinct.Add(unitPoint);
            }

            return unitPointsDistinct.ConvertAll(unitPoint =>
            {
                List<(int x, int z)> unitPointList = new List<(int x, int z)>() { (unitPoint.x, unitPoint.z) };
                List<List<(float x, float y, float z)>> quadList = MapUtilities.ConvertUnitPointsToQuads(unitPointList);
                return quadList.ConvertAll(quad =>
                {
                    return quad.ConvertAll(vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z, UseRelativeCoordinates);
                        return (x, z, unitPoint.color, unitPoint.tri);
                    });
                });
            }).SelectMany(list => list).ToList();
        }

        private void DrawVertexListsForControlWithUnits(
            List<List<(float x, float z, Color color, TriangleDataModel tri)>> vertexListsForControl, MapObjectHoverData hoverData)
        {
            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Begin(PrimitiveType.Quads);
            for (int i = 0; i < vertexListsForControl.Count; i++)
            {
                var vertexList = vertexListsForControl[i];
                foreach (var vertex in vertexList)
                {
                    byte opacityByte = OpacityByte;
                    if (this == hoverData?.MapObject && vertex.tri == hoverData?.Tri && i == hoverData?.Index)
                    {
                        opacityByte = MapUtilities.GetHoverOpacityByte();
                    }
                    GL.Color4(vertex.color.R, vertex.color.G, vertex.color.B, opacityByte);
                    GL.Vertex2(vertex.x, vertex.z);
                }
            }
            GL.End();

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (var vertexList in vertexListsForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach (var vertex in vertexList)
                    {
                        GL.Vertex2(vertex.x, vertex.z);
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

        public override bool GetShowTriUnits()
        {
            return _showTriUnits;
        }

        public override bool GetTruncateBottomOfHitbox()
        {
            return _truncateBottomOfHitbox;
        }

        protected List<ToolStripMenuItem> GetHorizontalTriangleToolStripMenuItems()
        {
            _itemShowTriUnits = new ToolStripMenuItem("Show Tri Units");
            _itemShowTriUnits.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeHorizontalTriangleShowTriUnits: true, newHorizontalTriangleShowTriUnits: !_showTriUnits);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemTruncateBottomOfHitbox = new ToolStripMenuItem("Truncate Bottom of Hitbox");
            _itemTruncateBottomOfHitbox.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeHorizontalTriangleTruncateBottomOfHitbox: true, newHorizontalTriangleTruncateBottomOfHitbox: !_truncateBottomOfHitbox);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemSetMinHeight = new ToolStripMenuItem(SET_MIN_HEIGHT_TEXT);
            _itemSetMinHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the min height.");
                float? minHeightNullable =
                    text == "" ?
                    Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!minHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeHorizontalTriangleMinHeight: true, newHorizontalTriangleMinHeight: minHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearMinHeight = new ToolStripMenuItem("Clear Min Height");
            itemClearMinHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeHorizontalTriangleMinHeight: true, newHorizontalTriangleMinHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemSetMaxHeight = new ToolStripMenuItem(SET_MAX_HEIGHT_TEXT);
            _itemSetMaxHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the max height.");
                float? maxHeightNullable =
                    text == "" ?
                    Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!maxHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeHorizontalTriangleMaxHeight: true, newHorizontalTriangleMaxHeight: maxHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearMaxHeight = new ToolStripMenuItem("Clear Max Height");
            itemClearMaxHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeHorizontalTriangleMaxHeight: true, newHorizontalTriangleMaxHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemShowTriUnits,
                _itemTruncateBottomOfHitbox,
                _itemSetMinHeight,
                itemClearMinHeight,
                _itemSetMaxHeight,
                itemClearMaxHeight,
            };
        }

        public override void Update()
        {
            base.Update();

            if (_colorByHeight)
            {
                if (_minHeight.HasValue)
                {
                    _minColorY = Math.Max(_minColorY, _minHeight.Value);
                }
                if (_maxHeight.HasValue)
                {
                    _maxColorY = Math.Min(_maxColorY, _maxHeight.Value);
                }
            }
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeHorizontalTriangleShowTriUnits)
            {
                _showTriUnits = settings.NewHorizontalTriangleShowTriUnits;
                _itemShowTriUnits.Checked = settings.NewHorizontalTriangleShowTriUnits;
            }

            if (settings.ChangeHorizontalTriangleTruncateBottomOfHitbox)
            {
                _truncateBottomOfHitbox = settings.NewHorizontalTriangleTruncateBottomOfHitbox;
                _itemTruncateBottomOfHitbox.Checked = settings.NewHorizontalTriangleTruncateBottomOfHitbox;
            }

            if (settings.ChangeHorizontalTriangleMinHeight)
            {
                _minHeight = settings.NewHorizontalTriangleMinHeight;
                string suffix = _minHeight.HasValue ? string.Format(" ({0})", _minHeight.Value) : "";
                _itemSetMinHeight.Text = SET_MIN_HEIGHT_TEXT + suffix;
            }

            if (settings.ChangeHorizontalTriangleMaxHeight)
            {
                _maxHeight = settings.NewHorizontalTriangleMaxHeight;
                string suffix = _maxHeight.HasValue ? string.Format(" ({0})", _maxHeight.Value) : "";
                _itemSetMaxHeight.Text = SET_MAX_HEIGHT_TEXT + suffix;
            }
        }

        private List<List<(float x, float y, float z, TriangleDataModel tri)>> GetVertexListsWithSplicing(float? minHeight, float? maxHeight)
        {
            List<List<(float x, float y, float z, TriangleDataModel tri)>> vertexLists =
                GetFilteredTriangles().ConvertAll(tri => tri.Get3DVerticesWithTri());
            if (!minHeight.HasValue && !maxHeight.HasValue) return vertexLists; // short circuit

            List<List<(float x, float y, float z, TriangleDataModel tri)>> splicedVertexLists =
                new List<List<(float x, float y, float z, TriangleDataModel tri)>>();
            foreach (List<(float x, float y, float z, TriangleDataModel tri)> vertexList in vertexLists)
            {
                List<(float x, float y, float z, TriangleDataModel tri)> splicedVertexList =
                    new List<(float x, float y, float z, TriangleDataModel tri)>();
                splicedVertexList.AddRange(vertexList);

                float minY = splicedVertexList.Min(vertex => vertex.y);
                float maxY = splicedVertexList.Max(vertex => vertex.y);

                if (minHeight.HasValue)
                {
                    if (minHeight.Value > maxY) continue; // don't add anything
                    if (minHeight.Value > minY)
                    {
                        List<(float x, float y, float z, TriangleDataModel tri)> tempVertexList =
                            new List<(float x, float y, float z, TriangleDataModel tri)>();
                        for (int i = 0; i < splicedVertexList.Count; i++)
                        {
                            (float x1, float y1, float z1, TriangleDataModel tri1) = splicedVertexList[i];
                            (float x2, float y2, float z2, TriangleDataModel tri2) = splicedVertexList[(i + 1) % splicedVertexList.Count];
                            bool isValid1 = y1 >= minHeight.Value;
                            bool isValid2 = y2 >= minHeight.Value;
                            if (isValid1)
                                tempVertexList.Add((x1, y1, z1, tri1));
                            if (isValid1 != isValid2)
                                tempVertexList.Add(InterpolatePointForY(x1, y1, z1, x2, y2, z2, minHeight.Value, tri1));
                        }
                        splicedVertexList.Clear();
                        splicedVertexList.AddRange(tempVertexList);
                    }
                }

                if (maxHeight.HasValue)
                {
                    if (maxHeight.Value < minY) continue; // don't add anything
                    if (maxHeight.Value < maxY)
                    {
                        List<(float x, float y, float z, TriangleDataModel tri)> tempVertexList =
                            new List<(float x, float y, float z, TriangleDataModel tri)>();
                        for (int i = 0; i < splicedVertexList.Count; i++)
                        {
                            (float x1, float y1, float z1, TriangleDataModel tri1) = splicedVertexList[i];
                            (float x2, float y2, float z2, TriangleDataModel tri2) = splicedVertexList[(i + 1) % splicedVertexList.Count];
                            bool isValid1 = y1 <= maxHeight.Value;
                            bool isValid2 = y2 <= maxHeight.Value;
                            if (isValid1)
                                tempVertexList.Add((x1, y1, z1, tri1));
                            if (isValid1 != isValid2)
                                tempVertexList.Add(InterpolatePointForY(x1, y1, z1, x2, y2, z2, maxHeight.Value, tri1));
                        }
                        splicedVertexList.Clear();
                        splicedVertexList.AddRange(tempVertexList);
                    }
                }

                splicedVertexLists.Add(splicedVertexList);
            }
            return splicedVertexLists;
        }

        private (float x, float y, float z, TriangleDataModel tri) InterpolatePointForY(
            float x1, float y1, float z1, float x2, float y2, float z2, float y, TriangleDataModel tri)
        {
            float proportion = (y - y1) / (y2 - y1);
            float x = x1 + proportion * (x2 - x1);
            float z = z1 + proportion * (z2 - z1);
            return (x, y, z, tri);
        }

        private (float x, float z) GetInGameMidpointFromControlQuad(List<(float x, float z)> vertexList)
        {
            List<(float x, float z)> inGameVertexList =
                vertexList.ConvertAll(v => MapUtilities.ConvertCoordsForInGameTopDownView(v.x, v.z));
            float xAverage = inGameVertexList.Average(v => v.x);
            float zAverage = inGameVertexList.Average(v => v.z);
            float xMidpoint = (int)xAverage + (xAverage >= 0 ? 0.5f : -0.5f);
            float zMidpoint = (int)zAverage + (zAverage >= 0 ? 0.5f : -0.5f);
            return (xMidpoint, zMidpoint);
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            bool isShowingTriUnits = _showTriUnits && MapUtilities.IsAbleToShowUnitPrecision();
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            List<(float? minHeight, float? maxHeight, Color color)> drawData = GetDrawData();
            if (_showTriUnits && MapUtilities.IsAbleToShowUnitPrecision())
            {
                List<List<(float x, float z, Color color, TriangleDataModel tri)>> vertexListsForControl =
                    GetVertexListsForControlWithUnits(drawData);
                for (int i = 0; i < vertexListsForControl.Count; i++)
                {
                    var vertexList = vertexListsForControl[i];
                    List<(float x, float z)> simpleVertexList = vertexList.ConvertAll(vertex => (vertex.x, vertex.z));
                    if (MapUtilities.IsWithinShapeForControl(simpleVertexList, relPos.X, relPos.Y, forceCursorPosition))
                    {
                        (float x, float z) inGameMidpoint = GetInGameMidpointFromControlQuad(simpleVertexList);
                        float y = vertexList[0].tri.GetTruncatedHeightOnTriangle(inGameMidpoint.x, inGameMidpoint.z);
                        return new MapObjectHoverData(
                            this, MapObjectHoverDataEnum.Triangle, inGameMidpoint.x, y, inGameMidpoint.z, tri: vertexList[0].tri, isTriUnit: true, index: i);
                    }
                }
                return null;
            }
            else
            {
                List<List<(float x, float z, Color color, TriangleDataModel tri)>> vertexListsForControl =
                    GetVertexListsForControlWithoutUnits(drawData);
                if (_customImage != null)
                {
                    for (int i = 0; i < vertexListsForControl.Count; i++)
                    {
                        var vertexList = vertexListsForControl[i];
                        for (int j = 0; j < vertexList.Count; j++)
                        {
                            var vertex = vertexList[j];
                            double dist = MoreMath.GetDistanceBetween(vertex.x, vertex.z, relPos.X, relPos.Y);
                            double radius = Scales ? _iconSize * Config.CurrentMapGraphics.MapViewScaleValue : _iconSize;
                            if (dist <= radius || forceCursorPosition)
                            {
                                (int x, int y, int z) = j == 0 ? vertex.tri.GetP1() : j == 1 ? vertex.tri.GetP2() : vertex.tri.GetP3();
                                return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, x, y, z, tri: vertex.tri, index: i, index2: j, info: string.Format("V{0}", j + 1));
                            }
                        }
                    }
                }
                for (int i = 0; i < vertexListsForControl.Count; i++)
                {
                    var vertexList = vertexListsForControl[i];
                    List<(float x, float z)> simpleVertexList = vertexList.ConvertAll(vertex => (vertex.x, vertex.z));
                    if (MapUtilities.IsWithinShapeForControl(simpleVertexList, relPos.X, relPos.Y, forceCursorPosition))
                    {
                        TriangleDataModel tri = vertexList[0].tri;
                        return new MapObjectHoverData(
                            this, MapObjectHoverDataEnum.Triangle, tri.GetMidpointX(), tri.GetMidpointY(), tri.GetMidpointZ(), tri: tri, index: i);
                    }
                }
                return null;
            }
        }
    }
}
