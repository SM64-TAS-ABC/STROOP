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
        private VariablePanel _variablePanel;

        public DataManager(List<VarXControl> varXControlList, VariablePanel variablePanel)
        {
            _variablePanel = variablePanel;
            _variablePanel.AddVariables(varXControlList);
        }

        protected void RemoveObjSpecificVars()
        {
            _variablePanel.RemoveVariables(VariableGroup.ObjectSpecific);
        }

        protected void AddVarXControls(List<VarXControl> varXControls)
        {
            _variablePanel.AddVariables(varXControls);
        }
        
        public virtual void Update(bool updateView = false)
        {
            //if (!updateView) return;
            _variablePanel.UpdateControls();
        }
    }
}
