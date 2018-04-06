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

namespace STROOP.M64Editor
{
    public class M64File
    {
        byte[] _headerBytes;
        string _currentFile;
        public BindingList<InputFrame> Inputs { get; } = new BindingList<InputFrame>();

        public bool LoadFile(string filePath)
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

            var loaded = LoadMupenFileBytes(movieBytes);

            if (loaded)
                _currentFile = filePath;

            return true;
        }

        private bool LoadMupenFileBytes(byte[] fileBytes)
        {
            // Check Header
            if (!fileBytes.Take(4).SequenceEqual(new byte[] { 0x4D, 0x36, 0x34, 0x1A }))
                return false;

            if (fileBytes.Length < 0x400)
                return false;

            Inputs.Clear();

            _headerBytes = fileBytes.Take(0x400).ToArray();
            var frameBytes = fileBytes.Skip(0x400).ToArray();

            var numOfInputs = BitConverter.ToUInt32(_headerBytes, 0x18);

            for (int i = 0; i < frameBytes.Length && i < 4 * numOfInputs; i += 4)
            {
                Inputs.Add(new InputFrame(BitConverter.ToUInt32(frameBytes, i), i / 4));
            }

            if (Inputs.Count == 0)
                InsertNew(0);

            return true;
        } 

        private byte[] GetFileBytes()
        {
            return _headerBytes.Concat(Inputs.SelectMany(i => BitConverter.GetBytes(i.RawValue))).ToArray();
        }

        public bool Save()
        {
            return Save(_currentFile);
        }

        public bool Save(string filePath)
        {
            try
            {
                File.WriteAllBytes(filePath, GetFileBytes());
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        public void InsertNew(int index)
        {
            for (int i = index; i < Inputs.Count; i++)
                Inputs[i].Index++;

            var frame = new InputFrame(index);
            Inputs.Insert(index, frame);  
        }

        public void CopyRows(List<int> rows)
        {
            if (rows.Count == 0)
                return;

            int smallestIndex = rows.Min();

            var inputList = rows.Select(i => (InputFrame)Inputs[i].Clone()).ToList();
            foreach (var input in inputList)
                input.Index -= smallestIndex;

            inputList = inputList.OrderBy(i => i.Index).ToList();

            Clipboard.SetData("FrameInputData", inputList);
        }

        public void PasteOnto(List<int> rows)
        {
            if (rows.Count == 0)
                return;

            if (!Clipboard.ContainsData("FrameInputData")
                || !(Clipboard.GetData("FrameInputData") is List<InputFrame>))
                return;

            int smallestIndex = rows.Min();

            var inputData = Clipboard.GetData("FrameInputData") as List<InputFrame>;

            foreach (var input in inputData)
            {
                if (!rows.Any(i => smallestIndex + input.Index == i))
                    continue;

                int index = rows.Find(i => smallestIndex + input.Index == i);
                input.Index = index;
                Inputs[index] = input;
            }
        }

        public void PasteInsert(int row)
        {
            if (!Clipboard.ContainsData("FrameInputData")
                || !(Clipboard.GetData("FrameInputData") is List<InputFrame>))
                return;

            var inputData = Clipboard.GetData("FrameInputData") as List<InputFrame>;
            inputData = inputData.OrderByDescending(i => i.Index).ToList();

            for (int i = row; i < Inputs.Count; i++)
                Inputs[i].Index += inputData.Count;

            int index = row + inputData.Count - 1;

            foreach (var input in inputData)
            {
                input.Index = index--;
                Inputs.Insert(row, input);
            }
        }
    }
}
