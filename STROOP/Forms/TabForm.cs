using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class TabForm : Form
    {
        public string StringValue;

        public TabForm()
        {
            InitializeComponent();
        }

        public void AddTab(TabPage tab)
        {
            tabControl1.TabPages.Add(tab);
        }
    }
}
