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
        Bitmap _lastGraphic = null;

        public MapSm64Object(int slotIndex) : base("Object")
        {
            _iconGraphics = new MapGraphicsIconItem(null);
            _slotIndex = slotIndex;
        }

        public override void Update()
        {
            ObjectDataModel obj = DataModels.Objects[_slotIndex];
            if (obj == null)
                return;

            string objectName = Config.ObjectAssociations.GetObjectName(obj.BehaviorCriteria);
            uint address = ObjectUtilities.GetObjectAddress(_slotIndex);
            string slotLabel = Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(address, true);
            Name = String.Format("{0} [{1}]", objectName, slotLabel);

            Bitmap currentGraphics = Config.ObjectAssociations.GetObjectMapImage(obj.BehaviorCriteria) as Bitmap;
            if (currentGraphics != _lastGraphic)
            {
                _lastGraphic = currentGraphics;
                _iconGraphics.ChangeImage(currentGraphics);
                ChangeImageAndName();
            }

            _iconGraphics.Rotation = (float) MoreMath.AngleUnitsToRadians(obj.FacingYaw);
            _iconGraphics.Position = new OpenTK.Vector3(obj.X, obj.Y, obj.Z);
        }
    }
}
