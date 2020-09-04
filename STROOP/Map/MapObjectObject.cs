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

namespace STROOP.Map
{
    public class MapObjectObject : MapIconPointObject
    {
        private readonly ObjectDataModel _obj;
        private readonly PositionAngle _posAngle;

        public MapObjectObject(uint objAddress)
            : base()
        {
            _obj = new ObjectDataModel(objAddress);
            _posAngle = PositionAngle.Obj(objAddress);
        }

        public override Image GetInternalImage()
        {
            _obj.Update();
            if (_obj.BehaviorAssociation == null)
            {
                return Config.ObjectAssociations.DefaultImage;
            }
            return _iconType == MapTrackerIconType.ObjectSlotImage ?
                _obj.BehaviorAssociation.Image :
                _obj.BehaviorAssociation.MapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override string GetName()
        {
            return _posAngle.GetMapName();
        }

        public override void Update()
        {
            base.Update();
            _obj.Update();
            InternalRotates = Config.ObjectAssociations.GetObjectMapRotates(_obj.BehaviorCriteria);
        }
    }
}
