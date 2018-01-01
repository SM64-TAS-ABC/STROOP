using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class VarXNumber : VarX
    {
        public VarXNumber(string name, AddressHolder addressHolder)
            : base(name, addressHolder)
        {
            AddContextMenuStrip();
        }

        private void AddContextMenuStrip()
        {
            ToolStripMenuItem itemEdit = new ToolStripMenuItem("N1");
            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("N2");

            itemEdit.Click += (sender, e) => { };
            itemHighlight.Click += (sender, e) => { };

            //contextMenuStrip.Items.Add(new ToolStripSeparator());
            //submenu.DropDownItems.Add(item);

            foreach (Control control in Controls)
            {
                control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
                control.ContextMenuStrip.Items.Add(itemEdit);
                control.ContextMenuStrip.Items.Add(itemHighlight);
            }
        }

    }
}
