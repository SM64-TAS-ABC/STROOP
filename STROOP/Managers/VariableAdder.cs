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
    public abstract class VariableAdder
    {
        public readonly string TabName;
        public readonly int TabIndex;

        public VariableAdder(Control control)
        {
            TabName = ControlUtilities.GetTabName(control);
            TabIndex = ControlUtilities.GetTabIndex(control);
        }

        public abstract void AddVariable(WatchVariableControl watchVarControl);

        public abstract void AddVariables(List<WatchVariableControl> watchVarControls);

        public override string ToString()
        {
            return TabName;
        }
    }
}
