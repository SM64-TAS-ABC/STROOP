using STROOP.M64Editor;
using STROOP.Structs.Gui;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class M64EditManager
    {
        bool _displaySaveChangesOnOpen = false;
        M64File _m64;
        int _lastRow = -1;

        M64EditGui _gui;

        public M64EditManager(M64EditGui gui)
        {
            _gui = gui;

            _gui.ButtonSave.Click += (sender, e) => _m64.Save();
            _gui.ButtonSaveAs.Click += ButtonSaveAs_Click;
            _gui.ButtonLoad.Click += ButtonLoad_Click;
            _gui.ButtonGoto.Click += ButtonGoto_Click;

            _gui.ToolStripMenuItemInsertNewAfter.Click += ToolStripMenuItemInsertNewAfter_Click;
            _gui.ToolStripMenuItemInsertNewAfter.Click += ToolStripMenuItemInsertNewAfter_Click;
            _gui.ToolStripMenuItemCopy.Click += ToolStripMenuItemCopy_Click;
            _gui.ToolStripMenuItemPasteOnto.Click += ToolStripMenuItemPasteOnto_Click;
            _gui.ToolStripMenuItemPasteBefore.Click += ToolStripMenuItemPasteBefore_Click;
            _gui.ToolStripMenuItemPasteAfter.Click += ToolStripMenuItemPasteAfter_Click;

            _gui.DataGridViewEditor.MouseClick += DataGridViewEditor_MouseClick;

            _m64 = new M64File();
            _gui.DataGridViewEditor.DataSource = _m64.Inputs;
            UpdateTableSettings();
            _gui.PropertyGridHeader.SelectedObject = _m64.Header;
            _gui.PropertyGridHeader.Refresh();
        }

        private void DataGridViewEditor_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _lastRow = _gui.DataGridViewEditor.HitTest(e.X, e.Y).RowIndex;

                if (_lastRow < 0)
                    return;

                _gui.ContextMenuStripEditor.Show(_gui.DataGridViewEditor, new Point(e.X, e.Y));

            }
        }

        private void ButtonGoto_Click(object sender, EventArgs e)
        {
            int value;
            if (!int.TryParse(_gui.TextBoxGoto.Text, out value) || value < 0
                || value >= _gui.DataGridViewEditor.Rows.Count)
                return;

            _gui.DataGridViewEditor.FirstDisplayedScrollingRowIndex = value;
        }

        private void ToolStripMenuItemPasteAfter_Click(object sender, EventArgs e)
        {
            _m64.PasteInsert(_lastRow + 1);
        }

        private void ToolStripMenuItemPasteBefore_Click(object sender, EventArgs e)
        {
            _m64.PasteInsert(_lastRow);
        }

        private void ToolStripMenuItemPasteOnto_Click(object sender, EventArgs e)
        {
            var rowIndices = new List<int>();
            foreach (DataGridViewRow row in _gui.DataGridViewEditor.SelectedRows)
                rowIndices.Add(row.Index);

            _m64.PasteOnto(rowIndices);
        }

        private void ToolStripMenuItemCopy_Click(object sender, EventArgs e)
        {
            var rowIndices = new List<int>();
            foreach (DataGridViewRow row in _gui.DataGridViewEditor.SelectedRows)
                rowIndices.Add(row.Index);

            _m64.CopyRows(rowIndices);
        }

        private void ToolStripMenuItemInsertNewBefore_Click(object sender, EventArgs e)
        {
            if (_lastRow < 0)
                return;

            _m64.InsertNew(_lastRow);
            _gui.DataGridViewEditor.ClearSelection();
            _gui.DataGridViewEditor.Rows[_lastRow].Selected = true;
        }

        private void ToolStripMenuItemInsertNewAfter_Click(object sender, EventArgs e)
        {
            if (_lastRow < 0)
                return;

            _m64.InsertNew(_lastRow + 1);
            _gui.DataGridViewEditor.ClearSelection();
            _gui.DataGridViewEditor.Rows[_lastRow].Selected = true;
        }

        private void ButtonSaveAs_Click(object sender, EventArgs e)
        {
            var dialogResult = _gui.SaveFileDialogM64.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            _m64.Save(_gui.SaveFileDialogM64.FileName);
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            if (CheckSaveChanges() == DialogResult.Cancel)
                return;

            var dialogResult = _gui.OpenFileDialogM64.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            _gui.DataGridViewEditor.DataSource = null;
            _gui.PropertyGridHeader.SelectedObject = null;
            _m64.LoadFile(_gui.OpenFileDialogM64.FileName);
            _gui.DataGridViewEditor.DataSource = _m64.Inputs;
            UpdateTableSettings();
            _gui.PropertyGridHeader.SelectedObject = _m64.Header;
            _gui.DataGridViewEditor.Refresh();
            _gui.PropertyGridHeader.Refresh();
        }

        private DialogResult CheckSaveChanges()
        {
            if (!_displaySaveChangesOnOpen)
                return DialogResult.OK;

            return MessageBox.Show("Do you want to save changes?", "Save Changes",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        }

        private void UpdateTableSettings()
        {
            DataGridView table = _gui.DataGridViewEditor;
            if (table.Columns.Count != M64InputFrame.ColumnParameters.Count)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                (string headerText, int fillWeight, Color? backColor) = M64InputFrame.ColumnParameters[i];
                table.Columns[i].HeaderText = headerText;
                table.Columns[i].FillWeight = fillWeight;
                if (backColor.HasValue) table.Columns[i].DefaultCellStyle.BackColor = backColor.Value;
                table.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
    }
}
