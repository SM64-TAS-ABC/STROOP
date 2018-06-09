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

namespace STROOP.Managers
{
    public class ObjectSlotsManager
    {
        /// <summary>
        /// The default size of the object slot UI element
        /// </summary>
        const int DefaultSlotSize = 36;

        public enum TabType { Object, Map, Model, Memory, Custom, CamHack, Other };
        public enum TabDestinationType { Object, Memory };
        public enum SortMethodType { ProcessingOrder, MemoryOrder, DistanceToMario };
        public enum SlotLabelType { Recommended, SlotPosVs, SlotPos, SlotIndex }
        public enum ClickType { ObjectClick, MapClick, ModelClick, MemoryClick, CamHackClick, MarkClick };

        public uint? HoveredObjectAdress;

        public List<ObjectSlot> ObjectSlots;

        ObjectSlotManagerGui _gui;

        Dictionary<ObjectDataModel, Tuple<int?, int?>> _lockedSlotIndices = new Dictionary<ObjectDataModel, Tuple<int?, int?>>();
        public bool LabelsLocked = false;

        uint? _lastSelectedAddress = null;
        public HashSet<uint> SelectedSlotsAddresses = new HashSet<uint>();
        public HashSet<uint> SelectedOnMapSlotsAddresses = new HashSet<uint>();
        public HashSet<uint> MarkedSlotsAddresses = new HashSet<uint>();

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
            ["Cam Hack"] = TabType.CamHack,
        };
        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            TabType tabType;
            if (!TabNameToTabType.TryGetValue(e.TabPage.Text, out tabType))
                tabType = TabType.Other;
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

            DoSlotClickUsingInput(selectedSlot, isCtrlKeyHeld, isShiftKeyHeld, isAltKeyHeld);
        }

        private void DoSlotClickUsingInput(
            ObjectSlot selectedSlot, bool isCtrlKeyHeld, bool isShiftKeyHeld, bool isAltKeyHeld)
        {
            ClickType click = GetClickType(isAltKeyHeld);
            bool shouldToggle = ShouldToggle(isCtrlKeyHeld, isAltKeyHeld);
            bool shouldExtendRange = isShiftKeyHeld;
            TabDestinationType? tabDestination = GetTabDestination(isAltKeyHeld);
            DoSlotClickUsingSpecifications(selectedSlot, click, shouldToggle, shouldExtendRange, tabDestination);
        }

        public void SelectSlotByAddress(uint address)
        {
            ObjectSlot slot = ObjectSlots.FirstOrDefault(s => s.CurrentObject.Address == address);
            if (slot != null) DoSlotClickUsingInput(slot, false, false, false);
        }

        private ClickType GetClickType(bool isAltKeyHeld)
        {
            if (isAltKeyHeld)
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
                    case TabType.Other:
                        return ClickType.ObjectClick;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private bool ShouldToggle(bool isCtrlKeyHeld, bool isAltKeyHeld)
        {
            bool isTogglingTab = ActiveTab == TabType.Map || ActiveTab == TabType.CamHack;
            bool isToggleState = isAltKeyHeld ? true : isTogglingTab;
            return isToggleState != isCtrlKeyHeld;
        }

        private TabDestinationType? GetTabDestination(bool isAltKeyHeld)
        {
            if (isAltKeyHeld) return null;
            if (ActiveTab == TabType.Other) return TabDestinationType.Object;
            return null;
        }

        public void DoSlotClickUsingSpecifications(
            ObjectSlot selectedSlot, ClickType click, bool shouldToggle, bool shouldExtendRange, TabDestinationType? tabDestination)
        {
            if (selectedSlot.CurrentObject == null)
                return;

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
                uint currentCamHackSlot = Config.Stream.GetUInt32(CameraHackConfig.CameraHackStructAddress + CameraHackConfig.ObjectOffset);
                uint newCamHackSlot = currentCamHackSlot == selectedSlot.CurrentObject.Address ? 0 
                    : selectedSlot.CurrentObject.Address;
                Config.Stream.SetValue(newCamHackSlot, CameraHackConfig.CameraHackStructAddress + CameraHackConfig.ObjectOffset);
            }
            else
            {
                HashSet<uint> selection;
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

                if (tabDestination.HasValue)
                {
                    switch (tabDestination.Value)
                    {
                        case TabDestinationType.Object:
                            _gui.TabControl.SelectedTab = _gui.TabControl.TabPages["tabPageObject"];
                            break;
                        case TabDestinationType.Memory:
                            _gui.TabControl.SelectedTab = _gui.TabControl.TabPages["tabPageMemory"];
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (shouldExtendRange && _lastSelectedAddress.HasValue)
                {
                    int? startRange = ObjectSlots.FirstOrDefault(s => s.CurrentObject.Address == _lastSelectedAddress)?.Index;
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
                    _lastSelectedAddress = selectedSlot.CurrentObject.Address;
                }
                else
                {
                    if (!shouldToggle)
                        selection.Clear();

                    if (selection.Contains(selectedSlot.CurrentObject.Address))
                    {
                        selection.Remove(selectedSlot.CurrentObject.Address);
                        _lastSelectedAddress = null;
                    }
                    else
                    {
                        selection.Add(selectedSlot.CurrentObject.Address);
                        _lastSelectedAddress = selectedSlot.CurrentObject.Address;
                    }
                }
            }

            if (click == ClickType.MemoryClick)
            {
                Config.MemoryManager.UpdateDisplay();
            }
        }

        public void Update()
        {
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

            SelectedObjects = DataModels.Objects.Where(o => o != null && SelectedSlotsAddresses.Contains(o.Address)).ToList();
        }

        private void UpdateSlots(IEnumerable<ObjectDataModel> sortedObjects)
        {
            // Update labels
            if (!LabelsLocked)
            {
                _lockedSlotIndices.Clear();
                foreach (ObjectDataModel obj in DataModels.Objects.Where(o => o != null))
                    _lockedSlotIndices[obj] = new Tuple<int?, int?>(obj.ProcessIndex, obj.VacantSlotIndex);
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
                && o.BehaviorAssociation?.Name == name).ToList();
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

            if (objAddress == 0) return noObjectString;
            if (objAddress == ObjectSlotsConfig.UnusedSlotAddress) return unusedObjectString;
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
                    return String.Format("{0}", (obj.Address - ObjectSlotsConfig.LinkStartAddress)
                        / ObjectConfig.StructSize + (SavedSettingsConfig.SlotIndexsFromOne ? 1 : 0));

                case SlotLabelType.SlotPos:
                    return String.Format("{0}", _lockedSlotIndices[obj].Item1
                        + (SavedSettingsConfig.SlotIndexsFromOne ? 1 : 0));

                case SlotLabelType.SlotPosVs:
                    var vacantSlotIndex = _lockedSlotIndices[obj].Item2;
                    if (!vacantSlotIndex.HasValue)
                        goto case SlotLabelType.SlotPos;

                    return String.Format("VS{0}", vacantSlotIndex.Value
                        + (SavedSettingsConfig.SlotIndexsFromOne ? 1 : 0));

                default:
                    return "";
            }
        }
    }
}
 