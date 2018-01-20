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
    public class VarHackManager
    {
        public static VarHackManager Instance;

        public VarHackManager(Control varHackControlControl, WatchVariablePanel variableTable)
        {
            Instance = this;

            /*
            SplitContainer splitContainerCustom = varHackControlControl.Controls["splitContainerCustom"] as SplitContainer;

            Button buttonClearVariables = splitContainerCustom.Panel1.Controls["buttonClearVariables"] as Button;
            buttonClearVariables.Click += (sender, e) => ClearVariables();

            Button buttonResetVariableSizeToDefault = splitContainerCustom.Panel1.Controls["buttonResetVariableSizeToDefault"] as Button;
            buttonResetVariableSizeToDefault.Click += (sender, e) =>
            {
                WatchVariableControl.VariableNameWidth = WatchVariableControl.DEFAULT_VARIABLE_NAME_WIDTH;
                WatchVariableControl.VariableValueWidth = WatchVariableControl.DEFAULT_VARIABLE_VALUE_WIDTH;
                WatchVariableControl.VariableHeight = WatchVariableControl.DEFAULT_VARIABLE_HEIGHT;
            };
            */
        }

        public void Update(bool updateView)
        {
            if (!updateView)
                return;

        }
    }
}
