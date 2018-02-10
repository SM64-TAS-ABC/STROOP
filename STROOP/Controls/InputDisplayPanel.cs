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
        InputImageGui _gui;

        object _gfxLock = new object();

        public InputDisplayPanel()
        {
            this.DoubleBuffered = true;
        }

        public void setInputDisplayGui(InputImageGui gui)
        {
            _gui = gui;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_gui == null) return;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle scaledRect = new Rectangle(new Point(), Size).Zoom(_gui.ControllerImage.Size);
            e.Graphics.DrawImage(_gui.ControllerImage, scaledRect);

            uint inputAddress = InputConfig.CurrentInputAddress;

            bool buttonAPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonAOffset) & InputConfig.ButtonAMask) != 0;
            if (buttonAPressed) e.Graphics.DrawImage(_gui.ButtonAImage, scaledRect);

            bool buttonBPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonBOffset) & InputConfig.ButtonBMask) != 0;
            if (buttonBPressed) e.Graphics.DrawImage(_gui.ButtonBImage, scaledRect);

            bool buttonZPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonZOffset) & InputConfig.ButtonZMask) != 0;
            if (buttonZPressed) e.Graphics.DrawImage(_gui.ButtonZImage, scaledRect);

            bool buttonStartPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonStartOffset) & InputConfig.ButtonStartMask) != 0;
            if (buttonStartPressed) e.Graphics.DrawImage(_gui.ButtonStartImage, scaledRect);

            bool buttonRPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonROffset) & InputConfig.ButtonRMask) != 0;
            if (buttonRPressed) e.Graphics.DrawImage(_gui.ButtonRImage, scaledRect);

            bool buttonLPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonLOffset) & InputConfig.ButtonLMask) != 0;
            if (buttonLPressed) e.Graphics.DrawImage(_gui.ButtonLImage, scaledRect);

            bool buttonCUpPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCUpOffset) & InputConfig.ButtonCUpMask) != 0;
            if (buttonCUpPressed) e.Graphics.DrawImage(_gui.ButtonCUpImage, scaledRect);

            bool buttonCDownPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCDownOffset) & InputConfig.ButtonCDownMask) != 0;
            if (buttonCDownPressed) e.Graphics.DrawImage(_gui.ButtonCDownImage, scaledRect);

            bool buttonCLeftPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCLeftOffset) & InputConfig.ButtonCLeftMask) != 0;
            if (buttonCLeftPressed) e.Graphics.DrawImage(_gui.ButtonCLeftImage, scaledRect);

            bool buttonCRightPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonCRightOffset) & InputConfig.ButtonCRightMask) != 0;
            if (buttonCRightPressed) e.Graphics.DrawImage(_gui.ButtonCRightImage, scaledRect);

            bool buttonDUpPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDUpOffset) & InputConfig.ButtonDUpMask) != 0;
            if (buttonDUpPressed) e.Graphics.DrawImage(_gui.ButtonDUpImage, scaledRect);

            bool buttonDDownPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDDownOffset) & InputConfig.ButtonDDownMask) != 0;
            if (buttonDDownPressed) e.Graphics.DrawImage(_gui.ButtonDDownImage, scaledRect);

            bool buttonDLeftPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDLeftOffset) & InputConfig.ButtonDLeftMask) != 0;
            if (buttonDLeftPressed) e.Graphics.DrawImage(_gui.ButtonDLeftImage, scaledRect);

            bool buttonDRightPressed = (Config.Stream.GetByte(inputAddress + InputConfig.ButtonDRightOffset) & InputConfig.ButtonDRightMask) != 0;
            if (buttonDRightPressed) e.Graphics.DrawImage(_gui.ButtonDRightImage, scaledRect);

            float controlStickOffsetScale = 0.0003f;
            sbyte controlStickH = (sbyte)Config.Stream.GetByte(inputAddress + InputConfig.ControlStickXOffset);
            sbyte controlStickV = (sbyte)Config.Stream.GetByte(inputAddress + InputConfig.ControlStickYOffset);
            float hOffset = controlStickH * controlStickOffsetScale * scaledRect.Width;
            float vOffset = controlStickV * controlStickOffsetScale * scaledRect.Width;

            RectangleF controlStickRectange = new RectangleF(scaledRect.X + hOffset, scaledRect.Y - vOffset, scaledRect.Width, scaledRect.Height);
            e.Graphics.DrawImage(_gui.ControlStickImage, controlStickRectange);
        }
    }
}
