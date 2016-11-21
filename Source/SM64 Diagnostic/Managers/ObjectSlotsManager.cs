using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System.Drawing;
using SM64_Diagnostic.Extensions;
using OpenTK.Input;

namespace SM64_Diagnostic.ManagerClasses
{
    public class ObjectSlotsManager
    {
        const int DefaultSlotSize = 36;
        public ObjectSlot[] ObjectSlots;

        public ObjectAssociations ObjectAssoc;
        ObjectManager _objManager;
        MapManager _mapManager;
        MiscManager _miscManager;
        ProcessStream _stream;
        public ObjectSlotManagerGui ManagerGui;

        public const byte VacantGroup = 0xFF;

        Dictionary<uint, MapObject> _mapObjects = new Dictionary<uint, MapObject>();
        Dictionary<uint, int> _memoryAddressSlotIndex;
        Dictionary<uint, string> _lastSlotLabel = new Dictionary<uint, string>();
        List<uint> _selectedSlotsAddresses = new List<uint>();

        List<byte> _toggleMapGroups = new List<byte>();
        List<BehaviorCriteria> _toggleMapBehaviors = new List<BehaviorCriteria>();
        List<uint> _toggleMapSlots = new List<uint>();

        BehaviorCriteria? _lastSelectedBehavior;
        uint _standingOnObject, _interactingObject, _holdingObject, _usingObject, _closestObject;
        int _activeObjCnt;
        bool _selectedUpdated = false;
        Image _multiImage = null;

        List<BehaviorCriteria> _prevSelectedBehaviorCriteria = new List<BehaviorCriteria>();

        public enum SortMethodType {ProcessingOrder, MemoryOrder, DistanceToMario};
        public enum MapToggleModeType {Single, ObjectType, ProcessGroup};
        public enum SlotLabelType {Recommended, SlotPosVs, SlotPos, SlotIndex}

        public void ChangeSlotSize(int newSize)
        {
            foreach (var objSlot in ObjectSlots)
                objSlot.Size = new Size(newSize, newSize);
        }

        public ObjectSlotsManager(ProcessStream stream, ObjectAssociations objAssoc, 
            ObjectManager objManager, ObjectSlotManagerGui managerGui, MapManager mapManager, MiscManager miscManager)
        {
            ObjectAssoc = objAssoc;
            _stream = stream;
            _objManager = objManager;
            ManagerGui = managerGui;
            _mapManager = mapManager;
            _miscManager = miscManager;

            // Add MapToggleModes
            ManagerGui.MapObjectToggleModeComboBox.DataSource = Enum.GetValues(typeof(MapToggleModeType));
            ManagerGui.MapObjectToggleModeComboBox.SelectedItem = MapToggleModeType.Single;

            // Add SortMethods
            ManagerGui.SortMethodComboBox.DataSource = Enum.GetValues(typeof(ObjectSlotsManager.SortMethodType));
            ManagerGui.SortMethodComboBox.SelectedItem = SortMethodType.ProcessingOrder;

            // Add LabelMethods
            ManagerGui.LabelMethodComboBox.DataSource = Enum.GetValues(typeof(ObjectSlotsManager.SlotLabelType));
            ManagerGui.LabelMethodComboBox.SelectedItem = SlotLabelType.Recommended;

            // Create and setup object slots
            ObjectSlots = new ObjectSlot[Config.ObjectSlots.MaxSlots];
            for (int i = 0; i < Config.ObjectSlots.MaxSlots; i++)
            {
                var objectSlot = new ObjectSlot(i, this, ManagerGui, new Size(DefaultSlotSize, DefaultSlotSize));
                ObjectSlots[i] = objectSlot;
                objectSlot.Click += (sender, e) => OnClick(sender, e);
                ManagerGui.FlowLayoutContainer.Controls.Add(objectSlot);
            }

            // Change default
            ChangeSlotSize(DefaultSlotSize);
        }

