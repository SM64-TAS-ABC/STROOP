using SM64_Diagnostic.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Structs
{
    public class FileImageGui
    {
        public Image PowerStarImage;
        public Image PowerStarBlackImage;
        public Image CannonImage;
        public Image CannonLidImage;
        public Image Door1StarImage;
        public Image Door3StarImage;
        public Image DoorBlackImage;
        public Image StarDoorOpenImage;
        public Image StarDoorClosedImage;
        public Image CapSwitchRedPressedImage;
        public Image CapSwitchRedUnpressedImage;
        public Image CapSwitchGreenPressedImage;
        public Image CapSwitchGreenUnpressedImage;
        public Image CapSwitchBluePressedImage;
        public Image CapSwitchBlueUnpressedImage;
        public Image FileStartedImage;
        public Image FileNotStartedImage;
        public Image DDDPaintingMovedBackImage;
        public Image DDDPaintingNotMovedBackImage;
        public Image MoatDrainedImage;
        public Image MoatNotDrainedImage;
        public Image KeyDoorClosedImage;
        public Image KeyDoorClosedKeyImage;
        public Image KeyDoorOpenImage;
        public Image KeyDoorOpenKeyImage;
        public Image HatOnMarioImage;
        public Image HatOnMarioGreyImage;
        public Image HatOnKleptoImage;
        public Image HatOnKleptoGreyImage;
        public Image HatOnSnowmanImage;
        public Image HatOnSnowmanGreyImage;
        public Image HatOnUkikiImage;
        public Image HatOnUkikiGreyImage;
        public Image HatOnGroundInSSLImage;
        public Image HatOnGroundInSSLGreyImage;
        public Image HatOnGroundInSLImage;
        public Image HatOnGroundInSLGreyImage;
        public Image HatOnGroundInTTMImage;
        public Image HatOnGroundInTTMGreyImage;


        ~FileImageGui()
        {
            PowerStarImage?.Dispose();
            PowerStarBlackImage?.Dispose();
            CannonImage?.Dispose();
            CannonLidImage?.Dispose();
            Door1StarImage?.Dispose();
            Door3StarImage?.Dispose();
            DoorBlackImage?.Dispose();
            StarDoorOpenImage?.Dispose();
            StarDoorClosedImage?.Dispose();
            CapSwitchRedPressedImage?.Dispose();
            CapSwitchRedUnpressedImage?.Dispose();
            CapSwitchGreenPressedImage?.Dispose();
            CapSwitchGreenUnpressedImage?.Dispose();
            CapSwitchBluePressedImage?.Dispose();
            CapSwitchBlueUnpressedImage?.Dispose();
            FileStartedImage?.Dispose();
            FileNotStartedImage?.Dispose();
            DDDPaintingMovedBackImage?.Dispose();
            DDDPaintingNotMovedBackImage?.Dispose();
            MoatDrainedImage?.Dispose();
            MoatNotDrainedImage?.Dispose();
            KeyDoorClosedImage?.Dispose();
            KeyDoorClosedKeyImage?.Dispose();
            KeyDoorOpenImage?.Dispose();
            KeyDoorOpenKeyImage?.Dispose();
            HatOnMarioImage?.Dispose();
            HatOnMarioGreyImage?.Dispose();
            HatOnKleptoImage?.Dispose();
            HatOnKleptoGreyImage?.Dispose();
            HatOnSnowmanImage?.Dispose();
            HatOnSnowmanGreyImage?.Dispose();
            HatOnUkikiImage?.Dispose();
            HatOnUkikiGreyImage?.Dispose();
            HatOnGroundInSSLImage?.Dispose();
            HatOnGroundInSSLGreyImage?.Dispose();
            HatOnGroundInSLImage?.Dispose();
            HatOnGroundInSLGreyImage?.Dispose();
            HatOnGroundInTTMImage?.Dispose();
            HatOnGroundInTTMGreyImage?.Dispose();
        }
    }
}
