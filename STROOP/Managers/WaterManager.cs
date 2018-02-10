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
    public class WaterManager : DataManager
    {
        public WaterManager(List<WatchVariableControlPrecursor> variables, WatchVariablePanel variableTable)
            : base(variables, variableTable)
        {

        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);
        }

    }
}
