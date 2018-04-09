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

namespace STROOP.M64Editor
{
    public class M64InputCell
    {
        public readonly string HeaderText;
        public string InputHeaderText
        {
            get
            {
                if (HeaderText == "Frame") return "";
                return HeaderText;
            }
        }
        private readonly M64InputFrame InputFrame;
        public int RowIndex { get => InputFrame.Index; }

        public M64InputCell(DataGridView table, DataGridViewCell cell)
        {
            HeaderText = table.Columns[cell.ColumnIndex].HeaderText;
            BindingList<M64InputFrame> inputs = table.DataSource as BindingList<M64InputFrame>;
            InputFrame = inputs[cell.RowIndex];
        }

        public void Clear()
        {
            M64Utilities.ClearSpecificInput(InputFrame, HeaderText);
        }
    }
}
