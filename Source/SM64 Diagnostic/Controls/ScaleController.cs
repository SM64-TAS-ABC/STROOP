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
        }
    }
}
