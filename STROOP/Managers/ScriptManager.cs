using STROOP.Controls;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class ScriptManager
    {
        RichTextBoxEx _output;
        TextBox _textBoxScriptAddress;

        static Regex functionRegex = new Regex("fn[0-9a-fA-F]{8}");

        public ScriptManager(Control tabControl)
        {
            _output = tabControl.Controls["richTextBoxExScript"] as RichTextBoxEx;
            _output.LinkClicked += _output_LinkClicked;
            _textBoxScriptAddress = tabControl.Controls["textBoxScriptAddress"] as TextBox;

            Button goButton = tabControl.Controls["buttonScriptGo"] as Button;
            goButton.Click += (sender, e) => Go();
        }

        private void _output_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Config.DisassemblyManager.Disassemble(e.LinkText);
            Config.StroopMainForm.SwitchTab("tabPageDisassembler");
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

            if (unprocessed == null)
            { 
                _output.Text = "Failed to decode";
                return;
            }

            _output.Text = unprocessed;

            foreach (Match function in functionRegex.Matches(unprocessed))
            {
                _output.Select(function.Index, function.Length);
                _output.SetSelectionLink(true);
            }
        }
    }
}
