using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectMarioSlidingArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;

        public MapObjectMarioSlidingArrow(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        protected override double GetYaw()
        {
            return WatchVariableSpecialUtilities.GetMarioSlidingAngle();
        }

        protected override double GetPitch()
        {
            return Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
        }

        protected override double GetRecommendedSize()
        {
            return WatchVariableSpecialUtilities.GetMarioSlidingSpeed();
        }

        public override void SetDragPosition(double? x = null, double? y = null, double? z = null)
        {
            if (!x.HasValue || !z.HasValue) return;

            PositionAngle posAngle = GetPositionAngle();
            double dist = MoreMath.GetDistanceBetween(posAngle.X, posAngle.Z, x.Value, z.Value);
            double angle = MoreMath.AngleTo_AngleUnits(posAngle.X, posAngle.Z, x.Value, z.Value);
            double xDiff = x.Value - posAngle.X;
            double zDiff = z.Value - posAngle.Z;

            if (_useRecommendedArrowLength)
            {
                Config.Stream.SetValue((float)xDiff, MarioConfig.StructAddress + MarioConfig.SlidingSpeedXOffset);
                Config.Stream.SetValue((float)zDiff, MarioConfig.StructAddress + MarioConfig.SlidingSpeedZOffset);
            }
            else
            {
                GetParentMapTracker().SetSize((float)(Scales ? dist : dist * Config.CurrentMapGraphics.MapViewScaleValue));
                WatchVariableSpecialUtilities.SetMarioSlidingAngle(angle);
            }
        }

        protected override void SetRecommendedSize(double size)
        {
            WatchVariableSpecialUtilities.SetMarioSlidingSpeed(size);
        }

        protected override void SetYaw(double yaw)
        {
            WatchVariableSpecialUtilities.SetMarioSlidingAngle(yaw);
        }

        public override string GetName()
        {
            return "Mario Sliding Arrow";
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
            };
        }
    }
}
