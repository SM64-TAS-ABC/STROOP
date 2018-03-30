using STROOP.Extensions;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace STROOP.Controls
{
    public class WatchVariableObjectWrapper : WatchVariableNumberWrapper
    {
        private bool _displayAsObject;

        public WatchVariableObjectWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl)
            : base(watchVar, watchVarControl, DEFAULT_ROUNDING_LIMIT, true)
        {
            _displayAsObject = true;

            AddObjectContextMenuStripItems();
        }

        private void AddObjectContextMenuStripItems()
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
                object value = GetValue(true, false);
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(value);
                if (!uintValueNullable.HasValue) return;
                uint uintValue = uintValueNullable.Value;
                Config.ObjectSlotsManager.SelectSlotByAddress(uintValue);
            };

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemDisplayAsObject);
            _contextMenuStrip.AddToBeginningList(itemSelectObject);
        }

        protected override object HandleHexDisplaying(object value)
        {
            // prevent hex display if we're displaying as object
            return _displayAsObject ? value : base.HandleHexDisplaying(value);
        }

        protected override object HandleObjectDisplaying(object value)
        {
            if (!_displayAsObject) return value;

            uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(value);
            if (!uintValueNullable.HasValue) return value;
            uint uintValue = uintValueNullable.Value;

            return Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(uintValue, false);
        }

        protected override object HandleObjectUndisplaying(object value)
        {
            string slotName = value.ToString().ToLower();

            if (slotName == "(no object)" || slotName == "no object") return 0;
            if (slotName == "(unused object)" || slotName == "unused object") return ObjectSlotsConfig.UnusedSlotAddress;

            if (!slotName.StartsWith("slot")) return value;
            slotName = slotName.Remove(0, "slot".Length);
            slotName = slotName.Trim();
            ObjectDataModel obj = Config.ObjectSlotsManager.GetObjectFromLabel(slotName);
            return obj != null ? obj.Address : value;
        }
    }
}
