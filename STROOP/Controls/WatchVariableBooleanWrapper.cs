using STROOP.Utilities;
using System.Windows.Forms;

namespace STROOP.Controls
{
    public class WatchVariableBooleanWrapper : WatchVariableNumberWrapper
    {
        private bool _displayAsCheckbox;
        private bool _displayAsInverted;

        public WatchVariableBooleanWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl,
            bool? displayAsInverted)
            : base(watchVar, watchVarControl, DEFAULT_ROUNDING_LIMIT, DEFAULT_DISPLAY_AS_HEX, true)
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
                _watchVarControl.SetUseCheckbox(_displayAsCheckbox);
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
