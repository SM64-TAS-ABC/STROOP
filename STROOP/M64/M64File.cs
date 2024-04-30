﻿using System;
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
    public class M64File
    {
        private readonly M64Gui _gui;

        public string CurrentFilePath { get; private set; }
        public string CurrentFileName { get; private set; }
        public byte[] RawBytes { get; private set; }
        public uint OriginalFrameCount { get; private set; }

        public bool IsModified = false;
        public readonly HashSet<M64InputFrame> ModifiedFrames = new HashSet<M64InputFrame>();

        public M64Header Header { get; }
        public BindingList<M64InputFrame> Inputs { get; }
        public M64Stats Stats { get; }

        public M64File(M64Gui gui)
        {
            _gui = gui;
            Header = new M64Header(this, gui);
            Inputs = new BindingList<M64InputFrame>();
            Stats = new M64Stats(this);
        }

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

        private bool LoadBytes(byte[] fileBytes)
        {
            // Check Header
            if (!fileBytes.Take(4).SequenceEqual(M64Config.SignatureBytes))
                return false;

            if (fileBytes.Length < M64Config.HeaderSize)
                return false;

            M64InputFrame.ClassIdIndex = 0;
            RawBytes = fileBytes;
            Inputs.Clear();
            byte[] headerBytes = fileBytes.Take(M64Config.HeaderSize).ToArray();
            Header.LoadBytes(headerBytes);
            byte[] frameBytes = fileBytes.Skip(M64Config.HeaderSize).ToArray();

            IsModified = false;
            ModifiedFrames.Clear();
            OriginalFrameCount = (uint)Header.NumInputs;
            for (int i = 0; i < frameBytes.Length && i < 4 * OriginalFrameCount; i += 4)
            {
                Inputs.Add(new M64InputFrame(
                    i / 4, BitConverter.ToUInt32(frameBytes, i), true, this, _gui.DataGridViewInputs));
            }
            _gui.DataGridViewInputs.Refresh();
            _gui.PropertyGridHeader.Refresh();
            _gui.PropertyGridStats.Refresh();

            return true;
        } 

        private byte[] ToBytes()
        {
            byte[] headerBytes = Header.ToBytes();
            byte[] inputBytes = Inputs.SelectMany(input => input.ToBytes()).ToArray();
            return headerBytes.Concat(inputBytes).ToArray();
        }

        public bool Save()
        {
            if (RawBytes == null) return false;
            if (CurrentFilePath == null || CurrentFileName == null) return false;
            return Save(CurrentFilePath, CurrentFileName);
        }

        public bool Save(string filePath, string fileName)
        {
            if (RawBytes == null) return false;
            try
            {
                if (_gui.CheckBoxMaxOutViCount.Checked)
                    Header.NumVis = uint.MaxValue;
                DialogUtilities.WriteFileBytes(filePath, ToBytes());
                int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
                Config.M64Manager.Open(filePath, fileName);
                Config.M64Manager.Goto(currentPosition);
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

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

        public void ResetChanges()
        {
            if (RawBytes == null) return;
            int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
            Config.M64Manager.Open(CurrentFilePath, CurrentFileName);
            Config.M64Manager.Goto(currentPosition);
        }

        public void DeleteRows(int startIndex, int endIndex)
        {
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

            IsModified = true;
            Header.NumInputs = (uint)Inputs.Count;
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        public void Paste(M64CopiedData copiedData, int index, bool insert, int multiplicity)
        {
            if (RawBytes == null) return;
            index = MoreMath.Clamp(index, 0, Inputs.Count);
            int pasteCount = copiedData.TotalFrames * multiplicity;
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
                List<M64InputFrame> inputsToOverwrite = Inputs.Skip(index).Take(pasteCount).ToList();
                copiedData.Apply(inputsToOverwrite);
            }

            if (bigPaste)
            {
                SetPasteProgressVisibility(false);
            }

            IsModified = true;
            Header.NumInputs = (uint)Inputs.Count;
            RefreshInputFrames(index);
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        public void AddPauseBufferFrames(int startIndex, int endIndex)
        {
            if (RawBytes == null) return;
            if (startIndex > endIndex) return;
            startIndex = MoreMath.Clamp(startIndex, 0, Inputs.Count - 1);
            endIndex = MoreMath.Clamp(endIndex, 0, Inputs.Count - 1);

            for (int index = startIndex; index <= endIndex; index++)
            {
                M64CopiedData.OnePauseFrameOverwrite.Apply(Inputs[index]);
            }

            int currentPosition = _gui.DataGridViewInputs.FirstDisplayedScrollingRowIndex;
            _gui.DataGridViewInputs.DataSource = null;

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

            RefreshInputFrames(startIndex);
            _gui.DataGridViewInputs.DataSource = Inputs;
            Config.M64Manager.UpdateTableSettings(ModifiedFrames);
            ControlUtilities.TableGoTo(_gui.DataGridViewInputs, currentPosition);

            IsModified = true;
            Header.NumInputs = (uint)Inputs.Count;
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        private void SetPasteProgressVisibility(bool visibility)
        {
            _gui.LabelProgressBar.Visible = visibility;
            _gui.LabelProgressBar.Update();
            _gui.ProgressBar.Visible = visibility;
            _gui.ProgressBar.Update();
        }

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

        private void RefreshInputFrames(int startIndex = 0)
        {
            for (int i = startIndex; i < Inputs.Count; i++)
            {
                Inputs[i].FrameIndex = i;
            }
        }
    }
}
