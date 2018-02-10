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
                    _toolTip.IsBalloon = false;
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

        public void Initialize(FileImageGui gui, uint addressOffset, byte mask, Image onImage, Image offImage, string missionName)
        {
            base.Initialize(addressOffset, mask, onImage, offImage);
            AddressToolTip.SetToolTip(this, missionName);
        }
    }
}
