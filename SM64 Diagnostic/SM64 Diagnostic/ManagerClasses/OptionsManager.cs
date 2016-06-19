using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;

namespace SM64_Diagnostic.ManagerClasses
{
    public class OptionsManager
    {
        Config _config;
        OptionsGui _gui;

        public OptionsManager(OptionsGui gui, Config config)
        {
            _config = config;
            _gui = gui;

            _gui.CheckBoxStartFromOne.CheckedChanged += CheckBoxStartFromOne_CheckedChanged;

        }

        private void CheckBoxStartFromOne_CheckedChanged(object sender, EventArgs e)
        {
            _config.SlotIndexsFromOne = _gui.CheckBoxStartFromOne.Checked;
        }
    }
}
