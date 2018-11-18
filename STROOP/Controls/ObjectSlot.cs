using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Managers;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Controls;
using STROOP.Extensions;
using System.Drawing.Drawing2D;
using STROOP.Structs.Configurations;
using STROOP.Models;
using static STROOP.Managers.ObjectSlotsManager;
using System.Windows.Input;
using STROOP.Map2;

namespace STROOP
{
    public class ObjectSlot : Panel
    {
        const int BorderSize = 2;

        ObjectSlotsManager _manager;
        ObjectSlotManagerGui _gui;

        public int Index { get; private set; }
        public ObjectDataModel CurrentObject { get; set; }

        #region Drawing Variables
        Color _mainColor, _borderColor, _backColor;
        SolidBrush _borderBrush = new SolidBrush(Color.White), _backBrush = new SolidBrush(Color.White);
        SolidBrush _textBrush = new SolidBrush(Color.Black);
        Image _objectImage;
        Image _bufferedObjectImage;
        Point _textLocation = new Point();
        Point _objectImageLocation = new Point();
        string _text;
        int _fontHeight;
        #endregion

        public new bool Show = false;

        enum SelectionType { NOT_SELECTED, NORMAL_SELECTION, MAP_SELECTION, MAP2_SELECTION, MODEL_SELECTION };
        SelectionType _selectionType = SelectionType.NOT_SELECTED;

        int prevHeight;
        object _gfxLock = new object();

        public enum MouseStateType {None, Over, Down};
        private MouseStateType _mouseState;
        private MouseStateType _mouseEnteredState;

        private BehaviorCriteria _behavior;
        public BehaviorCriteria Behavior
        {
            get
            {
                return _behavior;
            }
        }

        bool _isActive = false;

        private bool IsHovering;

        public override string Text => _text;
        Color _textColor
        {
            get => _textBrush.Color;
            set { lock (_gfxLock) { _textBrush.Color = value; } }
        }

        bool _drawSelectedOverlay, _drawStoodOnOverlay, _drawRiddenOverlay, _drawHeldOverlay, _drawInteractionOverlay, _drawUsedOverlay,
            _drawClosestOverlay, _drawCameraOverlay, _drawCameraHackOverlay, _drawModelOverlay,
            _drawFloorOverlay, _drawWallOverlay, _drawCeilingOverlay,
            _drawParentOverlay, _drawParentUnusedOverlay, _drawParentNoneOverlay, _drawChildOverlay,
            _drawCollision1Overlay, _drawCollision2Overlay, _drawCollision3Overlay, _drawCollision4Overlay,
            _drawMarkedOverlay;

        public ObjectSlot(ObjectSlotsManager manager, int index, ObjectSlotManagerGui gui, Size size)
        {
            _manager = manager;
            _gui = gui;
            Size = size;
            Index = index;
            Font = new Font(FontFamily.GenericSansSerif, 6);

            this.MouseDown += OnDrag;
            this.MouseUp += (s, e) => { _mouseState = _mouseEnteredState; UpdateColors(); };
            this.MouseEnter += (s, e) =>
            {
                IsHovering = true;
                _manager.HoveredObjectAdress = CurrentObject?.Address;
                _mouseEnteredState = MouseStateType.Over;
                _mouseState = MouseStateType.Over;
                UpdateColors();
            };
            this.MouseLeave += (s, e) =>
            {
                IsHovering = false;
                _manager.HoveredObjectAdress = null;
                _mouseEnteredState = MouseStateType.None;
                _mouseState = MouseStateType.None;
                UpdateColors();
            };
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DoubleBuffered = true;

            SetUpContextMenuStrip();
        }

        private void SetUpContextMenuStrip()
        {
            ToolStripMenuItem itemSelectInObjectTab = new ToolStripMenuItem("Select in Object Tab");
            itemSelectInObjectTab.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.DoSlotClickUsingSpecifications(
                    this, ClickType.ObjectClick, false, false, TabDestinationType.Object);
            };

            ToolStripMenuItem itemSelectInMemoryTab = new ToolStripMenuItem("Select in Memory Tab");
            itemSelectInMemoryTab.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.DoSlotClickUsingSpecifications(
                    this, ClickType.MemoryClick, false, false, TabDestinationType.Memory);
            };

