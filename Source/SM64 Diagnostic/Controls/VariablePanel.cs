using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VariablePanel : FlowLayoutPanel
    {
        private readonly List<VarXControl> _varXControlsList;
        private readonly Object _objectLock;

        public VariablePanel()
        {
            _varXControlsList = new List<VarXControl>();
            _objectLock = new Object();
            ContextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem item1 = new ToolStripMenuItem("option1");
            ToolStripMenuItem item2 = new ToolStripMenuItem("option2");

            ContextMenuStrip.Items.Add(item1);
            ContextMenuStrip.Items.Add(item2);
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public void AddVariables(List<VarXControl> varXControls)
        {
            lock (_objectLock)
            {
                varXControls.ForEach(varXControl =>
                {
                    _varXControlsList.Add(varXControl);
                    Controls.Add(varXControl);
                });
            }
        }

        public void RemoveVariables(List<VarXControl> varXControls)
        {
            lock(_objectLock)
            {
                varXControls.ForEach(varXControl =>
                {
                    _varXControlsList.Remove(varXControl);
                    Controls.Remove(varXControl);
                });
            }
        }

        public void RemoveVariables(VariableGroup varGroup)
        {
            List<VarXControl> varXControls =
                _varXControlsList.FindAll(
                    varXControl => varXControl.BelongsToGroup(varGroup));
            RemoveVariables(varXControls);
        }

        public void UpdateControls()
        {
            lock (_objectLock)
            {
                _varXControlsList.ForEach(varXControl => varXControl.UpdateControl());
            }
        }
    }
}
