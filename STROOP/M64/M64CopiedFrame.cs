using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.M64
{
    /// <summary>
    /// A frame of copied data from a .m64 recording.
    /// </summary>
    /// <seealso cref="M64CopiedData"/>
    public class M64CopiedFrame
    {
        /// <summary>
        /// The horizontal push of the joystick.
        /// </summary>
        public readonly sbyte? X;

        /// <summary>
        /// The vertical push of the joystick.
        /// </summary>
        public readonly sbyte? Y;

        /// <summary>
        /// Whether the A button is being pressed.
        /// </summary>
        public readonly bool? A;

        /// <summary>
        /// Whether the B button is being pressed.
        /// </summary>
        public readonly bool? B;

        /// <summary>
        /// Whether the Z button is being pressed.
        /// </summary>
        public readonly bool? Z;

        /// <summary>
        /// Whether the S button is being pressed.
        /// </summary>
        public readonly bool? S;

        /// <summary>
        /// Whether the R button is being pressed.
        /// </summary>
        public readonly bool? R;

        /// <summary>
        /// Whether the C^ button is being pressed.
        /// </summary>
        public readonly bool? C_Up;

        /// <summary>
        /// Whether the Cv button is being pressed.
        /// </summary>
        public readonly bool? C_Down;

        /// <summary>
        /// Whether the C&lt; button is being pressed.
        /// </summary>
        public readonly bool? C_Left;

        /// <summary>
        /// Whether the C&gt; button is being pressed.
        /// </summary>
        public readonly bool? C_Right;

        /// <summary>
        /// Whether the L button is being pressed.
        /// </summary>
        public readonly bool? L;

        /// <summary>
        /// Whether the D^ button is being pressed.
        /// </summary>
        public readonly bool? D_Up;

        /// <summary>
        /// Whether the Dv button is being pressed.
        /// </summary>
        public readonly bool? D_Down;

        /// <summary>
        /// Whether the D&lt; button is being pressed.
        /// </summary>
        public readonly bool? D_Left;
      
        /// <summary>
        /// Whether the D&gt; button is being pressed.
        /// </summary>
        public readonly bool? D_Right;

        /// <summary>
        /// The raw value of this frame.
        /// </summary>
        /// <seealso cref="M64InputFrame.RawValue"/>
        public readonly uint RawValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:STROOP.M64.M64CopiedFrame"/> class.
        /// </summary>
        /// <param name="X">The horizontal push of the joystick.</param>
        /// <param name="Y">The vertical push of the joystick.</param>
        /// <param name="A">Whether the A button is being pressed.</param>
        /// <param name="B">Whether the B button is being pressed.</param>
        /// <param name="Z">Whether the Z button is being pressed.</param>
        /// <param name="S">Whether the S button is being pressed.</param>
        /// <param name="R">Whether the R button is being pressed.</param>
        /// <param name="C_Up">Whether the C^ button is being pressed.</param>
        /// <param name="C_Down">Whether the Cv button is being pressed.</param>
        /// <param name="C_Left">Whether the C&lt; button is being pressed.</param>
        /// <param name="C_Right">Whether the C&gt; button is being pressed.</param>
        /// <param name="L">Whether the L button is being pressed.</param>
        /// <param name="D_Up">Whether the D^ button is being pressed.</param>
        /// <param name="D_Down">Whether the Dv button is being pressed.</param>
        /// <param name="D_Left">Whether the D&lt; button is being pressed.</param>
        /// <param name="D_Right">Whether the D&gt; button is being pressed.</param>
        public M64CopiedFrame(
            sbyte? X = null,
            sbyte? Y = null,
            bool? A = null,
            bool? B = null,
            bool? Z = null,
            bool? S = null,
            bool? R = null,
            bool? C_Up = null,
            bool? C_Down = null,
            bool? C_Left = null,
            bool? C_Right = null,
            bool? L = null,
            bool? D_Up = null,
            bool? D_Down = null,
            bool? D_Left = null,
            bool? D_Right = null)
        {
            this.X = X;
            this.Y = Y;
            this.A = A;
            this.B = B;
            this.Z = Z;
            this.S = S;
            this.R = R;
            this.C_Up = C_Up;
            this.C_Down = C_Down;
            this.C_Left = C_Left;
            this.C_Right = C_Right;
            this.L = L;
            this.D_Up = D_Up;
            this.D_Down = D_Down;
            this.D_Left = D_Left;
            this.D_Right = D_Right;

            // get the raw value
            RawValue = M64Utilities.GetRawValueFromInputs(
                X ?? 0,
                Y ?? 0,
                A ?? false,
                B ?? false,
                Z ?? false,
                S ?? false,
                R ?? false,
                C_Up ?? false,
                C_Down ?? false,
                C_Left ?? false,
                C_Right ?? false,
                L ?? false,
                D_Up ?? false,
                D_Down ?? false,
                D_Left ?? false,
                D_Right ?? false);
        }

        /// <summary>
        /// Copy an input frame into a copied frame.
        /// </summary>
        /// <returns>The copied frame.</returns>
        /// <param name="input">Input frame to copy from.</param>
        /// <param name="useRows">If set to <c>true</c> use the entire rows, otherwise use <paramref name="inputsList"/>.</param>
        /// <param name="inputsList">Inputs to select.</param>
        public static M64CopiedFrame CreateCopiedFrame(M64InputFrame input, bool useRows, string inputsList)
        {
            return new M64CopiedFrame(
                useRows || inputsList.Contains("X") ? input.X : (sbyte?)null,
                useRows || inputsList.Contains("Y") ? input.Y : (sbyte?)null,
                useRows || inputsList.Contains("A") ? input.A : (bool?)null,
                useRows || inputsList.Contains("B") ? input.B : (bool?)null,
                useRows || inputsList.Contains("Z") ? input.Z : (bool?)null,
                useRows || inputsList.Contains("S") ? input.S : (bool?)null,
                useRows || inputsList.Contains("R") ? input.R : (bool?)null,
                useRows || inputsList.Contains("C^") ? input.C_Up : (bool?)null,
                useRows || inputsList.Contains("Cv") ? input.C_Down : (bool?)null,
                useRows || inputsList.Contains("C<") ? input.C_Left : (bool?)null,
                useRows || inputsList.Contains("C>") ? input.C_Right : (bool?)null,
                useRows || inputsList.Contains("L") ? input.L : (bool?)null,
                useRows || inputsList.Contains("D^") ? input.D_Up : (bool?)null,
                useRows || inputsList.Contains("Dv") ? input.D_Down : (bool?)null,
                useRows || inputsList.Contains("D<") ? input.D_Left : (bool?)null,
                useRows || inputsList.Contains("D>") ? input.D_Right : (bool?)null);
        }

        /// <summary>
        /// An empty frame of copied data.
        /// </summary>
        public static readonly M64CopiedFrame OneEmptyFrame =
            new M64CopiedFrame(
                0, 0, false, false, false, false, false, false,
                false, false, false, false, false, false, false, false);

        /// <summary>
        /// A frame consisting of pressing pause.
        /// </summary>
        public static readonly M64CopiedFrame OnePauseFrame =
            new M64CopiedFrame(
                0, 0, false, false, false, true, false, false,
                false, false, false, false, false, false, false, false);

        /// <summary>
        /// A frame consisting of pressing pause, but with all other inputs null.
        /// </summary>
        public static readonly M64CopiedFrame OnePauseFrameOverwrite =
            new M64CopiedFrame(S: true);

        /// <summary>
        /// Apply the copied data to an input frame.
        /// </summary>
        /// <param name="input">Input frame to apply to.</param>
        public void Apply(M64InputFrame input)
        {
            if (X.HasValue) input.X = X.Value;
            if (Y.HasValue) input.Y = Y.Value;
            if (A.HasValue) input.A = A.Value;
            if (B.HasValue) input.B = B.Value;
            if (Z.HasValue) input.Z = Z.Value;
            if (S.HasValue) input.S = S.Value;
            if (R.HasValue) input.R = R.Value;
            if (C_Up.HasValue) input.C_Up = C_Up.Value;
            if (C_Down.HasValue) input.C_Down = C_Down.Value;
            if (C_Left.HasValue) input.C_Left = C_Left.Value;
            if (C_Right.HasValue) input.C_Right = C_Right.Value;
            if (L.HasValue) input.L = L.Value;
            if (D_Up.HasValue) input.D_Up = D_Up.Value;
            if (D_Down.HasValue) input.D_Down = D_Down.Value;
            if (D_Left.HasValue) input.D_Left = D_Left.Value;
            if (D_Right.HasValue) input.D_Right = D_Right.Value;
        }
    }
}
