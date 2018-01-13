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
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class AreaManager : DataManager
    {
        public static AreaManager Instance = null;

        public uint SelectedAreaAddress { get {return _selectedAreaAddress; } }
        private uint _selectedAreaAddress;

        List<RadioButton> _selectedAreaRadioButtons;
        CheckBox _selectCurrentAreaCheckbox;

        public AreaManager(Control tabControl, List<WatchVariableControl> variables, WatchVariablePanel watchVariableLayoutPanel) 
            : base(variables, watchVariableLayoutPanel)
        {
            Instance = this;

            _selectedAreaAddress = AreaUtilities.GetAreaAddress(0);

            SplitContainer splitContainerArea = tabControl.Controls["splitContainerArea"] as SplitContainer;

            _selectedAreaRadioButtons = new List<RadioButton>();
            for (int i = 0; i < 8; i++)
            {
                _selectedAreaRadioButtons.Add(splitContainerArea.Panel1.Controls["radioButtonArea" + i] as RadioButton);
            }
            _selectCurrentAreaCheckbox = splitContainerArea.Panel1.Controls["checkBoxSelectCurrentArea"] as CheckBox;

            for (int i = 0; i < _selectedAreaRadioButtons.Count; i++)
            {
                int index = i;
                _selectedAreaRadioButtons[i].Click += (sender, e) =>
                {
                    _selectCurrentAreaCheckbox.Checked = false;
                    _selectedAreaAddress = AreaUtilities.GetAreaAddress(index);
                };
            }
        }

        public override void Update(bool updateView)
        {
            if (_selectCurrentAreaCheckbox.Checked)
            {
                _selectedAreaAddress = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
            }

            if (!updateView) return;

            base.Update(updateView);

            int? currentAreaIndex = AreaUtilities.GetAreaIndex(_selectedAreaAddress);
            for (int i = 0; i < _selectedAreaRadioButtons.Count; i++)
            {
                _selectedAreaRadioButtons[i].Checked = i == currentAreaIndex;
            }
        }
    }
}
