using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class CustomManager : DataManager
    {
        public static CustomManager Instance;

        public CustomManager(List<VarXControl> variables, Control customControl, VariablePanel variableTable)
            : base(variables, variableTable)
        {
            Instance = this;
        }

        public void AddVariable(VarXControl varXControl)
        {
            AddVarXControls(new List<VarXControl>() { varXControl });
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            base.Update();
        }
    }
}
