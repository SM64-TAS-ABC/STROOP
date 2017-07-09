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
using static SM64_Diagnostic.Managers.FileManager;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic
{
    public class FileHatLocationPictureBox : FilePictureBox
    {
        private HatLocation _definingHatLocation;
        private HatLocation? _currentHatLocation;
        private Image _onImage;
        private Image _offImage;

        public FileHatLocationPictureBox()
        {
        }

        public void Initialize(ProcessStream stream, HatLocation definingHatLocation, Image onImage, Image offImage)
        {
            _stream = stream;
            _definingHatLocation = definingHatLocation;
            _onImage = onImage;
            _offImage = offImage;
            base.Initialize(stream, 0, 0);
        }

        private HatLocation? GetCurrentHatLocation()
        {
            ushort hatLocationCourse = _stream.GetUInt16(FileManager.Instance.CurrentFileAddress + Config.File.HatLocationCourseOffset);
            byte hatLocationMode = (byte)(_stream.GetByte(FileManager.Instance.CurrentFileAddress + Config.File.HatLocationModeOffset) & Config.File.HatLocationModeMask);

            return hatLocationMode == Config.File.HatLocationMarioMask ? HatLocation.Mario :
                   hatLocationMode == Config.File.HatLocationKleptoMask ? HatLocation.SSLKlepto :
                   hatLocationMode == Config.File.HatLocationSnowmanMask ? HatLocation.SLSnowman :
                   hatLocationMode == Config.File.HatLocationUkikiMask ? HatLocation.TTMUkiki :
                   hatLocationMode == Config.File.HatLocationGroundMask ?
                       (hatLocationCourse == Config.File.HatLocationCourseSSLValue ? HatLocation.SSLGround :
                        hatLocationCourse == Config.File.HatLocationCourseSLValue ? HatLocation.SLGround :
                        hatLocationCourse == Config.File.HatLocationCourseTTMValue ? HatLocation.TTMGround :
                        (HatLocation?)null) :
                   null;
        }

        private Image GetImageForValue(HatLocation? hatLocation)
        {
            if (_definingHatLocation == hatLocation)
                return _onImage;
            else
                return _offImage;
        }


        protected override void ClickAction(object sender, EventArgs e)
        {
            switch (_definingHatLocation)
            {
                case HatLocation.Mario:
                    SetHatMode(Config.File.HatLocationMarioMask);
                    break;

                case HatLocation.SSLKlepto:
                    SetHatMode(Config.File.HatLocationKleptoMask);
                    break;

                case HatLocation.SSLGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseSSLValue, FileManager.Instance.CurrentFileAddress + Config.File.HatLocationCourseOffset);
                    break;

                case HatLocation.SLSnowman:
                    SetHatMode(Config.File.HatLocationSnowmanMask);
                    break;

                case HatLocation.SLGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseSLValue, FileManager.Instance.CurrentFileAddress + Config.File.HatLocationCourseOffset);
                    break;

                case HatLocation.TTMUkiki:
                    SetHatMode(Config.File.HatLocationUkikiMask);
                    break;

                case HatLocation.TTMGround:
                    SetHatMode(Config.File.HatLocationGroundMask);
                    _stream.SetValue(Config.File.HatLocationCourseTTMValue, FileManager.Instance.CurrentFileAddress + Config.File.HatLocationCourseOffset);
                    break;
            }
        }

        private void SetHatMode(byte hatModeByte)
        {
            byte oldByte = _stream.GetByte(FileManager.Instance.CurrentFileAddress + Config.File.HatLocationModeOffset);
            byte newByte = MoreMath.ApplyValueToMaskedByte(oldByte, Config.File.HatLocationModeMask, hatModeByte);
            _stream.SetValue(newByte, FileManager.Instance.CurrentFileAddress + Config.File.HatLocationModeOffset);
        }

        public override void UpdateImage(bool force = false)
        {
            if (_stream == null) return;

            HatLocation? currentHatLocation = GetCurrentHatLocation();
            if (_currentHatLocation != currentHatLocation || force)
            {
                this.Image = GetImageForValue(currentHatLocation);
                _currentHatLocation = currentHatLocation;
                Invalidate();
            }
        }
    }
}
