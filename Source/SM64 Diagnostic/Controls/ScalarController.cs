using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64Diagnostic.Controls
{
    public static class ScalarController
    {
        public static void initialize(
            Button buttonLeft,
            Button buttonRight,
            TextBox textbox,
            Action<float> actionChangeScalar)
        {
            Action<int> actionButtonClick = (int sign) =>
            {
                float value;
                if (!float.TryParse(textbox.Text, out value)) return;
                actionChangeScalar(sign * value);
            };

            buttonLeft.Click += (sender, e) => actionButtonClick(-1);
            buttonRight.Click += (sender, e) => actionButtonClick(1);
        }
    }
}
