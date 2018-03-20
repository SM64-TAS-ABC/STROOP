using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using STROOP.Structs;
using STROOP.Utilities;
using System.Xml.Linq;
using STROOP.Structs.Configurations;
using System.Drawing.Drawing2D;
using STROOP.Interfaces;
using STROOP.Controls.Map.Objects;

namespace STROOP.Controls.Map.Trackers
{
    public partial class ColorSelector : UserControl
    {
        public Color SelectedColor
        {
            get
            {
                return panelColorSelector.BackColor;
            }
            private set
            {
                panelColorSelector.BackColor = value;
                textBoxColorSelector.Text = ColorUtilities.ConvertColorToDecimal(value);
            }
        }

        public ColorSelector()
        {
            InitializeComponent();

            textBoxColorSelector.AddEnterAction(() =>
            {
                Color? newColor = ColorUtilities.ConvertDecimalToColor(textBoxColorSelector.Text);
                if (newColor.HasValue)
                {
                    SelectedColor = newColor.Value;
                }
                else
                {
                    textBoxColorSelector.Text = ColorUtilities.ConvertColorToDecimal(panelColorSelector.BackColor);
                }
            });

            panelColorSelector.Click += (sender, e) =>
            {
                ColorDialog colorDialog = new ColorDialog()
                {
                    FullOpen = true,
                    Color = SelectedColor,
                };
                if (colorDialog.ShowDialog() == DialogResult.OK)
                    SelectedColor = colorDialog.Color;
            };
        }
    }
}
