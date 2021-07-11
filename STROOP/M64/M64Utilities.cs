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

namespace STROOP.M64
{
    /// <summary>
    /// Utility functions related to the reading, processing, or writing of .m64 recordings.
    /// </summary>
    public static class M64Utilities
    {
        /// <summary>
        /// A dictionary mapping buttons to their corresponding indices.
        /// </summary>
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

        /// <summary>
        /// A list of strings corresponding to inputs, that correspond to M64Utilities.InputHeaderTextToIndex.
        /// </summary>
        /// <seealso cref="M64Utilities.InputHeaderTextToIndex"/>
        public static readonly List<string> InputHeaderTexts =
            InputHeaderTextToIndex.Keys.ToList();

        /// <summary>
        /// A list of strings that correspond to names of buttons.
        /// </summary>
        /// <seealso cref="M64Utilities.InputHeaderTexts"/>
        public static readonly List<string> ButtonNameList = InputHeaderTexts.Skip(2).ToList();

        /// <summary>
        /// A list of functions that, given an input frame, will result in a <c>bool</c> value.
        /// </summary>
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

        /// <summary>
        /// A comparison to tell where two strings representing inputs would be inrelation to eachother.
        /// </summary>
        /// <remarks>Probably used in a sorting algorithm.</remarks>
        public static readonly Comparison<string> InputStringComparison =
            new Comparison<string>((inputString1, inputString2) =>
                InputHeaderTextToIndex[inputString1] - InputHeaderTextToIndex[inputString2]);

