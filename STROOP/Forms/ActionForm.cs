using STROOP.Models;
using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class ActionForm : Form
    {
        public ActionForm()
        {
            InitializeComponent();

            List<uint> actions = TableConfig.MarioActions.GetActionList();

        }
    }
}
