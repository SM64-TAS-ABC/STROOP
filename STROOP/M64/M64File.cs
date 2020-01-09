using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Xml.Serialization;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Structs.Gui;
using System.Drawing;
using STROOP.Utilities;

namespace STROOP.M64
{
    /// <summary>
    /// Represents a .m64 file, containing a recording of Nintendo 64 gameplay.
    /// </summary>
    /// <seealso href="http://tasvideos.org/EmulatorResources/Mupen/M64.html"/>
    public class M64File
    {
        private readonly M64Gui _gui;

        /// <summary>
        /// The path to the .m64 file.
        /// </summary>
        /// <value>The current file path.</value>
        public string CurrentFilePath { get; private set; }

        /// <summary>
        /// The name of the .m64 file.
        /// </summary>
        /// <value>The name of the current file.</value>
        public string CurrentFileName { get; private set; }

        /// <summary>
        /// The raw bytes of the file, read directly from the file.
        /// </summary>
        /// <value>The raw bytes.</value>
        public byte[] RawBytes { get; private set; }

        /// <summary>
        /// The number of frames originally in the recording.
        /// </summary>
        /// <value>The original frame count.</value>
        public int OriginalFrameCount { get; private set; }

        /// <summary>
        /// <c>true</c> if the recording has been modified, <c>false</c> otherwise.
        /// </summary>
        public bool IsModified = false;
      
        /// <summary>
        /// A list of frames that have been modified.
        /// </summary>
        public readonly HashSet<M64InputFrame> ModifiedFrames = new HashSet<M64InputFrame>();

        /// <summary>
        /// The header of this .m64 file.
        /// </summary>
        /// <value>The header of the recording.</value>
        public M64Header Header { get; }

        /// <summary>
        /// A list of frames containing inputs.
        /// </summary>
        /// <value>The input frames.</value>
        public BindingList<M64InputFrame> Inputs { get; }

