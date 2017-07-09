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
            _currentValue = GetCoinScoreFromMemory();
            this.Text = _currentValue.ToString();

            this.KeyDown += (sender, e) => { if (e.KeyData == Keys.Enter) SubmitCoinScore(); };
            this.LostFocus += (sender, e) => SubmitCoinScore();
        }

        private byte GetCoinScoreFromMemory()
        {
            return _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        private void SubmitCoinScore()
        {
            byte value;
            if (!byte.TryParse(this.Text, out value))
            {
                this.Text = GetCoinScoreFromMemory().ToString();
                return;
            }

            _stream.SetValue(value, FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        public void UpdateText()
        {
            byte value = GetCoinScoreFromMemory();
            if (_currentValue != value)
            {
                this.Text = value.ToString();
                _currentValue = value;
            }
        }
    }
}
