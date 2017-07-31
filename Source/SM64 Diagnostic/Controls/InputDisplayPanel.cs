using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using SM64_Diagnostic.Managers;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using System.Drawing.Drawing2D;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic
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

            uint inputStruct = Config.Input.CurrentInput;

            bool buttonAPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonAOffset) & Config.Input.ButtonAMask) != 0;
            if (buttonAPressed) e.Graphics.DrawImage(_gui.ButtonAImage, scaledRect);

            bool buttonBPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonBOffset) & Config.Input.ButtonBMask) != 0;
            if (buttonBPressed) e.Graphics.DrawImage(_gui.ButtonBImage, scaledRect);

            bool buttonZPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonZOffset) & Config.Input.ButtonZMask) != 0;
            if (buttonZPressed) e.Graphics.DrawImage(_gui.ButtonZImage, scaledRect);

            bool buttonStartPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonStartOffset) & Config.Input.ButtonStartMask) != 0;
            if (buttonStartPressed) e.Graphics.DrawImage(_gui.ButtonStartImage, scaledRect);

            bool buttonRPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonROffset) & Config.Input.ButtonRMask) != 0;
            if (buttonRPressed) e.Graphics.DrawImage(_gui.ButtonRImage, scaledRect);

            bool buttonLPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonLOffset) & Config.Input.ButtonLMask) != 0;
            if (buttonLPressed) e.Graphics.DrawImage(_gui.ButtonLImage, scaledRect);

            bool buttonCUpPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonCUpOffset) & Config.Input.ButtonCUpMask) != 0;
            if (buttonCUpPressed) e.Graphics.DrawImage(_gui.ButtonCUpImage, scaledRect);

            bool buttonCDownPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonCDownOffset) & Config.Input.ButtonCDownMask) != 0;
            if (buttonCDownPressed) e.Graphics.DrawImage(_gui.ButtonCDownImage, scaledRect);

            bool buttonCLeftPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonCLeftOffset) & Config.Input.ButtonCLeftMask) != 0;
            if (buttonCLeftPressed) e.Graphics.DrawImage(_gui.ButtonCLeftImage, scaledRect);

            bool buttonCRightPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonCRightOffset) & Config.Input.ButtonCRightMask) != 0;
            if (buttonCRightPressed) e.Graphics.DrawImage(_gui.ButtonCRightImage, scaledRect);

            bool buttonDUpPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonDUpOffset) & Config.Input.ButtonDUpMask) != 0;
            if (buttonDUpPressed) e.Graphics.DrawImage(_gui.ButtonDUpImage, scaledRect);

            bool buttonDDownPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonDDownOffset) & Config.Input.ButtonDDownMask) != 0;
            if (buttonDDownPressed) e.Graphics.DrawImage(_gui.ButtonDDownImage, scaledRect);

            bool buttonDLeftPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonDLeftOffset) & Config.Input.ButtonDLeftMask) != 0;
            if (buttonDLeftPressed) e.Graphics.DrawImage(_gui.ButtonDLeftImage, scaledRect);

            bool buttonDRightPressed = (Config.Stream.GetByte(inputStruct + Config.Input.ButtonDRightOffset) & Config.Input.ButtonDRightMask) != 0;
            if (buttonDRightPressed) e.Graphics.DrawImage(_gui.ButtonDRightImage, scaledRect);

            float controlStickOffsetScale = 0.0003f;
            sbyte controlStickH = (sbyte)Config.Stream.GetByte(inputStruct + Config.Input.ControlStickHOffset);
            sbyte controlStickV = (sbyte)Config.Stream.GetByte(inputStruct + Config.Input.ControlStickVOffset);
            float hOffset = controlStickH * controlStickOffsetScale * scaledRect.Width;
            float vOffset = controlStickV * controlStickOffsetScale * scaledRect.Width;

            RectangleF controlStickRectange = new RectangleF(scaledRect.X + hOffset, scaledRect.Y - vOffset, scaledRect.Width, scaledRect.Height);
            e.Graphics.DrawImage(_gui.ControlStickImage, controlStickRectange);
        }
    }
}
