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
    public class CameraManager
    {
        List<WatchVariableControl> _cameraDataControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;

        public CameraManager(ProcessStream stream, List<WatchVariable> cameraData, Control cameraControl, FlowLayoutPanel variableTable)
        {
            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(cameraControl);
            foreach (Control control in cameraControl.Controls)
                RegisterControlEvents(control);

            _variableTable = variableTable;
            _stream = stream;
            
            _cameraDataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in cameraData)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, Config.Mario.MarioStructAddress);
                variableTable.Controls.Add(watchControl.Control);
                _cameraDataControls.Add(watchControl);
            }
        }

        public void Update(bool updateView)
        {
            // Update watch variables
            foreach (var watchVar in _cameraDataControls)
                watchVar.Update();

            if (!updateView)
                return;
        }

        private void RegisterControlEvents(Control control)
        {
            //control.AllowDrop = true;
            //control.DragEnter += DragEnter;
            //control.DragDrop += OnDrop;
            //control.MouseDown += OnDrag;
        }
    }
}
