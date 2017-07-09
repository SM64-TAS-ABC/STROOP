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
    public class FileStarPictureBox : FileBinaryPictureBox
    {
        static ToolTip _toolTip;
        public static ToolTip AddressToolTip
        {
            get
            {
                if (_toolTip == null)
                {
                    _toolTip = new ToolTip();
                    _toolTip.IsBalloon = true;
                    _toolTip.ShowAlways = true;
                }
                return _toolTip;
            }
            set
            {
                _toolTip = value;
            }
        }

        public FileStarPictureBox()
        {
        }

        public void Initialize(ProcessStream stream, FileImageGui gui, uint addressOffset, byte mask, Image onImage, Image offImage, string missionName)
        {
            base.Initialize(stream, addressOffset, mask, onImage, offImage);
            AddressToolTip.SetToolTip(this, missionName);
        }
    }
}
