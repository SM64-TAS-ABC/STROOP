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
using STROOP.Models;
using System.Windows.Forms;

namespace STROOP.Map
{
    public abstract class MapObjectTriangle : MapObject
    {
        protected bool _showArrows;
        private float? _withinDist;
        private float? _withinCenter;
        protected bool _excludeDeathBarriers;
        protected bool _useCrossSection;

        private ToolStripMenuItem _itemShowArrows;
        private ToolStripMenuItem _itemSetWithinDist;
        private ToolStripMenuItem _itemSetWithinCenter;
        private ToolStripMenuItem _itemUseCrossSection;

        private static readonly string SET_WITHIN_DIST_TEXT = "Set Within Dist";
        private static readonly string SET_WITHIN_CENTER_TEXT = "Set Within Center";

        public MapObjectTriangle()
            : base()
        {
            _showArrows = false;
            _withinDist = null;
            _withinCenter = null;
            _excludeDeathBarriers = false;
            _useCrossSection = false;
        }

        protected List<List<(float x, float y, float z)>> GetVertexLists()
        {
            return GetFilteredTriangles().ConvertAll(tri => tri.Get3DVertices());
        }

        protected List<TriangleDataModel> GetFilteredTriangles()
        {
            float centerY = _withinCenter ?? Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            List<TriangleDataModel> tris = GetUnfilteredTriangles()
                .FindAll(tri => tri.IsTriWithinVerticalDistOfCenter(_withinDist, centerY));
            if (_excludeDeathBarriers)
            {
                tris = tris.FindAll(tri => tri.SurfaceType != 0x0A);
            }
            if (Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
            {
                if (_useCrossSection)
                {
                    tris.Sort((TriangleDataModel t1, TriangleDataModel t2) =>
                    {
                        string string1 = t1.Classification.ToString();
                        string string2 = t2.Classification.ToString();
                        return string1.CompareTo(string2);
                    });
                }
                else
                {
                    tris.Sort((TriangleDataModel t1, TriangleDataModel t2) =>
                    {
                        double dist1 = MapUtilities.GetSignedDistToCameraPlane(t1);
                        double dist2 = MapUtilities.GetSignedDistToCameraPlane(t2);
                        return dist2.CompareTo(dist1);
                    });
                }
            }
            return tris;
        }

        protected abstract List<TriangleDataModel> GetUnfilteredTriangles();

        protected static (float x, float y, float z) OffsetVertex(
            (float x, float y, float z) vertex, float xOffset, float yOffset, float zOffset)
        {
            return (vertex.x + xOffset, vertex.y + yOffset, vertex.z + zOffset);
        }

        public override void DrawOn2DControlOrthographicView()
        {
            if (_useCrossSection)
            {
                DrawOn2DControlOrthographicViewCrossSection();
            }
            else
            {
                DrawOn2DControlOrthographicViewTotal();
            }
        }

        public virtual float GetWallRelativeHeightForOrthographicView()
        {
            return 0;
        }

        public virtual Color GetColorForOrthographicView(TriangleClassification classification)
        {
            return Color;
        }

        public virtual float GetSizeForOrthographicView(TriangleClassification classification)
        {
            return Size;
        }

        public virtual bool GetShowTriUnits()
        {
            return false;
        }

        public void DrawOn2DControlOrthographicViewCrossSection()
        {
            List<TriangleMapData> triData =
                GetFilteredTriangles().ConvertAll(tri => MapUtilities.Get2DDataFromTri(tri))
                    .FindAll(data => data.HasValue)
                    .ConvertAll(data => data.Value);

            List<List<(float x, float y, float z, Color color, TriangleMapData data)>> vertexLists = triData.ConvertAll(data =>
            {
                Color color = GetColorForOrthographicView(data.Tri.Classification);
                float size = GetSizeForOrthographicView(data.Tri.Classification);
                switch (data.Tri.Classification)
                {
                    case TriangleClassification.Wall:
                        {
                            double pushAngleRadians = MoreMath.AngleUnitsToRadians(data.Tri.GetPushAngle());
                            double mapViewAngleRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewYawValue);
                            float relativeHeight = GetWallRelativeHeightForOrthographicView();
                            if (data.Tri.XProjection)
                            {
                                float projectionDist = size / (float)Math.Abs(Math.Cos(mapViewAngleRadians - pushAngleRadians + 0.5 * Math.PI));
                                return new List<List<(float x, float y, float z, Color color, TriangleMapData data)>>()
                                {
                                    new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                    {
                                        (data.X1, data.Y1 + relativeHeight, data.Z1, color, data),
                                        (data.X2, data.Y2 + relativeHeight, data.Z2, color, data),
                                        (data.X2 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y2 + relativeHeight, data.Z2 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                        (data.X1 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y1 + relativeHeight, data.Z1 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                    },
                                    new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                    {
                                        (data.X1, data.Y1 + relativeHeight, data.Z1, color, data),
                                        (data.X2, data.Y2 + relativeHeight, data.Z2, color, data),
                                        (data.X2 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y2 + relativeHeight, data.Z2 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                        (data.X1 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y1 + relativeHeight, data.Z1 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                    },
                                };
                            }
                            else
                            {
                                float projectionDist = size / (float)Math.Abs(Math.Sin(mapViewAngleRadians - pushAngleRadians));
                                return new List<List<(float x, float y, float z, Color color, TriangleMapData data)>>()
                                {
                                    new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                    {
                                        (data.X1, data.Y1 + relativeHeight, data.Z1, color, data),
                                        (data.X2, data.Y2 + relativeHeight, data.Z2, color, data),
                                        (data.X2 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y2 + relativeHeight, data.Z2 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                        (data.X1 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y1 + relativeHeight, data.Z1 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                    },
                                    new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                    {
                                        (data.X1, data.Y1 + relativeHeight, data.Z1, color, data),
                                        (data.X2, data.Y2 + relativeHeight, data.Z2, color, data),
                                        (data.X2 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y2 + relativeHeight, data.Z2 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                        (data.X1 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.Y1 + relativeHeight, data.Z1 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color, data),
                                    },
                                };
                            }
                        }
                    case TriangleClassification.Floor:
                    case TriangleClassification.Ceiling:
                        {
                            if (MapUtilities.IsAbleToShowUnitPrecision() && GetShowTriUnits())
                            {
                                if (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                                    Config.CurrentMapGraphics.MapViewYawValue == 32768)
                                {
                                    int xMin = (int)Math.Max(data.Tri.GetMinX(), Config.CurrentMapGraphics.MapViewXMin);
                                    int xMax = (int)Math.Min(data.Tri.GetMaxX(), Config.CurrentMapGraphics.MapViewXMax);
                                    float z = Config.CurrentMapGraphics.MapViewCenterZValue;
                                    List<List<(float x, float y, float z, Color color, TriangleMapData data)>> output =
                                        new List<List<(float x, float y, float z, Color color, TriangleMapData data)>>();
                                    List<(int xInner, int xOuter)> xPairs = new List<(int xInner, int xOuter)>();
                                    for (int x = xMin; x <= xMax; x++)
                                    {
                                        if (x <= 0) xPairs.Add((x, x - 1));
                                        if (x >= 0) xPairs.Add((x, x + 1));
                                    }
                                    foreach ((int xInner, int xOuter) in xPairs)
                                    {
                                        float? y = data.Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(xInner, z);
                                        if (y.HasValue)
                                        {
                                            output.Add(new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                            {
                                                (xInner, y.Value, z, color, data),
                                                (xOuter, y.Value, z, color, data),
                                                (xOuter, y.Value - size, z, color, data),
                                                (xInner, y.Value - size, z, color, data),
                                            });
                                        }
                                    }
                                    return output;
                                }
                                else if (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
                                    Config.CurrentMapGraphics.MapViewYawValue == 49152)
                                {
                                    int zMin = (int)Math.Max(data.Tri.GetMinZ(), Config.CurrentMapGraphics.MapViewZMin);
                                    int zMax = (int)Math.Min(data.Tri.GetMaxZ(), Config.CurrentMapGraphics.MapViewZMax);
                                    float x = Config.CurrentMapGraphics.MapViewCenterXValue;
                                    List<List<(float x, float y, float z, Color color, TriangleMapData data)>> output =
                                        new List<List<(float x, float y, float z, Color color, TriangleMapData data)>>();
                                    List<(int zInner, int zOuter)> zPairs = new List<(int zInner, int zOuter)>();
                                    for (int z = zMin; z <= zMax; z++)
                                    {
                                        if (z <= 0) zPairs.Add((z, z - 1));
                                        if (z >= 0) zPairs.Add((z, z + 1));
                                    }
                                    foreach ((int zInner, int zOuter) in zPairs)
                                    {
                                        float? y = data.Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(x, zInner);
                                        if (y.HasValue)
                                        {
                                            output.Add(new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                            {
                                                (x, y.Value, zInner, color, data),
                                                (x, y.Value, zOuter, color, data),
                                                (x, y.Value - size, zOuter, color, data),
                                                (x, y.Value - size, zInner, color, data),
                                            });
                                        }
                                    }
                                    return output;
                                }
                                else
                                {
                                    List<List<(float x, float y, float z, Color color, TriangleMapData data)>> output =
                                        new List<List<(float x, float y, float z, Color color, TriangleMapData data)>>();
                                    List<(double x, double z)> points = MapUtilities.GetUnitPointsCrossSection(5);
                                    for (int i = 0; i < points.Count - 1; i++)
                                    {
                                        (float x1, float z1) = ((float x, float z))points[i];
                                        (float x2, float z2) = ((float x, float z))points[i + 1];
                                        int x = (int)(Math.Abs(x1) < Math.Abs(x2) ? x1 : x2);
                                        int z = (int)(Math.Abs(z1) < Math.Abs(z2) ? z1 : z2);
                                        float? y = data.Tri.GetTruncatedHeightOnTriangleIfInsideTriangle(x, z);
                                        if (y.HasValue)
                                        {
                                            output.Add(new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                            {
                                                (x1, y.Value, z1, color, data),
                                                (x2, y.Value, z2, color, data),
                                                (x2, y.Value - size, z2, color, data),
                                                (x1, y.Value - size, z1, color, data),
                                            });
                                        }
                                    }
                                    return output;
                                }
                            }
                            return new List<List<(float x, float y, float z, Color color, TriangleMapData data)>>()
                            {
                                new List<(float x, float y, float z, Color color, TriangleMapData data)>()
                                {
                                    (data.X1, data.Y1, data.Z1, color, data),
                                    (data.X2, data.Y2, data.Z2, color, data),
                                    (data.X2, data.Y2 - size, data.Z2, color, data),
                                    (data.X1, data.Y1 - size, data.Z1, color, data),
                                },
                            };
                        }
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }).SelectMany(list => list).ToList();

            List<List<(float x, float z, Color color)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z);
                        return (x, z, vertex.color);
                    }));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            foreach (List<(float x, float z, Color color)> vertexList in vertexListsForControl)
            {
                GL.Begin(PrimitiveType.Polygon);
                foreach ((float x, float z, Color color) in vertexList)
                {
                    GL.Color4(color.R, color.G, color.B, OpacityByte);
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            // Draw arrows
            if (_showArrows || true) // TODO: UNDO THIS
            {
                foreach (var vertexList in vertexLists)
                {
                    float x1 = (vertexList[0].x + vertexList[3].x) / 2;
                    float y1 = (vertexList[0].y + vertexList[3].y) / 2;
                    float z1 = (vertexList[0].z + vertexList[3].z) / 2;
                    float x2 = (vertexList[1].x + vertexList[2].x) / 2;
                    float y2 = (vertexList[1].y + vertexList[2].y) / 2;
                    float z2 = (vertexList[1].z + vertexList[2].z) / 2;

                    double totalDistance = MoreMath.GetDistanceBetween(x1, y1, z1, x2, y2, z2);
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

                    List<(float x, float y, float z)> markPoints = new List<(float x, float y, float z)>();
                    foreach (double dist in markDistances)
                    {
                        double portion = dist / totalDistance;
                        (double x, double y, double z) point = (x1 + portion * (x2 - x1), y1 + portion * (y2 - y1), z1 + portion * (z2 - z1));
                        markPoints.Add(((float x, float y, float z))point);
                    }

                    double arrowAngle;
                    TriangleDataModel tri = vertexList[0].data.Tri;
                    switch (tri.Classification)
                    {
                        case TriangleClassification.Wall:
                            double angleDiff = MoreMath.GetAngleDifference(Config.CurrentMapGraphics.MapViewYawValue, tri.GetPushAngle());
                            arrowAngle = angleDiff > 0 ? 49152 : 16384;
                            break;
                        case TriangleClassification.Floor:
                            arrowAngle = 32768;
                            break;
                        case TriangleClassification.Ceiling:
                            arrowAngle = 0;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    double arrowBaseLength = 10;
                    double arrowSideLength = 10;

                    double angleUp = arrowAngle;
                    double angleDown = arrowAngle + 32768;
                    double angleLeft = arrowAngle + 16384;
                    double angleRight = arrowAngle - 16384;
                    double angleUpLeft = arrowAngle + 8192;
                    double angleUpRight = arrowAngle - 8192;
                    double angleDownLeft = arrowAngle + 24576;
                    double angleDownRight = arrowAngle - 24576;

                    foreach (var point in markPoints)
                    {
                        var controlPoint = MapUtilities.ConvertCoordsForControlOrthographicView(point.x, point.y, point.z);

                        (float x, float z) frontPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength, angleUp, controlPoint.x, controlPoint.z);
                        (float x, float z) leftOuterPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2 + arrowSideLength, angleLeft, controlPoint.x, controlPoint.z);
                        (float x, float z) leftInnerPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2, angleLeft, controlPoint.x, controlPoint.z);
                        (float x, float z) rightOuterPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2 + arrowSideLength, angleRight, controlPoint.x, controlPoint.z);
                        (float x, float z) rightInnerPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength / 2, angleRight, controlPoint.x, controlPoint.z);
                        (float x, float z) backLeftPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength, angleDown, leftInnerPoint.x, leftInnerPoint.z);
                        (float x, float z) backRightPoint = ((float, float))MoreMath.AddVectorToPoint(
                            arrowBaseLength, angleDown, rightInnerPoint.x, rightInnerPoint.z);

                        List<(float x, float z)> arrowPoints =
                            new List<(float x, float z)>()
                            {
                                frontPoint,
                                leftOuterPoint,
                                leftInnerPoint,
                                backLeftPoint,
                                backRightPoint,
                                rightInnerPoint,
                                rightOuterPoint,
                            };

                        Color arrowColor = vertexList[0].color.Darken(0.5);
                        GL.Color4(arrowColor.R, arrowColor.G, arrowColor.B, OpacityByte);
                        GL.Begin(PrimitiveType.Polygon);
                        foreach (var arrowPoint in arrowPoints)
                        {
                            GL.Vertex2(arrowPoint.x, arrowPoint.z);
                        }
                        GL.End();
                    }
                }
            }

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (List<(float x, float z, Color color)> vertexList in vertexListsForControl)
                {
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

        public void DrawOn2DControlOrthographicViewTotal()
        {
            List<List<(float x, float y, float z, Color color)>> vertexLists =
                GetFilteredTriangles().ConvertAll(tri =>
                {
                    Color color = GetColorForOrthographicView(tri.Classification);
                    return tri.Get3DVertices().ConvertAll(vertex => (vertex.x, vertex.y, vertex.z, color));
                });

            List<List<(float x, float z, Color color)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z);
                        return (x, z, vertex.color);
                    }));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw triangle
            foreach (List<(float x, float z, Color color)> vertexList in vertexListsForControl)
            {
                GL.Begin(PrimitiveType.Polygon);
                foreach ((float x, float z, Color color) in vertexList)
                {
                    GL.Color4(color.R, color.G, color.B, OpacityByte);
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (List<(float x, float z, Color color)> vertexList in vertexListsForControl)
                {
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

        protected List<ToolStripMenuItem> GetTriangleToolStripMenuItems()
        {
            _itemShowArrows = new ToolStripMenuItem("Show Arrows");
            _itemShowArrows.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeTriangleShowArrows: true, newTriangleShowArrows: !_showArrows);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemSetWithinDist = new ToolStripMenuItem(SET_WITHIN_DIST_TEXT);
            _itemSetWithinDist.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the vertical distance from the center (default: Mario) within which to show tris.");
                float? withinDistNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!withinDistNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeTriangleWithinDist: true, newTriangleWithinDist: withinDistNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearWithinDist = new ToolStripMenuItem("Clear Within Dist");
            itemClearWithinDist.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeTriangleWithinDist: true, newTriangleWithinDist: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemSetWithinCenter = new ToolStripMenuItem(SET_WITHIN_CENTER_TEXT);
            _itemSetWithinCenter.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the center y of the within-dist range.");
                float? withinCenterNullable =
                    text == "" ?
                    Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!withinCenterNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeTriangleWithinCenter: true, newTriangleWithinCenter: withinCenterNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            ToolStripMenuItem itemClearWithinCenter = new ToolStripMenuItem("Clear Within Center");
            itemClearWithinCenter.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeTriangleWithinCenter: true, newTriangleWithinCenter: null);
                GetParentMapTracker().ApplySettings(settings);
            };

            _itemUseCrossSection = new ToolStripMenuItem("Use Cross Section");
            _itemUseCrossSection.Click += (sender, e) =>
            {
                MapObjectSettings settings = new MapObjectSettings(
                    changeTriangleUseCrossSection: true, newTriangleUseCrossSection: !_useCrossSection);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemShowArrows,
                _itemSetWithinDist,
                itemClearWithinDist,
                _itemSetWithinCenter,
                itemClearWithinCenter,
                _itemUseCrossSection,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeTriangleShowArrows)
            {
                _showArrows = settings.NewTriangleShowArrows;
                _itemShowArrows.Checked = settings.NewTriangleShowArrows;
            }

            if (settings.ChangeTriangleWithinDist)
            {
                _withinDist = settings.NewTriangleWithinDist;
                string suffix = _withinDist.HasValue ? string.Format(" ({0})", _withinDist.Value) : "";
                _itemSetWithinDist.Text = SET_WITHIN_DIST_TEXT + suffix;
            }

            if (settings.ChangeTriangleWithinCenter)
            {
                _withinCenter = settings.NewTriangleWithinCenter;
                string suffix = _withinCenter.HasValue ? string.Format(" ({0})", _withinCenter.Value) : "";
                _itemSetWithinCenter.Text = SET_WITHIN_CENTER_TEXT + suffix;
            }

            if (settings.ChangeTriangleUseCrossSection)
            {
                _useCrossSection = settings.NewTriangleUseCrossSection;
                _itemUseCrossSection.Checked = settings.NewTriangleUseCrossSection;
            }
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
