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
    public class File1StarDoorPictureBox : FilePictureBox
    {
        public File1StarDoorPictureBox()
        {
        }

        protected override Image GetImageForValue(byte value)
        {
            if (value == 0)
                return _gui.DoorBlackImage;
            else
                return _gui.Door1StarImage;
        }
    }
}
