using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;

namespace SM64_Diagnostic.ManagerClasses
{
    public class HudManager : DataManager
    {
        Control _tabControl;

        public HudManager(ProcessStream stream, List<WatchVariable> hudData, Control tabControl)
            : base(stream, hudData, tabControl.Controls["NoTearFlowLayoutPanelHud"] as NoTearFlowLayoutPanel)
        {
            _tabControl = tabControl;

            (_tabControl.Controls["buttonFillHp"] as Button).Click += buttonFill_Click;
            (_tabControl.Controls["buttonDie"] as Button).Click += buttonDie_Click;
            (_tabControl.Controls["buttonStandardHud"] as Button).Click += buttonStandardHud_Click;
        }

        private void buttonStandardHud_Click(object sender, EventArgs e)
        {
            MarioActions.StandardHud(_stream);
        }

        private void buttonDie_Click(object sender, EventArgs e)
        {
            MarioActions.Die(_stream);
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            MarioActions.RefillHp(_stream);
        }
    }
}
