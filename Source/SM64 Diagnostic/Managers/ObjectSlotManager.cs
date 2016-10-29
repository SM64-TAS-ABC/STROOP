using System;
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

        Config _config;
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
        int activeObjCnt;

        public enum SortMethodType {ProcessingOrder, MemoryOrder, DistanceToMario};
        public enum MapToggleModeType {Single, ObjectType, ProcessGroup};

        public SortMethodType SortMethod = SortMethodType.ProcessingOrder;

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
            for (int i = 0; i < _config.ObjectSlots.MaxSlots; i++)
            {
                var objectSlot = new ObjectSlot(i, this, ManagerGui, new Size(40,40));
                ObjectSlots[i] = objectSlot;
                objectSlot.Click += (sender, e) => OnClick(sender, e);
                ManagerGui.FlowLayoutContainer.Controls.Add(objectSlot);
            }

            ChangeSlotSize(40);
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
                    ManagerGui.TabControl.SelectedTab = ManagerGui.TabControl.TabPages["tabPageObjects"];
                    _selectedSlotsAddresses.Clear();
                    _selectedSlotsAddresses.Add(selectedSlot.Address);
                    _objManager.CurrentAddress = selectedSlot.Address;
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
                    if (BitConverter.ToUInt16(_stream.ReadRam(currentGroupObject + _config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
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
                if (BitConverter.ToUInt16(_stream.ReadRam(currentObject + _config.ObjectSlots.HeaderOffset, 2), 0) != 0x18)
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

            // Calculate distance to Mario

            return newObjectSlotData.ToList();
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

            // Get mario position
            float marioX, marioY, marioZ;
            marioX = BitConverter.ToSingle(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.XOffset, 4), 0);
            marioY = BitConverter.ToSingle(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.YOffset, 4), 0);
            marioZ = BitConverter.ToSingle(_stream.ReadRam(_config.Mario.MarioStructAddress + _config.Mario.ZOffset, 4), 0);

            // Calculate distance to Mario
            foreach(var objSlot in newObjectSlotData)
            { 
                // Get object relative-to-maario position
                float dX, dY, dZ;
                dX = marioX - BitConverter.ToSingle(_stream.ReadRam(objSlot.Address + _config.ObjectSlots.ObjectXOffset, 4), 0);
                dY = marioY - BitConverter.ToSingle(_stream.ReadRam(objSlot.Address + _config.ObjectSlots.ObjectYOffset, 4), 0);
                dZ = marioZ - BitConverter.ToSingle(_stream.ReadRam(objSlot.Address + _config.ObjectSlots.ObjectZOffset, 4), 0);

                objSlot.DistanceToMario = (float) Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                // Check if active/loaded
                objSlot.IsActive = BitConverter.ToUInt16(_stream.ReadRam(objSlot.Address + _config.ObjectSlots.ObjectActiveOffset, 2), 0) != 0x0000;

                objSlot.Behavior = BitConverter.ToUInt32(_stream.ReadRam(objSlot.Address + _config.ObjectSlots.BehaviorScriptOffset, 4), 0)
                    & 0x7FFFFFFF;
            }

            // Processing sort order
            switch (SortMethod)
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

            activeObjCnt = 0;

            _standingOnObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.StandingOnObjectPointer, 4), 0);
            _interactingObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.InteractingObjectPointerOffset + _config.Mario.MarioStructAddress, 4), 0);
            _holdingObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.HoldingObjectPointerOffset + _config.Mario.MarioStructAddress, 4), 0);
            _usingObject = BitConverter.ToUInt32(_stream.ReadRam(_config.Mario.UsingObjectPointerOffset + _config.Mario.MarioStructAddress, 4), 0);
            _closestObject = newObjectSlotData.OrderBy(s => !s.IsActive || s.Behavior == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF) ? float.MaxValue
                : s.DistanceToMario).First().Address;

            // Update slots
            for (int i = 0; i < slotConfig.MaxSlots; i++)
            {
                UpdateSlot(newObjectSlotData[i], i);
            }
            _miscManager.ActiveObjectCount = activeObjCnt;
        }

        private void UpdateSlot(ObjectSlotData objectData, int index)
        {
            var objSlot = ObjectSlots[index];
            uint currentAddress = objectData.Address;

           
            objSlot.IsActive = objectData.IsActive;
            objSlot.Address = currentAddress;

            // Update Overlays
            objSlot.DrawSelectedOverlay = _selectedSlotsAddresses.Contains(currentAddress);
            objSlot.DrawStandingOnOverlay = _config.ShowOverlays && currentAddress == _standingOnObject;
            objSlot.DrawInteractingOverlay = _config.ShowOverlays && currentAddress == _interactingObject;
            objSlot.DrawHoldingOverlay = _config.ShowOverlays && currentAddress == _holdingObject;
            objSlot.DrawUsingOverlay = _config.ShowOverlays && currentAddress == _usingObject;
            objSlot.DrawClosestOverlay = _config.ShowOverlays && currentAddress == _closestObject;

            if (objectData.IsActive)
                activeObjCnt++;

            var gfxId = BitConverter.ToUInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorGfxOffset, 4), 0);
            var subType = BitConverter.ToInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorSubtypeOffset, 4), 0);
            var appearance = BitConverter.ToInt32(_stream.ReadRam(currentAddress + _config.ObjectSlots.BehaviorAppearance, 4), 0);

            var behaviorCriteria = new BehaviorCriteria()
            {
                BehaviorAddress = objectData.Behavior,
                GfxId = gfxId,
                SubType = subType,
                Appearance = appearance
            };

            ObjectSlots[index].Behavior = behaviorCriteria;

            var processGroup = objectData.ObjectProcessGroup;
            ObjectSlots[index].ProcessGroup = processGroup;

            var newColor = objectData.ObjectProcessGroup == VacantGroup ? _config.ObjectGroups.VacantSlotColor : _config.ObjectGroups.ProcessingGroupsColor[objectData.ObjectProcessGroup];
            ObjectSlots[index].BackColor = newColor;

            var labelText = (SortMethod == SortMethodType.ProcessingOrder && objectData.VacantSlotIndex.HasValue) ?
                String.Format("VS{0}", objectData.VacantSlotIndex.Value + (_config.SlotIndexsFromOne ? 1 : 0))
                : (index + (_config.SlotIndexsFromOne ? 1 : 0)).ToString();
            if (ManagerGui.LockLabelsCheckbox.Checked)
            {
                if (!_lastSlotLabel.ContainsKey(currentAddress))
                    _lastSlotLabel.Add(currentAddress, labelText);
                else
                    _lastSlotLabel[currentAddress] = labelText;
            }
            ObjectSlots[index].Text = ManagerGui.LockLabelsCheckbox.Checked ? _lastSlotLabel[currentAddress] : labelText;

            // Update object manager image
            if (_selectedSlotsAddresses.Contains(currentAddress))
            {
                var objAssoc = ObjectAssoc.FindObjectAssociation(behaviorCriteria);
                var newBehavior = objAssoc != null ? objAssoc.BehaviorCriteria : (BehaviorCriteria?)null;
                if (_lastSelectedBehavior != newBehavior)
                {
                    _objManager.Behavior = (objectData.Behavior + ObjectAssoc.RamOffset) & 0x00FFFFFF;
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
                    _mapObjects[currentAddress].X = BitConverter.ToSingle(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectXOffset, 4), 0);
                    _mapObjects[currentAddress].Y = BitConverter.ToSingle(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectYOffset, 4), 0);
                    _mapObjects[currentAddress].Z = BitConverter.ToSingle(_stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectZOffset, 4), 0);
                    _mapObjects[currentAddress].IsActive = objectData.IsActive;
                    _mapObjects[currentAddress].Rotation = (float)((UInt16)(BitConverter.ToUInt32(
                        _stream.ReadRam(currentAddress + _config.ObjectSlots.ObjectRotationOffset, 4), 0)) / 65536f * 360f);
                    _mapObjects[currentAddress].UsesRotation = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
                }
            }
        }

        public void OnSlotDropAction(DropAction dropAction, ObjectSlot objSlot)
        {
            switch (dropAction.Action)
            {
                case DropAction.ActionType.Mario:
                    // Move mario to object
                    var objectAddress = objSlot.Address;
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