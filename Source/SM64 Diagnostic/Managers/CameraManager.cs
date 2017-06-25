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

namespace SM64_Diagnostic.Managers
{
    public class CameraManager : DataManager
    {
        TextBox _cameraPosXZTextbox;
        TextBox _cameraPosYTextbox;

        public CameraManager(ProcessStream stream, List<WatchVariable> cameraData, Control tabControl, NoTearFlowLayoutPanel variableTable)
            : base(stream, cameraData, variableTable)
        {
            var cameraPosGroupBox = tabControl.Controls["groupBoxCameraPos"] as GroupBox;
            _cameraPosXZTextbox = cameraPosGroupBox.Controls["textBoxCameraPosXZ"] as TextBox;
            _cameraPosYTextbox = cameraPosGroupBox.Controls["textBoxCameraPosY"] as TextBox;
            (cameraPosGroupBox.Controls["buttoncameraPosXp"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, 1, 0);
            (cameraPosGroupBox.Controls["buttoncameraPosXn"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, -1, 0);
            (cameraPosGroupBox.Controls["buttoncameraPosZp"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, 0, 1);
            (cameraPosGroupBox.Controls["buttoncameraPosZn"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, 0, -1);
            (cameraPosGroupBox.Controls["buttoncameraPosXpZp"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, 1, 1);
            (cameraPosGroupBox.Controls["buttoncameraPosXpZn"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, 1, -1);
            (cameraPosGroupBox.Controls["buttoncameraPosXnZp"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, -1, 1);
            (cameraPosGroupBox.Controls["buttoncameraPosXnZn"] as Button).Click += (sender, e) => cameraPosXZButton_Click(sender, e, -1, -1);
            (cameraPosGroupBox.Controls["buttoncameraPosYp"] as Button).Click += (sender, e) => cameraPosYButton_Click(sender, e, 1);
            (cameraPosGroupBox.Controls["buttoncameraPosYn"] as Button).Click += (sender, e) => cameraPosYButton_Click(sender, e, -1);
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

        private void cameraPosXZButton_Click(object sender, EventArgs e, int xSign, int zSign)
        {
            float xzValue;
            if (!float.TryParse(_cameraPosXZTextbox.Text, out xzValue))
                return;

            MarioActions.MoveCamera(_stream, xSign * xzValue, 0, zSign * xzValue);
        }

        private void cameraPosYButton_Click(object sender, EventArgs e, int ySign)
        {
            float yValue;
            if (!float.TryParse(_cameraPosYTextbox.Text, out yValue))
                return;

            MarioActions.MoveCamera(_stream, 0, ySign * yValue, 0);
        }
    }
}
