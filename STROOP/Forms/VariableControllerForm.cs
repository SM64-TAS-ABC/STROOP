using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Extensions;
using STROOP.Utilities;
using STROOP.Controls;

namespace STROOP.Forms
{
    public partial class VariableControllerForm : Form
    {
        private static readonly Color COLOR_BLUE = Color.FromArgb(220, 255, 255);
        private static readonly Color COLOR_RED = Color.FromArgb(255, 220, 220);
        private static readonly Color COLOR_PURPLE = Color.FromArgb(200, 190, 230);

        private readonly List<string> _varNames;
        private readonly List<WatchVariableWrapper> _watchVarWrappers;
        private List<List<uint>> _fixedAddressLists;

        public VariableControllerForm(
            string varName, WatchVariableWrapper watchVarWrapper, List<uint> fixedAddressList) :
                this (new List<string>() { varName },
                      new List<WatchVariableWrapper>() { watchVarWrapper },
                      new List<List<uint>>() { fixedAddressList })
        {

        }

        public VariableControllerForm(
            List<string> varNames, List<WatchVariableWrapper> watchVarWrappers, List<List<uint>> fixedAddressLists)
        {
            _varNames = varNames;
            _watchVarWrappers = watchVarWrappers;
            _fixedAddressLists = fixedAddressLists;

            InitializeComponent();

            _textBoxVarName.Text = String.Join(",", _varNames);

            Action<bool> addAction = (bool add) =>
            {
                List<string> values = ParsingUtilities.ParseStringList(_textBoxAddSubtract.Text);
                if (values.Count == 0) return;
                for (int i = 0; i < _watchVarWrappers.Count; i++)
                    _watchVarWrappers[i].AddValue(values[i % values.Count], add, _fixedAddressLists[i]);
            };
            _buttonAdd.Click += (s, e) => addAction(true);
            _buttonSubtract.Click += (s, e) => addAction(false);

            Timer addTimer = new Timer { Interval = 30 };
            addTimer.Tick += (s, e) => { if (KeyboardUtilities.IsCtrlHeld()) addAction(true); };
            _buttonAdd.MouseDown += (sender, e) => addTimer.Start();
            _buttonAdd.MouseUp += (sender, e) => addTimer.Stop();

            Timer addTimer2 = new Timer { Interval = 30 };
            addTimer2.Tick += (s, e) => { addAction(true); };
            ControlUtilities.AddContextMenuStripFunctions(
                _buttonAdd,
                new List<string>() { "Start Continuous Add", "Stop Continuous Add" },
                new List<Action>() { () => addTimer2.Start(), () => addTimer2.Stop() });

            Timer subtractTimer = new Timer { Interval = 30 };
            subtractTimer.Tick += (s, e) => { if (KeyboardUtilities.IsCtrlHeld()) addAction(false); };
            _buttonSubtract.MouseDown += (sender, e) => subtractTimer.Start();
            _buttonSubtract.MouseUp += (sender, e) => subtractTimer.Stop();

            Timer subtractTimer2 = new Timer { Interval = 30 };
            subtractTimer2.Tick += (s, e) => { addAction(false); };
            ControlUtilities.AddContextMenuStripFunctions(
                _buttonSubtract,
                new List<string>() { "Start Continuous Subtract", "Stop Continuous Subtract" },
                new List<Action>() { () => subtractTimer2.Start(), () => subtractTimer2.Stop() });

            _buttonGet.Click += (s, e) => _textBoxGetSet.Text = GetValues();

            _buttonSet.Click += (s, e) => SetValues();
            _textBoxGetSet.AddEnterAction(() => SetValues());

            _checkBoxFixAddress.Click += (s, e) => ToggleFixedAddress();

            _checkBoxLock.Click += (s, e) =>
            {
                List<bool> lockedBools = new List<bool>();
                for (int i = 0; i < _watchVarWrappers.Count; i++)
                    lockedBools.Add(_watchVarWrappers[i].GetLockedBool(_fixedAddressLists[i]));
                bool anyLocked = lockedBools.Any(b => b);
                for (int i = 0; i < _watchVarWrappers.Count; i++)
                    _watchVarWrappers[i].ToggleLocked(!anyLocked, _fixedAddressLists[i]);
            };

            _checkBoxFixAddress.CheckState = BoolUtilities.GetCheckState(
                fixedAddressLists.ConvertAll(fixedAddressList => fixedAddressList != null));

            _textBoxCurrentValue.BackColor = GetColorForCheckState(BoolUtilities.GetCheckState(
                fixedAddressLists.ConvertAll(fixedAddressList => fixedAddressList != null)));

            Timer timer = new Timer { Interval = 30 };
            timer.Tick += (s, e) => UpdateForm();
            timer.Start();
        }

        private string GetValues()
        {
            List<object> values = new List<object>();
            for (int i = 0; i < _watchVarWrappers.Count; i++)
                values.Add(_watchVarWrappers[i].GetValue(true, true, _fixedAddressLists[i]));
            return String.Join(",", values);
        }

        private void SetValues()
        {
            List<string> values = ParsingUtilities.ParseStringList(_textBoxGetSet.Text);
            if (values.Count == 0) return;
            for (int i = 0; i < _watchVarWrappers.Count; i++)
                _watchVarWrappers[i].SetValue(values[i % values.Count], _fixedAddressLists[i]);
        }

        private Color GetColorForCheckState(CheckState checkState)
        {
            switch (checkState)
            {
                case CheckState.Unchecked:
                    return COLOR_BLUE;
                case CheckState.Checked:
                    return COLOR_RED;
                case CheckState.Indeterminate:
                    return COLOR_PURPLE;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateForm()
        {
            _textBoxCurrentValue.Text = GetValues();
            List<bool> lockedBools = new List<bool>();
            for (int i = 0; i < _watchVarWrappers.Count; i++)
                lockedBools.Add(_watchVarWrappers[i].GetLockedBool(_fixedAddressLists[i]));
            _checkBoxLock.CheckState = BoolUtilities.GetCheckState(lockedBools);
        }
        
        public void ToggleFixedAddress()
        {
            bool fixedAddress = _checkBoxFixAddress.Checked;
            if (fixedAddress)
            {
                _textBoxCurrentValue.BackColor = COLOR_RED;
                _fixedAddressLists = _watchVarWrappers.ConvertAll(
                    watchVarWrapper => watchVarWrapper.GetCurrentAddresses());
            }
            else
            {
                _textBoxCurrentValue.BackColor = COLOR_BLUE;
                _fixedAddressLists = _watchVarWrappers.ConvertAll(
                    watchVarWrapper => (List<uint>)null);
            }
        }
    }
}
