using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic
{
    public partial class ModifyAddWatchVariableForm : Form
    {
        WatchVariable _value = new WatchVariable();

        public WatchVariable Value
        {
            get
            {
                return _value;
            }
        }

        public ModifyAddWatchVariableForm()
        {
            InitializeComponent();
        }

        public ModifyAddWatchVariableForm(WatchVariable watchVar)
        {
            InitializeComponent();
            textBoxName.Text = watchVar.Name;
            comboBoxType.SelectedItem = watchVar.Type;
            maskedTextBoxAddress.Text = watchVar.Address.ToString();
            checkBoxAbsolute.Checked = watchVar.AbsoluteAddressing;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            // Parse data and close
            if (ResultValid())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private bool ResultValid()
        {
            uint address;
            if (comboBoxType.SelectedIndex < 0)
            {
                MessageBox.Show("Select a value type!");
                return false;
            }
            else if (ParsingUtilities.TryParseHex(maskedTextBoxAddress.Text, out address))
            {
                MessageBox.Show("Enter a valid address!");
                return false;
            }

            WatchVariable watchVar = new WatchVariable();
            watchVar.Name = textBoxName.Text;
            watchVar.Type = WatchVariableParsingExtensions.GetStringType((string)comboBoxType.SelectedItem);
            watchVar.Address = address;
            watchVar.AbsoluteAddressing = checkBoxAbsolute.Checked;
            _value = watchVar;

            return true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
