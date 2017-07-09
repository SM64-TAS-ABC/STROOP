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
    public class FileHatPositionTextbox : FileTextbox
    {
        private short _currentValue;

        public FileHatPositionTextbox()
        {
        }

        public override void Initialize(ProcessStream stream, uint addressOffset)
        {
            base.Initialize(stream, addressOffset);

            _currentValue = GetHatLocationValueFromMemory();
            this.Text = _currentValue.ToString();
        }

        private short GetHatLocationValueFromMemory()
        {
            return _stream.GetInt16(FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        protected override void SubmitValue()
        {
            short value;
            if (!short.TryParse(this.Text, out value))
            {
                this.Text = GetHatLocationValueFromMemory().ToString();
                return;
            }

            _stream.SetValue(value, FileManager.Instance.CurrentFileAddress + _addressOffset);
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
