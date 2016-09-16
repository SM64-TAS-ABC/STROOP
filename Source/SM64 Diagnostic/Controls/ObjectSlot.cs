using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.ManagerClasses;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using System.Drawing.Drawing2D;

namespace SM64_Diagnostic
{
    public class ObjectSlot : Panel
    {
        const int BorderSize = 2;

        ObjectSlotManager _manager;

        Color _mainColor, _borderColor, _backColor;
        Brush _borderBrush = new SolidBrush(Color.White), _backBrush = new SolidBrush(Color.White);
        Image _objectImage;
        string _text;

        bool _selected = true;
        bool _active = false;
        uint _behavior;

        int prevHeight;

        public enum MouseStateType {None, Over, Down};
        public MouseStateType MouseState;

        public bool Selected
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
        public uint Behavior
        {
            get
            {
                return _behavior;
            }
            set
            {
                if (_behavior != value)
                {
                    _behavior = value;
                    UpdateColors();
                }
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

                _text = value;
                Refresh();
            }
        }

        public ObjectSlotManager Manager
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
                if (_mainColor == value)
                    return;

                _mainColor = value;
                UpdateColors();
            }
        }

        void UpdateColors()
        {
            if (!_selected)
            {
                var newColor = _mainColor;
                switch (MouseState)
                {
                    case MouseStateType.Down:
                        _borderColor = newColor.Darken(0.5);
                        _backColor = newColor.Darken(0.5).Lighten(0.5);
                        break;
                    case MouseStateType.Over:
                        _borderColor = newColor.Lighten(0.75);
                        _backColor = newColor.Lighten(0.92);
                        break;
                    default:
                        _borderColor = newColor.Lighten(0.5);
                        _backColor = newColor.Lighten(0.85);
                        break;
                }
                Image newImage = _manager.ObjectAssoc.GetObjectImage(_behavior, true);
                if (_objectImage != newImage)
                    _objectImage = newImage;
            }
            else
            {
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
                    _objectImage = newImage;
            }
            (_borderBrush as SolidBrush).Color = _borderColor;
            (_backBrush as SolidBrush).Color = _backColor;
            Refresh();
        }

        public ObjectSlot(int index, ObjectSlotManager manager, Size size)
        {
            Index = index;
            _manager = manager;
            Size = size;
            Font = new Font(FontFamily.GenericSansSerif, 6);

            this.AllowDrop = true;
            this.MouseDown += OnDrag;
            this.MouseUp += (s, e) => { MouseState = MouseStateType.None; UpdateColors(); };
            this.MouseEnter += (s, e) => { MouseState = MouseStateType.Over; UpdateColors(); };
            this.MouseLeave += (s, e) => { MouseState = MouseStateType.None; UpdateColors(); };

            this.DragEnter += OnDragEnter;
            this.DragDrop += OnDrop;
            this.Cursor = Cursors.Hand;
        }

        private void OnDrag(object sender, MouseEventArgs e)
        {
            OnClick(new EventArgs());

            MouseState = MouseStateType.Down;
            UpdateColors();
            Refresh();

            // Start the drag and drop but setting the object slot index in Drag and Drop data
            var objectAddress = _manager.ObjectSlotData.First((objData) => objData.Index == Index).Address;
            var dropAction = new DropAction(DropAction.ActionType.Object, objectAddress); 
            DoDragDrop(dropAction, DragDropEffects.All);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {

            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var dropAction = ((DropAction) e.Data.GetData(typeof(DropAction))).Action;
            if (dropAction != DropAction.ActionType.Object && dropAction != DropAction.ActionType.Mario)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.Move;
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
                return;

            var dropAction = ((DropAction)e.Data.GetData(typeof(DropAction)));
            _manager.OnSlotDropAction(dropAction, this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // Border
            e.Graphics.FillRectangle(_borderBrush, new Rectangle(new Point(), Size));

            // Background
            e.Graphics.FillRectangle(_backBrush, new Rectangle(BorderSize, BorderSize, Width - BorderSize * 2, Height - BorderSize * 2));

            // Change font size
            if (Height != prevHeight)
            {
                prevHeight = Height;
                Font = new Font(FontFamily.GenericSansSerif, Math.Max(6, 6 / 40.0f * Height));
            }

            // Draw Text
            var textSize = e.Graphics.MeasureString(Text, Font);
            var textLocation = new PointF((Width - textSize.Width) / 2, Height - textSize.Height);
            e.Graphics.DrawString(Text, Font, Brushes.Black, textLocation);

            // Draw Object Image
            if (_objectImage != null)
            {
                e.Graphics.InterpolationMode = InterpolationMode.High;
                var objectImageLocaction = (new RectangleF(BorderSize, BorderSize + 1, 
                    Width - BorderSize * 2, textLocation.Y - 1 - BorderSize))
                    .Zoom(_objectImage.Size);
                e.Graphics.DrawImage(_objectImage, objectImageLocaction);
            }
        }

        
    }
}
