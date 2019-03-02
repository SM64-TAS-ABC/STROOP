using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using STROOP.Structs;
using System.ComponentModel;
using STROOP.Utilities;
using System.Windows.Forms;
using STROOP.Forms;

namespace STROOP.M64
{
    public class M64Stats
    {
        private readonly M64File _m64;
        private byte[] _rawBytes { get => _m64.RawBytes; }
        private M64Header _header { get => _m64.Header; }
        private BindingList<M64InputFrame> _inputs { get => _m64.Inputs; }

        [Category("\u200B\u200B\u200BMain Button Presses"), DisplayName("\u200B\u200B\u200B\u200BNum A Presses")]
        public int NumAPresses
        {
            get { return FindPresses(input => input.A).Count; }
            set { SetNumPreses(value, input => input.A = false); }
        }

        [Category("\u200B\u200B\u200BMain Button Presses"), DisplayName("\u200B\u200B\u200BNum B Presses")]
        public int NumBPresses
        {
            get { return FindPresses(input => input.B).Count; }
            set { SetNumPreses(value, input => input.B = false); }
        }

        [Category("\u200B\u200B\u200BMain Button Presses"), DisplayName("\u200B\u200BNum Z Presses")]
        public int NumZPresses
        {
            get { return FindPresses(input => input.Z).Count; }
            set { SetNumPreses(value, input => input.Z = false); }
        }

        [Category("\u200B\u200B\u200BMain Button Presses"), DisplayName("\u200BNum S Presses")]
        public int NumSPresses
        {
            get { return FindPresses(input => input.S).Count; }
            set { SetNumPreses(value, input => input.S = false); }
        }

        [Category("\u200B\u200B\u200BMain Button Presses"), DisplayName("Num R Presses")]
        public int NumRPresses
        {
            get { return FindPresses(input => input.R).Count; }
            set { SetNumPreses(value, input => input.R = false); }
        }

        [Category("\u200B\u200BC Button Presses"), DisplayName("\u200B\u200B\u200BNum C^ Presses")]
        public int NumCUpPresses
        {
            get { return FindPresses(input => input.C_Up).Count; }
            set { SetNumPreses(value, input => input.C_Up = false); }
        }

        [Category("\u200B\u200BC Button Presses"), DisplayName("\u200B\u200BNum Cv Presses")]
        public int NumCDownPresses
        {
            get { return FindPresses(input => input.C_Down).Count; }
            set { SetNumPreses(value, input => input.C_Down = false); }
        }

        [Category("\u200B\u200BC Button Presses"), DisplayName("\u200BNum C< Presses")]
        public int NumCLeftPresses
        {
            get { return FindPresses(input => input.C_Left).Count; }
            set { SetNumPreses(value, input => input.C_Left = false); }
        }

        [Category("\u200B\u200BC Button Presses"), DisplayName("Num C> Presses")]
        public int NumCRightPresses
        {
            get { return FindPresses(input => input.C_Right).Count; }
            set { SetNumPreses(value, input => input.C_Right = false); }
        }

        [Category("\u200BNoop Button Presses"), DisplayName("\u200B\u200B\u200B\u200BNum L Presses")]
        public int NumLPresses
        {
            get { return FindPresses(input => input.L).Count; }
            set { SetNumPreses(value, input => input.L = false); }
        }

        [Category("\u200BNoop Button Presses"), DisplayName("\u200B\u200B\u200BNum D^ Presses")]
        public int NumDUpPresses
        {
            get { return FindPresses(input => input.D_Up).Count; }
            set { SetNumPreses(value, input => input.D_Up = false); }
        }

        [Category("\u200BNoop Button Presses"), DisplayName("\u200B\u200BNum Dv Presses")]
        public int NumDDownPresses
        {
            get { return FindPresses(input => input.D_Down).Count; }
            set { SetNumPreses(value, input => input.D_Down = false); }
        }

