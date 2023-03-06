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
    public class MapObjectLevelFloor : MapObjectFloor, MapObjectLevelTriangleInterface
    {
        private readonly List<TriangleDataModel> _triList;
        private bool _removeCurrentTri;
        private TriangleListForm _triangleListForm;
        private bool _autoUpdate;
        private bool _updateOnLevelChange;
        private int _numLevelTris;
        private bool _includeObjectTris;
        private bool _useCurrentCellTris;

        private ToolStripMenuItem _itemAutoUpdate;
        private ToolStripMenuItem _itemUpdateOnLevelChange;
        private ToolStripMenuItem _itemIncludeObjectTris;
        private ToolStripMenuItem _itemUseCurrentCellTris;

        public MapObjectLevelFloor()
            : this(TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsFloor()))
        {
            _removeCurrentTri = false;
            _triangleListForm = null;
            _autoUpdate = false;
            _updateOnLevelChange = true;
            _numLevelTris = _triList.Count;
            _includeObjectTris = false;
            _useCurrentCellTris = false;
        }

        public MapObjectLevelFloor(List<TriangleDataModel> triList)
        {
            _triList = triList;
        }

        public static MapObjectLevelFloor Create(string text)
        {
            List<uint> triAddressList = MapUtilities.ParseCustomTris(text, null);
            if (triAddressList == null) return null;
            List<TriangleDataModel> triList = triAddressList.ConvertAll(address => TriangleDataModel.CreateLazy(address));
            return new MapObjectLevelFloor(triList);
        }

        protected override List<TriangleDataModel> GetUnfilteredTriangles()
        {
            if (_useCurrentCellTris)
            {
                List<TriangleDataModel> tris = MapUtilities.GetTriangles(
                    CellUtilities.GetTriangleAddressesInMarioCell(true, TriangleClassification.Floor));
                if (_includeObjectTris)
                {
                    tris.AddRange(MapUtilities.GetTriangles(
                        CellUtilities.GetTriangleAddressesInMarioCell(false, TriangleClassification.Floor)));
                }
                return tris;
            }
            else
            {
                List<TriangleDataModel> tris = new List<TriangleDataModel>(_triList);
                if (_includeObjectTris)
                {
                    tris.AddRange(TriangleUtilities.GetObjectTriangles().FindAll(tri => tri.IsFloor()));
                }
                return tris;
            }
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

                _itemUpdateOnLevelChange = new ToolStripMenuItem("Update on Level Change");
                _itemUpdateOnLevelChange.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUpdateOnLevelChange: true, newUpdateOnLevelChange: !_updateOnLevelChange);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUpdateOnLevelChange.Checked = _updateOnLevelChange;

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
                        this, TriangleClassification.Floor, _triList);
                    _triangleListForm.Show();
                };

                _itemIncludeObjectTris = new ToolStripMenuItem("Include Object Tris");
                _itemIncludeObjectTris.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeIncludeObjectTris: true, newIncludeObjectTris: !_includeObjectTris);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemUseCurrentCellTris = new ToolStripMenuItem("Use Current Cell Tris");
                _itemUseCurrentCellTris.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeUseCurrentCellTris: true, newUseCurrentCellTris: !_useCurrentCellTris);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemAutoUpdate);
                _contextMenuStrip.Items.Add(_itemUpdateOnLevelChange);
                _contextMenuStrip.Items.Add(itemReset);
                _contextMenuStrip.Items.Add(itemRemoveCurrentTri);
                _contextMenuStrip.Items.Add(itemShowTriData);
                _contextMenuStrip.Items.Add(itemOpenForm);
                _contextMenuStrip.Items.Add(_itemIncludeObjectTris);
                _contextMenuStrip.Items.Add(_itemUseCurrentCellTris);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetFloorToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetHorizontalTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }

        private void ResetTriangles()
        {
            _triList.Clear();
            _triList.AddRange(TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsFloor()));
            _triangleListForm?.RefreshAndSort();
        }

        public void NullifyTriangleListForm()
        {
            _triangleListForm = null;
        }

        public override void Update()
        {
            base.Update();

            if (_autoUpdate)
            {
                ResetTriangles();
            }

            if (_updateOnLevelChange)
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
                uint currentTriAddress = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
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
            return "Level Floor Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeAutoUpdate)
            {
                _autoUpdate = settings.NewAutoUpdate;
                _itemAutoUpdate.Checked = settings.NewAutoUpdate;
            }

            if (settings.ChangeUpdateOnLevelChange)
            {
                _updateOnLevelChange = settings.NewUpdateOnLevelChange;
                _itemUpdateOnLevelChange.Checked = settings.NewUpdateOnLevelChange;
            }

            if (settings.ChangeIncludeObjectTris)
            {
                _includeObjectTris = settings.NewIncludeObjectTris;
                _itemIncludeObjectTris.Checked = settings.NewIncludeObjectTris;
            }

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
