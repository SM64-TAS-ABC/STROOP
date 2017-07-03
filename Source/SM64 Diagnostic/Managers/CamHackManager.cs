using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SM64_Diagnostic.Managers
{
    public class CamHackManager : DataManager
    {
        public enum CamHackMode { REGULAR, RELATIVE_ANGLE, ABSOLUTE_ANGLE, FIXED_POS, FIXED_ORIENTATION };

        public CamHackMode _currentCamHackMode;

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

            _currentCamHackMode = CamHackMode.REGULAR;
        }

        public override void Update(bool updateView)
        {
            base.Update();

            CamHackMode correctCamHackMode = getCorrectCamHackMode();
            if (_currentCamHackMode != correctCamHackMode)
            {
                _currentCamHackMode = correctCamHackMode;
                getCorrespondingRadioButton(correctCamHackMode).Checked = true;
            }
        }

        private CamHackMode getCorrectCamHackMode()
        {
            int cameraMode = _stream.GetInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            ushort absoluteAngle = _stream.GetUInt16(Config.CameraHack.CameraHackStruct + Config.CameraHack.AbsoluteAngleOffset);
            return cameraMode == 1 && absoluteAngle == 0 ? CamHackMode.RELATIVE_ANGLE :
                   cameraMode == 1 ? CamHackMode.ABSOLUTE_ANGLE :
                   cameraMode == 2 ? CamHackMode.FIXED_POS :
                   cameraMode == 3 ? CamHackMode.FIXED_ORIENTATION : CamHackMode.REGULAR;
        }

        private RadioButton getCorrespondingRadioButton(CamHackMode camHackMode)
        {
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                    return _mode0RadioButton;

                case CamHackMode.RELATIVE_ANGLE:
                    return _mode1RadioButtonRelativeAngle;

                case CamHackMode.ABSOLUTE_ANGLE:
                    return _mode1RadioButtonAbsoluteAngle;

                case CamHackMode.FIXED_POS:
                    return _mode2RadioButton;

                case CamHackMode.FIXED_ORIENTATION:
                    return _mode3RadioButton;

                default:
                    return null;
            }
        }
    }
}
