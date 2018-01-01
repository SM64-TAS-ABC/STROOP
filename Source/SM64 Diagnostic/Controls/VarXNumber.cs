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
        private static readonly int NUM_ROUNDING_OPTIONS = 10;
        private static readonly int DEFAULT_ROUNDING_LIMIT = 3;

        private int? _roundingLimit = DEFAULT_ROUNDING_LIMIT;
        private bool _negate = false;

        public VarXNumber(string name, AddressHolder addressHolder)
            : base(name, addressHolder)
        {
            AddNumberContextMenuStrip();
        }

        private void AddNumberContextMenuStrip()
        {
            ToolStripMenuItem itemRoundTo = new ToolStripMenuItem("Round to ...");

            ToolStripMenuItem itemRoundNone = new ToolStripMenuItem("No rounding");
            List<ToolStripMenuItem> itemRoundList = new List<ToolStripMenuItem>();
            for (int i = 0; i <= NUM_ROUNDING_OPTIONS; i++)
            {
                itemRoundList.Add(new ToolStripMenuItem(i + " decimal place(s)"));
            }

            itemRoundNone.Click += (sender, e) =>
            {
                _roundingLimit = null;
                itemRoundNone.Checked = true;
                itemRoundList.ForEach(item => item.Checked = false);
            };
            for (int i = 0; i < itemRoundList.Count; i++)
            {
                ToolStripMenuItem item = itemRoundList[i];
                int index = i;
                item.Click += (sender, e) =>
                {
                    _roundingLimit = index;
                    itemRoundNone.Checked = false;
                    itemRoundList.ForEach(item2 => item2.Checked = item2 == item);
                };
            }

            if (_roundingLimit.HasValue) itemRoundList[_roundingLimit.Value].Checked = true;
            else itemRoundNone.Checked = true;

            itemRoundTo.DropDownItems.Add(itemRoundNone);
            itemRoundList.ForEach(item => itemRoundTo.DropDownItems.Add(item));

            ToolStripMenuItem itemNegate = new ToolStripMenuItem("Negate");
            itemNegate.Click += (sender, e) =>
            {
                _negate = !_negate;
                itemNegate.Checked = _negate;
            };

            Control.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            Control.ContextMenuStrip.Items.Add(itemRoundTo);
            Control.ContextMenuStrip.Items.Add(itemNegate);
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
