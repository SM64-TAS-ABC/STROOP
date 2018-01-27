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
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic
{
    public class FileCoinScoreTextbox : FileTextbox
    {
        private byte _currentValue;

        public FileCoinScoreTextbox()
        {
        }

        public override void Initialize(uint addressOffset)
        {
            base.Initialize(addressOffset);
        }

        private byte GetCoinScoreFromMemory()
        {
            return Config.Stream.GetByte(Config.FileManager.CurrentFileAddress + _addressOffset);
        }

        protected override void SubmitValue()
        {
            byte value;
            if (!byte.TryParse(this.Text, out value))
            {
                this.Text = GetCoinScoreFromMemory().ToString();
                return;
            }

            Config.Stream.SetValue(value, Config.FileManager.CurrentFileAddress + _addressOffset);
        }

        protected override void ResetValue()
        {
            byte value = GetCoinScoreFromMemory();
            this._currentValue = value;
            this.Text = value.ToString();
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
