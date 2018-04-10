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
        private readonly string _type;
        private readonly string _fileName;

        private M64CopiedData(int startFrame, int endFrame, string type, string fileName)
        {
            _startFrame = startFrame;
            _endFrame = endFrame;
            _totalFrames = endFrame - startFrame + 1;
            _type = type;
            _fileName = fileName;
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
            return new M64CopiedData(startFrame, endFrame, type, fileName);
        }

        /*
        public static M64CopiedData CreateCopiedDataFromCells(DataGridView table, string fileName)
        {
            if (table.SelectedCells.Count == 0 || fileName == null) return null;
            List<M64InputCell> cells = M64Utilities.GetSelectedInputCells(table);
            (int minFrame, int maxFrame, string inputsString) = M64Utilities.GetCellStats(cells);
            return new M64CopiedData(minFrame, maxFrame, inputsString, fileName);
        }

        public static M64CopiedData CreateCopiedDataFromRows(DataGridView table, string fileName)
        {
            if (table.SelectedRows.Count == 0 || fileName == null) return null;
            List<M64InputFrame> inputs = M64Utilities.GetSelectedInputFrames(table);
            int minFrame = inputs.Min(input => input.Frame);
            int maxFrame = inputs.Max(input => input.Frame);
            return new M64CopiedData(minFrame, maxFrame, "Row", fileName);
        }
        */

        public override string ToString()
        {
            return String.Format(
                "{0}f [{1}] {2}-{3} @{4}",
                _totalFrames, _type, _startFrame, _endFrame, _fileName);
        }
    }
}
