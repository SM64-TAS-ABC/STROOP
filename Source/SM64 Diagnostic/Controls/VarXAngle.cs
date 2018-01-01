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
    public class VarXAngle : VarXNumber
    {
        private bool? _signed;

        public VarXAngle(string name, AddressHolder addressHolder, bool? signed = null)
            : base(name, addressHolder, 0)
        {
            _signed = signed;
            AddAngleContextMenuStrip();
        }

        private void AddAngleContextMenuStrip()
        {
            ToolStripMenuItem itemSign = new ToolStripMenuItem("Signed?");
            ControlUtilities.AddDropDownItems(
                itemSign,
                new List<string> { "Recommended", "Signed", "Unsigned" },
                new List<object> { null, true, false },
                (object obj) => { _signed = (bool?)obj; },
                null);

            Control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Control.ContextMenuStrip.Items.Add(itemSign);
        }

        public override List<object> GetValue()
        {
            return base.GetValue();
        }

        public override void SetValue(string stringValue)
        {
            base.SetValue(stringValue);
        }
    }
}
