using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public class CameraDataModel : UpdatableDataModel
    {
        #region Position
        private float _x;
        public float X
        {
            get => _x;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.XOffset))
                    _x = value;
            }
        }

        private float _y;
        public float Y
        {
            get => _y;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.YOffset))
                    _y = value;
            }
        }

        private float _z;
        public float Z
        {
            get => _z;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.ZOffset))
                    _z = value;
            }
        }
        #endregion
        #region Rotation
        private ushort _facingAngle;
        public ushort FacingAngle
        {
            get => _facingAngle;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.YawFacingOffset))
                    _facingAngle = value;
            }
        }
        #endregion

        public override void Update(int dependencyLevel)
        {
            switch (dependencyLevel)
            {
                case 0:
                    // Update camera position and rotation
                    _x = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
                    _y = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
                    _z = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);
                    _facingAngle = Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.YawFacingOffset);
                    break;
            }
        }
    }
}
