using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Forms;
using System.IO;
using System.Xml.Linq;

namespace STROOP.Managers
{
    public class CustomManager : DataManager
    {
        private CheckBox _checkBoxCustomRecordValues;
        private BetterTextbox _textBoxRecordValuesCount;
        private Button _buttonCustomShowValues;
        private Button _buttonCustomClearValues;
        private CheckBox _checkBoxUseValueAtStartOfGlobalTimer;
        private Label _labelCustomRecordingFrequencyValue;
        private Label _labelCustomRecordingGapsValue;

        private Dictionary<int, List<string>> _recordedValues;
        private int? _lastTimer;
        private int _numGaps;
        private int _recordFreq;

        public CustomManager(List<WatchVariableControlPrecursor> variables, Control customControl, WatchVariableFlowLayoutPanel variableTable)
            : base(variables, variableTable)
        {
            EnableCustomVariableFunctionality();

            SplitContainer splitContainerCustom = customControl.Controls["splitContainerCustom"] as SplitContainer;
            SplitContainer splitContainerCustomControls = splitContainerCustom.Panel1.Controls["splitContainerCustomControls"] as SplitContainer;

            // Panel 1 controls

            Button buttonOpenVars = splitContainerCustomControls.Panel1.Controls["buttonOpenVars"] as Button;
            buttonOpenVars.Click += (sender, e) => OpenVariables();

            Button buttonSaveVars = splitContainerCustomControls.Panel1.Controls["buttonSaveVars"] as Button;
            buttonSaveVars.Click += (sender, e) => SaveVariables();

            Button buttonClearVars = splitContainerCustomControls.Panel1.Controls["buttonClearVars"] as Button;
            buttonClearVars.Click += (sender, e) => ClearVariables();
            ControlUtilities.AddContextMenuStripFunctions(
                buttonClearVars,
                new List<string>() { "Clear All Vars", "Clear Default Vars" },
                new List<Action>()
                {
                    () => ClearVariables(),
                    () => RemoveVariableGroup(VariableGroup.Custom),
                });

            _checkBoxCustomRecordValues = splitContainerCustomControls.Panel1.Controls["checkBoxCustomRecordValues"] as CheckBox;
            _checkBoxCustomRecordValues.Click += (sender, e) => ToggleRecording();

            _textBoxRecordValuesCount = splitContainerCustomControls.Panel1.Controls["textBoxRecordValuesCount"] as BetterTextbox;

            _buttonCustomShowValues = splitContainerCustomControls.Panel1.Controls["buttonCustomShowValues"] as Button;
            _buttonCustomShowValues.Click += (sender, e) => ShowRecordedValues();

            _buttonCustomClearValues = splitContainerCustomControls.Panel1.Controls["buttonCustomClearValues"] as Button;
            _buttonCustomClearValues.Click += (sender, e) => ClearRecordedValues();

            _checkBoxUseValueAtStartOfGlobalTimer = splitContainerCustomControls.Panel1.Controls["checkBoxUseValueAtStartOfGlobalTimer"] as CheckBox;

            _labelCustomRecordingFrequencyValue = splitContainerCustomControls.Panel1.Controls["labelCustomRecordingFrequencyValue"] as Label;

            _labelCustomRecordingGapsValue = splitContainerCustomControls.Panel1.Controls["labelCustomRecordingGapsValue"] as Label;

            _recordedValues = new Dictionary<int, List<string>>();
            _lastTimer = null;
            _numGaps = 0;
            _recordFreq = 1;

            // Panel 2 controls

            RadioButton radioButtonCustomTabFlushLeft = splitContainerCustomControls.Panel2.Controls["radioButtonCustomTabFlushLeft"] as RadioButton;
            radioButtonCustomTabFlushLeft.Click += (sender, e) => WatchVariableControl.LeftFlush = true;

            RadioButton radioButtonCustomTabFlushRight = splitContainerCustomControls.Panel2.Controls["radioButtonCustomTabFlushRight"] as RadioButton;
            radioButtonCustomTabFlushRight.Click += (sender, e) => WatchVariableControl.LeftFlush = false;

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

        private static void InitializeAddSubtractGetSetFuncionality(
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

        public override void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            base.AddVariables(watchVarControls);
            foreach (WatchVariableControl watchVarControl in watchVarControls)
            {
                watchVarControl.EnableCustomFunctionality();
            }
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
            _lastTimer = null;
            _numGaps = 0;
            _recordFreq = 1;
        }

        public override void Update(bool updateView)
        {
            if (_checkBoxCustomRecordValues.Checked)
            {
                int currentTimer = Config.Stream.GetInt32(MiscConfig.GlobalTimerAddress);

                bool alreadyContainsKey = _recordedValues.ContainsKey(currentTimer);
                bool recordEvenIfAlreadyHave = !_checkBoxUseValueAtStartOfGlobalTimer.Checked;

                if (alreadyContainsKey)
                {
                    _recordFreq++;
                }
                else
                {
                    _labelCustomRecordingFrequencyValue.Text = _recordFreq.ToString();
                    _recordFreq = 1;
                }

                if (_lastTimer.HasValue)
                {
                    int diff = currentTimer - _lastTimer.Value;
                    if (diff > 1) _numGaps += (diff - 1);
                }
                _lastTimer = currentTimer;

                if (!alreadyContainsKey || recordEvenIfAlreadyHave)
                {
                    List<string> currentValues = GetCurrentVariableValues();
                    _recordedValues[currentTimer] = currentValues;
                }
            }
            else
            {
                _labelCustomRecordingFrequencyValue.Text = "0";
            }
            _textBoxRecordValuesCount.Text = _recordedValues.Count.ToString();
            _labelCustomRecordingGapsValue.Text = _numGaps.ToString();

            if (!updateView) return;
            base.Update(updateView);
        }
    }
}
