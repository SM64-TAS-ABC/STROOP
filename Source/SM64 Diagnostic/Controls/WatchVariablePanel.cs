using SM64_Diagnostic.Forms;
using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class WatchVariablePanel : NoTearFlowLayoutPanel
    {
        private readonly Object _objectLock;
        private readonly List<WatchVariableControlPrecursor> _precursors;
        private readonly List<WatchVariableControl> _watchVarControlsList;
        private readonly List<VariableGroup> _allGroups;
        private readonly List<VariableGroup> _initialVisibleGroups;
        private readonly List<VariableGroup> _visibleGroups;

        private WatchVariableControl _reorderingWatchVarControl;

        public WatchVariablePanel()
        {
            _objectLock = new Object();
            _precursors = new List<WatchVariableControlPrecursor>();
            _watchVarControlsList = new List<WatchVariableControl>();
            _allGroups = new List<VariableGroup>();
            _initialVisibleGroups = new List<VariableGroup>();
            _visibleGroups = new List<VariableGroup>();

            ContextMenuStrip = new ContextMenuStrip();

            _reorderingWatchVarControl = null;
        }

        public void Initialize(
            List<WatchVariableControlPrecursor> precursors,
            List<VariableGroup> allGroups = null,
            List<VariableGroup> visibleGroups = null)
        {
            if (allGroups != null) _allGroups.AddRange(allGroups);
            if (visibleGroups != null) _initialVisibleGroups.AddRange(visibleGroups);
            if (visibleGroups != null) _visibleGroups.AddRange(visibleGroups);
            _precursors.AddRange(precursors);
            AddVariables(_precursors.ConvertAll(precursor => precursor.CreateWatchVariableControl()));

            AddItemsToContextMenuStrip();
        }

        private void AddItemsToContextMenuStrip()
        {
            ToolStripMenuItem enableCustomization = new ToolStripMenuItem("Enable Customization");
            enableCustomization.Click += (sender, e) => EnableCustomVariableFunctionality();

            ToolStripMenuItem showVariableXmlItem = new ToolStripMenuItem("Show Variable XML");
            showVariableXmlItem.Click += (sender, e) => ShowVariableXml();

            ToolStripMenuItem resetVariablesItem = new ToolStripMenuItem("Reset Variables");
            resetVariablesItem.Click += (sender, e) => ResetVariables();

            ToolStripMenuItem filterVariablesItem = new ToolStripMenuItem("Filter Variables...");
            List<ToolStripMenuItem> filteringDropDownItems =
                _allGroups.ConvertAll(varGroup =>
                    CreateFilterItem(varGroup, _visibleGroups.Contains(varGroup)));
            filteringDropDownItems.ForEach(item => filterVariablesItem.DropDownItems.Add(item));
            filterVariablesItem.DropDown.AutoClose = false;
            filterVariablesItem.DropDown.MouseLeave += (sender, e) => { filterVariablesItem.DropDown.Close(); };

            ContextMenuStrip.Items.Add(enableCustomization);
            ContextMenuStrip.Items.Add(showVariableXmlItem);
            ContextMenuStrip.Items.Add(resetVariablesItem);
            ContextMenuStrip.Items.Add(filterVariablesItem);
        }

        private ToolStripMenuItem CreateFilterItem(VariableGroup varGroup, bool visible)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(varGroup.ToString());
            item.Click += (sender, e) =>
            {
                bool newVisibility = !_visibleGroups.Contains(varGroup);
                if (newVisibility) // visible
                {
                    _visibleGroups.Add(varGroup);
                }
                else // hidden
                {
                    _visibleGroups.Remove(varGroup);
                }
                item.Checked = newVisibility;
                UpdateControlsBasedOnFilters();
            };
            item.Checked = visible;
            return item;
        }

        private void UpdateControlsBasedOnFilters()
        {
            lock (_objectLock)
            {
                Controls.Clear();
                _watchVarControlsList.ForEach(watchVarControl =>
                {
                    if (watchVarControl.BelongsToAnyGroup(_visibleGroups))
                        Controls.Add(watchVarControl);
                });
            }
        }

        public void AddVariable(WatchVariableControl watchVarControl)
        {
            AddVariables(new List<WatchVariableControl>() { watchVarControl });
        }

        public void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            lock (_objectLock)
            {
                watchVarControls.ForEach(watchVarControl =>
                {
                    _watchVarControlsList.Add(watchVarControl);
                    if (ShouldShow(watchVarControl)) Controls.Add(watchVarControl);
                    watchVarControl.SetPanel(this);
                });
            }
        }

        public void RemoveVariable(WatchVariableControl watchVarControl)
        {
            RemoveVariables(new List<WatchVariableControl>() { watchVarControl });
        }

        public void RemoveVariables(List<WatchVariableControl> watchVarControls)
        {
            lock (_objectLock)
            {
                watchVarControls.ForEach(watchVarControl =>
                {
                    _watchVarControlsList.Remove(watchVarControl);
                    if (ShouldShow(watchVarControl)) Controls.Remove(watchVarControl);
                    watchVarControl.SetPanel(null);
                });
            }
        }

        public void RemoveVariables(VariableGroup varGroup)
        {
            List<WatchVariableControl> watchVarControls =
                _watchVarControlsList.FindAll(
                    watchVarControl => watchVarControl.BelongsToGroup(varGroup));
            RemoveVariables(watchVarControls);
        }

        public void ClearVariables()
        {
            List<WatchVariableControl> watchVarControlListCopy =
                new List<WatchVariableControl>(_watchVarControlsList);
            RemoveVariables(watchVarControlListCopy);
        }

        public void ResetVariables()
        {
            _watchVarControlsList.ForEach(control => System.Diagnostics.Trace.WriteLine(control));
            ClearVariables();
            _visibleGroups.Clear();
            _visibleGroups.AddRange(_initialVisibleGroups);
            AddVariables(_precursors.ConvertAll(precursor => precursor.CreateWatchVariableControl()));
        }

        public void ShowVariableXml()
        {
            InfoForm infoForm = new InfoForm();
            infoForm.SetText(
                "Variable Info",
                "Variable XML",
                String.Join("\r\n", _precursors));
            infoForm.Show();
        }

        public void EnableCustomVariableFunctionality()
        {
            _watchVarControlsList.ForEach(control => control.EnableCustomFunctionality());
        }

        public void NotifyOfReordering(WatchVariableControl watchVarControl)
        {
            if (_reorderingWatchVarControl == null)
            {
                _reorderingWatchVarControl = watchVarControl;
                _reorderingWatchVarControl.FlashColor(WatchVariableControl.REORDER_START_COLOR);
            }
            else if (watchVarControl == _reorderingWatchVarControl)
            {
                _reorderingWatchVarControl.FlashColor(WatchVariableControl.REORDER_RESET_COLOR);
                _reorderingWatchVarControl = null;
            }
            else
            {
                int newIndex = Controls.IndexOf(watchVarControl);
                Controls.SetChildIndex(_reorderingWatchVarControl, newIndex);
                _reorderingWatchVarControl.FlashColor(WatchVariableControl.REORDER_END_COLOR);
                _reorderingWatchVarControl = null;
            }
        }

        public void UpdateControls()
        {
            _watchVarControlsList.ForEach(watchVarControl => watchVarControl.UpdateControl());
        }

        private bool ShouldShow(WatchVariableControl watchVarControl)
        {
            return _allGroups.Count == 0 || watchVarControl.BelongsToAnyGroup(_visibleGroups);
        }
    }
}
