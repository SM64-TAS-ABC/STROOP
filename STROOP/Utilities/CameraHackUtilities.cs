using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class CameraHackUtilities
    {
        public static double GetCamHackYawFacing(CamHackMode? camHackModeNullable = null)
        {
            CamHackMode camHackMode = camHackModeNullable ?? Config.CameraHackManager.CurrentCamHackMode;
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                    return Config.Stream.GetUInt16(CameraConfig.CameraStructAddress + CameraConfig.FacingYawOffset);

                case CamHackMode.RELATIVE_ANGLE:
                case CamHackMode.ABSOLUTE_ANGLE:
                case CamHackMode.FIXED_POS:
                case CamHackMode.FIXED_ORIENTATION:
                    float camHackPosX = Config.Stream.GetSingle(CameraHackConfig.CameraHackStructAddress + CameraHackConfig.CameraXOffset);
                    float camHackPosZ = Config.Stream.GetSingle(CameraHackConfig.CameraHackStructAddress + CameraHackConfig.CameraZOffset);
                    float camHackFocusX = Config.Stream.GetSingle(CameraHackConfig.CameraHackStructAddress + CameraHackConfig.FocusXOffset);
                    float camHackFocusZ = Config.Stream.GetSingle(CameraHackConfig.CameraHackStructAddress + CameraHackConfig.FocusZOffset);
                    return MoreMath.AngleTo_AngleUnits(camHackPosX, camHackPosZ, camHackFocusX, camHackFocusZ);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
