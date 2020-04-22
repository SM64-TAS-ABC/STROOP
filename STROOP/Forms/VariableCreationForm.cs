using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Extensions;
using STROOP.Utilities;
using STROOP.Controls;

namespace STROOP.Forms
{
    public partial class VariableCreationForm : Form
    {
        public VariableCreationForm()
        {
            InitializeComponent();
            comboBoxTypeValue.DataSource = TypeUtilities.InGameTypeList;
            comboBoxBaseValue.DataSource = Enum.GetValues(typeof(BaseAddressTypeEnum));
        }

        public void Initialize(WatchVariableFlowLayoutPanel varPanel)
        {
            buttonAddVariable.Click += (sender, e) =>
            {
                WatchVariableControl control = CreateWatchVariableControl();
                varPanel.AddVariable(control);
            };
        }

        private WatchVariableControl CreateWatchVariableControl()
        {
            string name = textBoxNameValue.Text;
            string memoryType = comboBoxTypeValue.SelectedItem.ToString();
            BaseAddressTypeEnum baseAddressType = (BaseAddressTypeEnum)comboBoxBaseValue.SelectedItem;
            uint offset = ParsingUtilities.ParseHexNullable(textBoxOffsetValue.Text) ?? 0;

            bool useOffsetDefault =
                baseAddressType != BaseAddressTypeEnum.Absolute &&
                baseAddressType != BaseAddressTypeEnum.Relative &&
                baseAddressType != BaseAddressTypeEnum.None;

            WatchVariable watchVar = new WatchVariable(
                memoryTypeName: memoryType,
                specialType: null,
                baseAddressType: baseAddressType,
                offsetUS: useOffsetDefault ? (uint?)null : offset,
                offsetJP: useOffsetDefault ? (uint?)null : offset,
                offsetSH: useOffsetDefault ? (uint?)null : offset,
                offsetDefault: useOffsetDefault ? offset : (uint?)null,
                mask: null,
                shift: null);
            WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                name: name,
                watchVar: watchVar,
                subclass: WatchVariableSubclass.Number,
                backgroundColor: null,
                displayType: null,
                roundingLimit: null,
                useHex: null,
                invertBool: null,
                isYaw: null,
                coordinate: null,
                groupList: new List<VariableGroup>() { VariableGroup.Custom });

            return precursor.CreateWatchVariableControl();
        }
    }
}
