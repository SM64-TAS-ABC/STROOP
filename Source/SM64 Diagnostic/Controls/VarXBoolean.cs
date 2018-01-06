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
    public class VarXBoolean : VarXNumber
    {
        private bool _displayAsCheckbox;

        public VarXBoolean(
            string name,
            AddressHolder addressHolder,
            Color? backgroundColor)
            : base(name, addressHolder, backgroundColor, 0, false, true)
        {
            _displayAsCheckbox = true;

            AddBooleanContextMenuStripItems();
        }

        protected void AddBooleanContextMenuStripItems()
        {
            ToolStripMenuItem itemDisplayAsCheckbox = new ToolStripMenuItem("Display as Checkbox");
            itemDisplayAsCheckbox.Click += (sender, e) =>
            {
                _displayAsCheckbox = !_displayAsCheckbox;
                itemDisplayAsCheckbox.Checked = _displayAsCheckbox;
                SetUseCheckbox(_displayAsCheckbox);
            };
            itemDisplayAsCheckbox.Checked = _displayAsCheckbox;

            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(itemDisplayAsCheckbox);
        }


        public override CheckState ConvertValueToCheckState(string value)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(value);
            if (!doubleValueNullable.HasValue) return CheckState.Unchecked;
            double doubleValue = doubleValueNullable.Value;
            return doubleValue == 0 ? CheckState.Unchecked : CheckState.Checked;
        }

        public override string ConvertCheckStateToValue(CheckState checkState)
        {
            switch (checkState)
            {
                case CheckState.Unchecked:
                    return "0";
                case CheckState.Checked:
                    return "1";
                case CheckState.Indeterminate:
                    return "";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
