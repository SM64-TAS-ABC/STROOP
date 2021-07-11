using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.M64
{
    /// <summary>
    /// Data copied from a .m64 recording.
    /// </summary>
    public class M64CopiedData
    {
        /// <summary>
        /// The total frames in the copied data
        /// </summary>
        /// <value>The total frames in the data.</value>
        public int TotalFrames { get => _totalFrames; }

        // variables representing the copied data
        private readonly int _startFrame;
        private readonly int _endFrame;
        private readonly int _totalFrames;
        private readonly string _typeString;
        private readonly string _fileName;
        private readonly string _customName;
        private readonly List<M64CopiedFrame> _copiedFrames;

        // instantiate a new instance of copied data
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

            // the total frames and the number of copied frames should match up
            if (_totalFrames != copiedFrames.Count) throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Create copied data from the data grid.
        /// </summary>
        /// <returns>An object representing the copied data.</returns>
        /// <param name="table">The table to pull the data from.</param>
        /// <param name="fileName">The file name of the recording.</param>
        /// <param name="startFrame">The frame to start copying from.</param>
        /// <param name="endFrame">The frame to end copying on.</param>
        /// <param name="useRows">If set to <c>true</c>, copy the rows. Otherwise, use the <paramref name="inputsString"/> variable.</param>
        /// <param name="inputsString">Inputs to copy into the data.</param>
        public static M64CopiedData CreateCopiedData(
            DataGridView table, string fileName, int startFrame, int endFrame, bool useRows, string inputsString = null)
        {
            // clamp the start and end to a range
            startFrame = Math.Max(startFrame, 0);
            endFrame = Math.Min(endFrame, table.Rows.Count - 1);

            // ensure the values given are valid
            if (startFrame > endFrame) return null;
            if (fileName == null) return null;
            if (!useRows && inputsString == null) return null;

            string type = useRows ? "Row" : inputsString;

            // get all the frames needed
            List<M64InputFrame> inputs = M64Utilities.GetInputFramesInRange(table, startFrame, endFrame);
            List<M64CopiedFrame> copiedFrames = inputs.ConvertAll(
                input => M64CopiedFrame.CreateCopiedFrame(input, useRows, inputsString));

            return new M64CopiedData(startFrame, endFrame, type, fileName, null /* customName */, copiedFrames);
        }

        /// <summary>
        /// Copied data consisting of a single empty frame.
        /// </summary>
        public static readonly M64CopiedData OneEmptyFrame =
            new M64CopiedData(0, 0, null, null, "One Empty Frame",
                new List<M64CopiedFrame>() { M64CopiedFrame.OneEmptyFrame });

        /// <summary>
        /// Copied data consisting of a single pause frame.
        /// </summary>
        public static readonly M64CopiedData OnePauseFrame =
            new M64CopiedData(0, 0, null, null, "One Pause Frame",
                new List<M64CopiedFrame>() { M64CopiedFrame.OnePauseFrame });

        /// <summary>
        /// Copied data consisting of a pause overwrite frame.
        /// </summary>
        public static readonly M64CopiedData OnePauseFrameOverwrite =
            new M64CopiedData(0, 0, null, null, "One Empty Frame",
                new List<M64CopiedFrame>() { M64CopiedFrame.OnePauseFrameOverwrite });

        /// <summary>
        /// Add a list of copied frames to this data.
        /// </summary>
        /// <param name="inputs">Inputs to add.</param>
        public void Apply(List<M64InputFrame> inputs)
        {
            for (int i = 0; i < inputs.Count; i++)
            {
                int copiedFrameIndex = i % _copiedFrames.Count;
                _copiedFrames[copiedFrameIndex].Apply(inputs[i]);
            }
        }

        /// <summary>
        /// Add an input to this data.
        /// </summary>
        /// <param name="input">Input to add.</param>
        public void Apply(M64InputFrame input)
        {
            Apply(new List<M64InputFrame>() { input });
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:STROOP.M64.M64CopiedData"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:STROOP.M64.M64CopiedData"/>.</returns>
        public override string ToString()
        {
            if (_customName != null) return _customName;
            return String.Format(
                "{0}f [{1}] {2}-{3} @{4}",
                _totalFrames, _typeString, _startFrame, _endFrame, _fileName);
        }
     
        /// <summary>
        /// Get the raw value of a frame.
        /// </summary>
        /// <returns>The raw value of the specified frame.</returns>
        /// <param name="index">Index of the frame to get the raw value of.</param>
        internal uint GetRawValue(int index)
        {
            index %= _copiedFrames.Count;
            return _copiedFrames[index].RawValue;
        }
    }
}
