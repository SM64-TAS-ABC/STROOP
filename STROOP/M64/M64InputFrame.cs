using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Drawing;
using STROOP.Structs;
using System.Windows.Forms;

namespace STROOP.M64
{
    /// <summary>
    /// A frame in a .m64 recording where inputs take place.
    /// </summary>
    public class M64InputFrame
    {
        public static int ClassIdIndex = 0;

        /// <summary>
        /// The frame that these inputs took place on.
        /// </summary>
        public int FrameIndex;

        /// <summary>
        /// The value of the frame, containing all of the inputs within.
        /// </summary>
        public uint RawValue;

        public readonly int IdIndex;

        private readonly DataGridView _table;
        private readonly M64File _m64File;
        private readonly bool IsOriginalFrame;

        /// <summary>
        /// Instantiate a new instance of the <see cref="T:STROOP.M64.M64InputFrame"/> class.
        /// </summary>
        /// <param name="frameIndex">The frame this takes place on.</param>
        /// <param name="rawValue">The raw value of the frame.</param>
        /// <param name="isOriginalFrame">If set to <c>true</c>, this is a frame read directly from a .m64 file.</param>
        /// <param name="m64File">The recording file this frame is from.</param>
        /// <param name="table">The table this input frame is rendered onto.</param>
        public M64InputFrame(int frameIndex, uint rawValue, bool isOriginalFrame, M64File m64File, DataGridView table)
        {
            FrameIndex = frameIndex;
            RawValue = rawValue;
            IsOriginalFrame = isOriginalFrame;
            IdIndex = ClassIdIndex;
            ClassIdIndex++;

            _m64File = m64File;
            _table = table;

            // cache all of the input values
            _X = X;
            _Y = Y;
            _A = A;
            _B = B;
            _Z = Z;
            _S = S;
            _R = R;
            _C_Up = C_Up;
            _C_Down = C_Down;
            _C_Left = C_Left;
            _C_Right = C_Right;
            _L = L;
            _D_Up = D_Up;
            _D_Down = D_Down;
            _D_Left = D_Left;
            _D_Right = D_Right;
        }

        /// <summary>
        /// The frame that these inputs took place on, in a displayable format.
        /// </summary>
        /// <value>The frame this took place on.</value>
        public int Frame { get => M64Utilities.ConvertFrameToDisplayedValue(FrameIndex); }
        public int Id { get => M64Utilities.ConvertFrameToDisplayedValue(IdIndex); }

        /// <summary>
        /// The "x" value of the joystick.
        /// </summary>
        /// <value>The horizontal push of the joystick.</value>
        public sbyte X { get => (sbyte)GetByte(2); set { SetByte(2, (byte)value); NotifyChange(); } }

        /// <summary>
        /// The "y" value of the joystick.
        /// </summary>
        /// <value>The vertical push of the joystick. Positive means it is being pushed downwards, negative means upwards.</value>
        public sbyte Y { get => (sbyte)GetByte(3); set { SetByte(3, (byte)value); NotifyChange(); } }

        /// <summary>
        /// The "a" button.
        /// </summary>
        /// <value>Whether the A button is being pressed.</value>
        public bool A { get => GetBit(7); set { SetBit(7, value); NotifyChange(); } }

        /// <summary>
        /// The "b" button.
        /// </summary>
        /// <value>Whether the B button is being pressed.</value>
        public bool B { get => GetBit(6); set { SetBit(6, value); NotifyChange(); } }

        /// <summary>
        /// The "z" button.
        /// </summary>
        /// <value>Whether the Z button is being pressed.</value>
        public bool Z { get => GetBit(5); set { SetBit(5, value); NotifyChange(); } }

        /// <summary>
        /// The "s" button.
        /// </summary>
        /// <value>Whether the S button is being pressed.</value>
        public bool S { get => GetBit(4); set { SetBit(4, value); NotifyChange(); } }

        /// <summary>
        /// The "r" button.
        /// </summary>
        /// <value>Whether the R button is being pressed.</value>
        public bool R { get => GetBit(12); set { SetBit(12, value); NotifyChange(); } }

