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

        public CustomManager(List<WatchVariableControl> variables, Control customControl, WatchVariablePanel variableTable)
            : base(variables, variableTable)
        {
            Instance = this;

            variables.ForEach(variable => variable.NotifyInCustomTab());
        }

        public override void AddVariable(WatchVariableControl watchVarControl)
        {
            base.AddVariable(watchVarControl);
            watchVarControl.NotifyInCustomTab();
        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            base.Update();
        }
    }
}
