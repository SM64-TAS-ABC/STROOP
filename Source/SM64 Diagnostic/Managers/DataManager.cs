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

        public DataManager(List<WatchVariableControl> watchVarControlList, WatchVariablePanel variablePanel)
        {
            _variablePanel = variablePanel;
            _variablePanel.AddVariables(watchVarControlList);
        }

        protected void RemoveObjSpecificVars()
        {
            _variablePanel.RemoveVariables(VariableGroup.ObjectSpecific);
        }

        protected void AddwatchVarControls(List<WatchVariableControl> watchVarControls)
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
