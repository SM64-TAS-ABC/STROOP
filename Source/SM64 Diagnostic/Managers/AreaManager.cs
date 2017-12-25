using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;
using static SM64_Diagnostic.Controls.AngleDataContainer;
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class AreaManager : DataManager
    {
        public static AreaManager Instance = null;

        public AreaManager(Control tabControl, List<WatchVariable> areaWatchVars, NoTearFlowLayoutPanel noTearFlowLayoutPanel) 
            : base(areaWatchVars, noTearFlowLayoutPanel)
        {
            Instance = this;

            SplitContainer splitContainerArea = tabControl.Controls["splitContainerArea"] as SplitContainer;
            
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}
