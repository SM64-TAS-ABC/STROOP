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

        public ValueForm(
            string textBoxText = "",
            string labelText = "Enter Value:",
            string buttonText = "OK")
        {
            InitializeComponent();
            textBox1.Text = textBoxText;
            label1.Text = labelText;
            button1.Text = buttonText;

            Action<string> okAction = (string stringValue) =>
            {
                StringValue = stringValue;
                DialogResult = DialogResult.OK;
                Close();
            };

            button1.Click += (sender, e) => okAction(textBox1.Text);
            textBox1.AddEnterAction(() => okAction(textBox1.Text));

            ControlUtilities.AddContextMenuStripFunctions(
                button1,
                new List<string>()
                {
                    "Use Clipboard",
                },
                new List<Action>()
                {
                    () => okAction(Clipboard.GetText()),
                });
        }
    }
}
