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
            Clear(InputFrame, HeaderText);
        }

        private void Clear(M64InputFrame inputFrame, string headerText)
        {
            switch (headerText)
            {
                case "X":
                    inputFrame.X = 0;
                    break;
                case "Y":
                    inputFrame.Y = 0;
                    break;
                case "A":
                    inputFrame.A = false;
                    break;
                case "B":
                    inputFrame.B = false;
                    break;
                case "Z":
                    inputFrame.Z = false;
                    break;
                case "S":
                    inputFrame.Start = false;
                    break;
                case "R":
                    inputFrame.R = false;
                    break;
                case "C^":
                    inputFrame.C_Up = false;
                    break;
                case "Cv":
                    inputFrame.C_Down = false;
                    break;
                case "C<":
                    inputFrame.C_Left = false;
                    break;
                case "C>":
                    inputFrame.C_Right = false;
                    break;
                case "L":
                    inputFrame.L = false;
                    break;
                case "D^":
                    inputFrame.D_Up = false;
                    break;
                case "Dv":
                    inputFrame.D_Down = false;
                    break;
                case "D<":
                    inputFrame.D_Left = false;
                    break;
                case "D>":
                    inputFrame.D_Right = false;
                    break;
            }
        }
    }
}
