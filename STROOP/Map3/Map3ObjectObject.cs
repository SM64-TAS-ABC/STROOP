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
        private readonly PositionAngle PosAngle;

        private BehaviorCriteria? _behaviorCriteriaToDisplay;

        public Map3ObjectObject(uint objAddress)
            : base()
        {
            ObjAddress = objAddress;
            Obj = new ObjectDataModel(objAddress);
            PosAngle = PositionAngle.Obj(objAddress);

            _behaviorCriteriaToDisplay = null;
        }

        public override Image GetImage()
        {
            Obj.Update();
            return Obj.BehaviorAssociation.MapImage;
        }

        public override PositionAngle GetPositionAngle()
        {
            return PosAngle;
        }

        public override string GetName()
        {
            return "Object"; // TODO change this
        }

        public override float GetY()
        {
            return (float)PosAngle.Y;
        }

        public override bool ShouldDisplay(MapTrackerVisibilityType visiblityType)
        {
            Obj.Update();
            switch (visiblityType)
            {
                case MapTrackerVisibilityType.VisibleAlways:
                    return true;
                case MapTrackerVisibilityType.VisibleWhenLoaded:
                    return Obj.IsActive;
                case MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded:
                    return Obj.IsActive && BehaviorCriteria.HasSameAssociation(Obj.BehaviorCriteria, _behaviorCriteriaToDisplay);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void NotifyStoreBehaviorCritera()
        {
            Obj.Update();
            _behaviorCriteriaToDisplay = Obj.BehaviorCriteria;
        }
    }
}
