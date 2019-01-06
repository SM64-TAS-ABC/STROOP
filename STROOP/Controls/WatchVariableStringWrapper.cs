using STROOP.Extensions;
using STROOP.Forms;
using STROOP.Managers;
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
    public class WatchVariableStringWrapper : WatchVariableWrapper
    {
        public WatchVariableStringWrapper(
            WatchVariable watchVar,
            WatchVariableControl watchVarControl)
            : base(watchVar, watchVarControl, DEFAULT_USE_CHECKBOX)
        {
            AddStringContextMenuStripItems(watchVar.SpecialType);
        }

        private void AddStringContextMenuStripItems(string specialType)
        {
            ToolStripMenuItem itemSelectValue = new ToolStripMenuItem("Select Value...");
            bool addedClickAction = false;

            switch (specialType)
            {
                case "ActionDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowActionSelectionForm();
                    addedClickAction = true;
                    break;
            }

            if (addedClickAction)
            {
                _contextMenuStrip.AddToBeginningList(new ToolStripSeparator());
                _contextMenuStrip.AddToBeginningList(itemSelectValue);
            }
        }

        protected override void HandleVerification(object value)
        {
            base.HandleVerification(value);
            if (!(value is string))
                throw new ArgumentOutOfRangeException(value + " is not a string");
        }
    }
}
