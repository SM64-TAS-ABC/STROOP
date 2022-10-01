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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectCustomWall : MapObjectWall
    {
        private List<uint> _triAddressList;
        private List<TriangleDataModel> _triList;
        private bool _autoUpdate;
        private ToolStripMenuItem _itemAutoUpdate;

        public MapObjectCustomWall(List<uint> triAddressList)
            : base()
        {
            _triAddressList = new List<uint>(triAddressList);
            _triList = triAddressList.ConvertAll(address => TriangleDataModel.CreateLazy(address));
            _autoUpdate = true;
        }

        public static MapObjectCustomWall Create(string text)
        {
            List<uint> triAddressList = MapUtilities.ParseCustomTris(text, TriangleClassification.Wall);
            if (triAddressList == null) return null;
            return new MapObjectCustomWall(triAddressList);
        }

        protected override List<TriangleDataModel> GetUnfilteredTriangles()
        {
            if (_autoUpdate)
            {
                _triList = _triAddressList.ConvertAll(address => TriangleDataModel.CreateLazy(address));
            }
            return _triList;
        }

        public override string GetName()
        {
            return "Custom Wall Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
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

                ToolStripMenuItem addMoreTrisItem = new ToolStripMenuItem("Add More Tris");
                addMoreTrisItem.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter triangle addresses as hex uints.");
                    List<uint> triAddressList = MapUtilities.ParseCustomTris(text, TriangleClassification.Wall);
                    if (triAddressList == null) return;
                    _triList.AddRange(triAddressList.ConvertAll(address => TriangleDataModel.CreateLazy(address)));
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemAutoUpdate);
                _contextMenuStrip.Items.Add(addMoreTrisItem);
                _contextMenuStrip.Items.Add(new ToolStripSeparator());
                GetWallToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
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
