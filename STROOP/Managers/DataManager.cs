using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using System.Drawing;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class DataManager : VariableAdder
    {
        protected WatchVariableFlowLayoutPanel _variablePanel;

        public DataManager(
            string varFilePath,
            WatchVariableFlowLayoutPanel variablePanel,
            List<VariableGroup> allVariableGroups = null,
            List<VariableGroup> visibleVariableGroups = null) : base(variablePanel)
        {
            _variablePanel = variablePanel;
            _variablePanel.Initialize(
                varFilePath,
                allVariableGroups,
                visibleVariableGroups);
        }

        public virtual void RemoveVariableGroup(VariableGroup varGroup)
        {
            _variablePanel.RemoveVariableGroup(varGroup);
        }

        public override void AddVariable(WatchVariableControl watchVarControl)
        {
            _variablePanel.AddVariable(watchVarControl);
        }

        public override void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            _variablePanel.AddVariables(watchVarControls);
        }

        public virtual void EnableCustomization()
        {
            _variablePanel.EnableCustomization(false);
        }

        public virtual List<object> GetCurrentVariableValues(bool useRounding = false, bool handleFormatting = true)
        {
            return _variablePanel.GetCurrentVariableValues(useRounding, handleFormatting);
        }

        public virtual List<string> GetCurrentVariableNames()
        {
            return _variablePanel.GetCurrentVariableNames();
        }

        public virtual void Update(bool updateView)
        {
            if (!updateView) return;
            _variablePanel.UpdatePanel();
        }
    }
}
