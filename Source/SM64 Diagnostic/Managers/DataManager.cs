using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;

namespace SM64_Diagnostic.ManagerClasses
{
    public class DataManager
    {
        protected List<IDataContainer> _dataControls;
        protected FlowLayoutPanel _variableTable;
        protected ProcessStream _stream;
        protected List<IDataContainer> _specialWatchVars;

        public DataManager(ProcessStream stream, List<WatchVariable> data, FlowLayoutPanel variableTable, uint otherOffset = 0)
        {
            _variableTable = variableTable;
            _stream = stream;
            
            _dataControls = new List<IDataContainer>();
            InitializeSpecialVariables();

            foreach (WatchVariable watchVar in data)
            {
                if (watchVar.Special && _specialWatchVars != null)
                {
                    if (_specialWatchVars.Exists(w => w.SpecialName == watchVar.SpecialType))
                    {
                        var specialVar = _specialWatchVars.Find(w => w.SpecialName == watchVar.SpecialType);
                        specialVar.Name = watchVar.Name;
                        variableTable.Controls.Add(specialVar.Control);
                        if (watchVar.BackroundColor.HasValue)
                            specialVar.Color = watchVar.BackroundColor.Value;
                    }
                    else
                    {
                        var failedContainer = new DataContainer(watchVar.Name);
                        failedContainer.Text = "Couldn't Find";
                        variableTable.Controls.Add(failedContainer.Control);
                    }
                    continue;
                }

                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, otherOffset);
                variableTable.Controls.Add(watchControl.Control);
                _dataControls.Add(watchControl);
            }
        }

        protected virtual void InitializeSpecialVariables()
        {
        }

        public virtual void Update(bool updateView = false)
        {
            // Update watch variables
            foreach (var watchVar in _dataControls)
                watchVar.Update();
        }
    }
}
