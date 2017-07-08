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
    public class FilePictureBox : PictureBox
    {
        ProcessStream _stream;
        FileImageGui _gui;
        uint _addressOffset;
        byte _mask;
        byte _currentValue;

        public FilePictureBox()
        {
        }

        public void Initialize(ProcessStream stream, FileImageGui gui, uint addressOffset, byte mask)
        {
            _stream = stream;
            _gui = gui;
            _addressOffset = addressOffset;
            _mask = mask;
            _currentValue = GetValue();
            this.Click += ClickAction;
            this.MouseEnter += (s, e) => this.Cursor = Cursors.Hand;
            this.MouseLeave += (s, e) => this.Cursor = Cursors.Arrow;
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
            byte maskedValue = (byte)(value & _mask);
            byte oldByte = _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
            byte unmaskedOldByte = (byte)(oldByte & ~_mask);
            byte newByte = (byte)(unmaskedOldByte | maskedValue);
            _stream.SetValue(newByte, FileManager.Instance.CurrentFileAddress + _addressOffset);
        }

        private byte GetValue()
        {
            byte currentByte = _stream.GetByte(FileManager.Instance.CurrentFileAddress + _addressOffset);
            byte maskedCurrentByte = (byte)(currentByte & _mask);
            return maskedCurrentByte;
        }

        protected Image GetImageForValue(byte value)
        {
            if (value == 0)
                return _gui.PowerStarBlackImage;
            else
                return _gui.PowerStarImage;
        }

        protected byte GetNewValueForValue(byte oldValue)
        {
            return oldValue == 0 ? _mask : (byte)0;
        }

        private void ClickAction(object sender, EventArgs e)
        {
            byte oldValue = GetValue();
            byte newValue = GetNewValueForValue(oldValue);
            SetValue(newValue);
        }

        public void UpdateImage()
        {
            if (_gui == null) return;

            byte value = GetValue();
            if (_currentValue != value)
            {
                this.Image = GetImageForValue(value);
                _currentValue = value;
                Invalidate();
            }
        }
    }
}
