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
    public class MapAllObjectWallObject : MapWallObject
    {
        private readonly List<TriangleDataModel> _tris;
        private bool _autoUpdate;

        public MapAllObjectWallObject()
            : base()
        {
            _tris = TriangleUtilities.GetObjectTriangles()
                .FindAll(tri => tri.IsWall());
            _autoUpdate = true;
        }

        protected override List<TriangleDataModel> GetTrianglesOfAnyDist()
        {
            return _tris;
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

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemAutoUpdate);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetWallToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        public override void Update()
        {
            if (_autoUpdate)
            {
                _tris.Clear();
                _tris.AddRange(TriangleUtilities.GetObjectTriangles()
                    .FindAll(tri => tri.IsWall()));
            }
        }

        public override string GetName()
        {
            return "All Object Wall Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }
    }
}
