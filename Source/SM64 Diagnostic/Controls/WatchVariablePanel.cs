using SM64_Diagnostic.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class WatchVariablePanel : FlowLayoutPanel
    {
        private readonly Object _objectLock;
        private readonly List<WatchVariableControl> _watchVarControlsList;
        private readonly List<VariableGroup> _allGroups;
        private readonly List<VariableGroup> _visibleGroups;

        public WatchVariablePanel()
        {
            _objectLock = new Object();
            _watchVarControlsList = new List<WatchVariableControl>();
            _allGroups = new List<VariableGroup>();
            _visibleGroups = new List<VariableGroup>();
            ContextMenuStrip = new ContextMenuStrip();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        public void SetVariableGroups(List<VariableGroup> allGroups, List<VariableGroup> visibleGroups)
        {
            _allGroups.Clear();
            _allGroups.AddRange(allGroups);
            _visibleGroups.Clear();
            _visibleGroups.AddRange(visibleGroups);
            UpdateControlsBasedOnFilters();

            _allGroups.ForEach(varGroup =>
            {
                ToolStripMenuItem item = CreateVariableGroupItem(varGroup, _visibleGroups.Contains(varGroup));
                ContextMenuStrip.Items.Add(item);
            });
        }

        private ToolStripMenuItem CreateVariableGroupItem(VariableGroup varGroup, bool visible)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(varGroup.ToString());
            item.Click += (sender, e) =>
            {
                item.Checked = !item.Checked;
                if (item.Checked) // visible
                {
                    _visibleGroups.Add(varGroup);
                }
                else // hidden
                {
                    _visibleGroups.Remove(varGroup);
                }
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

        public void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            lock (_objectLock)
            {
                watchVarControls.ForEach(watchVarControl =>
                {
                    _watchVarControlsList.Add(watchVarControl);
                    if (_allGroups.Count == 0 || watchVarControl.BelongsToAnyGroup(_visibleGroups))
                        Controls.Add(watchVarControl);
                });
            }
        }

        public void RemoveVariables(List<WatchVariableControl> watchVarControls)
        {
            lock (_objectLock)
            {
                watchVarControls.ForEach(watchVarControl =>
                {
                    _watchVarControlsList.Remove(watchVarControl);
                    if (_allGroups.Count == 0 || watchVarControl.BelongsToAnyGroup(_visibleGroups))
                        Controls.Remove(watchVarControl);
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

        public void UpdateControls()
        {
            _watchVarControlsList.ForEach(watchVarControl => watchVarControl.UpdateControl());
        }
    }
}
