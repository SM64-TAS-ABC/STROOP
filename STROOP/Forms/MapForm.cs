using STROOP.Controls;
using STROOP.Managers;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Forms
{
    public partial class MapForm : Form, IUpdatableForm
    {
        public MapForm()
        {
            InitializeComponent();
            FormManager.AddForm(this);
            FormClosing += (sender, e) => FormManager.RemoveForm(this);
        }

        public void UpdateForm()
        {
        }

        public void ShowForm()
        {
            Show();
        }
    }
}
