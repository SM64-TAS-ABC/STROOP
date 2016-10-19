﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System.Drawing;

namespace SM64_Diagnostic.ManagerClasses
{
    public class ObjectSlotManager
    {
        public ObjectSlot[] ObjectSlots;
        public ObjectSlotData[] ObjectSlotData;

        Config _config;
        public ObjectAssociations ObjectAssoc;
        ObjectManager _objManager;
        MapManager _mapManager;
        MiscManager _miscManager;
        ProcessStream _stream;
        public ObjectSlotManagerGui ManagerGui;

        Dictionary<uint, MapObject> _mapObjects = new Dictionary<uint, MapObject>();
        Dictionary<uint, int> _memoryAddressSlotIndex;
        Dictionary<uint, string> _lastSlotLabel = new Dictionary<uint, string>();
        int _selectedSlot;

        List<byte> _toggleMapGroups = new List<byte>();
        List<BehaviorCriteria> _toggleMapBehaviors = new List<BehaviorCriteria>();
        List<uint> _toggleMapSlots = new List<uint>();

        BehaviorCriteria? _lastSelectedBehavior;

        public uint? SelectedAddress = null;
        public const byte VacantGroup = 0xFF;

        public enum SortMethodType {ProcessingOrder, MemoryOrder};
        public enum MapToggleModeType {Single, ObjectType, ProcessGroup};

        public SortMethodType SortMethod = SortMethodType.ProcessingOrder;

        public int MaxSlots
        {
            get
            {
                return _config.ObjectSlots.MaxSlots;
            }
        }

        public int SelectedSlot
        {
            get
            {
                return _selectedSlot;
            }
            set
            {
                var selectedObjData = GetObjectDataFromSlot(value);
                SelectedAddress = selectedObjData.HasValue ? selectedObjData.Value.Address : (uint?) null;
                _objManager.CurrentAddress = SelectedAddress.Value;
                _selectedSlot = value;
            }
        }

        public void ChangeSlotSize(int newSize)
        {
            foreach (var objSlot in ObjectSlots)
                objSlot.Size = new Size(newSize, newSize);
        }

        public ObjectSlotManager(ProcessStream stream, Config config, ObjectAssociations objAssoc, 
            ObjectManager objManager, ObjectSlotManagerGui managerGui, MapManager mapManager, MiscManager miscManager)
        {

            _config = config;
            ObjectAssoc = objAssoc;
            _stream = stream;
            _stream.OnUpdate += OnUpdate;
            _objManager = objManager;
            ManagerGui = managerGui;
            _mapManager = mapManager;
            _miscManager = miscManager;

            foreach (var mode in Enum.GetValues(typeof(MapToggleModeType)))
                ManagerGui.MapObjectToggleModeComboBox.Items.Add(mode);
            ManagerGui.MapObjectToggleModeComboBox.SelectedIndex = 0;

            // Create and setup object slots
            ObjectSlots = new ObjectSlot[_config.ObjectSlots.MaxSlots];
            ObjectSlotData = new ObjectSlotData[_config.ObjectSlots.MaxSlots];
            for (int i = 0; i < _config.ObjectSlots.MaxSlots; i++)
            {
                var objectSlot = new ObjectSlot(i, this, ManagerGui, new Size(40,40));
                ObjectSlots[i] = objectSlot;
                int localI = i;
                objectSlot.Click += (sender, e) => OnClick(sender, e, localI);
                ManagerGui.FlowLayoutContainer.Controls.Add(objectSlot);
            }

            ChangeSlotSize(40);
        }

        private ObjectSlotData? GetObjectDataFromSlot(int slot)
        {
            if (ObjectSlotData.Count((objData) => objData.Index == slot) == 0)
                return null;

            return ObjectSlotData.First((objData) => objData.Index == slot);
        }

