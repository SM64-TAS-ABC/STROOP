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
        public Button ButtonSave;
        public Button ButtonSaveAs;
        public Button ButtonOpen;
        public Button ButtonClose;
        public Button ButtonGoto;
        public TextBox TextBoxGoto;

        public OpenFileDialog OpenFileDialogM64;
        public SaveFileDialog SaveFileDialogM64;

        public DataGridView DataGridViewInputs;
        public PropertyGrid PropertyGridHeader;
        public PropertyGrid PropertyGridStats;

        public TabControl TabControlDetails;
        public TabPage TabPageInputs;
        public TabPage TabPageHeader;
        public TabPage TabPageStats;
        
        public ContextMenuStrip ContextMenuStripEditor;
        public ToolStripMenuItem ToolStripMenuItemInsertNewBefore;
        public ToolStripMenuItem ToolStripMenuItemInsertNewAfter;
        public ToolStripMenuItem ToolStripMenuItemCopy;
        public ToolStripMenuItem ToolStripMenuItemPasteOnto;
        public ToolStripMenuItem ToolStripMenuItemPasteBefore;
        public ToolStripMenuItem ToolStripMenuItemPasteAfter;

    }
}
