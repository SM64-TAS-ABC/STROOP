using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace STROOP.Map3
{
    public abstract class Map3WallObject : Map3TriangleObject
    {
        private float? _relativeHeight;

        public Map3WallObject()
            : base()
        {
            Size = 50;
            Opacity = 0.5;
            Color = Color.Green;

            _relativeHeight = null;
        }

        protected List<(float x1, float z1, float x2, float z2, bool xProjection)> Get2DWallData()
        {
            float marioHeight = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float? height = _relativeHeight.HasValue ? marioHeight - _relativeHeight.Value : (float?)null;
            return GetTriangles()
                .ConvertAll(tri => Map3Utilities.GetWallDataFromTri(tri, height))
                .FindAll(wallDataNullable => wallDataNullable.HasValue)
                .ConvertAll(wallDataNullable => wallDataNullable.Value);
        }

        public override void DrawOn2DControl()
        {
            List<(float x1, float z1, float x2, float z2, bool xProjection)> wallData = Get2DWallData();
            foreach ((float x1, float z1, float x2, float z2, bool xProjection) in wallData)
            {
                float angle = (float)MoreMath.AngleTo_Radians(x1, z1, x2, z2);
                float projection = Size / (float)Math.Abs(xProjection ? Math.Cos(angle) : Math.Sin(angle));
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
                    addQuad(projection, 0);
                    addQuad(-1 * projection, 0);
                }
                else
                {
                    addQuad(0, projection);
                    addQuad(0, -1 * projection);
                }

                List<List<(float x, float z)>> quadsForControl =
                    quads.ConvertAll(quad => quad.ConvertAll(
                        vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z)));

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

        protected BetterContextMenuStrip CreateWallContextMenuStrip()
        {
            ToolStripMenuItem itemSetRelativeHeight = new ToolStripMenuItem("Set Relative Height");
            itemSetRelativeHeight.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter relative height of wall hitbox compared to wall triangle.");
                float? relativeHeightNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!relativeHeightNullable.HasValue) return;
                _relativeHeight = relativeHeightNullable.Value;
            };

            ToolStripMenuItem itemClearRelativeHeight = new ToolStripMenuItem("Clear Relative Height");
            itemClearRelativeHeight.Click += (sender, e) =>
            {
                _relativeHeight = null;
            };

            BetterContextMenuStrip contextMenuStrip = new BetterContextMenuStrip();
            contextMenuStrip.AddToEndingList(itemSetRelativeHeight);
            contextMenuStrip.AddToEndingList(itemClearRelativeHeight);

            return contextMenuStrip;
        }
    }
}
