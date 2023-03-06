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
    public class MapObjectAllObjectCeiling : MapObjectCeiling
    {
        private readonly List<TriangleDataModel> _tris;
        private bool _autoUpdate;
        private bool _useCurrentCellTris;

        private ToolStripMenuItem _itemAutoUpdate;
        private ToolStripMenuItem _itemUseCurrentCellTris;

        public MapObjectAllObjectCeiling()
            : base()
        {
            _tris = TriangleUtilities.GetObjectTriangles()
                .FindAll(tri => tri.IsCeiling());
            _autoUpdate = true;
            _useCurrentCellTris = false;
        }

        protected override List<TriangleDataModel> GetUnfilteredTriangles()
        {
            if (_useCurrentCellTris)
            {
                return MapUtilities.GetTriangles(
                    CellUtilities.GetTriangleAddressesInMarioCell(false, TriangleClassification.Ceiling));
            }
            return _tris;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemAutoUpdate = new ToolStripMenuItem("Auto Update");
                _itemAutoUpdate.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeAutoUpdate: true, newAutoUpdate: !_autoUpdate);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemAutoUpdate.Checked = _autoUpdate;

                _itemUseCurrentCellTris = new ToolStripMenuItem("Use Current Cell Tris");
                _itemUseCurrentCellTris.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUseCurrentCellTris: true, newUseCurrentCellTris: !_useCurrentCellTris);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemAutoUpdate);
                _contextMenuStrip.Items.Add(_itemUseCurrentCellTris);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetHorizontalTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeAutoUpdate)
            {
                _autoUpdate = settings.NewAutoUpdate;
                _itemAutoUpdate.Checked = settings.NewAutoUpdate;
            }

            if (settings.ChangeUseCurrentCellTris)
            {
                _useCurrentCellTris = settings.NewUseCurrentCellTris;
                _itemUseCurrentCellTris.Checked = settings.NewUseCurrentCellTris;
            }
        }

        public override void Update()
        {
            base.Update();

            if (_autoUpdate)
            {
                _tris.Clear();
                _tris.AddRange(TriangleUtilities.GetObjectTriangles()
                    .FindAll(tri => tri.IsCeiling()));
            }
        }

        public override string GetName()
        {
            return "All Object Ceiling Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleCeilingImage;
        }
    }
}