        [Category("\u200BNoop Button Presses"), DisplayName("\u200BNum D< Presses")]
        public int NumDLeftPresses
        {
            get { return FindPresses(input => input.D_Left).Count; }
            set { SetNumPreses(value, input => input.D_Left = false); }
        }

        [Category("\u200BNoop Button Presses"), DisplayName("Num D> Presses")]
        public int NumDRightPresses
        {
            get { return FindPresses(input => input.D_Right).Count; }
            set { SetNumPreses(value, input => input.D_Right = false); }
        }

        [Category("Misc"), DisplayName("\u200B\u200B\u200BLag VIs")]
        public int LagVis
        {
            get { return _header.NumVis - 2 * _header.NumInputs; }
            set { }
        }

        [Category("Misc"), DisplayName("\u200B\u200BNum Unused Inputs")]
        public int NumUnusedInputs
        {
            get
            {
                if (_rawBytes == null) return 0;
                int rawInputCount = (_rawBytes.Length - M64Config.HeaderSize) / 4;
                int headerInputCount = _header.NumInputs;
                return rawInputCount - headerInputCount;
            }
            set { }
        }

        [Category("Misc"), DisplayName("\u200BNum Joystick Frames")]
        public int NumJoystickFrames
        {
            get { return FindJoystickFrames().Count; }
            set { }
        }

        [Category("Misc"), DisplayName("Num Input Changes")]
        public int NumInputChanges
        {
            get { return Math.Max(FindInputChanges().Count - 1, 0); }
            set { }
        }

        public M64Stats(M64File m64)
        {
            _m64 = m64;
        }

        private List<(int, int)> FindPresses(Func<M64InputFrame, bool> isPressedFunction)
        {
            List<(int, int)> pressList = new List<(int, int)>();
            int startFrame = 0;
            bool isAlreadyPressed = false;

            for (int i = 0; i <= _inputs.Count; i++)
            {
                bool isCurrentlyPressed = i == _inputs.Count ?
                    false : isPressedFunction(_inputs[i]);
                if (isAlreadyPressed)
                {
                    if (isCurrentlyPressed) // still pressing
                    {
                        // do nothing
                    }
                    else // just stopped pressing
                    {
                        int endFrame = i - 1;
                        pressList.Add((startFrame, endFrame));
                        isAlreadyPressed = false;
                    }
                }
                else
                {
                    if (isCurrentlyPressed) // just started pressing
                    {
                        startFrame = i;
                        isAlreadyPressed = true;
                    }
                    else // still not pressing
                    {
                        // do nothing
                    }
                }
            }

            return pressList;
        }

        private void SetNumPreses(int numPresses, Action<M64InputFrame> unpressFunction)
        {
            if (numPresses != 0) return;
            for (int i = 0; i < _inputs.Count; i++)
            {
                unpressFunction(_inputs[i]);
            }
        }

        private List<int> FindJoystickFrames()
        {
            List<int> joystickFrames = new List<int>();
            for (int i = 0; i < _inputs.Count; i++)
            {
                M64InputFrame frame = _inputs[i];
                bool isJoystickFrame = frame.X != 0 || frame.Y != 0;
                if (isJoystickFrame) joystickFrames.Add(i);
            }
            return joystickFrames;
        }

        private List<(int, string)> FindInputChanges()
        {
            List<(int, string)> inputChanges = new List<(int, string)>();
            string lastInputsString = null;
            for (int i = 0; i < _inputs.Count; i++)
            {
                string inputsString = _inputs[i].GetInputsString();
                if (!Equals(inputsString, lastInputsString))
                {
                    inputChanges.Add((i, inputsString));
                    lastInputsString = inputsString;
                }
            }
            return inputChanges;
        }

