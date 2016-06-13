using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.ManagerClasses
{
    public class ObjectManager
    {
        Config _config;
        List<WatchVariableControl> _objectDataControls;
        ProcessStream _stream;
        ObjectAssociations _objAssoc;
        ObjectDataGui _objGui;

        public uint CurrentAddress;

        public Color BackColor
        {
            set
            {
                if (_objGui.ObjectBorderPanel.BackColor != value)
                {
                    _objGui.ObjectBorderPanel.BackColor = value;
                    _objGui.ObjectImagePictureBox.BackColor = ControlPaint.Light(ControlPaint.Light(ControlPaint.Light(value)));
                }
            }
            get
            {
                return _objGui.ObjectBorderPanel.BackColor;
            }
        }

        public Image Image
        {
            set
            {
                if (_objGui.ObjectImagePictureBox.Image != value)
                {
                    _objGui.ObjectImagePictureBox.Image = value;
                }
            }
            get
            {
                return _objGui.ObjectImagePictureBox.Image;
            }
        }

        public ObjectManager(ProcessStream stream, Config config, ObjectAssociations objAssoc, List<WatchVariable> objectData, ObjectDataGui objectGui)
        { 
            _config = config;
            _stream = stream;
            _objGui = objectGui;
            _objAssoc = objAssoc;

            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(_objGui.ObjectBorderPanel);
            foreach (Control control in _objGui.ObjectBorderPanel.Controls)
                RegisterControlEvents(control);

            _objectDataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in objectData)
            {
                WatchVariableControl watchControl = new WatchVariableControl(watchVar);
                objectGui.ObjectFlowLayout.Controls.Add(watchControl.Control);
                _objectDataControls.Add(watchControl);
            }

        }

        public void Update()
        {

            // Update watch variables
            foreach (var watchVar in _objectDataControls)
            {
                watchVar.Update(_stream, CurrentAddress);
            }
        }

        private void RegisterControlEvents(Control control)
        {
            control.AllowDrop = true;
            control.DragEnter += DragEnter;
            control.DragDrop += OnDrop;
            control.MouseDown += OnDrag;
        }

        private void OnDrag(object sender, EventArgs e)
        {
            // Start the drag and drop but setting the object slot index in Drag and Drop data
            var dropAction = new DropAction(DropAction.ActionType.Object, CurrentAddress);
            (sender as Control).DoDragDrop(dropAction, DragDropEffects.All);
        }

        private void DragEnter(object sender, DragEventArgs e)
        {
            // Make sure we have valid Drag and Drop data (it is an index)
            if (!e.Data.GetDataPresent(typeof(DropAction)))
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            var dropAction = ((DropAction)e.Data.GetData(typeof(DropAction))).Action;
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
        }
    }
}
