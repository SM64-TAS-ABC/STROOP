using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Structs;
using STROOP.Utilities;
using System.Drawing;
using STROOP.Extensions;
using STROOP.Structs.Configurations;
using STROOP.Controls;
using STROOP.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class ObjectSlotsManager
    {
        /// <summary>
        /// The default size of the object slot UI element
        /// </summary>
        public static readonly int DefaultSlotSize = 36;

        public enum TabType { Object, Map, Model, Memory, Custom, Warp, TAS, CamHack, Other };
        public enum SortMethodType { ProcessingOrder, MemoryOrder, DistanceToMario };
        public enum SlotLabelType { Recommended, SlotPosVs, SlotPos, SlotIndex };
        public enum SelectionMethodType { Clicked, Held, StoodOn, Interaction, Used, Floor, Wall, Ceiling, Closest };
        public enum ClickType { ObjectClick, MapClick, ModelClick, MemoryClick, CamHackClick, MarkClick };

        public uint? HoveredObjectAddress;

        public List<ObjectSlot> ObjectSlots;

        ObjectSlotManagerGui _gui;

        Dictionary<uint, Tuple<int?, int?>> _lockedSlotIndices = new Dictionary<uint, Tuple<int?, int?>>();
        public bool LabelsLocked = false;

        public readonly List<uint> SelectedSlotsAddresses = new List<uint>();
        public readonly List<uint> SelectedOnMapSlotsAddresses = new List<uint>();

        public readonly List<uint> MarkedSlotsAddresses = new List<uint>();
        public readonly Dictionary<uint, int> MarkedSlotsAddressesDictionary = new Dictionary<uint, int>();

        public List<ObjectDataModel> SelectedObjects = new List<ObjectDataModel>();

        private Dictionary<ObjectDataModel, string> _slotLabels = new Dictionary<ObjectDataModel, string>();
        public IReadOnlyDictionary<ObjectDataModel, string> SlotLabelsForObjects { get; private set; }

        public TabType ActiveTab;
        public SortMethodType SortMethod = SortMethodType.ProcessingOrder;
        public SlotLabelType LabelMethod = SlotLabelType.Recommended;

        public ObjectSlotsManager(ObjectSlotManagerGui gui, TabControl tabControlMain)
        {
            _gui = gui;

            // Add SortMethods adn LabelMethods
            _gui.SortMethodComboBox.DataSource = Enum.GetValues(typeof(SortMethodType));
            _gui.LabelMethodComboBox.DataSource = Enum.GetValues(typeof(SlotLabelType));
            _gui.SelectionMethodComboBox.DataSource = Enum.GetValues(typeof(SelectionMethodType));

            _gui.TabControl.Selected += TabControl_Selected;
            TabControl_Selected(this, new TabControlEventArgs(_gui.TabControl.SelectedTab, -1, TabControlAction.Selected));

            // Create and setup object slots
            ObjectSlots = new List<ObjectSlot>();
            foreach (int i in Enumerable.Range(0, ObjectSlotsConfig.MaxSlots))
            {
                var objectSlot = new ObjectSlot(this, i, _gui, new Size(DefaultSlotSize, DefaultSlotSize));
                objectSlot.Click += (sender, e) => OnSlotClick(sender, e);
                ObjectSlots.Add(objectSlot);
                _gui.FlowLayoutContainer.Controls.Add(objectSlot);
            };

            SlotLabelsForObjects = new ReadOnlyDictionary<ObjectDataModel, string>(_slotLabels);
        }

        public void ChangeSlotSize(int newSize)
        {
            foreach (var objSlot in ObjectSlots)
                objSlot.Size = new Size(newSize, newSize);
        }

        private static readonly Dictionary<string, TabType> TabNameToTabType = new Dictionary<string, TabType>()
        {
            ["Object"] = TabType.Object,
            ["Map"] = TabType.Map,
            ["Model"] = TabType.Model,
            ["Memory"] = TabType.Memory,
            ["Custom"] = TabType.Custom,
            ["Warp"] = TabType.Warp,
            ["TAS"] = TabType.TAS,
            ["Cam Hack"] = TabType.CamHack,
        };
        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            TabType tabType = TabType.Other;
            if (e.TabPage != null && TabNameToTabType.ContainsKey(e.TabPage.Text)) {
                tabType = TabNameToTabType[e.TabPage.Text];
            }
            ActiveTab = tabType;
        }

        private void OnSlotClick(object sender, EventArgs e)
        {
            // Make sure the tab has loaded
            if (_gui.TabControl.SelectedTab == null)
                return;

            ObjectSlot selectedSlot = sender as ObjectSlot;
            selectedSlot.Focus();

            bool isCtrlKeyHeld = KeyboardUtilities.IsCtrlHeld();
            bool isShiftKeyHeld = KeyboardUtilities.IsShiftHeld();
            bool isAltKeyHeld = KeyboardUtilities.IsAltHeld();
            int? numberHeld = KeyboardUtilities.GetCurrentlyInputtedNumber();

            DoSlotClickUsingInput(selectedSlot, isCtrlKeyHeld, isShiftKeyHeld, isAltKeyHeld, numberHeld);
        }

        private void DoSlotClickUsingInput(
            ObjectSlot selectedSlot, bool isCtrlKeyHeld, bool isShiftKeyHeld, bool isAltKeyHeld, int? numberHeld)
        {
            bool isMarking = isAltKeyHeld || numberHeld.HasValue;
            int? markedColor = isAltKeyHeld ? 10 : numberHeld;
            ClickType click = GetClickType(isMarking);
            bool shouldToggle = ShouldToggle(isCtrlKeyHeld, isMarking);
            bool shouldExtendRange = isShiftKeyHeld;
            TabPage tabDestination = GetTabDestination(isMarking);
            DoSlotClickUsingSpecifications(selectedSlot, click, shouldToggle, shouldExtendRange, tabDestination, markedColor);
        }

        public void SelectSlotByAddress(uint address)
        {
            ObjectSlot slot = ObjectSlots.FirstOrDefault(s => s.CurrentObject.Address == address);
            if (slot != null) DoSlotClickUsingInput(slot, false, false, false, null);
        }

        private ClickType GetClickType(bool isMarking)
        {
            if (isMarking)
            {
                return ClickType.MarkClick;
            }
            else
            {
                switch (ActiveTab)
                {
                    case TabType.CamHack:
                        return ClickType.CamHackClick;
                    case TabType.Map:
                        return ClickType.MapClick;
                    case TabType.Model:
                        return ClickType.ModelClick;
                    case TabType.Memory:
                        return ClickType.MemoryClick;
                    case TabType.Object:
                    case TabType.Custom:
                    case TabType.Warp:
                    case TabType.TAS:
                    case TabType.Other:
                        return ClickType.ObjectClick;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool ShouldToggle(bool isCtrlKeyHeld, bool isMarking)
        {
            bool isTogglingTab =
                ActiveTab == TabType.Map ||
                ActiveTab == TabType.CamHack;
            bool isToggleState = isMarking ? true : isTogglingTab;
            return isToggleState != isCtrlKeyHeld;
        }

        private TabPage GetTabDestination(bool isMarking)
        {
            if (isMarking) return null;
            if (ActiveTab == TabType.Other) return Config.ObjectManager.Tab;
            if (ActiveTab == TabType.TAS && !SpecialConfig.IsSelectedPA) return Config.ObjectManager.Tab;
            return null;
        }

        public void DoSlotClickUsingSpecifications(
            ObjectSlot selectedSlot, ClickType click, bool shouldToggle, bool shouldExtendRange, TabPage tabDestination, int? markedColor)
        {
            if (selectedSlot.CurrentObject == null)
                return;

            if (click == ClickType.ObjectClick)
            {
                _gui.SelectionMethodComboBox.SelectedItem = SelectionMethodType.Clicked;
            }

            if (click == ClickType.ModelClick)
            {
                uint currentModelObjectAddress = Config.ModelManager.ModelObjectAddress;
                uint newModelObjectAddress = currentModelObjectAddress == selectedSlot.CurrentObject.Address ? 0 
                    : selectedSlot.CurrentObject.Address;
                Config.ModelManager.ModelObjectAddress = newModelObjectAddress;
                Config.ModelManager.ManualMode = false;
            }
            else if (click == ClickType.CamHackClick)
            {
                uint currentCamHackSlot = Config.Stream.GetUInt32(CamHackConfig.StructAddress + CamHackConfig.ObjectOffset);
                uint newCamHackSlot = currentCamHackSlot == selectedSlot.CurrentObject.Address ? 0 
                    : selectedSlot.CurrentObject.Address;
                Config.Stream.SetValue(newCamHackSlot, CamHackConfig.StructAddress + CamHackConfig.ObjectOffset);
            }
            else
            {
                List<uint> selection;
                switch (click)
                {
                    case ClickType.ObjectClick:
                    case ClickType.MemoryClick:
                        selection = SelectedSlotsAddresses;
                        break;
                    case ClickType.MapClick:
                        selection = SelectedOnMapSlotsAddresses;
                        break;
                    case ClickType.MarkClick:
                        selection = MarkedSlotsAddresses;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (tabDestination != null)
                {
                    List<TabPage> tabPages = ControlUtilities.GetTabPages(Config.TabControlMain);
                    bool containsTab = tabPages.Any(tabPage => tabPage == tabDestination);
                    if (containsTab) Config.TabControlMain.SelectTab(tabDestination);
                }

                if (shouldExtendRange && selection.Count > 0)
                {
                    int? startRange = ObjectSlots.FirstOrDefault(s => s.CurrentObject.Address == selection.Last())?.Index;
                    int endRange = selectedSlot.Index;

                    if (!startRange.HasValue)
                        return;

                    int rangeSize = Math.Abs(endRange - startRange.Value);
                    int iteratorDirection = endRange > startRange ? 1 : -1;

                    for (int i = 0; i <= rangeSize; i++)
                    {
                        int index = startRange.Value + i * iteratorDirection;
                        uint address = ObjectSlots[index].CurrentObject.Address;
                        if (!selection.Contains(address))
                            selection.Add(address);
                    }
                }
                else
                {
                    if (!shouldToggle)
                    {
                        selection.Clear();
                        if (selection == MarkedSlotsAddresses)
                        {
                            MarkedSlotsAddressesDictionary.Clear();
                        }
                    }

                    if (selection.Contains(selectedSlot.CurrentObject.Address))
                    {
                        selection.Remove(selectedSlot.CurrentObject.Address);
                        if (selection == MarkedSlotsAddresses)
                        {
                            MarkedSlotsAddressesDictionary.Remove(selectedSlot.CurrentObject.Address);
                        }
                    }
                    else
                    {
                        selection.Add(selectedSlot.CurrentObject.Address);
                        if (selection == MarkedSlotsAddresses)
                        {
                            MarkedSlotsAddressesDictionary[selectedSlot.CurrentObject.Address] = markedColor.Value;
                        }
                    }
                }
            }

            if (click == ClickType.MemoryClick)
            {
                Config.MemoryManager.SetObjectAddress(selectedSlot.CurrentObject?.Address);
                Config.MemoryManager.UpdateHexDisplay();
            }
        }

        public void Update()
        {
            UpdateSelectionMethod();

            LabelMethod = (SlotLabelType)_gui.LabelMethodComboBox.SelectedItem;
            SortMethod = (SortMethodType) _gui.SortMethodComboBox.SelectedItem;

            // Lock label update
            LabelsLocked = _gui.LockLabelsCheckbox.Checked;

            // Processing sort order
            IEnumerable<ObjectDataModel> sortedObjects;
            switch (SortMethod)
            {
                case SortMethodType.ProcessingOrder:
                    // Data is already sorted by processing order
                    sortedObjects = DataModels.Objects.OrderBy(o => o?.ProcessIndex);
                    break;

                case SortMethodType.MemoryOrder:
                    // Order by address
                    sortedObjects = DataModels.Objects.OrderBy(o => o?.Address);
                    break;

                case SortMethodType.DistanceToMario:

                    // Order by address
                    var activeObjects = DataModels.Objects.Where(o => o?.IsActive ?? false).OrderBy(o => o?.DistanceToMarioCalculated);
                    var inActiveObjects = DataModels.Objects.Where(o => !o?.IsActive ?? true).OrderBy(o => o?.DistanceToMarioCalculated);

                    sortedObjects = activeObjects.Concat(inActiveObjects);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Uknown sort method type");
            }

            // Update slots
            UpdateSlots(sortedObjects);

            List<ObjectDataModel> objs = DataModels.Objects.Where(o => o != null && SelectedSlotsAddresses.Contains(o.Address)).ToList();
            objs.Sort((obj1, obj2) => SelectedSlotsAddresses.IndexOf(obj1.Address) - SelectedSlotsAddresses.IndexOf(obj2.Address));
            SelectedObjects = objs;
        }

        private void UpdateSelectionMethod()
        {
            SelectionMethodType selectionMethodType = (SelectionMethodType)_gui.SelectionMethodComboBox.SelectedItem;
            switch (selectionMethodType)
            {
                case SelectionMethodType.Clicked:
                    // do nothing
                    break;
                case SelectionMethodType.Held:
                    SelectedSlotsAddresses.Clear();
                    uint heldObjectAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.HeldObjectPointerOffset);
                    if (heldObjectAddress != 0) SelectedSlotsAddresses.Add(heldObjectAddress);
                    break;
                case SelectionMethodType.StoodOn:
                    SelectedSlotsAddresses.Clear();
                    uint stoodOnObjectAddress = Config.Stream.GetUInt32(MarioConfig.StoodOnObjectPointerAddress);
                    if (stoodOnObjectAddress != 0) SelectedSlotsAddresses.Add(stoodOnObjectAddress);
                    break;
                case SelectionMethodType.Interaction:
                    SelectedSlotsAddresses.Clear();
                    uint interactionObjectAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.InteractionObjectPointerOffset);
                    if (interactionObjectAddress != 0) SelectedSlotsAddresses.Add(interactionObjectAddress);
                    break;
                case SelectionMethodType.Used:
                    SelectedSlotsAddresses.Clear();
                    uint usedObjectAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.UsedObjectPointerOffset);
                    if (usedObjectAddress != 0) SelectedSlotsAddresses.Add(usedObjectAddress);
                    break;
                case SelectionMethodType.Floor:
                    SelectedSlotsAddresses.Clear();
                    uint floorTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                    if (floorTriangleAddress == 0) break;
                    uint floorObjectAddress = Config.Stream.GetUInt32(floorTriangleAddress + TriangleOffsetsConfig.AssociatedObject);
                    if (floorObjectAddress != 0) SelectedSlotsAddresses.Add(floorObjectAddress);
                    break;
                case SelectionMethodType.Wall:
                    SelectedSlotsAddresses.Clear();
                    uint wallTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
                    if (wallTriangleAddress == 0) break;
                    uint wallObjectAddress = Config.Stream.GetUInt32(wallTriangleAddress + TriangleOffsetsConfig.AssociatedObject);
                    if (wallObjectAddress != 0) SelectedSlotsAddresses.Add(wallObjectAddress);
                    break;
                case SelectionMethodType.Ceiling:
                    SelectedSlotsAddresses.Clear();
                    uint ceilingTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
                    if (ceilingTriangleAddress == 0) break;
                    uint ceilingObjectAddress = Config.Stream.GetUInt32(ceilingTriangleAddress + TriangleOffsetsConfig.AssociatedObject);
                    if (ceilingObjectAddress != 0) SelectedSlotsAddresses.Add(ceilingObjectAddress);
                    break;
                case SelectionMethodType.Closest:
                    SelectedSlotsAddresses.Clear();
                    SelectedSlotsAddresses.Add(DataModels.Mario.ClosestObject);
                    break;
            }
        }

        private void UpdateSlots(IEnumerable<ObjectDataModel> sortedObjects)
        {
            // Update labels
            if (!LabelsLocked)
            {
                _lockedSlotIndices.Clear();
                foreach (ObjectDataModel obj in DataModels.Objects.Where(o => o != null))
                    _lockedSlotIndices[obj.Address] = new Tuple<int?, int?>(obj.ProcessIndex, obj.VacantSlotIndex);
            }
            _slotLabels.Clear();
            foreach (ObjectDataModel obj in sortedObjects.Where(o => o != null))
                _slotLabels[obj] = GetSlotLabelFromObject(obj);

            // Update object slots
            foreach (var item in sortedObjects.Zip(ObjectSlots, (o, s) => new { Slot = s, Obj = o }))
                item.Slot.Update(item.Obj);
        }

        public List<ObjectDataModel> GetLoadedObjectsWithName(string name)
        {
            if (name == null) return new List<ObjectDataModel>();

            return DataModels.Objects.Where(o => o != null && o.IsActive
                && o.BehaviorAssociation?.Name?.ToLower() == name.ToLower()).ToList();
        }

        public List<ObjectDataModel> GetLoadedObjectsWithPredicate(Func<ObjectDataModel, bool> func)
        {
            return DataModels.Objects.Where(o => o != null && o.IsActive && func(o)).ToList();
        }

        public ObjectDataModel GetObjectFromLabel(string name)
        {
            if (name == null) return null;
            name = name.ToLower().Trim();
            ObjectSlot slot = ObjectSlots.FirstOrDefault(s => s.Text.ToLower() == name);
            return slot?.CurrentObject;
        }

        public int? GetSlotIndexFromObj(ObjectDataModel obj)
        {
            return ObjectSlots.FirstOrDefault(o => o.CurrentObject?.Equals(obj) ?? false)?.Index;
        }

        public ObjectDataModel GetObjectFromAddress(uint objAddress)
        {
            return DataModels.Objects.FirstOrDefault(o => o?.Address == objAddress);
        }

        /*
         * Returns a string that's either:
         * - the slot label if a slot has the address
         * - null if no slot has the address
         */
        public string GetSlotLabelFromAddress(uint objAddress)
        {
            ObjectDataModel obj = GetObjectFromAddress(objAddress);
            return Config.ObjectSlotsManager.GetSlotLabelFromObject(obj);
        }

        public string GetDescriptiveSlotLabelFromAddress(uint objAddress, bool concise)
        {
            string noObjectString = concise ? ".." : "(no object)";
            string unusedObjectString = concise ? "UU" : "(unused object)";
            string unknownObjectString = concise ? ".." : "(unknown object)";
            string slotLabelPrefix = concise ? "" : "Slot ";
            string processGroupPrefix = concise ? "PG" : "PG ";

            if (objAddress == 0) return noObjectString;
            if (objAddress == ObjectSlotsConfig.UnusedSlotAddress) return unusedObjectString;

            byte? processGroup = ObjectUtilities.GetProcessGroup(objAddress);
            if (processGroup.HasValue) return processGroupPrefix + HexUtilities.FormatValue(processGroup.Value, 1, false);

            string slotLabel = GetSlotLabelFromAddress(objAddress);
            if (slotLabel == null) return unknownObjectString;
            return slotLabelPrefix + slotLabel;
        }

        public string GetSlotLabelFromObject(ObjectDataModel obj)
        {
            if (obj == null) return null;
            switch (LabelMethod)
            {
                case SlotLabelType.Recommended:
                    if (SortMethod == SortMethodType.MemoryOrder)
                        goto case SlotLabelType.SlotIndex;
                    else
                        goto case SlotLabelType.SlotPosVs;

                case SlotLabelType.SlotIndex:
                    return String.Format("{0}", (obj.Address - ObjectSlotsConfig.ObjectSlotsStartAddress)
                        / ObjectConfig.StructSize + (SavedSettingsConfig.StartSlotIndexsFromOne ? 1 : 0));

                case SlotLabelType.SlotPos:
                    return String.Format("{0}", _lockedSlotIndices[obj.Address].Item1
                        + (SavedSettingsConfig.StartSlotIndexsFromOne ? 1 : 0));

                case SlotLabelType.SlotPosVs:
                    var vacantSlotIndex = _lockedSlotIndices[obj.Address].Item2;
                    if (!vacantSlotIndex.HasValue)
                        goto case SlotLabelType.SlotPos;

                    return String.Format("VS{0}", vacantSlotIndex.Value
                        + (SavedSettingsConfig.StartSlotIndexsFromOne ? 1 : 0));

                default:
                    return "";
            }
        }
    }
}
 