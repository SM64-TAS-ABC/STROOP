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
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowActionDescriptionSelectionForm();
                    addedClickAction = true;
                    break;
                case "PrevActionDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowPreviousActionDescriptionSelectionForm();
                    addedClickAction = true;
                    break;
                case "AnimationDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowAnimationDescriptionSelectionForm();
                    addedClickAction = true;
                    break;
                case "TriangleDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowTriangleDescriptionSelectionForm();
                    addedClickAction = true;
                    break;
                case "DemoCounterDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowDemoCounterDescriptionSelectionForm();
                    addedClickAction = true;
                    break;
                case "TtcSpeedSettingDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowTtcSpeedSettingDescriptionSelectionForm();
                    addedClickAction = true;
                    break;
                case "AreaTerrainDescription":
                    itemSelectValue.Click += (sender, e) => SelectionForm.ShowAreaTerrainDescriptionSelectionForm();
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
