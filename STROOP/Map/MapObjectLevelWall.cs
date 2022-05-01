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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectLevelWall : MapObjectWall, MapObjectLevelTriangleInterface
    {
        private readonly List<TriangleDataModel> _triList;
        private bool _removeCurrentTri;
        private TriangleListForm _triangleListForm;
        private bool _autoUpdate;
        private int _numLevelTris;
        private bool _useCurrentCellTris;

        private ToolStripMenuItem _itemUseCurrentCellTris;

        public MapObjectLevelWall()
            : this(TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsWall()))
        {
            _removeCurrentTri = false;
            _triangleListForm = null;
            _autoUpdate = true;
            _numLevelTris = _triList.Count;
            _useCurrentCellTris = false;
        }

        public MapObjectLevelWall(List<TriangleDataModel> triList)
        {
            _triList = triList;
        }

        public static MapObjectLevelWall Create(string text)
        {
            List<uint> triAddressList = MapUtilities.ParseCustomTris(text, null);
            if (triAddressList == null) return null;
            List<TriangleDataModel> triList = triAddressList.ConvertAll(address => TriangleDataModel.CreateLazy(address));
            return new MapObjectLevelWall(triList);
        }

        protected override List<TriangleDataModel> GetUnfilteredTriangles()
        {
            if (_useCurrentCellTris)
            {
                return MapUtilities.GetTriangles(
                    CellUtilities.GetTriangleAddressesInMarioCell(true, TriangleClassification.Wall));
            }
            return _triList;
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

                ToolStripMenuItem itemReset = new ToolStripMenuItem("Reset");
                itemReset.Click += (sender, e) => ResetTriangles();

                ToolStripMenuItem itemRemoveCurrentTri = new ToolStripMenuItem("Remove Current Tri");
                itemRemoveCurrentTri.Click += (sender, e) =>
                {
                    _removeCurrentTri = !_removeCurrentTri;
                    itemRemoveCurrentTri.Checked = _removeCurrentTri;
                };

                ToolStripMenuItem itemShowTriData = new ToolStripMenuItem("Show Tri Data");
                itemShowTriData.Click += (sender, e) =>
                {
                    TriangleUtilities.ShowTriangles(_triList);
                };

                ToolStripMenuItem itemOpenForm = new ToolStripMenuItem("Open Form");
                itemOpenForm.Click += (sender, e) =>
                {
                    if (_triangleListForm != null) return;
                    _triangleListForm = new TriangleListForm(
                        this, TriangleClassification.Wall, _triList);
                    _triangleListForm.Show();
                };

                _itemUseCurrentCellTris = new ToolStripMenuItem("Use Current Cell Tris");
                _itemUseCurrentCellTris.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUseCurrentCellTris: true, newUseCurrentCellTris: !_useCurrentCellTris);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemAutoUpdate);
                _contextMenuStrip.Items.Add(itemReset);
                _contextMenuStrip.Items.Add(itemRemoveCurrentTri);
                _contextMenuStrip.Items.Add(itemShowTriData);
                _contextMenuStrip.Items.Add(itemOpenForm);
                _contextMenuStrip.Items.Add(_itemUseCurrentCellTris);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetWallToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        private void ResetTriangles()
        {
            _triList.Clear();
            _triList.AddRange(TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsWall()));
            _triangleListForm?.RefreshAndSort();
        }

        public void NullifyTriangleListForm()
        {
            _triangleListForm = null;
        }

        public override void Update()
        {
            if (_autoUpdate)
            {
                int numLevelTriangles = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);
                if (_numLevelTris != numLevelTriangles)
                {
                    _numLevelTris = numLevelTriangles;
                    ResetTriangles();
                }
            }

            if (_removeCurrentTri)
            {
                uint currentTriAddress = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
                int index = _triList.FindIndex(tri => tri.Address == currentTriAddress);
                if (index >= 0)
                {
                    _triList.RemoveAt(index);
                    _triangleListForm?.RefreshDataGridViewAfterRemoval();
                }
            }
        }

        public override string GetName()
        {
            return "Level Wall Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeUseCurrentCellTris)
            {
                _useCurrentCellTris = settings.NewUseCurrentCellTris;
                _itemUseCurrentCellTris.Checked = settings.NewUseCurrentCellTris;
            }
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> hexList = _triList.ConvertAll(tri => HexUtilities.FormatValue(tri.Address));
            return new List<XAttribute>()
            {
                new XAttribute("triangles", string.Join(",", hexList)),
            };
        }
    }
}
