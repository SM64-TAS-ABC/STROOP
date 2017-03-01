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
        public CameraManager(ProcessStream stream, List<WatchVariable> cameraData, NoTearFlowLayoutPanel variableTable)
            : base(stream, cameraData, variableTable)
        {
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
    }
}
