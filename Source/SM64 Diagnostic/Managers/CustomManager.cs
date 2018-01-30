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
using SM64_Diagnostic.Forms;

namespace SM64_Diagnostic.Managers
{
    public class CustomManager : DataManager
    {
        private CheckBox _checkBoxCustomRecordValues;
        private BetterTextbox _textBoxRecordValuesCount;
        private Button _buttonCustomShowValues;
        private Button _buttonCustomClearValues;
        private CheckBox _checkBoxUseValueAtStartOfGlobalTimer;

        private Dictionary<int, List<string>> _recordedValues;

        public CustomManager(List<WatchVariableControlPrecursor> variables, Control customControl, WatchVariablePanel variableTable)
            : base(variables, variableTable)
        {
            EnableCustomVariableFunctionality();

            SplitContainer splitContainerCustom = customControl.Controls["splitContainerCustom"] as SplitContainer;
            SplitContainer splitContainerCustomControls = splitContainerCustom.Panel1.Controls["splitContainerCustomControls"] as SplitContainer;

            // Panel 1 controls

            Button buttonClearVariables = splitContainerCustomControls.Panel1.Controls["buttonClearVariables"] as Button;
            buttonClearVariables.Click += (sender, e) => ClearVariables();

            _checkBoxCustomRecordValues = splitContainerCustomControls.Panel1.Controls["checkBoxCustomRecordValues"] as CheckBox;
            _checkBoxCustomRecordValues.Click += (sender, e) => ToggleRecording();

            _textBoxRecordValuesCount = splitContainerCustomControls.Panel1.Controls["textBoxRecordValuesCount"] as BetterTextbox;

            _buttonCustomShowValues = splitContainerCustomControls.Panel1.Controls["buttonCustomShowValues"] as Button;
            _buttonCustomShowValues.Click += (sender, e) => ShowRecordedValues();

            _buttonCustomClearValues = splitContainerCustomControls.Panel1.Controls["buttonCustomClearValues"] as Button;
            _buttonCustomClearValues.Click += (sender, e) => ClearRecordedValues();

            _checkBoxUseValueAtStartOfGlobalTimer = splitContainerCustomControls.Panel1.Controls["checkBoxUseValueAtStartOfGlobalTimer"] as CheckBox;

            _recordedValues = new Dictionary<int, List<string>>();

            // Panel 2 controls

            Button buttonResetVariableSizeToDefault = splitContainerCustomControls.Panel2.Controls["buttonResetVariableSizeToDefault"] as Button;
            buttonResetVariableSizeToDefault.Click += (sender, e) =>
            {
                WatchVariableControl.VariableNameWidth = WatchVariableControl.DEFAULT_VARIABLE_NAME_WIDTH;
                WatchVariableControl.VariableValueWidth = WatchVariableControl.DEFAULT_VARIABLE_VALUE_WIDTH;
                WatchVariableControl.VariableHeight = WatchVariableControl.DEFAULT_VARIABLE_HEIGHT;
            };

            GroupBox groupBoxVarNameWidth = splitContainerCustomControls.Panel2.Controls["groupBoxVarNameWidth"] as GroupBox;
            InitializeAddSubtractGetSetFuncionality(
                groupBoxVarNameWidth.Controls["buttonVarNameWidthSubtract"] as Button,
                groupBoxVarNameWidth.Controls["buttonVarNameWidthAdd"] as Button,
                groupBoxVarNameWidth.Controls["buttonVarNameWidthGet"] as Button,
                groupBoxVarNameWidth.Controls["buttonVarNameWidthSet"] as Button,
                groupBoxVarNameWidth.Controls["betterTextboxVarNameWidthAddSubtract"] as TextBox,
                groupBoxVarNameWidth.Controls["betterTextboxVarNameWidthGetSet"] as TextBox,
                (int value) => { WatchVariableControl.VariableNameWidth = value; },
                () => WatchVariableControl.VariableNameWidth);

            GroupBox groupBoxVarValueWidth = splitContainerCustomControls.Panel2.Controls["groupBoxVarValueWidth"] as GroupBox;
            InitializeAddSubtractGetSetFuncionality(
                groupBoxVarValueWidth.Controls["buttonVarValueWidthSubtract"] as Button,
                groupBoxVarValueWidth.Controls["buttonVarValueWidthAdd"] as Button,
                groupBoxVarValueWidth.Controls["buttonVarValueWidthGet"] as Button,
                groupBoxVarValueWidth.Controls["buttonVarValueWidthSet"] as Button,
                groupBoxVarValueWidth.Controls["betterTextboxVarValueWidthAddSubtract"] as TextBox,
                groupBoxVarValueWidth.Controls["betterTextboxVarValueWidthGetSet"] as TextBox,
                (int value) => { WatchVariableControl.VariableValueWidth = value; },
                () => WatchVariableControl.VariableValueWidth);

            GroupBox groupBoxVarHeight = splitContainerCustomControls.Panel2.Controls["groupBoxVarHeight"] as GroupBox;
            InitializeAddSubtractGetSetFuncionality(
                groupBoxVarHeight.Controls["buttonVarHeightSubtract"] as Button,
                groupBoxVarHeight.Controls["buttonVarHeightAdd"] as Button,
                groupBoxVarHeight.Controls["buttonVarHeightGet"] as Button,
                groupBoxVarHeight.Controls["buttonVarHeightSet"] as Button,
                groupBoxVarHeight.Controls["betterTextboxVarHeightAddSubtract"] as TextBox,
                groupBoxVarHeight.Controls["betterTextboxVarHeightGetSet"] as TextBox,
                (int value) => { WatchVariableControl.VariableHeight = value; },
                () => WatchVariableControl.VariableHeight);
        }

