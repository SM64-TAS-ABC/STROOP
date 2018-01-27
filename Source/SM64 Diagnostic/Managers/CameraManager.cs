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
using static SM64_Diagnostic.Utilities.ControlUtilities;

namespace SM64_Diagnostic.Managers
{
    public class CameraManager : DataManager
    {
        public CameraManager(List<WatchVariableControlPrecursor> cameraData, Control tabControl, WatchVariablePanel variableTable)
            : base(cameraData, variableTable)
        {
            var splitContainer = tabControl.Controls["splitContainerCamera"] as SplitContainer;
            
            var cameraPosGroupBox = splitContainer.Panel1.Controls["groupBoxCameraPos"] as GroupBox;
            ControlUtilities.InitializeThreeDimensionController(
                CoordinateSystem.Euler,
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
        }

        public override void Update(bool updateView)
        {
            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
        }

        private (float pivotX, float pivotY, float pivotZ) getSphericalPivotPoint(bool pivotOnFocus)
        {
            float pivotX, pivotY, pivotZ;

            if (pivotOnFocus)
            {
                pivotX = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.FocusXOffset);
                pivotY = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.FocusYOffset);
                pivotZ = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.FocusZOffset);
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
