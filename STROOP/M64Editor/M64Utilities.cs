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
        public static readonly Color NewRowColor = Color.FromArgb(186, 255, 166);
        public static readonly Color EditedCellColor = Color.Pink;

        public static readonly Dictionary<string, int> InputHeaderTextToIndex =
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

        public static readonly List<string> InputHeaderTexts =
            InputHeaderTextToIndex.Keys.ToList();

        public static readonly List<string> ButtonNameList = InputHeaderTexts.Skip(2).ToList();

        public static readonly List<Func<M64InputFrame, bool>> IsButtonPressedFunctionList =
            new List<Func<M64InputFrame, bool>>()
            {
                input => input.A,
                input => input.B,
                input => input.Z,
                input => input.S,
                input => input.R,
                input => input.C_Up,
                input => input.C_Down,
                input => input.C_Left,
                input => input.C_Right,
                input => input.L,
                input => input.D_Up,
                input => input.D_Down,
                input => input.D_Left,
                input => input.D_Right,
            };

        public static readonly Comparison<string> InputStringComparison =
            new Comparison<string>((inputString1, inputString2) =>
                InputHeaderTextToIndex[inputString1] - InputHeaderTextToIndex[inputString2]);

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

        public static List<M64InputCell> GetSelectedInputCells(
            DataGridView table, CellSelectionType cellSelectionType,
            int? startFrameNullable = null, int? endFrameNullable = null, string inputsString = null)
        {
            if (cellSelectionType == CellSelectionType.PartialRowRange && inputsString == null)
                throw new ArgumentOutOfRangeException();

            if (cellSelectionType == CellSelectionType.Cells)
            {
                List<M64InputCell> cells = new List<M64InputCell>();
                foreach (DataGridViewCell cell in table.SelectedCells)
                {
                    cells.Add(new M64InputCell(cell));
                }
                return cells;
            }
            else
            {
                if (!startFrameNullable.HasValue || !endFrameNullable.HasValue) return new List<M64InputCell>();
                int startFrame = Math.Max(startFrameNullable.Value, 0);
                int endFrame = Math.Min(endFrameNullable.Value, table.Rows.Count - 1);

                List<M64InputCell> cells = new List<M64InputCell>();
                for (int rowIndex = startFrame; rowIndex <= endFrame; rowIndex++)
                {
                    DataGridViewRow row = table.Rows[rowIndex];
                    for (int colIndex = 0; colIndex < table.Columns.Count; colIndex++)
                    {
                        string headerText = table.Columns[colIndex].HeaderText;
                        if (cellSelectionType == CellSelectionType.PartialRowRange &&
                            !inputsString.Contains(headerText)) continue;
                        DataGridViewCell tableCell = row.Cells[colIndex];
                        cells.Add(new M64InputCell(tableCell));
                    }
                }
                return cells;
            }
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

        public static uint GetRawValueFromInputs(
            sbyte X,
            sbyte Y,
            bool A,
            bool B,
            bool Z,
            bool S,
            bool R,
            bool C_Up,
            bool C_Down,
            bool C_Left,
            bool C_Right,
            bool L,
            bool D_Up,
            bool D_Down,
            bool D_Left,
            bool D_Right)
        {
            uint rawValue = 0;

            rawValue = SetByte(rawValue, 2, (byte)X);
            rawValue = SetByte(rawValue, 3, (byte)Y);

            rawValue = SetBit(rawValue, 7, A);
            rawValue = SetBit(rawValue, 6, B);
            rawValue = SetBit(rawValue, 5, Z);
            rawValue = SetBit(rawValue, 4, S);
            rawValue = SetBit(rawValue, 12, R);

            rawValue = SetBit(rawValue, 11, C_Up);
            rawValue = SetBit(rawValue, 10, C_Down);
            rawValue = SetBit(rawValue, 9, C_Left);
            rawValue = SetBit(rawValue, 8, C_Right);

            rawValue = SetBit(rawValue, 13, L);

            rawValue = SetBit(rawValue, 3, D_Up);
            rawValue = SetBit(rawValue, 2, D_Down);
            rawValue = SetBit(rawValue, 1, D_Left);
            rawValue = SetBit(rawValue, 0, D_Right);

            return rawValue;
        }

        public static uint SetByte(uint rawValue, int num, byte value)
        {
            uint mask = ~(uint)(0xFF << (num * 8));
            return ((uint)(value << (num * 8)) | (rawValue & mask));
        }

        public static byte GetByte(uint rawValue, int num)
        {
            return (byte)(rawValue >> (num * 8));
        }

        public static uint SetBit(uint rawValue, int bit, bool value)
        {
            uint mask = (uint)(1 << bit);
            if (value)
            {
                return rawValue | mask;
            }
            else
            {
                return rawValue & ~mask;
            }
        }

        public static bool GetBit(uint rawValue, int bit)
        {
            return ((rawValue >> bit) & 0x01) == 0x01;
        }

        public static int ConvertFrameToDisplayedValue(int frame)
        {
            return frame + GetFrameInputRelationOffset();
        }

        public static int ConvertDisplayedValueToFrame(int displayedValue)
        {
            return displayedValue - GetFrameInputRelationOffset();
        }

        private static int GetFrameInputRelationOffset()
        {
            switch (M64Config.FrameInputRelation)
            {
                case FrameInputRelationType.FrameOfInput:
                    return -1;
                case FrameInputRelationType.FrameAfterInput:
                    return 0;
                case FrameInputRelationType.FrameWhenObserved:
                    return 1;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
