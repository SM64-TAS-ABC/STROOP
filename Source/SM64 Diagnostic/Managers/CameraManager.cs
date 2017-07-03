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
    public class CameraManager : DataManager
    {
        TextBox _cameraSphericalPosThetaPhiTextbox;
        TextBox _cameraSphericalPosRadiusTextbox;

        public CameraManager(ProcessStream stream, List<WatchVariable> cameraData, Control tabControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, cameraData, variableTable)
        {
            var cameraPosGroupBox = tabControl.Controls["groupBoxCameraPos"] as GroupBox;
            PositionController.initialize(
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
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.TranslateCamera(
                        _stream,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });

            /*
            var cameraSphericalPosGroupBox = tabControl.Controls["groupBoxCameraSphericalPos"] as GroupBox;
            PositionController.initialize(
                cameraSphericalPosGroupBox.Controls["buttonCameraPosTn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosTp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosPn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosPp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosTnPn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosTnPp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosTpPn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosTpPp"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosRn"] as Button,
                cameraSphericalPosGroupBox.Controls["buttonCameraPosRp"] as Button,
                cameraSphericalPosGroupBox.Controls["textBoxCameraPosXZ"] as TextBox,
                cameraSphericalPosGroupBox.Controls["textBoxCameraPosY"] as TextBox,
                cameraSphericalPosGroupBox.Controls["checkBoxCameraPosRelative"] as CheckBox,
                (float xOffset, float yOffset, float zOffset, bool useRelative) =>
                {
                    MarioActions.TranslateCamera(
                        _stream,
                        xOffset,
                        yOffset,
                        zOffset,
                        useRelative);
                });
                */


            var cameraSphericalPosGroupBox = tabControl.Controls["groupBoxCameraSphericalPos"] as GroupBox;
            _cameraSphericalPosThetaPhiTextbox = cameraSphericalPosGroupBox.Controls["textBoxCameraSphericalPosTP"] as TextBox;
            _cameraSphericalPosRadiusTextbox = cameraSphericalPosGroupBox.Controls["textBoxCameraSphericalPosR"] as TextBox;
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosTp"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, 1, 0);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosTn"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, -1, 0);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosPp"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, 0, 1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosPn"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, 0, -1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosTpPp"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, 1, 1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosTpPn"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, 1, -1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosTnPp"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, -1, 1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosTnPn"] as Button).Click += (sender, e) => cameraSphericalPosThetaPhiButton_Click(sender, e, -1, -1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosRp"] as Button).Click += (sender, e) => cameraSphericalPosRadiusButton_Click(sender, e, 1);
            (cameraSphericalPosGroupBox.Controls["buttoncameraSphericalPosRn"] as Button).Click += (sender, e) => cameraSphericalPosRadiusButton_Click(sender, e, -1);
        }

        protected override void InitializeSpecialVariables()
        {
            _specialWatchVars = new List<IDataContainer>()
            {
                new DataContainer("DistanceToMario"),
            };
        }

        public void ProcessSpecialVars()
        {
            float mX, mY, mZ;
            mX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            mY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            mZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);

            float cameraX, cameraY, cameraZ;
            cameraX = _stream.GetSingle(Config.Camera.CameraX);
            cameraY = _stream.GetSingle(Config.Camera.CameraY);
            cameraZ = _stream.GetSingle(Config.Camera.CameraZ);

            foreach (var specialVar in _specialWatchVars)
            {
                switch (specialVar.SpecialName)
                {
                    case "DistanceToMario":
                        (specialVar as DataContainer).Text = Math.Round(MoreMath.DistanceTo(cameraX, cameraY, cameraZ, mX, mY, mZ),3).ToString();
                        break;
                }
            }
        }

        public override void Update(bool updateView)
        {
            ProcessSpecialVars();

            // We are done if we don't need to update the Mario Manager view
            if (!updateView)
                return;

            base.Update();
        }

        private void cameraSphericalPosThetaPhiButton_Click(object sender, EventArgs e, int thetaSign, int phiSign)
        {
            float thetaPhiValue;
            if (!float.TryParse(_cameraSphericalPosThetaPhiTextbox.Text, out thetaPhiValue))
                return;

            float pivotX, pivotY, pivotZ;
            (pivotX, pivotY, pivotZ) = getSphericalPivotPoint();

            MarioActions.TranslateCameraSpherically(_stream, 0, thetaSign * thetaPhiValue, phiSign * thetaPhiValue, pivotX, pivotY, pivotZ);
        }

        private void cameraSphericalPosRadiusButton_Click(object sender, EventArgs e, int radiusSign)
        {
            float radiusValue;
            if (!float.TryParse(_cameraSphericalPosRadiusTextbox.Text, out radiusValue))
                return;

            float pivotX, pivotY, pivotZ;
            (pivotX, pivotY, pivotZ) = getSphericalPivotPoint();

            MarioActions.TranslateCameraSpherically(_stream, radiusSign * radiusValue, 0, 0, pivotX, pivotY, pivotZ);
        }

        private (float pivotX, float pivotY, float pivotZ) getSphericalPivotPoint()
        {
            float pivotX, pivotY, pivotZ;
            pivotX = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.XOffset);
            pivotY = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.YOffset);
            pivotZ = _stream.GetSingle(Config.Mario.StructAddress + Config.Mario.ZOffset);
            return (pivotX, pivotY, pivotZ);
        }
    }
}
