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
        private RichTextBox _richTextBoxMemory;

        public uint? Address { get; private set; }

        public MemoryManager(TabPage tabControl)
        {
            _textBoxMemoryStartAddress = tabControl.Controls["textBoxMemoryStartAddress"] as BetterTextbox;
            _buttonMemoryButtonGo = tabControl.Controls["buttonMemoryButtonGo"] as Button;
            _checkBoxMemoryUpdateContinuously = tabControl.Controls["checkBoxMemoryUpdateContinuously"] as CheckBox;
            _checkBoxMemoryLittleEndian = tabControl.Controls["checkBoxMemoryLittleEndian"] as CheckBox;
            _richTextBoxMemory = tabControl.Controls["richTextBoxMemory"] as RichTextBox;

            _textBoxMemoryStartAddress.AddEnterAction(() => TryToSetAddressAndUpdateMemory());
            _buttonMemoryButtonGo.Click += (sender, e) => TryToSetAddressAndUpdateMemory();

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
            _richTextBoxMemory.Text = FormatBytesForHexEditorDisplay(bytes, littleEndian);
        }

        private string FormatBytesForHexEditorDisplay(byte[] bytes, bool littleEndian)
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

        public void Update(bool updateView)
        {
            if (!updateView) return;

            if (_checkBoxMemoryUpdateContinuously.Checked) UpdateMemory();
        }
    }
}
