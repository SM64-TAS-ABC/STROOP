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
    class MapMarioObject : MapObject
    {
        MapGraphicsIconItem _icon;

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new List<MapGraphicsItem>() { _icon };

        public MapMarioObject()
        {
            _icon = new MapGraphicsIconItem(Config.ObjectAssociations.MarioMapImage as Bitmap);
        }

        public override void Update()
        {
            _icon.Rotation = (float)MoreMath.AngleUnitsToRadians(DataModels.Mario.FacingYaw);
            _icon.Position = new OpenTK.Vector3(DataModels.Mario.X, DataModels.Mario.Y, DataModels.Mario.Z);
        }
    }
}
