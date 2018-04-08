using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs.Gui
{
    public class M64EditGui
    {
        public Label LabelFileName;

        public Button ButtonSave;
        public Button ButtonSaveAs;
        public Button ButtonLoad;
        public Button ButtonClose;
        public DataGridView DataGridViewEditor;
        public PropertyGrid PropertyGridHeader;

        public ContextMenuStrip ContextMenuStripEditor;
        public ToolStripMenuItem ToolStripMenuItemInsertNewBefore;
        public ToolStripMenuItem ToolStripMenuItemInsertNewAfter;
        public ToolStripMenuItem ToolStripMenuItemCopy;
        public ToolStripMenuItem ToolStripMenuItemPasteOnto;
        public ToolStripMenuItem ToolStripMenuItemPasteBefore;
        public ToolStripMenuItem ToolStripMenuItemPasteAfter;

        public TextBox TextBoxGoto;
        public Button ButtonGoto;
        public OpenFileDialog OpenFileDialogM64;
        public SaveFileDialog SaveFileDialogM64;
    }
}
