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
using STROOP.Forms;

namespace STROOP.Map
{
    public class MapAllObjectFloorObject : MapFloorObject
    {
        private readonly List<TriangleDataModel> _tris;
        private bool _autoUpdate;
        private float? _withinDist;

        public MapAllObjectFloorObject()
            : base()
        {
            _tris = TriangleUtilities.GetObjectTriangles()
                .FindAll(tri => tri.IsFloor());
            _autoUpdate = true;
            _withinDist = null;
        }

        protected override List<TriangleDataModel> GetTriangles()
        {
            return _tris.FindAll(tri => tri.IsMarioWithinVerticalDist(_withinDist));
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemAutoUpdate = new ToolStripMenuItem("Auto Update");
                itemAutoUpdate.Click += (sender, e) =>
                {
                    _autoUpdate = !_autoUpdate;
                    itemAutoUpdate.Checked = _autoUpdate;
                };
                itemAutoUpdate.Checked = _autoUpdate;

                ToolStripMenuItem itemSetWithinDist = new ToolStripMenuItem("Set Within Dist");
                itemSetWithinDist.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter the vertical distance from Mario within which to show tris.");
                    float? withinDistNullable = ParsingUtilities.ParseFloatNullable(text);
                    if (!withinDistNullable.HasValue) return;
                    _withinDist = withinDistNullable.Value;
                };

                ToolStripMenuItem itemClearWithinDist = new ToolStripMenuItem("Clear Within Dist");
                itemClearWithinDist.Click += (sender, e) =>
                {
                    _withinDist = null;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemAutoUpdate);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                _contextMenuStrip.Items.Add(itemSetWithinDist);
                _contextMenuStrip.Items.Add(itemClearWithinDist);
            }

            return _contextMenuStrip;
        }

        public override void Update()
        {
            if (_autoUpdate)
            {
                _tris.Clear();
                _tris.AddRange(TriangleUtilities.GetObjectTriangles()
                    .FindAll(tri => tri.IsFloor()));
            }
        }

        public override string GetName()
        {
            return "All Object Floor Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
