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
    public class FileCoinScoreTextbox : FileTextbox
    {
        private byte _currentValue;

        public FileCoinScoreTextbox()
        {
        }

        public override void Initialize(ProcessStream stream, uint addressOffset)
        {
            base.Initialize(stream, addressOffset);

            _currentValue = GetCoinScoreFromMemory();
            this.Text = _currentValue.ToString();
        }

        private byte GetCoinScoreFromMemory()
        {
            return _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        protected override void SubmitValue()
        {
            byte value;
            if (!byte.TryParse(this.Text, out value))
            {
                this.Text = GetCoinScoreFromMemory().ToString();
                return;
            }

            _stream.SetValue(value, FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        public override void UpdateText()
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
