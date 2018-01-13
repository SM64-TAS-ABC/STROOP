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
        private readonly VarX _varX;
        private readonly Timer _timer;
        private List<uint> _addresses;

        public VariableControllerForm(string varName, VarX varX)
        {
            _varName = varName;
            _varX = varX;
            _timer = new System.Windows.Forms.Timer { Interval = 30 };
            _addresses = null;

            InitializeComponent();

            _textBoxVarName.Text = _varName;
            _buttonAdd.Click += (s, e) => { _varX.AddValue(_textBoxAddSubtract.Text, true, _addresses); };
            _buttonSubtract.Click += (s, e) => { _varX.AddValue(_textBoxAddSubtract.Text, false, _addresses); };
            _buttonGet.Click += (s, e) => { _textBoxGetSet.Text = _varX.GetStringValue(true, true, _addresses); };
            _buttonSet.Click += (s, e) => { _varX.SetStringValue(_textBoxGetSet.Text, _addresses); };
            _checkBoxFixAddress.Click += (s, e) => { ToggleFixedAddress(); };

            _timer.Tick += (s, e) => { _textBoxCurrentValue.Text = _varX.GetStringValue(true, true, _addresses); };
            _timer.Start();
        }
        
        public void ToggleFixedAddress()
        {
            bool fixedAddress = _checkBoxFixAddress.Checked;
            if (fixedAddress)
            {
                _textBoxCurrentValue.BackColor = COLOR_RED;
                _addresses = _varX.GetCurrentAddresses();
            }
            else
            {
                _textBoxCurrentValue.BackColor = COLOR_BLUE;
                _addresses = null;
            }
        }
    }
}
