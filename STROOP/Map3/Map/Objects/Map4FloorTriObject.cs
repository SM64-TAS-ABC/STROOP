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
    class Map4FloorTriObject : Map4TriangleObject
    {
        protected override TriangleDataModel _triangleData => DataModels.Mario.FloorTriangle;

        public Map4FloorTriObject() : base("Floor Tri", Config.ObjectAssociations.TriangleFloorImage as Bitmap, Color.Cyan)
        {
        }
    }
}
