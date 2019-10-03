using STROOP.Controls;
using STROOP.Forms;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace STROOP.Managers
{
    public class SearchManager : DataManager
    {
        private enum ValueRelationship
        {
            EqualTo,
            Between,
            GreaterThan,
            LessThan,
            Increased,
            Decreased,
            IncreasedBy,
            DecreasedBy,
            Unknown,
        };

        private readonly Dictionary<uint, object> _dictionary;
        private Type _memoryType;

        private readonly ComboBox _comboBoxSearchMemoryType;
        private readonly ComboBox _comboBoxValueRelationship;
        private readonly BetterTextbox _textBoxSearchValue;
        private readonly Button _buttonSearchFirstScan;
        private readonly Button _buttonSearchNextScan;
        private readonly Label _labelSearchNumResults;
        private readonly Button _buttonSearchAddSelectedAsVars;
        private readonly Button _buttonSearchAddAllAsVars;
        private readonly DataGridView _dataGridViewSearch;

        public SearchManager(TabPage tabControl, WatchVariableFlowLayoutPanel watchVariablePanel)
            : base(null, watchVariablePanel)
        {
            _dictionary = new Dictionary<uint, object>();
            _memoryType = typeof(byte);

            SplitContainer splitContainerSearch = tabControl.Controls["splitContainerSearch"] as SplitContainer;
            SplitContainer splitContainerSearchOptions = splitContainerSearch.Panel1.Controls["splitContainerSearchOptions"] as SplitContainer;

            _comboBoxSearchMemoryType = splitContainerSearchOptions.Panel1.Controls["comboBoxSearchMemoryType"] as ComboBox;
            _comboBoxSearchMemoryType.DataSource = TypeUtilities.InGameTypeList;

            _comboBoxValueRelationship = splitContainerSearchOptions.Panel1.Controls["comboBoxValueRelationship"] as ComboBox;
            _comboBoxValueRelationship.DataSource = Enum.GetValues(typeof(ValueRelationship));

            _textBoxSearchValue = splitContainerSearchOptions.Panel1.Controls["textBoxSearchValue"] as BetterTextbox;

            _buttonSearchFirstScan = splitContainerSearchOptions.Panel1.Controls["buttonSearchFirstScan"] as Button;
            _buttonSearchFirstScan.Click += (sender, e) => DoFirstScan();

            _buttonSearchNextScan = splitContainerSearchOptions.Panel1.Controls["buttonSearchNextScan"] as Button;
            _buttonSearchNextScan.Click += (sender, e) => DoNextScan();

            _labelSearchNumResults = splitContainerSearchOptions.Panel1.Controls["labelSearchNumResults"] as Label;

            _buttonSearchAddSelectedAsVars = splitContainerSearchOptions.Panel1.Controls["buttonSearchAddSelectedAsVars"] as Button;

            _buttonSearchAddAllAsVars = splitContainerSearchOptions.Panel1.Controls["buttonSearchAddAllAsVars"] as Button;

            _dataGridViewSearch = splitContainerSearchOptions.Panel2.Controls["dataGridViewSearch"] as DataGridView;
        }

        private void DoFirstScan()
        {
            string memoryTypeString = (string)_comboBoxSearchMemoryType.SelectedItem;
            _memoryType = TypeUtilities.StringToType[memoryTypeString];
            int memoryTypeSize = TypeUtilities.TypeSize[_memoryType];

            object searchValue = ParsingUtilities.ParseValueNullable(_textBoxSearchValue.Text, _memoryType);

            _dictionary.Clear();
            for (uint address = 0; address < Config.RamSize - memoryTypeSize; address += (uint)memoryTypeSize)
            {
                object memoryValue = Config.Stream.GetValue(_memoryType, address);
                if (Equals(memoryValue, searchValue))
                {
                    _dictionary[address] = memoryValue;
                }
            }

            UpdateControlsBasedOnDictionary();
        }

        private void DoNextScan()
        {
            object searchValue = ParsingUtilities.ParseValueNullable(_textBoxSearchValue.Text, _memoryType);
            List<uint> addresses = _dictionary.Keys.ToList();

            _dictionary.Clear();
            foreach (uint address in addresses)
            {
                object memoryValue = Config.Stream.GetValue(_memoryType, address);
                if (Equals(memoryValue, searchValue))
                {
                    _dictionary[address] = memoryValue;
                }
            }

            UpdateControlsBasedOnDictionary();
        }

        private void UpdateControlsBasedOnDictionary()
        {
            _labelSearchNumResults.Text = _dictionary.Count.ToString();

            _dataGridViewSearch.Rows.Clear();
            _dictionary.Keys.ToList().ForEach(key =>
            {
                _dataGridViewSearch.Rows.Add(
                    "0x80" + HexUtilities.FormatValue(key, 6, false),
                    _dictionary[key]);
            });
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;

            base.Update(updateView);
        }
    }
}