        private void OnClick(object sender, EventArgs e)
        {
            // Make sure the tab has loaded
            if (ManagerGui.TabControl.SelectedTab == null)
                return;

            var selectedSlot = sender as ObjectSlot;

            // Parse behavior based on tab opened
            switch (ManagerGui.TabControl.SelectedTab.Text)
            {
                default:
                    var keyboardState = Keyboard.GetState(0);
                    ManagerGui.TabControl.SelectedTab = ManagerGui.TabControl.TabPages["tabPageObjects"];
                    if (keyboardState.IsKeyDown(Key.ShiftLeft) || keyboardState.IsKeyDown(Key.ShiftRight)
                        && _selectedSlotsAddresses.Count > 0)
                    {
                        int minSelect = _selectedSlotsAddresses.Min(s => ObjectSlots.First(o => o.Address == s).Index);
                        int maxSelect = _selectedSlotsAddresses.Max(s => ObjectSlots.First(o => o.Address == s).Index);
                        int startRange = Math.Min(minSelect, selectedSlot.Index);
                        int endRange = Math.Max(maxSelect, selectedSlot.Index);
                        var selectedObjects = ObjectSlots.Where(o => o.Index >= startRange && o.Index <= endRange
                            && !_selectedSlotsAddresses.Contains(o.Address));
                        _selectedSlotsAddresses.AddRange(selectedObjects.Select(o => o.Address));
                    }
                    else
                    {
                        if (!keyboardState.IsKeyDown(Key.ControlLeft) && !keyboardState.IsKeyDown(Key.ControlRight))
                        {
                            _selectedSlotsAddresses.Clear();
                        }
                        if (_selectedSlotsAddresses.Contains(selectedSlot.Address))
                        {
                            if (_selectedSlotsAddresses.Count > 1)
                            {
                                _selectedSlotsAddresses.Remove(selectedSlot.Address);
                            }
                        }
                        else
                        {
                            _selectedSlotsAddresses.Add(selectedSlot.Address);
                        }

                    }

                    _objManager.CurrentAddresses = _selectedSlotsAddresses;
                    break;

                case "Map":
                    switch ((MapToggleModeType)ManagerGui.MapObjectToggleModeComboBox.SelectedItem)
                    {
                        case MapToggleModeType.Single:
                            if (_toggleMapSlots.Contains(selectedSlot.Address))
                                _toggleMapSlots.Remove(selectedSlot.Address);
                            else
                                _toggleMapSlots.Add(selectedSlot.Address);

                            UpdateSelectedObjectSlots();

                            break;
                        case MapToggleModeType.ObjectType:
                            var behavior = selectedSlot.Behavior;
                            if (_toggleMapBehaviors.Contains(behavior))
                                _toggleMapBehaviors.Remove(behavior);
                            else
                                _toggleMapBehaviors.Add(behavior);

                            UpdateSelectedObjectSlots();
                            break;
                        case MapToggleModeType.ProcessGroup:
                            var group = selectedSlot.ProcessGroup;
                            if (_toggleMapGroups.Contains(group))
                                _toggleMapGroups.Remove(group);
                            else
                                _toggleMapGroups.Add(group);

                            UpdateSelectedObjectSlots();
                            break;
                    }
                    break;
            }
        }

        public void SetAllSelectedObjectSlots()
        {
            foreach (var objSlot in ObjectSlots)
            {
                objSlot.Selected = true;
            }
        }

        public void UpdateSelectedObjectSlots()
        {
            foreach (var objSlot in ObjectSlots)
            {
                bool selected = !_toggleMapGroups.Contains(objSlot.ProcessGroup)
                    && !_toggleMapBehaviors.Contains(objSlot.Behavior)
                    && !_toggleMapSlots.Contains(objSlot.Address);

                objSlot.Selected = selected;
            }
        }

