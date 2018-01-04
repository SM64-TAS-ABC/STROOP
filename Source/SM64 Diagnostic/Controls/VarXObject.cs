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
    public class VarXObject : VarXNumber
    {
        private bool _displayAsObject;

        public VarXObject(
            string name,
            AddressHolder addressHolder)
            : base(name, addressHolder, 0, true)
        {
            _displayAsObject = true;
            AddAngleContextMenuStrip();
        }

        private void AddAngleContextMenuStrip()
        {
            ToolStripMenuItem itemDisplayAsObject = new ToolStripMenuItem("Display as Object");
            itemDisplayAsObject.Click += (sender, e) =>
            {
                _displayAsObject = !_displayAsObject;
                itemDisplayAsObject.Checked = _displayAsObject;
            };
            itemDisplayAsObject.Checked = _displayAsObject;

            ToolStripMenuItem itemSelectObject = new ToolStripMenuItem("Select Object");
            itemSelectObject.Click += (sender, e) =>
            {
                // TODO select obj
            };

            Control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Control.ContextMenuStrip.Items.Add(itemDisplayAsObject);
            Control.ContextMenuStrip.Items.Add(itemSelectObject);
        }

        public override string HandleHexDisplaying(string value)
        {
            // prevent hex display if we're displaying as object
            return _displayAsObject ? value : base.HandleHexDisplaying(value);
        }

        public override string HandleObjectDisplaying(string value)
        {
            return base.HandleObjectDisplaying(value);
        }

        public override string HandleObjectUndisplaying(string value)
        {
            return base.HandleObjectUndisplaying(value);
        }
    }
}