        public ContextMenuStrip CreateContextMenuStrip()
        {
            List<ToolStripMenuItem> items = M64Utilities.ButtonNameList.ConvertAll(
                buttonName => new ToolStripMenuItem(
                    String.Format("Show All {0} Presses", buttonName)));

            if (items.Count != M64Utilities.IsButtonPressedFunctionList.Count)
                throw new ArgumentOutOfRangeException();

            for (int i = 0; i < items.Count; i++)
            {
                int index = i;
                items[index].Click += (sender, e) =>
                {
                    string buttonName = M64Utilities.ButtonNameList[index];
                    Func<M64InputFrame, bool> isPressedFunction = M64Utilities.IsButtonPressedFunctionList[index];
                    List<(int, int)> buttonPresses = FindPresses(isPressedFunction);
                    string buttonPressesString = FormatButtonPressesString(buttonPresses, buttonName);
                    InfoForm.ShowValue(
                        buttonPressesString,
                        "Num Button Presses",
                        String.Format("Num {0} Presses", buttonName));
                };
            }

            ToolStripMenuItem itemShowAllJoystickFrames = new ToolStripMenuItem("Show All Joystick Frames");
            itemShowAllJoystickFrames.Click += (sender, e) =>
            {
                InfoForm.ShowValue(
                    FormatJoystickFramesString(FindJoystickFrames()),
                    "Joystick Frames",
                    "Joystick Frames");
            };
            items.Add(itemShowAllJoystickFrames);

            ToolStripMenuItem itemShowAllInputChanges = new ToolStripMenuItem("Show All Input Changes");
            itemShowAllInputChanges.Click += (sender, e) =>
            {
                InfoForm.ShowValue(
                    FormatInputChangesString(FindInputChanges()),
                    "Input Changes",
                    "Input Changes");
            };
            items.Add(itemShowAllInputChanges);

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            items.ForEach(item => contextMenuStrip.Items.Add(item));
            return contextMenuStrip;
        }

        private string FormatButtonPressesString(List<(int, int)> buttonPresses, string buttonName)
        {
            List<string> lines = new List<string>();
            lines.Add(String.Format(
                "{0} {1} press{2} total:",
                buttonPresses.Count, buttonName, buttonPresses.Count != 1 ? "es" : ""));
            for (int i = 0; i < buttonPresses.Count; i++)
            {
                (int startFrame, int endFrame) = buttonPresses[i];
                int frameSpan = endFrame - startFrame + 1;
                string pluralitySuffix = frameSpan != 1 ? "s" : "";
                lines.Add(String.Format(
                    "{0} press #{1}: frame {2} to frame {3} ({4} frame{5})",
                    buttonName, i + 1, startFrame, endFrame, frameSpan, pluralitySuffix));
            }
            return String.Join("\r\n", lines);
        }

        private string FormatJoystickFramesString(List<int> joystickFrames)
        {
            List<string> lines = new List<string>();
            lines.Add(String.Format(
                "{0} joystick frame{1} total:",
                joystickFrames.Count, joystickFrames.Count != 1 ? "s" : ""));
            for (int i = 0; i < joystickFrames.Count; i++)
            {
                lines.Add(String.Format(
                    "Joystick frame #{0} on frame {1}",
                    i + 1, joystickFrames[i]));
            }
            return String.Join("\r\n", lines);
        }

        private string FormatInputChangesString(List<(int, string)> inputChanges)
        {
            List<string> lines = new List<string>();
            lines.Add(String.Format(
                "{0} input change{1} total:",
                inputChanges.Count - 1, inputChanges.Count - 1 != 1 ? "s" : ""));
            for (int i = 0; i < inputChanges.Count - 1; i++)
            {
                (int frame1, string inputsString1) = inputChanges[i];
                (int frame2, string inputsString2) = inputChanges[i+1];
                lines.Add(String.Format(
                    "Input change #{0} on frame {1}: from {2} to {3}",
                    i + 1, frame2, inputsString1, inputsString2));
            }
            return String.Join("\r\n", lines);
        }
    }
}
