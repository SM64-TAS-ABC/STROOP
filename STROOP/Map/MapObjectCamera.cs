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

namespace STROOP.Map
{
    public class MapObjectCamera : MapObjectIconPoint
    {
        public MapObjectCamera()
            : base()
        {
            InternalRotates = true;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CameraMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.Camera;
        }

        public override string GetName()
        {
            return "Camera";
        }
    }
}