        /// <summary>
        /// Sets an input value on a particular frame
        /// </summary>
        /// <param name="inputFrame">The frame to set the value on.</param>
        /// <param name="headerText">The text representing the input.</param>
        /// <param name="value">Whether or not the input is activated.</param>
        /// <param name="intOnValue">If the value is an integer (like the joysticks), use this as the value. Can be <c>null</c>.</param>
        public static void SetSpecificInputValue(
            M64InputFrame inputFrame, string headerText, bool value, int? intOnValue = null)
        {
            switch (headerText)
            {
                case "X":
                    if (value)
                    {
                        if (intOnValue.HasValue)
                        {
                            sbyte sbyteValue = ParsingUtilities.ParseSByteRoundingCapping(intOnValue.Value);
                            inputFrame.X = sbyteValue;
                        }
                    }
                    else
                    {
                        inputFrame.X = 0;
                    }
                    break;
                case "Y":
                    if (value)
                    {
                        if (intOnValue.HasValue)
                        {
                            sbyte sbyteValue = ParsingUtilities.ParseSByteRoundingCapping(intOnValue.Value);
                            inputFrame.Y = sbyteValue;
                        }
                    }
                    else
                    {
                        inputFrame.Y = 0;
                    }
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

        /// <summary>
        /// A list describing the columns in the displayed table.
        /// </summary>
        public static readonly List<(string, int, Color)> ColumnParameters =
            new List<(string, int, Color)>()
            {
                ("Frame", M64Config.TextColumnFillWeight, M64Config.FrameColumnColor),
                ("Id", M64Config.TextColumnFillWeight, M64Config.FrameColumnColor),
                ("X", M64Config.TextColumnFillWeight, M64Config.MainButtonColor),
                ("Y", M64Config.TextColumnFillWeight, M64Config.MainButtonColor),
                ("A", M64Config.CheckBoxColumnFillWeight, M64Config.MainButtonColor),
                ("B", M64Config.CheckBoxColumnFillWeight, M64Config.MainButtonColor),
                ("Z", M64Config.CheckBoxColumnFillWeight, M64Config.MainButtonColor),
                ("S", M64Config.CheckBoxColumnFillWeight, M64Config.MainButtonColor),
                ("R", M64Config.CheckBoxColumnFillWeight, M64Config.MainButtonColor),
                ("C^", M64Config.CheckBoxColumnFillWeight, M64Config.CButtonColumnColor),
                ("Cv", M64Config.CheckBoxColumnFillWeight, M64Config.CButtonColumnColor),
                ("C<", M64Config.CheckBoxColumnFillWeight, M64Config.CButtonColumnColor),
                ("C>", M64Config.CheckBoxColumnFillWeight, M64Config.CButtonColumnColor),
                ("L", M64Config.CheckBoxColumnFillWeight, M64Config.NoopButtonColumnColor),
                ("D^", M64Config.CheckBoxColumnFillWeight, M64Config.NoopButtonColumnColor),
                ("Dv", M64Config.CheckBoxColumnFillWeight, M64Config.NoopButtonColumnColor),
                ("D<", M64Config.CheckBoxColumnFillWeight, M64Config.NoopButtonColumnColor),
                ("D>", M64Config.CheckBoxColumnFillWeight, M64Config.NoopButtonColumnColor),
            };

        /// <summary>
        /// Get the cells of the input table from
        /// </summary>
        /// <returns>The selected input cells.</returns>
        /// <param name="table">A list of input frames corresponding to the cells.</param>
        /// <param name="cellSelectionType">The type of cell to select.</param>
        /// <param name="startFrameNullable">The frame to start with.</param>
        /// <param name="endFrameNullable">The frame to end on.</param>
        /// <param name="inputsString">The types of inputs to add to the list.</param>
        public static List<M64InputCell> GetSelectedInputCells(
            DataGridView table, CellSelectionType cellSelectionType,
            int? startFrameNullable = null, int? endFrameNullable = null, string inputsString = null)
        {
            // if the partial row range option is selected but without input strings, throw an exception
            if (cellSelectionType == CellSelectionType.PartialRowRange && inputsString == null)
                throw new ArgumentOutOfRangeException();

            // if the cells are selected, pull the inputs from those
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
                // if both the starting frame and ending frame are
                if (!startFrameNullable.HasValue || !endFrameNullable.HasValue) return new List<M64InputCell>();

                // put the start and end frame in a range
                int startFrame = Math.Max(startFrameNullable.Value, 0);
                int endFrame = Math.Min(endFrameNullable.Value, table.Rows.Count - 1);
                
                // get each row and put all cells in that row in a list
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

        /// <summary>
        /// Get the statistics for a list of cells.
        /// </summary>
        /// <returns>These statistics.</returns>
        /// <param name="cells">The cells to get statistics from</param>
        /// <param name="useDisplayed">If set to <c>true</c> use the displayed value for min and max frames.</param>
        public static (int? minFrame, int? maxFrame, string inputsString) GetCellStats(
            List<M64InputCell> cells, bool useDisplayed)
        {
            if (cells.Count == 0) return (null, null, "");
            int minFrame = cells.Min(cell => cell.RowIndex);
            int maxFrame = cells.Max(cell => cell.RowIndex);
            if (useDisplayed)
            {
                minFrame = ConvertFrameToDisplayedValue(minFrame);
                maxFrame = ConvertFrameToDisplayedValue(maxFrame);
            }
            List<string> headerTexts = cells
                .FindAll(cell => cell.IsInput)
                .ConvertAll(cell => cell.HeaderText).Distinct().ToList();
            headerTexts.Sort(InputStringComparison);
            string inputsString = String.Join("", headerTexts);
            return (minFrame, maxFrame, inputsString);
        }

        /// <summary>
        /// Get all of the selected input frames from a table.
        /// </summary>
        /// <returns>The selected input frames.</returns>
        /// <param name="table">The table to get the selected cells from.</param>
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

        /// <summary>
        /// Get the input frames in between two rows.
        /// </summary>
        /// <returns>The input frames in the range.</returns>
        /// <param name="table">The table to get the cells from.</param>
        /// <param name="startRow">The row to start selection at.</param>
        /// <param name="endRow">The row to end selection at.</param>
        public static List<M64InputFrame> GetInputFramesInRange(DataGridView table, int startRow, int endRow)
        {
            BindingList<M64InputFrame> allInputs = table.DataSource as BindingList<M64InputFrame>;
            return allInputs.Skip(startRow).Take(endRow - startRow + 1).ToList();
        }

        /// <summary>
        /// Compose a series of inputs into a raw integer value.
        /// </summary>
        /// <returns>The raw value.</returns>
        /// <param name="X">X</param>
        /// <param name="Y">Y</param>
        /// <param name="A">A</param>
        /// <param name="B">B</param>
        /// <param name="Z">Z</param>
        /// <param name="S">S</param>
        /// <param name="R">R</param>
        /// <param name="C_Up">C^</param>
        /// <param name="C_Down">Cv</param>
        /// <param name="C_Left">C&lt;</param>
        /// <param name="C_Right">C&gt;</param>
        /// <param name="L">L</param>
        /// <param name="D_Up">D^.</param>
        /// <param name="D_Down">Dv</param>
        /// <param name="D_Left">D&lt;</param>
        /// <param name="D_Right">D&gt;</param>
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

        /// <summary>
        /// Set a particular byte of a value
        /// </summary>
        /// <returns>The modified value of the <c>rawValue</c> parameter.</returns>
        /// <param name="rawValue">The raw value</param>
        /// <param name="num">The index of the byte in the raw value</param>
        /// <param name="value">The byte to set to</param>
        public static uint SetByte(uint rawValue, int num, byte value)
        {
            uint mask = ~(uint)(0xFF << (num * 8));
            return ((uint)(value << (num * 8)) | (rawValue & mask));
        }

        /// <summary>
        /// Gets a particular byte of an int value.
        /// </summary>
        /// <returns>The byte from the value.</returns>
        /// <param name="rawValue">Value to get the byte from.</param>
        /// <param name="num">The index of the byte to get</param>
        public static byte GetByte(uint rawValue, int num)
        {
            return (byte)(rawValue >> (num * 8));
        }

        /// <summary>
        /// Sets a certain bit from <paramref name="rawValue"/>.
        /// </summary>
        /// <returns>The new value after the bit is set.</returns>
        /// <param name="rawValue">The raw value to set the bit on.</param>
        /// <param name="bit">The index of the bit to set.</param>
        /// <param name="value">If set to <c>true</c>, set the bit to 1. Otherwise, set it to 0.</param>
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

        /// <summary>
        /// Gets a certain bit from <paramref name="rawValue"/>.
        /// </summary>
        /// <returns><c>true</c>, if the bit is 1. <c>false</c> otherwise.</returns>
        /// <param name="rawValue">The raw value to get the bit from.</param>
        /// <param name="bit">The index of the bit to set.</param>
        public static bool GetBit(uint rawValue, int bit)
        {
            return ((rawValue >> bit) & 0x01) == 0x01;
        }

        /// <summary>
        /// Converts the frame to displayed value.
        /// </summary>
        /// <returns>A displayable value.</returns>
        /// <param name="frame">The frame number to convert.</param>
        public static int ConvertFrameToDisplayedValue(int frame)
        {
            return frame + GetFrameInputRelationOffset();
        }

        /// <summary>
        /// Converts the displayed value to a frmae
        /// </summary>
        /// <returns>The frame number corresponding to the display value.</returns>
        /// <param name="displayedValue">The value that is displayed</param>
        public static int ConvertDisplayedValueToFrame(int displayedValue)
        {
            return displayedValue - GetFrameInputRelationOffset();
        }

        /// <summary>
        /// Get the frame input relation offset, in order to display it properly.
        /// </summary>
        /// <returns>The frame input relation offset.</returns>
        private static int GetFrameInputRelationOffset()
        {
            // note: personally, I would cache this value
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
