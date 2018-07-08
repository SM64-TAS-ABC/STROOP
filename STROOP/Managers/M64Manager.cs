using STROOP.M64;
using STROOP.Structs;
using STROOP.Structs.Gui;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Managers
{
    public class M64Manager
    {
        private bool _displaySaveChangesOnOpen = false;
        private readonly M64File _m64File;
        private readonly M64Gui _gui;

        private ushort? _copiedCountryCode = null;
        private uint? _copiedCrc32 = null;

        public M64Manager(M64Gui gui)
        {
            _gui = gui;

            _gui.ButtonSave.Click += (sender, e) => Save();
            _gui.ButtonSaveAs.Click += (sender, e) => SaveAs();
            _gui.ButtonResetChanges.Click += (sender, e) => _m64File.ResetChanges();
            _gui.ButtonOpen.Click += (sender, e) => Open();
            _gui.ButtonClose.Click += (sender, e) => Close();

            _gui.ButtonGoto.Click += (sender, e) => Goto();
            _gui.TextBoxGoto.AddEnterAction(() => Goto());

            _gui.ButtonSetUsRom.Click += (sender, e) => SetHeaderRomVersion(RomVersion.US);
            _gui.ButtonSetJpRom.Click += (sender, e) => SetHeaderRomVersion(RomVersion.JP);
            _gui.ButtonCopyRom.Click += (sender, e) => CopyHeaderRomVersion();
            _gui.ButtonPasteRom.Click += (sender, e) => PasteHeaderRomVersion();

            _gui.DataGridViewInputs.DataError += (sender, e) => _gui.DataGridViewInputs.CancelEdit();
            _gui.DataGridViewInputs.SelectionChanged += (sender, e) => UpdateSelectionTextboxes();
            _gui.DataGridViewInputs.CellContentClick += (sender, e) =>
            {
                if (e.ColumnIndex >= 4)
                {
                    _gui.DataGridViewInputs.ClearSelection();
                    _gui.DataGridViewInputs.Parent.Focus();
                }
            };

            _m64File = new M64File(_gui);
            _gui.DataGridViewInputs.DataSource = _m64File.Inputs;
            UpdateTableSettings();
            _gui.PropertyGridHeader.SelectedObject = _m64File.Header;
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.SelectedObject = _m64File.Stats;
            _gui.PropertyGridStats.Refresh();
            _gui.PropertyGridStats.ContextMenuStrip = _m64File.Stats.CreateContextMenuStrip();
            _gui.TabControlDetails.SelectedIndexChanged += TabControlDetails_SelectedIndexChanged;

            _gui.ButtonTurnOffRowRange.Click += (sender, e) => SetValuesOfSelection(CellSelectionType.RowRange, false);
            _gui.ButtonTurnOffInputRange.Click += (sender, e) => SetValuesOfSelection(CellSelectionType.PartialRowRange, false);
            _gui.ButtonTurnOffCells.Click += (sender, e) => SetValuesOfSelection(CellSelectionType.Cells, false);
            _gui.ButtonTurnOnInputRange.Click += (sender, e) => SetValuesOfSelection(CellSelectionType.PartialRowRange, true);
            _gui.ButtonTurnOnCells.Click += (sender, e) => SetValuesOfSelection(CellSelectionType.Cells, true);

            _gui.ButtonDeleteRowRange.Click += (sender, e) => DeleteRows();

            _gui.ButtonCopyInputRange.Click += (sender, e) => CopyData(false);
            _gui.ButtonCopyRowRange.Click += (sender, e) => CopyData(true);
            _gui.ButtonPasteInsert.Click += (sender, e) => PasteData(true);
            _gui.ButtonPasteOverwrite.Click += (sender, e) => PasteData(false);

            _gui.ListBoxCopied.Items.Add(M64CopiedData.OneEmptyFrame);
            _gui.ListBoxCopied.SelectedItem = M64CopiedData.OneEmptyFrame;
            _gui.ListBoxCopied.KeyDown += (sender, e) => ListBoxCopied_KeyDown();

            _gui.ComboBoxFrameInputRelation.DataSource = Enum.GetValues(typeof(FrameInputRelationType));
            _gui.ComboBoxFrameInputRelation.SelectedItem = M64Config.FrameInputRelation;

            _gui.ButtonQuickDuplicationDuplicate.Click += (sender, e) => PerformQuickDuplication();
            _gui.ButtonAddPauseBufferFrames.Click += (sender, e) => AddPauseBufferFrames();

            _gui.ProgressBar.Visible = false;
            _gui.LabelProgressBar.Visible = false;
        }

        private void DeleteRows()
        {
            (int? startFrame, int? endFrame) = GetFrameBounds();
            if (!startFrame.HasValue || !endFrame.HasValue) return;
            _m64File.DeleteRows(startFrame.Value, endFrame.Value);
        }

        private void PasteData(bool insert)
        {
            M64CopiedData copiedData = _gui.ListBoxCopied.SelectedItem as M64CopiedData;
            if (copiedData == null) return;
            (int? startFrame, int? endFrame) = GetFrameBounds();
            if (!startFrame.HasValue) return;
            int? multiplicity = ParsingUtilities.ParseIntNullable(_gui.TextBoxPasteMultiplicity.Text);
            if (!multiplicity.HasValue) return;
            _m64File.Paste(copiedData, startFrame.Value, insert, multiplicity.Value);
        }

        private void CopyData(bool useRow)
        {
            (int? startFrame, int? endFrame) = GetFrameBounds();
            string inputsString = _gui.TextBoxSelectionInputs.Text;
            if (!startFrame.HasValue || !endFrame.HasValue) return;
            M64CopiedData copiedData = M64CopiedData.CreateCopiedData(
                _gui.DataGridViewInputs, _m64File.CurrentFileName,
                startFrame.Value, endFrame.Value, useRow, inputsString);
            if (copiedData == null) return;
            _gui.ListBoxCopied.Items.Add(copiedData);
            _gui.ListBoxCopied.SelectedItem = copiedData;
        }

        private void CopyHeaderRomVersion()
        {
            if (_m64File.RawBytes == null) return;
            _copiedCountryCode = _m64File.Header.CountryCode;
            _copiedCrc32 = _m64File.Header.Crc32;
        }

        private void PasteHeaderRomVersion()
        {
            if (_m64File.RawBytes == null) return;
            if (!_copiedCountryCode.HasValue || !_copiedCrc32.HasValue) return;
            _m64File.Header.CountryCode = _copiedCountryCode.Value;
            _m64File.Header.Crc32 = _copiedCrc32.Value;
            _gui.PropertyGridHeader.Refresh();
        }

        private void SetHeaderRomVersion(RomVersion romVersion)
        {
            if (_m64File.RawBytes == null) return;
            switch (romVersion)
            {
                case RomVersion.US:
                    _m64File.Header.CountryCode = M64Config.CountryCodeUS;
                    _m64File.Header.Crc32 = M64Config.CrcUS;
                    break;
                case RomVersion.JP:
                    _m64File.Header.CountryCode = M64Config.CountryCodeJP;
                    _m64File.Header.Crc32 = M64Config.CrcJP;
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

        public void UpdateSelectionTextboxes()
        {
            List<M64InputCell> cells = M64Utilities.GetSelectedInputCells(
                _gui.DataGridViewInputs, CellSelectionType.Cells);
            (int? minFrame, int? maxFrame, string inputsString) = M64Utilities.GetCellStats(cells, true);
            if (minFrame.HasValue) _gui.TextBoxSelectionStartFrame.Text = minFrame.Value.ToString();
            if (maxFrame.HasValue) _gui.TextBoxSelectionEndFrame.Text = maxFrame.Value.ToString();
            _gui.TextBoxSelectionInputs.Text = inputsString;
        }

        private void SetValuesOfSelection(CellSelectionType cellSelectionType, bool value)
        {
            (int? startFrame, int? endFrame) = GetFrameBounds();
            List<M64InputCell> cells = M64Utilities.GetSelectedInputCells(
                _gui.DataGridViewInputs,
                cellSelectionType,
                startFrame,
                endFrame,
                _gui.TextBoxSelectionInputs.Text);
            int? intOnValue = ParsingUtilities.ParseIntNullable(_gui.TextBoxOnValue.Text);
            cells.ForEach(cell => cell.SetValue(value, intOnValue));
            _gui.DataGridViewInputs.Refresh();
        }

        public void Goto(int? gotoValueNullable = null)
        {
            gotoValueNullable = gotoValueNullable ?? ParsingUtilities.ParseIntNullable(_gui.TextBoxGoto.Text);
            if (gotoValueNullable.HasValue)
            {
                int gotoValue = M64Utilities.ConvertDisplayedValueToFrame(gotoValueNullable.Value);
                ControlUtilities.TableGoTo(_gui.DataGridViewInputs, gotoValue);
            }
        }

        private void SaveAs()
        {
            SaveFileDialog saveFileDialog = DialogUtilities.CreateSaveFileDialog(FileType.MupenMovie);
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            string filePath = saveFileDialog.FileName;
            string fileName = new FileInfo(filePath).Name;
            bool success = _m64File.Save(filePath, fileName);
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

        private void Save()
        {
            bool success = _m64File.Save();
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

        private void Open()
        {
            if (CheckSaveChanges() == DialogResult.Cancel)
                return;

            OpenFileDialog openFileDialog = DialogUtilities.CreateOpenFileDialog(FileType.MupenMovie);
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK)
                return;

            string filePath = openFileDialog.FileName;
            string fileName = openFileDialog.SafeFileName;
            Open(filePath, fileName);
        }

        public void Open(string filePath, string fileName)
        {
            _gui.DataGridViewInputs.DataSource = null;
            _gui.PropertyGridHeader.SelectedObject = null;
            bool success = _m64File.OpenFile(filePath, fileName);
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
            _gui.DataGridViewInputs.DataSource = _m64File.Inputs;
            UpdateTableSettings();
            _gui.PropertyGridHeader.SelectedObject = _m64File.Header;
            _gui.DataGridViewInputs.Refresh();
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.Refresh();
        }

        private void Close()
        {
            _m64File.Close();
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

        private void ListBoxCopied_KeyDown()
        {
            if (KeyboardUtilities.IsDeletishKeyHeld())
            {
                M64CopiedData copiedData = _gui.ListBoxCopied.SelectedItem as M64CopiedData;
                if (copiedData == null || copiedData == M64CopiedData.OneEmptyFrame) return;
                int index = _gui.ListBoxCopied.SelectedIndex;
                _gui.ListBoxCopied.Items.Remove(copiedData);
                if (index == _gui.ListBoxCopied.Items.Count) index--;
                _gui.ListBoxCopied.SelectedIndex = index;
            }
        }

        private void PerformQuickDuplication()
        {
            int? iter1StartObserved = ParsingUtilities.ParseIntNullable(
                _gui.TextBoxQuickDuplication1stIterationStart.Text);
            int? iter2StartObserved = ParsingUtilities.ParseIntNullable(
                _gui.TextBoxQuickDuplication2ndIterationStart.Text);
            int? totalIters = ParsingUtilities.ParseIntNullable(
                _gui.TextBoxQuickDuplicationTotalIterations.Text);
            if (!iter1StartObserved.HasValue ||
                !iter2StartObserved.HasValue ||
                !totalIters.HasValue) return;

            int iter1Start = iter1StartObserved.Value - 1;
            int iter2Start = iter2StartObserved.Value - 1;
            int multiplicity = totalIters.Value - 1;
            int iter1End = iter2Start - 1;

            M64CopiedData copiedData = M64CopiedData.CreateCopiedData(
                _gui.DataGridViewInputs, _m64File.CurrentFileName,
                iter1Start, iter1End, true /* useRow */);
            _m64File.Paste(copiedData, iter2Start, true /* insert */, multiplicity);
        }

        private void AddPauseBufferFrames()
        {
            (int? startFrameNullable, int? endFrameNullable) = GetFrameBounds();
            if (!startFrameNullable.HasValue || !endFrameNullable.HasValue) return;
            int startFrame = startFrameNullable.Value;
            int endFrame = endFrameNullable.Value;
            _m64File.AddPauseBufferFrames(startFrame, endFrame);
        }

        private (int? startFrame, int? endFrame) GetFrameBounds()
        {
            int? startFrame = ParsingUtilities.ParseIntNullable(_gui.TextBoxSelectionStartFrame.Text);
            int? endFrame = ParsingUtilities.ParseIntNullable(_gui.TextBoxSelectionEndFrame.Text);
            if (startFrame.HasValue) startFrame = M64Utilities.ConvertDisplayedValueToFrame(startFrame.Value);
            if (endFrame.HasValue) endFrame = M64Utilities.ConvertDisplayedValueToFrame(endFrame.Value);
            return (startFrame, endFrame);
        }

        public void UpdateTableSettings(IEnumerable<M64InputFrame> modifiedFrames = null)
        {
            DataGridView table = _gui.DataGridViewInputs;
            if (table.Columns.Count != M64Utilities.ColumnParameters.Count)
                throw new ArgumentOutOfRangeException();

            if (modifiedFrames != null)
            {
                foreach (M64InputFrame input in modifiedFrames)
                {
                    input.UpdateRowColor();
                    input.UpdateCellColors();
                }
            }

            for (int i = 0; i < table.Columns.Count; i++)
            {
                (string headerText, int fillWeight, Color backColor) = M64Utilities.ColumnParameters[i];
                table.Columns[i].HeaderText = headerText;
                table.Columns[i].FillWeight = fillWeight;
                table.Columns[i].DefaultCellStyle.BackColor = backColor;
                table.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;

            string fileName = _m64File.CurrentFileName ?? "(No File Opened)";
            string isModifiedSuffix = _m64File.IsModified ? " [MODIFIED]" : "";
            _gui.LabelFileName.Text = fileName + isModifiedSuffix;

            int currentFrameCount = _m64File.Inputs.Count;
            int originalFrameCount = _m64File.OriginalFrameCount;
            int frameCountDiff = currentFrameCount - originalFrameCount;
            _gui.LabelNumInputsValue.Text = String.Format(
                "{0} / {1} [{2}]",
                currentFrameCount,
                originalFrameCount,
                StringUtilities.FormatIntegerWithSign(frameCountDiff));

            FrameInputRelationType selectedFrameInputRelation =
                (FrameInputRelationType)_gui.ComboBoxFrameInputRelation.SelectedItem;
            if (selectedFrameInputRelation != M64Config.FrameInputRelation)
            {
                M64Config.FrameInputRelation = selectedFrameInputRelation;
                _gui.DataGridViewInputs.Refresh();
                UpdateSelectionTextboxes();
            }
        }
    }
}
