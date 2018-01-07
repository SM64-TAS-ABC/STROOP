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

        public uint SelectedAreaAddress { get {return _selectedAreaAddress; } }
        private uint _selectedAreaAddress;

        List<RadioButton> _selectedAreaRadioButtons;
        CheckBox _selectCurrentAreaCheckbox;

        public AreaManager(Control tabControl, List<WatchVariable> areaWatchVars, NoTearFlowLayoutPanel noTearFlowLayoutPanel) 
            : base(areaWatchVars, noTearFlowLayoutPanel)
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

        protected override List<SpecialWatchVariable> _specialWatchVars { get; } = new List<SpecialWatchVariable>()
        {
            new SpecialWatchVariable("CurrentAreaIndexMario"),
            new SpecialWatchVariable("CurrentAreaIndex"),
            new SpecialWatchVariable("AreaTerrainDescription"),
        };

        public void ProcessSpecialVars()
        {
            foreach (var specialVar in _specialDataControls)
            {
                switch (specialVar.SpecialName)
                {
                    case "CurrentAreaIndexMario":
                        uint currentAreaMario = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
                        (specialVar as DataContainer).Text = AreaUtilities.GetAreaIndex(currentAreaMario).ToString();
                        break;

                    case "CurrentAreaIndex":
                        uint currentArea = Config.Stream.GetUInt32(Config.Area.CurrentAreaPointerAddress);
                        (specialVar as DataContainer).Text = AreaUtilities.GetAreaIndex(currentArea).ToString();
                        break;

                    case "AreaTerrainDescription":
                        short terrainType = Config.Stream.GetInt16(_selectedAreaAddress + Config.Area.TerrainTypeOffset);
                        (specialVar as DataContainer).Text = AreaUtilities.GetAreaDescription(terrainType);
                        break;
                }
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
            ProcessSpecialVars();

            int? currentAreaIndex = AreaUtilities.GetAreaIndex(_selectedAreaAddress);
            for (int i = 0; i < _selectedAreaRadioButtons.Count; i++)
            {
                _selectedAreaRadioButtons[i].Checked = i == currentAreaIndex;
            }
        }
    }
}
