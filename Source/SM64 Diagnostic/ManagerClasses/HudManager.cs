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
    public class HudManager
    {
        Config _config;
        List<WatchVariableControl> _HudDataControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;

        public HudManager(ProcessStream stream, Config config, List<WatchVariable> hudData, Control hudControl, FlowLayoutPanel variableTable)
        {
            // Register controls on the control (for drag-and-drop)
            RegisterControlEvents(hudControl);
            foreach (Control control in hudControl.Controls)
                RegisterControlEvents(control);

            _config = config;
            _variableTable = variableTable;
            _stream = stream;
            
            _HudDataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in hudData)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, _config.Mario.MarioStructAddress);
                variableTable.Controls.Add(watchControl.Control);
                _HudDataControls.Add(watchControl);
            }
        }

        public void Update(bool updateView)
        {
            if (!updateView)
                return;

            // Update watch variables
            foreach (var watchVar in _HudDataControls)
            {
                watchVar.Update();
            }
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
