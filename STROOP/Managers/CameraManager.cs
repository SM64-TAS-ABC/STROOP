using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using System.Windows.Forms;
using STROOP.Utilities;
using STROOP.Controls;
using STROOP.Extensions;
using STROOP.Structs.Configurations;

namespace STROOP.Managers
{
    public class CameraManager : DataManager
    {
        private BinaryButton _buttonDisableFOVFunctions;

        public CameraManager(string varFilePath, Control tabControl, WatchVariableFlowLayoutPanel variableTable)
            : base(varFilePath, variableTable)
        {
            var splitContainer = tabControl.Controls["splitContainerCamera"] as SplitContainer;

            var cameraPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
                true,
                cameraPosGroupBox,
                cameraPosGroupBox.Controls["buttonCameraPosXn"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosXp"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosZn"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosZp"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosXnZn"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosXnZp"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosXpZn"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosXpZp"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosYp"] as Button,
                cameraPosGroupBox.Controls["buttonCameraPosYn"] as Button,
                cameraPosGroupBox.Controls["textBoxCameraPosXZ"] as TextBox,
                cameraPosGroupBox.Controls["textBoxCameraPosY"] as TextBox,
                cameraPosGroupBox.Controls["checkBoxCameraPosRelative"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool useRelative) =>
                {
                    ButtonUtilities.TranslateCamera(
                        hOffset,
                        nOffset,
                        -1 * vOffset,
                        useRelative);
                });

            var cameraSphericalPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraSphericalPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Spherical,
                false,
                cameraSphericalPosGroupBox,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosTn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosTp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosPn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosPp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosTnPn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosTnPp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosTpPn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosTpPp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosRn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraSphericalPosRp"] as Button,
                cameraSphericalPosGroupBox.Controls["textBoxCameraSphericalPosTP"] as TextBox,
                cameraSphericalPosGroupBox.Controls["textBoxCameraSphericalPosR"] as TextBox,
                cameraSphericalPosGroupBox.Controls["checkBoxCameraSphericalPosPivotOnFocus"] as CheckBox,
                (float hOffset, float vOffset, float nOffset, bool pivotOnFocus) =>
                {
                    ButtonUtilities.TranslateCameraSpherically(
                        -1 * nOffset,
                        hOffset,
                        -1 * vOffset,
                        getSphericalPivotPoint(pivotOnFocus));
                });

            _buttonDisableFOVFunctions = splitContainer.Panel1.Controls["buttonDisableFOVFunctions"] as BinaryButton;
            _buttonDisableFOVFunctions.Initialize(
                "Disable FOV Functions",
                "Enable FOV Functions",
                () =>
                {
                    List<uint> addresses = CameraConfig.FovFunctionAddresses;
                    for (int i = 0; i < addresses.Count; i++)
                    {
                        Config.Stream.SetValue(0, addresses[i]);
                    }
                },
                () =>
                {
                    List<uint> addresses = CameraConfig.FovFunctionAddresses;
                    List<uint> values = CameraConfig.FovFunctionValues;
                    for (int i = 0; i < addresses.Count; i++)
                    {
                        Config.Stream.SetValue(values[i], addresses[i]);
                    }
                },
                () =>
                {
                    return CameraConfig.FovFunctionAddresses.All(
                        address => Config.Stream.GetUInt32(address) == 0);
                });
        }

        public override void Update(bool updateView)
        {
            if (!updateView) return;
            base.Update(updateView);

            _buttonDisableFOVFunctions.UpdateButton();
        }

        private (float pivotX, float pivotY, float pivotZ) getSphericalPivotPoint(bool pivotOnFocus)
        {
            float pivotX, pivotY, pivotZ;

            if (pivotOnFocus)
            {
                pivotX = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusXOffset);
                pivotY = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusYOffset);
                pivotZ = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.FocusZOffset);
            }
            else // pivot on Mario
            {
                pivotX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
                pivotY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
                pivotZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            }
            return (pivotX, pivotY, pivotZ);
        }
    }
}
