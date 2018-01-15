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

        private Action _updateItemsFunction;

        private bool _hasSetVariableGroups;
        private bool _hasAddedVariables;

        public WatchVariablePanel()
        {
            _objectLock = new Object();
            _watchVarControlsList = new List<WatchVariableControl>();
            _allGroups = new List<VariableGroup>();
            _visibleGroups = new List<VariableGroup>();

            ContextMenuStrip = new ContextMenuStrip();

            _hasSetVariableGroups = false;
            _hasAddedVariables = false;
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
            if (_hasSetVariableGroups || _hasAddedVariables)
            {
                throw new ArgumentOutOfRangeException(
                    "Can only set var groups once, and must be done before adding vars");
            }
            _hasSetVariableGroups = true;

            _allGroups.AddRange(allGroups);
            _visibleGroups.AddRange(visibleGroups);

            (List<ToolStripMenuItem> items, Action updateFunction) = CreateFilterItemsAndUpdateFunction();
            items.ForEach(item => ContextMenuStrip.Items.Add(item));
            _updateItemsFunction = updateFunction;
        }

        private (List<ToolStripMenuItem> items, Action updateFunction) CreateFilterItemsAndUpdateFunction()
        {
            List<ToolStripMenuItem> items =
                _allGroups.ConvertAll(varGroup =>
                    CreateFilterItem(varGroup));

            Action updateFunction = () =>
            {
                for(int i = 0; i < _allGroups.Count; i++)
                {
                    items[i].Checked = _visibleGroups.Contains(_allGroups[i]);
                }
            };

            return (items, updateFunction);
        }

        private ToolStripMenuItem CreateFilterItem(VariableGroup varGroup)
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
                UpdateControlsBasedOnFilters();
            };
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
            _hasAddedVariables = true;
            lock (_objectLock)
            {
                watchVarControls.ForEach(watchVarControl =>
                {
                    _watchVarControlsList.Add(watchVarControl);
                    if (ShouldShow(watchVarControl)) Controls.Add(watchVarControl);
                    watchVarControl.NotifyPanel(this);
                    /*
                    if (_hasSetVariableGroups)
                    {
                        (List<ToolStripMenuItem> items, Action updateFunction) =
                            CreateFilterItemsAndUpdateFunction();
                        watchVarControl.NotifyFiltering(items, updateFunction);
                    }
                    */
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
                    watchVarControl.NotifyPanel(null);
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

        public void UpdateControls()
        {
            _updateItemsFunction?.Invoke();
            _watchVarControlsList.ForEach(watchVarControl => watchVarControl.UpdateControl());
        }

        private bool ShouldShow(WatchVariableControl watchVarControl)
        {
            return _allGroups.Count == 0 || watchVarControl.BelongsToAnyGroup(_visibleGroups);
        }
    }
}
