using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Map3.Map.Graphics.Items;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Map3.Map.Objects
{
    class Map4MarioObject : Map4IconObject
    {
        protected override Map4GraphicsIconItem _iconGraphics { get; set; }

        public Map4MarioObject() : base("Mario", Config.ObjectAssociations.MarioMapImage as Bitmap, null, true)
        {
            _iconGraphics = new Map4GraphicsIconItem(Config.ObjectAssociations.MarioMapImage as Bitmap);
        }

        public override void Update()
        {
            _iconGraphics.Rotation = Rotates ? (float)MoreMath.AngleUnitsToRadians(DataModels.Mario.FacingYaw) : 0;
            _iconGraphics.Position = new OpenTK.Vector3(DataModels.Mario.X, DataModels.Mario.Y, DataModels.Mario.Z);
        }
    }
}
