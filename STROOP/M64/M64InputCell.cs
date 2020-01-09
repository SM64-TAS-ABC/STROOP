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
    /// <summary>
    /// A cell corresponding to a .m64 recording input frame.
    /// </summary>
    public class M64InputCell
    {
        /// <summary>
        /// The text corresponding to a specific input.
        /// </summary>
        public readonly string HeaderText;

        /// <summary>
        /// Tell if this value is a specific input.
        /// </summary>
        public readonly bool IsInput;

        // m64 frame stored internally
        private readonly M64InputFrame InputFrame;

        /// <summary>
        /// Gets the frame number of the internal frame.
        /// </summary>
        /// <value>The index of the frame in the recording.</value>
        public int RowIndex { get => InputFrame.FrameIndex; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:STROOP.M64.M64InputCell"/> class.
        /// </summary>
        /// <param name="cell">The cell that this class represents.</param>
        public M64InputCell(DataGridViewCell cell)
        {
            DataGridView table = cell.DataGridView;
            HeaderText = table.Columns[cell.ColumnIndex].HeaderText;
            IsInput = M64Utilities.InputHeaderTexts.Contains(HeaderText);
            BindingList<M64InputFrame> inputs = table.DataSource as BindingList<M64InputFrame>;
            InputFrame = inputs[cell.RowIndex];
        }

        /// <summary>
        /// Set the value of this input.
        /// </summary>
        /// <param name="value">If set to <c>true</c>, the button is being pressed.</param>
        /// <param name="intOnValue">How much the joystick is being pushed.</param>
        public void SetValue(bool value, int? intOnValue = null)
        {
            M64Utilities.SetSpecificInputValue(InputFrame, HeaderText, value, intOnValue);
        }
    }
}
