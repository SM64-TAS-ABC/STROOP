using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Managers
{
    public class OptionsManager
    {
        OptionsGui _gui;

        public OptionsManager(OptionsGui gui)
        {
            _gui = gui;

            _gui.CheckBoxStartFromOne.CheckedChanged += CheckBoxStartFromOne_CheckedChanged;

        }

        private void CheckBoxStartFromOne_CheckedChanged(object sender, EventArgs e)
        {
            Config.SlotIndexsFromOne = _gui.CheckBoxStartFromOne.Checked;
        }
    }
}
