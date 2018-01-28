using SM64_Diagnostic.Controls;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Structs.Configurations;
using SM64_Diagnostic.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SM64_Diagnostic.Managers
{
    public class CamHackManager : DataManager
    {
        public CamHackMode _currentCamHackMode;

        RadioButton _mode0RadioButton;
        RadioButton _mode1RadioButtonRelativeAngle;
        RadioButton _mode1RadioButtonAbsoluteAngle;
        RadioButton _mode2RadioButton;
        RadioButton _mode3RadioButton;

        public CamHackManager(List<WatchVariableControlPrecursor> variables, TabPage camHackControl, WatchVariablePanel variableTable)
            : base(variables, variableTable)
        {
            _currentCamHackMode = CamHackMode.REGULAR;

            var splitContainer = camHackControl.Controls["splitContainerCamHack"] as SplitContainer;
            _mode0RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode0"] as RadioButton;
            _mode1RadioButtonRelativeAngle = splitContainer.Panel1.Controls["radioButtonCamHackMode1RelativeAngle"] as RadioButton;
            _mode1RadioButtonAbsoluteAngle = splitContainer.Panel1.Controls["radioButtonCamHackMode1AbsoluteAngle"] as RadioButton;
            _mode2RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode2"] as RadioButton;
            _mode3RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode3"] as RadioButton;

            _mode0RadioButton.Click += (sender, e) => Config.Stream.SetValue(0, CameraHackConfig.CameraHackStruct + CameraHackConfig.CameraModeOffset);
            _mode1RadioButtonRelativeAngle.Click += (sender, e) =>
            {
                Config.Stream.SetValue(1, CameraHackConfig.CameraHackStruct + CameraHackConfig.CameraModeOffset);
                Config.Stream.SetValue((ushort)0, CameraHackConfig.CameraHackStruct + CameraHackConfig.AbsoluteAngleOffset);
            };
            _mode1RadioButtonAbsoluteAngle.Click += (sender, e) =>
            {
                Config.Stream.SetValue(1, CameraHackConfig.CameraHackStruct + CameraHackConfig.CameraModeOffset);
                Config.Stream.SetValue((ushort)1, CameraHackConfig.CameraHackStruct + CameraHackConfig.AbsoluteAngleOffset);
            };
            _mode2RadioButton.Click += (sender, e) => Config.Stream.SetValue(2, CameraHackConfig.CameraHackStruct + CameraHackConfig.CameraModeOffset);
            _mode3RadioButton.Click += (sender, e) => Config.Stream.SetValue(3, CameraHackConfig.CameraHackStruct + CameraHackConfig.CameraModeOffset);

            var cameraHackPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                cameraHackPosGroupBox,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosXn"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosXp"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosZn"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosZp"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosXnZn"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosXnZp"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosXpZn"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosXpZp"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosYp"] as Button,
                cameraHackPosGroupBox.Controls["buttonCameraHackPosYn"] as Button,
                cameraHackPosGroupBox.Controls["textBoxCameraHackPosXZ"] as TextBox,
                cameraHackPosGroupBox.Controls["textBoxCameraHackPosY"] as TextBox,
                cameraHackPosGroupBox.Controls["checkBoxCameraHackPosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateCameraHack(
                        _currentCamHackMode,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var cameraHackSphericalPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackSphericalPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                cameraHackSphericalPosGroupBox,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosTn"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosTp"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosPn"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosPp"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosTnPn"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosTnPp"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosTpPn"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosTpPp"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosRn"] as Button,
                cameraHackSphericalPosGroupBox.Controls["buttonCameraHackSphericalPosRp"] as Button,
                cameraHackSphericalPosGroupBox.Controls["textBoxCameraHackSphericalPosTP"] as TextBox,
                cameraHackSphericalPosGroupBox.Controls["textBoxCameraHackSphericalPosR"] as TextBox,
                null /* checkbox */,
                (float hOffset, float vOffset, float nOffset, bool _) =>
                {
                    ButtonUtilities.TranslateCameraHackSpherically(
                        _currentCamHackMode,
                        -1 * nOffset,
                        hOffset,
                        -1 * vOffset);
                });

            var cameraHackFocusPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackFocusPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                cameraHackFocusPosGroupBox,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosXn"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosXp"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosZn"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosZp"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosXnZn"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosXnZp"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosXpZn"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosXpZp"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosYp"] as Button,
                cameraHackFocusPosGroupBox.Controls["buttonCameraHackFocusPosYn"] as Button,
                cameraHackFocusPosGroupBox.Controls["textBoxCameraHackFocusPosXZ"] as TextBox,
                cameraHackFocusPosGroupBox.Controls["textBoxCameraHackFocusPosY"] as TextBox,
                cameraHackFocusPosGroupBox.Controls["checkBoxCameraHackFocusPosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateCameraHackFocus(
                        _currentCamHackMode,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var cameraHackSphericalFocusPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackSphericalFocusPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                cameraHackSphericalFocusPosGroupBox,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosTn"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosTp"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosPn"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosPp"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosTnPn"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosTnPp"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosTpPn"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosTpPp"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosRp"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["buttonCameraHackSphericalFocusPosRn"] as Button,
                cameraHackSphericalFocusPosGroupBox.Controls["textBoxCameraHackSphericalFocusPosTP"] as TextBox,
                cameraHackSphericalFocusPosGroupBox.Controls["textBoxCameraHackSphericalFocusPosR"] as TextBox,
                null /* checkbox */,
                (float hOffset, float vOffset, float nOffset, bool _) =>
                {
                    ButtonUtilities.TranslateCameraHackFocusSpherically(
                        _currentCamHackMode,
                        nOffset,
                        hOffset,
                        -1 * vOffset);
                });

            var cameraHackBothPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackBothPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                cameraHackBothPosGroupBox,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosXn"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosXp"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosZn"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosZp"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosXnZn"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosXnZp"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosXpZn"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosXpZp"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosYp"] as Button,
                cameraHackBothPosGroupBox.Controls["buttonCameraHackBothPosYn"] as Button,
                cameraHackBothPosGroupBox.Controls["textBoxCameraHackBothPosXZ"] as TextBox,
                cameraHackBothPosGroupBox.Controls["textBoxCameraHackBothPosY"] as TextBox,
                cameraHackBothPosGroupBox.Controls["checkBoxCameraHackBothPosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateCameraHackBoth(
                        _currentCamHackMode,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            CamHackMode correctCamHackMode = getCorrectCamHackMode();
            if (_currentCamHackMode != correctCamHackMode)
            {
                _currentCamHackMode = correctCamHackMode;
                getCorrespondingRadioButton(correctCamHackMode).Checked = true;
            }

            //DoTestingCalculations();
        }

        private int _globalTimer = 0;

        private void DoTestingCalculations()
        {
            uint objAddress = Config.Stream.GetUInt32(CameraHackConfig.CameraHackStruct + CameraHackConfig.ObjectOffset);
            if (objAddress == 0) return;

            int currentGlobalTimer = Config.Stream.GetInt32(MiscConfig.GlobalTimerAddress);
            if (currentGlobalTimer == _globalTimer) return;
            _globalTimer = currentGlobalTimer;

            uint swooperTargetOffset = 0xFE;
            ushort swooperTargetAngle = Config.Stream.GetUInt16(objAddress + swooperTargetOffset);
            ushort cameraAngle = Config.Stream.GetUInt16(CameraHackConfig.CameraHackStruct + CameraHackConfig.ThetaOffset);

            double angleCap = 1024;
            ushort newCameraAngle = MoreMath.NormalizeAngleUshort(MoreMath.RotateAngleTowards(cameraAngle, swooperTargetAngle, angleCap));
            Config.Stream.SetValue(newCameraAngle, CameraHackConfig.CameraHackStruct + CameraHackConfig.ThetaOffset);

            //Console.WriteLine(currentGlobalTimer.ToString() + ": " + swooperTargetAngle.ToString());
        }

        private CamHackMode getCorrectCamHackMode()
        {
            int cameraMode = Config.Stream.GetInt32(CameraHackConfig.CameraHackStruct + CameraHackConfig.CameraModeOffset);
            ushort absoluteAngle = Config.Stream.GetUInt16(CameraHackConfig.CameraHackStruct + CameraHackConfig.AbsoluteAngleOffset);
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
