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
    public interface IVariableAdder
    {
        void AddVariable(WatchVariableControl watchVarControl);
        void AddVariables(List<WatchVariableControl> watchVarControls);
    }
}
