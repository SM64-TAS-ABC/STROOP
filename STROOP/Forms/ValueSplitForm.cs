using STROOP.Models;
using STROOP.Structs;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Forms
{
    public partial class ValueSplitForm : Form
    {
        public string StringValue;
        public bool RightButtonClicked;

        public ValueSplitForm(
            string textBoxText = "",
            string labelText = "Enter Value:",
            string button1Text = "OK",
            string button2Text = "OK")
        {
            InitializeComponent();
            textBox1.Text = textBoxText;
            label1.Text = labelText;
            button1.Text = button1Text;
            button2.Text = button2Text;

            Action<string, bool> okAction = (string stringValue, bool rightButtonClicked) =>
            {
                StringValue = stringValue;
                RightButtonClicked = rightButtonClicked;
                DialogResult = DialogResult.OK;
                Close();
            };

            button1.Click += (sender, e) => okAction(textBox1.Text, false);
            button2.Click += (sender, e) => okAction(textBox1.Text, true);

            ControlUtilities.AddContextMenuStripFunctions(
                button1,
                new List<string>()
                {
                    "Use Clipboard",
                },
                new List<Action>()
                {
                    () => okAction(Clipboard.GetText(), false),
                });
            ControlUtilities.AddContextMenuStripFunctions(
                button2,
                new List<string>()
                {
                    "Use Clipboard",
                },
                new List<Action>()
                {
                    () => okAction(Clipboard.GetText(), true),
                });
        }
    }
}
