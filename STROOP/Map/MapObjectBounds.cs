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

namespace STROOP.Map
{
    public class MapObjectBounds : MapObject
    {
        public MapObjectBounds()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Magenta;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float y, float z, Color color, bool isHovered)>> quadList =
                new List<List<(float x, float y, float z, Color color, bool isHovered)>>()
                {
                    new List<(float x, float y, float z, Color color, bool isHovered)>()
                    {
                       (-100, 0, -100, Color, false),
                       (-100, 0, 100, Color, false),
                       (100, 0, 100, Color, false),
                       (100, 0, -100, Color, false),
                    }
                };
            List<List<(float x, float z, Color color, bool isHovered)>> quadListForControl =
                quadList.ConvertAll(quad => quad.ConvertAll(
                    vertex =>
                    {
                        (float x, float z) = MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z, UseRelativeCoordinates);
                        return (x, z, vertex.color, vertex.isHovered);
                    }));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Begin(PrimitiveType.Quads);
            foreach (List<(float x, float z, Color color, bool isHovered)> quad in quadListForControl)
            {
                foreach ((float x, float z, Color color, bool isHovered) in quad)
                {
                    GL.Color4(color.R, color.G, color.B, OpacityByte);
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                foreach (List<(float x, float z, Color color, bool isHovered)> quad in quadListForControl)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z, Color color, bool isHovered) in quad)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        public override string GetName()
        {
            return "Bounds";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.WatersImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            //Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            //if (!relPosMaybe.HasValue) return null;
            //Point relPos = relPosMaybe.Value;
            //(float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);
            //var quadList = GetQuadList(null);
            //for (int i = quadList.Count - 1; i >= 0; i--)
            //{
            //    var quad = quadList[i];
            //    var simpleQuad = quad.ConvertAll(q => (q.x, q.y, q.z));
            //    if (MapUtilities.IsWithinRectangularQuad(simpleQuad, inGameX, inGameZ) || forceCursorPosition)
            //    {
            //        return new MapObjectHoverData(this, MapObjectHoverDataEnum.Rectangle, 0, 0, 0, index: i);
            //    }
            //}
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            //Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            //if (!relPosMaybe.HasValue) return null;
            //Point relPos = relPosMaybe.Value;

            //var quadList = GetQuadList(null);
            //for (int i = quadList.Count - 1; i >= 0; i--)
            //{
            //    var quad = quadList[i];
            //    var quadForControl = quad.ConvertAll(p => MapUtilities.ConvertCoordsForControlOrthographicView(p.x, p.y, p.z, UseRelativeCoordinates));
            //    if (MapUtilities.IsWithinShapeForControl(quadForControl, relPos.X, relPos.Y, forceCursorPosition))
            //    {
            //        return new MapObjectHoverData(this, MapObjectHoverDataEnum.Rectangle, 0, 0, 0, index: i);
            //    }
            //}
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            //var quadList = GetQuadList(null);
            //var quad = quadList[hoverData.Index.Value];
            //if (quad.Count == 0) return output;

            //double xMin = quad.Min(p => p.x);
            //double xMax = quad.Max(p => p.x);
            //double zMin = quad.Min(p => p.z);
            //double zMax = quad.Max(p => p.z);
            //double y = quad.Max(p => p.y);

            //ToolStripMenuItem copyXMin = new ToolStripMenuItem(string.Format("Copy X Min ({0})", xMin));
            //ToolStripMenuItem copyXMax = new ToolStripMenuItem(string.Format("Copy X Max ({0})", xMax));
            //ToolStripMenuItem copyZMin = new ToolStripMenuItem(string.Format("Copy Z Min ({0})", zMin));
            //ToolStripMenuItem copyZMax = new ToolStripMenuItem(string.Format("Copy Z Max ({0})", zMax));
            //ToolStripMenuItem copyY = new ToolStripMenuItem(string.Format("Copy Y ({0})", y));

            //copyXMin.Click += (sender, e) => Clipboard.SetText(xMin.ToString());
            //copyXMax.Click += (sender, e) => Clipboard.SetText(xMax.ToString());
            //copyZMin.Click += (sender, e) => Clipboard.SetText(zMin.ToString());
            //copyZMax.Click += (sender, e) => Clipboard.SetText(zMax.ToString());
            //copyY.Click += (sender, e) => Clipboard.SetText(y.ToString());

            //output.Insert(0, copyXMin);
            //output.Insert(1, copyXMax);
            //output.Insert(2, copyZMin);
            //output.Insert(3, copyZMax);
            //output.Insert(4, copyY);

            return output;
        }
    }
}
