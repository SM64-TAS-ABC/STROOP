using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using STROOP.Models;

namespace STROOP.Map3
{
    public class Map3HomeObject : Map3IconPointObject
    {
        private readonly PositionAngle _posAngle;

        public Map3HomeObject(uint objAddress)
            : base()
        {
            _posAngle = PositionAngle.ObjHome(objAddress);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.HomeImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override string GetName()
        {
            return _posAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)_posAngle.Y;
        }
    }
}
