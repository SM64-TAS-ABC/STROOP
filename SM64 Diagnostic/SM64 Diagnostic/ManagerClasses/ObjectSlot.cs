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

namespace SM64_Diagnostic
{
    public class ObjectSlot
    {
        ObjectSlotManager _manager;

        PictureBox PictureBox;
        Panel BorderPanel;
        Panel ContentPanel;
        Label Label;
        public int Index;
        public int? ObjectAddress = null;

        public Control Control
        {
            get
            {
                return BorderPanel;
            }
        }

        public ObjectSlotManager Manager
        {
            get
            {
                return _manager;
            }
        }

        public Color BackColor
        {
            set
            {
                if (BorderPanel.BackColor != value)
                {
                    BorderPanel.BackColor = value;
                    ContentPanel.BackColor = ControlPaint.Light(ControlPaint.Light(ControlPaint.Light(value)));
                }
            }
            get
            {
                return BorderPanel.BackColor;
            }
        }

        public Image Image
        {
            set
            {
                if (PictureBox.Image != value)
                {
                    PictureBox.Image = value;
                }
            }
            get
            {
                return PictureBox.Image;
            }
        }

        public string Text
        {
            get
            {
                return Label.Text;
            }
            set
            {
                Label.Text = value;
            }
        }

        public ObjectSlot(int index, ObjectSlotManager manager)
        {
            Index = index;
            _manager = manager;

            // Create picture box
            const int borderSize = 2;

            this.BorderPanel = new Panel();
            this.BorderPanel.Size = new Size(40, 40);

            this.ContentPanel = new Panel();
            this.ContentPanel.Size = new Size(this.BorderPanel.Size.Width - borderSize * 2, this.BorderPanel.Size.Height - borderSize * 2);
            this.ContentPanel.Location = new Point(borderSize, borderSize);

            this.Label = new Label();
            this.Label.Text = "1";
            this.Label.Font = new Font(FontFamily.GenericSansSerif, 6);
            this.Label.TextAlign = ContentAlignment.TopCenter;
            this.Label.Anchor = AnchorStyles.None;
            this.Label.Location =
                new Point((this.ContentPanel.Width - this.Label.Size.Width) / 2, this.ContentPanel.Height - this.Label.Font.Height);

            this.PictureBox = new PictureBox();
            this.PictureBox.Size = new Size(this.ContentPanel.Width, this.Label.Location.Y - 1);
            this.PictureBox.AllowDrop = true;
            this.PictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            this.PictureBox.BackColor = Color.FromKnownColor(KnownColor.Transparent);
            this.PictureBox.Location = new Point((ContentPanel.Width - PictureBox.Width) / 2, 1);
            this.PictureBox.Anchor = AnchorStyles.None;

            RegisterControl(this.PictureBox);
            RegisterControl(this.Label);
            RegisterControl(this.BorderPanel);
            RegisterControl(this.ContentPanel);

            this.ContentPanel.Controls.Add(PictureBox);
            this.ContentPanel.Controls.Add(Label);
            this.BorderPanel.Controls.Add(ContentPanel);
        }

        private void RegisterControl(Control control)
        {
            control.AllowDrop = true;
            control.MouseDown += OnDrag;
            control.DragEnter += DragEnter;
            control.DragDrop += OnDrop;
            control.Click += OnClick;
        }

        private void OnClick(object sender, EventArgs e)
        {
            //_manager.SelectedSlot = Index;
        }

        private void OnDrag(object sender, EventArgs e)
        {
            // Start the drag and drop but setting the object slot index in Drag and Drop data
            var objectAddress = _manager.ObjectSlotData.First((objData) => objData.Index == Index).Address;
            var dropAction = new DropAction(DropAction.ActionType.Object, objectAddress); 
            PictureBox.DoDragDrop(dropAction, DragDropEffects.All);

            _manager.SelectedSlot = Index;
        }

        private void DragEnter(object sender, DragEventArgs e)
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
    }
}
