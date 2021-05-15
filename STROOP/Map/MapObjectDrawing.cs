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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectDrawing : MapObjectLine
    {
        private readonly List<(float x, float y, float z)> _vertices;
        private bool _drawingEnabled;

        private bool _mouseIsDown;
        private (float x, float y, float z) _lastVertex;

        public MapObjectDrawing()
            : base()
        {
            LineWidth = 3;
            LineColor = Color.Red;

            _vertices = new List<(float x, float y, float z)>();
            _drawingEnabled = false;

            _mouseIsDown = false;
        }

        public MapObjectDrawing(List<(float x, float y, float z)> vertices) : this()
        {
            _vertices.AddRange(vertices);
        }

        public static MapObjectDrawing Create(string text)
        {
            List<(double x, double y, double z)> points = MapUtilities.ParsePoints(text, true);
            if (points == null) return null;
            List<(float x, float y, float z)> floatPoints = points.ConvertAll(
                point => ((float)point.x, (float)point.y, (float)point.z));
            return new MapObjectDrawing(floatPoints);
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            return _vertices;
        }

        public override string GetName()
        {
            return "Drawing";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.PathImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemEnableDrawing = new ToolStripMenuItem("Enable Drawing");
                itemEnableDrawing.Click += (sender, e) =>
                {
                    _drawingEnabled = !_drawingEnabled;
                    itemEnableDrawing.Checked = _drawingEnabled;
                    Config.MapManager.NotifyDrawingEnabledChange(_drawingEnabled);
                };

                ToolStripMenuItem itemClearDrawing = new ToolStripMenuItem("Clear Drawing");
                itemClearDrawing.Click += (sender, e) =>
                {
                    _vertices.Clear();
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemEnableDrawing);
                _contextMenuStrip.Items.Add(itemClearDrawing);
            }

            return _contextMenuStrip;
        }

        public override void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY)
        {
            (float x, float z) inGameCoords = MapUtilities.ConvertCoordsForInGame(mouseX, mouseY);
            (float x, float y, float z) currentVertex = (inGameCoords.x, 0, inGameCoords.z);
            switch (mouseEvent)
            {
                case MouseEvent.MouseDown:
                    _mouseIsDown = true;
                    break;
                case MouseEvent.MouseMove:
                    if (_drawingEnabled && _mouseIsDown)
                    {
                        _vertices.Add(_lastVertex);
                        _vertices.Add(currentVertex);
                    }
                    break;
                case MouseEvent.MouseUp:
                    _mouseIsDown = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _lastVertex = currentVertex;
        }

        public override void CleanUp()
        {
            if (_drawingEnabled)
            {
                _drawingEnabled = false;
                Config.MapManager.NotifyDrawingEnabledChange(_drawingEnabled);
            }
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = _vertices.ConvertAll(
                p => string.Format("({0},{1},{2})", (double)p.x, (double)p.y, (double)p.z));
            return new List<XAttribute>()
            {
                new XAttribute("points", string.Join(",", pointList)),
            };
        }
    }
}
