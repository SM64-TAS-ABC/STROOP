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
    public class MapObjectCameraFocus : MapObjectIconPoint
    {
        public MapObjectCameraFocus()
            : base()
        {
            Size = 15;
            InternalRotates = false;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CameraFocusMapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return PositionAngle.CameraFocus;
        }

        public override string GetName()
        {
            return "Camera Focus";
        }
    }
}
