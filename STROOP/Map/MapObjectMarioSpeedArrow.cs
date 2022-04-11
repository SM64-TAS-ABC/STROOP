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
    public class MapObjectMarioSpeedArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;

        public MapObjectMarioSpeedArrow(PositionAngle posAngle)
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
            float x = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            float z = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
            return MoreMath.AngleTo_AngleUnits(x, z);
        }

        protected override double GetPitch()
        {
            return Config.Stream.GetShort(MarioConfig.StructAddress + MarioConfig.FacingPitchOffset);
        }

        protected override double GetRecommendedSize()
        {
            float x = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            float z = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
            return MoreMath.GetHypotenuse(x, z);
        }

        public override void SetDragPositionTopDownView(double? x = null, double? y = null, double? z = null)
        {
            if (!x.HasValue || !z.HasValue) return;

            PositionAngle posAngle = GetPositionAngle();
            double dist = MoreMath.GetDistanceBetween(posAngle.X, posAngle.Z, x.Value, z.Value);
            double angle = MoreMath.AngleTo_AngleUnits(posAngle.X, posAngle.Z, x.Value, z.Value);
            double xDiff = x.Value - posAngle.X;
            double zDiff = z.Value - posAngle.Z;

            if (_useRecommendedArrowLength)
            {
                Config.Stream.SetValue((float)xDiff, MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
                Config.Stream.SetValue((float)zDiff, MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
            }
            else
            {
                GetParentMapTracker().SetSize((float)(Scales ? dist : dist * Config.CurrentMapGraphics.MapViewScaleValue));
                SetYaw(angle);
            }
        }

        protected override void SetRecommendedSize(double size)
        {
            float xSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            float zSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
            if (xSpeed == 0 && zSpeed == 0) xSpeed = 1;
            double hSpeed = MoreMath.GetHypotenuse(xSpeed, zSpeed);

            double multiplier = size / hSpeed;
            double newXSpeed = xSpeed * multiplier;
            double newZSpeed = zSpeed * multiplier;

            Config.Stream.SetValue((float)newXSpeed, MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            Config.Stream.SetValue((float)newZSpeed, MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
        }

        protected override void SetYaw(double yaw)
        {
            float xSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            float zSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
            double hSpeed = MoreMath.GetHypotenuse(xSpeed, zSpeed);

            (double newXSpeed, double newZSpeed) = MoreMath.GetComponentsFromVector(hSpeed, yaw);

            Config.Stream.SetValue((float)newXSpeed, MarioConfig.StructAddress + MarioConfig.XSpeedOffset);
            Config.Stream.SetValue((float)newZSpeed, MarioConfig.StructAddress + MarioConfig.ZSpeedOffset);
        }

        public override string GetName()
        {
            return "Mario Speed Arrow for " + _posAngle.GetMapName();
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
