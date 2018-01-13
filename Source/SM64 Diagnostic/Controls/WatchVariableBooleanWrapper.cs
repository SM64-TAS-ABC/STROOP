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
    public class WatchVariableBooleanWrapper : WatchVariableNumberWrapper
    {
        private bool _displayAsCheckbox;
        private bool _displayAsInverted;

        public WatchVariableBooleanWrapper(
            WatchVariable watchVar,
            WatchVariableControl varXControl,
            bool? displayAsInverted)
            : base(watchVar, varXControl, DEFAULT_ROUNDING_LIMIT, DEFAULT_DISPLAY_AS_HEX, true)
        {
            _displayAsCheckbox = true;
            _displayAsInverted = displayAsInverted ?? false;

            AddBooleanContextMenuStripItems();
        }

        private void AddBooleanContextMenuStripItems()
        {
            ToolStripMenuItem itemDisplayAsCheckbox = new ToolStripMenuItem("Display as Checkbox");
            itemDisplayAsCheckbox.Click += (sender, e) =>
            {
                _displayAsCheckbox = !_displayAsCheckbox;
                itemDisplayAsCheckbox.Checked = _displayAsCheckbox;
                _varXControl.SetUseCheckbox(_displayAsCheckbox);
            };
            itemDisplayAsCheckbox.Checked = _displayAsCheckbox;

            ToolStripMenuItem itemDisplayAsInverted = new ToolStripMenuItem("Display as Inverted");
            itemDisplayAsInverted.Click += (sender, e) =>
            {
                _displayAsInverted = !_displayAsInverted;
                itemDisplayAsInverted.Checked = _displayAsInverted;
            };
            itemDisplayAsInverted.Checked = _displayAsInverted;

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemDisplayAsCheckbox);
            _contextMenuStrip.AddToBeginningList(itemDisplayAsInverted);
        }

        protected override CheckState ConvertValueToCheckState(string value)
        {
            double? doubleValueNullable = ParsingUtilities.ParseDoubleNullable(value);
            if (!doubleValueNullable.HasValue) return CheckState.Unchecked;
            double doubleValue = doubleValueNullable.Value;
            return HandleInverting(doubleValue == 0) ? CheckState.Unchecked : CheckState.Checked;
        }

        protected override string ConvertCheckStateToValue(CheckState checkState)
        {
            if (checkState == CheckState.Indeterminate) return "";

            string offValue = "0";
            string onValue = _watchVar.Mask?.ToString() ?? "1";

            return HandleInverting(checkState == CheckState.Unchecked) ? offValue : onValue;
        }

        private bool HandleInverting(bool boolValue)
        {
            return boolValue != _displayAsInverted;
        }


    }
}
