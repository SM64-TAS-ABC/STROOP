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
using OpenTK.Input;
using STROOP.Structs.Configurations;
using STROOP.Controls;

namespace STROOP.Managers
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
        public ObjectSlotManagerGui ManagerGui;

        public const byte VacantGroup = 0xFF;

        Dictionary<uint, MapObject> _mapObjects = new Dictionary<uint, MapObject>();
        Dictionary<uint, int> _memoryAddressSlotIndex;
        public Dictionary<uint, ObjectSlot> MemoryAddressSortedSlot = new Dictionary<uint, ObjectSlot>();
        Dictionary<uint, Tuple<int?, int?>> _lastSlotLabel = new Dictionary<uint, Tuple<int?, int?>>();
        bool _labelsLocked = false;
        public List<uint> SelectedSlotsAddresses = new List<uint>();
        public List<uint> SelectedOnMapSlotsAddresses = new List<uint>();
        public List<uint> MarkedSlotsAddresses = new List<uint>();

        BehaviorCriteria? _lastSelectedBehavior;
        uint _stoodOnObject, _interactionObject, _heldObject, _usedObject, _closestObject, _cameraObject, _cameraHackObject,
            _modelObject, _floorObject, _wallObject, _ceilingObject, _parentObject, _parentUnusedObject, _parentNoneObject;
        bool _selectedUpdatePending = false;
        Image _multiImage = null;

        private int _activeObjectCount;
        public int ActiveObjectCount { get { return _activeObjectCount; } }

        public enum SortMethodType { ProcessingOrder, MemoryOrder, DistanceToMario };
        public enum SlotLabelType { Recommended, SlotPosVs, SlotPos, SlotIndex }
        public SortMethodType SortMethod = SortMethodType.ProcessingOrder;
        public SlotLabelType LabelMethod = SlotLabelType.Recommended;

        public void ChangeSlotSize(int newSize)
        {
            foreach (var objSlot in ObjectSlots)
                objSlot.Size = new Size(newSize, newSize);
        }

        public ObjectSlotsManager(ObjectSlotManagerGui managerGui, TabControl tabControlMain)
        {
            ManagerGui = managerGui;

            // Add SortMethods
            ManagerGui.SortMethodComboBox.DataSource = Enum.GetValues(typeof(ObjectSlotsManager.SortMethodType));

            // Add LabelMethods
            ManagerGui.LabelMethodComboBox.DataSource = Enum.GetValues(typeof(ObjectSlotsManager.SlotLabelType));

            ManagerGui.TabControl.Selected += TabControl_Selected;
            TabControl_Selected(this, new TabControlEventArgs(ManagerGui.TabControl.SelectedTab, -1, TabControlAction.Selected));

            // Create and setup object slots
            ObjectSlot.tabControlMain = tabControlMain;
            ObjectSlots = new ObjectSlot[ObjectSlotsConfig.MaxSlots];
            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
            {
                var objectSlot = new ObjectSlot(i, this, ManagerGui, new Size(DefaultSlotSize, DefaultSlotSize));
                ObjectSlots[i] = objectSlot;
                objectSlot.Click += (sender, e) => OnSlotClick(sender, e);
                ManagerGui.FlowLayoutContainer.Controls.Add(objectSlot);
            }

            // Change default
            ChangeSlotSize(DefaultSlotSize);
        }

        private void TabControl_Selected(object sender, TabControlEventArgs e)
        {
            switch (e.TabPage.Text)
            {
                case "Object":
                    ActiveTab = TabType.Object;
                    break;

                case "Map":
                    ActiveTab = TabType.Map;
                    break;

                case "Model":
                    ActiveTab = TabType.Model;
                    break;

                case "Custom":
                    ActiveTab = TabType.Custom;
                    break;

                case "Cam Hack":
                    ActiveTab = TabType.CamHack;
                    break;

                default:
                    ActiveTab = TabType.Other;
                    break;
            }
        }

        public enum TabType { Object, Map, Model, Custom, CamHack, Other };

        public TabType ActiveTab;

        public enum ClickType { ObjectClick, MapClick, ModelClick, CamHackClick, MarkClick };

        private void OnSlotClick(object sender, EventArgs e)
        {
            // Make sure the tab has loaded
            if (ManagerGui.TabControl.SelectedTab == null)
                return;

            _selectedUpdatePending = true;
            ObjectSlot selectedSlot = sender as ObjectSlot;
            selectedSlot.Focus();
            KeyboardState keyboardState = Keyboard.GetState();
            bool isCtrlKeyHeld = keyboardState.IsKeyDown(Key.ControlLeft) || keyboardState.IsKeyDown(Key.ControlRight);
            bool isShiftKeyHeld = keyboardState.IsKeyDown(Key.ShiftLeft) || keyboardState.IsKeyDown(Key.ShiftRight);
            bool isAltKeyHeld = keyboardState.IsKeyDown(Key.AltLeft) || keyboardState.IsKeyDown(Key.AltRight);

            DoSlotClickUsingInput(selectedSlot, isCtrlKeyHeld, isShiftKeyHeld, isAltKeyHeld);
        }

        public void SelectSlotByAddress(uint address)
        {
            ObjectSlot slot = ObjectSlots.FirstOrDefault(s => s.Address == address);
            if (slot != null) DoSlotClickUsingInput(slot, false, false, false);
        }

        private void DoSlotClickUsingInput(
            ObjectSlot selectedSlot, bool isCtrlKeyHeld, bool isShiftKeyHeld, bool isAltKeyHeld)
        {
            ClickType click = GetClickType(isAltKeyHeld);
            bool shouldToggle = ShouldToggle(isCtrlKeyHeld, isAltKeyHeld);
            bool shouldExtendRange = isShiftKeyHeld;
            DoSlotClickUsingSpecifications(selectedSlot, click, shouldToggle, shouldExtendRange);
        }

        private ClickType GetClickType(bool isAltKeyHeld)
        {
            ClickType click;
            if (isAltKeyHeld)
            {
                click = ClickType.MarkClick;
            }
            else
            {
                switch (ActiveTab)
                {
                    case TabType.CamHack:
                        click = ClickType.CamHackClick;
                        break;

                    case TabType.Map:
                        click = ClickType.MapClick;
                        break;

                    case TabType.Model:
                        click = ClickType.ModelClick;
                        break;

                    case TabType.Object:
                    case TabType.Custom:
                    case TabType.Other:
                        click = ClickType.ObjectClick;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return click;
        }

        private bool ShouldToggle(bool isCtrlKeyHeld, bool isAltKeyHeld)
        {
            bool isTogglingTab = ActiveTab == TabType.Map || ActiveTab == TabType.CamHack;
            bool isToggleState = isAltKeyHeld ? true : isTogglingTab;
            return isToggleState != isCtrlKeyHeld;
        }

        private bool ShouldSwitchToObjTabByDefault()
        {
            return ActiveTab == TabType.Object || ActiveTab == TabType.Other;
        }

        public void DoSlotClickUsingSpecifications(
            ObjectSlot selectedSlot, ClickType click, bool shouldToggle, bool shouldExtendRange, bool? switchToObjTabNullable = null)
        {
            if (click == ClickType.ModelClick)
            {
                uint currentModelObjectAddress = Config.ModelManager.ModelObjectAddress;
                uint newModelObjectAddress = currentModelObjectAddress == selectedSlot.Address ? 0 : selectedSlot.Address;
                Config.ModelManager.ModelObjectAddress = newModelObjectAddress;
                Config.ModelManager.ManualMode = false;
            }
            else if (click == ClickType.CamHackClick)
            {
                uint currentCamHackSlot = Config.Stream.GetUInt32(CameraHackConfig.CameraHackStruct + CameraHackConfig.ObjectOffset);
                uint newCamHackSlot = currentCamHackSlot == selectedSlot.Address ? 0 : selectedSlot.Address;
                Config.Stream.SetValue(newCamHackSlot, CameraHackConfig.CameraHackStruct + CameraHackConfig.ObjectOffset);
            }
            else
            {
                List<uint> selection;
                switch (click)
                {
                    case ClickType.ObjectClick:
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

                bool switchToObjTab = switchToObjTabNullable ?? ShouldSwitchToObjTabByDefault();
                if (switchToObjTab)
                {
                    ManagerGui.TabControl.SelectedTab = ManagerGui.TabControl.TabPages["tabPageObjects"];
                }

                if (shouldExtendRange && selection.Count > 0)
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
                    if (!shouldToggle)
                    {
                        selection.Clear();
                    }

                    if (selection.Contains(selectedSlot.Address))
                    {
                        selection.Remove(selectedSlot.Address);
                        if (click == ClickType.ObjectClick)
                        {
                            _lastSelectedBehavior = null;
                        }
                    }
                    else
                    {
                        selection.Add(selectedSlot.Address);
                    }
                }

                if (click == ClickType.ObjectClick)
                {
                    Config.ObjectManager.CurrentAddresses = selection;
                }
            }
        }
 
        public string GetSlotNameFromAddress(uint address)
        {
            ObjectSlot slot = ObjectSlots.FirstOrDefault(s => s.Address == address);
            return slot?.Text;
        }

        public uint? GetSlotAddressFromName(string name)
        {
            if (name == null) return null;
            name = name.ToLower().Trim();
            ObjectSlot slot = ObjectSlots.FirstOrDefault(s => s.Text.ToLower() == name);
            return slot?.Address;
        }

        private List<ObjectSlotData> GetProcessedObjects()
        {
            var newObjectSlotData = new ObjectSlotData[ObjectSlotsConfig.MaxSlots];

            // Loop through each processing group
            int currentSlot = 0;
            foreach (var objectProcessGroup in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.FirstGroupingAddress + objectProcessGroup * ObjectSlotsConfig.ProcessGroupStructSize;

                // Calculate start and ending objects
                uint currentGroupObject = Config.Stream.GetUInt32(processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset);

                // Loop through every object within the group
                 while ((currentGroupObject != processGroupStructAddress && currentSlot < ObjectSlotsConfig.MaxSlots))
                {
                    // Validate current object
                    if (Config.Stream.GetUInt16(currentGroupObject + ObjectConfig.HeaderOffset) != 0x18)
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
                    currentGroupObject = Config.Stream.GetUInt32(currentGroupObject + ObjectConfig.ProcessedNextLinkOffset);

                    // Mark next slot
                    currentSlot++;
                }
            }

            var vacantSlotStart = currentSlot;

            // Now calculate vacant addresses
            uint currentObject = Config.Stream.GetUInt32(ObjectSlotsConfig.VactantPointerAddress);
            for (; currentSlot < ObjectSlotsConfig.MaxSlots; currentSlot++)
            {
                // Validate current object
                if (Config.Stream.GetUInt16(currentObject + ObjectConfig.HeaderOffset) != 0x18)
                    return null;

                newObjectSlotData[currentSlot] = new ObjectSlotData()
                {
                    Address = currentObject,
                    ObjectProcessGroup = VacantGroup,
                    ProcessIndex = currentSlot,
                    VacantSlotIndex = currentSlot - vacantSlotStart
                };

                currentObject = Config.Stream.GetUInt32(currentObject + ObjectConfig.ProcessedNextLinkOffset);
            }

            return newObjectSlotData.ToList();
        }

        public void Update()
        {
            LabelMethod = (SlotLabelType) ManagerGui.LabelMethodComboBox.SelectedItem;
            SortMethod = (SortMethodType) ManagerGui.SortMethodComboBox.SelectedItem;

            var newObjectSlotData = GetProcessedObjects();
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

            // Check behavior bank
            Config.ObjectAssociations.BehaviorBankStart = Config.Stream.GetUInt32(Config.ObjectAssociations.SegmentTable + 0x13 * 4);

            // Get mario position
            float marioX, marioY, marioZ;
            marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);

            // Calculate distance to Mario
            foreach (var objSlot in newObjectSlotData)
            {
                // Get object relative-to-maario position
                float dX, dY, dZ;
                dX = marioX - Config.Stream.GetSingle(objSlot.Address + ObjectConfig.XOffset);
                dY = marioY - Config.Stream.GetSingle(objSlot.Address + ObjectConfig.YOffset);
                dZ = marioZ - Config.Stream.GetSingle(objSlot.Address + ObjectConfig.ZOffset);

                objSlot.DistanceToMario = (float)Math.Sqrt(dX * dX + dY * dY + dZ * dZ);

                // Check if active/loaded
                objSlot.IsActive = Config.Stream.GetUInt16(objSlot.Address + ObjectConfig.ActiveOffset) != 0x0000;

                objSlot.Behavior = Config.Stream.GetUInt32(objSlot.Address + ObjectConfig.BehaviorScriptOffset) & 0x7FFFFFFF;
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

            _activeObjectCount = 0;

            _stoodOnObject = Config.Stream.GetUInt32(MarioConfig.StoodOnObjectPointerAddress);
            _interactionObject = Config.Stream.GetUInt32(MarioConfig.InteractionObjectPointerOffset + MarioConfig.StructAddress);
            _heldObject = Config.Stream.GetUInt32(MarioConfig.HeldObjectPointerOffset + MarioConfig.StructAddress);
            _usedObject = Config.Stream.GetUInt32(MarioConfig.UsedObjectPointerOffset + MarioConfig.StructAddress);
            _cameraObject = Config.Stream.GetUInt32(CameraConfig.SecondaryObjectAddress);
            _cameraHackObject = Config.Stream.GetUInt32(CameraHackConfig.CameraHackStruct + CameraHackConfig.ObjectOffset);
            _modelObject = Config.ModelManager.ModelObjectAddress;

            List<ObjectSlotData> closestObjectCandidates =
                newObjectSlotData.FindAll(s =>
                    s.IsActive &&
                    s.Behavior != (MarioObjectConfig.BehaviorValue & 0x00FFFFFF) + Config.ObjectAssociations.BehaviorBankStart);
            if (OptionsConfig.ExcludeDustForClosestObject)
            {
                closestObjectCandidates =
                    closestObjectCandidates.FindAll(s =>
                        s.Behavior != (ObjectConfig.DustSpawnerBehaviorValue & 0x00FFFFFF) + Config.ObjectAssociations.BehaviorBankStart &&
                        s.Behavior != (ObjectConfig.DustBallBehaviorValue & 0x00FFFFFF) + Config.ObjectAssociations.BehaviorBankStart &&
                        s.Behavior != (ObjectConfig.DustBehaviorValue & 0x00FFFFFF) + Config.ObjectAssociations.BehaviorBankStart);
            }
            _closestObject = closestObjectCandidates.OrderBy(s => s.DistanceToMario).FirstOrDefault()?.Address ?? 0;

            uint floorTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
            _floorObject = floorTriangleAddress == 0 ? 0 : Config.Stream.GetUInt32(floorTriangleAddress + TriangleOffsetsConfig.AssociatedObject);

            uint wallTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.WallTriangleOffset);
            _wallObject = wallTriangleAddress == 0 ? 0 : Config.Stream.GetUInt32(wallTriangleAddress + TriangleOffsetsConfig.AssociatedObject);

            uint ceilingTriangleAddress = Config.Stream.GetUInt32(MarioConfig.StructAddress + MarioConfig.CeilingTriangleOffset);
            _ceilingObject = ceilingTriangleAddress == 0 ? 0 : Config.Stream.GetUInt32(ceilingTriangleAddress + TriangleOffsetsConfig.AssociatedObject);

            ObjectSlot hoverObjectSlot = ObjectSlotsConfig.HoverObjectSlot;
            if (hoverObjectSlot != null)
            {
                _parentObject = Config.Stream.GetUInt32(hoverObjectSlot.Address + ObjectConfig.ParentOffset);
                _parentUnusedObject = _parentObject == ObjectSlotsConfig.UnusedSlotAddress ? hoverObjectSlot.Address : 0;
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

            for (int i = 0; i < ObjectSlotsConfig.MaxSlots; i++)
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

            if (SelectedSlotsAddresses.Count > 1)
            {
                if (_selectedUpdatePending)
                {
                    if (_lastSelectedBehavior != multiBehavior)
                    {
                        if (multiBehavior.HasValue)
                        {
                            Config.ObjectManager.Behavior = String.Format("0x{0}", multiBehavior.Value.BehaviorAddress.ToString("X4"));
                            Config.ObjectManager.SetBehaviorWatchVariables(
                                Config.ObjectAssociations.GetWatchVarControls(multiBehavior.Value),
                                ObjectSlotsConfig.VacantSlotColor.Lighten(0.8));
                        }
                        else
                        {
                            Config.ObjectManager.Behavior = "";
                            Config.ObjectManager.SetBehaviorWatchVariables(new List<WatchVariableControl>(), Color.White);
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
                                Image image = Config.ObjectAssociations.GetObjectImage(selectedBehaviorCriterias[index], false);
                                Rectangle rect = new Rectangle(col * imageSize, row * imageSize, imageSize, imageSize);
                                Rectangle zoomedRect = rect.Zoom(image.Size);
                                gfx.DrawImage(image, zoomedRect);
                            }
                        }
                    }
                    _multiImage = multiBitmap;
                    Config.ObjectManager.Image = _multiImage;

                    Config.ObjectManager.Name = SelectedSlotsAddresses.Count + " Objects Selected";
                    Config.ObjectManager.BackColor = ObjectSlotsConfig.VacantSlotColor;
                    Config.ObjectManager.SlotIndex = "";
                    Config.ObjectManager.SlotPos = "";

                    _selectedUpdatePending = false;
                }
            }
            else if (SelectedSlotsAddresses.Count == 0)
            {
                Config.ObjectManager.Name = "No Object Selected";
                Config.ObjectManager.BackColor = ObjectSlotsConfig.VacantSlotColor;
                Config.ObjectManager.Behavior = "";
                Config.ObjectManager.SlotIndex = "";
                Config.ObjectManager.SlotPos = "";
                Config.ObjectManager.Image = null;
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
            objSlot.DrawStoodOnOverlay = OverlayConfig.ShowOverlayStoodOnObject && objAddress == _stoodOnObject;
            objSlot.DrawInteractionOverlay = OverlayConfig.ShowOverlayInteractionObject && objAddress == _interactionObject;
            objSlot.DrawHeldOverlay = OverlayConfig.ShowOverlayHeldObject && objAddress == _heldObject;
            objSlot.DrawUsedOverlay = OverlayConfig.ShowOverlayUsedObject && objAddress == _usedObject;
            objSlot.DrawClosestOverlay = OverlayConfig.ShowOverlayClosestObject && objAddress == _closestObject;
            objSlot.DrawCameraOverlay = OverlayConfig.ShowOverlayCameraObject && objAddress == _cameraObject;
            objSlot.DrawCameraHackOverlay = OverlayConfig.ShowOverlayCameraHackObject && objAddress == _cameraHackObject;
            objSlot.DrawModelOverlay = objAddress == _modelObject;
            objSlot.DrawFloorOverlay = OverlayConfig.ShowOverlayFloorObject && objAddress == _floorObject;
            objSlot.DrawWallOverlay = OverlayConfig.ShowOverlayWallObject && objAddress == _wallObject;
            objSlot.DrawCeilingOverlay = OverlayConfig.ShowOverlayCeilingObject && objAddress == _ceilingObject;
            objSlot.DrawParentOverlay = OverlayConfig.ShowOverlayParentObject && objAddress == _parentObject;
            objSlot.DrawParentUnusedOverlay = OverlayConfig.ShowOverlayParentObject && objAddress == _parentUnusedObject;
            objSlot.DrawParentNoneOverlay = OverlayConfig.ShowOverlayParentObject && objAddress == _parentNoneObject;
            objSlot.DrawMarkedOverlay = MarkedSlotsAddresses.Contains(objAddress);

            if (objData.IsActive)
                _activeObjectCount++;

            var gfxId = Config.Stream.GetUInt32(objAddress + ObjectConfig.BehaviorGfxOffset);
            var subType = Config.Stream.GetInt32(objAddress + ObjectConfig.BehaviorSubtypeOffset);
            var appearance = Config.Stream.GetInt32(objAddress + ObjectConfig.BehaviorAppearanceOffset);

            uint segmentedBehavior = 0x13000000 + objData.Behavior - Config.ObjectAssociations.BehaviorBankStart;
            if (objData.Behavior == 0) // uninitialized object
                segmentedBehavior = 0;
            behaviorCriteria = new BehaviorCriteria()
            {
                BehaviorAddress = Config.SwitchRomVersion(segmentedBehavior, Config.ObjectAssociations.AlignJPBehavior(segmentedBehavior)),
                GfxId = gfxId,
                SubType = subType,
                Appearance = appearance
            };

            if (Config.ObjectAssociations.RecognizedBehavior(behaviorCriteria))
            {
                objSlot.Behavior = behaviorCriteria;
            }
            else
            {
                behaviorCriteria = objSlot.Behavior; // skip update, bad behavior
            }
            var processGroup = objData.ObjectProcessGroup;
            objSlot.ProcessGroup = processGroup;

            var newColor = objData.ObjectProcessGroup == VacantGroup ? ObjectSlotsConfig.VacantSlotColor :
                ObjectSlotsConfig.ProcessingGroupsColor[objData.ObjectProcessGroup];
            objSlot.BackColor = newColor;

            if (!_labelsLocked)
                _lastSlotLabel[objAddress] = new Tuple<int?, int?>(objData.ProcessIndex, objData.VacantSlotIndex);

            string labelText = "";
            switch (LabelMethod)
            {
                case SlotLabelType.Recommended:
                    if (SortMethod == SortMethodType.MemoryOrder)
                        goto case SlotLabelType.SlotIndex;
                    goto case SlotLabelType.SlotPosVs;

                case SlotLabelType.SlotIndex:
                    labelText = String.Format("{0}", (objData.Address - ObjectSlotsConfig.LinkStartAddress)
                        / ObjectConfig.StructSize + (OptionsConfig.SlotIndexsFromOne ? 1 : 0));
                    break;

                case SlotLabelType.SlotPos:
                    labelText = String.Format("{0}", _lastSlotLabel[objAddress].Item1
                        + (OptionsConfig.SlotIndexsFromOne ? 1 : 0));
                    break;

                case SlotLabelType.SlotPosVs:
                    var vacantSlotIndex = _lastSlotLabel[objAddress].Item2;
                    if (!vacantSlotIndex.HasValue)
                        goto case SlotLabelType.SlotPos;

                    labelText = String.Format("VS{0}", vacantSlotIndex.Value
                        + (OptionsConfig.SlotIndexsFromOne ? 1 : 0));
                    break;
            }

            objSlot.TextColor = _labelsLocked ? Color.Blue : Color.Black;
            objSlot.Text = labelText;

            // Update object manager image
            if (SelectedSlotsAddresses.Count <= 1 && SelectedSlotsAddresses.Contains(objAddress))
            {
                UpdateObjectManager(objSlot, behaviorCriteria, objData);
            }

            // Update the map
            UpdateMapObject(objData, objSlot, behaviorCriteria);
        }

        void UpdateObjectManager(ObjectSlot objSlot, BehaviorCriteria behaviorCriteria, ObjectSlotData objData)
        {
            var objAssoc = Config.ObjectAssociations.FindObjectAssociation(behaviorCriteria);
            var newBehavior = objAssoc != null ? objAssoc.BehaviorCriteria : behaviorCriteria;
            if (_lastSelectedBehavior != newBehavior || SelectedSlotsAddresses.Count == 0)
            {
                Config.ObjectManager.Behavior = String.Format("0x{0}", (behaviorCriteria.BehaviorAddress & 0xffffff).ToString("X4"));
                Config.ObjectManager.Name = Config.ObjectAssociations.GetObjectName(behaviorCriteria);

                Config.ObjectManager.SetBehaviorWatchVariables(Config.ObjectAssociations.GetWatchVarControls(behaviorCriteria), objSlot.BackColor.Lighten(0.8));
                _lastSelectedBehavior = newBehavior;
            }
            Config.ObjectManager.Image = objSlot.ObjectImage;
            Config.ObjectManager.BackColor = objSlot.BackColor;
            int slotPos = objData.ObjectProcessGroup == VacantGroup ? objData.VacantSlotIndex.Value : objData.ProcessIndex;
            Config.ObjectManager.SlotIndex = (_memoryAddressSlotIndex[objData.Address] + (OptionsConfig.SlotIndexsFromOne ? 1 : 0)).ToString();
            Config.ObjectManager.SlotPos = (objData.ObjectProcessGroup == VacantGroup ? "VS " : "")
                + (slotPos + (OptionsConfig.SlotIndexsFromOne ? 1 : 0)).ToString();
        }

        void UpdateMapObject(ObjectSlotData objData, ObjectSlot objSlot, BehaviorCriteria behaviorCriteria)
        {
            if (ActiveTab != TabType.Map || !Config.MapManager.IsLoaded)
                return;

            var objAddress = objData.Address;

            // Update image
            var mapObjImage = Config.ObjectAssociations.GetObjectMapImage(behaviorCriteria);
            var mapObjRotates = Config.ObjectAssociations.GetObjectMapRotates(behaviorCriteria);
            if (!_mapObjects.ContainsKey(objAddress))
            {
                var mapObj = new MapObject(mapObjImage);
                mapObj.UsesRotation = mapObjRotates;
                _mapObjects.Add(objAddress, mapObj);
                Config.MapManager.AddMapObject(mapObj);
            }
            else if (_mapObjects[objAddress].Image != mapObjImage)
            {
                Config.MapManager.RemoveMapObject(_mapObjects[objAddress]);
                var mapObj = new MapObject(mapObjImage);
                mapObj.UsesRotation = mapObjRotates;
                _mapObjects[objAddress] = mapObj;
                Config.MapManager.AddMapObject(mapObj);
            }

            if (objData.Behavior == (Config.ObjectAssociations.MarioBehavior & 0x00FFFFFF) + Config.ObjectAssociations.BehaviorBankStart)
            {
                _mapObjects[objAddress].Show = false;
            }
            else
            {
                // Update map object coordinates and rotation
                var mapObj = _mapObjects[objAddress];
                mapObj.Show = SelectedOnMapSlotsAddresses.Contains(objAddress);
                objSlot.Show = _mapObjects[objAddress].Show;
                mapObj.X = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                mapObj.Y = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                mapObj.Z = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                mapObj.IsActive = objData.IsActive;
                mapObj.Transparent = !mapObj.IsActive;
                ushort objYaw = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                mapObj.Rotation = (float)MoreMath.AngleUnitsToDegrees(objYaw);
                mapObj.UsesRotation = Config.ObjectAssociations.GetObjectMapRotates(behaviorCriteria);
            }
        }
    }
}
 