            Func<List<ObjectDataModel>> getObjects = () => KeyboardUtilities.IsCtrlHeld()
                ? Config.ObjectSlotsManager.SelectedObjects
                : new List<ObjectDataModel>() { CurrentObject };

            ToolStripMenuItem itemGoto = new ToolStripMenuItem("Go to");
            itemGoto.Click += (sender, e) => ButtonUtilities.GotoObjects(getObjects());

            ToolStripMenuItem itemRetrieve = new ToolStripMenuItem("Retrieve");
            itemRetrieve.Click += (sender, e) => ButtonUtilities.RetrieveObjects(getObjects());

            ToolStripMenuItem itemGotoHome = new ToolStripMenuItem("Go to Home");
            itemGotoHome.Click += (sender, e) => ButtonUtilities.GotoObjectsHome(getObjects());

            ToolStripMenuItem itemRetrieveHome = new ToolStripMenuItem("Retrieve Home");
            itemRetrieveHome.Click += (sender, e) => ButtonUtilities.RetrieveObjectsHome(getObjects());

            ToolStripMenuItem itemRelease = new ToolStripMenuItem("Release");
            itemRelease.Click += (sender, e) => ButtonUtilities.ReleaseObject(getObjects());

            ToolStripMenuItem itemUnRelease = new ToolStripMenuItem("UnRelease");
            itemUnRelease.Click += (sender, e) => ButtonUtilities.UnReleaseObject(getObjects());

            ToolStripMenuItem itemInteract = new ToolStripMenuItem("Interact");
            itemInteract.Click += (sender, e) => ButtonUtilities.ReleaseObject(getObjects());

            ToolStripMenuItem itemUnInteract = new ToolStripMenuItem("UnInteract");
            itemUnInteract.Click += (sender, e) => ButtonUtilities.UnInteractObject(getObjects());

            ToolStripMenuItem itemClone = new ToolStripMenuItem("Clone");
            itemClone.Click += (sender, e) => ButtonUtilities.CloneObject(CurrentObject);

            ToolStripMenuItem itemUnClone = new ToolStripMenuItem("UnClone");
            itemUnClone.Click += (sender, e) => ButtonUtilities.UnCloneObject();

            ToolStripMenuItem itemUnload = new ToolStripMenuItem("Unload");
            itemUnload.Click += (sender, e) => ButtonUtilities.UnloadObject(getObjects());

            ToolStripMenuItem itemRevive = new ToolStripMenuItem("Revive");
            itemRevive.Click += (sender, e) => ButtonUtilities.ReviveObject(getObjects());

            ToolStripMenuItem itemRide = new ToolStripMenuItem("Ride");
            itemRide.Click += (sender, e) => ButtonUtilities.RideObject(CurrentObject);

            ToolStripMenuItem itemCopyAddress = new ToolStripMenuItem("Copy Address");
            itemCopyAddress.Click += (sender, e) => Clipboard.SetText(HexUtilities.FormatValue(CurrentObject.Address));

            ToolStripMenuItem itemCopyPosition = new ToolStripMenuItem("Copy Position");
            itemCopyPosition.Click += (sender, e) => Clipboard.SetText(
                String.Format("{0},{1},{2}", CurrentObject.X, CurrentObject.Y, CurrentObject.Z));

            ToolStripMenuItem itemPastePosition = new ToolStripMenuItem("Paste Position");
            itemPastePosition.Click += (sender, e) =>
            {
                List<string> stringList = ParsingUtilities.ParseStringList(Clipboard.GetText());
                int count = stringList.Count;
                if (count != 2 && count != 3) return;
                getObjects().ForEach(obj =>
                {
                    if (obj == null) return;

                    List<float?> floatList = stringList.ConvertAll(s => ParsingUtilities.ParseFloatNullable(s));
                    Config.Stream.Suspend();
                    if (count == 2)
                    {
                        if (floatList[0].HasValue) obj.X = floatList[0].Value;
                        if (floatList[1].HasValue) obj.Z = floatList[1].Value;
                    }
                    else
                    {
                        if (floatList[0].HasValue) obj.X = floatList[0].Value;
                        if (floatList[1].HasValue) obj.Y = floatList[1].Value;
                        if (floatList[2].HasValue) obj.Z = floatList[2].Value;
                    }
                    Config.Stream.Resume();
                });
            };

