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

        protected override List<SpecialWatchVariable> _specialWatchVars { get; } = new List<SpecialWatchVariable>()
        {
            new SpecialWatchVariable("CurrentAreaIndex1"),
            new SpecialWatchVariable("CurrentAreaIndex2"),
        };

        public void ProcessSpecialVars()
        {
            foreach (var specialVar in _specialDataControls)
            {
                switch (specialVar.SpecialName)
                {
                    case "CurrentAreaIndex1":
                        uint currentArea1 = Config.Stream.GetUInt32(0x8033B200);
                        (specialVar as DataContainer).Text = Config.Area.GetAreaIndex(currentArea1).ToString();
                        break;

                    case "CurrentAreaIndex2":
                        uint currentArea2 = Config.Stream.GetUInt32(0x8032DDCC);
                        (specialVar as DataContainer).Text = Config.Area.GetAreaIndex(currentArea2).ToString();
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
            ProcessSpecialVars();
        }
    }
}
