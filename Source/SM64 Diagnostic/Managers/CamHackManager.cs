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
    public class CamHackManager : DataManager
    {
        RadioButton _mode0RadioButton;
        RadioButton _mode1RadioButtonRelativeAngle;
        RadioButton _mode1RadioButtonAbsoluteAngle;
        RadioButton _mode2RadioButton;
        RadioButton _mode3RadioButton;

        public CamHackManager(ProcessStream stream, List<WatchVariable> controllerData, TabPage camHackControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, controllerData, variableTable)
        {
            var splitContainer = camHackControl.Controls["splitContainerCamHack"] as SplitContainer;
            _mode0RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode0"] as RadioButton;
            _mode1RadioButtonRelativeAngle = splitContainer.Panel1.Controls["radioButtonCamHackMode1RelativeAngle"] as RadioButton;
            _mode1RadioButtonAbsoluteAngle = splitContainer.Panel1.Controls["radioButtonCamHackMode1AbsoluteAngle"] as RadioButton;
            _mode2RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode2"] as RadioButton;
            _mode3RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode3"] as RadioButton;

            _mode0RadioButton.Click += (sender, e) => _stream.SetValue(0, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            _mode1RadioButtonRelativeAngle.Click += (sender, e) =>
            {
                _stream.SetValue(1, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
                _stream.SetValue((ushort)0, Config.CameraHack.CameraHackStruct + Config.CameraHack.AbsoluteAngleOffset);
            };
            _mode1RadioButtonAbsoluteAngle.Click += (sender, e) =>
            {
                _stream.SetValue(1, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
                _stream.SetValue((ushort)1, Config.CameraHack.CameraHackStruct + Config.CameraHack.AbsoluteAngleOffset);
            };
            _mode2RadioButton.Click += (sender, e) => _stream.SetValue(2, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            _mode3RadioButton.Click += (sender, e) => _stream.SetValue(3, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
        }

        public override void Update(bool updateView)
        {
            base.Update();

            int cameraMode = _stream.GetInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            ushort absoluteAngle = _stream.GetUInt16(Config.CameraHack.CameraHackStruct + Config.CameraHack.AbsoluteAngleOffset);
            RadioButton correspondingRadioButton =
                cameraMode == 1 && absoluteAngle == 0 ? _mode1RadioButtonRelativeAngle :
                cameraMode == 1 ? _mode1RadioButtonAbsoluteAngle :
                cameraMode == 2 ? _mode2RadioButton :
                cameraMode == 3 ? _mode3RadioButton : _mode0RadioButton;
            if (!correspondingRadioButton.Checked) correspondingRadioButton.Checked = true;
            

        }
    }
}
