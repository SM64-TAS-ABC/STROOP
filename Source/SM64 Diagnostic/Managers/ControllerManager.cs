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
        ControllerImageGui _gui;
        ControllerDisplayPanel _controllerDisplayPanel;

        public ControllerManager(ProcessStream stream, List<WatchVariable> controllerData, Control controllerControl, NoTearFlowLayoutPanel variableTable, ControllerImageGui gui)
            : base(stream, controllerData, variableTable, Config.Mario.StructAddress)
        {
            _gui = gui;

            SplitContainer splitContainerController = controllerControl.Controls["splitContainerController"] as SplitContainer;
            ControllerDisplayPanel _controllerDisplayPanel = splitContainerController.Panel1.Controls["controllerDisplayPanel"] as ControllerDisplayPanel;

            _controllerDisplayPanel.setControllerDisplayGui(_gui);

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

        public override void Update(bool updateView)
        {
            base.Update();

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
