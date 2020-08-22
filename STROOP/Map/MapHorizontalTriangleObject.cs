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
    public abstract class MapHorizontalTriangleObject : MapTriangleObject
    {
        private float? _minHeight;
        private float? _maxHeight;
        protected bool _enableQuarterFrameLandings;

        public MapHorizontalTriangleObject()
            : base()
        {
            _minHeight = null;
            _maxHeight = null;
        }

        public override void DrawOn2DControl()
        {
            if (_enableQuarterFrameLandings)
            {
                float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                float marioYSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
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
                for (int i = 0; i < yBounds.Count; i++)
                {
                    (float yMin, float yMax) = yBounds[i];
                    List<Color> colors = new List<Color>() { Color.Red, Color.Yellow, Color.Green, Color.Cyan };
                    Color color = colors[i % 4];
                    if (ShowTriUnits && MapUtilities.IsAbleToShowUnitPrecision())
                    {
                        DrawOn2DControlWithUnits(yMin, yMax, color);
                    }
                    else
                    {
                        DrawOn2DControlWithoutUnits(yMin, MoreMath.GetPreviousFloat(yMax), color);
                    }
                }
            }
            else
            {
                if (ShowTriUnits && MapUtilities.IsAbleToShowUnitPrecision())
                {
                    DrawOn2DControlWithUnits(_minHeight, _maxHeight, Color);
                }
                else
                {
                    DrawOn2DControlWithoutUnits(_minHeight, _maxHeight, Color);
                }
            }
        }

        private void DrawOn2DControlWithoutUnits(float? minHeight, float? maxHeight, Color color)
        {
            List<List<(float x, float y, float z)>> vertexLists = GetVertexListsWithSplicing(minHeight, maxHeight);
            List<List<(float x, float y, float z)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.y, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            GL.Color4(color.R, color.G, color.B, OpacityByte);
            foreach (List<(float x, float y, float z)> vertexList in vertexListsForControl)
            {
                GL.Begin(PrimitiveType.Polygon);
                foreach ((float x, float y, float z) in vertexList)
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

        private void DrawOn2DControlWithUnits(float? minHeight, float? maxHeight, Color color)
        {
            List<TriangleDataModel> triangles = GetTrianglesWithinDist();
            List<(int x, int z)> unitPoints = triangles.ConvertAll(triangle =>
            {
                int xMin = (int)Math.Max(triangle.GetMinX(), Config.MapGraphics.MapViewXMin - 1);
                int xMax = (int)Math.Min(triangle.GetMaxX(), Config.MapGraphics.MapViewXMax + 1);
                int zMin = (int)Math.Max(triangle.GetMinZ(), Config.MapGraphics.MapViewZMin - 1);
                int zMax = (int)Math.Min(triangle.GetMaxZ(), Config.MapGraphics.MapViewZMax + 1);

                List<(int x, int z)> points = new List<(int x, int z)>();
                for (int x = xMin; x <= xMax; x++)
                {
                    for (int z = zMin; z <= zMax; z++)
                    {
                        float? y = triangle.GetTruncatedHeightOnTriangleIfInsideTriangle(x, z);
                        if (y.HasValue &&
                            (!minHeight.HasValue || y.Value >= minHeight.Value) &&
                            (!maxHeight.HasValue || y.Value <= maxHeight.Value))
                        {
                            points.Add((x, z));
                        }
                    }
                }
                return points;
            }).SelectMany(points => points).Distinct().ToList();

            List<List<(float x, float y, float z)>> quadList = MapUtilities.ConvertUnitPointsToQuads(unitPoints);
            List<List<(float x, float z)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z)));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Color4(color.R, color.G, color.B, OpacityByte);
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

        protected List<ToolStripMenuItem> GetHorizontalTriangleToolStripMenuItems()
        {
            ToolStripMenuItem itemSetMinHeight = new ToolStripMenuItem("Set Min Height");
            itemSetMinHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the min height.");
                float? minHeightNullable =
                    text == "" ?
                    Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!minHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    triangleChangeMinHeight: true, triangleNewMinHeight: minHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearMinHeight = new ToolStripMenuItem("Clear Min Height");
            itemClearMinHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    triangleChangeMinHeight: true, triangleNewMinHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemSetMaxHeight = new ToolStripMenuItem("Set Max Height");
            itemSetMaxHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the max height.");
                float? maxHeightNullable =
                    text == "" ?
                    Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!maxHeightNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    triangleChangeMaxHeight: true, triangleNewMaxHeight: maxHeightNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearMaxHeight = new ToolStripMenuItem("Clear Max Height");
            itemClearMaxHeight.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    triangleChangeMaxHeight: true, triangleNewMaxHeight: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                itemSetMinHeight,
                itemClearMinHeight,
                itemSetMaxHeight,
                itemClearMaxHeight,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.TriangleChangeMinHeight)
            {
                _minHeight = settings.TriangleNewMinHeight;
            }

            if (settings.TriangleChangeMaxHeight)
            {
                _maxHeight = settings.TriangleNewMaxHeight;
            }
        }

        private List<List<(float x, float y, float z)>> GetVertexListsWithSplicing(float? minHeight, float? maxHeight)
        {
            List<List<(float x, float y, float z)>> vertexLists = GetVertexLists();
            if (!minHeight.HasValue && !maxHeight.HasValue) return vertexLists; // short circuit

            List<List<(float x, float y, float z)>> splicedVertexLists = new List<List<(float x, float y, float z)>>();
            foreach (List<(float x, float y, float z)> vertexList in vertexLists)
            {
                List<(float x, float y, float z)> splicedVertexList = new List<(float x, float y, float z)>();
                splicedVertexList.AddRange(vertexList);

                float minY = splicedVertexList.Min(vertex => vertex.y);
                float maxY = splicedVertexList.Max(vertex => vertex.y);

                if (minHeight.HasValue)
                {
                    if (minHeight.Value > maxY) continue; // don't add anything
                    if (minHeight.Value > minY)
                    {
                        List<(float x, float y, float z)> tempVertexList = new List<(float x, float y, float z)>();
                        for (int i = 0; i < splicedVertexList.Count; i++)
                        {
                            (float x1, float y1, float z1) = splicedVertexList[i];
                            (float x2, float y2, float z2) = splicedVertexList[(i + 1) % splicedVertexList.Count];
                            bool isValid1 = y1 >= minHeight.Value;
                            bool isValid2 = y2 >= minHeight.Value;
                            if (isValid1)
                                tempVertexList.Add((x1, y1, z1));
                            if (isValid1 != isValid2)
                                tempVertexList.Add(InterpolatePointForY(x1, y1, z1, x2, y2, z2, minHeight.Value));
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
                        List<(float x, float y, float z)> tempVertexList = new List<(float x, float y, float z)>();
                        for (int i = 0; i < splicedVertexList.Count; i++)
                        {
                            (float x1, float y1, float z1) = splicedVertexList[i];
                            (float x2, float y2, float z2) = splicedVertexList[(i + 1) % splicedVertexList.Count];
                            bool isValid1 = y1 <= maxHeight.Value;
                            bool isValid2 = y2 <= maxHeight.Value;
                            if (isValid1)
                                tempVertexList.Add((x1, y1, z1));
                            if (isValid1 != isValid2)
                                tempVertexList.Add(InterpolatePointForY(x1, y1, z1, x2, y2, z2, maxHeight.Value));
                        }
                        splicedVertexList.Clear();
                        splicedVertexList.AddRange(tempVertexList);
                    }
                }

                splicedVertexLists.Add(splicedVertexList);
            }
            return splicedVertexLists;
        }

        private (float x, float y, float z) InterpolatePointForY(
            float x1, float y1, float z1, float x2, float y2, float z2, float y)
        {
            float proportion = (y - y1) / (y2 - y1);
            float x = x1 + proportion * (x2 - x1);
            float z = z1 + proportion * (z2 - z1);
            return (x, y, z);
        }
    }
}
