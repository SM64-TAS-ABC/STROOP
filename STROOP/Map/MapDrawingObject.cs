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
    public class MapDrawingObject : MapLineObject
    {
        private readonly List<(float x, float y, float z)> _vertices;
        private bool _drawingEnabled;

        public MapDrawingObject()
            : base()
        {
            OutlineWidth = 3;
            OutlineColor = Color.Red;

            _vertices = new List<(float x, float y, float z)>();
            _drawingEnabled = false;
        }

        protected override List<(float x, float y, float z)> GetVertices()
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

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemEnableDrawing);
            }

            return _contextMenuStrip;
        }
    }
}
