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
    public class FileBinaryPictureBox : FilePictureBox
    {
        private Image _onImage;
        private Image _offImage;

        public FileBinaryPictureBox()
        {
        }

        public void Initialize(ProcessStream stream, FileImageGui gui, uint addressOffset, byte mask, Image onImage, Image offImage)
        {
            _onImage = onImage;
            _offImage = offImage;
            base.Initialize(stream, gui, addressOffset, mask);
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
