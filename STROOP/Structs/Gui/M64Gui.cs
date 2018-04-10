using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs.Gui
{
    public class M64Gui
    {
        public Label LabelFileName;
        public Label LabelNumInputsValue;

        public Button ButtonSave;
        public Button ButtonSaveAs;
        public Button ButtonOpen;
        public Button ButtonClose;
        public Button ButtonGoto;
        public TextBox TextBoxGoto;
        
        public DataGridView DataGridViewInputs;
        public PropertyGrid PropertyGridHeader;
        public PropertyGrid PropertyGridStats;

        public TabControl TabControlDetails;
        public TabPage TabPageInputs;
        public TabPage TabPageHeader;
        public TabPage TabPageStats;

        public Button ButtonSetUsHeader;
        public Button ButtonSetJpHeader;

        public BetterTextbox TextBoxSelectionStartFrame;
        public BetterTextbox TextBoxSelectionEndFrame;
        public BetterTextbox TextBoxSelectionInputs;

        public Button ButtonTurnOffRowRange;
        public Button ButtonTurnOffInputRange;
        public Button ButtonTurnOffCells;
        public Button ButtonDeleteRowRange;
        public Button ButtonTurnOnInputRange;
        public Button ButtonTurnOnCells;
        public Button ButtonCopyRowRange;
        public Button ButtonCopyInputRange;

        public ListBox ListBoxCopied;
        public Button ButtonPasteInsert;
        public Button ButtonPasteOverwrite;
        public BetterTextbox TextBoxPasteMultiplicity;
    }
}
