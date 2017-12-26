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

            _selectedAreaAddress = Config.Area.GetAreaAddress(0);

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
                    _selectedAreaAddress = Config.Area.GetAreaAddress(index);
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
                        uint currentArea1 = Config.Stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.AreaPointerOffset);
                        (specialVar as DataContainer).Text = Config.Area.GetAreaIndex(currentArea1).ToString();
                        break;

                    case "CurrentAreaIndex":
                        uint currentArea2 = Config.Stream.GetUInt32(Config.Area.CurrentAreaPointerAddress);
                        (specialVar as DataContainer).Text = Config.Area.GetAreaIndex(currentArea2).ToString();
                        break;

                    case "AreaTerrainDescription":
                        short terrainType = Config.Stream.GetInt16(_selectedAreaAddress + 0x02);
                        (specialVar as DataContainer).Text = GetAreaDescription(terrainType);
                        break;
                }
            }
        }

        public string GetAreaDescription(int terrainType)
        {
            switch (terrainType)
            {
                case 0:
                    return "Grassy";
                case 1:
                    return "Normal";
                case 2:
                    return "Cold";
                case 3:
                    return "Sandy";
                case 4:
                    return "Spooky";
                case 5:
                    return "Aquatic";
                case 6:
                    return "Slide";
                default:
                    return "Unrecognized";
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

            int? currentAreaIndex = Config.Area.GetAreaIndex(_selectedAreaAddress);
            for (int i = 0; i < _selectedAreaRadioButtons.Count; i++)
            {
                _selectedAreaRadioButtons[i].Checked = i == currentAreaIndex;
            }
        }
    }
}
