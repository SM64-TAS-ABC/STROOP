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

namespace SM64_Diagnostic
{
    public partial class VariableViewerForm : Form
    {
        string _name, _type, _n64Address, _processAddress;

        public VariableViewerForm(string name, string type, string n64Address, string processAddress)
        {
            InitializeComponent();
            _name = name;
            _type = type;
            _n64Address = n64Address;
            _processAddress = processAddress;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VariableViewerForm_Load(object sender, EventArgs e)
        {
            labelVarName.Text = _name;
            textBoxVarTypeValue.Text = _type;
            textBoxN64AddValue.Text = _n64Address;
            textBoxEmuAddValue.Text = _processAddress;
        }
    }
}
