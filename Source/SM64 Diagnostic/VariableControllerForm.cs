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

        private readonly string _varName;
        private readonly VarX _varX;

        public VariableControllerForm(string varName, VarX varX)
        {
            InitializeComponent();
            _varName = varName;
            _varX = varX;
        }

        private void VariableViewerForm_Load(object sender, EventArgs eventArgs)
        {
            _labelVarName.Text = _varName;
            _buttonAdd.Click += (s, e) => { _varX.AddValue(_textBoxAddSubtract.Text, true); };
            _buttonSubtract.Click += (s, e) => { _varX.AddValue(_textBoxAddSubtract.Text, false); };
            _buttonGet.Click += (s, e) => { _textBoxGetSet.Text = _varX.GetStringValue(); };
            _buttonSet.Click += (s, e) => { _varX.SetStringValue(_textBoxGetSet.Text); };
        }

        public void UpdateForm()
        {
            _textBoxCurrentValue.Text = _varX.GetStringValue();
        }
    }
}
