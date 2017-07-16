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
        public bool Show = false;
        bool _active = false;
        BehaviorCriteria _behavior;

        enum SelectionType { NOT_SELECTED, NORMAL_SELECTION, MAP_SELECTION };
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

        bool _drawSelectedOverlay, _drawStoodOnOverlay, _drawHeldOverlay, _drawInteractionObject, _drawUsedObject,
            _drawClosestOverlay, _drawCameraOverlay, _drawCameraHackOverlay, _drawFloorObject, _drawWallObject, _drawCeilingObject, _drawParentObject;
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
                return _drawInteractionObject;
            }
            set
            {
                if (_drawInteractionObject == value)
                    return;
                _drawInteractionObject = value;
                Invalidate();
            }
        }
        public bool DrawUsedOverlay
        {
            get
            {
                return _drawUsedObject;
            }
            set
            {
                if (_drawUsedObject == value)
                    return;
                _drawUsedObject = value;
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
                return _drawWallObject;
            }
            set
            {
                if (_drawWallObject == value)
                    return;
                _drawWallObject = value;
                Invalidate();
            }
        }
        public bool DrawCeilingOverlay
        {
            get
            {
                return _drawCeilingObject;
            }
            set
            {
                if (_drawCeilingObject == value)
                    return;
                _drawCeilingObject = value;
                Invalidate();
            }
        }
        public bool DrawParentOverlay
        {
            get
            {
                return _drawParentObject;
            }
            set
            {
                if (_drawParentObject == value)
                    return;
                _drawParentObject = value;
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
            this.MouseEnter += (s, e) => { MouseState = MouseStateType.Over; UpdateColors(); };
            this.MouseLeave += (s, e) => { MouseState = MouseStateType.None; UpdateColors(); };
            this.Cursor = Cursors.Hand;
            this.DoubleBuffered = true;
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
            _bufferedObjectImage = _manager.ObjectAssoc.GetCachedBufferedObjectImage(_objectImage, objectImageRec.Size);
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

            _manager.ObjectAssoc.CreateCachedBufferedObjectImage(_objectImage, _bufferedObjectImage);
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
            Image newImage = _manager.ObjectAssoc.GetObjectImage(_behavior, !_active);
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

            SelectionType newSelectionType =
                tabControlMain.SelectedTab.Text.Equals("Map") && Show ? SelectionType.MAP_SELECTION :
                tabControlMain.SelectedTab.Text.Equals("Map") && !Show ? SelectionType.NOT_SELECTED :
                tabControlMain.SelectedTab.Text.Equals("Cam Hack") ? SelectionType.NOT_SELECTED :
                DrawSelectedOverlay ? SelectionType.NORMAL_SELECTION : SelectionType.NOT_SELECTED;
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
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
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
                }

                // Draw Text
                var textSize = TextRenderer.MeasureText(e.Graphics, Text, Font);
                var textLocation = new Point((int) (Width - textSize.Width) / 2, (int)(Height - textSize.Height - BorderSize));
                TextRenderer.DrawText(e.Graphics, Text, Font, textLocation, TextColor);
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
            switch (_selectionType)
            {
                case SelectionType.MAP_SELECTION:
                    e.Graphics.DrawImage(_gui.TrackedAndShownObjectOverlayImage, new Rectangle(new Point(), Size));
                    break;

                case SelectionType.NORMAL_SELECTION:
                    e.Graphics.DrawImage(_gui.SelectedObjectOverlayImage, new Rectangle(new Point(), Size));
                    break;
            }
            if (_drawWallObject)
                e.Graphics.DrawImage(_gui.WallObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawFloorObject)
                e.Graphics.DrawImage(_gui.FloorObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCeilingObject)
                e.Graphics.DrawImage(_gui.CeilingObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawInteractionObject)
                e.Graphics.DrawImage(_gui.InteractionObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawHeldOverlay)
                e.Graphics.DrawImage(_gui.HeldObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawStoodOnOverlay)
                e.Graphics.DrawImage(_gui.StoodOnObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawUsedObject)
                e.Graphics.DrawImage(_gui.UsedObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawClosestOverlay)
                e.Graphics.DrawImage(_gui.ClosestObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCameraOverlay)
                e.Graphics.DrawImage(_gui.CameraObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCameraHackOverlay)
                e.Graphics.DrawImage(_gui.CameraHackObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawParentObject)
                e.Graphics.DrawImage(_gui.ParentObjectOverlayImage, new Rectangle(new Point(), Size));
        }

        
    }
}