        /// <summary>
        /// The top "C" button.
        /// </summary>
        /// <value>Whether the C^ button is being pressed.</value>
        public bool C_Up { get => GetBit(11); set { SetBit(11, value); NotifyChange(); } }

        /// <summary>
        /// The bottom "C" button.
        /// </summary>
        /// <value>Whether the Cv button is being pressed.</value>
        public bool C_Down { get => GetBit(10); set { SetBit(10, value); NotifyChange(); } }

        /// <summary>
        /// The leftmost "C" button.
        /// </summary>
        /// <value>Whether the C&lt; button is being pressed.</value>
        public bool C_Left { get => GetBit(9); set { SetBit(9, value); NotifyChange(); } }

        /// <summary>
        /// The rightmost "C" button.
        /// </summary>
        /// <value>Whether the C&gt; button is being pressed.</value>
        public bool C_Right { get => GetBit(8); set { SetBit(8, value); NotifyChange(); } }

        /// <summary>
        /// The "L" button.
        /// </summary>
        /// <value>Whether the "L" button is being pressed.</value>
        public bool L { get => GetBit(13); set { SetBit(13, value); NotifyChange(); } }

        /// <summary>
        /// The top "D" button.
        /// </summary>
        /// <value>Whether the D^ button is being pressed.</value>
        public bool D_Up { get => GetBit(3); set { SetBit(3, value); NotifyChange(); } }

        /// <summary>
        /// The bottom "D" button.
        /// </summary>
        /// <value>Whether the Dv button is being pressed.</value>
        public bool D_Down { get => GetBit(2); set { SetBit(2, value); NotifyChange(); } }

        /// <summary>
        /// The leftmost "D" button.
        /// </summary>
        /// <value>Whether the D&lt; button is being pressed.</value>
        public bool D_Left { get => GetBit(1); set { SetBit(1, value); NotifyChange(); } }

        /// <summary>
        /// The rightmost "D" button.
        /// </summary>
        /// <value>Whether the D&gt; button is being pressed.</value>
        public bool D_Right { get => GetBit(0); set { SetBit(0, value); NotifyChange(); } }

        // input value caches, caches the original value
        private readonly sbyte _X;
        private readonly sbyte _Y;
        private readonly bool _A;
        private readonly bool _B;
        private readonly bool _Z;
        private readonly bool _S;
        private readonly bool _R;
        private readonly bool _C_Up;
        private readonly bool _C_Down;
        private readonly bool _C_Left;
        private readonly bool _C_Right;
        private readonly bool _L;
        private readonly bool _D_Up;
        private readonly bool _D_Down;
        private readonly bool _D_Left;
        private readonly bool _D_Right;

        // gets a list of the original values
        private List<object> GetOriginalValues()
        {
            return new List<object>()
            {
                _X, _Y,
                _A, _B, _Z, _S, _R,
                _C_Up, _C_Down, _C_Left, _C_Right,
                _L, _D_Up, _D_Down, _D_Left, _D_Right,
            };
        }

        // gets a list of the current values
        private List<object> GetCurrentValues()
        {
            return new List<object>()
            {
                X, Y,
                A, B, Z, S, R,
                C_Up, C_Down, C_Left, C_Right,
                L, D_Up, D_Down, D_Left, D_Right,
            };
        }

        // if there's a change, update things
        private void NotifyChange()
        {
            _m64File.IsModified = true;
            _m64File.ModifiedFrames.Add(this);
            UpdateCellColors();
        }

        /// <summary>
        /// Update the colors of the cells.
        /// </summary>
        public void UpdateCellColors()
        {
            List<object> originalValues = GetOriginalValues();
            List<object> currentvalues = GetCurrentValues();
            for (int i = 0; i < 16; i++)
            {
                bool valueChanged = !Equals(originalValues[i], currentvalues[i]);
                int columnIndex = i + 2;
                DataGridViewRow row = _table.Rows[FrameIndex];
                DataGridViewColumn col = _table.Columns[columnIndex];
                DataGridViewCell cell = row.Cells[columnIndex];
                Color defaultColor = row.DefaultCellStyle.BackColor == M64Config.NewRowColor ?
                    M64Config.NewRowColor : col.DefaultCellStyle.BackColor;
                cell.Style.BackColor = valueChanged ? M64Config.EditedCellColor : defaultColor;
            }
        }

