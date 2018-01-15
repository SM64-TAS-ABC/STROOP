using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;

namespace SM64_Diagnostic
{
    public partial class VariableControllerForm : Form
    {
        private static readonly Color COLOR_BLUE = Color.FromArgb(220, 255, 255);
        private static readonly Color COLOR_RED = Color.FromArgb(255, 220, 220);

        private readonly string _varName;
        private readonly WatchVariableWrapper _watchVarWrapper;
        private readonly Timer _timer;
        private List<uint> _addresses;

        public VariableControllerForm(string varName, WatchVariableWrapper watchVarWrapper, List<uint> fixedAddressList)
        {
            _varName = varName;
            _watchVarWrapper = watchVarWrapper;
            _timer = new System.Windows.Forms.Timer { Interval = 30 };
            _addresses = fixedAddressList;

            InitializeComponent();

            _textBoxVarName.Text = _varName;
            _buttonAdd.Click += (s, e) => { _watchVarWrapper.AddValue(_textBoxAddSubtract.Text, true, _addresses); };
            _buttonSubtract.Click += (s, e) => { _watchVarWrapper.AddValue(_textBoxAddSubtract.Text, false, _addresses); };
            _buttonGet.Click += (s, e) => { _textBoxGetSet.Text = _watchVarWrapper.GetStringValue(true, true, _addresses); };
            _buttonSet.Click += (s, e) => { _watchVarWrapper.SetStringValue(_textBoxGetSet.Text, _addresses); };
            _checkBoxLock.Click += (s, e) => { ToggleFixedAddress(); };

            _checkBoxLock.Checked = fixedAddressList != null;
            _textBoxCurrentValue.BackColor = fixedAddressList == null ? COLOR_BLUE : COLOR_RED;

            _timer.Tick += (s, e) => { _textBoxCurrentValue.Text = _watchVarWrapper.GetStringValue(true, true, _addresses); };
            _timer.Start();
        }
        
        public void ToggleFixedAddress()
        {
            bool fixedAddress = _checkBoxLock.Checked;
            if (fixedAddress)
            {
                _textBoxCurrentValue.BackColor = COLOR_RED;
                _addresses = _watchVarWrapper.GetCurrentAddresses();
            }
            else
            {
                _textBoxCurrentValue.BackColor = COLOR_BLUE;
                _addresses = null;
            }
        }
    }
}
