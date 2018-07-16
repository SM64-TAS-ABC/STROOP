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
    class MapHolpObject : MapIconObject
    {
        protected override MapGraphicsIconItem _iconGraphics { get; set; }

        public MapHolpObject() : base("HOLP", Config.ObjectAssociations.HolpImage as Bitmap)
        {
            _iconGraphics = new MapGraphicsIconItem(Config.ObjectAssociations.HolpImage as Bitmap);
        }

        public override void Update()
        {
            _iconGraphics.Position = new OpenTK.Vector3(DataModels.Mario.HolpX, DataModels.Mario.HolpY, DataModels.Mario.HolpZ);
        }
    }
}