        /// <summary>
        /// Updates the color of the row.
        /// </summary>
        public void UpdateRowColor()
        {
            if (!IsOriginalFrame)
            {
                DataGridViewRow row = _table.Rows[FrameIndex];
                row.DefaultCellStyle.BackColor = M64Config.NewRowColor;
            }
        }

        /// <summary>
        /// Sets a particular byte of the raw value.
        /// </summary>
        /// <param name="num">The index of the byte to set.</param>
        /// <param name="value">The value to set the byte to.</param>
        private void SetByte(int num, byte value)
        {
            RawValue = M64Utilities.SetByte(RawValue, num, value);
        }

        /// <summary>
        /// Gets a particular byte of the raw value.
        /// </summary>
        /// <returns>The byte selected.</returns>
        /// <param name="num">The index of the byte to select.</param>
        private byte GetByte(int num)
        {
            return M64Utilities.GetByte(RawValue, num);
        }

        /// <summary>
        /// Sets a particular bit of the raw value.
        /// </summary>
        /// <param name="bit">The bit of the index to set.</param>
        /// <param name="value">If set to <c>true</c>, set the bit to 1, otherwise 0.</param>
        private void SetBit(int bit, bool value)
        {
            RawValue = M64Utilities.SetBit(RawValue, bit, value);
        }

        /// <summary>
        /// Gets a particular bit of the raw value.
        /// </summary>
        /// <returns><c>true</c>, if bit is 1, <c>false</c> otherwise.</returns>
        /// <param name="bit">The index of the bit to set.</param>
        private bool GetBit(int bit)
        {
            return M64Utilities.GetBit(RawValue, bit);
        }

        /// <summary>
        /// Convert the raw value to a series of bytes.
        /// </summary>
        /// <returns>The bytes that were retrieved from the raw value.</returns>
        public byte[] ToBytes()
        {
            return BitConverter.GetBytes(RawValue).ToArray();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:STROOP.M64.M64InputFrame"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:STROOP.M64.M64InputFrame"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:STROOP.M64.M64InputFrame"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj is M64InputFrame input)
            {
                return IdIndex == input.IdIndex;
            }
            return false;
        }

        /// <summary>
        /// Serves as a hash function for a <see cref="T:STROOP.M64.M64InputFrame"/> object.
        /// </summary>
        /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a
        /// hash table.</returns>
        public override int GetHashCode()
        {
            return IdIndex;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:STROOP.M64.M64InputFrame"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:STROOP.M64.M64InputFrame"/>.</returns>
        public override string ToString()
        {
            return String.Format("Frame={0}, Id={1}, Inputs={2}", FrameIndex, IdIndex, GetInputsString());
        }

        /// <summary>
        /// Gets a string representation of the inputs.
        /// </summary>
        /// <returns>The inputs as a string.</returns>
        public string GetInputsString()
        {
            List<string> inputList = new List<string>();

            if (X != 0) inputList.Add("X" + X);
            if (Y != 0) inputList.Add("Y" + Y);

            if (A) inputList.Add("A");
            if (B) inputList.Add("B");
            if (Z) inputList.Add("Z");
            if (S) inputList.Add("S");
            if (R) inputList.Add("R");

            if (C_Up) inputList.Add("C^");
            if (C_Down) inputList.Add("Cv");
            if (C_Left) inputList.Add("C<");
            if (C_Right) inputList.Add("C>");

            if (L) inputList.Add("L");

            if (D_Up) inputList.Add("D^");
            if (D_Down) inputList.Add("Dv");
            if (D_Left) inputList.Add("D<");
            if (D_Right) inputList.Add("D>");

            return "[" + String.Join(",", inputList) + "]";

        }

        /// <summary>
        /// Compares two input values.
        /// </summary>
        /// <returns><c>true</c>, if inputs are equal, <c>false</c> otherwise.</returns>
        /// <param name="other">The other input frame to compare to.</param>
        public bool MatchesInputs(M64InputFrame other)
        {
            return Enumerable.SequenceEqual(GetCurrentValues(), other.GetCurrentValues());
        }
    }
}
