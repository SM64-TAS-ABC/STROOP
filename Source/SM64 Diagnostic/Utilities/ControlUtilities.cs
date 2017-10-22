using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;

namespace SM64_Diagnostic.Utilities
{
    public static class ControlUtilities
    {
        public enum CoordinateSystem { Euler, Spherical };
        private enum FacingDirection { Left, Right, Up, Down, UpLeft, DownLeft, UpRight, DownRight };

        private static readonly string SUBTRACT_SYMBOL = "-";
        private static readonly string ADD_SYMBOL = "+";
        private static readonly string DIVIDE_SYMBOL = "÷";
        private static readonly string MULTIPLY_SYMBOL = "×";

        public static void InitializeThreeDimensionController(
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

            // Implement ToolStripMenu

            List<Button> buttonList = new List<Button>()
            {
                buttonSquareUp,
                buttonSquareUpRight,
                buttonSquareRight,
                buttonSquareDownRight,
                buttonSquareDown,
                buttonSquareDownLeft,
                buttonSquareLeft,
                buttonSquareUpLeft,
            };

            List<Point> positionList = buttonList.ConvertAll(
                button => new Point(button.Location.X, button.Location.Y));

            ToolStripMenuItem itemLeft = new ToolStripMenuItem("Face Left");
            ToolStripMenuItem itemRight = new ToolStripMenuItem("Face Right");
            ToolStripMenuItem itemUp = new ToolStripMenuItem("Face Up");
            ToolStripMenuItem itemDown = new ToolStripMenuItem("Face Down");
            ToolStripMenuItem itemUpLeft = new ToolStripMenuItem("Face Up-Left");
            ToolStripMenuItem itemDownLeft = new ToolStripMenuItem("Face Down-Left");
            ToolStripMenuItem itemUpRight = new ToolStripMenuItem("Face Up-Right");
            ToolStripMenuItem itemDownRight = new ToolStripMenuItem("Face Down-Right");

            Action<FacingDirection, int> SetFacingDirection = (FacingDirection facingDirection, int direction) =>
            {
                itemLeft.Checked = facingDirection == FacingDirection.Left;
                itemRight.Checked = facingDirection == FacingDirection.Right;
                itemUp.Checked = facingDirection == FacingDirection.Up;
                itemDown.Checked = facingDirection == FacingDirection.Down;
                itemUpLeft.Checked = facingDirection == FacingDirection.UpLeft;
                itemDownLeft.Checked = facingDirection == FacingDirection.DownLeft;
                itemUpRight.Checked = facingDirection == FacingDirection.UpRight;
                itemDownRight.Checked = facingDirection == FacingDirection.DownRight;

                for (int i = 0; i < buttonList.Count; i++)
                {
                    int newDirection = (direction + i) % buttonList.Count;
                    Point newPoint = positionList[newDirection];
                    Button button = buttonList[i];
                    button.Location = newPoint;
                }
            };

            itemLeft.Click += (sender, e) => SetFacingDirection(FacingDirection.Left, 6);
            itemRight.Click += (sender, e) => SetFacingDirection(FacingDirection.Right, 2);
            itemUp.Click += (sender, e) => SetFacingDirection(FacingDirection.Up, 0);
            itemDown.Click += (sender, e) => SetFacingDirection(FacingDirection.Down, 4);
            itemUpLeft.Click += (sender, e) => SetFacingDirection(FacingDirection.UpLeft, 7);
            itemDownLeft.Click += (sender, e) => SetFacingDirection(FacingDirection.DownLeft, 5);
            itemUpRight.Click += (sender, e) => SetFacingDirection(FacingDirection.UpRight, 1);
            itemDownRight.Click += (sender, e) => SetFacingDirection(FacingDirection.DownRight, 3);

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

        public static void InitializeScaleController(
            Button scaleWidthLeftButton,
            Button scaleWidthRightButton,
            Button scaleHeightLeftButton,
            Button scaleHeightRightButton,
            Button scaleDepthLeftButton,
            Button scaleDepthRightButton,
            Button scaleAggregateLeftButton,
            Button scaleAggregateRightButton,
            TextBox scaleWidthTextbox,
            TextBox scaleHeightTextbox,
            TextBox scaleDepthTextbox,
            TextBox scaleAggregateTextbox,
            CheckBox aggregateCheckbox,
            CheckBox multiplyCheckbox,
            Action<float, float, float, bool> actionScaleChange)
        {
            Action<bool> actionScaleWidthChange = (bool rightSide) =>
            {
                float rawValue;
                if (!float.TryParse(scaleWidthTextbox.Text, out rawValue)) return;

                // Don't divide by 0.
                if (rawValue == 0 && !rightSide && multiplyCheckbox.Checked) return;

                float widthValue = multiplyCheckbox.Checked
                    ? (rightSide ? rawValue : 1 / rawValue)
                    : (rightSide ? rawValue : -1 * rawValue);

                float defaultValue = multiplyCheckbox.Checked ? 1 : 0;

                actionScaleChange(widthValue, defaultValue, defaultValue, multiplyCheckbox.Checked);
            };

            Action<bool> actionScaleHeightChange = (bool rightSide) =>
            {
                float rawValue;
                if (!float.TryParse(scaleHeightTextbox.Text, out rawValue)) return;

                // Don't divide by 0.
                if (rawValue == 0 && !rightSide && multiplyCheckbox.Checked) return;

                float heightValue = multiplyCheckbox.Checked
                    ? (rightSide ? rawValue : 1 / rawValue)
                    : (rightSide ? rawValue : -1 * rawValue);

                float defaultValue = multiplyCheckbox.Checked ? 1 : 0;

                actionScaleChange(defaultValue, heightValue, defaultValue, multiplyCheckbox.Checked);
            };

            Action<bool> actionScaleDepthChange = (bool rightSide) =>
            {
                float rawValue;
                if (!float.TryParse(scaleDepthTextbox.Text, out rawValue)) return;

                // Don't divide by 0.
                if (rawValue == 0 && !rightSide && multiplyCheckbox.Checked) return;

                float depthValue = multiplyCheckbox.Checked
                    ? (rightSide ? rawValue : 1 / rawValue)
                    : (rightSide ? rawValue : -1 * rawValue);

                float defaultValue = multiplyCheckbox.Checked ? 1 : 0;

                actionScaleChange(defaultValue, defaultValue, depthValue, multiplyCheckbox.Checked);
            };

            Action<bool> actionScaleAggregateChange = (bool rightSide) =>
            {
                float rawValue;
                if (!float.TryParse(scaleAggregateTextbox.Text, out rawValue)) return;

                // Don't divide by 0.
                if (rawValue == 0 && !rightSide && multiplyCheckbox.Checked) return;

                float aggregateValue = multiplyCheckbox.Checked
                    ? (rightSide ? rawValue : 1 / rawValue)
                    : (rightSide ? rawValue : -1 * rawValue);

                actionScaleChange(aggregateValue, aggregateValue, aggregateValue, multiplyCheckbox.Checked);
            };

            Action<bool> setShowAggregate = (bool showAggregate) =>
            {
                scaleWidthLeftButton.Visible = !showAggregate;
                scaleWidthRightButton.Visible = !showAggregate;
                scaleHeightLeftButton.Visible = !showAggregate;
                scaleHeightRightButton.Visible = !showAggregate;
                scaleDepthLeftButton.Visible = !showAggregate;
                scaleDepthRightButton.Visible = !showAggregate;
                scaleWidthTextbox.Visible = !showAggregate;
                scaleHeightTextbox.Visible = !showAggregate;
                scaleDepthTextbox.Visible = !showAggregate;

                scaleAggregateLeftButton.Visible = showAggregate;
                scaleAggregateRightButton.Visible = showAggregate;
                scaleAggregateTextbox.Visible = showAggregate;
            };

            Action actionAggregateCheckedChanged = () =>
            {
                setShowAggregate(aggregateCheckbox.Checked);
            };

            Action<string, string> setOperationSymbols = (string leftSymbol, string rightSymbol) =>
            {
                scaleWidthLeftButton.Text = "Width" + leftSymbol;
                scaleWidthRightButton.Text = "Width" + rightSymbol;
                scaleHeightLeftButton.Text = "Height" + leftSymbol;
                scaleHeightRightButton.Text = "Height" + rightSymbol;
                scaleDepthLeftButton.Text = "Depth" + leftSymbol;
                scaleDepthRightButton.Text = "Depth" + rightSymbol;
                scaleAggregateLeftButton.Text = "Scale" + leftSymbol;
                scaleAggregateRightButton.Text = "Scale" + rightSymbol;
            };

            Action actionMultiplyCheckedChanged = () =>
            {
                if (multiplyCheckbox.Checked) setOperationSymbols(DIVIDE_SYMBOL, MULTIPLY_SYMBOL);
                else setOperationSymbols(SUBTRACT_SYMBOL, ADD_SYMBOL);
            };

            scaleWidthLeftButton.Click += (sender, e) => actionScaleWidthChange(false);
            scaleWidthRightButton.Click += (sender, e) => actionScaleWidthChange(true);
            scaleHeightLeftButton.Click += (sender, e) => actionScaleHeightChange(false);
            scaleHeightRightButton.Click += (sender, e) => actionScaleHeightChange(true);
            scaleDepthLeftButton.Click += (sender, e) => actionScaleDepthChange(false);
            scaleDepthRightButton.Click += (sender, e) => actionScaleDepthChange(true);
            scaleAggregateLeftButton.Click += (sender, e) => actionScaleAggregateChange(false);
            scaleAggregateRightButton.Click += (sender, e) => actionScaleAggregateChange(true);

            aggregateCheckbox.CheckedChanged += (sender, e) => actionAggregateCheckedChanged();
            multiplyCheckbox.CheckedChanged += (sender, e) => actionMultiplyCheckedChanged();

            AddInversionContextMenuStrip(scaleWidthLeftButton, scaleWidthRightButton);
            AddInversionContextMenuStrip(scaleHeightLeftButton, scaleHeightRightButton);
            AddInversionContextMenuStrip(scaleDepthLeftButton, scaleDepthRightButton);
            AddInversionContextMenuStrip(scaleAggregateLeftButton, scaleAggregateRightButton);
        }

        public static void InitializeScalarController(
            Button buttonLeft,
            Button buttonRight,
            TextBox textbox,
            Action<float> actionChangeScalar)
        {
            Action<int> actionButtonClick = (int sign) =>
            {
                float value;
                if (!float.TryParse(textbox.Text, out value)) return;
                actionChangeScalar(sign * value);
            };

            buttonLeft.Click += (sender, e) => actionButtonClick(-1);
            buttonRight.Click += (sender, e) => actionButtonClick(1);

            AddInversionContextMenuStrip(buttonLeft, buttonRight);
        }

        private static void AddInversionContextMenuStrip(
            Button buttonLeft,
            Button buttonRight)
        {
            Point leftPoint = new Point(buttonLeft.Location.X, buttonLeft.Location.Y);
            Point rightPoint = new Point(buttonRight.Location.X, buttonRight.Location.Y);

            ToolStripMenuItem itemNormal = new ToolStripMenuItem("Normal");
            ToolStripMenuItem itemInverted = new ToolStripMenuItem("Inverted");

            Action<bool> SetOrientation = (bool inverted) =>
            {
                itemNormal.Checked = !inverted;
                itemInverted.Checked = inverted;
                buttonLeft.Location = inverted ? rightPoint : leftPoint;
                buttonRight.Location = inverted ? leftPoint : rightPoint;
            };

            itemNormal.Click += (sender, e) => SetOrientation(false);
            itemInverted.Click += (sender, e) => SetOrientation(true);

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add(itemNormal);
            contextMenuStrip.Items.Add(itemInverted);
            buttonLeft.ContextMenuStrip = contextMenuStrip;
            buttonRight.ContextMenuStrip = contextMenuStrip;

            itemNormal.Checked = true;
        }
    }
}
