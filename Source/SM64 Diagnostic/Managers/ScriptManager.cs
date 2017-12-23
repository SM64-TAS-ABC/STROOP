using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Managers
{
    public class ScriptManager
    {
        RichTextBoxEx _output;
        TextBox _textBoxScriptAddress;

        public ScriptManager(Control tabControl)
        {
            _output = tabControl.Controls["richTextBoxExScript"] as RichTextBoxEx;
            _textBoxScriptAddress = tabControl.Controls["textBoxScriptAddress"] as TextBox;

            Button goButton = tabControl.Controls["buttonScriptGo"] as Button;
            goButton.Click += (sender, e) => Go();
        }

        public void Go(uint? scriptAddress = null)
        {            
            uint address;
            
            // Change textbox if provided address
            if (scriptAddress != null)
            {
                address = scriptAddress.Value;
                _textBoxScriptAddress.Text = $"0x{scriptAddress:X8}";
            }
            // Otherwise use textbox address
            else
            {
                if (!ParsingUtilities.TryParseHex(_textBoxScriptAddress.Text, out address))
                {
                    MessageBox.Show($"Address {_textBoxScriptAddress.Text} is not valid!",
                        "Address Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            ShowDecoding(address);
        }

        private void ShowDecoding(uint address)
        {
            string unprocessed = BehaviorDecoder.Decode(address);

            _output.Text = unprocessed == null ? "Failed to decode" : unprocessed;
        }
    }
}
