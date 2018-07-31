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
    public class WatchVariableAddressWrapper : WatchVariableNumberWrapper
    {
        public WatchVariableAddressWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl)
            : base(watchVar, watchVarControl, DEFAULT_DISPLAY_TYPE, DEFAULT_ROUNDING_LIMIT, true)
        {
            AddAddressContextMenuStripItems();
        }

        private void AddAddressContextMenuStripItems()
        {
            ToolStripMenuItem itemViewAddress = new ToolStripMenuItem("View Address");
            itemViewAddress.Click += (sender, e) =>
            {
                object value = GetValue(true, false, _watchVar.AddressList);
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(value);
                if (!uintValueNullable.HasValue) return;
                uint uintValue = uintValueNullable.Value;
                if (uintValue == 0) return;
                if (ObjectUtilities.IsObjectAddress(uintValue))
                {
                    Config.MemoryManager.SetObjectAddress(uintValue);
                }
                else
                {
                    Config.MemoryManager.SetCustomAddress(uintValue);
                }
                Config.TabControlMain.SelectedTab = Config.TabControlMain.TabPages["tabPageMemory"];
            };

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemViewAddress);
        }

        protected override void HandleVerification(object value)
        {
            base.HandleVerification(value);
            if (!(value is uint))
                throw new ArgumentOutOfRangeException(value + " is not a uint, but represents an address");
        }
    }
}
