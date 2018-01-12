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
        private NoTearFlowLayoutPanel _variableTable;
        protected List<VarXControl> _varXControlList;

        public DataManager(List<VarXControl> varXControlList, NoTearFlowLayoutPanel variableTable)
        {
            _variableTable = variableTable;
            _varXControlList = varXControlList;
            foreach (VarXControl varXControl in varXControlList)
            {
                _variableTable.Controls.Add(varXControl);
            }                
        }

        protected void RemoveObjSpecificVars()
        {
            List<VarXControl> objSpecificsVars =
                _varXControlList.FindAll(
                    varXControl => varXControl.BelongsToGroup(VariableGroup.ObjectSpecific));

            objSpecificsVars.ForEach(objSepcificVar =>
            {
                _varXControlList.Remove(objSepcificVar);
                _variableTable.Controls.Remove(objSepcificVar);
            });
        }

        protected void AddTheseVarXControls(List<VarXControl> varXControls)
        {
            varXControls.ForEach(varXControl =>
            {
                _varXControlList.Add(varXControl);
                _variableTable.Controls.Add(varXControl);
            });
        }
        
        public virtual void Update(bool updateView = false)
        {
            foreach (VarXControl varXControl in _varXControlList)
            {
                varXControl.UpdateControl();
            }
        }
    }
}
