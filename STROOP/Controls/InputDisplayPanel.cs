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
    public class InputDisplayPanel : Panel
    {
        List<InputImageGui> _guiList;
        Dictionary<InputDisplayTypeEnum, InputImageGui> _guiDictionary;
        InputDisplayTypeEnum _inputDisplayType;
        bool _isRecording = false;
        RecordingSession _currentRecordingSession;
        InputFrame _currentInputs = null;

        class RecordingSession
        {
            public List<(int, InputFrame)> Inputs { get; set; } = new List<(int, InputFrame)>();

            public int LastViCount { get; set; }
            public InputFrame LastInputFrame { get; set; }
            public Size Size { get; set; }

            public RecordingSession(Size imageSize)
            {
                Size = new Size((imageSize.Width / 2) * 2, (imageSize.Height / 2) * 2);
                LastViCount = MupenUtilities.GetVICount();
                LastInputFrame = InputFrame.GetCurrent();
            }

            public void AddFrame(InputFrame inputs)
            {
                const double visPerSecond = 60;
                int viCount = MupenUtilities.GetVICount();

                if (LastViCount == viCount) return;

                int deltaVi = viCount - LastViCount;
                TimeSpan time = TimeSpan.FromSeconds((deltaVi) / visPerSecond);
                LastViCount = viCount;

                Inputs.Add((deltaVi, inputs));
            }

            public void Render(string filePath, Control controlToRender, Action<InputFrame> setCurrentInput) 
            {
                using (VideoFileWriter videoWriter = new VideoFileWriter())
                {
                    videoWriter.Open(filePath, Size.Width, Size.Height, 60, VideoCodec.Default, Size.Width * Size.Height * 1000);

                    foreach ((int deltaVi, InputFrame inputs) in Inputs) {
                        using (Bitmap bitmap = new Bitmap(Size.Width, Size.Height))
                        {
                            setCurrentInput(inputs);
                            controlToRender.DrawToBitmap(bitmap, new Rectangle(new Point(), Size));

                            for (int i = 0; i < deltaVi; i++)
                            {
                                videoWriter.WriteVideoFrame(bitmap);
                            }
                        }
                    }

                    videoWriter.Close();
                };
            }
        }


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

            ContextMenuStrip = new ContextMenuStrip() { };
            items.ForEach(item => ContextMenuStrip.Items.Add(item));

            ToolStripMenuItem recordToolStrip = new ToolStripMenuItem("Record Video");
            recordToolStrip.Click += RecordToolStrip_Click;
            ContextMenuStrip.Items.Add(new ToolStripSeparator());
            ContextMenuStrip.Items.Add(recordToolStrip);
        }

        public void UpdateInputs()
        {
            _currentInputs = InputFrame.GetCurrent();
            if (!_isRecording)
            {
                return;
            }

            InputFrame inputs = InputFrame.GetCurrent();
            if (inputs.Equals(_currentRecordingSession.LastInputFrame)) {
                return;
            }

            _currentRecordingSession.AddFrame(inputs);
        }

        private void RecordToolStrip_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem recordToolStrip = (sender as ToolStripMenuItem);
            if (_isRecording)
            {
                _isRecording = false;
                recordToolStrip.Checked = false;

                string path;
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Windows Movie|*.mp4";

                    DialogResult result = saveFileDialog.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        _currentRecordingSession = null;
                        return;
                    }

                    path = saveFileDialog.FileName;
                }

                _currentRecordingSession.AddFrame(InputFrame.GetCurrent());
                _currentRecordingSession.Render(path, this, (i) => _currentInputs = i);
                _currentRecordingSession = null;
            }
            else
            {
                _currentRecordingSession = new RecordingSession(this.Size);
                _isRecording = true;
                recordToolStrip.Checked = true;
            }
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
                    return 0.0004f;
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
            
            InputFrame inputs = _currentInputs;
            if (inputs == null) return;
            
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
