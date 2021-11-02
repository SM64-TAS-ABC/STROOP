using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Extensions;
using STROOP.Controls;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class QuarterFrameManager : DataManager
    {
        private static readonly List<VariableGroup> ALL_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.QuarterFrameHack,
                VariableGroup.PreviousPositions,
            };

        private static readonly List<VariableGroup> VISIBLE_VAR_GROUPS =
            new List<VariableGroup>()
            {
                VariableGroup.QuarterFrameHack,
            };

        public QuarterFrameManager(string varFilePath, WatchVariableFlowLayoutPanel variableTable)
            : base(varFilePath, variableTable, ALL_VAR_GROUPS, VISIBLE_VAR_GROUPS)
        {

        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);
        }

    }
}
