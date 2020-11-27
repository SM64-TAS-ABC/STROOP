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
        private bool _useCrossSection;

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
            return GetTrianglesWithinDist().ConvertAll(tri => tri.Get3DVertices());
        }

        protected List<TriangleDataModel> GetTrianglesWithinDist()
        {
            float centerY = _withinCenter ?? Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            List<TriangleDataModel> tris = GetTrianglesOfAnyDist()
                .FindAll(tri => tri.IsTriWithinVerticalDistOfCenter(_withinDist, centerY));
            if (_excludeDeathBarriers)
            {
                tris = tris.FindAll(tri => tri.SurfaceType != 0x0A);
            }
            return tris;
        }

        protected abstract List<TriangleDataModel> GetTrianglesOfAnyDist();

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

        public void DrawOn2DControlSideViewCrossSection()
        {
            List<(float x1, float y1, float z1, float x2, float y2, float z2, TriangleClassification classification)> triData =
                GetTrianglesWithinDist().ConvertAll(tri => MapUtilities.Get2DDataFromTri(tri))
                    .FindAll(data => data.HasValue)
                    .ConvertAll(data => data.Value);

            List<List<(float x, float y, float z)>> vertexLists = triData.ConvertAll(data =>
            {
                switch (data.classification)
                {
                    case TriangleClassification.Wall:
                        {
                            return new List<(float x, float y, float z)>(); // TODO FIX THIS
                        }
                    case TriangleClassification.Floor:
                    case TriangleClassification.Ceiling:
                        {
                            return new List<(float x, float y, float z)>()
                            {
                                (data.x1, data.y1, data.z1),
                                (data.x2, data.y2, data.z2),
                                (data.x2, data.y2 - Size, data.z2),
                                (data.x1, data.y1 - Size, data.z1),
                            };
                        }
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

        public void DrawOn2DControlSideViewTotal()
        {
            List<List<(float x, float y, float z)>> vertexLists = GetVertexLists();
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
