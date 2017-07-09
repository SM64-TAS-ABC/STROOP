using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using System.Drawing.Drawing2D;

namespace SM64_Diagnostic
{
    public class FileCoinScoreTextbox : TextBox
    {
        protected ProcessStream _stream;
        protected uint _addressOffset;
        protected byte _currentValue;

        public FileCoinScoreTextbox()
        {
        }

        public void Initialize(ProcessStream stream, uint addressOffset)
        {
            _stream = stream;
            _addressOffset = addressOffset;
            _currentValue = _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
            this.Text = _currentValue.ToString();

            this.LostFocus += (sender, e) => SubmitCoinScore();
        }

        private void KeyDownAction(object sender, KeyEventArgs e)
        {
            // On "Enter" key press
            if (e.KeyData != Keys.Enter)
                return;

            SubmitCoinScore();
        }

        private void SubmitCoinScore()
        {
            byte value;
            if (!byte.TryParse(this.Text, out value)) return;

            _stream.SetValue(value, FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        public void UpdateText()
        {
            byte value = _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
            if (_currentValue != value)
            {
                this.Text = value.ToString();
                _currentValue = value;
            }
        }
    }
}
