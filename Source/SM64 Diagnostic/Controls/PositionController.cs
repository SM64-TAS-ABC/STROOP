using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64_Diagnostic.Controls
{
    public class PositionController
    {
        protected Button _buttonSquareLeft;
        protected Button _buttonSquareRight;
        protected Button _buttonSquareTop;
        protected Button _buttonSquareBottom;
        protected Button _buttonSquareTopLeft;
        protected Button _buttonSquareBottomLeft;
        protected Button _buttonSquareTopRight;
        protected Button _buttonSquareBottomRight;
        protected Button _buttonLineTop;
        protected Button _buttonLineBottom;
        protected TextBox _textboxSquare;
        protected TextBox _textboxLine;
        protected CheckBox _checkbox;
        protected Action<float, float, float, bool> _actionMove;

        public PositionController(
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
            this._buttonSquareLeft = buttonSquareLeft;
            this._buttonSquareRight = buttonSquareRight;
            this._buttonSquareTop = buttonSquareTop;
            this._buttonSquareBottom = buttonSquareBottom;
            this._buttonSquareTopLeft = buttonSquareTopLeft;
            this._buttonSquareBottomLeft = buttonSquareBottomLeft;
            this._buttonSquareTopRight = buttonSquareTopRight;
            this._buttonSquareBottomRight = buttonSquareBottomRight;
            this._buttonLineTop = buttonLineTop;
            this._buttonLineBottom = buttonLineBottom;
            this._textboxSquare = textboxSquare;
            this._textboxLine = textboxLine;
            this._checkbox = checkbox;
            this._actionMove = actionMove;

            initialize();
        }

        private void initialize()
        {
            _buttonSquareLeft.Click += (sender, e) => actionSquare(-1, 0);
            _buttonSquareRight.Click += (sender, e) => actionSquare(1, 0);
            _buttonSquareTop.Click += (sender, e) => actionSquare(0, -1);
            _buttonSquareBottom.Click += (sender, e) => actionSquare(0, 1);
            _buttonSquareTopLeft.Click += (sender, e) => actionSquare(-1, -1);
            _buttonSquareBottomLeft.Click += (sender, e) => actionSquare(-1, 1);
            _buttonSquareTopRight.Click += (sender, e) => actionSquare(1, -1);
            _buttonSquareBottomRight.Click += (sender, e) => actionSquare(1, 1);
            _buttonLineTop.Click += (sender, e) => actionLine(1);
            _buttonLineBottom.Click += (sender, e) => actionLine(-1);
            _checkbox.CheckedChanged += (sender, e) => actionCheckedChanged(sender);
        }

        private void actionSquare(int hSign, int vSign)
        {
            float value;
            if (!float.TryParse(_textboxSquare.Text, out value))
                return;

            _actionMove(hSign * value, 0, vSign * value, _checkbox.Checked);
        }

        private void actionLine(int sign)
        {
            float value;
            if (!float.TryParse(_textboxLine.Text, out value))
                return;

            _actionMove(0, sign * value, 0, _checkbox.Checked);
        }

        private void actionCheckedChanged(object sender)
        {
            if ((sender as CheckBox).Checked)
            {
                _buttonSquareLeft.Text = "L";
                _buttonSquareRight.Text = "R";
                _buttonSquareTop.Text = "F";
                _buttonSquareBottom.Text = "B";
                _buttonSquareTopLeft.Text = "FL";
                _buttonSquareBottomLeft.Text = "BL";
                _buttonSquareTopRight.Text = "FR";
                _buttonSquareBottomRight.Text = "BR";
                _buttonLineTop.Text = "U";
                _buttonLineBottom.Text = "D";
            }
            else
            {
                _buttonSquareLeft.Text = "X-";
                _buttonSquareRight.Text = "X+";
                _buttonSquareTop.Text = "Z-";
                _buttonSquareBottom.Text = "Z+";
                _buttonSquareTopLeft.Text = "X-Z-";
                _buttonSquareBottomLeft.Text = "X-Z+";
                _buttonSquareTopRight.Text = "X+Z-";
                _buttonSquareBottomRight.Text = "X+Z+";
                _buttonLineTop.Text = "U";
                _buttonLineBottom.Text = "D";
            }
        }
    }
}
