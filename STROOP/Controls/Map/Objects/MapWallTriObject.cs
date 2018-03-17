using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using STROOP.Models;
using STROOP.Structs;

namespace STROOP.Controls.Map.Objects
{
    class MapWallTriObject : MapTriangleObject
    {
        protected override TriangleDataModel _triangleData => DataModels.Mario.WallTriangle;

        public MapWallTriObject()
        {
            Color = Color4.Green;
        }
    }
}
