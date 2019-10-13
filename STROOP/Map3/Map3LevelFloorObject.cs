using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Drawing.Imaging;
using STROOP.Models;
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.Map3
{
    public class Map3LevelFloorObject : Map3TriangleObject, Map3LevelTriangleObjectI
    {
        private List<uint> _triAddressList;
        private bool _removeCurrentTri;
        private TriangleListForm _triangleListForm;

        public Map3LevelFloorObject()
            : base()
        {
            _triAddressList = TriangleUtilities.GetLevelTriangles()
                .FindAll(tri => tri.IsFloor())
                .ConvertAll(tri => tri.Address);
            _removeCurrentTri = false;
            _triangleListForm = null;

            Opacity = 0.5;
            Color = Color.Blue;
        }

        protected override List<List<(float x, float z)>> GetVertexLists()
        {
            return _triAddressList.ConvertAll(address => new TriangleDataModel(address))
                .ConvertAll(tri => tri.Get2DVertices());
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemReset = new ToolStripMenuItem("Reset");
                itemReset.Click += (sender, e) =>
                {
                    _triAddressList = TriangleUtilities.GetLevelTriangles()
                        .FindAll(tri => tri.IsFloor())
                        .ConvertAll(tri => tri.Address);
                };

                ToolStripMenuItem itemRemoveCurrentTri = new ToolStripMenuItem("Remove Current Tri");
                itemRemoveCurrentTri.Click += (sender, e) =>
                {
                    _removeCurrentTri = !_removeCurrentTri;
                    itemRemoveCurrentTri.Checked = _removeCurrentTri;
                };

                ToolStripMenuItem itemShowTriData = new ToolStripMenuItem("Show Tri Data");
                itemShowTriData.Click += (sender, e) =>
                {
                    List<TriangleDataModel> tris = _triAddressList.ConvertAll(address => new TriangleDataModel(address));
                    TriangleUtilities.ShowTriangles(tris);
                };

                ToolStripMenuItem itemOpenForm = new ToolStripMenuItem("Open Form");
                itemOpenForm.Click += (sender, e) =>
                {
                    if (_triangleListForm != null) return;
                    _triangleListForm = new TriangleListForm(this, TriangleClassification.Floor);
                    _triangleListForm.Show();
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemReset);
                _contextMenuStrip.Items.Add(itemRemoveCurrentTri);
                _contextMenuStrip.Items.Add(itemShowTriData);
                _contextMenuStrip.Items.Add(itemOpenForm);
            }

            return _contextMenuStrip;
        }

        public void NullifyTriangleListForm()
        {
            _triangleListForm = null;
        }

        public override void Update()
        {
            if (_removeCurrentTri)
            {
                uint currentTri = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                _triAddressList.Remove(currentTri);
            }
        }

        public override string GetName()
        {
            return "Level Floor Tris";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
