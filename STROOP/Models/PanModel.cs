using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public class PanModel
    {
        public double PanGlobalTimer
        {
            get => Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
            set => Config.Stream.SetValue((uint)value, MiscConfig.GlobalTimerAddress);
        }
        public double PanStartTime = 0;
        public double PanEndTime = 0;
        public double PanDuration
        {
            get => PanEndTime - PanStartTime;
            set => PanEndTime = PanStartTime + value;
        }

        public double PanEaseStart = 0;
        public double PanEaseEnd = 0;
        public double PanEaseDegree = 3;

        public double PanRotateCW = 0;

        public double PanCamStartX = 0;
        public double PanCamStartY = 0;
        public double PanCamStartZ = 0;
        public double PanCamStartYaw = 0;
        public double PanCamStartPitch = 0;

        public double PanCamEndX = 0;
        public double PanCamEndY = 0;
        public double PanCamEndZ = 0;
        public double PanCamEndYaw = 0;
        public double PanCamEndPitch = 0;

        public double PanRadiusStart = 1000;
        public double PanRadiusEnd = 1000;

        public double PanFOVStart = 45;
        public double PanFOVEnd = 45;
    }
}
