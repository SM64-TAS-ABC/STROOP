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
        private readonly List<List<(float x, float y, float z)>> _vertices;
        private bool _drawingEnabled;
        private List<(float x, float y, float z)> _currentStroke;
        private (float x, float y, float z) _lastVertex;

        public MapObjectDrawing()
            : base()
        {
            LineWidth = 3;
            LineColor = Color.Red;

            _vertices = new List<List<(float x, float y, float z)>>();
            _drawingEnabled = false;
            _currentStroke = null;
        }

        public MapObjectDrawing(List<List<(float x, float y, float z)>> vertices) : this()
        {
            _vertices.AddRange(vertices);
        }

        public static MapObjectDrawing Create(string text)
        {
            List<string> strokes = text.Split(';').ToList();
            List<List<(float x, float y, float z)>> vertices = new List<List<(float x, float y, float z)>>();
            foreach (string stroke in strokes)
            {
                List<(double x, double y, double z)> points = MapUtilities.ParsePoints(stroke, true);
                if (points == null) return null;
                List<(float x, float y, float z)> floatPoints = points.ConvertAll(
                    point => ((float)point.x, (float)point.y, (float)point.z));
                vertices.Add(floatPoints);
            }
            return new MapObjectDrawing(vertices);
        }

        protected override List<(float x, float y, float z)> GetVerticesTopDownView()
        {
            return _vertices.SelectMany(list => list).ToList();
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
                    _currentStroke = new List<(float x, float y, float z)>();
                    _vertices.Add(_currentStroke);
                    break;
                case MouseEvent.MouseMove:
                    if (_drawingEnabled && _currentStroke != null)
                    {
                        _currentStroke.Add(_lastVertex);
                        _currentStroke.Add(currentVertex);
                    }
                    break;
                case MouseEvent.MouseUp:
                    _currentStroke = null;
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
            string verticesString = string.Join(";", _vertices.ConvertAll(
                stroke => string.Join(",", stroke.ConvertAll(
                    p => string.Format("({0},{1},{2})", (double)p.x, (double)p.y, (double)p.z)))));
            return new List<XAttribute>()
            {
                new XAttribute("points", verticesString),
            };
        }
    }
}
