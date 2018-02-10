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
    public class BetterContextMenuStrip : ContextMenuStrip
    {
        private int numBeginningItems;

        public BetterContextMenuStrip()
        {
            numBeginningItems = 0;
        }

        public void AddToBeginningList(ToolStripItem item)
        {
            base.Items.Insert(numBeginningItems, item);
            numBeginningItems++;
        }

        public void AddToEndingList(ToolStripItem item)
        {
            base.Items.Add(item);
        }

    }
}
