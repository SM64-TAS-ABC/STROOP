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
        uint _address;
        byte _mask;
        byte _currentValue;

        public FilePictureBox()
        {
        }
                    
        public void Initialize(ProcessStream stream, FileImageGui gui, uint address, byte mask)
        {
            _stream = stream;
            _gui = gui;
            _address = address;
            _mask = mask;
            _currentValue = GetValue();
        }

        private void SetValue(byte value)
        {
            byte maskedValue = (byte)(value & _mask);
            byte oldByte = _stream.GetByte(_address);
            byte unmaskedOldByte = (byte)(oldByte & ~_mask);
            byte newByte = (byte)(unmaskedOldByte | maskedValue);
            _stream.SetValue(newByte, _address);
        }

        private byte GetValue()
        {
            byte currentByte = _stream.GetByte(_address);
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

        public void UpdateImage()
        {
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
