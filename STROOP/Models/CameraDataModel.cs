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
        private ushort _facingYaw;
        public ushort FacingYaw
        {
            get => _facingYaw;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.FacingYawOffset))
                    _facingYaw = value;
            }
        }
        private ushort _facingPitch;
        public ushort FacingPitch
        {
            get => _facingPitch;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.FacingPitchOffset))
                    _facingPitch = value;
            }
        }
        private ushort _facingRoll;
        public ushort FacingRoll
        {
            get => _facingRoll;
            set
            {
                if (Config.Stream.SetValue(value, CameraConfig.CameraStructAddress + CameraConfig.FacingRollOffset))
                    _facingRoll = value;
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
                if (Config.Stream.SetValue(value, CameraHackConfig.CameraHackStruct + CameraHackConfig.ObjectOffset))
                    _hackObject = value;
            }
        }
        #endregion

        public void Update()
        {
            // Update camera position and rotation
            _x = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.XOffset);
            _y = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.YOffset);
            _z = Config.Stream.GetSingle(CameraConfig.CameraStructAddress + CameraConfig.ZOffset);

            _facingYaw =    Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.FacingYawOffset);
            _facingPitch =  Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.FacingPitchOffset);
            _facingRoll =   Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.FacingRollOffset);

            _secondaryObject = Config.Stream.GetUInt32(CameraConfig.SecondaryObjectAddress);
            _hackObject = Config.Stream.GetUInt32(CameraHackConfig.CameraHackStruct + CameraHackConfig.ObjectOffset);
        }

        public void Update2() { }
    }
}
