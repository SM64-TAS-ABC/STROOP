using STROOP.Forms;
using STROOP.Managers;
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
    public class WatchVariableFlowLayoutPanel : NoTearFlowLayoutPanel
    {
        public bool IsInitialized { get; private set; }

        private string _varFilePath;

        private readonly Object _objectLock;
        private List<WatchVariableControl> _watchVarControls;
        private readonly List<VariableGroup> _allGroups;
        private readonly List<VariableGroup> _initialVisibleGroups;
        private readonly List<VariableGroup> _visibleGroups;
        private List<ToolStripMenuItem> _filteringDropDownItems;

        private List<WatchVariableControl> _selectedWatchVarControls;
        private List<WatchVariableControl> _reorderingWatchVarControls;

        private List<ToolStripItem> _selectionToolStripItems;

        public WatchVariableFlowLayoutPanel()
        {
            IsInitialized = false;

            _objectLock = new Object();
            _watchVarControls = new List<WatchVariableControl>();
            _allGroups = new List<VariableGroup>();
            _initialVisibleGroups = new List<VariableGroup>();
            _visibleGroups = new List<VariableGroup>();

            ContextMenuStrip = new ContextMenuStrip();

            _selectedWatchVarControls = new List<WatchVariableControl>();
            _reorderingWatchVarControls = new List<WatchVariableControl>();

            Click += (sender, e) =>
            {
                UnselectAllVariables();
                StopEditing();
            };
            ContextMenuStrip.Opening += (sender, e) => UnselectAllVariables();
        }

        public void Initialize(
            string varFilePath = null,
            List<VariableGroup> allVariableGroupsNullable = null,
            List<VariableGroup> visibleVariableGroupsNullable = null)
        {
            if (IsInitialized)
            {
                throw new ArgumentOutOfRangeException("WatchVariableFlowLayoutPanel already initialized");
            }

            List<VariableGroup> allVariableGroups = allVariableGroupsNullable ?? new List<VariableGroup>();
            if (allVariableGroups.Contains(VariableGroup.Custom)) throw new ArgumentOutOfRangeException();
            allVariableGroups.Add(VariableGroup.Custom);

            List<VariableGroup> visibleVariableGroups = visibleVariableGroupsNullable ?? new List<VariableGroup>();
            if (visibleVariableGroups.Contains(VariableGroup.Custom)) throw new ArgumentOutOfRangeException();
            visibleVariableGroups.Add(VariableGroup.Custom);

            _varFilePath = varFilePath;
            _allGroups.AddRange(allVariableGroups);
            _initialVisibleGroups.AddRange(visibleVariableGroups);
            _visibleGroups.AddRange(visibleVariableGroups);

            _selectionToolStripItems =
                WatchVariableSelectionUtilities.CreateSelectionToolStripItems(
                    () => new List<WatchVariableControl>(_selectedWatchVarControls), this);

            List<WatchVariableControlPrecursor> precursors = _varFilePath == null
                ? new List<WatchVariableControlPrecursor>()
                : XmlConfigParser.OpenWatchVariableControlPrecursors(_varFilePath);
            AddVariables(precursors.ConvertAll(precursor => precursor.CreateWatchVariableControl()));
            AddItemsToContextMenuStrip();

            IsInitialized = true;
        }

        private void AddItemsToContextMenuStrip()
        {
            ToolStripMenuItem resetVariablesItem = new ToolStripMenuItem("Reset Variables");
            resetVariablesItem.Click += (sender, e) => ResetVariables();

            ToolStripMenuItem clearAllButHighlightedItem = new ToolStripMenuItem("Clear All But Highlighted");
            clearAllButHighlightedItem.Click += (sender, e) => ClearAllButHighlightedVariables();

            ToolStripMenuItem fixVerticalScrollItem = new ToolStripMenuItem("Fix Vertical Scroll");
            fixVerticalScrollItem.Click += (sender, e) => FixVerticalScroll();

            ToolStripMenuItem addCustomVariablesItem = new ToolStripMenuItem("Add Custom Variables");
            addCustomVariablesItem.Click += (sender, e) =>
            {
                VariableCreationForm form = new VariableCreationForm();
                form.Initialize(this);
                form.Show();
            };

            ToolStripMenuItem addMappingVariablesItem = new ToolStripMenuItem("Add Mapping Variables");
            addMappingVariablesItem.Click += (sender, e) => AddVariables(MappingConfig.GetVariables());

            ToolStripMenuItem addDummyVariableItem = new ToolStripMenuItem("Add Dummy Variable...");
            foreach (string typeString in TypeUtilities.InGameTypeList)
            {
                ToolStripMenuItem typeItem = new ToolStripMenuItem(typeString);
                addDummyVariableItem.DropDownItems.Add(typeItem);
                typeItem.Click += (sender, e) =>
                {
                    int numEntries = 1;
                    if (KeyboardUtilities.IsCtrlHeld())
                    {
                        string numEntriesString = DialogUtilities.GetStringFromDialog(labelText: "Enter Num Vars:");
                        if (numEntriesString == null) return;
                        int parsed = ParsingUtilities.ParseInt(numEntriesString);
                        parsed = Math.Max(parsed, 0);
                        numEntries = parsed;
                    }

                    List<WatchVariableControl> controls = new List<WatchVariableControl>();
                    for (int i = 0; i < numEntries; i++)
                    {
                        string specialType = WatchVariableSpecialUtilities.AddDummyEntry(typeString);
                        WatchVariable watchVariable =
                            new WatchVariable(
                                memoryTypeName: null,
                                specialType: specialType,
                                baseAddressType: BaseAddressTypeEnum.None,
                                offsetUS: null,
                                offsetJP: null,
                                offsetSH: null,
                                offsetEU: null,
                                offsetDefault: null,
                                mask: null,
                                shift: null,
                                handleMapping: true);
                        WatchVariableControlPrecursor precursor =
                            new WatchVariableControlPrecursor(
                                name: specialType,
                                watchVar: watchVariable,
                                subclass: WatchVariableSubclass.Number,
                                backgroundColor: null,
                                displayType: null,
                                roundingLimit: null,
                                useHex: null,
                                invertBool: null,
                                isYaw: null,
                                coordinate: null,
                                groupList: new List<VariableGroup>() { VariableGroup.Custom });
                        WatchVariableControl control = precursor.CreateWatchVariableControl();
                        controls.Add(control);
                    }
                    AddVariables(controls);
                };
            }

            ToolStripMenuItem openSaveClearItem = new ToolStripMenuItem("Open / Save / Clear ...");
            ControlUtilities.AddDropDownItems(
                openSaveClearItem,
                new List<string>() { "Open", "Open as Pop Out", "Save in Place", "Save As", "Clear" },
                new List<Action>()
                {
                    () => OpenVariables(),
                    () => OpenVariablesAsPopOut(),
                    () => SaveVariablesInPlace(),
                    () => SaveVariables(),
                    () => ClearVariables(),
                });

            ToolStripMenuItem doToAllVariablesItem = new ToolStripMenuItem("Do to all variables...");
            WatchVariableSelectionUtilities.CreateSelectionToolStripItems(
                () => GetCurrentVariableControls(), this)
                .ForEach(item => doToAllVariablesItem.DropDownItems.Add(item));

            ToolStripMenuItem filterVariablesItem = new ToolStripMenuItem("Filter Variables...");
            _filteringDropDownItems = _allGroups.ConvertAll(varGroup => CreateFilterItem(varGroup));
            UpdateFilterItemCheckedStatuses();
            _filteringDropDownItems.ForEach(item => filterVariablesItem.DropDownItems.Add(item));
            filterVariablesItem.DropDown.MouseEnter += (sender, e) =>
            {
                filterVariablesItem.DropDown.AutoClose = false;
            };
            filterVariablesItem.DropDown.MouseLeave += (sender, e) =>
            {
                filterVariablesItem.DropDown.AutoClose = true;
                filterVariablesItem.DropDown.Close();
            };

            ContextMenuStrip.Items.Add(resetVariablesItem);
            ContextMenuStrip.Items.Add(clearAllButHighlightedItem);
            ContextMenuStrip.Items.Add(fixVerticalScrollItem);
            ContextMenuStrip.Items.Add(addCustomVariablesItem);
            ContextMenuStrip.Items.Add(addMappingVariablesItem);
            ContextMenuStrip.Items.Add(addDummyVariableItem);
            ContextMenuStrip.Items.Add(openSaveClearItem);
            ContextMenuStrip.Items.Add(doToAllVariablesItem);
            ContextMenuStrip.Items.Add(filterVariablesItem);
        }

        public List<ToolStripItem> GetSelectionToolStripItems()
        {
            return _selectionToolStripItems;
        }

        private ToolStripMenuItem CreateFilterItem(VariableGroup varGroup)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(varGroup.ToString());
            item.Click += (sender, e) => ToggleVarGroupVisibility(varGroup);
            return item;
        }

        private void ToggleVarGroupVisibility(VariableGroup varGroup, bool? newVisibilityNullable = null)
        {
            // Toggle visibility if no visibility is provided
            bool newVisibility = newVisibilityNullable ?? !_visibleGroups.Contains(varGroup);
            if (newVisibility) // change to visible
            {
                _visibleGroups.Add(varGroup);
            }
            else // change to hidden
            {
                _visibleGroups.Remove(varGroup);
            }
            UpdateControlsBasedOnFilters();
            UpdateFilterItemCheckedStatuses();
        }

        private void UpdateFilterItemCheckedStatuses()
        {
            if (_allGroups.Count != _filteringDropDownItems.Count) throw new ArgumentOutOfRangeException();

            for (int i = 0; i < _allGroups.Count; i++)
            {
                _filteringDropDownItems[i].Checked = _visibleGroups.Contains(_allGroups[i]);
            }
        }

        private void UpdateControlsBasedOnFilters()
        {
            lock (_objectLock)
            {
                Controls.Clear();
                _watchVarControls.ForEach(watchVarControl =>
                {
                    if (ShouldShow(watchVarControl))
                        Controls.Add(watchVarControl);
                });
            }
        }

        public void AddVariable(WatchVariableControl watchVarControl)
        {
            lock (_objectLock)
            {
                AddVariables(new List<WatchVariableControl>() { watchVarControl });
            }
        }

        public void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            lock (_objectLock)
            {
                foreach (WatchVariableControl watchVarControl in watchVarControls)
                {
                    _watchVarControls.Add(watchVarControl);
                    if (ShouldShow(watchVarControl)) Controls.Add(watchVarControl);
                    watchVarControl.SetPanel(this);
                }
            }
        }

        public void RemoveVariable(WatchVariableControl watchVarControl)
        {
            // No need to lock, since this calls into a method that locks
            RemoveVariables(new List<WatchVariableControl>() { watchVarControl });
        }

        public void RemoveVariables(List<WatchVariableControl> watchVarControls)
        {
            lock (_objectLock)
            {
                foreach (WatchVariableControl watchVarControl in watchVarControls)
                {
                    if (_reorderingWatchVarControls.Contains(watchVarControl))
                        _reorderingWatchVarControls.Remove(watchVarControl);

                    _watchVarControls.Remove(watchVarControl);
                    if (ShouldShow(watchVarControl)) Controls.Remove(watchVarControl);
                    watchVarControl.SetPanel(null);
                }
            }
        }

        public void RemoveVariableGroup(VariableGroup varGroup)
        {
            List<WatchVariableControl> watchVarControls =
                _watchVarControls.FindAll(
                    watchVarControl => watchVarControl.BelongsToGroup(varGroup));
            RemoveVariables(watchVarControls);
        }

        public void ShowOnlyVariableGroup(VariableGroup visibleVarGroup)
        {
            ShowOnlyVariableGroups(new List<VariableGroup>() { visibleVarGroup });
        }

        public void ShowOnlyVariableGroups(List<VariableGroup> visibleVarGroups)
        {
            foreach (VariableGroup varGroup in _allGroups)
            {
                bool newVisibility = visibleVarGroups.Contains(varGroup);
                ToggleVarGroupVisibility(varGroup, newVisibility);
            }
        }

        public void ClearVariables()
        {
            List<WatchVariableControl> watchVarControlListCopy =
                new List<WatchVariableControl>(_watchVarControls);
            RemoveVariables(watchVarControlListCopy);
        }

        public void ClearAllButHighlightedVariables()
        {
            List<WatchVariableControl> nonHighlighted =
                _watchVarControls.FindAll(control => !control.Highlighted);
            RemoveVariables(nonHighlighted);
            _watchVarControls.ForEach(control => control.Highlighted = false);
        }

        public void FixVerticalScroll()
        {
            List<WatchVariableControl> controls = GetCurrentVariableControls();
            RemoveVariables(controls);
            AddVariables(controls);
        }

        private void ResetVariables()
        {
            ClearVariables();
            _visibleGroups.Clear();
            _visibleGroups.AddRange(_initialVisibleGroups);
            UpdateFilterItemCheckedStatuses();

            List<WatchVariableControlPrecursor> precursors = _varFilePath == null
                ? new List<WatchVariableControlPrecursor>()
                : XmlConfigParser.OpenWatchVariableControlPrecursors(_varFilePath);
            AddVariables(precursors.ConvertAll(precursor => precursor.CreateWatchVariableControl()));
        }

        public void UnselectAllVariables()
        {
            _selectedWatchVarControls.ForEach(control => control.IsSelected = false);
            _selectedWatchVarControls.Clear();
            UnselectText();
        }

        public void UnselectText()
        {
            foreach (WatchVariableControl control in _watchVarControls)
            {
                control.UnselectText();
            }
        }

        public void StopEditing()
        {
            foreach (WatchVariableControl control in _watchVarControls)
            {
                control.StopEditing();
            }
        }

        private void AddAllVariablesToCustomTab()
        {
            GetCurrentVariableControls().ForEach(varControl =>
                varControl.AddToTab(Config.CustomManager));
        }

        private List<XElement> GetCurrentVarXmlElements(bool useCurrentState = true)
        {
            return GetCurrentVariableControls().ConvertAll(control => control.ToXml(useCurrentState));
        }

        public void OpenVariables()
        {
            List<XElement> elements = DialogUtilities.OpenXmlElements(FileType.StroopVariables);
            OpenVariables(elements);
        }

        public void OpenVariablesAsPopOut()
        {
            List<XElement> elements = DialogUtilities.OpenXmlElements(FileType.StroopVariables);
            if (elements.Count == 0) return;
            List<WatchVariableControlPrecursor> precursors =
                elements.ConvertAll(element => new WatchVariableControlPrecursor(element));
            List<WatchVariableControl> controls = precursors.ConvertAll(p => p.CreateWatchVariableControl());
            VariablePopOutForm form = new VariablePopOutForm();
            form.Initialize(controls);
            form.ShowForm();
        }

        public void OpenVariables(List<XElement> elements)
        {
            List<WatchVariableControlPrecursor> precursors =
                elements.ConvertAll(element => new WatchVariableControlPrecursor(element));
            AddVariables(precursors.ConvertAll(w => w.CreateWatchVariableControl()));
        }

        public void SaveVariablesInPlace()
        {
            if (_varFilePath == null) return;
            if (!DialogUtilities.AskQuestionAboutSavingVariableFileInPlace()) return;
            SaveVariables(_varFilePath);
        }

        public void SaveVariables(string fileName = null)
        {
            DialogUtilities.SaveXmlElements(
                FileType.StroopVariables, "VarData", GetCurrentVarXmlElements(), fileName);
        }

        public void NotifyOfReordering(WatchVariableControl watchVarControl)
        {
            if (_reorderingWatchVarControls.Count == 0)
            {
                NotifyOfReorderingStart(new List<WatchVariableControl>() { watchVarControl });
            }
            else
            {
                NotifyOfReorderingEnd(new List<WatchVariableControl>() { watchVarControl });
            }
        }

        public void NotifyOfReorderingStart(List<WatchVariableControl> watchVarControls)
        {
            if (watchVarControls.Count == 0) return;

            _reorderingWatchVarControls.Clear();
            _reorderingWatchVarControls.AddRange(watchVarControls);
            _reorderingWatchVarControls.ForEach(control => control.FlashColor(WatchVariableControl.REORDER_START_COLOR));
        }

        public void NotifyOfReorderingEnd(List<WatchVariableControl> watchVarControls)
        {
            if (watchVarControls.Count == 0) return;

            int newIndex = Controls.IndexOf(watchVarControls[0]);
            _reorderingWatchVarControls.ForEach(control => Controls.Remove(control));
            _reorderingWatchVarControls.ForEach(control => Controls.Add(control));
            for (int i = 0; i < _reorderingWatchVarControls.Count; i++)
            {
                Controls.SetChildIndex(_reorderingWatchVarControls[i], newIndex + i);
                _reorderingWatchVarControls[i].FlashColor(WatchVariableControl.REORDER_END_COLOR);
            }
            _reorderingWatchVarControls.Clear();
        }

        public void NotifyOfReorderingClear()
        {
            _reorderingWatchVarControls.ForEach(
                control => control.FlashColor(WatchVariableControl.REORDER_RESET_COLOR));
            _reorderingWatchVarControls.Clear();
        }

        public void NotifySelectClick(
            WatchVariableControl clickedControl, bool ctrlHeld, bool shiftHeld)
        {
            List<WatchVariableControl> currentControls = GetCurrentVariableControls();

            if (shiftHeld && _selectedWatchVarControls.Count > 0)
            {
                int index1 = currentControls.IndexOf(_selectedWatchVarControls.Last());
                int index2 = currentControls.IndexOf(clickedControl);
                int diff = Math.Abs(index2 - index1);
                int diffSign = index2 > index1 ? 1 : -1;
                for (int i = 0; i <= diff; i++)
                {
                    int index = index1 + diffSign * i;
                    WatchVariableControl control = currentControls[index];
                    if (!_selectedWatchVarControls.Contains(control))
                    {
                        control.IsSelected = true;
                        _selectedWatchVarControls.Add(control);
                    }
                }
            }
            else
            {
                bool toggle = ctrlHeld ||(_selectedWatchVarControls.Count == 1 && _selectedWatchVarControls[0] == clickedControl);
                if (!toggle) UnselectAllVariables();
                if (clickedControl.IsSelected)
                {
                    clickedControl.IsSelected = false;
                    _selectedWatchVarControls.Remove(clickedControl);
                }
                else
                {
                    clickedControl.IsSelected = true;
                    _selectedWatchVarControls.Add(clickedControl);
                }
            }
        }

        public List<WatchVariableControl> GetCurrentVariableControls()
        {
            List<WatchVariableControl> watchVarControls = new List<WatchVariableControl>();
            lock (_objectLock)
            {
                foreach (Control control in Controls)
                {
                    WatchVariableControl watchVarControl = control as WatchVariableControl;
                    watchVarControls.Add(watchVarControl);
                }
            }
            return watchVarControls;
        }

        public List<WatchVariableControlPrecursor> GetCurrentVariablePrecursors()
        {
            return GetCurrentVariableControls().ConvertAll(control => control.WatchVarPrecursor);
        }

        public List<object> GetCurrentVariableValues(bool useRounding = false, bool handleFormatting = true)
        {
            return GetCurrentVariableControls().ConvertAll(control => control.GetValue(useRounding, handleFormatting));
        }

        public List<string> GetCurrentVariableNames()
        {
            return GetCurrentVariableControls().ConvertAll(control => control.VarName);
        }

        public List<(string, object)> GetCurrentVariableNamesAndValues(bool useRounding = false, bool handleFormatting = true)
        {
            return GetCurrentVariableControls().ConvertAll(control => (control.VarName, control.GetValue(useRounding, handleFormatting)));
        }

        public bool SetVariableValueByName(string name, object value)
        {
            WatchVariableControl control = GetCurrentVariableControls().FirstOrDefault(c => c.VarName == name);
            if (control == null) return false;
            return control.SetValue(value);
        }

        public void UpdatePanel()
        {
            if (!ContainsFocus)
            {
                UnselectAllVariables();
            }
            GetCurrentVariableControls().ForEach(watchVarControl => watchVarControl.UpdateControl());
        }

        private bool ShouldShow(WatchVariableControl watchVarControl)
        {
            return watchVarControl.BelongsToAnyGroupOrHasNoGroup(_visibleGroups);
        }

        public override string ToString()
        {
            List<string> varNames = _watchVarControls.ConvertAll(control => control.VarName);
            return String.Join(",", varNames);
        }

        public void ColorVarsUsingFunction(Func<WatchVariableControl, Color> getColor)
        {
            foreach (WatchVariableControl control in _watchVarControls)
            {
                control.BaseColor = getColor(control);
            }
        }
    }
}
