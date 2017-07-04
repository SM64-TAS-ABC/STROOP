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
            _controllerDisplayPanel = splitContainerController.Panel1.Controls["controllerDisplayPanel"] as ControllerDisplayPanel;

            _controllerDisplayPanel.setControllerDisplayGui(_gui);
            _controllerDisplayPanel.setProcessStream(_stream);
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

            _controllerDisplayPanel.Invalidate();
        }
    }
}
