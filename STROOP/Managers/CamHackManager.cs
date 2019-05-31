using STROOP.Controls;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Managers
{
    public class CamHackManager : DataManager
    {
        public CamHackMode CurrentCamHackMode { get; private set; }

        RadioButton _mode0RadioButton;
        RadioButton _mode1RadioButtonRelativeAngle;
        RadioButton _mode1RadioButtonAbsoluteAngle;
        RadioButton _mode2RadioButton;
        RadioButton _mode3RadioButton;

        private int _numPans = 0;
        private List<List<WatchVariableControl>> _panVars = new List<List<WatchVariableControl>>();

        public CamHackManager(string varFilePath, TabPage camHackControl, WatchVariableFlowLayoutPanel variableTable)
            : base(varFilePath, variableTable)
        {
            CurrentCamHackMode = CamHackMode.REGULAR;

            var splitContainer = camHackControl.Controls["splitContainerCamHack"] as SplitContainer;

            Label labelCamHackMode = splitContainer.Panel1.Controls["labelCamHackMode"] as Label;
            ControlUtilities.AddContextMenuStripFunctions(
                labelCamHackMode,
                new List<string>() { "Download Camera Hack ROM" },
                new List<Action>()
                {
                    () => System.Diagnostics.Process.Start("http://download1436.mediafire.com/t3unklq170ag/hdd377v5794u319/Camera+Hack+ROM.z64"),
                });

        _mode0RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode0"] as RadioButton;
            _mode1RadioButtonRelativeAngle = splitContainer.Panel1.Controls["radioButtonCamHackMode1RelativeAngle"] as RadioButton;
            _mode1RadioButtonAbsoluteAngle = splitContainer.Panel1.Controls["radioButtonCamHackMode1AbsoluteAngle"] as RadioButton;
            _mode2RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode2"] as RadioButton;
            _mode3RadioButton = splitContainer.Panel1.Controls["radioButtonCamHackMode3"] as RadioButton;

            _mode0RadioButton.Click += (sender, e) => Config.Stream.SetValue(0, CamHackConfig.StructAddress + CamHackConfig.CameraModeOffset);
            _mode1RadioButtonRelativeAngle.Click += (sender, e) =>
            {
                Config.Stream.SetValue(1, CamHackConfig.StructAddress + CamHackConfig.CameraModeOffset);
                Config.Stream.SetValue((ushort)0, CamHackConfig.StructAddress + CamHackConfig.AbsoluteAngleOffset);
            };
            _mode1RadioButtonAbsoluteAngle.Click += (sender, e) =>
            {
                Config.Stream.SetValue(1, CamHackConfig.StructAddress + CamHackConfig.CameraModeOffset);
                Config.Stream.SetValue((ushort)1, CamHackConfig.StructAddress + CamHackConfig.AbsoluteAngleOffset);
            };
            _mode2RadioButton.Click += (sender, e) => Config.Stream.SetValue(2, CamHackConfig.StructAddress + CamHackConfig.CameraModeOffset);
            _mode3RadioButton.Click += (sender, e) => Config.Stream.SetValue(3, CamHackConfig.StructAddress + CamHackConfig.CameraModeOffset);

            var cameraHackPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
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
                        CurrentCamHackMode,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var cameraHackSphericalPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackSphericalPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                false,
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
                        CurrentCamHackMode,
                        -1 * nOffset,
                        hOffset,
                        -1 * vOffset);
                });

            var cameraHackFocusPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackFocusPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
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
                        CurrentCamHackMode,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var cameraHackSphericalFocusPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackSphericalFocusPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                false,
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
                        CurrentCamHackMode,
                        nOffset,
                        hOffset,
                        -1 * vOffset);
                });

            var cameraHackBothPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraHackBothPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
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
                        CurrentCamHackMode,
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });
        }

        public override void Update(bool updateView)
        {
            UpdatePanning();

            if (!updateView) return;
            base.Update(updateView);

            CamHackMode correctCamHackMode = getCorrectCamHackMode();
            if (CurrentCamHackMode != correctCamHackMode)
            {
                CurrentCamHackMode = correctCamHackMode;
                getCorrespondingRadioButton(correctCamHackMode).Checked = true;
            }
        }

        public void NotifyNumPanChange(int numPans)
        {
            if (numPans > _numPans) // Need to add vars
            {
                for (int i = _numPans; i < numPans; i++)
                {
                    SpecialConfig.PanModels.Add(new PanModel());
                    List<WatchVariableControl> panVars = CreatePanVars(i);
                    _panVars.Add(panVars);
                    _variablePanel.AddVariables(panVars);
                }
            }
            if (numPans < _numPans) // Need to remove vars
            {
                for (int i = _numPans - 1; i >= numPans; i--)
                {
                    SpecialConfig.PanModels.RemoveAt(i);
                    List<WatchVariableControl> panVars = _panVars[i];
                    _panVars.Remove(panVars);
                    _variablePanel.RemoveVariables(panVars);
                }
            }
            _numPans = numPans;
        }

        private WatchVariableControl CreatePanVar(
            string name,
            string specialType,
            string color,
            string subclass = null,
            string coord = null,
            string display = null,
            string yaw = null)
        {
            XElement xElement = new XElement("Data", name);
            xElement.Add(new XAttribute("base", "None"));
            xElement.Add(new XAttribute("specialType", specialType));
            xElement.Add(new XAttribute("color", color));
            if (subclass != null) xElement.Add(new XAttribute("subclass", subclass));
            if (coord != null) xElement.Add(new XAttribute("coord", coord));
            if (display != null) xElement.Add(new XAttribute("display", display));
            if (yaw != null) xElement.Add(new XAttribute("yaw", yaw));
            WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(xElement);
            return precursor.CreateWatchVariableControl();
        }

        private List<WatchVariableControl> CreatePanVars(int index)
        {
            WatchVariableSpecialUtilities.AddPanEntriesToDictionary(index);
            return new List<WatchVariableControl>
            {
                CreatePanVar("Global Timer", String.Format("Pan{0}GlobalTimer", index), "Orange"),
                CreatePanVar(String.Format("Pan{0} Start Time", index), String.Format("Pan{0}StartTime", index), "Orange"),
                CreatePanVar(String.Format("Pan{0} End Time", index), String.Format("Pan{0}EndTime", index), "Orange"),
                CreatePanVar(String.Format("Pan{0} Duration", index), String.Format("Pan{0}Duration", index), "Orange"),

                CreatePanVar(String.Format("Pan{0} Ease Start", index), String.Format("Pan{0}EaseStart", index), "LightBlue", subclass: "Boolean"),
                CreatePanVar(String.Format("Pan{0} Ease End", index), String.Format("Pan{0}EaseEnd", index), "LightBlue", subclass: "Boolean"),
                CreatePanVar(String.Format("Pan{0} Ease Degree", index), String.Format("Pan{0}EaseDegree", index), "LightBlue"),

                CreatePanVar(String.Format("Pan{0} Rotate CW", index), String.Format("Pan{0}RotateCW", index), "Yellow", subclass: "Boolean"),

                CreatePanVar(String.Format("Pan{0} Cam Start X", index), String.Format("Pan{0}CamStartX", index), "Green", coord: "X"),
                CreatePanVar(String.Format("Pan{0} Cam Start Y", index), String.Format("Pan{0}CamStartY", index), "Green", coord: "Y"),
                CreatePanVar(String.Format("Pan{0} Cam Start Z", index), String.Format("Pan{0}CamStartZ", index), "Green", coord: "Z"),
                CreatePanVar(String.Format("Pan{0} Cam Start Yaw", index), String.Format("Pan{0}CamStartYaw", index), "Green", subclass: "Angle", display: "ushort", yaw: "true"),
                CreatePanVar(String.Format("Pan{0} Cam Start Pitch", index), String.Format("Pan{0}CamStartPitch", index), "Green", subclass: "Angle", display: "short"),

                CreatePanVar(String.Format("Pan{0} Cam End X", index), String.Format("Pan{0}CamEndX", index), "Red", coord: "X"),
                CreatePanVar(String.Format("Pan{0} Cam End Y", index), String.Format("Pan{0}CamEndY", index), "Red", coord: "Y"),
                CreatePanVar(String.Format("Pan{0} Cam End Z", index), String.Format("Pan{0}CamEndZ", index), "Red", coord: "Z"),
                CreatePanVar(String.Format("Pan{0} Cam End Yaw", index), String.Format("Pan{0}CamEndYaw", index), "Red", subclass: "Angle", display: "ushort", yaw: "true"),
                CreatePanVar(String.Format("Pan{0} Cam End Pitch", index), String.Format("Pan{0}CamEndPitch", index), "Red", subclass: "Angle", display: "short"),
            };
        }

        public void UpdatePanning()
        {
            // Short circuit the logic if panning is disabled
            if (SpecialConfig.PanCamPos == 0 && SpecialConfig.PanCamAngle == 0) return;

            bool streamAlreadySuspended = Config.Stream.IsSuspended;
            if (!streamAlreadySuspended) Config.Stream.Suspend();

            int panIndex = (int)SpecialConfig.CurrentPan;
            if (panIndex == -1) return;
            PanModel panModel = SpecialConfig.PanModels[panIndex];

            uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
            double camX = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
            double camY = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
            double camZ = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);

            if (SpecialConfig.PanCamPos != 0)
            {
                if (globalTimer <= panModel.PanStartTime)
                {
                    Config.Stream.SetValue((float)panModel.PanCamStartX, CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    Config.Stream.SetValue((float)panModel.PanCamStartY, CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    Config.Stream.SetValue((float)panModel.PanCamStartZ, CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                }
                else if (globalTimer >= panModel.PanEndTime)
                {
                    Config.Stream.SetValue((float)panModel.PanCamEndX, CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    Config.Stream.SetValue((float)panModel.PanCamEndY, CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    Config.Stream.SetValue((float)panModel.PanCamEndZ, CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                }
                else
                {
                    double proportion = (globalTimer - panModel.PanStartTime) / (panModel.PanEndTime - panModel.PanStartTime);
                    proportion = EasingUtilities.Ease(panModel.PanEaseDegree, proportion, panModel.PanEaseStart != 0, panModel.PanEaseEnd != 0);
                    camX = panModel.PanCamStartX + proportion * (panModel.PanCamEndX - panModel.PanCamStartX);
                    camY = panModel.PanCamStartY + proportion * (panModel.PanCamEndY - panModel.PanCamStartY);
                    camZ = panModel.PanCamStartZ + proportion * (panModel.PanCamEndZ - panModel.PanCamStartZ);
                    Config.Stream.SetValue((float)camX, CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    Config.Stream.SetValue((float)camY, CamHackConfig.StructAddress + CamHackConfig.CameraYOffset);
                    Config.Stream.SetValue((float)camZ, CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                }
            }

            if (SpecialConfig.PanCamAngle != 0)
            {
                double camYaw;
                double camPitch;

                if (globalTimer <= panModel.PanStartTime)
                {
                    camYaw = panModel.PanCamStartYaw;
                    camPitch = panModel.PanCamStartPitch;
                }
                else if (globalTimer >= panModel.PanEndTime)
                {
                    camYaw = panModel.PanCamEndYaw;
                    camPitch = panModel.PanCamEndPitch;
                }
                else
                {
                    double proportion = (globalTimer - panModel.PanStartTime) / (panModel.PanEndTime - panModel.PanStartTime);
                    proportion = EasingUtilities.Ease(panModel.PanEaseDegree, proportion, panModel.PanEaseStart != 0, panModel.PanEaseEnd != 0);

                    double yawDist = MoreMath.GetUnsignedAngleDifference(panModel.PanCamStartYaw, panModel.PanCamEndYaw);
                    if (panModel.PanRotateCW != 0 && yawDist != 0) yawDist -= 65536;
                    camYaw = panModel.PanCamStartYaw + proportion * yawDist;
                    camYaw = MoreMath.NormalizeAngleDouble(camYaw);

                    double pitchDist = panModel.PanCamEndPitch - panModel.PanCamStartPitch;
                    camPitch = panModel.PanCamStartPitch + proportion * pitchDist;
                }

                (double diffX, double diffY, double diffZ) = MoreMath.SphericalToEuler_AngleUnits(1000, camYaw, camPitch);
                (double focusX, double focusY, double focusZ) = (camX + diffX, camY + diffY, camZ + diffZ);
                Config.Stream.SetValue((float)focusX, CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                Config.Stream.SetValue((float)focusY, CamHackConfig.StructAddress + CamHackConfig.FocusYOffset);
                Config.Stream.SetValue((float)focusZ, CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
            }

            if (!streamAlreadySuspended) Config.Stream.Resume();
        }

        private int _globalTimer = 0;

        private void DoTestingCalculations()
        {
            uint objAddress = Config.Stream.GetUInt32(CamHackConfig.StructAddress + CamHackConfig.ObjectOffset);
            if (objAddress == 0) return;

            int currentGlobalTimer = Config.Stream.GetInt32(MiscConfig.GlobalTimerAddress);
            if (currentGlobalTimer == _globalTimer) return;
            _globalTimer = currentGlobalTimer;

            uint swooperTargetOffset = 0xFE;
            ushort swooperTargetAngle = Config.Stream.GetUInt16(objAddress + swooperTargetOffset);
            ushort cameraAngle = Config.Stream.GetUInt16(CamHackConfig.StructAddress + CamHackConfig.ThetaOffset);

            double angleCap = 1024;
            ushort newCameraAngle = MoreMath.NormalizeAngleUshort(MoreMath.RotateAngleTowards(cameraAngle, swooperTargetAngle, angleCap));
            Config.Stream.SetValue(newCameraAngle, CamHackConfig.StructAddress + CamHackConfig.ThetaOffset);

            //Console.WriteLine(currentGlobalTimer.ToString() + ": " + swooperTargetAngle.ToString());
        }

        private CamHackMode getCorrectCamHackMode()
        {
            int cameraMode = Config.Stream.GetInt32(CamHackConfig.StructAddress + CamHackConfig.CameraModeOffset);
            ushort absoluteAngle = Config.Stream.GetUInt16(CamHackConfig.StructAddress + CamHackConfig.AbsoluteAngleOffset);
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
