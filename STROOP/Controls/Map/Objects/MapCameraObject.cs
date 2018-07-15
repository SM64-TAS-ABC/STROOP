using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Controls.Map.Objects
{
    class MapCameraObject : MapIconObject
    {
        protected override MapGraphicsIconItem _iconGraphics { get; set; }

        public MapCameraObject()
        {
            _iconGraphics = new MapGraphicsIconItem(Config.ObjectAssociations.CameraMapImage as Bitmap, "Camera");
        }

        public override void Update()
        {
            _iconGraphics.Rotation = Rotates ? (float)MoreMath.AngleUnitsToRadians(DataModels.Camera.FacingYaw) : 0;
            _iconGraphics.Position = new OpenTK.Vector3(DataModels.Camera.X, DataModels.Camera.Y, DataModels.Camera.Z);
        }
    }
}
