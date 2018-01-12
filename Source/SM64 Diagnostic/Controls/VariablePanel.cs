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
        private readonly Object _objectLock;
        private readonly List<VarXControl> _varXControlsList;
        private readonly List<VariableGroup> _allGroups;
        private readonly List<VariableGroup> _visibleGroups;

        public VariablePanel()
        {
            _objectLock = new Object();
            _varXControlsList = new List<VarXControl>();
            _allGroups = new List<VariableGroup>();
            _visibleGroups = new List<VariableGroup>();
            ContextMenuStrip = new ContextMenuStrip();
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

        public void SetVariableGroups(List<VariableGroup> allGroups, List<VariableGroup> visibleGroups)
        {
            _allGroups.Clear();
            _allGroups.AddRange(allGroups);
            _visibleGroups.Clear();
            _visibleGroups.AddRange(visibleGroups);
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
