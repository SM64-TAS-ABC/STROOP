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

        ~FileImageGui()
        {
            PowerStarImage?.Dispose();
            PowerStarBlackImage?.Dispose();
            CannonImage?.Dispose();
            CannonLidImage?.Dispose();
            Door1StarImage?.Dispose();
            Door3StarImage?.Dispose();
            DoorBlackImage?.Dispose();
        }
    }
}
