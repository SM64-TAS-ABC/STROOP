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
    public class DataManager
    {
        Config _config;
        List<WatchVariableControl> _dataControls;
        FlowLayoutPanel _variableTable;
        ProcessStream _stream;

        public DataManager(ProcessStream stream, Config config, List<WatchVariable> data, FlowLayoutPanel variableTable)
        {
            _config = config;
            _variableTable = variableTable;
            _stream = stream;
            
            _dataControls = new List<WatchVariableControl>();
            foreach (WatchVariable watchVar in data)
            {
                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, 0);
                variableTable.Controls.Add(watchControl.Control);
                _dataControls.Add(watchControl);
            }
        }

        public void Update(bool updateView)
        {
            // Update watch variables
            foreach (var watchVar in _dataControls)
                watchVar.Update();

            if (!updateView)
                return;
        }
    }
}
