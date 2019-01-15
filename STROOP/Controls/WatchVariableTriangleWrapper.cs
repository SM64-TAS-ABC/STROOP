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
    public class WatchVariableTriangleWrapper : WatchVariableAddressWrapper
    {
        public WatchVariableTriangleWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl)
            : base(watchVar, watchVarControl)
        {
            AddTriangleContextMenuStripItems();
        }

        private void AddTriangleContextMenuStripItems()
        {
            ToolStripMenuItem itemSelectTriangle = new ToolStripMenuItem("Select Triangle");
            itemSelectTriangle.Click += (sender, e) =>
            {
                object value = GetValue(true, false, _watchVar.AddressList);
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(value);
                if (!uintValueNullable.HasValue) return;
                uint uintValue = uintValueNullable.Value;
                Config.TriangleManager.SetCustomTriangleAddress(uintValue);
            };

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemSelectTriangle);
        }
    }
}
