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
    public class DataManager
    {
        private readonly string _varFilePath;
        protected WatchVariableFlowLayoutPanel _variablePanel;
        public readonly string TabName;
        public readonly int TabIndex;

        public DataManager(
            string varFilePath,
            WatchVariableFlowLayoutPanel variablePanel,
            List<VariableGroup> allVariableGroupsNullable = null,
            List<VariableGroup> visibleVariableGroupsNullable = null)
        {
            _varFilePath = varFilePath;
            List<WatchVariableControlPrecursor> variables = varFilePath == null
                ? new List<WatchVariableControlPrecursor>()
                : XmlConfigParser.OpenWatchVariableControlPrecursors(varFilePath);

            _variablePanel = variablePanel;

            List<VariableGroup> allVariableGroups = allVariableGroupsNullable ?? new List<VariableGroup>();
            if (allVariableGroups.Contains(VariableGroup.Custom)) throw new ArgumentOutOfRangeException();
            allVariableGroups.Add(VariableGroup.Custom);

            List<VariableGroup> visibleVariableGroups = visibleVariableGroupsNullable ?? new List<VariableGroup>();
            if (visibleVariableGroups.Contains(VariableGroup.Custom)) throw new ArgumentOutOfRangeException();
            visibleVariableGroups.Add(VariableGroup.Custom);

            _variablePanel.Initialize(
                this,
                variables,
                allVariableGroups,
                visibleVariableGroups);
            TabName = ControlUtilities.GetTabName(_variablePanel);
            TabIndex = ControlUtilities.GetTabIndex(_variablePanel);
        }

        public virtual void RemoveVariableGroup(VariableGroup varGroup)
        {
            _variablePanel.RemoveVariableGroup(varGroup);
        }

        public virtual void AddVariable(WatchVariableControl watchVarControl)
        {
            _variablePanel.AddVariable(watchVarControl);
        }

        public virtual void AddVariables(List<WatchVariableControl> watchVarControls)
        {
            _variablePanel.AddVariables(watchVarControls);
        }

        public virtual void ClearVariables()
        {
            _variablePanel.ClearVariables();
        }

        public void OpenVariables()
        {
            _variablePanel.OpenVariables();
        }

        public void SaveVariables()
        {
            _variablePanel.SaveVariables();
        }

        public void SaveVariablesInPlace()
        {
            if (!DialogUtilities.AskQuestionAboutSavingVariableFileInPlace()) return;
            _variablePanel.SaveVariables(_varFilePath);
        }

        public virtual void EnableCustomVariableFunctionality()
        {
            _variablePanel.EnableCustomVariableFunctionality();
        }

        public virtual List<object> GetCurrentVariableValues(bool useRounding = false)
        {
            return _variablePanel.GetCurrentVariableValues(useRounding);
        }

        public virtual List<string> GetCurrentVariableNames()
        {
            return _variablePanel.GetCurrentVariableNames();
        }

        public virtual void Update(bool updateView)
        {
            if (!updateView) return;
            _variablePanel.UpdateControls();
        }

        public override string ToString()
        {
            return TabName;
        }
    }
}
