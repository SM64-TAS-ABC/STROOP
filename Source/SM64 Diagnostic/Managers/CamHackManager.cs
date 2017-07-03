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
        RadioButton _mode1RadioButton;
        RadioButton _mode2RadioButton;
        RadioButton _mode3RadioButton;

        public CamHackManager(ProcessStream stream, List<WatchVariable> controllerData, TabPage camHackControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, controllerData, variableTable)
        {
            var splitContainer = camHackControl.Controls["splitContainerCamHack"] as SplitContainer;
            _mode0RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode0"] as RadioButton;
            _mode1RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode1"] as RadioButton;
            _mode2RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode2"] as RadioButton;
            _mode3RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode3"] as RadioButton;

            _mode0RadioButton.Click += (sender, e) => _stream.SetValue(0, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            _mode1RadioButton.Click += (sender, e) => _stream.SetValue(1, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            _mode2RadioButton.Click += (sender, e) => _stream.SetValue(2, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            _mode3RadioButton.Click += (sender, e) => _stream.SetValue(3, Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
        }

        public override void Update(bool updateView)
        {
            base.Update();

            int cameraMode = _stream.GetInt32(Config.CameraHack.CameraHackStruct + Config.CameraHack.CameraModeOffset);
            RadioButton correspondingRadioButton =
                cameraMode == 1 ? _mode1RadioButton :
                cameraMode == 2 ? _mode2RadioButton :
                cameraMode == 3 ? _mode3RadioButton : _mode0RadioButton;
            if (!correspondingRadioButton.Checked) correspondingRadioButton.Checked = true;
            

        }
    }
}