        public static void InitializeAddSubtractGetSetFuncionality(
            Button buttonSubtract,
            Button buttonAdd,
            Button buttonGet,
            Button buttonSet,
            TextBox textboxAddSubtract,
            TextBox textboxGetSet,
            Action<int> setterFunction,
            Func<int> getterFunction)
        {
            buttonSubtract.Click += (sender, e) =>
            {
                int? intValueNullable = ParsingUtilities.ParseIntNullable(textboxAddSubtract.Text);
                if (!intValueNullable.HasValue) return;
                int intValue = intValueNullable.Value;
                int newValue = getterFunction() - intValue;
                setterFunction(newValue);
            };

            buttonAdd.Click += (sender, e) =>
            {
                int? intValueNullable = ParsingUtilities.ParseIntNullable(textboxAddSubtract.Text);
                if (!intValueNullable.HasValue) return;
                int intValue = intValueNullable.Value;
                int newValue = getterFunction() + intValue;
                setterFunction(newValue);
            };

            buttonGet.Click += (sender, e) =>
            {
                textboxGetSet.Text = getterFunction().ToString();
            };

            buttonSet.Click += (sender, e) =>
            {
                int? intValueNullable = ParsingUtilities.ParseIntNullable(textboxGetSet.Text);
                if (!intValueNullable.HasValue) return;
                int intValue = intValueNullable.Value;
                setterFunction(intValue);
            };
        }

        public override void AddVariable(WatchVariableControl watchVarControl)
        {
            base.AddVariable(watchVarControl);
            watchVarControl.EnableCustomFunctionality();
        }

        private void ToggleRecording()
        {
            RefreshRateConfig.LimitRefreshRate = !_checkBoxCustomRecordValues.Checked;
        }

        private void ShowRecordedValues()
        {
            InfoForm infoForm = new InfoForm();

            List<string> variableNames = GetCurrentVariableNames();
            List<string> variableValueRowStrings = _recordedValues.ToList()
                .ConvertAll(pair => pair.Key + "\t" + String.Join("\t", pair.Value));
            string variableValueText =
                "Timer\t" + String.Join("\t", variableNames) +"\r\n" +
                String.Join("\r\n", variableValueRowStrings);
            infoForm.SetText(
                "Variable Value Info",
                "Variable Values",
                variableValueText);

            infoForm.Show();
        }

        private void ClearRecordedValues()
        {
            _recordedValues.Clear();
        }

        public override void Update(bool updateView)
        {
            if (_checkBoxCustomRecordValues.Checked)
            {
                int currentTimer = Config.Stream.GetInt32(MiscConfig.GlobalTimerAddress);

                // record new values if we don't already have values
                // or we record at the end of the timer
                if (!_recordedValues.ContainsKey(currentTimer) ||
                    !_checkBoxUseValueAtStartOfGlobalTimer.Checked)
                {
                    List<string> currentValues = GetCurrentVariableValues();
                    _recordedValues[currentTimer] = currentValues;
                }
            }
            _textBoxRecordValuesCount.Text = _recordedValues.Count.ToString();

            if (!updateView) return;
            base.Update(updateView);
        }
    }
}
