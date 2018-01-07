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
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class DataManager
    {
        private bool _varXSystem;
        private NoTearFlowLayoutPanel _variableTable;
        protected List<IDataContainer> _dataControls;
        protected List<VarXControl> _varXControlList;
        protected List<IDataContainer> _specialDataControls = new List<IDataContainer>();

        protected virtual List<SpecialWatchVariable> _specialWatchVars { get; } = new List<SpecialWatchVariable>();

        public DataManager(List<VarXControl> varXControlList, NoTearFlowLayoutPanel variableTable)
            : this(null, variableTable, true, varXControlList)
        {

        }

        public DataManager(List<WatchVariable> data, NoTearFlowLayoutPanel variableTable, bool varXSystem = false, List<VarXControl> varXControlList = null)
        {
            _varXSystem = varXSystem;

            if (_varXSystem)
            {
                _variableTable = variableTable;
                _varXControlList = varXControlList;
                foreach (VarXControl varXControl in varXControlList)
                {
                    _variableTable.Controls.Add(varXControl);
                }


                _dataControls = new List<IDataContainer>();
                /*
                foreach (WatchVariable watchVar in data)
                {
                    WatchVariableControl watchControl = new WatchVariableControl(watchVar);
                    _variableTable.Controls.Add(watchControl.Control);
                    _dataControls.Add(watchControl);
                }
                */
            }
            else
            {
                _variableTable = variableTable;
                _dataControls = new List<IDataContainer>();
                InitializeSpecialVariables();

                AddWatchVariables(data);
            }
        }

        protected void RemoveWatchVariables(IEnumerable<IDataContainer> watchVars)
        {
            foreach (var watchVar in watchVars)
            {
                _dataControls.Remove(watchVar);
                _variableTable.Controls.Remove(watchVar.Control);
            }
        }

        protected List<IDataContainer> AddWatchVariables(IEnumerable<WatchVariable> watchVars, Color? color = null)
        {
            var newControls = new List<IDataContainer>();
            // Add every watch variable
            foreach (WatchVariable watchVar in watchVars)
            {
                // Handle special variables
                if (watchVar.IsSpecial)
                {
                    // Find special variable container
                    if (_specialWatchVars.Exists(w => w.Name == watchVar.SpecialType))
                    {
                        // Create new container
                        SpecialWatchVariable specialVar = _specialWatchVars.Find(w => w.Name == watchVar.SpecialType);
                        IDataContainer specialVarControl;
                        if (specialVar.IsAngle)
                            specialVarControl = new AngleDataContainer(watchVar.SpecialType, specialVar.AngleViewMode);
                        else
                            specialVarControl = new DataContainer(watchVar.SpecialType);
                        specialVarControl.Name = watchVar.Name;

                        // Add special variable control to the tableview
                        _variableTable.Controls.Add(specialVarControl.Control);
                        _specialDataControls.Add(specialVarControl);
                        _dataControls.Add(specialVarControl);
                        newControls.Add(specialVarControl);

                        // Colorize control
                        if (watchVar.BackroundColor.HasValue)
                            specialVarControl.Color = watchVar.BackroundColor.Value;
                        else if (color.HasValue)
                            specialVarControl.Color = color.Value;
                    }
                    else
                    {
                        var failedContainer = new DataContainer(watchVar.Name);
                        failedContainer.Text = "Couldn't Find";
                        _variableTable.Controls.Add(failedContainer.Control);
                        _dataControls.Add(failedContainer);
                    }
                    continue;
                }

                // If not special, add a new watch control.
                WatchVariableControl watchControl = new WatchVariableControl(watchVar);
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
            if (_varXSystem)
            {
                foreach (VarXControl varXControl in _varXControlList)
                {
                    varXControl.UpdateControl();
                }
            }
            else
            {
                foreach (var watchVar in _dataControls)
                {
                    watchVar.Update();
                }
            }
        }
    }
}
