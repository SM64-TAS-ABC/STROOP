using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;

namespace STROOP.Controls.Map.Objects
{
    class Map4WallTriObject : Map4TriangleObject
    {
        protected override TriangleDataModel _triangleData => DataModels.Mario.WallTriangle;

        public Map4WallTriObject() : base("Wall Tri", Config.ObjectAssociations.TriangleWallImage as Bitmap, Color.Green)
        {
        }
    }
}
