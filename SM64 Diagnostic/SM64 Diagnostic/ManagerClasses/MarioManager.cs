using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MarioManager
    {
        Config _config;
        List<WatchVariableControl> _marioDataControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;


        public MarioManager(ProcessStream stream, Config config, List<WatchVariable> marioData, Control marioControl, FlowLayoutPanel variableTable)
        {
            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(marioControl);
            foreach (Control control in marioControl.Controls)
                RegisterControlEvents(control);

            _config = config;
            _variableTable = variableTable;
            _stream = stream;

            _marioDataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in marioData)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, _config.Mario.MarioPointerAddress);
                variableTable.Controls.Add(watchControl.Control);
                _marioDataControls.Add(watchControl);
            }
            
        }

        public void Update()
        {
            // Update watch variables
            foreach (var watchVar in _marioDataControls)
            {
                watchVar.Update();
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
            var dropAction = new DropAction(DropAction.ActionType.Mario, _config.Mario.MarioPointerAddress);
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
            if (dropAction.Action != DropAction.ActionType.Object)
                return;

            // Move object to Mario
            var marioAddress = _config.Mario.MarioPointerAddress;

            // Get Mario position
            float x, y, z;
            x = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.XOffset, 4), 0);
            y = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.YOffset, 4), 0);
            z = BitConverter.ToSingle(_stream.ReadRam(marioAddress + _config.Mario.ZOffset, 4), 0);

            // Add offset
            y += 300f;

            // Move object to Mario
            _stream.WriteRam(BitConverter.GetBytes(x), dropAction.Address + _config.ObjectSlots.ObjectXOffset);
            _stream.WriteRam(BitConverter.GetBytes(y), dropAction.Address + _config.ObjectSlots.ObjectYOffset);
            _stream.WriteRam(BitConverter.GetBytes(z), dropAction.Address + _config.ObjectSlots.ObjectZOffset);
        }
    }
}
