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
    public class ControllerDisplayPanel : Panel
    {
        ControllerImageGui _gui;
        ProcessStream _stream;

        object _gfxLock = new object();

        public ControllerDisplayPanel()
        {
            this.DoubleBuffered = true;
        }

        public void setControllerDisplayGui(ControllerImageGui gui)
        {
            _gui = gui;
        }

        public void setProcessStream(ProcessStream stream)
        {
            _stream = stream;
        }

        /*
        private void RebufferObjectImage()
        {
            // Remove last image reference
            _bufferedObjectImage = null;

            // Make sure object needs a new image
            if (_objectImage == null)
                return;

            // Calculate new rectangle to draw image
            var objectImageRec = (new Rectangle(BorderSize, BorderSize + 1,
            Width - BorderSize * 2, _textLocation.Y - 1 - BorderSize))
            .Zoom(_objectImage.Size);
            _objectImageLocation = objectImageRec.Location;

            // If the image is too small, we don't need to draw it
            if (objectImageRec.Height <= 0 || objectImageRec.Width <= 0)
            {
                _bufferedObjectImage = new Bitmap(1, 1);
                return;
            }

            // Look for cached image and use it if it exists
            _bufferedObjectImage = _manager.ObjectAssoc.GetCachedBufferedObjectImage(_objectImage, objectImageRec.Size);
            if (_bufferedObjectImage != null)
                return;

            // Otherwise create new image and add it to cache
            _bufferedObjectImage = new Bitmap(objectImageRec.Width, objectImageRec.Height);
            objectImageRec.Location = new Point();
            using (var graphics = Graphics.FromImage(_bufferedObjectImage))
            {
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.DrawImage(_objectImage, objectImageRec);
            }

            _manager.ObjectAssoc.CreateCachedBufferedObjectImage(_objectImage, _bufferedObjectImage);
        }
        */

        /*
        public void UpdateColors(bool force = false)
        {
            var oldBorderColor = _borderColor;
            var oldBackColor = _backColor;
            bool imageUpdated = false;
            var newColor = _mainColor;
            switch (MouseState)
            {
                case MouseStateType.Down:
                    _borderColor = newColor.Darken(0.5);
                    _backColor = newColor.Darken(0.5).Lighten(0.5);
                    break;
                case MouseStateType.Over:
                    _borderColor = newColor.Lighten(0.5);
                    _backColor = newColor.Lighten(0.85);
                    break;
                default:
                    _borderColor = newColor;
                    _backColor = newColor.Lighten(0.7);
                    break;
            }
            Image newImage = _manager.ObjectAssoc.GetObjectImage(_behavior, !_active);
            if (_objectImage != newImage)
            {
                lock (_gfxLock)
                {
                    _objectImage = newImage;
                    RebufferObjectImage();
                }
                imageUpdated = true;
            }

            bool colorUpdated = false;
            colorUpdated |= (_backColor != oldBackColor);
            colorUpdated |= (_borderColor != oldBorderColor);

            if (colorUpdated)
            {
                lock (_gfxLock)
                {
                    _borderBrush.Color = _borderColor;
                    _backBrush.Color = _backColor;
                }
            }

            if (!imageUpdated && !colorUpdated && !force)
                return;

            Invalidate();
        }
        */

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_gui == null) return;

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            Rectangle scaledRect = new Rectangle(new Point(), Size).Zoom(_gui.ControllerBaseImage.Size);
            e.Graphics.DrawImage(_gui.ControllerBaseImage, scaledRect);

            uint inputStruct = Config.Controller.BufferedInput;

            bool buttonAPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonAOffset) & Config.Controller.ButtonAMask) != 0;
            if (buttonAPressed) e.Graphics.DrawImage(_gui.ButtonAImage, scaledRect);

            bool buttonBPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonBOffset) & Config.Controller.ButtonBMask) != 0;
            if (buttonBPressed) e.Graphics.DrawImage(_gui.ButtonBImage, scaledRect);

            bool buttonZPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonZOffset) & Config.Controller.ButtonZMask) != 0;
            if (buttonZPressed) e.Graphics.DrawImage(_gui.ButtonZImage, scaledRect);

            bool buttonStartPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonStartOffset) & Config.Controller.ButtonStartMask) != 0;
            if (buttonStartPressed) e.Graphics.DrawImage(_gui.ButtonStartImage, scaledRect);

            bool buttonRPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonROffset) & Config.Controller.ButtonRMask) != 0;
            if (buttonRPressed) e.Graphics.DrawImage(_gui.ButtonRImage, scaledRect);

            bool buttonLPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonLOffset) & Config.Controller.ButtonLMask) != 0;
            if (buttonLPressed) e.Graphics.DrawImage(_gui.ButtonLImage, scaledRect);

            bool buttonCUpPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCUpOffset) & Config.Controller.ButtonCUpMask) != 0;
            if (buttonCUpPressed) e.Graphics.DrawImage(_gui.ButtonCUpImage, scaledRect);

            bool buttonCDownPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCDownOffset) & Config.Controller.ButtonCDownMask) != 0;
            if (buttonCDownPressed) e.Graphics.DrawImage(_gui.ButtonCDownImage, scaledRect);

            bool buttonCLeftPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCLeftOffset) & Config.Controller.ButtonCLeftMask) != 0;
            if (buttonCLeftPressed) e.Graphics.DrawImage(_gui.ButtonCLeftImage, scaledRect);

            bool buttonCRightPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCRightOffset) & Config.Controller.ButtonCRightMask) != 0;
            if (buttonCRightPressed) e.Graphics.DrawImage(_gui.ButtonCRightImage, scaledRect);

            bool buttonDUpPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDUpOffset) & Config.Controller.ButtonDUpMask) != 0;
            if (buttonDUpPressed) e.Graphics.DrawImage(_gui.ButtonDUpImage, scaledRect);

            bool buttonDDownPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDDownOffset) & Config.Controller.ButtonDDownMask) != 0;
            if (buttonDDownPressed) e.Graphics.DrawImage(_gui.ButtonDDownImage, scaledRect);

            bool buttonDLeftPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDLeftOffset) & Config.Controller.ButtonDLeftMask) != 0;
            if (buttonDLeftPressed) e.Graphics.DrawImage(_gui.ButtonDLeftImage, scaledRect);

            bool buttonDRightPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDRightOffset) & Config.Controller.ButtonDRightMask) != 0;
            if (buttonDRightPressed) e.Graphics.DrawImage(_gui.ButtonDRightImage, scaledRect);

            sbyte controlStickH = (sbyte)_stream.GetByte(inputStruct + Config.Controller.ControlStickHOffset);

            sbyte controlStickV = (sbyte)_stream.GetByte(inputStruct + Config.Controller.ControlStickVOffset);


            /*
            int index = row * numCols + col;
            if (index >= selectedBehaviorCriterias.Count) break;
            Image image = ObjectAssoc.GetObjectImage(selectedBehaviorCriterias[index], false);
            Rectangle rect = new Rectangle(col * imageSize, row * imageSize, imageSize, imageSize);
            Rectangle zoomedRect = rect.Zoom(image.Size);
            gfx.DrawImage(image, zoomedRect);
            */

            /*
            if (_drawWallObject)
                e.Graphics.DrawImage(_gui.WallObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawFloorObject)
                e.Graphics.DrawImage(_gui.FloorObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCeilingObject)
                e.Graphics.DrawImage(_gui.CeilingObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawInteractionObject)
                e.Graphics.DrawImage(_gui.InteractionObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawHeldOverlay)
                e.Graphics.DrawImage(_gui.HeldObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawStoodOnOverlay)
                e.Graphics.DrawImage(_gui.StoodOnObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawUsedObject)
                e.Graphics.DrawImage(_gui.UsedObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawClosestOverlay)
                e.Graphics.DrawImage(_gui.ClosestObjectOverlayImage, new Rectangle(new Point(), Size));
            if (_drawCameraOverlay)
                e.Graphics.DrawImage(_gui.CameraObjectOverlayImage, new Rectangle(new Point(), Size));
                */
        }


    }
}
