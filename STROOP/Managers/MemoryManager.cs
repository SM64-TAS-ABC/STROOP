using STROOP.Controls;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class MemoryManager
    {
        private BetterTextbox _textBoxMemoryStartAddress;
        private Button _buttonMemoryButtonGo;
        private CheckBox _checkBoxMemoryUpdateContinuously;
        private CheckBox _checkBoxMemoryLittleEndian;
        private ComboBox _comboBoxMemoryTypes;

        private RichTextBox _richTextBoxMemoryAddresses;
        private RichTextBox _richTextBoxMemoryBytes;
        private RichTextBox _richTextBoxMemoryValues;

        public uint? Address { get; private set; }

        public MemoryManager(TabPage tabControl)
        {
            _textBoxMemoryStartAddress = tabControl.Controls["textBoxMemoryStartAddress"] as BetterTextbox;
            _buttonMemoryButtonGo = tabControl.Controls["buttonMemoryButtonGo"] as Button;
            _checkBoxMemoryUpdateContinuously = tabControl.Controls["checkBoxMemoryUpdateContinuously"] as CheckBox;
            _checkBoxMemoryLittleEndian = tabControl.Controls["checkBoxMemoryLittleEndian"] as CheckBox;
            _comboBoxMemoryTypes = tabControl.Controls["comboBoxMemoryTypes"] as ComboBox;

            _richTextBoxMemoryAddresses = tabControl.Controls["richTextBoxMemoryAddresses"] as RichTextBox;
            _richTextBoxMemoryBytes = tabControl.Controls["richTextBoxMemoryBytes"] as RichTextBox;
            _richTextBoxMemoryValues = tabControl.Controls["richTextBoxMemoryValues"] as RichTextBox;

            _textBoxMemoryStartAddress.AddEnterAction(() => TryToSetAddressAndUpdateMemory());
            _buttonMemoryButtonGo.Click += (sender, e) => TryToSetAddressAndUpdateMemory();

            _comboBoxMemoryTypes.DataSource = TypeUtilities.StringToType.Keys.ToList();

            Address = null;
        }

        private void TryToSetAddressAndUpdateMemory()
        {
            uint? addressNullable = ParsingUtilities.ParseHexNullable(_textBoxMemoryStartAddress.Text);
            if (addressNullable.HasValue) SetAddressAndUpdateMemory(addressNullable.Value);
        }

        public void SetAddressAndUpdateMemory(uint address)
        {
            _textBoxMemoryStartAddress.Text = HexUtilities.Format(address, 8);
            Address = address;
            UpdateMemory();
        }

        private void UpdateMemory()
        {
            if (!Address.HasValue) return;
            byte[] bytes = Config.Stream.ReadRam(Address.Value, (int)ObjectConfig.StructSize);
            bool littleEndian = _checkBoxMemoryLittleEndian.Checked;
            Type type = TypeUtilities.StringToType[(string)_comboBoxMemoryTypes.SelectedItem];
            _richTextBoxMemoryAddresses.Text = FormatAddresses(Address.Value, (int)ObjectConfig.StructSize);
            _richTextBoxMemoryBytes.Text = FormatBytes(bytes, littleEndian);
            _richTextBoxMemoryValues.Text = FormatValues(bytes, type, littleEndian);
        }

        private string FormatAddresses(uint startAddress, int totalMemorySize)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < totalMemorySize; i += 16)
            {
                uint address = startAddress + (uint)i;
                builder.Append(HexUtilities.Format(address, 8));
                builder.Append("\r\n");
            }
            return builder.ToString();
        }

        private string FormatBytes(byte[] bytes, bool littleEndian)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                int byteIndex = i;
                if (littleEndian)
                {
                    int mod = i % 4;
                    int antiMod = 3 - mod;
                    byteIndex = byteIndex - mod + antiMod;
                }
                builder.Append(HexUtilities.Format(bytes[byteIndex], 2, false));
                string whiteSpace = " ";
                if (i % 4 == 3) whiteSpace = "  ";
                if (i % 16 == 15) whiteSpace = "\r\n";
                builder.Append(whiteSpace);
            }
            return builder.ToString();
        }

        private string FormatValues(byte[] bytes, Type type, bool littleEndian)
        {
            int typeSize = TypeUtilities.TypeSize[type];
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += typeSize)
            {
                int byteIndex = i;
                if (true)
                {
                    int mod = i % 4;
                    int antiMod = 3 - mod;
                    byteIndex = byteIndex - mod + antiMod;
                }
                builder.Append(HexUtilities.Format(bytes[byteIndex], 2, false));
                string whiteSpace = " ";
                if (i % 4 == 3) whiteSpace = "  ";
                if (i % 16 == 15) whiteSpace = "\r\n";
                builder.Append(whiteSpace);
            }
            return builder.ToString();
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            if (_checkBoxMemoryUpdateContinuously.Checked) UpdateMemory();
        }
    }
}
