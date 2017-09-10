using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64Diagnostic.Controls
{
    public enum CoordinateSystem { Euler, Spherical };

    public static class ThreeDimensionController
    {
        public static void initialize(
            CoordinateSystem coordinateSystem,
            GroupBox groupbox,
            Button buttonSquareLeft,
            Button buttonSquareRight,
            Button buttonSquareTop,
            Button buttonSquareBottom,
            Button buttonSquareTopLeft,
            Button buttonSquareBottomLeft,
            Button buttonSquareTopRight,
            Button buttonSquareBottomRight,
            Button buttonLineTop,
            Button buttonLineBottom,
            TextBox textboxSquare,
            TextBox textboxLine,
            CheckBox checkbox,
            Action<float, float, float, bool> actionMove)
        {
            Action<int, int> actionSquare = (int hSign, int vSign) =>
            {
                float value;
                if (!float.TryParse(textboxSquare.Text, out value)) return;
                actionMove(hSign * value, vSign * value, 0, checkbox?.Checked ?? false);
            };

            Action<int> actionLine = (int nSign) =>
            {
                float value;
                if (!float.TryParse(textboxLine.Text, out value)) return;
                actionMove(0, 0, nSign * value, checkbox?.Checked ?? false);
            };

            Action setEulerNames = () =>
            {
                buttonSquareLeft.Text = "X-";
                buttonSquareRight.Text = "X+";
                buttonSquareTop.Text = "Z-";
                buttonSquareBottom.Text = "Z+";
                buttonSquareTopLeft.Text = "X-Z-";
                buttonSquareBottomLeft.Text = "X-Z+";
                buttonSquareTopRight.Text = "X+Z-";
                buttonSquareBottomRight.Text = "X+Z+";
                buttonLineTop.Text = "Y+";
                buttonLineBottom.Text = "Y-";
            };

            Action setRelativeNames = () =>
            {
                buttonSquareLeft.Text = "L";
                buttonSquareRight.Text = "R";
                buttonSquareTop.Text = "F";
                buttonSquareBottom.Text = "B";
                buttonSquareTopLeft.Text = "FL";
                buttonSquareBottomLeft.Text = "BL";
                buttonSquareTopRight.Text = "FR";
                buttonSquareBottomRight.Text = "BR";
                buttonLineTop.Text = "U";
                buttonLineBottom.Text = "D";
            };

            Action actionCheckedChanged = () =>
            {
                if (checkbox.Checked) setRelativeNames();
                else setEulerNames();
            };

            buttonSquareLeft.Click += (sender, e) => actionSquare(-1, 0);
            buttonSquareRight.Click += (sender, e) => actionSquare(1, 0);
            buttonSquareTop.Click += (sender, e) => actionSquare(0, 1);
            buttonSquareBottom.Click += (sender, e) => actionSquare(0, -1);
            buttonSquareTopLeft.Click += (sender, e) => actionSquare(-1, 1);
            buttonSquareBottomLeft.Click += (sender, e) => actionSquare(-1, -1);
            buttonSquareTopRight.Click += (sender, e) => actionSquare(1, 1);
            buttonSquareBottomRight.Click += (sender, e) => actionSquare(1, -1);
            buttonLineTop.Click += (sender, e) => actionLine(1);
            buttonLineBottom.Click += (sender, e) => actionLine(-1);
            if (coordinateSystem == CoordinateSystem.Euler)
            {
                checkbox.CheckedChanged += (sender, e) => actionCheckedChanged();
            }
        }
    }
}
