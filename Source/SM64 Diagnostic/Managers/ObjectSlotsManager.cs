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
        public static ObjectSlotsManager Instance;

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
        public ObjectSlotManagerGui ManagerGui;

        public const byte VacantGroup = 0xFF;

        Dictionary<uint, MapObject> _mapObjects = new Dictionary<uint, MapObject>();
        Dictionary<uint, int> _memoryAddressSlotIndex;
        public Dictionary<uint, ObjectSlot> MemoryAddressSortedSlot = new Dictionary<uint, ObjectSlot>();
        Dictionary<uint, Tuple<int?, int?>> _lastSlotLabel = new Dictionary<uint, Tuple<int?, int?>>();
        bool _labelsLocked = false;
        public List<uint> SelectedSlotsAddresses = new List<uint>();
        public List<uint> SelectedOnMapSlotsAddresses = new List<uint>();

        BehaviorCriteria? _lastSelectedBehavior;
        uint _stoodOnObject, _interactionObject, _heldObject, _usedObject, _closestObject, _cameraObject, _cameraHackObject,
            _floorObject, _wallObject, _ceilingObject, _parentObject, _parentUnusedObject, _parentNoneObject;
        int _activeObjCnt;
        bool _selectedUpdatePending = false;
        Image _multiImage = null;

        public enum SortMethodType { ProcessingOrder, MemoryOrder, DistanceToMario };
        public enum SlotLabelType { Recommended, SlotPosVs, SlotPos, SlotIndex }

        public void ChangeSlotSize(int newSize)
        {
            foreach (var objSlot in ObjectSlots)
                objSlot.Size = new Size(newSize, newSize);
        }

        public ObjectSlotsManager(ObjectAssociations objAssoc,
            ObjectManager objManager, ObjectSlotManagerGui managerGui, MapManager mapManager, MiscManager miscManager, TabControl tabControlMain)
        {
            Instance = this;

            ObjectAssoc = objAssoc;
            _objManager = objManager;
            ManagerGui = managerGui;
            _mapManager = mapManager;
            _miscManager = miscManager;

            // Add SortMethods
            ManagerGui.SortMethodComboBox.DataSource = Enum.GetValues(typeof(ObjectSlotsManager.SortMethodType));
            ManagerGui.SortMethodComboBox.SelectedItem = SortMethodType.ProcessingOrder;

            // Add LabelMethods
            ManagerGui.LabelMethodComboBox.DataSource = Enum.GetValues(typeof(ObjectSlotsManager.SlotLabelType));
            ManagerGui.LabelMethodComboBox.SelectedItem = SlotLabelType.Recommended;

            // Create and setup object slots
            ObjectSlot.tabControlMain = tabControlMain;
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
            bool rightClick = ((System.Windows.Forms.MouseEventArgs)e).Button == MouseButtons.Right;

            // Parse behavior based on tab opened. Right click overrides the tab to be an object tab click.
            if (ManagerGui.TabControl.SelectedTab.Text.Equals("Cam Hack") && !rightClick)
            {
                uint currentCamHackSlot = Config.Stream.GetUInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.ObjectOffset);
                uint newCamHackSlot = currentCamHackSlot == selectedSlot.Address ? 0 : selectedSlot.Address;
                Config.Stream.SetValue(newCamHackSlot, Config.CameraHack.CameraHackStruct + Config.CameraHack.ObjectOffset);
            }
            else
            {
                bool isMapTabClick = ManagerGui.TabControl.SelectedTab.Text.Equals("Map") && !rightClick;
                List<uint> selection = isMapTabClick ? SelectedOnMapSlotsAddresses : SelectedSlotsAddresses;
                var keyboardState = Keyboard.GetState();

                bool isShiftKeyHeld = keyboardState.IsKeyDown(Key.ShiftLeft) || keyboardState.IsKeyDown(Key.ShiftRight);
                bool isCtrlKeyHeld = keyboardState.IsKeyDown(Key.ControlLeft) || keyboardState.IsKeyDown(Key.ControlRight);

                if (!isMapTabClick)
                {
                    ManagerGui.TabControl.SelectedTab = ManagerGui.TabControl.TabPages["tabPageObjects"];
                }

                if (isShiftKeyHeld && selection.Count > 0)
                {
                    uint startRangeAddress = selection[selection.Count - 1];
                    int startRange = ObjectSlots.First(o => o.Address == startRangeAddress).Index;
                    int endRange = selectedSlot.Index;

                    int rangeSize = Math.Abs(endRange - startRange);
                    int iteratorDirection = endRange > startRange ? 1 : -1;

                    for (int i = 0; i <= rangeSize; i++)
                    {
                        int index = startRange + i * iteratorDirection;
                        uint address = ObjectSlots[index].Address;
                        if (!selection.Contains(address))
                            selection.Add(address);
                    }
                }
                else
                {
                    // ctrl functionality is default in map tab
                    if (isCtrlKeyHeld == isMapTabClick)
                    {
                        selection.Clear();
                    }
                    if (selection.Contains(selectedSlot.Address))
                    {
                        selection.Remove(selectedSlot.Address);
                        if (!isMapTabClick)
                        {
                            _lastSelectedBehavior = null;
                        }
                    }
                    else
                    {
                        selection.Add(selectedSlot.Address);
                    }
                }

                if (isMapTabClick)
                {
                        
                }
                else
                {
                    _objManager.CurrentAddresses = selection;
                }
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
                uint currentGroupObject = Config.Stream.GetUInt32(processGroupStructAddress + groupConfig.ProcessNextLinkOffset);

                // Loop through every object within the group
                 while ((currentGroupObject != processGroupStructAddress && currentSlot < slotConfig.MaxSlots))
                {
                    // Validate current object
                    if (Config.Stream.GetUInt16(currentGroupObject + Config.ObjectSlots.HeaderOffset) != 0x18)
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
                    currentGroupObject = Config.Stream.GetUInt32(currentGroupObject + groupConfig.ProcessNextLinkOffset);

                    // Mark next slot
                    currentSlot++;
                }
            }

            var vacantSlotStart = currentSlot;

            // Now calculate vacant addresses
            uint currentObject = Config.Stream.GetUInt32(groupConfig.VactantPointerAddress);
            for (; currentSlot < slotConfig.MaxSlots; currentSlot++)
            {
                // Validate current object
                if (Config.Stream.GetUInt16(currentObject + Config.ObjectSlots.HeaderOffset) != 0x18)
                    return null;

                newObjectSlotData[currentSlot] = new ObjectSlotData()
                {
                    Address = currentObject,
                    ObjectProcessGroup = VacantGroup,
                    ProcessIndex = currentSlot,
                    VacantSlotIndex = currentSlot - vacantSlotStart
                };

                currentObject = Config.Stream.GetUInt32(currentObject + groupConfig.ProcessNextLinkOffset);
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
            marioX = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            marioY = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            marioZ = Config.Stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            // Calculate distance to Mario
            foreach (var objSlot in newObjectSlotData)
            {
                // Get object relative-to-maario position
                float dX, dY, dZ;
                dX = marioX - Config.Stream.GetSingle(objSlot.Address + Config.ObjectSlots.ObjectXOffset);
                dY = marioY - Config.Stream.GetSingle(objSlot.Address + Config.ObjectSlots.ObjectYOffset);
                dZ = marioZ - Config.Stream.GetSingle(objSlot.Address + Config.ObjectSlots.ObjectZOffset);

                objSlot.DistanceToMario = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                // Check if active/loaded
                objSlot.IsActive = Config.Stream.GetUInt16(objSlot.Address + Config.ObjectSlots.ObjectActiveOffset) != 0x0000;

                objSlot.Behavior = Config.Stream.GetUInt32(objSlot.Address + Config.ObjectSlots.BehaviorScriptOffset) & 0x7FFFFFFF;
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

            _stoodOnObject = Config.Stream.GetUInt32(Config.Mario.StoodOnObjectPointer);
            _interactionObject = Config.Stream.GetUInt32(Config.Mario.InteractionObjectPointerOffset + Config.Mario.StructAddress);
            _heldObject = Config.Stream.GetUInt32(Config.Mario.HeldObjectPointerOffset + Config.Mario.StructAddress);
            _usedObject = Config.Stream.GetUInt32(Config.Mario.UsedObjectPointerOffset + Config.Mario.StructAddress);
            _closestObject = newObjectSlotData.OrderBy(s => !s.IsActive || s.Behavior == (ObjectAssoc.MarioBehavior & 0x0FFFFFFF) ? float.MaxValue
                : s.DistanceToMario).First().Address;
            _cameraObject = Config.Stream.GetUInt32(Config.Camera.SecondObject);
            _cameraHackObject = Config.Stream.GetUInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.ObjectOffset);

            uint floorTriangleAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            _floorObject = floorTriangleAddress == 0 ? 0 : Config.Stream.GetUInt32(floorTriangleAddress + Config.TriangleOffsets.AssociatedObject);

            uint wallTriangleAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.WallTriangleOffset);
            _wallObject = wallTriangleAddress == 0 ? 0 : Config.Stream.GetUInt32(wallTriangleAddress + Config.TriangleOffsets.AssociatedObject);

            uint ceilingTriangleAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.CeilingTriangleOffset);
            _ceilingObject = ceilingTriangleAddress == 0 ? 0 : Config.Stream.GetUInt32(ceilingTriangleAddress + Config.TriangleOffsets.AssociatedObject);

            ObjectSlot hoverObjectSlot = Config.ObjectSlots.HoverObjectSlot;
            if (hoverObjectSlot != null)
            {
                _parentObject = Config.Stream.GetUInt32(hoverObjectSlot.Address + Config.ObjectSlots.ParentOffset);
                _parentUnusedObject = _parentObject == Config.ObjectSlots.UnusedSlotAddress ? hoverObjectSlot.Address : 0;
                _parentNoneObject = _parentObject == 0 ? hoverObjectSlot.Address : 0;
            }
            else
            {
                _parentObject = 0;
                _parentUnusedObject = 0;
                _parentNoneObject = 0;
            }

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
                MemoryAddressSortedSlot[newObjectSlotData[i].Address] = ObjectSlots[i];
            }

            BehaviorCriteria? multiBehavior = null;
            List<BehaviorCriteria> selectedBehaviorCriterias = new List<BehaviorCriteria>();
            bool firstObject = true;
            foreach (uint slotAddress in SelectedSlotsAddresses)
            {
                var behaviorCritera = MemoryAddressSortedSlot[slotAddress].Behavior;

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
                _objManager.BackColor = Config.ObjectGroups.VacantSlotColor;
                _objManager.Behavior = "";
                _objManager.SlotIndex = "";
                _objManager.SlotPos = "";
                _objManager.Image = null;
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
            objSlot.DrawStoodOnOverlay = Config.ShowOverlayStoodOnObject && objAddress == _stoodOnObject;
            objSlot.DrawInteractionOverlay = Config.ShowOverlayInteractionObject && objAddress == _interactionObject;
            objSlot.DrawHeldOverlay = Config.ShowOverlayHeldObject && objAddress == _heldObject;
            objSlot.DrawUsedOverlay = Config.ShowOverlayUsedObject && objAddress == _usedObject;
            objSlot.DrawClosestOverlay = Config.ShowOverlayClosestObject && objAddress == _closestObject;
            objSlot.DrawCameraOverlay = Config.ShowOverlayCameraObject && objAddress == _cameraObject;
            objSlot.DrawCameraHackOverlay = Config.ShowOverlayCameraHackObject && objAddress == _cameraHackObject;
            objSlot.DrawFloorOverlay = Config.ShowOverlayFloorObject && objAddress == _floorObject;
            objSlot.DrawWallOverlay = Config.ShowOverlayWallObject && objAddress == _wallObject;
            objSlot.DrawCeilingOverlay = Config.ShowOverlayCeilingObject && objAddress == _ceilingObject;
            objSlot.DrawParentOverlay = Config.ShowOverlayParentObject && objAddress == _parentObject;
            objSlot.DrawParentUnusedOverlay = Config.ShowOverlayParentObject && objAddress == _parentUnusedObject;
            objSlot.DrawParentNoneOverlay = Config.ShowOverlayParentObject && objAddress == _parentNoneObject;

            if (objData.IsActive)
                _activeObjCnt++;

            var gfxId = Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.BehaviorGfxOffset);
            var subType = Config.Stream.GetInt32(objAddress + Config.ObjectSlots.BehaviorSubtypeOffset);
            var appearance = Config.Stream.GetInt32(objAddress + Config.ObjectSlots.BehaviorAppearance);

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
            UpdateMapObject(objData, objSlot, behaviorCriteria);
        }

        void UpdateObjectManager(ObjectSlot objSlot, BehaviorCriteria behaviorCriteria, ObjectSlotData objData)
        {
            var objAssoc = ObjectAssoc.FindObjectAssociation(behaviorCriteria);
            var newBehavior = objAssoc != null ? objAssoc.BehaviorCriteria : behaviorCriteria;
            if (_lastSelectedBehavior != newBehavior || SelectedSlotsAddresses.Count == 0)
            {
                _objManager.Behavior = String.Format("0x{0}", ((objData.Behavior + ObjectAssoc.RamOffset) & 0x00FFFFFF).ToString("X4"));
                _objManager.Name = ObjectAssoc.GetObjectName(behaviorCriteria);

                _objManager.SetBehaviorWatchVariables(ObjectAssoc.GetWatchVariables(behaviorCriteria), objSlot.BackColor.Lighten(0.8));
                _lastSelectedBehavior = newBehavior;
            }
            _objManager.Image = objSlot.ObjectImage;
            _objManager.BackColor = objSlot.BackColor;
            int slotPos = objData.ObjectProcessGroup == VacantGroup ? objData.VacantSlotIndex.Value : objData.ProcessIndex;
            _objManager.SlotIndex = (_memoryAddressSlotIndex[objData.Address] + (Config.SlotIndexsFromOne ? 1 : 0)).ToString();
            _objManager.SlotPos = (objData.ObjectProcessGroup == VacantGroup ? "VS " : "")
                + (slotPos + (Config.SlotIndexsFromOne ? 1 : 0)).ToString();
        }

        void UpdateMapObject(ObjectSlotData objData, ObjectSlot objSlot, BehaviorCriteria behaviorCriteria)
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
                _mapObjects[objAddress].Show = SelectedOnMapSlotsAddresses.Contains(objAddress);
                objSlot.Show = _mapObjects[objAddress].Show;
                _mapObjects[objAddress].X = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectXOffset);
                _mapObjects[objAddress].Y = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectYOffset);
                _mapObjects[objAddress].Z = Config.Stream.GetSingle(objAddress + Config.ObjectSlots.ObjectZOffset);
                _mapObjects[objAddress].IsActive = objData.IsActive;
                _mapObjects[objAddress].Rotation = (float)((UInt16)(
                    Config.Stream.GetUInt32(objAddress + Config.ObjectSlots.ObjectRotationOffset)) / 65536f * 360f);
                _mapObjects[objAddress].UsesRotation = ObjectAssoc.GetObjectMapRotates(behaviorCriteria);
            }
        }
    }
}
 