        private void OnClick(object sender, EventArgs e, int slotIndex)
        {
            if (ManagerGui.TabControl.SelectedTab == null)
                return;

            switch (ManagerGui.TabControl.SelectedTab.Text)
            {
                default:
                    ManagerGui.TabControl.SelectedTab = ManagerGui.TabControl.TabPages["tabPageObjects"];
                    SelectedSlot = slotIndex;
                    break;
                case "Map":
                    switch ((MapToggleModeType)ManagerGui.MapObjectToggleModeComboBox.SelectedItem)
                    {
                        case MapToggleModeType.Single:
                            var objectData = GetObjectDataFromSlot(slotIndex);
                            if (objectData.HasValue)
                            {
                                if (_toggleMapSlots.Contains(objectData.Value.Address))
                                    _toggleMapSlots.Remove(objectData.Value.Address);
                                else
                                    _toggleMapSlots.Add(objectData.Value.Address);

                                UpdateSelectedObjectSlots();
                            }

                            break;
                        case MapToggleModeType.ObjectType:
                            var behavior = ObjectSlots[slotIndex].Behavior;
                            if (_toggleMapBehaviors.Contains(behavior))
                                _toggleMapBehaviors.Remove(behavior);
                            else
                                _toggleMapBehaviors.Add(behavior);

                            UpdateSelectedObjectSlots();
                            break;
                        case MapToggleModeType.ProcessGroup:
                            var group = ObjectSlots[slotIndex].ProcessGroup;
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
            for (int index = 0; index < ObjectSlots.Length; index++)
            {
                ObjectSlots[index].Selected = true;
            }
        }

        public void UpdateSelectedObjectSlots()
        {
            for (int index = 0; index < ObjectSlots.Length; index++)
            {
                bool selected = !_toggleMapGroups.Contains(ObjectSlots[index].ProcessGroup)
                    && !_toggleMapBehaviors.Contains(ObjectSlots[index].Behavior)
                    && !_toggleMapSlots.Contains(ObjectSlots[index].Address);

                ObjectSlots[index].Selected = selected;
            }
        }

        private ObjectSlotData[] GetProcessedObjects(ObjectGroupsConfig groupConfig, ObjectSlotsConfig slotConfig)
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
                    if (BitConverter.ToUInt16(_stream.ReadRam(currentGroupObject + _config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
                        return null;

                    // Get data
                    newObjectSlotData[currentSlot].Address = currentGroupObject;
                    newObjectSlotData[currentSlot].ObjectProcessGroup = objectProcessGroup;
                    newObjectSlotData[currentSlot].Index = currentSlot;
                    newObjectSlotData[currentSlot].ProcessIndex = currentSlot;
                    newObjectSlotData[currentSlot].VacantSlotIndex = null;

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
                if (BitConverter.ToUInt16(_stream.ReadRam(currentObject + _config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
                    return null;

                newObjectSlotData[currentSlot].Address = currentObject;
                newObjectSlotData[currentSlot].ObjectProcessGroup = VacantGroup;
                newObjectSlotData[currentSlot].Index = currentSlot;
                newObjectSlotData[currentSlot].ProcessIndex = currentSlot;
                newObjectSlotData[currentSlot].VacantSlotIndex = currentSlot - vacantSlotStart;

                currentObject = BitConverter.ToUInt32(
                    _stream.ReadRam(currentObject + groupConfig.ProcessNextLinkOffset, 4), 0);
            }

            return newObjectSlotData;
        }

        public void OnUpdate(object sender, EventArgs e)
        {
            var groupConfig = _config.ObjectGroups;
            var slotConfig = _config.ObjectSlots;

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

            // Processing sort order
            switch (SortMethod)
            {
                case SortMethodType.ProcessingOrder:
                    // Data is already sorted
                    break;

                case SortMethodType.MemoryOrder:
                    // Order by address
                    for (int i = 0; i < slotConfig.MaxSlots; i++)
                    {
                        newObjectSlotData[i].Index = _memoryAddressSlotIndex[newObjectSlotData[i].Address];
                    }
                    break;
            }
            int activeObjCnt = 0;

            var standingOnObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.StandingOnObjectPointer, 4), 0);
            var interactingObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.InteractingObjectPointerOffset + _config.Mario.MarioStructAddress, 4), 0);
            var holdingObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.HoldingObjectPointerOffset + _config.Mario.MarioStructAddress, 4), 0);
            var usingObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.UsingObjectPointerOffset + _config.Mario.MarioStructAddress, 4), 0);

            // Update slots
            foreach (var objectData in newObjectSlotData)
            {
                uint currentAddress = objectData.Address;
                int index = objectData.Index;
                var isActive = BitConverter.ToUInt16(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectActiveOffset, 2), 0) != 0x0000;
                ObjectSlots[index].IsActive = isActive;
                ObjectSlots[index].Address = currentAddress;

                // Update Overlays
                ObjectSlots[index].DrawSelectedOverlay = SelectedAddress == currentAddress;
                ObjectSlots[index].DrawStandingOnOverlay = _config.ShowOverlays && currentAddress == standingOnObject;
                ObjectSlots[index].DrawInteractingOverlay = _config.ShowOverlays && currentAddress == interactingObject;
                ObjectSlots[index].DrawHoldingOverlay = _config.ShowOverlays && currentAddress == holdingObject;
                ObjectSlots[index].DrawUsingOverlay = _config.ShowOverlays && currentAddress == usingObject;

                if (isActive)
                    activeObjCnt++;

