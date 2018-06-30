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
            _buttonAdd.Click += (s, e) =>
            {
                List<string> values = ParsingUtilities.ParseStringList(_textBoxAddSubtract.Text);
                for (int i = 0; i < _watchVarWrappers.Count; i++)
                    _watchVarWrappers[i].AddValue(values[i % values.Count], true, _fixedAddressLists[i]);
            };

            _buttonSubtract.Click += (s, e) =>
            {
                List<string> values = ParsingUtilities.ParseStringList(_textBoxAddSubtract.Text);
                for (int i = 0; i < _watchVarWrappers.Count; i++)
                    _watchVarWrappers[i].AddValue(values[i % values.Count], false, _fixedAddressLists[i]);
            };

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
