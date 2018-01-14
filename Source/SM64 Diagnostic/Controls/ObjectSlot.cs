using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using System.Drawing.Drawing2D;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic
{
    public class ObjectSlot : Panel
    {
        public static TabControl tabControlMain;
        const int BorderSize = 2;

        ObjectSlotsManager _manager;
        ObjectSlotManagerGui _gui;

        Color _mainColor, _borderColor, _backColor;
        SolidBrush _borderBrush = new SolidBrush(Color.White), _backBrush = new SolidBrush(Color.White);
        SolidBrush _textBrush = new SolidBrush(Color.Black);
        Image _objectImage;
        Image _bufferedObjectImage;
        Point _textLocation = new Point();
        Point _objectImageLocation = new Point();
        string _text;

        bool _selected = false;
        public new bool Show = false;
        bool _active = false;
        BehaviorCriteria _behavior;

        enum SelectionType { NOT_SELECTED, NORMAL_SELECTION, MAP_SELECTION, MODEL_SELECTION };
        SelectionType _selectionType = SelectionType.NOT_SELECTED;

        int prevHeight;
        object _gfxLock = new object();

        public enum MouseStateType {None, Over, Down};
        public MouseStateType MouseState;

        public bool SelectedOnMap
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                UpdateColors();
            }
        }
        public int Index;
        public uint Address;

        public byte ProcessGroup;
        public BehaviorCriteria Behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                if (_behavior == value)
                    return;
                _behavior = value;
                UpdateColors();
            }
        }

        public bool IsActive
        {
            get
            {
                return _active;
            }
            set
            {
                if (_active != value)
                {
                    _active = value;
                    UpdateColors();
                }
            }
        }

        public Image ObjectImage
        {
            get
            {
                return _objectImage;
            }
        }

        public Color TextColor
        {
            get
            {
                return _textBrush.Color;
            }
            set
            {
                if (_textBrush.Color == value)
                    return;

                lock (_gfxLock)
                {
                    _textBrush.Color = value;
                }
                Invalidate();
            }
        }

        public new string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text == value)
                    return;
                lock(_gfxLock)
                {
                    _text = value;
                }
                Invalidate();
            }
        }

        public ObjectSlotsManager Manager
        {
            get
            {
                return _manager;
            }
        }

        public override Color BackColor
        {
            get
            {
                return _mainColor;
            }
            set
            {
                _mainColor = value;
                UpdateColors();
            }
        }

        bool _drawSelectedOverlay, _drawStoodOnOverlay, _drawHeldOverlay, _drawInteractionOverlay, _drawUsedOverlay,
            _drawClosestOverlay, _drawCameraOverlay, _drawCameraHackOverlay, _drawModelOverlay,
            _drawFloorObject, _drawWallOverlay, _drawCeilingOverlay,
            _drawParentOverlay, _drawParentUnusedOverlay, _drawParentNoneOverlay, _drawMarkedOverlay;
        public bool DrawSelectedOverlay
        {
            get
            {
                return _drawSelectedOverlay;
            }
            set
            {
                if (_drawSelectedOverlay == value)
                    return;
                _drawSelectedOverlay = value;
                Invalidate();
            }
        }
        public bool DrawStoodOnOverlay
        {
            get
            {
                return _drawStoodOnOverlay;
            }
            set
            {
                if (_drawStoodOnOverlay == value)
                    return;
                _drawStoodOnOverlay = value;
                Invalidate();
            }
        }
        public bool DrawHeldOverlay
        {
            get
            {
                return _drawHeldOverlay;
            }
            set
            {
                if (_drawHeldOverlay == value)
                    return;
                _drawHeldOverlay = value;
                Invalidate();
            }
        }
        public bool DrawInteractionOverlay
        {
            get
            {
                return _drawInteractionOverlay;
            }
            set
            {
                if (_drawInteractionOverlay == value)
                    return;
                _drawInteractionOverlay = value;
                Invalidate();
            }
        }
        public bool DrawUsedOverlay
        {
            get
            {
                return _drawUsedOverlay;
            }
            set
            {
                if (_drawUsedOverlay == value)
                    return;
                _drawUsedOverlay = value;
                Invalidate();
            }
        }
        public bool DrawClosestOverlay
        {
            get
            {
                return _drawClosestOverlay;
            }
            set
            {
                if (_drawClosestOverlay == value)
                    return;
                _drawClosestOverlay = value;
                Invalidate();
            }
        }
        public bool DrawCameraOverlay
        {
            get
            {
                return _drawCameraOverlay;
            }
            set
            {
                if (_drawCameraOverlay == value)
                    return;
                _drawCameraOverlay = value;
                Invalidate();
            }
        }
        public bool DrawCameraHackOverlay
        {
            get
            {
                return _drawCameraHackOverlay;
            }
            set
            {
                if (_drawCameraHackOverlay == value)
                    return;
                _drawCameraHackOverlay = value;
                Invalidate();
            }
        }
        public bool DrawModelOverlay
        {
            get
            {
                return _drawModelOverlay;
            }
            set
            {
                if (_drawModelOverlay == value)
                    return;
                _drawModelOverlay = value;
                Invalidate();
            }
        }
        public bool DrawFloorOverlay
        {
            get
            {
                return _drawFloorObject;
            }
            set
            {
                if (_drawFloorObject == value)
                    return;
                _drawFloorObject = value;
                Invalidate();
            }
        }
        public bool DrawWallOverlay
        {
            get
            {
                return _drawWallOverlay;
            }
            set
            {
                if (_drawWallOverlay == value)
                    return;
                _drawWallOverlay = value;
                Invalidate();
            }
        }
        public bool DrawCeilingOverlay
        {
            get
            {
                return _drawCeilingOverlay;
            }
            set
            {
                if (_drawCeilingOverlay == value)
                    return;
                _drawCeilingOverlay = value;
                Invalidate();
            }
        }
        public bool DrawParentOverlay
        {
            get
            {
                return _drawParentOverlay;
            }
            set
            {
                if (_drawParentOverlay == value)
                    return;
                _drawParentOverlay = value;
                Invalidate();
            }
        }
        public bool DrawParentUnusedOverlay
        {
            get
            {
                return _drawParentUnusedOverlay;
            }
            set
            {
                if (_drawParentUnusedOverlay == value)
                    return;
                _drawParentUnusedOverlay = value;
                Invalidate();
            }
        }
        public bool DrawParentNoneOverlay
        {
            get
            {
                return _drawParentNoneOverlay;
            }
            set
            {
                if (_drawParentNoneOverlay == value)
                    return;
                _drawParentNoneOverlay = value;
                Invalidate();
            }
        }
        public bool DrawMarkedOverlay
        {
            get
            {
                return _drawMarkedOverlay;
            }
            set
            {
                if (_drawMarkedOverlay == value)
                    return;
                _drawMarkedOverlay = value;
                Invalidate();
            }
        }

        public ObjectSlot(int index, ObjectSlotsManager manager, ObjectSlotManagerGui gui, Size size)
        {
            Index = index;
            _manager = manager;
            _gui = gui;
            Size = size;
            Font = new Font(FontFamily.GenericSansSerif, 6);

            this.MouseDown += OnDrag;
            this.MouseUp += (s, e) => { MouseState = MouseStateType.None; UpdateColors(); };
            this.MouseEnter += (s, e) =>
            {
                Config.ObjectSlots.HoverObjectSlot = this;
                MouseState = MouseStateType.Over;
                UpdateColors();
            };
            this.MouseLeave += (s, e) =>
            {
                Config.ObjectSlots.HoverObjectSlot = null;
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
            itemSelect.Click += (sender, e) => { };

            ToolStripMenuItem itemSelectAndSwitch = new ToolStripMenuItem("Select & Switch to Obj Tab");
            itemSelectAndSwitch.Click += (sender, e) => { };

            ToolStripMenuItem itemSelectAndDontSwitch = new ToolStripMenuItem("Select & Don't Switch to Obj Tab");
            itemSelectAndDontSwitch.Click += (sender, e) => { };

            ToolStripMenuItem itemGoto = new ToolStripMenuItem("Go to");
            itemGoto.Click += (sender, e) => ButtonUtilities.GotoObjects(new List<uint>() { Address });

            ToolStripMenuItem itemRetrieve = new ToolStripMenuItem("Retrieve");
            itemRetrieve.Click += (sender, e) => ButtonUtilities.RetrieveObjects(new List<uint>() { Address });

            ToolStripMenuItem itemGotoHome = new ToolStripMenuItem("Go to Home");
            itemGotoHome.Click += (sender, e) => ButtonUtilities.GotoObjectsHome(new List<uint>() { Address });

            ToolStripMenuItem itemRetrieveHome = new ToolStripMenuItem("Retrieve Home");
            itemRetrieveHome.Click += (sender, e) => ButtonUtilities.RetrieveObjectsHome(new List<uint>() { Address });

            ToolStripMenuItem itemRelease = new ToolStripMenuItem("Release");
            itemRelease.Click += (sender, e) => ButtonUtilities.ReleaseObject(new List<uint>() { Address });

            ToolStripMenuItem itemUnRelease = new ToolStripMenuItem("UnRelease");
            itemUnRelease.Click += (sender, e) => ButtonUtilities.UnReleaseObject(new List<uint>() { Address });

            ToolStripMenuItem itemInteract = new ToolStripMenuItem("Interact");
            itemInteract.Click += (sender, e) => ButtonUtilities.ReleaseObject(new List<uint>() { Address });

            ToolStripMenuItem itemUnInteract = new ToolStripMenuItem("UnInteract");
            itemUnInteract.Click += (sender, e) => ButtonUtilities.UnInteractObject(new List<uint>() { Address });

            ToolStripMenuItem itemClone = new ToolStripMenuItem("Clone");
            itemClone.Click += (sender, e) => ButtonUtilities.CloneObject(Address);

            ToolStripMenuItem itemUnClone = new ToolStripMenuItem("UnClone");
            itemUnClone.Click += (sender, e) => ButtonUtilities.UnCloneObject();

            ToolStripMenuItem itemUnload = new ToolStripMenuItem("Unload");
            itemUnload.Click += (sender, e) => ButtonUtilities.UnloadObject(new List<uint>() { Address });

            ToolStripMenuItem itemRevive = new ToolStripMenuItem("Revive");
            itemRevive.Click += (sender, e) => ButtonUtilities.ReviveObject(new List<uint>() { Address });

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

        public void UpdateColors()
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
            Image newImage = Config.ObjectAssociations.GetObjectImage(_behavior, !_active);
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

            SelectionType newSelectionType;
            switch (_manager.ActiveTab)
            {
                case ObjectSlotsManager.TabType.Map:
                    newSelectionType = Show ? SelectionType.MAP_SELECTION 
                        : SelectionType.NOT_SELECTED;
                    break;

                case ObjectSlotsManager.TabType.Model:
                    newSelectionType = DrawModelOverlay ? SelectionType.MODEL_SELECTION
                        : SelectionType.NOT_SELECTED;
                    break;

                case ObjectSlotsManager.TabType.CamHack:
                    newSelectionType = SelectionType.NOT_SELECTED;
                    break;

                default:
                    newSelectionType = DrawSelectedOverlay ? SelectionType.NORMAL_SELECTION 
                        : SelectionType.NOT_SELECTED;
                    break;
            }

            bool selectionTypeUpdated = newSelectionType != _selectionType;
            _selectionType = newSelectionType;

            if (!imageUpdated && !colorUpdated && !selectionTypeUpdated)
                return;

            Invalidate();
        }

        private void OnDrag(object sender, MouseEventArgs e)
        {
            MouseState = MouseStateType.Down;
            UpdateColors();
            Refresh();
        }

        int _fontHeight;
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
                TextRenderer.DrawText(e.Graphics, Text, Font, textLocation, TextColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.Top);
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
            if (_drawFloorObject)
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


    }
}
