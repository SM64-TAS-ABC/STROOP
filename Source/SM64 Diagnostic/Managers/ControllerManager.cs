using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using System.Windows.Forms;
using SM64_Diagnostic.Utilities;
using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Extensions;
using SM64_Diagnostic.Structs.Configurations;
using SM64Diagnostic.Controls;

namespace SM64_Diagnostic.Managers
{
    public class ControllerManager : DataManager
    {
        public ControllerManager(ProcessStream stream, List<WatchVariable> controllerData, Control controllerControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, controllerData, variableTable, Config.Mario.StructAddress)
        {
            /*
            _mapManager = mapManager;

            var toggleHandsfree = marioControl.Controls["buttonMarioToggleHandsfree"] as Button;
            toggleHandsfree.Click += (sender, e) => MarioActions.ToggleHandsfree(_stream);

            var toggleVisibility = marioControl.Controls["buttonMarioVisibility"] as Button;
            toggleVisibility.Click += (sender, e) => MarioActions.ToggleVisibility(_stream);

            var marioPosGroupBox = marioControl.Controls["groupBoxMarioPos"] as GroupBox;
            PositionController.initialize(
                marioPosGroupBox.Controls["buttonMarioPosXn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXnZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXnZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXpZn"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosXpZp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosYp"] as Button,
                marioPosGroupBox.Controls["buttonMarioPosYn"] as Button,
                marioPosGroupBox.Controls["textBoxMarioPosXZ"] as TextBox,
                marioPosGroupBox.Controls["textBoxMarioPosY"] as TextBox,
                marioPosGroupBox.Controls["checkBoxMarioPosRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.MoveMario(
                        _stream,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });

            var marioStatsGroupBox = marioControl.Controls["groupBoxMarioStats"] as GroupBox;
            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsYawN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsYawP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsYaw"] as TextBox,
                (float yawValue) =>
                {
                    MarioActions.MarioChangeYaw(_stream, (int)yawValue);
                });
            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsHspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsHspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsHspd"] as TextBox,
                (float hspdValue) =>
                {
                    MarioActions.MarioChangeHspd(_stream, hspdValue);
                });

            ScalarController.initialize(
                marioStatsGroupBox.Controls["buttonMarioStatsVspdN"] as Button,
                marioStatsGroupBox.Controls["buttonMarioStatsVspdP"] as Button,
                marioStatsGroupBox.Controls["textBoxMarioStatsVspd"] as TextBox,
                (float vspdValue) =>
                {
                    MarioActions.MarioChangeVspd(_stream, vspdValue);
                });

            var marioHOLPGroupBox = marioControl.Controls["groupBoxMarioHOLP"] as GroupBox;
            PositionController.initialize(
                marioHOLPGroupBox.Controls["buttonMarioHOLPXn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXnZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXnZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXpZn"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPXpZp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPYp"] as Button,
                marioHOLPGroupBox.Controls["buttonMarioHOLPYn"] as Button,
                marioHOLPGroupBox.Controls["textBoxMarioHOLPXZ"] as TextBox,
                marioHOLPGroupBox.Controls["textBoxMarioHOLPY"] as TextBox,
                marioHOLPGroupBox.Controls["checkBoxMarioHOLPRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.MoveHOLP(
                        _stream,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });
                */
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("DeFactoSpeed"),
                new DataContainer("SlidingSpeed"),
                new AngleDataContainer("SlidingAngle"),
                new DataContainer("FallHeight"),
                new DataContainer("ActionDescription"),
                new DataContainer("PrevActionDescription"),
                new DataContainer("MovementX"),
                new DataContainer("MovementY"),
                new DataContainer("MovementZ"),
                new DataContainer("MovementLateral"),
                new DataContainer("Movment"),
                new DataContainer("QFrameCountEstimate")
            };
        }

