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
    public partial class VariableControllerForm : Form
    {

        public VariableControllerForm(string name, string type, string n64Address, string processAddress)
        {
            InitializeComponent();

            /*
            _buttonAdd.Click += (sender, e) => { };
            _buttonSubtract.Click += (sender, e) => { };
            _buttonGet.Click += (sender, e) => { };
            _buttonSet.Click += (sender, e) => { };
            */

        }
        /*
        private void buttonOk_Click(object sender, EventArgs e)
        {
            Close();
        }
                */

        private void VariableViewerForm_Load(object sender, EventArgs e)
        {
            
        }
    }
}