        private List<ObjectSlotData> GetProcessedObjects(ObjectGroupsConfig groupConfig, ObjectSlotsConfig slotConfig)
        {
            var newObjectSlotData = new ObjectSlotData[slotConfig.MaxSlots];

            // Loop through each processing group
            int currentSlot = 0;
            foreach (var objectProcessGroup in groupConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = groupConfig.FirstGroupingAddress + objectProcessGroup * groupConfig.ProcessGroupStructSize;

                // Calculate start and ending objects
                uint currentGroupObject = BitConverter.ToUInt32(_stream.ReadRam(processGroupStructAddress
                    + groupConfig.ProcessNextLinkOffset, 4), 0);

                // Make sure there are objects within the group
                if (currentGroupObject == processGroupStructAddress)
                    continue;

                // Loop through every object within the group
                while ((currentGroupObject != processGroupStructAddress && currentSlot < slotConfig.MaxSlots))
                {

                    // Validate current object
                    if (BitConverter.ToUInt16(_stream.ReadRam(currentGroupObject + Config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
                        return null;

                    // Get data
                    newObjectSlotData[currentSlot] = new ObjectSlotData()
                    {
                        Address = currentGroupObject,
                        ObjectProcessGroup = objectProcessGroup,
                        ProcessIndex = currentSlot,
                        VacantSlotIndex = null
                    };

                    // Move to next object
                    currentGroupObject = BitConverter.ToUInt32(
                    _stream.ReadRam(currentGroupObject + groupConfig.ProcessNextLinkOffset, 4), 0);

                    // Mark next slot
                    currentSlot++;
                }
            }

            var vacantSlotStart = currentSlot;

            // Now calculate vacant addresses
            uint currentObject = BitConverter.ToUInt32(_stream.ReadRam(groupConfig.VactantPointerAddress, 4), 0);
            for (; currentSlot < slotConfig.MaxSlots; currentSlot++)
            {
                // Validate current object
                if (BitConverter.ToUInt16(_stream.ReadRam(currentObject + Config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
                    return null;

                newObjectSlotData[currentSlot] = new ObjectSlotData()
                {
                    Address = currentObject,
                    ObjectProcessGroup = VacantGroup,
                    ProcessIndex = currentSlot,
                    VacantSlotIndex = currentSlot - vacantSlotStart
                };

                currentObject = BitConverter.ToUInt32(
                    _stream.ReadRam(currentObject + groupConfig.ProcessNextLinkOffset, 4), 0);
            }

            return newObjectSlotData.ToList();
        }

        public void Update()
        {
            var groupConfig = Config.ObjectGroups;
            var slotConfig = Config.ObjectSlots;

            var newObjectSlotData = GetProcessedObjects(groupConfig, slotConfig);

            if (newObjectSlotData == null)
                return;

            // Create Memory-SlotIndex lookup table
            if (_memoryAddressSlotIndex == null)
            {
                _memoryAddressSlotIndex = new Dictionary<uint, int>();
                var sortedList = newObjectSlotData.OrderBy((objData) => objData.Address).ToList();
                foreach (var objectData in newObjectSlotData)
                {
                    _memoryAddressSlotIndex.Add(objectData.Address,
                        sortedList.FindIndex((objData) => objectData.Address == objData.Address));
                }
            }

            // Get mario position
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(_stream.ReadRam(Config.Mario.StructAddress + Config.Mario.ZOffset, 4), 0);

            // Calculate distance to Mario
            foreach(var objSlot in newObjectSlotData)
            { 
                // Get object relative-to-maario position
                float dX, dY, dZ;
                dX = marioX - BitConverter.ToSingle(_stream.ReadRam(objSlot.Address + Config.ObjectSlots.ObjectXOffset, 4), 0);
                dY = marioY - BitConverter.ToSingle(_stream.ReadRam(objSlot.Address + Config.ObjectSlots.ObjectYOffset, 4), 0);
                dZ = marioZ - BitConverter.ToSingle(_stream.ReadRam(objSlot.Address + Config.ObjectSlots.ObjectZOffset, 4), 0);

                objSlot.DistanceToMario = (float) Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                // Check if active/loaded
                objSlot.IsActive = BitConverter.ToUInt16(_stream.ReadRam(objSlot.Address + Config.ObjectSlots.ObjectActiveOffset, 2), 0) != 0x0000;

                objSlot.Behavior = BitConverter.ToUInt32(_stream.ReadRam(objSlot.Address + Config.ObjectSlots.BehaviorScriptOffset, 4), 0)
                    & 0x7FFFFFFF;
            }

            // Processing sort order
            switch ((SortMethodType)ManagerGui.SortMethodComboBox.SelectedItem)
            {
                case SortMethodType.ProcessingOrder:
                    // Data is already sorted by processing order
                    break;

                case SortMethodType.MemoryOrder:
                    // Order by address
                    newObjectSlotData = newObjectSlotData.OrderBy(s => s.Address).ToList();
                    break;

                case SortMethodType.DistanceToMario:

                    // Order by address
                    var activeObjects = newObjectSlotData.Where(s => s.IsActive).OrderBy(s => s.DistanceToMario);
                    var inActiveObjects = newObjectSlotData.Where(s => !s.IsActive).OrderBy(s => s.DistanceToMario);

                    newObjectSlotData = activeObjects.Concat(inActiveObjects).ToList();
                    break;
            }

            _activeObjCnt = 0;

            _standingOnObject = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.StandingOnObjectPointer, 4), 0);
            _interactingObject = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.InteractingObjectPointerOffset + Config.Mario.StructAddress, 4), 0);
            _holdingObject = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.HoldingObjectPointerOffset + Config.Mario.StructAddress, 4), 0);
            _usingObject = BitConverter.ToUInt32(_stream.ReadRam(Config.Mario.UsingObjectPointerOffset + Config.Mario.StructAddress, 4), 0);
            _closestObject = newObjectSlotData.OrderBy(s => !s.IsActive || s.Behavior == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF) ? float.MaxValue
                : s.DistanceToMario).First().Address;

