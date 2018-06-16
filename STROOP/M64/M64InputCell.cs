using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace STROOP.M64
{
    public class M64InputCell
    {
        public readonly string HeaderText;
        public readonly bool IsInput;
        private readonly M64InputFrame InputFrame;
        public int RowIndex { get => InputFrame.FrameIndex; }

        public M64InputCell(DataGridViewCell cell)
        {
            DataGridView table = cell.DataGridView;
            HeaderText = table.Columns[cell.ColumnIndex].HeaderText;
            IsInput = M64Utilities.InputHeaderTexts.Contains(HeaderText);
            BindingList<M64InputFrame> inputs = table.DataSource as BindingList<M64InputFrame>;
            InputFrame = inputs[cell.RowIndex];
        }

        public void SetValue(bool value, int? intOnValue = null)
        {
            M64Utilities.SetSpecificInputValue(InputFrame, HeaderText, value, intOnValue);
        }
    }
}
