using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using STROOP.Structs;
using System.ComponentModel;
using STROOP.Utilities;
using System.Windows.Forms;
using STROOP.Forms;
using System.Drawing;

namespace STROOP.M64Editor
{
    public static class M64Utilities
    {

        public static readonly Dictionary<string, int> InputStringToIndex =
            new Dictionary<string, int>()
            {
                ["X"] = 0,
                ["Y"] = 1,
                ["A"] = 2,
                ["B"] = 3,
                ["Z"] = 4,
                ["S"] = 5,
                ["R"] = 6,
                ["C^"] = 7,
                ["Cv"] = 8,
                ["C<"] = 9,
                ["C>"] = 10,
                ["L"] = 11,
                ["D^"] = 12,
                ["Dv"] = 13,
                ["D<"] = 14,
                ["D>"] = 15,
            };

        public static readonly Comparison<string> InputStringComparison =
            new Comparison<string>((inputString1, inputString2) =>
                InputStringToIndex[inputString1] - InputStringToIndex[inputString2]);

        public static void SetSpecificInputValue(
            M64InputFrame inputFrame, string headerText, bool value)
        {
            switch (headerText)
            {
                case "X":
                    if (!value) inputFrame.X = 0;
                    break;
                case "Y":
                    if (!value) inputFrame.Y = 0;
                    break;
                case "A":
                    inputFrame.A = value;
                    break;
                case "B":
                    inputFrame.B = value;
                    break;
                case "Z":
                    inputFrame.Z = value;
                    break;
                case "S":
                    inputFrame.S = value;
                    break;
                case "R":
                    inputFrame.R = value;
                    break;
                case "C^":
                    inputFrame.C_Up = value;
                    break;
                case "Cv":
                    inputFrame.C_Down = value;
                    break;
                case "C<":
                    inputFrame.C_Left = value;
                    break;
                case "C>":
                    inputFrame.C_Right = value;
                    break;
                case "L":
                    inputFrame.L = value;
                    break;
                case "D^":
                    inputFrame.D_Up = value;
                    break;
                case "Dv":
                    inputFrame.D_Down = value;
                    break;
                case "D<":
                    inputFrame.D_Left = value;
                    break;
                case "D>":
                    inputFrame.D_Right = value;
                    break;
            }
        }

        public static readonly List<(string, int, Color?)> ColumnParameters =
            new List<(string, int, Color?)>()
            {
                ("Frame", 200, null),
                ("Id", 200, null),
                ("X", 200, null),
                ("Y", 200, null),
                ("A", 100, null),
                ("B", 100, null),
                ("Z", 100, null),
                ("S", 100, null),
                ("R", 100, null),
                ("C^", 100, Color.Yellow),
                ("Cv", 100, Color.Yellow),
                ("C<", 100, Color.Yellow),
                ("C>", 100, Color.Yellow),
                ("L", 100, Color.LightGray),
                ("D^", 100, Color.LightGray),
                ("Dv", 100, Color.LightGray),
                ("D<", 100, Color.LightGray),
                ("D>", 100, Color.LightGray),
            };

        public static List<M64InputCell> GetSelectedInputCells(DataGridView table, CellSelectionType cellSelectionType)
        {
            List<M64InputCell> cells = new List<M64InputCell>();
            foreach (DataGridViewCell cell in table.SelectedCells)
            {
                cells.Add(new M64InputCell(cell));
            }
            return cells;
        }

        public static (int minFrame, int maxFrame, string inputsString) GetCellStats(List<M64InputCell> cells)
        {
            if (cells.Count == 0) return (0, 0, "");
            int minFrame = cells.Min(cell => cell.RowIndex);
            int maxFrame = cells.Max(cell => cell.RowIndex);
            List<string> headerTexts = cells
                .FindAll(cell => cell.IsInput)
                .ConvertAll(cell => cell.HeaderText).Distinct().ToList();
            headerTexts.Sort(InputStringComparison);
            string inputsString = String.Join("", headerTexts);
            return (minFrame, maxFrame, inputsString);
        }

        public static List<M64InputFrame> GetSelectedInputFrames(DataGridView table)
        {
            BindingList<M64InputFrame> allInputs = table.DataSource as BindingList<M64InputFrame>;
            List<M64InputFrame> inputs = new List<M64InputFrame>();
            foreach (DataGridViewRow row in table.SelectedRows)
            {
                inputs.Add(allInputs[row.Index]);
            }
            return inputs;
        }

        public static List<M64InputFrame> GetInputFramesInRange(DataGridView table, int startRow, int endRow)
        {
            BindingList<M64InputFrame> allInputs = table.DataSource as BindingList<M64InputFrame>;
            return allInputs.Skip(startRow).Take(endRow - startRow + 1).ToList();
        }
    }
}
