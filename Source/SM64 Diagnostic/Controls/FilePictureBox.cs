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
    public abstract class FilePictureBox : PictureBox
    {
        protected ProcessStream _stream;
        protected uint _addressOffset;
        protected byte _mask;
        protected byte _currentValue;

        public FilePictureBox()
        {
        }

        protected void Initialize(ProcessStream stream, uint addressOffset, byte mask)
        {
            _stream = stream;
            _addressOffset = addressOffset;
            _mask = mask;
            _currentValue = GetValue();

            this.Click += ClickAction;
            this.MouseEnter += (s, e) => this.Cursor = Cursors.Hand;
            this.MouseLeave += (s, e) => this.Cursor = Cursors.Arrow;
            UpdateImage(true);
        }

        private void SetValue(bool boolValue)
        {
            if (boolValue)
                SetValue((byte)0xFF);
            else
                SetValue((byte)0x00);
        }

        private void SetValue(byte value)
        {
            byte oldByte = _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
            byte newByte = MoreMath.ApplyValueToMaskedByte(oldByte, _mask, value);
            _stream.SetValue(newByte, FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        private byte GetValue()
        {
            byte currentByte = _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
            byte maskedCurrentByte = (byte)(currentByte & _mask);
            return maskedCurrentByte;
        }

        protected abstract Image GetImageForValue(byte value);

        protected virtual byte GetNewValueForValue(byte oldValue)
        {
            return oldValue == 0 ? _mask : (byte)0;
        }

        private void ClickAction(object sender, EventArgs e)
        {
            byte oldValue = GetValue();
            byte newValue = GetNewValueForValue(oldValue);
            SetValue(newValue);
        }

        public void UpdateImage(bool force = false)
        {
            if (_stream == null) return;

            byte value = GetValue();
            if (_currentValue != value || force)
            {
                this.Image = GetImageForValue(value);
                _currentValue = value;
                Invalidate();
            }
        }
    }
}
