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
    class MapSm64Object : MapIconObject
    {
        protected override MapGraphicsIconItem _iconGraphics { get; set; }

        public MapSm64Object()
        {
            _iconGraphics = new MapGraphicsIconItem(Config.ObjectAssociations.MarioMapImage as Bitmap);
        }


        public override void Update()
        {
            _iconGraphics.Rotation = (float) MoreMath.AngleUnitsToRadians(DataModels.Mario.FacingYaw);
            _iconGraphics.Position = new OpenTK.Vector3(DataModels.Mario.X, DataModels.Mario.Y, DataModels.Mario.Z);
        }
    }
}
