using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.M64Editor
{
    public class M64CopiedFrame
    {
        public readonly sbyte? X;
        public readonly sbyte? Y;
        public readonly bool? A;
        public readonly bool? B;
        public readonly bool? Z;
        public readonly bool? S;
        public readonly bool? R;
        public readonly bool? C_Up;
        public readonly bool? C_Down;
        public readonly bool? C_Left;
        public readonly bool? C_Right;
        public readonly bool? L;
        public readonly bool? D_Up;
        public readonly bool? D_Down;
        public readonly bool? D_Left;
        public readonly bool? D_Right;

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
        }

        public static M64CopiedFrame CreateCopiedFrame(M64InputFrame input, string inputsList)
        {
            return new M64CopiedFrame(
                inputsList.Contains("X") ? input.X : (sbyte?)null,
                inputsList.Contains("Y") ? input.Y : (sbyte?)null,
                inputsList.Contains("A") ? input.A : (bool?)null,
                inputsList.Contains("B") ? input.B : (bool?)null,
                inputsList.Contains("Z") ? input.Z : (bool?)null,
                inputsList.Contains("S") ? input.S : (bool?)null,
                inputsList.Contains("R") ? input.R : (bool?)null,
                inputsList.Contains("C^") ? input.C_Up : (bool?)null,
                inputsList.Contains("Cv") ? input.C_Down : (bool?)null,
                inputsList.Contains("C<") ? input.C_Left : (bool?)null,
                inputsList.Contains("C>") ? input.C_Right : (bool?)null,
                inputsList.Contains("L") ? input.L : (bool?)null,
                inputsList.Contains("D^") ? input.D_Up : (bool?)null,
                inputsList.Contains("Dv") ? input.D_Down : (bool?)null,
                inputsList.Contains("D<") ? input.D_Left : (bool?)null,
                inputsList.Contains("D>") ? input.D_Right : (bool?)null);
        }

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
