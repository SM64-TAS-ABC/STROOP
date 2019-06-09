using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class ValueForm : Form
    {
        public string StringValue;

        public ValueForm(string initialText = "")
        {
            InitializeComponent();
            betterTextbox1.Text = initialText;

            Action okayAction = () =>
            {
                StringValue = betterTextbox1.Text;
                DialogResult = DialogResult.OK;
                Close();
            };

            button1.Click += (sender, e) => okayAction();
            betterTextbox1.AddEnterAction(okayAction);
        }
    }
}
