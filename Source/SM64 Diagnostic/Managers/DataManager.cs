using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using System.Drawing;

namespace SM64_Diagnostic.Managers
{
    public class DataManager
    {
        protected List<IDataContainer> _dataControls;
        protected NoTearFlowLayoutPanel _variableTable;
        protected ProcessStream _stream;
        protected List<IDataContainer> _specialWatchVars;
        uint _otherOffset;

        public DataManager(ProcessStream stream, List<WatchVariable> data, NoTearFlowLayoutPanel variableTable, uint otherOffset = 0)
        {
            _variableTable = variableTable;
            _stream = stream;
            _otherOffset = otherOffset;

            _dataControls = new List<IDataContainer>();
            InitializeSpecialVariables();

            AddWatchVariables(data);
        }

        protected void RemoveWatchVariables(IEnumerable<IDataContainer> watchVars)
        {
            foreach (var watchVar in watchVars)
            {
                if (_dataControls.Contains(watchVar))
                    _dataControls.Remove(watchVar);

                _variableTable.Controls.Remove(watchVar.Control);
            }
        }

        protected List<IDataContainer> AddWatchVariables(IEnumerable<WatchVariable> watchVars, Color? color = null)
        {
            var newControls = new List<IDataContainer>();
            foreach (WatchVariable watchVar in watchVars)
            {
                if (watchVar.Special && _specialWatchVars != null)
                {
                    if (_specialWatchVars.Exists(w => w.SpecialName == watchVar.SpecialType))
                    {
                        var specialVar = _specialWatchVars.Find(w => w.SpecialName == watchVar.SpecialType);
                        specialVar.Name = watchVar.Name;
                        _variableTable.Controls.Add(specialVar.Control);
                        newControls.Add(specialVar);
                        if (watchVar.BackroundColor.HasValue)
                            specialVar.Color = watchVar.BackroundColor.Value;
                        else if (color.HasValue)
                            specialVar.Color = color.Value;
                    }
                    else
                    {
                        var failedContainer = new DataContainer(watchVar.Name);
                        failedContainer.Text = "Couldn't Find";
                        _variableTable.Controls.Add(failedContainer.Control);
                    }
                    continue;
                }

                WatchVariableControl watchControl = new WatchVariableControl(_stream, watchVar, _otherOffset);
                if (color.HasValue)
                    watchControl.Color = color.Value;
                _variableTable.Controls.Add(watchControl.Control);
                _dataControls.Add(watchControl);
                newControls.Add(watchControl);
            }

            return newControls;
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
