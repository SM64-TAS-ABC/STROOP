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
    public class Map3ObjectObject : Map3IconPointObject
    {
        private readonly uint ObjAddress;
        private readonly ObjectDataModel Obj;

        public Map3ObjectObject(uint objAddress)
            : base()
        {
            ObjAddress = objAddress;
            Obj = new ObjectDataModel(objAddress);
        }

        protected override Image GetImage()
        {
            Obj.Update();
            return Obj.BehaviorAssociation.MapImage;
        }

        protected override (double x, double y, double z, double angle) GetPositionAngle()
        {
            return PositionAngle.Obj(ObjAddress).GetValues();
        }
    }
}
