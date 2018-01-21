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

        private readonly VarHackPanel _varHackPanel;
        private readonly BinaryButton _buttonEnableDisableRomHack;

        public VarHackManager(Control varHackControlControl, VarHackPanel varHackPanel)
        {
            Instance = this;

            _varHackPanel = varHackPanel;

            SplitContainer splitContainerVarHack =
                varHackControlControl.Controls["splitContainerVarHack"] as SplitContainer;

            Button buttonVarHackAddNewVariable =
                splitContainerVarHack.Panel1.Controls["buttonVarHackAddNewVariable"] as Button;
            buttonVarHackAddNewVariable.Click +=
                (sender, e) => _varHackPanel.AddNewControl();

            Button buttonVarHackClearVariables =
                splitContainerVarHack.Panel1.Controls["buttonVarHackClearVariables"] as Button;
            buttonVarHackClearVariables.Click +=
                (sender, e) => _varHackPanel.ClearControls();

            Button buttonVarHackShowVariableBytesInLittleEndian =
                splitContainerVarHack.Panel1.Controls["buttonVarHackShowVariableBytesInLittleEndian"] as Button;
            buttonVarHackShowVariableBytesInLittleEndian.Click +=
                (sender, e) => _varHackPanel.ShowVariableBytesInLittleEndian();

            Button buttonVarHackShowVariableBytesInBigEndian =
                splitContainerVarHack.Panel1.Controls["buttonVarHackShowVariableBytesInBigEndian"] as Button;
            buttonVarHackShowVariableBytesInBigEndian.Click +=
                (sender, e) => _varHackPanel.ShowVariableBytesInBigEndian();

            Button buttonVarHackApplyVariablesToMemory =
                splitContainerVarHack.Panel1.Controls["buttonVarHackApplyVariablesToMemory"] as Button;
            buttonVarHackApplyVariablesToMemory.Click +=
                (sender, e) => _varHackPanel.ApplyVariablesToMemory();

            Button buttonVarHackClearVariablesInMemory =
                splitContainerVarHack.Panel1.Controls["buttonVarHackClearVariablesInMemory"] as Button;
            buttonVarHackClearVariablesInMemory.Click +=
                (sender, e) => _varHackPanel.ClearVariablesInMemory();

            _buttonEnableDisableRomHack =
                splitContainerVarHack.Panel1.Controls["buttonEnableDisableRomHack"] as BinaryButton;
            _buttonEnableDisableRomHack.Initialize(
                "Enable ROM Hack",
                "Disable ROM Hack",
                () => Config.VarHack.ShowVarRomHack.LoadPayload(),
                () => Config.VarHack.ShowVarRomHack.ClearPayload(),
                () => Config.VarHack.ShowVarRomHack.Enabled);
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            _buttonEnableDisableRomHack.UpdateButton();
        }

        public void AddVariable(string varName, uint address, Type memoryType, bool useHex, uint? pointerOffset)
        {
            _varHackPanel.AddNewControlWithParameters(varName, address, memoryType, useHex, pointerOffset);
        }
    }
}
