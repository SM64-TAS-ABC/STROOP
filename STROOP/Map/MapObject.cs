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
using System.Windows.Forms;
using OpenTK.Graphics;
using STROOP.Models;
using System.Xml.Linq;

namespace STROOP.Map
{
    public abstract class MapObject
    {
        public float Size = 25;
        public double Opacity = 1;
        public byte OpacityByte
        {
            get => (byte)(Opacity * 255);
            set => Opacity = value / 255f;
        }
        public int OpacityPercent
        {
            get => (int)(Opacity * 100);
            set => Opacity = value / 100.0;
        }
        public float OutlineWidth = 1;
        public Color Color = SystemColors.Control;
        public Color4 Color4 { get => new Color4(Color.R, Color.G, Color.B, OpacityByte); }
        public Color OutlineColor = Color.Black;

        public bool? CustomRotates = null;
        public bool InternalRotates = false;
        public bool Rotates
        {
            get => CustomRotates ?? InternalRotates;
        }

        private BehaviorCriteria? _behaviorCriteriaToDisplay = null;

        public bool ShowTriUnits = false;

        protected ContextMenuStrip _contextMenuStrip = null;

        private MapObjectSettingsAccumulator _accumulator = new MapObjectSettingsAccumulator();

        public MapObject()
        {
        }

        public void DrawOn2DControl()
        {
            if (Config.MapGui.checkBoxMapOptionsEnableOrthographicView.Checked)
            {
                DrawOn2DControlOrthographicView();
            }
            else
            {
                DrawOn2DControlTopDownView();
            }
        }

        public abstract void DrawOn2DControlTopDownView();

        public abstract void DrawOn2DControlOrthographicView();

        public abstract void DrawOn3DControl();

        public virtual Matrix4 GetModelMatrix()
        {
            return Matrix4.Identity;
        }

        public abstract string GetName();

        protected Image _customImage = null;
        public abstract Image GetInternalImage();
        public Image GetImage() { return _customImage ?? GetInternalImage(); }

        protected MapTrackerIconType _iconType = MapTrackerIconType.TopDownImage;
        public virtual void SetIconType(MapTrackerIconType iconType, Image image = null)
        {
            if ((iconType == MapTrackerIconType.CustomImage) != (image != null))
                throw new ArgumentOutOfRangeException();

            _iconType = iconType;
            _customImage = image;
        }

        public abstract MapDrawType GetDrawType();

        public virtual float GetY()
        {
            PositionAngle posAngle = GetPositionAngle();
            if (posAngle == null) return float.PositiveInfinity;
            return (float)posAngle.Y;
        }

        public void NotifyStoreBehaviorCritera()
        {
            ObjectDataModel obj = GetObject();
            if (obj == null) return;
            obj.Update();
            _behaviorCriteriaToDisplay = obj.BehaviorCriteria;
        }

        public bool ShouldDisplay(MapTrackerVisibilityType visiblityType)
        {
            ObjectDataModel obj = GetObject();
            if (obj == null) return true;
            obj.Update();
            switch (visiblityType)
            {
                case MapTrackerVisibilityType.VisibleAlways:
                    return true;
                case MapTrackerVisibilityType.VisibleWhenLoaded:
                    return obj.IsActive;
                case MapTrackerVisibilityType.VisibleWhenThisBhvrIsLoaded:
                    return obj.IsActive && BehaviorCriteria.HasSameAssociation(obj.BehaviorCriteria, _behaviorCriteriaToDisplay);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual PositionAngle GetPositionAngle()
        {
            return null;
        }

        public virtual ObjectDataModel GetObject()
        {
            PositionAngle posAngle = GetPositionAngle();
            if (posAngle == null) return null;
            if (!posAngle.IsObjectDependent()) return null;
            uint objAddress = posAngle.GetObjectAddressIfObjectDependent().Value;
            return new ObjectDataModel(objAddress, true);
        }

        public override string ToString()
        {
            return GetName();
        }

        public virtual ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem item = new ToolStripMenuItem("There are no additional options");
                item.Enabled = false;
                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(item);
            }

            return _contextMenuStrip;
        }

        public virtual void Update()
        {
        }

        public virtual bool ParticipatesInGlobalIconSize()
        {
            return false;
        }

        public virtual void ApplySettings(MapObjectSettings settings)
        {
            GetContextMenuStrip();

            _accumulator.ApplySettings(settings);
        }

