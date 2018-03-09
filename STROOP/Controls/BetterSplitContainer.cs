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
    public class BetterSplitContainer : SplitContainer
    {
        public BetterSplitContainer()
        {
            ToolStripMenuItem itemJustPanel1 = new ToolStripMenuItem("Just Panel 1");
            itemJustPanel1.Click += (sender, e) =>
            {
                Panel1Collapsed = false;
                Panel2Collapsed = true;
            };

            ToolStripMenuItem itemJustPanel2 = new ToolStripMenuItem("Just Panel 2");
            itemJustPanel2.Click += (sender, e) =>
            {
                Panel1Collapsed = true;
                Panel2Collapsed = false;
            };

            ToolStripMenuItem itemBothPanels = new ToolStripMenuItem("Both Panels");
            itemBothPanels.Click += (sender, e) =>
            {
                Panel1Collapsed = false;
                Panel2Collapsed = false;
            };

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add(itemJustPanel1);
            ContextMenuStrip.Items.Add(itemJustPanel2);
            ContextMenuStrip.Items.Add(itemBothPanels);
        }
    }
}
