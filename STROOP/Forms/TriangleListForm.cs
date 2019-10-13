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

        public TriangleListForm(
            Map3LevelTriangleObjectI levelTriangleObject, TriangleClassification classification)
        {
            InitializeComponent();

            _levelTriangleObject = levelTriangleObject;

            FormClosing += (sender, e) => TriangleListFormClosing();
            Text = classification + " Triangle List";
        }

        private void TriangleListFormClosing()
        {
            _levelTriangleObject.NullifyTriangleListForm();
        }
    }
}
