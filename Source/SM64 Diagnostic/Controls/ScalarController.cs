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

            // Implement ToolStripMenu

            Point leftPoint = new Point(buttonLeft.Location.X, buttonLeft.Location.Y);
            Point rightPoint = new Point(buttonRight.Location.X, buttonRight.Location.Y);

            ToolStripMenuItem itemNormal = new ToolStripMenuItem("Normal");
            ToolStripMenuItem itemInverted = new ToolStripMenuItem("Inverted");

            Action<bool> SetOrientation = (bool inverted) =>
            {
                itemNormal.Checked = !inverted;
                itemInverted.Checked = inverted;
                buttonLeft.Location = inverted ? rightPoint : leftPoint;
                buttonRight.Location = inverted ? leftPoint : rightPoint;
            };

            itemNormal.Click += (sender, e) => SetOrientation(false);
            itemInverted.Click += (sender, e) => SetOrientation(true);

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(itemNormal);
            contextMenuStrip.Items.Add(itemInverted);
            buttonLeft.ContextMenuStrip = contextMenuStrip;
            buttonRight.ContextMenuStrip = contextMenuStrip;

            itemNormal.Checked = true;
        }
    }
}
