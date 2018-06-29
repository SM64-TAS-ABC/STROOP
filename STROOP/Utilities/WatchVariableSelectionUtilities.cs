using STROOP.Controls;
using STROOP.Managers;
using STROOP.Models;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace STROOP.Structs
{
    public static class WatchVariableSelectionUtilities
    {

        public static ContextMenuStrip CreateSelectionContextMenuStrip(WatchVariableFlowLayoutPanel panel)
        {
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            ToolStripMenuItem itemHighlight = new ToolStripMenuItem("Highlight...");
            ControlUtilities.AddDropDownItems(
                itemHighlight,
                new List<string>() { "Set Highlighted", "Set Not Highlighted" },
                new List<Action>()
                {
                    () => panel.ApplySettingsToSelected(new WatchVariableControlSettings(changeHighlighted: true, newHighlighted: true)),
                    () => panel.ApplySettingsToSelected(new WatchVariableControlSettings(changeHighlighted: true, newHighlighted: false)),
                });

            ToolStripMenuItem itemLock = new ToolStripMenuItem("Lock...");
            ControlUtilities.AddDropDownItems(
                itemLock,
                new List<string>() { "Set Locked", "Set Not Locked" },
                new List<Action>()
                {
                    () => panel.ApplySettingsToSelected(new WatchVariableControlSettings(changeLocked: true, newLocked: true)),
                    () => panel.ApplySettingsToSelected(new WatchVariableControlSettings(changeLocked: true, newLocked: false)),
                });

            ToolStripMenuItem itemCopy = new ToolStripMenuItem("Copy");
            ToolStripMenuItem itemPaste = new ToolStripMenuItem("Paste");

            ToolStripMenuItem itemRoundTo = new ToolStripMenuItem("Round to...");
            ToolStripMenuItem itemDisplayAsHex = new ToolStripMenuItem("Display as Hex...");

            ToolStripMenuItem itemAngleSigned = new ToolStripMenuItem("Angle: Signed...");
            ToolStripMenuItem itemAngleUnits = new ToolStripMenuItem("Angle: Units...");
            ToolStripMenuItem itemAngleTruncateToMultipleOf16 = new ToolStripMenuItem("Angle: Truncate to Multiple of 16...");
            ToolStripMenuItem itemAngleConstrainToOneRevolution = new ToolStripMenuItem("Angle: Constrain to One Revolution...");

            ToolStripMenuItem itemMove = new ToolStripMenuItem("Move");
            ToolStripMenuItem itemDelete = new ToolStripMenuItem("Delete");
            ToolStripMenuItem itemOpenController = new ToolStripMenuItem("Open Controller");
            ToolStripMenuItem itemAddToCustomTab = new ToolStripMenuItem("Add to Custom Tab");

            contextMenuStrip.Items.Add(itemHighlight);
            contextMenuStrip.Items.Add(itemLock);
            contextMenuStrip.Items.Add(itemCopy);
            contextMenuStrip.Items.Add(itemPaste);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(itemRoundTo);
            contextMenuStrip.Items.Add(itemDisplayAsHex);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(itemAngleSigned);
            contextMenuStrip.Items.Add(itemAngleUnits);
            contextMenuStrip.Items.Add(itemAngleTruncateToMultipleOf16);
            contextMenuStrip.Items.Add(itemAngleConstrainToOneRevolution);
            contextMenuStrip.Items.Add(new ToolStripSeparator());
            contextMenuStrip.Items.Add(itemMove);
            contextMenuStrip.Items.Add(itemDelete);
            contextMenuStrip.Items.Add(itemOpenController);
            contextMenuStrip.Items.Add(itemAddToCustomTab);

            return contextMenuStrip;
        }
        
    }
}