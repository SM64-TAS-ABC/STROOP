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

namespace STROOP.M64Editor
{
    public class M64File
    {
        public string CurrentFilePath { get; private set; }
        public string CurrentFileName { get; private set; }
        public byte[] RawBytes { get; private set; }

        public M64Header Header { get; }
        public BindingList<M64InputFrame> Inputs { get; }
        public M64Stats Stats { get; }

        public M64File()
        {
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

            RawBytes = fileBytes;
            Inputs.Clear();
            byte[] headerBytes = fileBytes.Take(M64Config.HeaderSize).ToArray();
            Header.LoadBytes(headerBytes);
            var frameBytes = fileBytes.Skip(M64Config.HeaderSize).ToArray();

            int numOfInputs = Header.NumInputs;

            for (int i = 0; i < frameBytes.Length && i < 4 * numOfInputs; i += 4)
            {
                Inputs.Add(new M64InputFrame(BitConverter.ToUInt32(frameBytes, i), i / 4));
            }

            if (Inputs.Count == 0)
                InsertNew(0);

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
            Header.Clear();
            Inputs.Clear();
        }

        public void InsertNew(int index)
        {
            for (int i = index; i < Inputs.Count; i++)
                Inputs[i]._frame++;

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
                input._frame -= smallestIndex;

            inputList = inputList.OrderBy(i => i._frame).ToList();

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
                if (!rows.Any(i => smallestIndex + input._frame == i))
                    continue;

                int index = rows.Find(i => smallestIndex + input._frame == i);
                input._frame = index;
                Inputs[index] = input;
            }
        }

        public void PasteInsert(int row)
        {
            if (!Clipboard.ContainsData("FrameInputData")
                || !(Clipboard.GetData("FrameInputData") is List<M64InputFrame>))
                return;

            var inputData = Clipboard.GetData("FrameInputData") as List<M64InputFrame>;
            inputData = inputData.OrderByDescending(i => i._frame).ToList();

            for (int i = row; i < Inputs.Count; i++)
                Inputs[i]._frame += inputData.Count;

            int index = row + inputData.Count - 1;

            foreach (var input in inputData)
            {
                input._frame = index--;
                Inputs.Insert(row, input);
            }
        }
    }
}
