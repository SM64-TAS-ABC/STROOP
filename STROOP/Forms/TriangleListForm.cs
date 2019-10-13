using STROOP.Map3;
using STROOP.Models;
using STROOP.Structs;
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
            Enumerable.OrderBy(dataList, data => data.dist);
            dataList.ForEach(data =>
            {
                dataGridView.Rows.Add(HexUtilities.FormatValue(data.address), data.dist);
            });
        }

        private void Annihilate()
        {

        }

        private void Inject()
        {

        }

        private void Remove()
        {

        }

        public void RefreshDataGridViewAfterRemoval()
        {

        }
    }
}
