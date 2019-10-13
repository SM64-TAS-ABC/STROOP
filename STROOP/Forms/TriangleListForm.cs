using STROOP.Map3;
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
        private readonly Map3LevelTriangleObjectI _levelTriangleObject;
        private readonly List<uint> _triAddressList;

        public TriangleListForm(
            Map3LevelTriangleObjectI levelTriangleObject, 
            TriangleClassification classification,
            List<uint> triAddressList)
        {
            InitializeComponent();

            _levelTriangleObject = levelTriangleObject;
            _triAddressList = triAddressList;

            Text = classification + " Triangle List";
            FormClosing += (sender, e) => TriangleListFormClosing();
            buttonSort.Click += (sender, e) => Sort();
            buttonAnnihilate.Click += (sender, e) => Annihilate();
            buttonInject.Click += (sender, e) => Inject();
            buttonRemove.Click += (sender, e) => Remove();

            Sort();
        }

        private void TriangleListFormClosing()
        {
            _levelTriangleObject.NullifyTriangleListForm();
        }

        private void Sort()
        {
            dataGridView.Rows.Clear();
            List<(uint address, double dist)> dataList =_triAddressList.ConvertAll(address =>
            {
                TriangleDataModel tri = new TriangleDataModel(address);
                double dist = tri.GetDistToMidpoint();
                return (address, dist);
            });
            dataList = Enumerable.OrderBy(dataList, data => data.dist).ToList();
            dataList.ForEach(data =>
            {
                dataGridView.Rows.Add(HexUtilities.FormatValue(data.address), data.dist);
            });
        }

        private void Annihilate()
        {
            List<DataGridViewRow> rows = ControlUtilities.GetTableSelectedRows(dataGridView);
            rows.ForEach(row =>
            {
                uint address = ParsingUtilities.ParseHex(row.Cells[0].Value);
                ButtonUtilities.AnnihilateTriangle(address);
            });
        }

        private void Inject()
        {
            Config.GfxManager.InjectHitboxViewCode();
        }

        private void Remove()
        {
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
        }
    }
}
