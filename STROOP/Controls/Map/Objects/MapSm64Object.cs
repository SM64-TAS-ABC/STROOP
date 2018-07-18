using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Models;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;

namespace STROOP.Controls.Map.Objects
{
    class MapSm64Object : MapIconObject
    {
        int _slotIndex;
        protected override MapGraphicsIconItem _iconGraphics { get; set; }

        private BehaviorCriteria? _visibilityBehaviorCriteria = null;
        public override MapTrackerVisibilityType VisibilityType
        {
            set
            {
                base.VisibilityType = value;
                if (VisibilityType == MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded)
                {
                    ObjectDataModel obj = DataModels.Objects[_slotIndex];
                    _visibilityBehaviorCriteria = obj.BehaviorCriteria;
                }
            }
        }

        public MapSm64Object(int slotIndex) : base("Object", null, null, false)
        {
            _iconGraphics = new MapGraphicsIconItem(null);
            _slotIndex = slotIndex;
        }

        private bool GetDisplayed()
        {
            ObjectDataModel obj = DataModels.Objects[_slotIndex];
            switch (VisibilityType)
            {
                case MapTrackerVisibilityType.VisibleAlways:
                    return true;
                case MapTrackerVisibilityType.VisibleWhenLoaded:
                    return obj.IsActive;
                case MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded:
                    if (!_visibilityBehaviorCriteria.HasValue) return false;
                    return obj.IsActive && obj.BehaviorCriteria == _visibilityBehaviorCriteria;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Update()
        {
            ObjectDataModel obj = DataModels.Objects[_slotIndex];
            if (obj == null)
                return;

            string objectName = Config.ObjectAssociations.GetObjectName(obj.BehaviorCriteria);
            uint address = ObjectUtilities.GetObjectAddress(_slotIndex);
            string slotLabel = Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(address, true);
            Name = String.Format("[{0}] {1}", slotLabel, objectName);
            BackColor = ObjectUtilities.GetProcessingGroupColorForObjAddress(address);
            Displayed = GetDisplayed();

            Bitmap currentGraphics = Config.ObjectAssociations.GetObjectMapImage(obj.BehaviorCriteria) as Bitmap;
            if (currentGraphics != BitmapImage)
            {
                BitmapImage = currentGraphics;
                _iconGraphics.ChangeImage(currentGraphics);
                Rotates = Config.ObjectAssociations.GetObjectMapRotates(obj.BehaviorCriteria);
            }

            _iconGraphics.Rotation = Rotates ? (float)MoreMath.AngleUnitsToRadians(obj.FacingYaw) : 0;
            _iconGraphics.Position = new OpenTK.Vector3(obj.X, obj.Y, obj.Z);
        }
    }
}
