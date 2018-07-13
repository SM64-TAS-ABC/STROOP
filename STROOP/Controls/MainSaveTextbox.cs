using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Managers;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Controls;
using STROOP.Extensions;
using System.Drawing.Drawing2D;
using STROOP.Structs.Configurations;

namespace STROOP
{
    public class MainSaveTextbox : FileTextbox
    {
        private byte _currentValue;

        public MainSaveTextbox()
        {
        }

        public override void Initialize(uint addressOffset)
        {
            base.Initialize(addressOffset);
            this.Text = _currentValue.ToString();
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
