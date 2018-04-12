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

namespace STROOP.M64Editor
{
    public class M64File
    {
        private readonly M64Gui _gui;

        public string CurrentFilePath { get; private set; }
        public string CurrentFileName { get; private set; }
        public byte[] RawBytes { get; private set; }
        public int OriginalFrameCount { get; private set; }

        public bool IsModified = false;

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
                movieBytes = File.ReadAllBytes(filePath);
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
            OriginalFrameCount = Header.NumInputs;
            for (int i = 0; i < frameBytes.Length && i < 4 * OriginalFrameCount; i += 4)
            {
                Inputs.Add(new M64InputFrame(i / 4, BitConverter.ToUInt32(frameBytes, i), this, _gui.DataGridViewInputs));
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
            if (CurrentFilePath == null || CurrentFileName == null) return false;
            return Save(CurrentFilePath, CurrentFileName);
        }

        public bool Save(string filePath, string fileName)
        {
            try
            {
                if (_gui.CheckBoxMaxOutViCount.Checked)
                    Header.NumVis = int.MaxValue;
                File.WriteAllBytes(filePath, ToBytes());
                OpenFile(filePath, fileName);
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
            OpenFile(CurrentFilePath, CurrentFileName);
        }

        public void DeleteRows(int startIndex, int endIndex)
        {
            startIndex = Math.Max(startIndex, 0);
            endIndex = Math.Min(endIndex, Inputs.Count - 1);
            int numDeletes = endIndex - startIndex + 1;
            if (numDeletes <= 0) return;

            for (int i = 0; i < numDeletes; i++)
            {
                Inputs.RemoveAt(startIndex);
            }

            IsModified = true;
            Header.NumInputs = Inputs.Count;
            RefreshInputFrames(startIndex);
            _gui.DataGridViewInputs.Refresh();
        }

        public void Paste(M64CopiedData copiedData, int index, bool insert, int multiplicity)
        {
            index = MoreMath.Clamp(index, 0, Inputs.Count);
            int pasteCount = copiedData.TotalFrames * multiplicity;
            if (insert)
            {
                for (int i = 0; i < pasteCount; i++)
                {
                    int insertionIndex = index + i;
                    Inputs.Insert(
                        insertionIndex,
                        new M64InputFrame(insertionIndex, copiedData.GetRawValue(i), this, _gui.DataGridViewInputs));
                    _gui.DataGridViewInputs.Rows[insertionIndex].DefaultCellStyle.BackColor = M64Utilities.NewRowColor;
                }
            }
            else
            {
                List<M64InputFrame> inputsToOverwrite = Inputs.Skip(index).Take(pasteCount).ToList();
                copiedData.Apply(inputsToOverwrite);
            }

            IsModified = true;
            Header.NumInputs = Inputs.Count;
            RefreshInputFrames(index);
            _gui.DataGridViewInputs.Refresh();
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
