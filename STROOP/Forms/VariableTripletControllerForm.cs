using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Forms
{
    public partial class VariableTripletControllerForm : Form
    {
        public VariableTripletControllerForm()
        {
            InitializeComponent();
        }

        public void Initialize(List<WatchVariableControl> controls)
        {

        }

        public void ShowForm()
        {
            Show();
        }
    }
}
