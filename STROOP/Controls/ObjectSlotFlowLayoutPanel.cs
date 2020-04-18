using STROOP.Forms;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Controls
{
    public class ObjectSlotFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        public ObjectSlotFlowLayoutPanel()
        {
            ToolStripMenuItem itemSelectMarkedSlots = new ToolStripMenuItem("Select Marked Slots");
            itemSelectMarkedSlots.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.SelectedSlotsAddresses.Clear();
                Config.ObjectSlotsManager.SelectedSlotsAddresses.AddRange(Config.ObjectSlotsManager.MarkedSlotsAddresses);
            };

            ToolStripMenuItem itemSelectCopiedAddress = new ToolStripMenuItem("Select Copied Address");
            itemSelectCopiedAddress.Click += (sender, e) =>
            {
                uint? address = ParsingUtilities.ParseHexNullable(Clipboard.GetText());
                if (address.HasValue) Config.ObjectSlotsManager.SelectSlotByAddress(address.Value);
            };

            ToolStripMenuItem itemClearMarkedSlots = new ToolStripMenuItem("Clear Marked Slots");
            itemClearMarkedSlots.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.MarkedSlotsAddresses.Clear();
            };

            ToolStripMenuItem itemClearSelectedSlots = new ToolStripMenuItem("Clear Selected Slots");
            itemClearSelectedSlots.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.SelectedSlotsAddresses.Clear();
            };

            ToolStripMenuItem itemFixHorizontalScroll = new ToolStripMenuItem("Fix Horizontal Scroll");
            itemFixHorizontalScroll.Click += (sender, e) =>
            {
                List<Control> controls = new List<Control>();
                foreach (Control control in Controls)
                {
                    controls.Add(control);
                }
                while (Controls.Count > 0)
                {
                    Controls.RemoveAt(0);
                }
                foreach (Control control in controls)
                {
                    Controls.Add(control);
                }
            };

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add(itemSelectMarkedSlots);
            ContextMenuStrip.Items.Add(itemSelectCopiedAddress);
            ContextMenuStrip.Items.Add(itemClearMarkedSlots);
            ContextMenuStrip.Items.Add(itemClearSelectedSlots);
            ContextMenuStrip.Items.Add(itemFixHorizontalScroll);
        }
    }
}
