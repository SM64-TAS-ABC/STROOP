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

namespace STROOP.Forms
{
    public partial class VariableCreationForm : Form
    {
        public VariableCreationForm()
        {
            InitializeComponent();

            buttonOk.Click += (sender, e) => Close();
        }
    }
}
