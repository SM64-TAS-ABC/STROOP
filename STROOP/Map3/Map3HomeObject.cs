using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
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
        private readonly uint ObjAddress;
        private readonly PositionAngle ObjPosAngle;
        private readonly PositionAngle HomePosAngle;

        public Map3HomeObject(uint objAddress)
            : base()
        {
            ObjAddress = objAddress;
            ObjPosAngle = PositionAngle.Obj(objAddress);
            HomePosAngle = PositionAngle.ObjHome(objAddress);
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.HomeImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return HomePosAngle;
        }

        public override string GetName()
        {
            return "Home for " + ObjPosAngle.GetMapName();
        }

        public override float GetY()
        {
            return (float)HomePosAngle.Y;
        }
    }
}
