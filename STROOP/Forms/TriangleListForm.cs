using STROOP.Map;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class TriangleListForm : Form
    {
        private readonly MapLevelTriangleObjectI _levelTriangleObject;
        private readonly List<uint> _triAddressList;
        private long _lastRemoveTime;

        public TriangleListForm(
            MapLevelTriangleObjectI levelTriangleObject, 
            TriangleClassification classification,
            List<uint> triAddressList)
        {
            InitializeComponent();

            _levelTriangleObject = levelTriangleObject;
            _triAddressList = triAddressList;
            _lastRemoveTime = 0;

            Text = classification + " Triangle List";
            labelNumTriangles.Text = _triAddressList.Count + " Triangles";
            FormClosing += (sender, e) => TriangleListFormClosing();
            buttonSort.Click += (sender, e) => RefreshAndSort();
            buttonAnnihilate.Click += (sender, e) => Annihilate();
            buttonInject.Click += (sender, e) => Inject();
            buttonRemove.Click += (sender, e) => Remove();

            RefreshAndSort();
        }

        private void TriangleListFormClosing()
        {
            _levelTriangleObject.NullifyTriangleListForm();
        }

        public void RefreshAndSort()
        {
            dataGridView.Rows.Clear();
            List<(uint address, double dist)> dataList =_triAddressList.ConvertAll(address =>
            {
                TriangleDataModel tri = TriangleDataModel.Create(address);
                double dist = tri.GetDistToMidpoint();
                return (address, dist);
            });
            dataList = Enumerable.OrderBy(dataList, data => data.dist).ToList();
            dataList.ForEach(data =>
            {
                dataGridView.Rows.Add(HexUtilities.FormatValue(data.address), Math.Round(data.dist, 3));
            });
            labelNumTriangles.Text = _triAddressList.Count + " Triangles";
        }

        private void Annihilate()
        {
            List<DataGridViewRow> rows = ControlUtilities.GetTableSelectedRows(dataGridView);
            rows.ForEach(row =>
            {
                uint address = ParsingUtilities.ParseHex(row.Cells[0].Value);
                ButtonUtilities.AnnihilateTriangle(new List<uint>() { address });
            });
        }

        private void Inject()
        {
            Config.GfxManager.InjectHitboxViewCode();
        }

        private void Remove()
        {
            long removeTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            if (removeTime < _lastRemoveTime + 1000)
            {
                DialogUtilities.DisplayMessage("Attempted to remove twice in 1 second.", "Warning");
                return;
            }
            _lastRemoveTime = removeTime;

            List<DataGridViewRow> rows = ControlUtilities.GetTableSelectedRows(dataGridView);
            rows.ForEach(row =>
            {
                uint address = ParsingUtilities.ParseHex(row.Cells[0].Value);
                _triAddressList.Remove(address);
            });
            RefreshDataGridViewAfterRemoval();
        }

        public void RefreshDataGridViewAfterRemoval()
        {
            List<DataGridViewRow> rows = ControlUtilities.GetTableAllRows(dataGridView);
            rows.ForEach(row =>
            {
                uint address = ParsingUtilities.ParseHex(row.Cells[0].Value);
                if (!_triAddressList.Contains(address))
                {
                    dataGridView.Rows.Remove(row);
                }
            });
            labelNumTriangles.Text = _triAddressList.Count + " Triangles";
        }
    }
}
