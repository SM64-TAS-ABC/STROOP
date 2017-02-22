using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using System.Drawing;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.Managers
{
    public class DisassemblyManager
    {
        const int NumberOfLinesAdd = 40;

        ProcessStream _stream;
        RichTextBox _output;
        MaskedTextBox _textBoxStartAdd;
        Button _goButton;
        Form _formContext;
        bool _addressChanged = false;
        uint _lastProcessAddress;

        public DisassemblyManager(Form formContext, RichTextBox disTextBox, MaskedTextBox textBoxStartAdd, ProcessStream stream, Button goButton)
        {
            _stream = stream;
            _output = disTextBox;
            _textBoxStartAdd = textBoxStartAdd;
            _goButton = goButton;
            _formContext = formContext;

            goButton.Click += GoButton_Pressed;
            textBoxStartAdd.TextChanged += (sender, e) =>
            {
                _addressChanged = true;
                _goButton.Text = "Go";
            };
            _stream.OnStatusChanged += Stream_StatusChanged;
        }

        private void GoButton_Pressed(object sender, EventArgs e)
        {
            if (!_addressChanged)
            {
                DisassemblyLines(NumberOfLinesAdd);
                return;
            }

            uint newAddress;
            if (!ParsingUtilities.TryParseHex(_textBoxStartAdd.Text, out newAddress))
            {
                MessageBox.Show(String.Format("Address {0} is not valid!", _textBoxStartAdd.Text),
                    "Address Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            StartShowDisassmbly(newAddress, NumberOfLinesAdd);
        }

        private void Stream_StatusChanged(object sender, EventArgs e)
        {
            // Yay... thread safety
            if (_formContext.Disposing || _formContext.IsDisposed || _formContext == null)
                return;

            _formContext.Invoke(new Action(() =>
            {
                if (_stream.IsRunning)
                {
                    _goButton.Enabled = true;
                    _textBoxStartAdd.Enabled = true;
                }
                else
                {
                    _goButton.Enabled = false;
                    _textBoxStartAdd.Enabled = false;
                }
            }));
        }

        private void StartShowDisassmbly(uint newAddress, int numberOfLines)
        {
            _goButton.Text = "More";
            _addressChanged = false;

            _output.Text = "";
            _lastProcessAddress = newAddress & 0x0FFFFFFF;
            DisassemblyLines(numberOfLines);
        }

        private void DisassemblyLines(int numberOfLines)
        {
            _output.Visible = false;
            var instructionBytes = _stream.ReadRamLittleEndian(_lastProcessAddress, 4 * numberOfLines);
            for (int i = 0; i < numberOfLines; i++, _lastProcessAddress += 4)
            {
                // Get next bytes
                var nextBytes = new byte[4];
                Array.Copy(instructionBytes, i * 4, nextBytes, 0, 4);

                // Write Address
                _output.AppendText(String.Format("0x{0:X8}: ", _lastProcessAddress | 0x80000000), Color.Blue);

                // Write byte-code
                _output.AppendText(BitConverter.ToString(nextBytes.Reverse().ToArray()).Replace('-', ' '), Color.DarkGray);

                // Write Disassembly
                uint instruction = BitConverter.ToUInt32(nextBytes, 0);
                uint address = (uint)(((uint)_lastProcessAddress) & 0x0FFFFFFF);
                string disassembly = "\t" + N64Disassembler.DisassembleInstruction(address, instruction);
                _output.AppendText(disassembly, Color.Red);

                // Replace "span's"
                string searchText = "<span class='dis-reg-";
                int findIndex = _output.Text.IndexOf(searchText); ;
                while (findIndex >= 0) {
                    _output.ReadOnly = false;
                    _output.Select(findIndex, _output.Text.IndexOf('>', findIndex) - findIndex + 1);
                    _output.SelectedText = "";
                    _output.Select(findIndex, _output.Text.IndexOf('<', findIndex) - findIndex);
                    _output.SelectionColor = Color.Green;
                    _output.Select(_output.Text.IndexOf('<', findIndex), "</span>".Length);
                    _output.SelectedText = "";
                    _output.ReadOnly = true;

                    findIndex = _output.Text.IndexOf(searchText);
                }

                searchText = "<span class='dis-address-jump'>";
                findIndex = _output.Text.IndexOf(searchText); ;
                while (findIndex >= 0) {
                    _output.ReadOnly = false;
                    _output.Select(findIndex, searchText.Length);
                    _output.SelectedText = "";
                    _output.Select(findIndex, _output.Text.IndexOf('<', findIndex) - findIndex);
                    _output.SelectionColor = Color.Blue;
                    _output.Select(_output.Text.IndexOf('<', findIndex), "</span>".Length);
                    _output.SelectedText = "";
                    _output.ReadOnly = true;

                    findIndex = _output.Text.IndexOf(searchText);
                }

                // Finish line (no pun intended)
                _output.AppendText(Environment.NewLine);
            }
            _output.Visible = true;
        }
    }
}
