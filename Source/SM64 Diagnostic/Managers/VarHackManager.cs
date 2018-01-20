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

        public VarHackManager(Control varHackControlControl, VarHackPanel varHackPanel)
        {
            Instance = this;

            SplitContainer splitContainerVarHack = varHackControlControl.Controls["splitContainerVarHack"] as SplitContainer;

            Button buttonVarHackApplyVariables = splitContainerVarHack.Panel1.Controls["buttonVarHackApplyVariables"] as Button;
            buttonVarHackApplyVariables.Click += (sender, e) =>
            {
                VarHackContainerForm varHackContainerForm =
                    new VarHackContainerForm();
                varHackContainerForm.Show();
            };

            Button buttonVarHackAddNewVariable = splitContainerVarHack.Panel1.Controls["buttonVarHackAddNewVariable"] as Button;
            buttonVarHackAddNewVariable.Click += (sender, e) =>
            {
                VarHackContainer varHackContainer = new VarHackContainer();
                varHackPanel.Controls.Add(varHackContainer);
            };
        }

        public void Update(bool updateView)
        {
            if (!updateView)
                return;

        }
    }
}
