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

namespace STROOP.M64Editor
{
    public class M64File
    {
        private readonly M64Gui _gui;

        public string CurrentFilePath { get; private set; }
        public string CurrentFileName { get; private set; }
        public byte[] RawBytes { get; private set; }
        public int OriginalFrameCount { get; private set; }

        public M64Header Header { get; }
        public BindingList<M64InputFrame> Inputs { get; }
        public M64Stats Stats { get; }

        public M64File(M64Gui gui)
        {
            _gui = gui;
            Header = new M64Header();
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

            OriginalFrameCount = Header.NumInputs;
            for (int i = 0; i < frameBytes.Length && i < 4 * OriginalFrameCount; i += 4)
            {
                Inputs.Add(new M64InputFrame(i / 4, BitConverter.ToUInt32(frameBytes, i)));
            }

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
            if (CurrentFilePath == null) return false;
            return Save(CurrentFilePath);
        }

        public bool Save(string filePath)
        {
            try
            {
                File.WriteAllBytes(filePath, ToBytes());
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void Close()
        {
            CurrentFilePath = null;
            CurrentFileName = null;
            RawBytes = null;
            OriginalFrameCount = 0;
            Header.Clear();
            Inputs.Clear();
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

            RefreshInputFrames(startIndex);
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
        }

        public void Paste(M64CopiedData copiedData, int index, bool insert, int multiplicity)
        {
            int pasteCount = copiedData.TotalFrames * multiplicity;
            if (insert)
            {
                for (int i = 0; i < pasteCount; i++)
                {
                    Inputs.Insert(index + i, new M64InputFrame(0, 0));
                    _gui.DataGridViewInputs.Rows[index + i].DefaultCellStyle.BackColor = Color.FromArgb(186, 255, 166);
                }
            }
            List<M64InputFrame> inputsToOverwrite = Inputs.Skip(index).Take(pasteCount).ToList();
            copiedData.Apply(inputsToOverwrite);
            RefreshInputFrames(index);
            _gui.DataGridViewInputs.Refresh();
            Config.M64Manager.UpdateSelectionTextboxes();
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