            ToolStripMenuItem itemCopyGraphics = new ToolStripMenuItem("Copy Graphics");
            itemCopyGraphics.Click += (sender, e) => Clipboard.SetText(HexUtilities.FormatValue(CurrentObject.GraphicsID));

            ToolStripMenuItem itemPasteGraphics = new ToolStripMenuItem("Paste Graphics");
            itemPasteGraphics.Click += (sender, e) =>
            {
                uint? address = ParsingUtilities.ParseHexNullable(Clipboard.GetText());
                if (!address.HasValue) return;
                getObjects().ForEach(obj =>
                {
                    obj.GraphicsID = address.Value;
                });
            };

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add(itemSelectInObjectTab);
            ContextMenuStrip.Items.Add(itemSelectInMemoryTab);
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(itemGoto);
            ContextMenuStrip.Items.Add(itemRetrieve);
            ContextMenuStrip.Items.Add(itemGotoHome);
            ContextMenuStrip.Items.Add(itemRetrieveHome);
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(itemRelease);
            ContextMenuStrip.Items.Add(itemUnRelease);
            ContextMenuStrip.Items.Add(itemInteract);
            ContextMenuStrip.Items.Add(itemUnInteract);
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(itemClone);
            ContextMenuStrip.Items.Add(itemUnClone);
            ContextMenuStrip.Items.Add(itemUnload);
            ContextMenuStrip.Items.Add(itemRevive);
            ContextMenuStrip.Items.Add(itemRide);
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(itemCopyAddress);
            ContextMenuStrip.Items.Add(itemCopyPosition);
            ContextMenuStrip.Items.Add(itemPastePosition);
            ContextMenuStrip.Items.Add(itemCopyGraphics);
            ContextMenuStrip.Items.Add(itemPasteGraphics);
        }

        private void RebufferObjectImage()
        {
            // Remove last image reference
            _bufferedObjectImage = null;

            // Make sure object needs a new image
            if (_objectImage == null)
                return;

            // Calculate new rectangle to draw image
            var objectImageRec = (new Rectangle(BorderSize, BorderSize + 1,
            Width - BorderSize * 2, _textLocation.Y - 1 - BorderSize))
            .Zoom(_objectImage.Size);
            _objectImageLocation = objectImageRec.Location;

            // If the image is too small, we don't need to draw it
            if (objectImageRec.Height <= 0 || objectImageRec.Width <= 0)
            {
                _bufferedObjectImage = new Bitmap(1, 1);
                return;
            }

            // Look for cached image and use it if it exists
            _bufferedObjectImage = Config.ObjectAssociations.GetCachedBufferedObjectImage(_objectImage, objectImageRec.Size);
            if (_bufferedObjectImage != null)
                return;

            // Otherwise create new image and add it to cache
            _bufferedObjectImage = new Bitmap(objectImageRec.Width, objectImageRec.Height);
            objectImageRec.Location = new Point();
            using (var graphics = Graphics.FromImage(_bufferedObjectImage))
            {
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.DrawImage(_objectImage, objectImageRec);
            }

            Config.ObjectAssociations.CreateCachedBufferedObjectImage(_objectImage, _bufferedObjectImage);
        }

        public bool UpdateColors()
        {
            var oldBorderColor = _borderColor;
            var oldBackColor = _backColor;
            bool imageUpdated = false;
            var newColor = _mainColor;
            switch (_mouseState)
            {
                case MouseStateType.Down:
                    _borderColor = newColor.Darken(0.5);
                    _backColor = newColor.Darken(0.5).Lighten(0.5);
                    break;
                case MouseStateType.Over:
                    _borderColor = newColor.Lighten(0.5);
                    _backColor = newColor.Lighten(0.85);
                    break;
                default:
                    _borderColor = newColor;
                    _backColor = newColor.Lighten(0.7);
                    break;
            }
            Image newImage = Config.ObjectAssociations.GetObjectImage(_behavior, !_isActive);
            if (_objectImage != newImage)
            {
                lock (_gfxLock)
                {
                    _objectImage = newImage;
                    RebufferObjectImage();
                }
                imageUpdated = true;
            }

            bool colorUpdated = false;
            colorUpdated |= (_backColor != oldBackColor);
            colorUpdated |= (_borderColor != oldBorderColor);

            if (colorUpdated)
            {
                lock (_gfxLock)
                {
                    _borderBrush.Color = _borderColor;
                    _backBrush.Color = _backColor;
                }
            }


            if (!imageUpdated && !colorUpdated)
                return false;

            Invalidate();
            return true;
        }

        private void OnDrag(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            _mouseState = MouseStateType.Down;
            UpdateColors();
        }

        private List<bool> GetCurrentOverlayValues()
        {
            return new List<bool>()
            {
                _drawSelectedOverlay,
                _drawStoodOnOverlay,
                _drawRiddenOverlay,
                _drawInteractionOverlay,
                _drawHeldOverlay,
                _drawUsedOverlay,
                _drawClosestOverlay,
                _drawCameraOverlay,
                _drawCameraHackOverlay,
                _drawModelOverlay,
                _drawWallOverlay,
                _drawFloorOverlay,
                _drawCeilingOverlay,
                _drawParentOverlay,
                _drawParentUnusedOverlay,
                _drawParentNoneOverlay,
                _drawChildOverlay,
                _drawCollision1Overlay,
                _drawCollision2Overlay,
                _drawCollision3Overlay,
                _drawCollision4Overlay,
                _drawMarkedOverlay,
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.CompositingQuality = CompositingQuality.HighSpeed;
            lock (_gfxLock)
            {
                // Border
                e.Graphics.FillRectangle(_borderBrush, new Rectangle(new Point(), Size));

                // Background
                e.Graphics.FillRectangle(_backBrush, new Rectangle(BorderSize, BorderSize, Width - BorderSize * 2, Height - BorderSize * 2));

                // Change font size
                if (Height != prevHeight)
                {
                    prevHeight = Height;
                    Font?.Dispose();
                    Font = new Font(FontFamily.GenericSansSerif, Math.Max(6, 6 / 40.0f * Height));

                    // Font.Height doesn't work for some reason that probably makes sense, but don't really want to look into right now
                    _fontHeight = TextRenderer.MeasureText(e.Graphics, "ABCDEF", Font).Height;
                }

                // Draw Text
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
                var textLocation = new Point(Width + 1, Height - BorderSize - _fontHeight + 1);
                TextRenderer.DrawText(e.Graphics, _text, Font, textLocation, _textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
                if (textLocation != _textLocation)
                {
                    _textLocation = textLocation;
                    RebufferObjectImage();
                }

                // Draw Object Image
                if (_objectImage != null)
                {
                    try
                    {
                        e.Graphics.DrawImageUnscaled(_bufferedObjectImage, _objectImageLocation);
                    }
                    catch (ObjectDisposedException)
                    {
                        // The buffered image may have gotten disposed
                        RebufferObjectImage();
                        Invalidate();
                        return;
                    }
                }
            }

            // TODO reorder object slots overlays
            // Draw Overlays
            if (_drawMarkedOverlay)
                e.Graphics.DrawImage(_gui.MarkedObjectOverlayImage, new Rectangle(new Point(), Size));
            switch (_selectionType)
            {
                case SelectionType.NORMAL_SELECTION:
                    e.Graphics.DrawImage(_gui.SelectedObjectOverlayImage, new Rectangle(new Point(), Size));
                    break;

                case SelectionType.MODEL_SELECTION:
                    e.Graphics.DrawImage(_gui.ModelObjectOverlayImage, new Rectangle(new Point(), Size));
                    break;

                case SelectionType.MAP_SELECTION:
                    e.Graphics.DrawImage(_gui.TrackedAndShownObjectOverlayImage, new Rectangle(new Point(), Size));
                    break;

                case SelectionType.MAP2_SELECTION:
                    e.Graphics.DrawImage(_gui.TrackedAndShownObjectOverlayImage, new Rectangle(new Point(), Size));
                    break;

                case SelectionType.NOT_SELECTED:
                    // do nothing
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (_drawWallOverlay)
                e.Graphics.DrawImage(_gui.WallObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawFloorOverlay)
                e.Graphics.DrawImage(_gui.FloorObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCeilingOverlay)
                e.Graphics.DrawImage(_gui.CeilingObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawInteractionOverlay)
                e.Graphics.DrawImage(_gui.InteractionObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawHeldOverlay)
                e.Graphics.DrawImage(_gui.HeldObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawStoodOnOverlay)
                e.Graphics.DrawImage(_gui.StoodOnObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawRiddenOverlay)
                e.Graphics.DrawImage(_gui.RiddenObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawUsedOverlay)
                e.Graphics.DrawImage(_gui.UsedObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawClosestOverlay)
                e.Graphics.DrawImage(_gui.ClosestObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCameraOverlay)
                e.Graphics.DrawImage(_gui.CameraObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCameraHackOverlay)
                e.Graphics.DrawImage(_gui.CameraHackObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawParentOverlay)
                e.Graphics.DrawImage(_gui.ParentObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawParentUnusedOverlay)
                e.Graphics.DrawImage(_gui.ParentUnusedObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawParentNoneOverlay)
                e.Graphics.DrawImage(_gui.ParentNoneObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawChildOverlay)
                e.Graphics.DrawImage(_gui.ChildObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCollision1Overlay)
                e.Graphics.DrawImage(_gui.Collision1OverlayImage, new Rectangle(new Point(), Size));
            if (_drawCollision2Overlay)
                e.Graphics.DrawImage(_gui.Collision2OverlayImage, new Rectangle(new Point(), Size));
            if (_drawCollision3Overlay)
                e.Graphics.DrawImage(_gui.Collision3OverlayImage, new Rectangle(new Point(), Size));
            if (_drawCollision4Overlay)
                e.Graphics.DrawImage(_gui.Collision4OverlayImage, new Rectangle(new Point(), Size));
        }

        public void Update(ObjectDataModel obj)
        {
            CurrentObject = obj;

            uint? address = CurrentObject?.Address;

            // Update Overlays
            List<bool> prevOverlays = GetCurrentOverlayValues();
            if (address.HasValue)
            {
                _drawSelectedOverlay = _manager.SelectedSlotsAddresses.Contains(address.Value);
                _drawStoodOnOverlay = OverlayConfig.ShowOverlayStoodOnObject && address == DataModels.Mario.StoodOnObject;
                _drawRiddenOverlay = OverlayConfig.ShowOverlayRiddenObject && address == DataModels.Mario.RiddenObject;
                _drawInteractionOverlay = OverlayConfig.ShowOverlayInteractionObject && address == DataModels.Mario.InteractionObject;
                _drawHeldOverlay = OverlayConfig.ShowOverlayHeldObject && address == DataModels.Mario.HeldObject;
                _drawUsedOverlay = OverlayConfig.ShowOverlayUsedObject && address == DataModels.Mario.UsedObject;
                _drawClosestOverlay = OverlayConfig.ShowOverlayClosestObject && address == DataModels.Mario.ClosestObject;
                _drawCameraOverlay = OverlayConfig.ShowOverlayCameraObject && address == DataModels.Camera.SecondaryObject;
                _drawCameraHackOverlay = OverlayConfig.ShowOverlayCameraHackObject && address == DataModels.Camera.HackObject;
                _drawModelOverlay = address == Config.ModelManager.ModelObjectAddress;
                _drawWallOverlay = OverlayConfig.ShowOverlayWallObject && address == DataModels.Mario.WallTriangle?.AssociatedObject;
                _drawFloorOverlay = OverlayConfig.ShowOverlayFloorObject && address == DataModels.Mario.FloorTriangle?.AssociatedObject;
                _drawCeilingOverlay = OverlayConfig.ShowOverlayCeilingObject && address == DataModels.Mario.CeilingTriangle?.AssociatedObject;

                uint? hoveredAddress = Config.ObjectSlotsManager.HoveredObjectAdress;
                if (hoveredAddress.HasValue)
                {
                    ObjectDataModel hoveredObject = new ObjectDataModel(hoveredAddress.Value);

                    _drawParentOverlay = (OverlayConfig.ShowOverlayParentObject || Keyboard.IsKeyDown(Key.P)) &&
                        address == hoveredObject.Parent;
                    _drawParentNoneOverlay = (OverlayConfig.ShowOverlayParentObject || Keyboard.IsKeyDown(Key.P)) &&
                        address == hoveredObject.Address &&
                        hoveredObject.Parent == 0;
                    _drawParentUnusedOverlay = (OverlayConfig.ShowOverlayParentObject || Keyboard.IsKeyDown(Key.P)) &&
                        address == hoveredObject.Address &&
                        hoveredObject.Parent == ObjectSlotsConfig.UnusedSlotAddress;
                    _drawChildOverlay = (OverlayConfig.ShowOverlayChildObject || Keyboard.IsKeyDown(Key.P)) &&
                        CurrentObject?.Parent == hoveredObject.Address;
                }

                uint collisionObjAddress = hoveredAddress.HasValue && Keyboard.IsKeyDown(Key.C)
                    ? hoveredAddress.Value : Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress);
                _drawCollision1Overlay = OverlayConfig.ShowOverlayCollisionObject &&
                    address == ObjectUtilities.GetCollisionObject(collisionObjAddress, 1);
                _drawCollision2Overlay = OverlayConfig.ShowOverlayCollisionObject && 
                    address == ObjectUtilities.GetCollisionObject(collisionObjAddress, 2);
                _drawCollision3Overlay = OverlayConfig.ShowOverlayCollisionObject && 
                    address == ObjectUtilities.GetCollisionObject(collisionObjAddress, 3);
                _drawCollision4Overlay = OverlayConfig.ShowOverlayCollisionObject && 
                    address == ObjectUtilities.GetCollisionObject(collisionObjAddress, 4);

                _drawMarkedOverlay = _manager.MarkedSlotsAddresses.Contains(address.Value);
            }
            else
            {
                _drawSelectedOverlay = false;
                _drawStoodOnOverlay = false;
                _drawRiddenOverlay = false;
                _drawInteractionOverlay = false;
                _drawHeldOverlay = false;
                _drawUsedOverlay = false;
                _drawClosestOverlay = false;
                _drawCameraOverlay = false;
                _drawCameraHackOverlay = false;
                _drawModelOverlay = false;
                _drawWallOverlay = false;
                _drawFloorOverlay = false;
                _drawCeilingOverlay = false;
                _drawParentOverlay = false;
                _drawParentUnusedOverlay = false;
                _drawParentNoneOverlay = false;
                _drawChildOverlay = false;
                _drawCollision1Overlay = false;
                _drawCollision2Overlay = false;
                _drawCollision3Overlay = false;
                _drawCollision4Overlay = false;
                _drawMarkedOverlay = false;
            }
            List<bool> overlays = GetCurrentOverlayValues();

            SelectionType selectionType;
            switch (_manager.ActiveTab)
            {
                case ObjectSlotsManager.TabType.Map:
                    selectionType = address.HasValue && Config.ObjectSlotsManager.SelectedOnMapSlotsAddresses.Contains(address.Value)
                        ? SelectionType.MAP_SELECTION
                        : SelectionType.NOT_SELECTED;
                    break;

                case ObjectSlotsManager.TabType.Map2:
                    selectionType = Show ? SelectionType.MAP2_SELECTION
                        : SelectionType.NOT_SELECTED;
                    break;

                case ObjectSlotsManager.TabType.Model:
                    selectionType = CurrentObject?.Address == Config.ModelManager.ModelObjectAddress
                        ? SelectionType.MODEL_SELECTION : SelectionType.NOT_SELECTED;
                    break;

                case ObjectSlotsManager.TabType.CamHack:
                    selectionType = SelectionType.NOT_SELECTED;
                    break;

                default:
                    selectionType = CurrentObject != null && _manager.SelectedSlotsAddresses.Contains(CurrentObject.Address)
                        ? SelectionType.NORMAL_SELECTION : SelectionType.NOT_SELECTED;
                    break;
            }

            Color mainColor = ObjectSlotsConfig.GetProcessingGroupColor(CurrentObject?.CurrentProcessGroup);
            Color textColor = _manager.LabelsLocked ? Color.Blue : Color.Black;
            string text = CurrentObject != null ? _manager.SlotLabelsForObjects[CurrentObject] : "";

            // Update UI element

            bool updateColors = false;
            bool redraw = false;

            if (text != _text)
            {
                _text = text;
                redraw = true;
            }
            if (textColor != _textColor)
            {
                _textColor = textColor;
                redraw = true;
            }
            if (mainColor != _mainColor)
            {
                _mainColor = mainColor;
                updateColors = true;
            }

            if (_selectionType != selectionType)
            {
                _selectionType = selectionType;
                redraw = true;
            }

            if (_behavior != (CurrentObject?.BehaviorCriteria ?? default(BehaviorCriteria)))
            {
                _behavior = CurrentObject?.BehaviorCriteria ?? default(BehaviorCriteria);
                updateColors = true;
            }
            if (_isActive != (CurrentObject?.IsActive ?? false))
            {
                _isActive = CurrentObject?.IsActive ?? false;
                updateColors = true;
            }         
            if (!overlays.SequenceEqual(prevOverlays))
                redraw = true;

            if (updateColors)
            {
                if (UpdateColors())
                    redraw = false; // UpdateColors already calls refresh
            }

            if (redraw)
                Invalidate();

            UpdateMapObject();
        }

        private void UpdateMapObject()
        {
            if (!Config.Map2Manager.IsLoaded || CurrentObject == null)
                return;

            Dictionary<uint, Map2Object> _mapObjects = Config.Map2Manager._mapObjectDictionary;
            BehaviorCriteria behaviorCriteria = CurrentObject.BehaviorCriteria;
            uint objAddress = CurrentObject.Address;

            // Update image
            var mapObjImage = Config.ObjectAssociations.GetObjectMapImage(behaviorCriteria);
            var mapObjRotates = Config.ObjectAssociations.GetObjectMapRotates(behaviorCriteria);
            if (!_mapObjects.ContainsKey(objAddress))
            {
                var mapObj = new Map2Object(mapObjImage);
                mapObj.UsesRotation = mapObjRotates;
                _mapObjects.Add(objAddress, mapObj);
                Config.Map2Manager.AddMapObject(mapObj);
            }
            else if (_mapObjects[objAddress].Image != mapObjImage)
            {
                Config.Map2Manager.RemoveMapObject(_mapObjects[objAddress]);
                var mapObj = new Map2Object(mapObjImage);
                mapObj.UsesRotation = mapObjRotates;
                _mapObjects[objAddress] = mapObj;
                Config.Map2Manager.AddMapObject(mapObj);
            }
            
            if (CurrentObject.SegmentedBehavior ==
                   (Config.ObjectAssociations.MarioBehavior & 0x00FFFFFF) +
                   Config.ObjectAssociations.BehaviorBankStart)
            {
                _mapObjects[objAddress].Show = false;
            }
            else
            {
                // Update map object coordinates and rotation
                var mapObj = _mapObjects[objAddress];
                mapObj.Show = Config.ObjectSlotsManager.SelectedOnMap2SlotsAddresses.Contains(objAddress);
                Show = _mapObjects[objAddress].Show;
                mapObj.X = Config.Stream.GetSingle(objAddress + ObjectConfig.XOffset);
                mapObj.Y = Config.Stream.GetSingle(objAddress + ObjectConfig.YOffset);
                mapObj.Z = Config.Stream.GetSingle(objAddress + ObjectConfig.ZOffset);
                mapObj.IsActive = CurrentObject.IsActive;
                mapObj.Transparent = !mapObj.IsActive;
                ushort objYaw = Config.Stream.GetUInt16(objAddress + ObjectConfig.YawFacingOffset);
                mapObj.Rotation = (float)MoreMath.AngleUnitsToDegrees(objYaw);
                mapObj.UsesRotation = Config.ObjectAssociations.GetObjectMapRotates(behaviorCriteria);
            }
        }

        public override string ToString()
        {
            string objectString = CurrentObject?.ToString() ?? "(no object)";
            return objectString + " " + _text;
        }
    }
}
