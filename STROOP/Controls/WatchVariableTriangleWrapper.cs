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
    public class WatchVariableTriangleWrapper : WatchVariableNumberWrapper
    {
        public WatchVariableTriangleWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl)
            : base(watchVar, watchVarControl, DEFAULT_DISPLAY_TYPE, DEFAULT_ROUNDING_LIMIT, true)
        {
            AddTriangleContextMenuStripItems();
        }

        private void AddTriangleContextMenuStripItems()
        {
            ToolStripMenuItem itemSelectTriangle = new ToolStripMenuItem("Select Triangle");
            itemSelectTriangle.Click += (sender, e) =>
            {
                object value = GetValue(true, false);
                uint? uintValueNullable = ParsingUtilities.ParseUIntNullable(value);
                if (!uintValueNullable.HasValue) return;
                uint uintValue = uintValueNullable.Value;
                Config.TriangleManager.SetCustomTriangleAddress(uintValue);
                // TODO switch to triangle tab
            };

            _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
            _contextMenuStrip.AddToBeginningList(itemSelectTriangle);
        }

        protected override void HandleVerification(object value)
        {
            base.HandleVerification(value);
            if (!(value is uint))
                throw new ArgumentOutOfRangeException(value + " is not a uint, but represents a triangle");
        }
    }
}