        protected MapTracker GetParentMapTracker()
        {
            foreach (MapTracker mapTracker in Config.MapGui.flowLayoutPanelMapTrackers.Controls)
            {
                if (mapTracker.ContainsMapObject(this)) return mapTracker;
            }
            return null;
        }

        public virtual void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY)
        {
        }

        public virtual void CleanUp()
        {
        }

        public virtual List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>();
        }

        public XElement ToXElement()
        {
            XElement xElement = new XElement("MapObject");
            xElement.Add(new XAttribute("type", GetType().Name));
            List<XAttribute> xAttributes = GetXAttributes();
            foreach (XAttribute xAttribute in xAttributes)
            {
                xElement.Add(xAttribute);
            }
            xElement.Add(_accumulator.ToXElement());
            return xElement;
        }

        public MapObject FromXElement(XElement xElement)
        {
            string type = xElement.Attribute(XName.Get("type")).Value;
            switch (type)
            {
                case "MapObjectAggregatedPath":
                    return new MapObjectAggregatedPath();
                case "MapObjectAllObjectCeiling":
                    return new MapObjectAllObjectCeiling();
                case "MapObjectAllObjectFloor":
                    return new MapObjectAllObjectFloor();
                case "MapObjectAllObjectsWithName":
                    return MapObjectAllObjectsWithName.Create(xElement.Attribute(XName.Get("objectName")).Value);
                //case "MapObjectAllObjectWall":
                //    return new MapObjectAllObjectWall();
                //case "MapObjectAngleRange":
                //    return new MapObjectAngleRange();
                //case "MapObjectBranchPath":
                //    return new MapObjectBranchPath();
                //case "MapObjectCamera":
                //    return new MapObjectCamera();
                //case "MapObjectCellGridlines":
                //    return new MapObjectCellGridlines();
                //case "MapObjectCoffinBox":
                //    return new MapObjectCoffinBox();
                //case "MapObjectCompass":
                //    return new MapObjectCompass();
                //case "MapObjectCoordinateLabels":
                //    return new MapObjectCoordinateLabels();
                //case "MapObjectCUpFloor":
                //    return new MapObjectCUpFloor();
                //case "MapObjectCurrentBackground":
                //    return new MapObjectCurrentBackground();
                //case "MapObjectCurrentCell":
                //    return new MapObjectCurrentCell();
                //case "MapObjectCurrentMap":
                //    return new MapObjectCurrentMap();
                //case "MapObjectCurrentUnit":
                //    return new MapObjectCurrentUnit();
                //case "MapObjectCustomBackground":
                //    return new MapObjectCustomBackground();
                //case "MapObjectCustomCeiling":
                //    return new MapObjectCustomCeiling();
                //case "MapObjectCustomCylinder":
                //    return new MapObjectCustomCylinder();
                //case "MapObjectCustomCylinderPoints":
                //    return new MapObjectCustomCylinderPoints();
                //case "MapObjectCustomFloor":
                //    return new MapObjectCustomFloor();
                //case "MapObjectCustomGridlines":
                //    return new MapObjectCustomGridlines();
                //case "MapObjectCustomMap":
                //    return new MapObjectCustomMap();
                //case "MapObjectCustomPositionAngle":
                //    return new MapObjectCustomPositionAngle();
                //case "MapObjectCustomPositionAngleArrow":
                //    return new MapObjectCustomPositionAngleArrow();
                //case "MapObjectCustomSphere":
                //    return new MapObjectCustomSphere();
                //case "MapObjectCustomSpherePoints":
                //    return new MapObjectCustomSpherePoints();
                //case "MapObjectCustomUnitPoints":
                //    return new MapObjectCustomUnitPoints();
                //case "MapObjectCustomWall":
                //    return new MapObjectCustomWall();
                //case "MapObjectDrawDistanceSphere":
                //    return new MapObjectDrawDistanceSphere();
                //case "MapObjectDrawing":
                //    return new MapObjectDrawing();
                //case "MapObjectEffectiveHitboxCylinder":
                //    return new MapObjectEffectiveHitboxCylinder();
                //case "MapObjectEffectiveHurtboxCylinder":
                //    return new MapObjectEffectiveHurtboxCylinder();
                //case "MapObjectFacingDivider":
                //    return new MapObjectFacingDivider();
                //case "MapObjectFloatGridlines":
                //    return new MapObjectFloatGridlines();
                //case "MapObjectGhost":
                //    return new MapObjectGhost();
                //case "MapObjectHitboxCylinder":
                //    return new MapObjectHitboxCylinder();
                //case "MapObjectHitboxHackTriangle":
                //    return new MapObjectHitboxHackTriangle();
                //case "MapObjectHolp":
                //    return new MapObjectHolp();
                //case "MapObjectHome":
                //    return new MapObjectHome();
                //case "MapObjectHomeLine":
                //    return new MapObjectHomeLine();
                //case "MapObjectHurtboxCylinder":
                //    return new MapObjectHurtboxCylinder();
                //case "MapObjectIwerlipses":
                //    return new MapObjectIwerlipses();
                //case "MapObjectLedgeGrabChecker":
                //    return new MapObjectLedgeGrabChecker();
                //case "MapObjectLevelCeiling":
                //    return new MapObjectLevelCeiling();
                //case "MapObjectLevelFloor":
                //    return new MapObjectLevelFloor();
                //case "MapObjectLevelWall":
                //    return new MapObjectLevelWall();
                //case "MapObjectLineSegment":
                //    return new MapObjectLineSegment();
                //case "MapObjectMario":
                //    return new MapObjectMario();
                //case "MapObjectMarioCeiling":
                //    return new MapObjectMarioCeiling();
                //case "MapObjectMarioFacingArrow":
                //    return new MapObjectMarioFacingArrow();
                //case "MapObjectMarioFloor":
                //    return new MapObjectMarioFloor();
                //case "MapObjectMarioFloorArrow":
                //    return new MapObjectMarioFloorArrow();
                //case "MapObjectMarioIntendedArrow":
                //    return new MapObjectMarioIntendedArrow();
                //case "MapObjectMarioMovingArrow":
                //    return new MapObjectMarioMovingArrow();
                //case "MapObjectMarioSlidingArrow":
                //    return new MapObjectMarioSlidingArrow();
                //case "MapObjectMarioTwirlArrow":
                //    return new MapObjectMarioTwirlArrow();
                //case "MapObjectMarioWall":
                //    return new MapObjectMarioWall();
                //case "MapObjectNextPositions":
                //    return new MapObjectNextPositions();
                //case "MapObjectObject":
                //    return new MapObjectObject();
                //case "MapObjectObjectAngleToMarioArrow":
                //    return new MapObjectObjectAngleToMarioArrow();
                //case "MapObjectObjectCeiling":
                //    return new MapObjectObjectCeiling();
                //case "MapObjectObjectCustomArrow":
                //    return new MapObjectObjectCustomArrow();
                //case "MapObjectObjectFacingArrow":
                //    return new MapObjectObjectFacingArrow();
                //case "MapObjectObjectFloor":
                //    return new MapObjectObjectFloor();
                //case "MapObjectObjectGraphicsArrow":
                //    return new MapObjectObjectGraphicsArrow();
                //case "MapObjectObjectMovingArrow":
                //    return new MapObjectObjectMovingArrow();
                //case "MapObjectObjectWall":
                //    return new MapObjectObjectWall();
                //case "MapObjectPath":
                //    return new MapObjectPath();
                //case "MapObjectPoint":
                //    return new MapObjectPoint();
                //case "MapObjectPreviousPositions":
                //    return new MapObjectPreviousPositions();
                //case "MapObjectPuGridlines":
                //    return new MapObjectPuGridlines();
                //case "MapObjectPunchDetector":
                //    return new MapObjectPunchDetector();
                //case "MapObjectPunchFloor":
                //    return new MapObjectPunchFloor();
                //case "MapObjectPushHitboxCylinder":
                //    return new MapObjectPushHitboxCylinder();
                //case "MapObjectSector":
                //    return new MapObjectSector();
                //case "MapObjectSelf":
                //    return new MapObjectSelf();
                //case "MapObjectSwooperEffectiveTargetArrow":
                //    return new MapObjectSwooperEffectiveTargetArrow();
                //case "MapObjectTangibilitySphere":
                //    return new MapObjectTangibilitySphere();
                //case "MapObjectUnitGridlines":
                //    return new MapObjectUnitGridlines();
                //case "MapObjectWaters":
                //    return new MapObjectWaters();
                default:
                    throw new ArgumentOutOfRangeException("Unknown MapObject type: " + type);
            }
        }
    }
}
