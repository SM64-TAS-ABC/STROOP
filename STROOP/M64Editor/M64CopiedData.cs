using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.M64Editor
{
    public class M64CopiedData
    {
        private readonly int _startFrame;
        private readonly int _endFrame;
        private readonly int _totalFrames;
        private readonly string _typeString;
        private readonly string _fileName;
        private readonly List<M64CopiedFrame> _copiedFrames;

        private M64CopiedData(
            int startFrame,
            int endFrame,
            string typeString,
            string fileName,
            List<M64CopiedFrame> copiedFrames)
        {
            _startFrame = startFrame;
            _endFrame = endFrame;
            _totalFrames = endFrame - startFrame + 1;
            _typeString = typeString;
            _fileName = fileName;
            _copiedFrames = copiedFrames;
            if (_totalFrames != copiedFrames.Count) throw new ArgumentOutOfRangeException();
        }

        public static M64CopiedData CreateCopiedData(
            DataGridView table, string fileName, int startFrame, int endFrame, bool useRows, string inputsString = null)
        {
            if (startFrame < 0) return null;
            if (endFrame >= table.Rows.Count) return null;
            if (startFrame > endFrame) return null;
            if (fileName == null) return null;
            if (!useRows && inputsString == null) return null;

            string type = useRows ? "Row" : inputsString;

            List<M64InputFrame> inputs = M64Utilities.GetInputFramesInRange(table, startFrame, endFrame);
            List<M64CopiedFrame> copiedFrames = inputs.ConvertAll(
                input => M64CopiedFrame.CreateCopiedFrame(input, inputsString));

            return new M64CopiedData(startFrame, endFrame, type, fileName, copiedFrames);
        }

        public void Apply(List<M64InputFrame> inputs)
        {
            if (inputs.Count != _copiedFrames.Count) throw new ArgumentOutOfRangeException();
            for (int i = 0; i < inputs.Count; i++)
            {
                _copiedFrames[i].Apply(inputs[i]);
            }
        }

        public override string ToString()
        {
            return String.Format(
                "{0}f [{1}] {2}-{3} @{4}",
                _totalFrames, _typeString, _startFrame, _endFrame, _fileName);
        }
    }
}
