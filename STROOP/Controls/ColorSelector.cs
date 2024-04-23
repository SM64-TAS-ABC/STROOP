﻿using System;
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

namespace STROOP.Controls
{
    public partial class ColorSelector : UserControl
    {
        public Color SelectedColor
        {
            get
            {
                return panelColorSelector.BackColor;
            }
            set
            {
                Color originalColor = SelectedColor;
                panelColorSelector.BackColor = value;
                textBoxColorSelector.SubmitText(ColorUtilities.ConvertColorToDecimal(value));
                if (value != originalColor)
                {
                    _colorChangeActions.ForEach(action => action(value));
                }
            }
        }

        private List<Action<Color>> _colorChangeActions = new List<Action<Color>>();

        public ColorSelector()
        {
            InitializeComponent();

            textBoxColorSelector.AddEnterAction(() => SubmitColorText());
            textBoxColorSelector.AddLostFocusAction(() => SubmitColorText());

            panelColorSelector.Click += (sender, e) =>
            {
                Config.MapManager.PauseMapUpdating = true;
                Color? newColor = ColorUtilities.GetColorFromDialog(SelectedColor);
                if (newColor.HasValue) SelectedColor = newColor.Value;
                Config.MapManager.PauseMapUpdating = false;
            };
        }

        public void AddColorChangeAction(Action<Color> action)
        {
            _colorChangeActions.Add(action);
        }

        private void SubmitColorText()
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
        }
    }
}
