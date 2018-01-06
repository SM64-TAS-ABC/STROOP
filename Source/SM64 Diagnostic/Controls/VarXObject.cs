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
            : base(addressHolder, varXControl, 0, true)
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
                string stringValue = _varXControl.TextBoxValue;
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(stringValue);
                if (uintValueNullable.HasValue)
                {
                    uint uintValue = uintValueNullable.Value;
                    ObjectSlotsManager objectSlotsManager = ManagerContext.Current.ObjectSlotManager;
                    objectSlotsManager.SelectedSlotsAddresses.Clear();
                    objectSlotsManager.SelectedSlotsAddresses.Add(uintValue);
                }
            };

            _contextMenuStrip.Items.Add(new ToolStripSeparator());
            _contextMenuStrip.Items.Add(itemDisplayAsObject);
            _contextMenuStrip.Items.Add(itemSelectObject);
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

            string slotName = ObjectSlotsManager.Instance.GetSlotNameFromAddressVarX(uintValue);
            return "Slot " + slotName;
        }

        protected override string HandleObjectUndisplaying(string stringValue)
        {
            string slotName = stringValue.ToLower();

            if (slotName == "(no object)" || slotName == "no object") return "0";

            if (!slotName.StartsWith("slot")) return stringValue;
            slotName = slotName.Remove(0, "slot".Length);
            slotName = slotName.Trim();
            uint? address = ObjectSlotsManager.Instance.GetSlotAddressFromNameVarX(slotName);
            return address != null ? address.Value.ToString() : stringValue;
        }
    }
}
