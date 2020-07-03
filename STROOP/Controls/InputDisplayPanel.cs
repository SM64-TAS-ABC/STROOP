﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using STROOP.Managers;
using STROOP.Utilities;
using STROOP.Structs;
using STROOP.Controls;
using STROOP.Extensions;
using System.Drawing.Drawing2D;
using STROOP.Structs.Configurations;

namespace STROOP
{
    public class InputDisplayPanel : Panel
    {
        List<InputImageGui> _guiList;
        Dictionary<InputDisplayTypeEnum, InputImageGui> _guiDictionary;
        InputDisplayTypeEnum _inputDisplayType;

        object _gfxLock = new object();

        public InputDisplayPanel()
        {
            this.DoubleBuffered = true;
        }

        public void SetInputDisplayGui(List<InputImageGui> guiList)
        {
            _guiList = guiList;
            _guiDictionary = new Dictionary<InputDisplayTypeEnum, InputImageGui>();
            _guiList.ForEach(gui => _guiDictionary.Add(gui.InputDisplayType, gui));
            _inputDisplayType = InputDisplayTypeEnum.Classic;

            List<ToolStripMenuItem> items = _guiList.ConvertAll(
                gui => new ToolStripMenuItem(gui.InputDisplayType.ToString()));
            for (int i = 0; i < items.Count; i++)
            {
                ToolStripMenuItem item = items[i];
                InputImageGui gui = _guiList[i];
                InputDisplayTypeEnum inputDisplayType = gui.InputDisplayType;

                item.Click += (sender, e) =>
                {
                    BackColor = GetBackColor(inputDisplayType);
                    _inputDisplayType = inputDisplayType;
                    items.ForEach(item2 => item2.Checked = item2 == item);
                };

                item.Checked = inputDisplayType == _inputDisplayType;
            }

            ContextMenuStrip = new ContextMenuStrip();
            items.ForEach(item => ContextMenuStrip.Items.Add(item));
        }

        private Color GetBackColor(InputDisplayTypeEnum inputDisplayType)
        {
            switch (inputDisplayType)
            {
                case InputDisplayTypeEnum.Classic:
                    return SystemColors.Control;
                case InputDisplayTypeEnum.Sleek:
                    return Color.Black;
                case InputDisplayTypeEnum.Vertical:
                    return Color.Black;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private float GetScale(InputDisplayTypeEnum inputDisplayType)
        {
            switch (inputDisplayType)
            {
                case InputDisplayTypeEnum.Classic:
                    return 0.0003f;
                case InputDisplayTypeEnum.Sleek:
                    return 0.0007f;
                case InputDisplayTypeEnum.Vertical:
                    return 0.0014f;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_guiDictionary == null) return;
            InputImageGui gui = _guiDictionary[_inputDisplayType];

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle scaledRect = new Rectangle(new Point(), Size).Zoom(gui.ControllerImage.Size);
            e.Graphics.DrawImage(gui.ControllerImage, scaledRect);
            
            InputFrame inputs = InputFrame.GetCurrent();
            
            if (inputs.IsButtonPressed_A) e.Graphics.DrawImage(gui.ButtonAImage, scaledRect);
            if (inputs.IsButtonPressed_B) e.Graphics.DrawImage(gui.ButtonBImage, scaledRect);
            if (inputs.IsButtonPressed_Z) e.Graphics.DrawImage(gui.ButtonZImage, scaledRect);
            if (inputs.IsButtonPressed_Start) e.Graphics.DrawImage(gui.ButtonStartImage, scaledRect);
            if (inputs.IsButtonPressed_R) e.Graphics.DrawImage(gui.ButtonRImage, scaledRect);
            if (inputs.IsButtonPressed_L) e.Graphics.DrawImage(gui.ButtonLImage, scaledRect);
            if (inputs.IsButtonPressed_CUp) e.Graphics.DrawImage(gui.ButtonCUpImage, scaledRect);
            if (inputs.IsButtonPressed_CDown) e.Graphics.DrawImage(gui.ButtonCDownImage, scaledRect);
            if (inputs.IsButtonPressed_CLeft) e.Graphics.DrawImage(gui.ButtonCLeftImage, scaledRect);
            if (inputs.IsButtonPressed_CRight) e.Graphics.DrawImage(gui.ButtonCRightImage, scaledRect);
            if (inputs.IsButtonPressed_DUp) e.Graphics.DrawImage(gui.ButtonDUpImage, scaledRect);
            if (inputs.IsButtonPressed_DDown) e.Graphics.DrawImage(gui.ButtonDDownImage, scaledRect);
            if (inputs.IsButtonPressed_DLeft) e.Graphics.DrawImage(gui.ButtonDLeftImage, scaledRect);
            if (inputs.IsButtonPressed_DRight) e.Graphics.DrawImage(gui.ButtonDRightImage, scaledRect);
            if (inputs.IsButtonPressed_U1) e.Graphics.DrawImage(gui.ButtonU1Image, scaledRect);
            if (inputs.IsButtonPressed_U2) e.Graphics.DrawImage(gui.ButtonU2Image, scaledRect);

            float controlStickOffsetScale = GetScale(_inputDisplayType);
            float hOffset = inputs.ControlStickH * controlStickOffsetScale * scaledRect.Width;
            float vOffset = inputs.ControlStickV * controlStickOffsetScale * scaledRect.Width;

            RectangleF controlStickRectange = new RectangleF(scaledRect.X + hOffset, scaledRect.Y - vOffset, scaledRect.Width, scaledRect.Height);
            e.Graphics.DrawImage(gui.ControlStickImage, controlStickRectange);
        }
}
}
