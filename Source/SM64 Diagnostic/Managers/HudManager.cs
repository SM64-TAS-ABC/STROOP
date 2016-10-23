using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.ManagerClasses
{
    public class HudManager
    {
        Config _config;
        List<WatchVariableControl> _watchVarControls;
        Control _tabControl;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;

        public HudManager(ProcessStream stream, Config config, List<WatchVariable> data, Control tabControl)
        {
            _config = config;
            _tabControl = tabControl;
            _stream = stream;

            _variableTable = _tabControl.Controls["flowLayoutPanelHud"] as FlowLayoutPanel;
            
            _watchVarControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in data)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, 0);
                _variableTable.Controls.Add(watchControl.Control);
                _watchVarControls.Add(watchControl);
            }

            (_tabControl.Controls["buttonFillHp"] as Button).Click += buttonFill_Click;
            (_tabControl.Controls["buttonDie"] as Button).Click += buttonDie_Click;
            (_tabControl.Controls["buttonStandardHud"] as Button).Click += buttonStandardHud_Click;
        }

        private void buttonStandardHud_Click(object sender, EventArgs e)
        {
            MarioActions.StandardHud(_stream, _config);
        }

        private void buttonDie_Click(object sender, EventArgs e)
        {
            MarioActions.Die(_stream, _config);
        }

        private void buttonFill_Click(object sender, EventArgs e)
        {
            MarioActions.RefillHp(_stream, _config);
        }

        public virtual void Update(bool updateView)
        {
            // Update watch variables
            foreach (var watchVar in _watchVarControls)
                watchVar.Update();

            if (!updateView)
                return;
        }
    }
}
