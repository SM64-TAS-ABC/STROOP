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
        public string CurrentFile { get; private set; }
        public M64Header Header { get; } = new M64Header();
        public BindingList<M64InputFrame> Inputs { get; } = new BindingList<M64InputFrame>();

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
                CurrentFile = filePath;

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

            byte[] headerBytes = fileBytes.Take(0x400).ToArray();
            Header.LoadBytes(headerBytes);
            var frameBytes = fileBytes.Skip(0x400).ToArray();

            int numOfInputs = Header.Inputs;

            for (int i = 0; i < frameBytes.Length && i < 4 * numOfInputs; i += 4)
            {
                Inputs.Add(new M64InputFrame(BitConverter.ToUInt32(frameBytes, i), i / 4));
            }

            if (Inputs.Count == 0)
                InsertNew(0);

            return true;
        } 

        private byte[] GetFileBytes()
        {
            return Header.ToBytes().Concat(Inputs.SelectMany(i => BitConverter.GetBytes(i.RawValue))).ToArray();
        }

        public bool Save()
        {
            return Save(CurrentFile);
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

            var frame = new M64InputFrame(index);
            Inputs.Insert(index, frame);  
        }

        public void CopyRows(List<int> rows)
        {
            if (rows.Count == 0)
                return;

            int smallestIndex = rows.Min();

            var inputList = rows.Select(i => (M64InputFrame)Inputs[i].Clone()).ToList();
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
                || !(Clipboard.GetData("FrameInputData") is List<M64InputFrame>))
                return;

            int smallestIndex = rows.Min();

            var inputData = Clipboard.GetData("FrameInputData") as List<M64InputFrame>;

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
                || !(Clipboard.GetData("FrameInputData") is List<M64InputFrame>))
                return;

            var inputData = Clipboard.GetData("FrameInputData") as List<M64InputFrame>;
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
