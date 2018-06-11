using STROOP.Structs;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace STROOP.Utilities
{
    public static class CamHackUtilities
    {
        public static double GetCamHackYawFacing(CamHackMode? camHackModeNullable = null)
        {
            CamHackMode camHackMode = camHackModeNullable ?? Config.CamHackManager.CurrentCamHackMode;
            switch (camHackMode)
            {
                case CamHackMode.REGULAR:
                    return Config.Stream.GetUInt16(CameraConfig.StructAddress + CameraConfig.FacingYawOffset);

                case CamHackMode.RELATIVE_ANGLE:
                case CamHackMode.ABSOLUTE_ANGLE:
                case CamHackMode.FIXED_POS:
                case CamHackMode.FIXED_ORIENTATION:
                    float camHackPosX = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraXOffset);
                    float camHackPosZ = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.CameraZOffset);
                    float camHackFocusX = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.FocusXOffset);
                    float camHackFocusZ = Config.Stream.GetSingle(CamHackConfig.StructAddress + CamHackConfig.FocusZOffset);
                    return MoreMath.AngleTo_AngleUnits(camHackPosX, camHackPosZ, camHackFocusX, camHackFocusZ);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
