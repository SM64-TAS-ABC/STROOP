using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;

namespace STROOP.Map3
{
    public abstract class Map3Object
    {
        public float Size = 50;
        public double Opacity = 1;
        public byte OpacityByte { get => (byte)(Opacity * 255); }
        public Color Color = SystemColors.Control;

        public Map3Object()
        {
        }

        public abstract void DrawOnControl();

        public abstract string GetName();

        public abstract Image GetImage();

        public virtual float GetY()
        {
            return float.PositiveInfinity;
        }

        public virtual bool ShouldDisplay(MapTrackerVisibilityType visiblityType)
        {
            return true;
        }

        public virtual void NotifyStoreBehaviorCritera()
        {
        }

        public virtual PositionAngle GetPositionAngle()
        {
            return null;
        }
    }
}
