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
        private RichTextBox _richTextBoxMemory;

        public MemoryManager(TabPage tabControl)
        {
            _textBoxMemoryStartAddress = tabControl.Controls["textBoxMemoryStartAddress"] as BetterTextbox;
            _buttonMemoryButtonGo = tabControl.Controls["buttonMemoryButtonGo"] as Button;
            _checkBoxMemoryUpdateContinuously = tabControl.Controls["checkBoxMemoryUpdateContinuously"] as CheckBox;
            _richTextBoxMemory = tabControl.Controls["richTextBoxMemory"] as RichTextBox;
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
           
        }
    }
}