        public void ProcessSpecialVars()
        {
            UInt32 floorTriangle = _stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.FloorTriangleOffset);
            var floorY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.GroundYOffset);

            float hSpeed = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.HSpeedOffset);

            float slidingSpeedX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedXOffset);
            float slidingSpeedZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.SlidingSpeedZOffset);

            float movementX = (_stream.GetSingle(Config.RngRecordingAreaAddress + 0x10)
                - _stream.GetSingle(Config.RngRecordingAreaAddress + 0x1C));
            float movementY = (_stream.GetSingle(Config.RngRecordingAreaAddress + 0x14)
                - _stream.GetSingle(Config.RngRecordingAreaAddress + 0x20));
            float movementZ = (_stream.GetSingle(Config.RngRecordingAreaAddress + 0x18)
                - _stream.GetSingle(Config.RngRecordingAreaAddress + 0x24));

            foreach (var specialVar in _specialWatchVars)
            {
                switch(specialVar.SpecialName)
                {
                    case "DeFactoSpeed":
                        if (floorTriangle != 0x00)
                        {
                            float normY = _stream.GetSingle(floorTriangle + Config.TriangleOffsets.NormY);
                            (specialVar as DataContainer).Text = Math.Round(hSpeed * normY, 3).ToString();
                        }
                        else
                        {
                            (specialVar as DataContainer).Text = "(No Floor)";
                        }
                        break;

                    case "SlidingSpeed":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(slidingSpeedX * slidingSpeedX + slidingSpeedZ * slidingSpeedZ), 3).ToString();
                        break;

                    case "SlidingAngle":
                        (specialVar as AngleDataContainer).AngleValue = Math.PI / 2 - Math.Atan2(slidingSpeedZ, slidingSpeedX);
                        (specialVar as AngleDataContainer).ValueExists = (slidingSpeedX != 0) || (slidingSpeedZ != 0);
                        break;

                    case "FallHeight":
                        (specialVar as DataContainer).Text = (_stream.GetSingle(Config.Mario.StructAddress + Config.Mario.PeakHeightOffset) - floorY).ToString();
                        break;

                    case "ActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.ActionOffset));
                        break;

                    case "PrevActionDescription":
                        (specialVar as DataContainer).Text = Config.MarioActions.GetActionName(_stream.GetUInt32(Config.Mario.StructAddress + Config.Mario.PrevActionOffset));
                        break;

                    case "MovementX":
                        (specialVar as DataContainer).Text = movementX.ToString();
                        break;

                    case "MovementY":
                        (specialVar as DataContainer).Text = movementY.ToString();
                        break;

                    case "MovementZ":
                        (specialVar as DataContainer).Text = movementZ.ToString();
                        break;

                    case "MovementLateral":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(movementX * movementX + movementZ * movementZ),3).ToString();
                        break;

                    case "Movement":
                        (specialVar as DataContainer).Text = Math.Round(Math.Sqrt(movementX * movementX + movementY * movementY + movementZ * movementZ), 3).ToString();
                        break;

                    case "QFrameCountEstimate":
                        var oldHSpeed = _stream.GetSingle(Config.RngRecordingAreaAddress + 0x28);
                        var qframes = Math.Abs(Math.Round(Math.Sqrt(movementX * movementX + movementZ * movementZ) / (oldHSpeed / 4)));
                        if (qframes > 4)
                            qframes = double.NaN;
                        (specialVar as DataContainer).Text = qframes.ToString();
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            base.Update();
            ProcessSpecialVars();

            uint inputStruct = Config.Controller.CurrentInput;

            bool buttonAPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonAOffset) & Config.Controller.ButtonAMask) != 0;
            bool buttonBPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonBOffset) & Config.Controller.ButtonBMask) != 0;
            bool buttonZPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonZOffset) & Config.Controller.ButtonZMask) != 0;
            bool buttonStartPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonStartOffset) & Config.Controller.ButtonStartMask) != 0;
            bool buttonRPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonROffset) & Config.Controller.ButtonRMask) != 0;
            bool buttonLPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonLOffset) & Config.Controller.ButtonLMask) != 0;
            bool buttonCUpPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCUpOffset) & Config.Controller.ButtonCUpMask) != 0;
            bool buttonCDownPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCDownOffset) & Config.Controller.ButtonCDownMask) != 0;
            bool buttonCLeftPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCLeftOffset) & Config.Controller.ButtonCLeftMask) != 0;
            bool buttonCRightPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonCRightOffset) & Config.Controller.ButtonCRightMask) != 0;
            bool buttonDUpPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDUpOffset) & Config.Controller.ButtonDUpMask) != 0;
            bool buttonDDownPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDDownOffset) & Config.Controller.ButtonDDownMask) != 0;
            bool buttonDLeftPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDLeftOffset) & Config.Controller.ButtonDLeftMask) != 0;
            bool buttonDRightPressed = (_stream.GetByte(inputStruct + Config.Controller.ButtonDRightOffset) & Config.Controller.ButtonDRightMask) != 0;
            sbyte controlStickH = (sbyte)_stream.GetByte(inputStruct + Config.Controller.ControlStickHOffset);
            sbyte controlStickV = (sbyte)_stream.GetByte(inputStruct + Config.Controller.ControlStickVOffset);

            /*
            counter++;
            if (counter % 25 == 0)
            {
                Console.WriteLine("");
                Console.WriteLine("[" + counter + "]");

                Console.WriteLine("buttonAPressed = " + buttonAPressed);
                Console.WriteLine("buttonBPressed = " + buttonBPressed);
                Console.WriteLine("buttonZPressed = " + buttonZPressed);
                Console.WriteLine("buttonStartPressed = " + buttonStartPressed);

                Console.WriteLine("buttonRPressed = " + buttonRPressed);
                Console.WriteLine("buttonLPressed = " + buttonLPressed);

                Console.WriteLine("buttonCUpPressed = " + buttonCUpPressed);
                Console.WriteLine("buttonCDownPressed = " + buttonCDownPressed);
                Console.WriteLine("buttonCLeftPressed = " + buttonCLeftPressed);
                Console.WriteLine("buttonCRightPressed = " + buttonCRightPressed);

                Console.WriteLine("buttonDUpPressed = " + buttonDUpPressed);
                Console.WriteLine("buttonDDownPressed = " + buttonDDownPressed);
                Console.WriteLine("buttonDLeftPressed = " + buttonDLeftPressed);
                Console.WriteLine("buttonDRightPressed = " + buttonDRightPressed);

                Console.WriteLine("controlStickH = " + controlStickH);
                Console.WriteLine("controlStickV = " + controlStickV);
            }
            */
        }
    }
}