        /// <summary>
        /// Statistics describing the recording.
        /// </summary>
        /// <value>The stats for this file.</value>
        public M64Stats Stats { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:STROOP.M64.M64File"/> class.
        /// </summary>
        /// <param name="gui">GUI that expresses the M64 file.</param>
        public M64File(M64Gui gui)
        {
            _gui = gui;
            Header = new M64Header(this, gui);
            Inputs = new BindingList<M64InputFrame>();
            Stats = new M64Stats(this);
        }

        /// <summary>
        /// Open a file and read the recording from it.
        /// </summary>
        /// <returns><c>true</c>, if file was successfully read, <c>false</c> otherwise.</returns>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="fileName">Name of the file.</param>
        public bool OpenFile(string filePath, string fileName)
        {
            if (!File.Exists(filePath))
                return false;

            byte[] movieBytes;
            try
            {
                movieBytes = DialogUtilities.ReadFileBytes(filePath);
            }
            catch (IOException)
            {
                return false;
            }

            bool loadedSuccessfully = LoadBytes(movieBytes);
            if (loadedSuccessfully)
            {
                CurrentFilePath = filePath;
                CurrentFileName = fileName;
            }

            return true;
        }

        // load the recording from bytes
        private bool LoadBytes(byte[] fileBytes)
        {
            // Check Header
            if (!fileBytes.Take(4).SequenceEqual(M64Config.SignatureBytes))
                return false;

            // if the header can't fit in the file, it's obviously not a valid file
            if (fileBytes.Length < M64Config.HeaderSize)
                return false;

            M64InputFrame.ClassIdIndex = 0;
            RawBytes = fileBytes;
            Inputs.Clear();

            // load the bytes for the header and the frmaes
            byte[] headerBytes = fileBytes.Take(M64Config.HeaderSize).ToArray();
            Header.LoadBytes(headerBytes);
            byte[] frameBytes = fileBytes.Skip(M64Config.HeaderSize).ToArray();

            IsModified = false;
            ModifiedFrames.Clear();
            OriginalFrameCount = Header.NumInputs;
            for (int i = 0; i < frameBytes.Length && i < 4 * OriginalFrameCount; i += 4)
            {
                Inputs.Add(new M64InputFrame(
                    i / 4, BitConverter.ToUInt32(frameBytes, i), true, this, _gui.DataGridViewInputs));
            }

            // refresh the gui
            _gui.DataGridViewInputs.Refresh();
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.Refresh();

            return true;
        } 

        // convert this class to raw bytes
        private byte[] ToBytes()
        {
            byte[] headerBytes = Header.ToBytes();
            byte[] inputBytes = Inputs.SelectMany(input => input.ToBytes()).ToArray();
            return headerBytes.Concat(inputBytes).ToArray();
        }

        /// <summary>
        /// Save the recording to the current file.
        /// </summary>
        /// <returns>Whether or not the file successfully saved.</returns>
        public bool Save()
        {
            if (RawBytes == null) return false;
            if (CurrentFilePath == null || CurrentFileName == null) return false;
            return Save(CurrentFilePath, CurrentFileName);
        }

        /// <summary>
        /// Save the recording to a new path.
        /// </summary>
        /// <returns>The save.</returns>
        /// <param name="filePath">File path to save to.</param>
        /// <param name="fileName">File name to select.</param>
        public bool Save(string filePath, string fileName)
        {
            if (RawBytes == null) return false;
            try
            {
                if (_gui.CheckBoxMaxOutViCount.Checked)
                    Header.NumVis = int.MaxValue;

                // write the bytes from the ToBytes function to a file
                DialogUtilities.WriteFileBytes(filePath, ToBytes());
                int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
                Config.M64Manager.Open(filePath, fileName);
                Config.M64Manager.Goto(currentPosition);
            }
            catch (IOException)
            {
                // if there was an IO error, it should probably be output to the console
                return false;
            }
            return true;
        }

        /// <summary>
        /// Close this recording.
        /// </summary>
        public void Close()
        {
            Header.Clear();
            Inputs.Clear();
            CurrentFilePath = null;
            CurrentFileName = null;
            RawBytes = null;
            OriginalFrameCount = 0;
            IsModified = false;
        }

        /// <summary>
        /// Reset the changes made to this recording.
        /// </summary>
        public void ResetChanges()
        {
            if (RawBytes == null) return;
            int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
            Config.M64Manager.Open(CurrentFilePath, CurrentFileName);
            Config.M64Manager.Goto(currentPosition);
        }

        /// <summary>
        /// Delete inputs from this recording.
        /// </summary>
        /// <param name="startIndex">The start of the range of inputs to delete.</param>
        /// <param name="endIndex">The end of the range of inputs to delete.</param>
        public void DeleteRows(int startIndex, int endIndex)
        {
            // cap the start and end index to a range
            startIndex = Math.Max(startIndex, 0);
            endIndex = Math.Min(endIndex, Inputs.Count - 1);

            int numDeletes = endIndex - startIndex + 1;
            if (numDeletes <= 0) return;

            int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
            _gui.DataGridViewInputs.DataSource = null;
            for (int i = 0; i < numDeletes; i++)
            {
                ModifiedFrames.Remove(Inputs[startIndex]);
                Inputs.RemoveAt(startIndex);
            }
            RefreshInputFrames(startIndex);
            _gui.DataGridViewInputs.DataSource = Inputs;
            Config.M64Manager.UpdateTableSettings(ModifiedFrames);
            ControlUtilities.TableGoTo(_gui.DataGridViewInputs, currentPosition);

            // we have modified
            IsModified = true;
            Header.NumInputs = Inputs.Count;
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        /// <summary>
        /// Paste some copied data into this recording.
        /// </summary>
        /// <param name="copiedData">The data to paste in.</param>
        /// <param name="index">Index.</param>
        /// <param name="insert">If set to <c>true</c> insert.</param>
        /// <param name="multiplicity">Multiplicity.</param>
        public void Paste(M64CopiedData copiedData, int index, bool insert, int multiplicity)
        {
            if (RawBytes == null) return;

            // clamp the index to a range
            index = MoreMath.Clamp(index, 0, Inputs.Count);
            int pasteCount = copiedData.TotalFrames * multiplicity;

            // warn if the user is pasting many frames
            bool bigPaste = pasteCount > M64Config.PasteWarningLimit;
            if (bigPaste)
            {
                if (!DialogUtilities.AskQuestionAboutM64Pasting(pasteCount)) return;
                SetPasteProgressCount(0, pasteCount);
                SetPasteProgressVisibility(true);
            }

            if (insert)
            {
                int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
                _gui.DataGridViewInputs.DataSource = null;
                for (int i = 0; i < pasteCount; i++)
                {
                    // create the new input frame and insert it into the list
                    int insertionIndex = index + i;
                    M64InputFrame newInput = new M64InputFrame(
                        insertionIndex, copiedData.GetRawValue(i), false, this, _gui.DataGridViewInputs);
                    Inputs.Insert(insertionIndex, newInput);
                    ModifiedFrames.Add(newInput);

                    if (bigPaste)
                    {
                        SetPasteProgressCount(i + 1, pasteCount);
                    }
                }
                RefreshInputFrames(index);
                _gui.DataGridViewInputs.DataSource = Inputs;
                Config.M64Manager.UpdateTableSettings(ModifiedFrames);
                ControlUtilities.TableGoTo(_gui.DataGridViewInputs, currentPosition);
            }
            else
            {
                // overwrite the frames with the frames to take
                List<M64InputFrame> inputsToOverwrite = Inputs.Skip(index).Take(pasteCount).ToList();
                copiedData.Apply(inputsToOverwrite);
            }

            if (bigPaste)
            {
                SetPasteProgressVisibility(false);
            }

            // update this rom
            // TODO: this could probably be made into a valid function
            IsModified = true;
            Header.NumInputs = Inputs.Count;
            RefreshInputFrames(index);
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        /// <summary>
        /// Pause buffer several frames, in order to execute techniques like bomb-omb teleportation.
        /// </summary>
        /// <param name="startIndex">The index to start pause buffering at.</param>
        /// <param name="endIndex">The index to end pause buffering at.</param>
        /// <seealso href="https://www.youtube.com/watch?v=IhlBrVaK7DU"/>
        public void AddPauseBufferFrames(int startIndex, int endIndex)
        {
            if (RawBytes == null) return;

            // if the indices are misplaced, return
            if (startIndex > endIndex) return;

            // clamp the indices to a range
            startIndex = MoreMath.Clamp(startIndex, 0, Inputs.Count - 1);
            endIndex = MoreMath.Clamp(endIndex, 0, Inputs.Count - 1);

            // overwrite frames with one pause frame
            for (int index = startIndex; index <= endIndex; index++)
            {
                M64CopiedData.OnePauseFrameOverwrite.Apply(Inputs[index]);
            }

            int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
            _gui.DataGridViewInputs.DataSource = null;

            // insert 3 frames of pause buffering between each frame
            for (int index = startIndex; index <= endIndex; index++)
            {
                int currentFrame = startIndex + (index - startIndex) * 4;

                M64InputFrame newInput1 = new M64InputFrame(
                    currentFrame + 1, M64CopiedData.OneEmptyFrame.GetRawValue(0), false, this, _gui.DataGridViewInputs);
                Inputs.Insert(currentFrame + 1, newInput1);
                ModifiedFrames.Add(newInput1);

                M64InputFrame newInput2 = new M64InputFrame(
                    currentFrame + 2, M64CopiedData.OnePauseFrame.GetRawValue(0), false, this, _gui.DataGridViewInputs);
                Inputs.Insert(currentFrame + 2, newInput2);
                ModifiedFrames.Add(newInput2);

                M64InputFrame newInput3 = new M64InputFrame(
                    currentFrame + 3, M64CopiedData.OneEmptyFrame.GetRawValue(0), false, this, _gui.DataGridViewInputs);
                Inputs.Insert(currentFrame + 3, newInput3);
                ModifiedFrames.Add(newInput3);
            }

            // refresh the gui and mark as modified
            RefreshInputFrames(startIndex);
            _gui.DataGridViewInputs.DataSource = Inputs;
            Config.M64Manager.UpdateTableSettings(ModifiedFrames);
            ControlUtilities.TableGoTo(_gui.DataGridViewInputs, currentPosition);

            IsModified = true;
            Header.NumInputs = Inputs.Count;
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        // set the paste progress visibility
        private void SetPasteProgressVisibility(bool visibility)
        {
            _gui.LabelProgressBar.Visible = visibility;
            _gui.LabelProgressBar.Update();
            _gui.ProgressBar.Visible = visibility;
            _gui.ProgressBar.Update();
        }

        // set the current paste progress
        private void SetPasteProgressCount(int value, int maximum)
        {
            string maximumString = maximum.ToString();
            string valueString = String.Format("{0:D" + maximumString.Length + "}", value);
            double percent = Math.Round(100d * value / maximum, 1);
            string percentString = percent.ToString("N1");
            _gui.LabelProgressBar.Text = String.Format(
                "{0}% ({1} / {2})", percentString, valueString, maximumString);
            _gui.LabelProgressBar.Update();
            _gui.ProgressBar.Maximum = maximum;
            _gui.ProgressBar.Value = value;
            _gui.ProgressBar.Update();
        }

        // make sure each frame has the proper index
        private void RefreshInputFrames(int startIndex = 0)
        {
            for (int i = startIndex; i < Inputs.Count; i++)
            {
                Inputs[i].FrameIndex = i;
            }
        }
    }
}
