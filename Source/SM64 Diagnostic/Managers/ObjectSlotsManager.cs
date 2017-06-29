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
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class ObjectSlotsManager
    {
        public class ObjectSlotData
        {
            public uint Address;
            public byte ObjectProcessGroup;
            public int ProcessIndex;
            public int? VacantSlotIndex;
            public float DistanceToMario;
            public bool IsActive;
            public uint Behavior;
        }

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
        Dictionary<uint, ObjectSlot> _memoryAddressSortedSlot = new Dictionary<uint, ObjectSlot>();
        Dictionary<uint, Tuple<int?, int?>> _lastSlotLabel = new Dictionary<uint, Tuple<int?, int?>>();
        bool _labelsLocked = false;
        public List<uint> SelectedSlotsAddresses = new List<uint>();

        List<byte> _toggleMapGroups = new List<byte>();
        List<BehaviorCriteria> _toggleMapBehaviors = new List<BehaviorCriteria>();
        List<uint> _toggleMapSlots = new List<uint>();

        BehaviorCriteria? _lastSelectedBehavior;
        uint _standingOnObject, _interactingObject, _holdingObject, _usingObject, _closestObject, _cameraObject;
        int _activeObjCnt;
        bool _selectedUpdatePending = false;
        Image _multiImage = null;

        bool _firstSlotSelect = true;

        public enum SortMethodType { ProcessingOrder, MemoryOrder, DistanceToMario };
        public enum MapToggleModeType { Single, ObjectType, ProcessGroup };
        public enum SlotLabelType { Recommended, SlotPosVs, SlotPos, SlotIndex }

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
                objectSlot.Click += (sender, e) => OnSlotClick(sender, e);
                ManagerGui.FlowLayoutContainer.Controls.Add(objectSlot);
            }

            // Change default
            ChangeSlotSize(DefaultSlotSize);
        }

        private void OnSlotClick(object sender, EventArgs e)
        {
            // Make sure the tab has loaded
            if (ManagerGui.TabControl.SelectedTab == null)
                return;

            _selectedUpdatePending = true;
            var selectedSlot = sender as ObjectSlot;

            // Parse behavior based on tab opened
            switch (ManagerGui.TabControl.SelectedTab.Text)
            {
                default:
                    var keyboardState = Keyboard.GetState();
                    ManagerGui.TabControl.SelectedTab = ManagerGui.TabControl.TabPages["tabPageObjects"];
                    if ((keyboardState.IsKeyDown(Key.ShiftLeft) || keyboardState.IsKeyDown(Key.ShiftRight))
                        && SelectedSlotsAddresses.Count > 0)
                    {
                        uint startRangeAddress = SelectedSlotsAddresses[SelectedSlotsAddresses.Count - 1];
                        int startRange = ObjectSlots.First(o => o.Address == startRangeAddress).Index;
                        int endRange = selectedSlot.Index;

                        int rangeSize = Math.Abs(endRange - startRange);
                        int iteratorDirection = endRange > startRange ? 1 : -1;

                        for (int i = 0; i <= rangeSize; i++)
                        {
                            int index = startRange + i * iteratorDirection;
                            uint address = ObjectSlots[index].Address;
                            if (!SelectedSlotsAddresses.Contains(address))
                                SelectedSlotsAddresses.Add(address);
                        }
                    }
                    else
                    {
                        if (!(keyboardState.IsKeyDown(Key.ControlLeft) || keyboardState.IsKeyDown(Key.ControlRight)))
                        {
                            SelectedSlotsAddresses.Clear();
                        }
                        if (SelectedSlotsAddresses.Contains(selectedSlot.Address))
                        {
                            if (SelectedSlotsAddresses.Count > 1)
                            {
                                SelectedSlotsAddresses.Remove(selectedSlot.Address);
                            }
                        }
                        else
                        {
                            SelectedSlotsAddresses.Add(selectedSlot.Address);
                        }
                    }

                    _objManager.CurrentAddresses = SelectedSlotsAddresses;
                    break;

                case "Map":
                    switch ((MapToggleModeType)ManagerGui.MapObjectToggleModeComboBox.SelectedItem)
                    {
                        case MapToggleModeType.Single:
                            if (_toggleMapSlots.Contains(selectedSlot.Address))
                                _toggleMapSlots.Remove(selectedSlot.Address);
                            else
                                _toggleMapSlots.Add(selectedSlot.Address);

                            UpdateSelectedMapObjectSlots();

                            break;
                        case MapToggleModeType.ObjectType:
                            var behavior = selectedSlot.Behavior;
                            if (_toggleMapBehaviors.Contains(behavior))
                                _toggleMapBehaviors.Remove(behavior);
                            else
                                _toggleMapBehaviors.Add(behavior);

                            UpdateSelectedMapObjectSlots();
                            break;

                        case MapToggleModeType.ProcessGroup:
                            var group = selectedSlot.ProcessGroup;
                            if (_toggleMapGroups.Contains(group))
                                _toggleMapGroups.Remove(group);
                            else
                                _toggleMapGroups.Add(group);

                            UpdateSelectedMapObjectSlots();
                            break;
                    }
                    break;
            }
        }

        /*
        public void SetAllSelectedMapObjectSlots()
        {
            foreach (var objSlot in ObjectSlots)
            {
                objSlot.SelectedOnMap = true;
            }
        }
        */

        public void UpdateSelectedMapObjectSlots()
        {
            foreach (var objSlot in ObjectSlots)
            {
                bool selected = !_toggleMapGroups.Contains(objSlot.ProcessGroup)
                    && !_toggleMapBehaviors.Contains(objSlot.Behavior)
                    && !_toggleMapSlots.Contains(objSlot.Address);

                objSlot.SelectedOnMap = selected;
            }
        }

        public string GetSlotNameFromAddress(uint address)
        {
            var slot = ObjectSlots.FirstOrDefault(s => s.Address == address);
            if (slot == null)
                return null;

            return slot.Text;
        }

        public uint? GetSlotAddressFromName(string name)
        {
            name = name.ToLower();

            if (!name.StartsWith("slot: "))
                return null;

            name = name.Remove(0, "slot: ".Length);

            var slot = ObjectSlots.FirstOrDefault(s => s.Text.ToLower() == name);
            if (slot == null)
                return null;

            return slot.Address;
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
                uint currentGroupObject = _stream.GetUInt32(processGroupStructAddress + groupConfig.ProcessNextLinkOffset);

                // Loop through every object within the group
                 while ((currentGroupObject != processGroupStructAddress && currentSlot < slotConfig.MaxSlots))
                {
                    // Validate current object
                    if (_stream.GetUInt16(currentGroupObject + Config.ObjectSlots.HeaderOffset) != 0x18)
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
                    currentGroupObject = _stream.GetUInt32(currentGroupObject + groupConfig.ProcessNextLinkOffset);

                    // Mark next slot
                    currentSlot++;
                }
            }

            var vacantSlotStart = currentSlot;

            // Now calculate vacant addresses
            uint currentObject = _stream.GetUInt32(groupConfig.VactantPointerAddress);
            for (; currentSlot < slotConfig.MaxSlots; currentSlot++)
            {
                // Validate current object
                if (_stream.GetUInt16(currentObject + Config.ObjectSlots.HeaderOffset) != 0x18)
                    return null;

                newObjectSlotData[currentSlot] = new ObjectSlotData()
                {
                    Address = currentObject,
                    ObjectProcessGroup = VacantGroup,
                    ProcessIndex = currentSlot,
                    VacantSlotIndex = currentSlot - vacantSlotStart
                };

                currentObject = _stream.GetUInt32(currentObject + groupConfig.ProcessNextLinkOffset);
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
            marioX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            marioY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            marioZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            // Calculate distance to Mario
            foreach (var objSlot in newObjectSlotData)
            {
                // Get object relative-to-maario position
                float dX, dY, dZ;
                dX = marioX - _stream.GetSingle(objSlot.Address + Config.ObjectSlots.ObjectXOffset);
                dY = marioY - _stream.GetSingle(objSlot.Address + Config.ObjectSlots.ObjectYOffset);
                dZ = marioZ - _stream.GetSingle(objSlot.Address + Config.ObjectSlots.ObjectZOffset);

                objSlot.DistanceToMario = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                // Check if active/loaded
                objSlot.IsActive = _stream.GetUInt16(objSlot.Address + Config.ObjectSlots.ObjectActiveOffset) != 0x0000;

                objSlot.Behavior = _stream.GetUInt32(objSlot.Address + Config.ObjectSlots.BehaviorScriptOffset) & 0x7FFFFFFF;
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

            _standingOnObject = _stream.GetUInt32(Config.Mario.StandingOnObjectPointer);
            _interactingObject = _stream.GetUInt32(Config.Mario.InteractingObjectPointerOffset + Config.Mario.StructAddress);
            _holdingObject = _stream.GetUInt32(Config.Mario.HoldingObjectPointerOffset + Config.Mario.StructAddress);
            _usingObject = _stream.GetUInt32(Config.Mario.UsingObjectPointerOffset + Config.Mario.StructAddress);
            _closestObject = newObjectSlotData.OrderBy(s => !s.IsActive || s.Behavior == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF) ? float.MaxValue
                : s.DistanceToMario).First().Address;
            _cameraObject = _stream.GetUInt32(Config.Camera.SecondObject);

            // Update slots
            UpdateSlots(newObjectSlotData);
        }

        private void UpdateSlots(List<ObjectSlotData> newObjectSlotData)
        {
            // Lock label update
            _labelsLocked = ManagerGui.LockLabelsCheckbox.Checked;

            for (int i = 0; i < Config.ObjectSlots.MaxSlots; i++)
            {
                UpdateSlot(newObjectSlotData[i], ObjectSlots[i]);
                _memoryAddressSortedSlot[newObjectSlotData[i].Address] = ObjectSlots[i];
            }

            BehaviorCriteria? multiBehavior = null;
            List<BehaviorCriteria> selectedBehaviorCriterias = new List<BehaviorCriteria>();
            bool firstObject = true;
            foreach (uint slotAddress in SelectedSlotsAddresses)
            {
                var behaviorCritera = _memoryAddressSortedSlot[slotAddress].Behavior;

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

            if (SelectedSlotsAddresses.Count > 1)
            {
                if (_selectedUpdatePending)
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
                    using (Graphics gfx = Graphics.FromImage(multiBitmap))
                    {
                        int numCols = (int)Math.Ceiling(Math.Sqrt(selectedBehaviorCriterias.Count));
                        int numRows = (int)Math.Ceiling(selectedBehaviorCriterias.Count / (double)numCols);
                        int imageSize = 256 / numCols;
                        for (int row = 0; row < numRows; row++)
                        {
                            for (int col = 0; col < numCols; col++)
                            {
                                int index = row * numCols + col;
                                if (index >= selectedBehaviorCriterias.Count) break;
                                Image image = ObjectAssoc.GetObjectImage(selectedBehaviorCriterias[index], false);
                                Rectangle rect = new Rectangle(col * imageSize, row * imageSize, imageSize, imageSize);
                                Rectangle zoomedRect = rect.Zoom(image.Size);
                                gfx.DrawImage(image, zoomedRect);
                            }
                        }
                    }
                    _multiImage = multiBitmap;
                    _objManager.Image = _multiImage;

                    _objManager.Name = SelectedSlotsAddresses.Count + " Objects Selected";
                    _objManager.BackColor = Config.ObjectGroups.VacantSlotColor;
                    _objManager.SlotIndex = "";
                    _objManager.SlotPos = "";
                    _selectedUpdatePending = false;
                }
            }
            else if (SelectedSlotsAddresses.Count == 0)
            {
                _objManager.Name = "No Object Selected";
                _firstSlotSelect = true;
            }
        }

        private void UpdateSlot(ObjectSlotData objData, ObjectSlot objSlot)
        {
            uint objAddress = objData.Address;
            BehaviorCriteria behaviorCriteria;

            objSlot.IsActive = objData.IsActive;
            objSlot.Address = objAddress;

            // Update Overlays
            objSlot.DrawSelectedOverlay = SelectedSlotsAddresses.Contains(objAddress);
            objSlot.DrawStandingOnOverlay = Config.ShowOverlays && objAddress == _standingOnObject;
            objSlot.DrawInteractingOverlay = Config.ShowOverlays && objAddress == _interactingObject;
            objSlot.DrawHoldingOverlay = Config.ShowOverlays && objAddress == _holdingObject;
            objSlot.DrawUsingOverlay = Config.ShowOverlays && objAddress == _usingObject;
            objSlot.DrawClosestOverlay = Config.ShowOverlays && objAddress == _closestObject;
            objSlot.DrawCameraOverlay = Config.ShowOverlays && objAddress == _cameraObject;

            if (objData.IsActive)
                _activeObjCnt++;

            var gfxId = _stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset);
            var subType = _stream.GetInt32(objAddress + Config.ObjectSlots.BehaviorSubtypeOffset);
            var appearance = _stream.GetInt32(objAddress + Config.ObjectSlots.BehaviorAppearance);

            behaviorCriteria = new BehaviorCriteria()
            {
                BehaviorAddress = objData.Behavior,
                GfxId = gfxId,
                SubType = subType,
                Appearance = appearance
            };

            objSlot.Behavior = behaviorCriteria;

            var processGroup = objData.ObjectProcessGroup;
            objSlot.ProcessGroup = processGroup;

            var newColor = objData.ObjectProcessGroup == VacantGroup ? Config.ObjectGroups.VacantSlotColor :
                Config.ObjectGroups.ProcessingGroupsColor[objData.ObjectProcessGroup];
            objSlot.BackColor = newColor;

            if (!_labelsLocked)
                _lastSlotLabel[objAddress] = new Tuple<int?, int?>(objData.ProcessIndex, objData.VacantSlotIndex);

            string labelText = "";
            switch ((SlotLabelType)ManagerGui.LabelMethodComboBox.SelectedItem)
            {
                case SlotLabelType.Recommended:
                    var sortMethod = (SortMethodType)ManagerGui.SortMethodComboBox.SelectedItem;
                    if (sortMethod == SortMethodType.MemoryOrder)
                        goto case SlotLabelType.SlotIndex;
                    goto case SlotLabelType.SlotPosVs;

                case SlotLabelType.SlotIndex:
                    labelText = String.Format("{0}", (objData.Address - Config.ObjectSlots.LinkStartAddress)
                        / Config.ObjectSlots.StructSize + (Config.SlotIndexsFromOne ? 1 : 0));
                    break;

                case SlotLabelType.SlotPos:
                    labelText = String.Format("{0}", _lastSlotLabel[objAddress].Item1
                        + (Config.SlotIndexsFromOne ? 1 : 0));
                    break;

                case SlotLabelType.SlotPosVs:
                    var vacantSlotIndex = _lastSlotLabel[objAddress].Item2;
                    if (!vacantSlotIndex.HasValue)
                        goto case SlotLabelType.SlotPos;

                    labelText = String.Format("VS{0}", vacantSlotIndex.Value
                        + (Config.SlotIndexsFromOne ? 1 : 0));
                    break;
            }

            objSlot.TextColor = _labelsLocked ? Color.Blue : Color.Black;
            objSlot.Text = labelText;

            // Update object manager image
            if (SelectedSlotsAddresses.Count <= 1 && SelectedSlotsAddresses.Contains(objAddress))
                UpdateObjectManager(objSlot, behaviorCriteria, objData);

            // Update the map
            UpdateMapObject(objData, behaviorCriteria);
        }

        void UpdateObjectManager(ObjectSlot objSlot, BehaviorCriteria behaviorCriteria, ObjectSlotData objData)
        {
            var objAssoc = ObjectAssoc.FindObjectAssociation(behaviorCriteria);
            var newBehavior = objAssoc != null ? objAssoc.BehaviorCriteria : behaviorCriteria;
            if (_lastSelectedBehavior != newBehavior || _firstSlotSelect)
            {
                _objManager.Behavior = String.Format("0x{0}", ((objData.Behavior + ObjectAssoc.RamOffset) & 0x00FFFFFF).ToString("X4"));
                _objManager.Name = ObjectAssoc.GetObjectName(behaviorCriteria);

                _objManager.SetBehaviorWatchVariables(ObjectAssoc.GetWatchVariables(behaviorCriteria), objSlot.BackColor.Lighten(0.8));
                _lastSelectedBehavior = newBehavior;
                _firstSlotSelect = false;
            }
            _objManager.Image = objSlot.ObjectImage;
            _objManager.BackColor = objSlot.BackColor;
            int slotPos = objData.ObjectProcessGroup == VacantGroup ? objData.VacantSlotIndex.Value : objData.ProcessIndex;
            _objManager.SlotIndex = (_memoryAddressSlotIndex[objData.Address] + (Config.SlotIndexsFromOne ? 1 : 0)).ToString();
            _objManager.SlotPos = (objData.ObjectProcessGroup == VacantGroup ? "VS " : "")
                + (slotPos + (Config.SlotIndexsFromOne ? 1 : 0)).ToString();
        }

        void UpdateMapObject(ObjectSlotData objData, BehaviorCriteria behaviorCriteria)
        {
            if (ManagerGui.TabControl.SelectedTab.Text != "Map" || !_mapManager.IsLoaded)
                return;

            var objAddress = objData.Address;

            // Update image
            var mapObjImage = ObjectAssoc.GetObjectMapImage(behaviorCriteria, !objData.IsActive);
            var mapObjRotates = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
            if (!_mapObjects.ContainsKey(objAddress))
            {
                _mapObjects.Add(objAddress, new MapObject(mapObjImage));
                _mapManager.AddMapObject(_mapObjects[objAddress]);
                _mapObjects[objAddress].UsesRotation = mapObjRotates;
            }
            else if (_mapObjects[objAddress].Image != mapObjImage)
            {
                _mapManager.RemoveMapObject(_mapObjects[objAddress]);
                _mapObjects[objAddress] = new MapObject(mapObjImage);
                _mapManager.AddMapObject(_mapObjects[objAddress]);
                _mapObjects[objAddress].UsesRotation = mapObjRotates;
            }

            if (objData.Behavior == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF))
            {
                _mapObjects[objAddress].Show = false;
            }
            else
            {
                // Update map object coordinates and rotation
                _mapObjects[objAddress].Show = _toggleMapBehaviors.Contains(behaviorCriteria)
                    || _toggleMapGroups.Contains(objData.ObjectProcessGroup) || _toggleMapSlots.Contains(objAddress);
                _mapObjects[objAddress].X = _stream.GetSingle(objAddress + Config.ObjectSlots.ObjectXOffset);
                _mapObjects[objAddress].Y = _stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                _mapObjects[objAddress].Z = _stream.GetSingle(objAddress + Config.ObjectSlots.ObjectZOffset);
                _mapObjects[objAddress].IsActive = objData.IsActive;
                _mapObjects[objAddress].Rotation = (float)((UInt16)(
                    _stream.GetUInt32(objAddress + Config.ObjectSlots.ObjectRotationOffset)) / 65536f * 360f);
                _mapObjects[objAddress].UsesRotation = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
            }
        }
    }
}
 