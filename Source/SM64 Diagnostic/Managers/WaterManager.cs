using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class WaterManager : DataManager
    {
        public WaterManager(List<VarXControl> variables, NoTearFlowLayoutPanel variableTable)
            : base(variables, variableTable)
        {

        }

        public override void Update(bool updateView)
        {
            if (!updateView)
                return;

            base.Update();
        }

    }
}
