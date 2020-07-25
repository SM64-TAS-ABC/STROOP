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
using Accord.Video.FFMPEG;

namespace STROOP
{
    public class CoinRingDisplayPanel : Panel
    {
        private readonly Image _coinImage;

        public CoinRingDisplayPanel()
        {
            DoubleBuffered = true;

            _coinImage = Config.ObjectAssociations.GetObjectImage("Yellow Coin");
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle totalRect = new Rectangle(new Point(), Size);
            e.Graphics.DrawImage(_coinImage, totalRect);

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 24; col++)
                {
                    Rectangle rect = GetRectangle(row, col);
                    e.Graphics.DrawImage(_coinImage, rect);
                }
            }


            //if (_guiDictionary == null) return;
            //InputImageGui gui = _guiDictionary[_inputDisplayType];

            //e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            //Rectangle scaledRect = new Rectangle(new Point(), Size).Zoom(gui.ControllerImage.Size);
            //e.Graphics.DrawImage(gui.ControllerImage, scaledRect);

            //InputFrame inputs = _currentInputs;
            //if (inputs == null) return;

            //if (inputs.IsButtonPressed_A) e.Graphics.DrawImage(gui.ButtonAImage, scaledRect);
            //if (inputs.IsButtonPressed_B) e.Graphics.DrawImage(gui.ButtonBImage, scaledRect);
            //if (inputs.IsButtonPressed_Z) e.Graphics.DrawImage(gui.ButtonZImage, scaledRect);
            //if (inputs.IsButtonPressed_Start) e.Graphics.DrawImage(gui.ButtonStartImage, scaledRect);
            //if (inputs.IsButtonPressed_R) e.Graphics.DrawImage(gui.ButtonRImage, scaledRect);
            //if (inputs.IsButtonPressed_L) e.Graphics.DrawImage(gui.ButtonLImage, scaledRect);
            //if (inputs.IsButtonPressed_CUp) e.Graphics.DrawImage(gui.ButtonCUpImage, scaledRect);
            //if (inputs.IsButtonPressed_CDown) e.Graphics.DrawImage(gui.ButtonCDownImage, scaledRect);
            //if (inputs.IsButtonPressed_CLeft) e.Graphics.DrawImage(gui.ButtonCLeftImage, scaledRect);
            //if (inputs.IsButtonPressed_CRight) e.Graphics.DrawImage(gui.ButtonCRightImage, scaledRect);
            //if (inputs.IsButtonPressed_DUp) e.Graphics.DrawImage(gui.ButtonDUpImage, scaledRect);
            //if (inputs.IsButtonPressed_DDown) e.Graphics.DrawImage(gui.ButtonDDownImage, scaledRect);
            //if (inputs.IsButtonPressed_DLeft) e.Graphics.DrawImage(gui.ButtonDLeftImage, scaledRect);
            //if (inputs.IsButtonPressed_DRight) e.Graphics.DrawImage(gui.ButtonDRightImage, scaledRect);
            //if (inputs.IsButtonPressed_U1) e.Graphics.DrawImage(gui.ButtonU1Image, scaledRect);
            //if (inputs.IsButtonPressed_U2) e.Graphics.DrawImage(gui.ButtonU2Image, scaledRect);

            //float controlStickOffsetScale = GetScale(_inputDisplayType);
            //float hOffset = inputs.ControlStickH * controlStickOffsetScale * scaledRect.Width;
            //float vOffset = inputs.ControlStickV * controlStickOffsetScale * scaledRect.Width;

            //RectangleF controlStickRectange = new RectangleF(scaledRect.X + hOffset, scaledRect.Y - vOffset, scaledRect.Width, scaledRect.Height);
            //e.Graphics.DrawImage(gui.ControlStickImage, controlStickRectange);
        }

        private Rectangle GetRectangle(int row, int col)
        {
            bool tooWide = Size.Width > Size.Height * 6;
            int totalWidth = tooWide ? Size.Height * 6 : Size.Width;
            int totalHeight = tooWide ? Size.Height : Size.Width / 6;

            int rectWidth = totalWidth / 24;
            int rectHeight = totalHeight / 4;

            return new Rectangle(row * rectHeight, col * rectWidth, rectWidth, rectHeight);
        }
    }
}
