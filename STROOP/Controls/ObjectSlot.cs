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

        enum SelectionType { NOT_SELECTED, NORMAL_SELECTION, MAP_SELECTION, MODEL_SELECTION };
        SelectionType _selectionType = SelectionType.NOT_SELECTED;

        int prevHeight;
        object _gfxLock = new object();

        public enum MouseStateType {None, Over, Down};
        public MouseStateType MouseState;

        BehaviorCriteria _behavior;

        bool _isActive = false;

        public bool IsHovering { get; private set; }

        public override string Text => _text;
        Color _textColor
        {
            get => _textBrush.Color;
            set { lock (_gfxLock) { _textBrush.Color = value; } }
        }

        bool _drawSelectedOverlay, _drawStoodOnOverlay, _drawHeldOverlay, _drawInteractionOverlay, _drawUsedOverlay,
            _drawClosestOverlay, _drawCameraOverlay, _drawCameraHackOverlay, _drawModelOverlay,
            _drawFloorOverlay, _drawWallOverlay, _drawCeilingOverlay,
            _drawParentOverlay, _drawParentUnusedOverlay, _drawParentNoneOverlay, _drawMarkedOverlay;

        public ObjectSlot(ObjectSlotsManager manager, int index, ObjectSlotManagerGui gui, Size size)
        {
            _manager = manager;
            _gui = gui;
            Size = size;
            Index = index;
            Font = new Font(FontFamily.GenericSansSerif, 6);

            this.MouseDown += OnDrag;
            this.MouseUp += (s, e) => { MouseState = MouseStateType.None; UpdateColors(); };
            this.MouseEnter += (s, e) =>
            {
                IsHovering = true;
                MouseState = MouseStateType.Over;
                UpdateColors();
            };
            this.MouseLeave += (s, e) =>
            {
                IsHovering = false;
                MouseState = MouseStateType.None;
                UpdateColors();
            };
            this.Cursor = Cursors.Hand;
            this.DoubleBuffered = true;

            SetUpContextMenuStrip();
        }

        private void SetUpContextMenuStrip()
        {
            ToolStripMenuItem itemSelect = new ToolStripMenuItem("Select");
            itemSelect.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.DoSlotClickUsingSpecifications(
                    this, ObjectSlotsManager.ClickType.ObjectClick, false, false);
            };

            ToolStripMenuItem itemSelectAndSwitch = new ToolStripMenuItem("Select && Switch to Obj Tab");
            itemSelectAndSwitch.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.DoSlotClickUsingSpecifications(
                    this, ObjectSlotsManager.ClickType.ObjectClick, false, false, true);
            };

            ToolStripMenuItem itemSelectAndDontSwitch = new ToolStripMenuItem("Select && Don't Switch to Obj Tab");
            itemSelectAndDontSwitch.Click += (sender, e) =>
            {
                Config.ObjectSlotsManager.DoSlotClickUsingSpecifications(
                    this, ObjectSlotsManager.ClickType.ObjectClick, false, false, false);
            };

            ToolStripMenuItem itemGoto = new ToolStripMenuItem("Go to");
            itemGoto.Click += (sender, e) => ButtonUtilities.GotoObjects(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemRetrieve = new ToolStripMenuItem("Retrieve");
            itemRetrieve.Click += (sender, e) => ButtonUtilities.RetrieveObjects(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemGotoHome = new ToolStripMenuItem("Go to Home");
            itemGotoHome.Click += (sender, e) => ButtonUtilities.GotoObjectsHome(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemRetrieveHome = new ToolStripMenuItem("Retrieve Home");
            itemRetrieveHome.Click += (sender, e) => ButtonUtilities.RetrieveObjectsHome(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemRelease = new ToolStripMenuItem("Release");
            itemRelease.Click += (sender, e) => ButtonUtilities.ReleaseObject(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemUnRelease = new ToolStripMenuItem("UnRelease");
            itemUnRelease.Click += (sender, e) => ButtonUtilities.UnReleaseObject(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemInteract = new ToolStripMenuItem("Interact");
            itemInteract.Click += (sender, e) => ButtonUtilities.ReleaseObject(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemUnInteract = new ToolStripMenuItem("UnInteract");
            itemUnInteract.Click += (sender, e) => ButtonUtilities.UnInteractObject(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemClone = new ToolStripMenuItem("Clone");
            itemClone.Click += (sender, e) => ButtonUtilities.CloneObject(CurrentObject);

            ToolStripMenuItem itemUnClone = new ToolStripMenuItem("UnClone");
            itemUnClone.Click += (sender, e) => ButtonUtilities.UnCloneObject();

            ToolStripMenuItem itemUnload = new ToolStripMenuItem("Unload");
            itemUnload.Click += (sender, e) => ButtonUtilities.UnloadObject(new List<ObjectDataModel>() { CurrentObject });

            ToolStripMenuItem itemRevive = new ToolStripMenuItem("Revive");
            itemRevive.Click += (sender, e) => ButtonUtilities.ReviveObject(new List<ObjectDataModel>() { CurrentObject });

            ContextMenuStrip = new ContextMenuStrip();
            ContextMenuStrip.Items.Add(itemSelect);
            ContextMenuStrip.Items.Add(itemSelectAndSwitch);
            ContextMenuStrip.Items.Add(itemSelectAndDontSwitch);
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
            switch (MouseState)
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

        private void OnDrag(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            MouseState = MouseStateType.Down;
            UpdateColors();
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
        }

        public void Update(ObjectDataModel obj)
        {
            CurrentObject = obj;

            uint? address = CurrentObject?.Address;

            // Update Overlays
            var prevOverlays = new List<bool>()
            {
                _drawSelectedOverlay,
                _drawStoodOnOverlay,
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
                _drawMarkedOverlay,
            };
            if (address.HasValue)
            {
                _drawSelectedOverlay = _manager.SelectedSlotsAddresses.Contains(address.Value);
                _drawStoodOnOverlay = OverlayConfig.ShowOverlayStoodOnObject && address == DataModels.Mario.StoodOnObject;
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
                _drawParentOverlay = OverlayConfig.ShowOverlayParentObject && address == _manager.HoveredOverSlot.CurrentObject.Parent;
                _drawParentUnusedOverlay = _drawParentOverlay && _manager.HoveredOverSlot.CurrentObject?.Parent == 0;
                _drawParentNoneOverlay = _drawParentOverlay && _manager.HoveredOverSlot.CurrentObject?.Parent == ObjectSlotsConfig.UnusedSlotAddress;
                _drawMarkedOverlay = _manager.MarkedSlotsAddresses.Contains(address.Value);
            }
            else
            {
                _drawSelectedOverlay = false;
                _drawStoodOnOverlay = false;
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
                _drawMarkedOverlay = false;
            }
            var overlays = new List<bool>()
            {
                _drawSelectedOverlay,
                _drawStoodOnOverlay,
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
                _drawMarkedOverlay,
            };

            SelectionType selectionType;
            switch (_manager.ActiveTab)
            {
                case ObjectSlotsManager.TabType.Map:
                    selectionType = Show ? SelectionType.MAP_SELECTION
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
            string text = address.HasValue ? _manager.SlotLabelsForObjects[address.Value] : "";

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
        }
    }
}
