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
    class Map4CeilingTriObject : Map4TriangleObject
    {
        protected override TriangleDataModel _triangleData => DataModels.Mario.CeilingTriangle;

        public Map4CeilingTriObject() : base("Ceiling Tri", Config.ObjectAssociations.TriangleCeilingImage as Bitmap, Color.Red)
        {
        }
    }
}
