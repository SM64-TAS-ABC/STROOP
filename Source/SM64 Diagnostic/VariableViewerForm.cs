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
        public VariableViewerForm(string name, string type, string n64Address, string processAddress)
        {
            InitializeComponent();

            labelVariableName.Text = name;
            textBoxTypeValue.Text = type;
            textBoxN64AddressValue.Text = n64Address;
            textBoxEmulatorAddressValue.Text = processAddress;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
