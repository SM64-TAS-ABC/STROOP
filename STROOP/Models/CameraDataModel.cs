using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public class CameraDataModel : IUpdatableDataModel
    {
        #region Position
        private float _x;
        public float X
        {
            get => _x;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.XOffset))
                    _x = value;
            }
        }

        private float _y;
        public float Y
        {
            get => _y;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.YOffset))
                    _y = value;
            }
        }

        private float _z;
        public float Z
        {
            get => _z;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.ZOffset))
                    _z = value;
            }
        }
        #endregion
        #region Rotation
        private ushort _facingYaw;
        public ushort FacingYaw
        {
            get => _facingYaw;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.FacingYawOffset))
                    _facingYaw = value;
            }
        }
        private ushort _facingPitch;
        public ushort FacingPitch
        {
            get => _facingPitch;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.FacingPitchOffset))
                    _facingPitch = value;
            }
        }
        private ushort _facingRoll;
        public ushort FacingRoll
        {
            get => _facingRoll;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.FacingRollOffset))
                    _facingRoll = value;
            }
        }
        #endregion
        #region FOV
        private float _fov;
        public float FOV
        {
            get => _fov;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.StructAddress + CameraConfig.FOV))
                    _fov = value;
            }
        }
        #endregion
        #region Objects
        private uint _secondaryObject;
        public uint SecondaryObject
        {
            get => _secondaryObject;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.SecondaryObjectAddress))
                    _secondaryObject = value;
            }
        }

        private uint _hackObject;
        public uint HackObject
        {
            get => _hackObject;
            set
            {
                if (Config.Stream.SetValue(value, CamHackConfig.StructAddress + CamHackConfig.ObjectOffset))
                    _hackObject = value;
            }
        }
        #endregion

        public void Update()
        {
            // Update camera position and rotation
            _x = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.XOffset);
            _y = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.YOffset);
            _z = Config.Stream.GetSingle(CameraConfig.StructAddress + CameraConfig.ZOffset);

            _facingYaw =    Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);
            _facingPitch =  Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingPitchOffset);
            _facingRoll =   Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingRollOffset);

            _fov = Config.Stream.GetSingle(CameraConfig.FOV);

            _secondaryObject = Config.Stream.GetUInt32(CameraConfig.SecondaryObjectAddress);
            _hackObject = Config.Stream.GetUInt32(CamHackConfig.StructAddress + CamHackConfig.ObjectOffset);
        }

        public void Update2() { }
    }
}
