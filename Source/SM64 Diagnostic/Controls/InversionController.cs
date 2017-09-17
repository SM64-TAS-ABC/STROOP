using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64Diagnostic.Controls
{
    public static class InversionController
    {
        public static void AddInversionContextMenuStrip(
            Button buttonLeft,
            Button buttonRight)
        {
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
