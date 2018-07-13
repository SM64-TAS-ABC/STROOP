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
using STROOP.Structs.Configurations;

namespace STROOP
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

        public void Initialize(HatLocation definingHatLocation, Image onImage, Image offImage)
        {
            _definingHatLocation = definingHatLocation;
            _onImage = onImage;
            _offImage = offImage;
            base.Initialize(0, 0);
        }

        private HatLocation? GetCurrentHatLocation()
        {
            byte hatLocationLevel = Config.Stream.GetByte(Config.FileManager.CurrentFileAddress + FileConfig.HatLocationLevelOffset);
            byte hatLocationMode = (byte)(Config.Stream.GetByte(Config.FileManager.CurrentFileAddress + FileConfig.HatLocationModeOffset) & FileConfig.HatLocationModeMask);

            return hatLocationMode == FileConfig.HatLocationMarioMask ? HatLocation.Mario :
                   hatLocationMode == FileConfig.HatLocationKleptoMask ? HatLocation.SSLKlepto :
                   hatLocationMode == FileConfig.HatLocationSnowmanMask ? HatLocation.SLSnowman :
                   hatLocationMode == FileConfig.HatLocationUkikiMask ? HatLocation.TTMUkiki :
                   hatLocationMode == FileConfig.HatLocationGroundMask ?
                       (hatLocationLevel == FileConfig.HatLocationLevelSSLValue ? HatLocation.SSLGround :
                        hatLocationLevel == FileConfig.HatLocationLevelSLValue ? HatLocation.SLGround :
                        hatLocationLevel == FileConfig.HatLocationLevelTTMValue ? HatLocation.TTMGround :
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
                    SetHatMode(FileConfig.HatLocationMarioMask);
                    break;

                case HatLocation.SSLKlepto:
                    SetHatMode(FileConfig.HatLocationKleptoMask);
                    break;

                case HatLocation.SSLGround:
                    SetHatMode(FileConfig.HatLocationGroundMask);
                    Config.Stream.SetValue(FileConfig.HatLocationLevelSSLValue, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationLevelOffset);
                    Config.Stream.SetValue(FileConfig.HatLocationAreaSSLValue, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationAreaOffset);
                    break;

                case HatLocation.SLSnowman:
                    SetHatMode(FileConfig.HatLocationSnowmanMask);
                    break;

                case HatLocation.SLGround:
                    SetHatMode(FileConfig.HatLocationGroundMask);
                    Config.Stream.SetValue(FileConfig.HatLocationLevelSLValue, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationLevelOffset);
                    Config.Stream.SetValue(FileConfig.HatLocationAreaSLValue, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationAreaOffset);
                    break;

                case HatLocation.TTMUkiki:
                    SetHatMode(FileConfig.HatLocationUkikiMask);
                    break;

                case HatLocation.TTMGround:
                    SetHatMode(FileConfig.HatLocationGroundMask);
                    Config.Stream.SetValue(FileConfig.HatLocationLevelTTMValue, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationLevelOffset);
                    Config.Stream.SetValue(FileConfig.HatLocationAreaTTMValue, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationAreaOffset);
                    break;
            }
        }

        private void SetHatMode(byte hatModeByte)
        {
            byte oldByte = Config.Stream.GetByte(Config.FileManager.CurrentFileAddress + FileConfig.HatLocationModeOffset);
            byte newByte = MoreMath.ApplyValueToMaskedByte(oldByte, FileConfig.HatLocationModeMask, hatModeByte);
            Config.Stream.SetValue(newByte, Config.FileManager.CurrentFileAddress + FileConfig.HatLocationModeOffset);
        }

        public override void UpdateImage()
        {
            HatLocation? currentHatLocation = GetCurrentHatLocation();
            if (_currentHatLocation != currentHatLocation || !_hasUpdated)
            {
                this.Image = GetImageForValue(currentHatLocation);
                _currentHatLocation = currentHatLocation;
                Invalidate();
            }
            _hasUpdated = true;
        }
    }
}
