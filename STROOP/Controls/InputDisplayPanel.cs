using System;
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
            InputImageGui gui = _guiDictionary[_inputDisplayType];

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle scaledRect = new Rectangle(new Point(), Size).Zoom(gui.ControllerImage.Size);
            e.Graphics.DrawImage(gui.ControllerImage, scaledRect);

            uint inputAddress = InputConfig.CurrentInputAddress;

            bool buttonAPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonAOffset) & InputConfig.ButtonAMask) != 0;
            if (buttonAPressed) e.Graphics.DrawImage(gui.ButtonAImage, scaledRect);

            bool buttonBPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonBOffset) & InputConfig.ButtonBMask) != 0;
            if (buttonBPressed) e.Graphics.DrawImage(gui.ButtonBImage, scaledRect);

            bool buttonZPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonZOffset) & InputConfig.ButtonZMask) != 0;
            if (buttonZPressed) e.Graphics.DrawImage(gui.ButtonZImage, scaledRect);

            bool buttonStartPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonStartOffset) & InputConfig.ButtonStartMask) != 0;
            if (buttonStartPressed) e.Graphics.DrawImage(gui.ButtonStartImage, scaledRect);

            bool buttonRPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonROffset) & InputConfig.ButtonRMask) != 0;
            if (buttonRPressed) e.Graphics.DrawImage(gui.ButtonRImage, scaledRect);

            bool buttonLPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonLOffset) & InputConfig.ButtonLMask) != 0;
            if (buttonLPressed) e.Graphics.DrawImage(gui.ButtonLImage, scaledRect);

            bool buttonCUpPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCUpOffset) & InputConfig.ButtonCUpMask) != 0;
            if (buttonCUpPressed) e.Graphics.DrawImage(gui.ButtonCUpImage, scaledRect);

            bool buttonCDownPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCDownOffset) & InputConfig.ButtonCDownMask) != 0;
            if (buttonCDownPressed) e.Graphics.DrawImage(gui.ButtonCDownImage, scaledRect);

            bool buttonCLeftPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCLeftOffset) & InputConfig.ButtonCLeftMask) != 0;
            if (buttonCLeftPressed) e.Graphics.DrawImage(gui.ButtonCLeftImage, scaledRect);

            bool buttonCRightPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCRightOffset) & InputConfig.ButtonCRightMask) != 0;
            if (buttonCRightPressed) e.Graphics.DrawImage(gui.ButtonCRightImage, scaledRect);

            bool buttonDUpPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDUpOffset) & InputConfig.ButtonDUpMask) != 0;
            if (buttonDUpPressed) e.Graphics.DrawImage(gui.ButtonDUpImage, scaledRect);

            bool buttonDDownPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDDownOffset) & InputConfig.ButtonDDownMask) != 0;
            if (buttonDDownPressed) e.Graphics.DrawImage(gui.ButtonDDownImage, scaledRect);

            bool buttonDLeftPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDLeftOffset) & InputConfig.ButtonDLeftMask) != 0;
            if (buttonDLeftPressed) e.Graphics.DrawImage(gui.ButtonDLeftImage, scaledRect);

            bool buttonDRightPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDRightOffset) & InputConfig.ButtonDRightMask) != 0;
            if (buttonDRightPressed) e.Graphics.DrawImage(gui.ButtonDRightImage, scaledRect);

            bool buttonU1Pressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonU1Offset) & InputConfig.ButtonU1Mask) != 0;
            if (buttonU1Pressed) e.Graphics.DrawImage(gui.ButtonU1Image, scaledRect);

            bool buttonU2Pressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonU2Offset) & InputConfig.ButtonU2Mask) != 0;
            if (buttonU2Pressed) e.Graphics.DrawImage(gui.ButtonU2Image, scaledRect);

            float controlStickOffsetScale = GetScale(_inputDisplayType);
            sbyte controlStickH = (sbyte)Config.Stream.GetByte(inputAddress + InputConfig.ControlStickXOffset);
            sbyte controlStickV = (sbyte)Config.Stream.GetByte(inputAddress + InputConfig.ControlStickYOffset);
            float hOffset = controlStickH * controlStickOffsetScale * scaledRect.Width;
            float vOffset = controlStickV * controlStickOffsetScale * scaledRect.Width;

            RectangleF controlStickRectange = new RectangleF(scaledRect.X + hOffset, scaledRect.Y - vOffset, scaledRect.Width, scaledRect.Height);
            e.Graphics.DrawImage(gui.ControlStickImage, controlStickRectange);
        }
}
}
