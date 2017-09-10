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
    public enum FacingDirection { Left, Right, Up, Down, UpLeft, DownLeft, UpRight, DownRight };

    public static class ThreeDimensionController
    {
        public static void initialize(
            CoordinateSystem coordinateSystem,
            GroupBox groupbox,
            Button buttonSquareLeft,
            Button buttonSquareRight,
            Button buttonSquareUp,
            Button buttonSquareDown,
            Button buttonSquareUpLeft,
            Button buttonSquareDownLeft,
            Button buttonSquareUpRight,
            Button buttonSquareDownRight,
            Button buttonLineUp,
            Button buttonLineDown,
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
                buttonSquareUp.Text = "Z-";
                buttonSquareDown.Text = "Z+";
                buttonSquareUpLeft.Text = "X-Z-";
                buttonSquareDownLeft.Text = "X-Z+";
                buttonSquareUpRight.Text = "X+Z-";
                buttonSquareDownRight.Text = "X+Z+";
                buttonLineUp.Text = "Y+";
                buttonLineDown.Text = "Y-";
            };

            Action setRelativeNames = () =>
            {
                buttonSquareLeft.Text = "L";
                buttonSquareRight.Text = "R";
                buttonSquareUp.Text = "F";
                buttonSquareDown.Text = "B";
                buttonSquareUpLeft.Text = "FL";
                buttonSquareDownLeft.Text = "BL";
                buttonSquareUpRight.Text = "FR";
                buttonSquareDownRight.Text = "BR";
                buttonLineUp.Text = "U";
                buttonLineDown.Text = "D";
            };

            Action actionCheckedChanged = () =>
            {
                if (checkbox.Checked) setRelativeNames();
                else setEulerNames();
            };

            buttonSquareLeft.Click += (sender, e) => actionSquare(-1, 0);
            buttonSquareRight.Click += (sender, e) => actionSquare(1, 0);
            buttonSquareUp.Click += (sender, e) => actionSquare(0, 1);
            buttonSquareDown.Click += (sender, e) => actionSquare(0, -1);
            buttonSquareUpLeft.Click += (sender, e) => actionSquare(-1, 1);
            buttonSquareDownLeft.Click += (sender, e) => actionSquare(-1, -1);
            buttonSquareUpRight.Click += (sender, e) => actionSquare(1, 1);
            buttonSquareDownRight.Click += (sender, e) => actionSquare(1, -1);
            buttonLineUp.Click += (sender, e) => actionLine(1);
            buttonLineDown.Click += (sender, e) => actionLine(-1);
            if (coordinateSystem == CoordinateSystem.Euler)
            {
                checkbox.CheckedChanged += (sender, e) => actionCheckedChanged();
            }

            ToolStripMenuItem itemLeft = new ToolStripMenuItem("Face Left");
            ToolStripMenuItem itemRight = new ToolStripMenuItem("Face Right");
            ToolStripMenuItem itemUp = new ToolStripMenuItem("Face Up");
            ToolStripMenuItem itemDown = new ToolStripMenuItem("Face Down");
            ToolStripMenuItem itemUpLeft = new ToolStripMenuItem("Face Up-Left");
            ToolStripMenuItem itemDownLeft = new ToolStripMenuItem("Face Down-Left");
            ToolStripMenuItem itemUpRight = new ToolStripMenuItem("Face Up-Right");
            ToolStripMenuItem itemDownRight = new ToolStripMenuItem("Face Down-Right");

            Action<FacingDirection> SetFacingDirection = (FacingDirection facingDirection) =>
            {
                itemLeft.Checked = facingDirection == FacingDirection.Left;
                itemRight.Checked = facingDirection == FacingDirection.Right;
                itemUp.Checked = facingDirection == FacingDirection.Up;
                itemDown.Checked = facingDirection == FacingDirection.Down;
                itemUpLeft.Checked = facingDirection == FacingDirection.UpLeft;
                itemDownLeft.Checked = facingDirection == FacingDirection.DownLeft;
                itemUpRight.Checked = facingDirection == FacingDirection.UpRight;
                itemDownRight.Checked = facingDirection == FacingDirection.DownRight;
            };

            itemLeft.Click += (sender, e) => SetFacingDirection(FacingDirection.Left);
            itemRight.Click += (sender, e) => SetFacingDirection(FacingDirection.Right);
            itemUp.Click += (sender, e) => SetFacingDirection(FacingDirection.Up);
            itemDown.Click += (sender, e) => SetFacingDirection(FacingDirection.Down);
            itemUpLeft.Click += (sender, e) => SetFacingDirection(FacingDirection.UpLeft);
            itemDownLeft.Click += (sender, e) => SetFacingDirection(FacingDirection.DownLeft);
            itemUpRight.Click += (sender, e) => SetFacingDirection(FacingDirection.UpRight);
            itemDownRight.Click += (sender, e) => SetFacingDirection(FacingDirection.DownRight);

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(itemLeft);
            contextMenuStrip.Items.Add(itemRight);
            contextMenuStrip.Items.Add(itemUp);
            contextMenuStrip.Items.Add(itemDown);
            contextMenuStrip.Items.Add(itemUpLeft);
            contextMenuStrip.Items.Add(itemDownLeft);
            contextMenuStrip.Items.Add(itemUpRight);
            contextMenuStrip.Items.Add(itemDownRight);
            groupbox.ContextMenuStrip = contextMenuStrip;

            itemUp.Checked = true;
        }

    }
}
