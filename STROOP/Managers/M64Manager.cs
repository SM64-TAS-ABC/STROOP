using STROOP.M64Editor;
using STROOP.Structs;
using STROOP.Structs.Gui;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class M64Manager
    {
        bool _displaySaveChangesOnOpen = false;
        M64File _m64;
        int _lastRow = -1;

        M64Gui _gui;

        public M64Manager(M64Gui gui)
        {
            _gui = gui;

            _gui.ButtonSave.Click += ButtonSave_Click;
            _gui.ButtonSaveAs.Click += ButtonSaveAs_Click;
            _gui.ButtonOpen.Click += ButtonOpen_Click;
            _gui.ButtonClose.Click += ButtonClose_Click;
            _gui.ButtonGoto.Click += ButtonGoto_Click;
            _gui.ButtonSetUsHeader.Click += (sender, e) => SetHeaderRomVersion(RomVersion.US);
            _gui.ButtonSetJpHeader.Click += (sender, e) => SetHeaderRomVersion(RomVersion.JP);

            _gui.DataGridViewInputs.DataError += (sender, e) => _gui.DataGridViewInputs.CancelEdit();
            _gui.DataGridViewInputs.SelectionChanged += DataGridViewEditor_SelectionChanged;

            _m64 = new M64File(() => _gui.DataGridViewInputs.Refresh());
            _gui.DataGridViewInputs.DataSource = _m64.Inputs;
            UpdateTableSettings();
            _gui.PropertyGridHeader.SelectedObject = _m64.Header;
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.SelectedObject = _m64.Stats;
            _gui.PropertyGridStats.Refresh();
            _gui.PropertyGridStats.ContextMenuStrip = _m64.Stats.CreateContextMenuStrip();
            _gui.TabControlDetails.SelectedIndexChanged += TabControlDetails_SelectedIndexChanged;

            _gui.ButtonClearCells.Click += ButtonClearCells_Click;
            _gui.ButtonCopyInputs.Click += (sender, e) => CopyData(false);
            _gui.ButtonCopyRows.Click += (sender, e) => CopyData(true);
            _gui.ButtonPasteInsert.Click += (sender, e) => PasteData(true);
            _gui.ButtonPasteOverwrite.Click += (sender, e) => PasteData(false);

            _gui.ListBoxCopied.Items.Add(M64CopiedData.OneEmptyFrame);
        }

        private void PasteData(bool insert)
        {
            M64CopiedData copiedData = _gui.ListBoxCopied.SelectedItem as M64CopiedData;
            if (copiedData == null) return;
            int? minRowIndex = ControlUtilities.GetMinSelectedRowIndex(_gui.DataGridViewInputs);
            if (!minRowIndex.HasValue) return;
            int? multiplicity = ParsingUtilities.ParseIntNullable(_gui.TextBoxPasteMultiplicity.Text);
            if (!multiplicity.HasValue) return;
            _m64.Paste(copiedData, minRowIndex.Value, insert, multiplicity.Value);
        }

        private void CopyData(bool useRow)
        {
            int? startFrame = ParsingUtilities.ParseIntNullable(_gui.TextBoxSelectionStartFrame.Text);
            int? endFrame = ParsingUtilities.ParseIntNullable(_gui.TextBoxSelectionEndFrame.Text);
            string inputsString = _gui.TextBoxSelectionInputs.Text;

            if (!startFrame.HasValue || !endFrame.HasValue) return;
            M64CopiedData copiedData = M64CopiedData.CreateCopiedData(
                _gui.DataGridViewInputs, _m64.CurrentFileName, startFrame.Value, endFrame.Value, useRow, inputsString);
            if (copiedData == null) return;
            _gui.ListBoxCopied.Items.Add(copiedData);
            _gui.ListBoxCopied.SelectedItem = copiedData;
        }

        private void SetHeaderRomVersion(RomVersion romVersion)
        {
            switch (romVersion)
            {
                case RomVersion.US:
                    _m64.Header.CountryCode = M64Config.CountryCodeUS;
                    _m64.Header.Crc32 = M64Config.CrcUS;
                    break;
                case RomVersion.JP:
                    _m64.Header.CountryCode = M64Config.CountryCodeJP;
                    _m64.Header.Crc32 = M64Config.CrcJP;
                    break;
                case RomVersion.PAL:
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _gui.PropertyGridHeader.Refresh();
        }

        private void TabControlDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_gui.TabControlDetails.SelectedTab == _gui.TabPageInputs)
            {
                _gui.DataGridViewInputs.Refresh();
             }
            else if (_gui.TabControlDetails.SelectedTab == _gui.TabPageHeader)
            {
                ControlUtilities.SetPropertyGridLabelColumnWidth(_gui.PropertyGridHeader, 160);
                _gui.PropertyGridHeader.Refresh();
            }
            else if (_gui.TabControlDetails.SelectedTab == _gui.TabPageStats)
            {
                ControlUtilities.SetPropertyGridLabelColumnWidth(_gui.PropertyGridStats, 160);
                _gui.PropertyGridStats.Refresh();
            }
        }

        private void DataGridViewEditor_SelectionChanged(object sender, EventArgs e)
        {
            List<M64InputCell> cells = M64Utilities.GetSelectedInputCells(_gui.DataGridViewInputs);
            (int minFrame, int maxFrame, string inputsString) = M64Utilities.GetCellStats(cells);
            _gui.TextBoxSelectionStartFrame.Text = minFrame.ToString();
            _gui.TextBoxSelectionEndFrame.Text = maxFrame.ToString();
            _gui.TextBoxSelectionInputs.Text = inputsString;
        }

        private void ButtonClearCells_Click(object sender, EventArgs e)
        {
            List<M64InputCell> cells = M64Utilities.GetSelectedInputCells(_gui.DataGridViewInputs);
            cells.ForEach(cell => cell.Clear());
            _gui.DataGridViewInputs.Refresh();
        }

        private void ButtonGoto_Click(object sender, EventArgs e)
        {
            int value;
            if (!int.TryParse(_gui.TextBoxGoto.Text, out value) || value < 0
                || value >= _gui.DataGridViewInputs.Rows.Count)
                return;

            _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex = value;
        }

        private void ButtonSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = FileUtilities.CreateSaveFileDialog(FileType.MupenMovie);
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            bool success = _m64.Save(saveFileDialog.FileName);
            if (!success)
            {
                MessageBox.Show(
                    "Could not save file.\n" +
                        "Perhaps Mupen is currently editing it.\n" +
                        "Try closing Mupen and trying again.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            bool success = _m64.Save();
            if (!success)
            {
                MessageBox.Show(
                    "Could not save file.\n" +
                        "Perhaps Mupen is currently editing it.\n" +
                        "Try closing Mupen and trying again.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void ButtonOpen_Click(object sender, EventArgs e)
        {
            if (CheckSaveChanges() == DialogResult.Cancel)
                return;

            OpenFileDialog openFileDialog = FileUtilities.CreateOpenFileDialog(FileType.MupenMovie);
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            string filePath = openFileDialog.FileName;
            string fileName = openFileDialog.SafeFileName;

            _gui.DataGridViewInputs.DataSource = null;
            _gui.PropertyGridHeader.SelectedObject = null;
            bool success = _m64.OpenFile(filePath, fileName);
            if (!success)
            {
                MessageBox.Show(
                    "Could not open file " + filePath + ".\n" +
                        "Perhaps Mupen is currently editing it.\n" +
                        "Try closing Mupen and trying again.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            _gui.DataGridViewInputs.DataSource = _m64.Inputs;
            UpdateTableSettings();
            _gui.PropertyGridHeader.SelectedObject = _m64.Header;
            _gui.DataGridViewInputs.Refresh();
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.Refresh();
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            _m64.Close();
            _gui.DataGridViewInputs.Refresh();
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.Refresh();
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
            DataGridView table = _gui.DataGridViewInputs;
            if (table.Columns.Count != M64Utilities.ColumnParameters.Count)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < table.Columns.Count; i++)
            {
                (string headerText, int fillWeight, Color? backColor) = M64Utilities.ColumnParameters[i];
                table.Columns[i].HeaderText = headerText;
                table.Columns[i].FillWeight = fillWeight;
                if (backColor.HasValue) table.Columns[i].DefaultCellStyle.BackColor = backColor.Value;
                table.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            _gui.LabelFileName.Text = _m64.CurrentFileName ?? "(No File Opened)";

            int currentFrameCount = _m64.Inputs.Count;
            int originalFrameCount = _m64.OriginalFrameCount;
            int diff = currentFrameCount - originalFrameCount;
            _gui.LabelNumInputsValue.Text = String.Format(
                "{0} / {1} [{2}]",
                currentFrameCount,
                originalFrameCount,
                StringUtilities.FormatIntegerWithSign(diff));
        }
    }
}