            // Update slots
            BehaviorCriteria? multiBehavior = null;
            List<BehaviorCriteria> selectedBehaviorCriterias = new List<BehaviorCriteria>();
            bool firstObject = true;
            for (int i = 0; i < slotConfig.MaxSlots; i++)
            {
                var behaviorCritera = UpdateSlot(newObjectSlotData[i], i);
                if (!_selectedSlotsAddresses.Contains(newObjectSlotData[i].Address))
                    continue;

                selectedBehaviorCriterias.Add(behaviorCritera);

                if (multiBehavior.HasValue)
                {
                    multiBehavior = multiBehavior.Value.Generalize(behaviorCritera);
                }
                else if (firstObject)
                {
                    multiBehavior = behaviorCritera;
                    firstObject = false;
                }
            }
            _miscManager.ActiveObjectCount = _activeObjCnt;
            
            if (_selectedSlotsAddresses.Count > 1)
            {
                if (_selectedUpdated || !selectedBehaviorCriterias.SequenceEqual(_prevSelectedBehaviorCriteria))
                {
                    if (_lastSelectedBehavior != multiBehavior)
                    {
                        if (multiBehavior.HasValue)
                        {
                            _objManager.Behavior = String.Format("0x{0}", ((multiBehavior.Value.BehaviorAddress + ObjectAssoc.RamOffset) & 0x00FFFFFF).ToString("X4"));
                            _objManager.SetBehaviorWatchVariables(ObjectAssoc.GetWatchVariables(multiBehavior.Value), Config.ObjectGroups.VacantSlotColor.Lighten(0.8));
                        }
                        else
                        {
                            _objManager.Behavior = "";
                            _objManager.SetBehaviorWatchVariables(new List<WatchVariable>(), Color.White);
                        }
                        _lastSelectedBehavior = multiBehavior;
                    }

                    _multiImage?.Dispose();
                    var multiBitmap = new Bitmap(256, 256);
                    int subImageCount = Math.Min(4, selectedBehaviorCriterias.Count);
                    using (Graphics gfx = Graphics.FromImage(multiBitmap))
                    {
                        Rectangle[] subImagePlaces = 
                        {
                            new Rectangle(0, 0, 128, 128),
                            new Rectangle(128, 0, 128, 128),
                            new Rectangle(0, 128, 128, 128),
                            new Rectangle(128, 128, 128, 128)
                        };
                        for (int i = 0; i < subImageCount; i++)
                        {
                            gfx.DrawImage(ObjectAssoc.GetObjectImage(selectedBehaviorCriterias[i], false), subImagePlaces[i]);
                        }
                    }
                    _multiImage = multiBitmap;
                    _objManager.Image = _multiImage;

                    _objManager.Name = "Multiple Objects Selected";
                    _objManager.BackColor = Config.ObjectGroups.VacantSlotColor;
                    _objManager.SlotIndex = "";
                    _objManager.SlotPos = "";
                    _prevSelectedBehaviorCriteria = selectedBehaviorCriterias;
                }
            }
        }

