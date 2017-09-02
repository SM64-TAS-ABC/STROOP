using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using System.Windows.Forms;
using static SM64_Diagnostic.Structs.Configurations.Config;
using static SM64_Diagnostic.Structs.Configurations.PositionControllerRelativeAngleConfig;

namespace SM64_Diagnostic.Managers
{
    public class TestingManager
    {
        public TestingManager(TabPage tabControl)
        {


            /*
            // rom version
            GroupBox groupBoxRomVersion = tabControl.Controls["groupBoxRomVersion"] as GroupBox;
            RadioButton radioButtonRomVersionUS = groupBoxRomVersion.Controls["radioButtonRomVersionUS"] as RadioButton;
            radioButtonRomVersionUS.Checked = Config.Version == Config.RomVersion.US;
            radioButtonRomVersionUS.Click += (sender, e) => { Config.Version = RomVersion.US; };
            RadioButton radioButtonRomVersionJP = groupBoxRomVersion.Controls["radioButtonRomVersionJP"] as RadioButton;
            radioButtonRomVersionJP.Checked = Config.Version == Config.RomVersion.JP;
            radioButtonRomVersionJP.Click += (sender, e) => { Config.Version = RomVersion.JP; };
            RadioButton radioButtonRomVersionPAL = groupBoxRomVersion.Controls["radioButtonRomVersionPAL"] as RadioButton;
            radioButtonRomVersionPAL.Checked = Config.Version == Config.RomVersion.PAL;
            radioButtonRomVersionPAL.Click += (sender, e) => { Config.Version = RomVersion.PAL; };
            */
        }

        internal void Update(bool updateView)
        {
            if (!updateView) return;


        }
    }
}