                var behaviorScriptAdd = BitConverter.ToUInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorScriptOffset, 4), 0)
                    & 0x7FFFFFFF;

                var gfxId = BitConverter.ToUInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorGfxOffset, 4), 0);
                var subType = BitConverter.ToInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorSubtypeOffset, 4), 0);
                var appearance = BitConverter.ToInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorAppearance, 4), 0);

                var behaviorCriteria = new BehaviorCriteria()
                {
                    BehaviorAddress = behaviorScriptAdd,
                    GfxId = gfxId,
                    SubType = subType,
                    Appearance = appearance
                };

                ObjectSlots[index].Behavior = behaviorCriteria;

                var processGroup = objectData.ObjectProcessGroup;
                ObjectSlots[index].ProcessGroup = processGroup;

                var newColor = objectData.ObjectProcessGroup == VacantGroup ? groupConfig.VacantSlotColor : groupConfig.ProcessingGroupsColor[objectData.ObjectProcessGroup];
                ObjectSlots[index].BackColor = newColor;

                var labelText = (SortMethod == SortMethodType.ProcessingOrder && objectData.VacantSlotIndex.HasValue) ?
                    String.Format("VS{0}", objectData.VacantSlotIndex.Value + (_config.SlotIndexsFromOne ? 1 : 0))
                    : (objectData.Index + (_config.SlotIndexsFromOne ? 1 : 0)).ToString();
                if (ManagerGui.LockLabelsCheckbox.Checked)
                {
                    if (!_lastSlotLabel.ContainsKey(currentAddress))
                        _lastSlotLabel.Add(currentAddress, labelText);
                    else
                        _lastSlotLabel[currentAddress] = labelText;
                }
                ObjectSlots[index].Text = ManagerGui.LockLabelsCheckbox.Checked ? _lastSlotLabel[currentAddress] : labelText;

                // Update object manager image
                if (SelectedAddress.HasValue && SelectedAddress.Value == currentAddress)
                {
                    var objAssoc = ObjectAssoc.FindObjectAssociation(behaviorCriteria);
                    var newBehavior = objAssoc != null ? objAssoc.BehaviorCriteria : (BehaviorCriteria ?)null;
                    if (_lastSelectedBehavior != newBehavior)
                    {
                        _objManager.Behavior = (behaviorScriptAdd + ObjectAssoc.RamOffset) & 0x00FFFFFF;
                        _objManager.Name = ObjectAssoc.GetObjectName(behaviorCriteria);
                        
                        _objManager.SetBehaviorWatchVariables(ObjectAssoc.GetWatchVariables(behaviorCriteria), newColor.Lighten(0.8));
                        _lastSelectedBehavior = newBehavior;
                    }
                    _objManager.Image = ObjectSlots[index].ObjectImage;
                    _objManager.BackColor = newColor;
                    int slotPos = objectData.ObjectProcessGroup == VacantGroup ? objectData.VacantSlotIndex.Value : objectData.ProcessIndex;
                    _objManager.SlotIndex = _memoryAddressSlotIndex[currentAddress] + (_config.SlotIndexsFromOne ? 1 : 0);
                    _objManager.SlotPos = (objectData.ObjectProcessGroup == VacantGroup ? "VS " : "")
                        + (slotPos + (_config.SlotIndexsFromOne ? 1 : 0)).ToString();
                    _objManager.Update();   
                }

                // Update the map
                if (ManagerGui.TabControl.SelectedTab.Text == "Map" && _mapManager.IsLoaded) 
                {

                    // Update image
                    var mapObjImage = ObjectAssoc.GetObjectMapImage(behaviorCriteria, !isActive);
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

                    if (behaviorScriptAdd == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF))
                    {
                        _mapObjects[currentAddress].Show = false;
                    }
                    else
                    {
                        // Update map object coordinates and rotation
                        _mapObjects[currentAddress].Show = !_toggleMapBehaviors.Contains(behaviorCriteria)
                            && !_toggleMapGroups.Contains(processGroup) && !_toggleMapSlots.Contains(currentAddress);
                        _mapObjects[currentAddress].X = BitConverter.ToSingle(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectXOffset, 4), 0);
                        _mapObjects[currentAddress].Y = BitConverter.ToSingle(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectYOffset, 4), 0);
                        _mapObjects[currentAddress].Z = BitConverter.ToSingle(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectZOffset, 4), 0);
                        _mapObjects[currentAddress].IsActive = isActive;
                        _mapObjects[currentAddress].Rotation = (float)((UInt16)(BitConverter.ToUInt32(
                            _stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectRotationOffset, 4), 0)) / 65536f * 360f);
                        _mapObjects[currentAddress].UsesRotation = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
                    }
                }
            }
            _miscManager.ActiveObjectCount = activeObjCnt;
            ObjectSlotData = newObjectSlotData;
        }

        public void OnSlotDropAction(DropAction dropAction, ObjectSlot objSlot)
        {
            switch (dropAction.Action)
            {
                case DropAction.ActionType.Mario:
                    // Move mario to object
                    var objectAddress = ObjectSlotData.First((objData) => objData.Index == objSlot.Index).Address;
                    MarioActions.MoveMarioToObject(_stream, _config, objectAddress);
                    break;

                case DropAction.ActionType.Object:
                    break;

                default:
                    return;
            }
        }
    }
}