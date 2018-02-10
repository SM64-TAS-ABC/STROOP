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

namespace SM64_Diagnostic
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
