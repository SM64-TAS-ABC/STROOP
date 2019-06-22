using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Utilities;
using System.Drawing;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Controls;

namespace STROOP.Managers
{
    public class DisassemblyManager
    {
        const int NumberOfLinesAdd = 40;

        RichTextBoxEx _output;
        TextBox _textBoxStartAdd;
        uint _lastAddress;
        Button _goButton, _moreButton;
        int _currentLines = NumberOfLinesAdd;

        public DisassemblyManager(Control tabControl)
        {
            _output = tabControl.Controls["richTextBoxDissasembly"] as RichTextBoxEx;
            _textBoxStartAdd = tabControl.Controls["textBoxDisAddress"] as TextBox;
            _goButton = tabControl.Controls["buttonDisGo"] as Button;
            _moreButton = tabControl.Controls["buttonDisMore"] as Button;

            _output.LinkClicked += _output_LinkClicked;
            _goButton.Click += (sender, e) => Disassemble(_textBoxStartAdd.Text, _currentLines);
            _moreButton.Click += MoreButton_Click;
            _textBoxStartAdd.TextChanged += (sender, e) =>
            {
                _currentLines = NumberOfLinesAdd;
                _goButton.Text = "Go";
            };
        }

        private void _output_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            uint address;
            if (!ParsingUtilities.TryParseHex(e.LinkText, out address))
                return;

            _textBoxStartAdd.Text = e.LinkText;
            StartShowDisassmbly(address, NumberOfLinesAdd);
        }

        private void MoreButton_Click(object sender, EventArgs e)
        {
            DisassemblyLines(NumberOfLinesAdd);
            _currentLines += NumberOfLinesAdd;
        }

        public void Disassemble(string strAddress, int numberOfLines = NumberOfLinesAdd)
        {
            uint newAddress;
            if (!ParsingUtilities.TryParseHex(strAddress, out newAddress))
            {
                MessageBox.Show(String.Format("Address {0} is not valid!", strAddress),
                    "Address Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _currentLines = NumberOfLinesAdd;
            _textBoxStartAdd.Text = strAddress;
            StartShowDisassmbly(newAddress, _currentLines);
        }

        private void StartShowDisassmbly(uint newAddress, int numberOfLines)
        {
            newAddress &= ~0x03U;

            _goButton.Text = "Refresh";
            _moreButton.Visible = true;

            _output.Text = "";
            _lastAddress = newAddress & 0x0FFFFFFF;
            DisassemblyLines(numberOfLines);
        }

        private void DisassemblyLines(int numberOfLines)
        {
            _output.Visible = false;
            var instructionBytes = Config.Stream.ReadRam(_lastAddress, 4 * numberOfLines, EndiannessType.Little);
            for (int i = 0; i < numberOfLines; i++, _lastAddress += 4)
            {
                // Get next bytes
                var nextBytes = new byte[4];
                Array.Copy(instructionBytes, i * 4, nextBytes, 0, 4);

                // Write Address
                _output.AppendText(HexUtilities.FormatValue(_lastAddress | 0x80000000, 8) + ": ", Color.Blue);

                // Write byte-code
                _output.AppendText(BitConverter.ToString(nextBytes.Reverse().ToArray()).Replace('-', ' '), Color.DarkGray);

                // Write Disassembly
                uint instruction = BitConverter.ToUInt32(nextBytes, 0);
                uint address = (uint)(((uint)_lastAddress) & 0x0FFFFFFF);
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
                    _output.SetSelectionLink(true);
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
