using STROOP.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs
{
    public class InputImageGui
    {
        public Image ButtonAImage;
        public Image ButtonBImage;
        public Image ButtonZImage;
        public Image ButtonStartImage;
        public Image ButtonRImage;
        public Image ButtonLImage;
        public Image ButtonCUpImage;
        public Image ButtonCDownImage;
        public Image ButtonCLeftImage;
        public Image ButtonCRightImage;
        public Image ButtonDUpImage;
        public Image ButtonDDownImage;
        public Image ButtonDLeftImage;
        public Image ButtonDRightImage;
        public Image ButtonU1Image;
        public Image ButtonU2Image;
        public Image ControlStickImage;
        public Image ControllerImage;

        ~InputImageGui()
        {
            ButtonAImage?.Dispose();
            ButtonBImage?.Dispose();
            ButtonZImage?.Dispose();
            ButtonStartImage?.Dispose();
            ButtonRImage?.Dispose();
            ButtonLImage?.Dispose();
            ButtonCUpImage?.Dispose();
            ButtonCDownImage?.Dispose();
            ButtonCLeftImage?.Dispose();
            ButtonCRightImage?.Dispose();
            ButtonDUpImage?.Dispose();
            ButtonDDownImage?.Dispose();
            ButtonDLeftImage?.Dispose();
            ButtonDRightImage?.Dispose();
            ButtonU1Image?.Dispose();
            ButtonU2Image?.Dispose();
            ControlStickImage?.Dispose();
            ControllerImage?.Dispose();
        }
    }
}
