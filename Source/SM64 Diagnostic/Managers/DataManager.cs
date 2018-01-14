using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using System.Drawing;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class DataManager
    {
        private WatchVariablePanel _variablePanel;

        public DataManager(
            List<WatchVariableControl> watchVarControlList,
            WatchVariablePanel variablePanel,
            List<VariableGroup> allVariableGroups = null,
            List<VariableGroup> visibleVariableGroups = null)
        {
            _variablePanel = variablePanel;
            if (allVariableGroups != null && visibleVariableGroups != null)
            {
                _variablePanel.SetVariableGroups(allVariableGroups, visibleVariableGroups);
            }
            _variablePanel.AddVariables(watchVarControlList);
        }

        public virtual void RemoveObjSpecificVariables()
        {
            _variablePanel.RemoveVariables(VariableGroup.ObjectSpecific);
        }

        public virtual void AddVariable(WatchVariableControl watchVarControl)
        {
            _variablePanel.AddVariable(watchVarControl);
        }

        public virtual void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            _variablePanel.AddVariables(watchVarControls);
        }
        
        public virtual void Update(bool updateView = false)
        {
            //if (!updateView) return;
            _variablePanel.UpdateControls();
        }
    }
}