        private BehaviorCriteria UpdateSlot(ObjectSlotData objectData, int index)
        {
            var objSlot = ObjectSlots[index];
            uint currentAddress = objectData.Address;
            BehaviorCriteria behaviorCriteria;
           
            objSlot.IsActive = objectData.IsActive;
            objSlot.Address = currentAddress;

            // Update Overlays
            objSlot.DrawSelectedOverlay = _selectedSlotsAddresses.Contains(currentAddress);
            objSlot.DrawStandingOnOverlay = Config.ShowOverlays && currentAddress == _standingOnObject;
            objSlot.DrawInteractingOverlay = Config.ShowOverlays && currentAddress == _interactingObject;
            objSlot.DrawHoldingOverlay = Config.ShowOverlays && currentAddress == _holdingObject;
            objSlot.DrawUsingOverlay = Config.ShowOverlays && currentAddress == _usingObject;
            objSlot.DrawClosestOverlay = Config.ShowOverlays && currentAddress == _closestObject;

            if (objectData.IsActive)
                _activeObjCnt++;

            var gfxId = _stream.GetUInt32(currentAddress + Config.ObjectSlots.BehaviorGfxOffset);
            var subType = _stream.GetInt32(currentAddress + Config.ObjectSlots.BehaviorSubtypeOffset);
            var appearance = _stream.GetInt32(currentAddress + Config.ObjectSlots.BehaviorAppearance);

            behaviorCriteria = new BehaviorCriteria()
            {
                BehaviorAddress = objectData.Behavior,
                GfxId = gfxId,
                SubType = subType,
                Appearance = appearance
            };

            ObjectSlots[index].Behavior = behaviorCriteria;

            var processGroup = objectData.ObjectProcessGroup;
            ObjectSlots[index].ProcessGroup = processGroup;

            var newColor = objectData.ObjectProcessGroup == VacantGroup ? Config.ObjectGroups.VacantSlotColor :
                Config.ObjectGroups.ProcessingGroupsColor[objectData.ObjectProcessGroup];
            ObjectSlots[index].BackColor = newColor;

            string labelText = "";
            switch ((SlotLabelType)ManagerGui.LabelMethodComboBox.SelectedItem)
            {
                case SlotLabelType.Recommended:
                    var sortMethod = (SortMethodType)ManagerGui.SortMethodComboBox.SelectedItem;
                    if (sortMethod == SortMethodType.MemoryOrder)
                        goto case SlotLabelType.SlotIndex;
                    goto case SlotLabelType.SlotPosVs;

                case SlotLabelType.SlotIndex:
                    labelText = String.Format("{0}", (objectData.Address - Config.ObjectSlots.LinkStartAddress) 
                        / Config.ObjectSlots.StructSize);
                    break;

                case SlotLabelType.SlotPos:
                    labelText = String.Format("{0}", objectData.ProcessIndex
                        + (Config.SlotIndexsFromOne ? 1 : 0));
                    break;

                case SlotLabelType.SlotPosVs:
                    if (!objectData.VacantSlotIndex.HasValue)
                        goto case SlotLabelType.SlotPos;

                    labelText = String.Format("VS{0}", objectData.VacantSlotIndex.Value 
                        + (Config.SlotIndexsFromOne ? 1 : 0));
                    break;
            }

            if (ManagerGui.LockLabelsCheckbox.Checked)
            {
                if (!_lastSlotLabel.ContainsKey(currentAddress))
                    _lastSlotLabel.Add(currentAddress, labelText);
                else
                    _lastSlotLabel[currentAddress] = labelText;
            }
            ObjectSlots[index].Text = ManagerGui.LockLabelsCheckbox.Checked ? _lastSlotLabel[currentAddress] : labelText;

            // Update object manager image
            if (_selectedSlotsAddresses.Count <= 1 && _selectedSlotsAddresses.Contains(currentAddress))
            {
                var objAssoc = ObjectAssoc.FindObjectAssociation(behaviorCriteria);
                var newBehavior = objAssoc != null ? objAssoc.BehaviorCriteria : (BehaviorCriteria?)null;
                if (_lastSelectedBehavior != newBehavior)
                {
                    _objManager.Behavior = String.Format("0x{0}", ((objectData.Behavior + ObjectAssoc.RamOffset) & 0x00FFFFFF).ToString("X4"));
                    _objManager.Name = ObjectAssoc.GetObjectName(behaviorCriteria);

                    _objManager.SetBehaviorWatchVariables(ObjectAssoc.GetWatchVariables(behaviorCriteria), newColor.Lighten(0.8));
                    _lastSelectedBehavior = newBehavior;
                }
                _objManager.Image = ObjectSlots[index].ObjectImage;
                _objManager.BackColor = newColor;
                int slotPos = objectData.ObjectProcessGroup == VacantGroup ? objectData.VacantSlotIndex.Value : objectData.ProcessIndex;
                _objManager.SlotIndex = (_memoryAddressSlotIndex[currentAddress] + (Config.SlotIndexsFromOne ? 1 : 0)).ToString();
                _objManager.SlotPos = (objectData.ObjectProcessGroup == VacantGroup ? "VS " : "")
                    + (slotPos + (Config.SlotIndexsFromOne ? 1 : 0)).ToString();
            }

            // Update the map
            if (ManagerGui.TabControl.SelectedTab.Text == "Map" && _mapManager.IsLoaded)
            {

                // Update image
                var mapObjImage = ObjectAssoc.GetObjectMapImage(behaviorCriteria, !objectData.IsActive);
                var mapObjRotates = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
                if (!_mapObjects.ContainsKey(currentAddress))
                {
                    _mapObjects.Add(currentAddress, new MapObject(mapObjImage));
                    _mapManager.AddMapObject(_mapObjects[currentAddress]);
                    _mapObjects[currentAddress].UsesRotation = mapObjRotates;
                }
                else if (_mapObjects[currentAddress].Image != mapObjImage)
                {
                    _mapManager.RemoveMapObject(_mapObjects[currentAddress]);
                    _mapObjects[currentAddress] = new MapObject(mapObjImage);
                    _mapManager.AddMapObject(_mapObjects[currentAddress]);
                    _mapObjects[currentAddress].UsesRotation = mapObjRotates;
                }

                if (objectData.Behavior == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF))
                {
                    _mapObjects[currentAddress].Show = false;
                }
                else
                {
                    // Update map object coordinates and rotation
                    _mapObjects[currentAddress].Show = !_toggleMapBehaviors.Contains(behaviorCriteria)
                        && !_toggleMapGroups.Contains(processGroup) && !_toggleMapSlots.Contains(currentAddress);
                    _mapObjects[currentAddress].X = BitConverter.ToSingle(_stream.ReadRam(currentAddress + Config.ObjectSlots.ObjectXOffset, 4), 0);
                    _mapObjects[currentAddress].Y = BitConverter.ToSingle(_stream.ReadRam(currentAddress + Config.ObjectSlots.ObjectYOffset, 4), 0);
                    _mapObjects[currentAddress].Z = BitConverter.ToSingle(_stream.ReadRam(currentAddress + Config.ObjectSlots.ObjectZOffset, 4), 0);
                    _mapObjects[currentAddress].IsActive = objectData.IsActive;
                    _mapObjects[currentAddress].Rotation = (float)((UInt16)(BitConverter.ToUInt32(
                        _stream.ReadRam(currentAddress + Config.ObjectSlots.ObjectRotationOffset, 4), 0)) / 65536f * 360f);
                    _mapObjects[currentAddress].UsesRotation = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
                }
            }
            return behaviorCriteria;
        }

        public void OnSlotDropAction(DropAction dropAction, ObjectSlot objSlot)
        {
            switch (dropAction.Action)
            {
                case DropAction.ActionType.Mario:
                    // Move mario to object
                    var objectAddress = objSlot.Address;
                    MarioActions.RetreiveObjects(_stream, new List<uint>() { objectAddress });
                    break;

                case DropAction.ActionType.Object:
                    break;

                default:
                    return;
            }
        }
    }
}