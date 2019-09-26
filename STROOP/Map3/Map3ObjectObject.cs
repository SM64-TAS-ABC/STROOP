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
        private readonly ObjectDataModel _obj;
        private readonly PositionAngle _posAngle;

        private BehaviorCriteria? _behaviorCriteriaToDisplay;

        public Map3ObjectObject(uint objAddress)
            : base()
        {
            _obj = new ObjectDataModel(objAddress);
            _posAngle = PositionAngle.Obj(objAddress);

            _behaviorCriteriaToDisplay = null;
        }

        public override Image GetImage()
        {
            _obj.Update();
            return _obj.BehaviorAssociation.MapImage;
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

        public override bool ShouldDisplay(MapTrackerVisibilityType visiblityType)
        {
            _obj.Update();
            switch (visiblityType)
            {
                case MapTrackerVisibilityType.VisibleAlways:
                    return true;
                case MapTrackerVisibilityType.VisibleWhenLoaded:
                    return _obj.IsActive;
                case MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded:
                    return _obj.IsActive && BehaviorCriteria.HasSameAssociation(_obj.BehaviorCriteria, _behaviorCriteriaToDisplay);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void NotifyStoreBehaviorCritera()
        {
            _obj.Update();
            _behaviorCriteriaToDisplay = _obj.BehaviorCriteria;
        }
    }
}
