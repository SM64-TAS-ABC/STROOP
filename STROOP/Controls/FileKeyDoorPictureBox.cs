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

namespace STROOP
{
    public class FileKeyDoorPictureBox : FilePictureBox
    {
        private byte _mask1;
        private byte _mask2;

        private Image _onOnImage;
        private Image _onOffImage;
        private Image _offOnImage;
        private Image _offOffImage;

        public FileKeyDoorPictureBox()
        {
        }

        public void Initialize(uint addressOffset, byte mask1, byte mask2,
            Image onOnImage, Image onOffImage, Image offOnImage, Image offOffImage)
        {
            _mask1 = mask1;
            _mask2 = mask2;

            _onOnImage = onOnImage;
            _onOffImage = onOffImage;
            _offOnImage = offOnImage;
            _offOffImage = offOffImage;

            base.Initialize(addressOffset, (byte)(mask1 | mask2));
        }

        protected override Image GetImageForValue(byte value)
        {
            if (value == 0)
                return _offOffImage;
            else if (value == _mask1)
                return _onOffImage;
            else if (value == _mask2)
                return _offOnImage;
            else
                return _onOnImage;
        }

        protected override byte GetNewValueForValue(byte oldValue)
        {
            if (oldValue == 0)
                return _mask1;
            else if (oldValue == _mask1)
                return _mask2;
            else if (oldValue == _mask2)
                return 0;
            else
                return (byte)(_mask1 | _mask2);
        }
    }
}
