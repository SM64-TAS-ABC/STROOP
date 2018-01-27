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

namespace SM64_Diagnostic.Managers
{
    public class VarHackManager
    {
        public static VarHackManager Instance;

        private readonly VarHackPanel _varHackPanel;
        private readonly BinaryButton _buttonEnableDisableRomHack;

        private readonly BetterTextbox _textBoxXPosValue;
        private readonly BetterTextbox _textBoxYPosValue;
        private readonly BetterTextbox _textBoxYDeltaValue;

        public VarHackManager(Control varHackControlControl, VarHackPanel varHackPanel)
        {
            Instance = this;

            _varHackPanel = varHackPanel;

            // Top buttons

            SplitContainer splitContainerVarHack =
                varHackControlControl.Controls["splitContainerVarHack"] as SplitContainer;

            Button buttonVarHackAddNewVariable =
                splitContainerVarHack.Panel1.Controls["buttonVarHackAddNewVariable"] as Button;
            buttonVarHackAddNewVariable.Click +=
                (sender, e) => _varHackPanel.AddNewControl();

            ControlUtilities.AddContextMenuStripFunctions(
                buttonVarHackAddNewVariable,
                new List<string>()
                {
                    "RNG Index",
                    "Floor YNorm",
                    "Defacto Speed",
                    "Mario Action",
                    "Mario Animation",
                },
                new List<Action>()
                {
                    () => AddVariable(() => "Index " + RngIndexer.GetRngIndex()),
                    () => AddVariable(() =>
                    {
                        uint triFloorAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                        float yNorm = Config.Stream.GetSingle(triFloorAddress + TriangleOffsetsConfig.NormY);
                        return "YNorm " + FormatDouble(yNorm, 4, true);
                    }),
                    () => AddVariable(() => "Defacto " + FormatInteger(WatchVariableSpecialUtilities.GetMarioDeFactoSpeed())),
                    () => AddVariable(() => "Action " + Config.MarioActions.GetActionName()),
                    () => AddVariable(() => "Animation " + Config.MarioAnimations.GetAnimationName()),
                });

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

            // Bottom buttons

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

            // Middle buttons

            _textBoxXPosValue = splitContainerVarHack.Panel1.Controls["textBoxXPosValue"] as BetterTextbox;
            _textBoxXPosValue.AddEnterAction(() => SetPositionsAndApplyVariablesToMemory());
            _textBoxXPosValue.Text = Config.VarHack.DefaultXPos.ToString();
            InitializePositionControls(
                _textBoxXPosValue,
                splitContainerVarHack.Panel1.Controls["textBoxXPosChange"] as TextBox,
                splitContainerVarHack.Panel1.Controls["buttonXPosSubtract"] as Button,
                splitContainerVarHack.Panel1.Controls["buttonXPosAdd"] as Button);

            _textBoxYPosValue = splitContainerVarHack.Panel1.Controls["textBoxYPosValue"] as BetterTextbox;
            _textBoxYPosValue.AddEnterAction(() => SetPositionsAndApplyVariablesToMemory());
            _textBoxYPosValue.Text = Config.VarHack.DefaultYPos.ToString();
            InitializePositionControls(
                _textBoxYPosValue,
                splitContainerVarHack.Panel1.Controls["textBoxYPosChange"] as TextBox,
                splitContainerVarHack.Panel1.Controls["buttonYPosSubtract"] as Button,
                splitContainerVarHack.Panel1.Controls["buttonYPosAdd"] as Button);

            _textBoxYDeltaValue = splitContainerVarHack.Panel1.Controls["textBoxYDeltaValue"] as BetterTextbox;
            _textBoxYDeltaValue.AddEnterAction(() => SetPositionsAndApplyVariablesToMemory());
            _textBoxYDeltaValue.Text = Config.VarHack.DefaultYDelta.ToString();
            InitializePositionControls(
                _textBoxYDeltaValue,
                splitContainerVarHack.Panel1.Controls["textBoxYDeltaChange"] as TextBox,
                splitContainerVarHack.Panel1.Controls["buttonYDeltaSubtract"] as Button,
                splitContainerVarHack.Panel1.Controls["buttonYDeltaAdd"] as Button);

            Button buttonSetPositionsAndApplyVariablesToMemory =
                splitContainerVarHack.Panel1.Controls["buttonSetPositionsAndApplyVariablesToMemory"] as Button;
            buttonSetPositionsAndApplyVariablesToMemory.Click +=
                (sender, e) => SetPositionsAndApplyVariablesToMemory();
        }

        private void InitializePositionControls(
            TextBox valueTextbox,
            TextBox changeTextbox,
            Button subtractButton,
            Button addButton)
        {
            subtractButton.Click += (sender, e) =>
            {
                int? change = ParsingUtilities.ParseIntNullable(changeTextbox.Text);
                if (!change.HasValue) return;
                int? oldValue = ParsingUtilities.ParseIntNullable(valueTextbox.Text);
                if (!oldValue.HasValue) return;
                int newValue = oldValue.Value - change.Value;
                valueTextbox.Text = newValue.ToString();
                SetPositionsAndApplyVariablesToMemory();
            };

            addButton.Click += (sender, e) =>
            {
                int? change = ParsingUtilities.ParseIntNullable(changeTextbox.Text);
                if (!change.HasValue) return;
                int? oldValue = ParsingUtilities.ParseIntNullable(valueTextbox.Text);
                if (!oldValue.HasValue) return;
                int newValue = oldValue.Value + change.Value;
                valueTextbox.Text = newValue.ToString();
                SetPositionsAndApplyVariablesToMemory();
            };
        }

        private void SetPositionsAndApplyVariablesToMemory()
        {
            int? xPos = ParsingUtilities.ParseIntNullable(_textBoxXPosValue.Text);
            int? yPos = ParsingUtilities.ParseIntNullable(_textBoxYPosValue.Text);
            int? yDelta = ParsingUtilities.ParseIntNullable(_textBoxYDeltaValue.Text);
            if (!xPos.HasValue || !yPos.HasValue || !yDelta.HasValue) return;
            _varHackPanel.SetPositions(xPos.Value, yPos.Value, yDelta.Value);
            _varHackPanel.ApplyVariablesToMemory();
        }

        public void AddVariable(string varName, uint address, Type memoryType, bool useHex, uint? pointerOffset)
        {
            _varHackPanel.AddNewControlWithParameters(varName, address, memoryType, useHex, pointerOffset);
        }

        public void AddVariable(Func<string> getterFunction)
        {
            _varHackPanel.AddNewControlWithGetterFunction(getterFunction);
        }

        public string FormatDouble(double value, int numDigits = 4, bool usePadding = true)
        {
            string stringValue = Math.Round(value, numDigits).ToString();
            if (usePadding)
            {
                int decimalIndex = stringValue.IndexOf(".");
                if (decimalIndex == -1)
                {
                    stringValue += ".";
                    decimalIndex = stringValue.Length - 1;
                }
                while (stringValue.Length <= decimalIndex + numDigits)
                {
                    stringValue += "0";
                }
            }
            stringValue = stringValue.Replace(".", Config.VarHack.CoinChar);
            return stringValue;
        }

        public string FormatInteger(double value)
        {
            string stringValue = Math.Truncate(value).ToString();
            stringValue = stringValue.Replace("-", "M");
            return stringValue;
        }

        public void Update(bool updateView)
        {
            _varHackPanel.UpdateControls();

            if (!updateView) return;

            _buttonEnableDisableRomHack.UpdateButton();
        }
    }
}
