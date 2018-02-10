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
    public class FileBinaryPictureBox : FilePictureBox
    {
        private Image _onImage;
        private Image _offImage;

        public FileBinaryPictureBox()
        {
        }

        public void Initialize(uint addressOffset, byte mask, Image onImage, Image offImage)
        {
            _onImage = onImage;
            _offImage = offImage;
            base.Initialize(addressOffset, mask);
        }

        protected override Image GetImageForValue(byte value)
        {
            if (value == 0)
                return _offImage;
            else
                return _onImage;
        }
    }
}
