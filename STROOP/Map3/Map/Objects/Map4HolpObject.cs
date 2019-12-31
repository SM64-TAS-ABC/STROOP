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
    class Map4HolpObject : Map4IconObject
    {
        protected override Map4GraphicsIconItem _iconGraphics { get; set; }

        public Map4HolpObject() : base("HOLP", Config.ObjectAssociations.HolpImage as Bitmap, null, false)
        {
            _iconGraphics = new Map4GraphicsIconItem(Config.ObjectAssociations.HolpImage as Bitmap);
        }

        public override void Update()
        {
            _iconGraphics.Position = new OpenTK.Vector3(DataModels.Mario.HolpX, DataModels.Mario.HolpY, DataModels.Mario.HolpZ);
        }
    }
}
