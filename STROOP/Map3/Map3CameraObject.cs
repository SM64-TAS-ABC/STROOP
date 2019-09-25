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
    public class Map3CameraObject : Map3IconPointObject
    {
        public Map3CameraObject()
            : base()
        {
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.CameraMapImage;
        }

        protected override (double x, double y, double z, double angle) GetPositionAngle()
        {
            return PositionAngle.Camera.GetValues();
        }

        public override string GetName()
        {
            return "Camera";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Camera.Y;
        }
    }
}
