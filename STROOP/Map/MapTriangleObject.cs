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
    public abstract class MapTriangleObject : MapObject
    {
        private float? _withinDist;
        private float? _withinCenter;
        protected bool _excludeDeathBarriers;
        protected bool _useCrossSection;

        public MapTriangleObject()
            : base()
        {
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
            float centerY = _withinCenter ?? Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            List<TriangleDataModel> tris = GetUnfilteredTriangles()
                .FindAll(tri => tri.IsTriWithinVerticalDistOfCenter(_withinDist, centerY));
            if (_excludeDeathBarriers)
            {
                tris = tris.FindAll(tri => tri.SurfaceType != 0x0A);
            }
            tris = tris.OrderByDescending(tri => tri.Classification).ToList();
            return tris;
        }

        protected abstract List<TriangleDataModel> GetUnfilteredTriangles();

        protected static (float x, float y, float z) OffsetVertex(
            (float x, float y, float z) vertex, float xOffset, float yOffset, float zOffset)
        {
            return (vertex.x + xOffset, vertex.y + yOffset, vertex.z + zOffset);
        }

        public override void DrawOn2DControlSideView()
        {
            if (_useCrossSection)
            {
                DrawOn2DControlSideViewCrossSection();
            }
            else
            {
                DrawOn2DControlSideViewTotal();
            }
        }

        public virtual float GetWallRelativeHeightForSideView()
        {
            return 0;
        }

        public virtual Color GetColorForSideView(TriangleClassification classification)
        {
            return Color;
        }

        public virtual float GetSizeForSideView(TriangleClassification classification)
        {
            return Size;
        }

        public void DrawOn2DControlSideViewCrossSection()
        {
            List<(float x1, float y1, float z1,
                float x2, float y2, float z2,
                TriangleClassification classification, bool xProjection, double pushAngle)> triData =
                GetFilteredTriangles().ConvertAll(tri => MapUtilities.Get2DDataFromTri(tri))
                    .FindAll(data => data.HasValue)
                    .ConvertAll(data => data.Value);

            List<List<(float x, float y, float z, Color color)>> vertexLists = triData.ConvertAll(data =>
            {
                Color color = GetColorForSideView(data.classification);
                float size = GetSizeForSideView(data.classification);
                switch (data.classification)
                {
                    case TriangleClassification.Wall:
                        {
                            double pushAngleRadians = MoreMath.AngleUnitsToRadians(data.pushAngle);
                            double mapViewAngleRadians = MoreMath.AngleUnitsToRadians(Config.MapGraphics.MapViewAngleValue);
                            float relativeHeight = GetWallRelativeHeightForSideView();
                            switch (Config.MapGraphics.MapViewAngleValue)
                            {
                                case 0:
                                case 32768:
                                    if (data.xProjection)
                                    {
                                        float projectionDist = size / (float)Math.Abs(Math.Sin(pushAngleRadians));
                                        return new List<List<(float x, float y, float z, Color color)>>()
                                        {
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2 - projectionDist, data.y2 + relativeHeight, data.z2, color),
                                                (data.x1 - projectionDist, data.y1 + relativeHeight, data.z1, color),
                                            },
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2 + projectionDist, data.y2 + relativeHeight, data.z2, color),
                                                (data.x1 + projectionDist, data.y1 + relativeHeight, data.z1, color),
                                            },
                                        };
                                    }
                                    else
                                    {
                                        return new List<List<(float x, float y, float z, Color color)>>();
                                    }
                                case 16384:
                                case 49152:
                                    if (data.xProjection)
                                    {
                                        return new List<List<(float x, float y, float z, Color color)>>();
                                    }
                                    else
                                    {
                                        float projectionDist = size / (float)Math.Abs(Math.Cos(pushAngleRadians));
                                        return new List<List<(float x, float y, float z, Color color)>>()
                                        {
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2 - projectionDist, color),
                                                (data.x1, data.y1 + relativeHeight, data.z1 - projectionDist, color),
                                            },
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2 + projectionDist, color),
                                                (data.x1, data.y1 + relativeHeight, data.z1 + projectionDist, color),
                                            },
                                        };
                                    }
                                default:
                                    if (data.xProjection)
                                    {
                                        float projectionDist = size / (float)Math.Abs(Math.Cos(mapViewAngleRadians));
                                        return new List<List<(float x, float y, float z, Color color)>>()
                                        {
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y2 + relativeHeight, data.z2 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                                (data.x1 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y1 + relativeHeight, data.z1 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                            },
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y2 + relativeHeight, data.z2 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                                (data.x1 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y1 + relativeHeight, data.z1 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                            },
                                        };
                                    }
                                    else
                                    {
                                        float projectionDist = size / (float)Math.Abs(Math.Sin(mapViewAngleRadians));
                                        return new List<List<(float x, float y, float z, Color color)>>()
                                        {
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y2 + relativeHeight, data.z2 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                                (data.x1 - (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y1 + relativeHeight, data.z1 + (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                            },
                                            new List<(float x, float y, float z, Color color)>()
                                            {
                                                (data.x1, data.y1 + relativeHeight, data.z1, color),
                                                (data.x2, data.y2 + relativeHeight, data.z2, color),
                                                (data.x2 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y2 + relativeHeight, data.z2 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                                (data.x1 + (float)Math.Cos(mapViewAngleRadians) * projectionDist, data.y1 + relativeHeight, data.z1 - (float)Math.Sin(mapViewAngleRadians) * projectionDist, color),
                                            },
                                        };
                                    }
                            }
                        }
                    case TriangleClassification.Floor:
                    case TriangleClassification.Ceiling:
                        {
                            return new List<List<(float x, float y, float z, Color color)>>()
                            {
                                new List<(float x, float y, float z, Color color)>()
                                {
                                    (data.x1, data.y1, data.z1, color),
                                    (data.x2, data.y2, data.z2, color),
                                    (data.x2, data.y2 - size, data.z2, color),
                                    (data.x1, data.y1 - size, data.z1, color),
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
                        (float x, float z) = MapUtilities.ConvertCoordsForControlSideView(vertex.x, vertex.y, vertex.z);
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
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
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

        public void DrawOn2DControlSideViewTotal()
        {
            List<List<(float x, float y, float z, Color color)>> vertexLists =
                GetFilteredTriangles().ConvertAll(tri =>
                {
                    Color color = GetColorForSideView(tri.Classification);
                    return tri.Get3DVertices().ConvertAll(vertex => (vertex.x, vertex.y, vertex.z, color));
                });

            List<List<(float x, float z, Color color)>> vertexListsForControl =
                vertexLists.ConvertAll(vertexList => vertexList.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlSideView(vertex.x, vertex.y, vertex.z);
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
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
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
            ToolStripMenuItem itemSetWithinDist = new ToolStripMenuItem("Set Within Dist");
            itemSetWithinDist.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the vertical distance from the center (default: Mario) within which to show tris.");
                float? withinDistNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!withinDistNullable.HasValue) return;
                _withinDist = withinDistNullable.Value;
            };

            ToolStripMenuItem itemClearWithinDist = new ToolStripMenuItem("Clear Within Dist");
            itemClearWithinDist.Click += (sender, e) =>
            {
                _withinDist = null;
            };

            ToolStripMenuItem itemSetWithinCenter = new ToolStripMenuItem("Set Within Center");
            itemSetWithinCenter.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the center y of the within-dist range.");
                float? withinCenterNullable =
                    text == "" ?
                    Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset) :
                    ParsingUtilities.ParseFloatNullable(text);
                if (!withinCenterNullable.HasValue) return;
                _withinCenter = withinCenterNullable.Value;
            };

            ToolStripMenuItem itemClearWithinCenter = new ToolStripMenuItem("Clear Within Center");
            itemClearWithinCenter.Click += (sender, e) =>
            {
                _withinCenter = null;
            };

            ToolStripMenuItem itemUseCrossSection = new ToolStripMenuItem("Use Cross Section");
            itemUseCrossSection.Click += (sender, e) =>
            {
                _useCrossSection = !_useCrossSection;
                itemUseCrossSection.Checked = _useCrossSection;
            };

            return new List<ToolStripMenuItem>()
            {
                itemSetWithinDist,
                itemClearWithinDist,
                itemSetWithinCenter,
                itemClearWithinCenter,
                itemUseCrossSection,
            };
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
