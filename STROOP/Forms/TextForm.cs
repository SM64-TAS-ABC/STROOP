using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class TextForm : Form
    {
        public string StringValue;

        public TextForm(string initialText = "")
        {
            InitializeComponent();
            betterTextbox1.Text = initialText;
            button1.Click += (sender, e) =>
            {
                StringValue = betterTextbox1.Text;
                DialogResult = DialogResult.OK;
                Close();
            };
        }
    }
}
