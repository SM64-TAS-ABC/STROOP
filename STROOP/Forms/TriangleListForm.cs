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

            FormClosing += (sender, e) => TriangleListFormClosing();
            Text = classification + " Triangle List";
        }

        private void TriangleListFormClosing()
        {
            _levelTriangleObject.NullifyTriangleListForm();
        }
    }
}
