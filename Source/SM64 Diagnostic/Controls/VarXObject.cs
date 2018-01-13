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
            AddressHolder addressHolder,
            VarXControl varXControl)
            : base(addressHolder, varXControl, DEFAULT_ROUNDING_LIMIT, true)
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
                string stringValue = GetStringValue(true, false);
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(stringValue);
                if (!uintValueNullable.HasValue) return;
                uint uintValue = uintValueNullable.Value;
                ObjectSlotsManager.Instance.SelectSlotByAddress(uintValue);
            };

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemDisplayAsObject);
            _contextMenuStrip.AddToBeginningList(itemSelectObject);
        }

        protected override string HandleHexDisplaying(string value)
        {
            // prevent hex display if we're displaying as object
            return _displayAsObject ? value : base.HandleHexDisplaying(value);
        }

        protected override string HandleObjectDisplaying(string stringValue)
        {
            if (!_displayAsObject) return stringValue;

            uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(stringValue);
            if (!uintValueNullable.HasValue) return stringValue;
            uint uintValue = uintValueNullable.Value;

            if (uintValue == 0) return "(no object)";
            if (uintValue == Config.ObjectSlots.UnusedSlotAddress) return "(unused slot)";

            string slotName = ObjectSlotsManager.Instance.GetSlotNameFromAddress(uintValue);
            if (slotName == null) return "(unrecognized slot)";
            return "Slot " + slotName;
        }

        protected override string HandleObjectUndisplaying(string stringValue)
        {
            string slotName = stringValue.ToLower();

            if (slotName == "(no object)" || slotName == "no object") return "0";
            if (slotName == "(unused slot)" || slotName == "unused slot") return Config.ObjectSlots.UnusedSlotAddress.ToString();

            if (!slotName.StartsWith("slot")) return stringValue;
            slotName = slotName.Remove(0, "slot".Length);
            slotName = slotName.Trim();
            uint? address = ObjectSlotsManager.Instance.GetSlotAddressFromName(slotName);
            return address != null ? address.Value.ToString() : stringValue;
        }
    }
}
