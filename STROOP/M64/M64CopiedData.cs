using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.M64
{
    public class M64CopiedData
    {
        public int TotalFrames { get => _totalFrames; }

        private readonly int _startFrame;
        private readonly int _endFrame;
        private readonly int _totalFrames;
        private readonly string _typeString;
        private readonly string _fileName;
        private readonly string _customName;
        private readonly List<M64CopiedFrame> _copiedFrames;

        private M64CopiedData(
            int startFrame,
            int endFrame,
            string typeString,
            string fileName,
            string customName,
            List<M64CopiedFrame> copiedFrames)
        {
            _startFrame = startFrame;
            _endFrame = endFrame;
            _totalFrames = endFrame - startFrame + 1;
            _typeString = typeString;
            _fileName = fileName;
            _customName = customName;
            _copiedFrames = copiedFrames;
            if (_totalFrames != copiedFrames.Count) throw new ArgumentOutOfRangeException();
        }

        public static M64CopiedData CreateCopiedData(
            DataGridView table, string fileName, int startFrame, int endFrame, bool useRows, string inputsString = null)
        {
            startFrame = Math.Max(startFrame, 0);
            endFrame = Math.Min(endFrame, table.Rows.Count - 1);
            if (startFrame > endFrame) return null;
            if (fileName == null) return null;
            if (!useRows && inputsString == null) return null;

            string type = useRows ? "Row" : inputsString;

            List<M64InputFrame> inputs = M64Utilities.GetInputFramesInRange(table, startFrame, endFrame);
            List<M64CopiedFrame> copiedFrames = inputs.ConvertAll(
                input => M64CopiedFrame.CreateCopiedFrame(input, useRows, inputsString));

            return new M64CopiedData(startFrame, endFrame, type, fileName, null /* customName */, copiedFrames);
        }

        public static readonly M64CopiedData OneEmptyFrame =
            new M64CopiedData(0, 0, null, null, "One Empty Frame",
                new List<M64CopiedFrame>() { M64CopiedFrame.OneEmptyFrame });

        public static readonly M64CopiedData PauseBufferTriplet =
            new M64CopiedData(0, 2, null, null, "Pause Buffer Triplet",
                new List<M64CopiedFrame>() {
                    M64CopiedFrame.OneEmptyFrame,
                    M64CopiedFrame.OnePauseFrame,
                    M64CopiedFrame.OneEmptyFrame });

        public static readonly M64CopiedData OnePauseFrameOverwrite =
            new M64CopiedData(0, 0, null, null, "One Empty Frame",
                new List<M64CopiedFrame>() { M64CopiedFrame.OnePauseFrameOverwrite });

        public void Apply(List<M64InputFrame> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                int copiedFrameIndex = i % _copiedFrames.Count;
                _copiedFrames[copiedFrameIndex].Apply(inputs[i]);
            }
        }

        public override string ToString()
        {
            if (_customName != null) return _customName;
            return String.Format(
                "{0}f [{1}] {2}-{3} @{4}",
                _totalFrames, _typeString, _startFrame, _endFrame, _fileName);
        }

        internal uint GetRawValue(int index)
        {
            index %= _copiedFrames.Count;
            return _copiedFrames[index].RawValue;
        }
    }
}
