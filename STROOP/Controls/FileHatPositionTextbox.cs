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
    public class FileHatPositionTextbox : FileTextbox
    {
        private short _currentValue;

        public FileHatPositionTextbox()
        {
        }

        public override void Initialize(uint addressOffset)
        {
            base.Initialize(addressOffset);
            this.Text = _currentValue.ToString();
        }

        private short GetHatLocationValueFromMemory()
        {
            return Config.Stream.GetInt16(Config.FileManager.CurrentFileAddress + _addressOffset);
        }

        protected override void SubmitValue()
        {
            short value;
            if (!short.TryParse(this.Text, out value))
            {
                this.Text = GetHatLocationValueFromMemory().ToString();
                return;
            }

            Config.Stream.SetValue(value, Config.FileManager.CurrentFileAddress + _addressOffset);
        }

        protected override void ResetValue()
        {
            short value = GetHatLocationValueFromMemory();
            this._currentValue = value;
            this.Text = value.ToString();
        }

        public override void UpdateText()
        {
            short value = GetHatLocationValueFromMemory();
            if (_currentValue != value)
            {
                this.Text = value.ToString();
                _currentValue = value;
            }
        }
    }
}
