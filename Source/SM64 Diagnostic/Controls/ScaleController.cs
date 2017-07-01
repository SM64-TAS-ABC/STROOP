using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM64Diagnostic.Controls
{
    public static class ScaleController
    {
        private static readonly string SUBTRACT_SYMBOL = "-";
        private static readonly string ADD_SYMBOL = "+";
        private static readonly string DIVIDE_SYMBOL = "÷";
        private static readonly string MULTIPLY_SYMBOL = "×";

        public static void initialize(
            Button scaleWidthLeftButton,
            Button scaleWidthRightButton,
            Button scaleHeightLeftButton,
            Button scaleHeightRightButton,
            Button scaleDepthLeftButton,
            Button scaleDepthRightButton,
            TextBox scaleWidthTextbox,
            TextBox scaleHeightTextbox,
            TextBox scaleDepthTextbox,
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

            Action setAdditionNames = () =>
            {
                scaleWidthLeftButton.Text = "Width" + SUBTRACT_SYMBOL;
                scaleWidthRightButton.Text = "Width" + ADD_SYMBOL;
                scaleHeightLeftButton.Text = "Height" + SUBTRACT_SYMBOL;
                scaleHeightRightButton.Text = "Height" + ADD_SYMBOL;
                scaleDepthLeftButton.Text = "Depth" + SUBTRACT_SYMBOL;
                scaleDepthRightButton.Text = "Depth" + ADD_SYMBOL;
            };

            Action setMultiplicationNames = () =>
            {
                scaleWidthLeftButton.Text = "Width" + DIVIDE_SYMBOL;
                scaleWidthRightButton.Text = "Width" + MULTIPLY_SYMBOL;
                scaleHeightLeftButton.Text = "Height" + DIVIDE_SYMBOL;
                scaleHeightRightButton.Text = "Height" + MULTIPLY_SYMBOL;
                scaleDepthLeftButton.Text = "Depth" + DIVIDE_SYMBOL;
                scaleDepthRightButton.Text = "Depth" + MULTIPLY_SYMBOL;
            };

            Action actionMultiplyCheckedChanged = () =>
            {
                if (multiplyCheckbox.Checked) setMultiplicationNames();
                else setAdditionNames();
            };

            scaleWidthLeftButton.Click += (sender, e) => actionScaleWidthChange(false);
            scaleWidthRightButton.Click += (sender, e) => actionScaleWidthChange(true);
            scaleHeightLeftButton.Click += (sender, e) => actionScaleHeightChange(false);
            scaleHeightRightButton.Click += (sender, e) => actionScaleHeightChange(true);
            scaleDepthLeftButton.Click += (sender, e) => actionScaleDepthChange(false);
            scaleDepthRightButton.Click += (sender, e) => actionScaleDepthChange(true);
            multiplyCheckbox.CheckedChanged += (sender, e) => actionMultiplyCheckedChanged();
        }
    }
}
