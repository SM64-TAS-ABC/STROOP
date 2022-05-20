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
            set => Opacity = value / 255.0;
        }
        public int OpacityPercent
        {
            get => (int)(Opacity * 100);
            set => Opacity = value / 100.0;
        }
        public float LineWidth = 1;
        public Color Color = SystemColors.Control;
        public Color4 Color4 { get => new Color4(Color.R, Color.G, Color.B, OpacityByte); }
        public Color LineColor = Color.Black;

        public bool? CustomRotates = null;
        public bool InternalRotates = false;
        public bool Rotates
        {
            get => CustomRotates ?? InternalRotates;
        }

        private BehaviorCriteria? _behaviorCriteriaToDisplay = null;

        public bool Scales = false;

        public bool UseRelativeCoordinates = false;

        protected ContextMenuStrip _contextMenuStrip = null;

        private MapObjectSettingsAccumulator _accumulator = new MapObjectSettingsAccumulator();

        public MapObject()
        {
        }

        public void DrawOn2DControl(MapObjectHoverData hoverData)
        {
            if (Config.CurrentMapGraphics.IsOrthographicViewEnabled)
            {
                DrawOn2DControlOrthographicView(hoverData);
            }
            else
            {
                DrawOn2DControlTopDownView(hoverData);
            }
        }

        public abstract void DrawOn2DControlTopDownView(MapObjectHoverData hoverData);

        public abstract void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData);

        public abstract void DrawOn3DControl();

        public virtual Matrix4 GetModelMatrix()
        {
            return Matrix4.Identity;
        }

        public abstract string GetName();

        public Image _customImage = null;
        public string _customImagePath = null;
        public int? _customImageTex = null;
        public abstract Image GetInternalImage();
        public Image GetImage() { return _customImage ?? GetInternalImage(); }

        protected MapTrackerIconType _iconType = MapTrackerIconType.TopDownImage;
        public virtual void SetIconType(MapTrackerIconType iconType, Image image = null, string path = null)
        {
            if ((iconType == MapTrackerIconType.CustomImage) != (image != null))
                throw new ArgumentOutOfRangeException();

            _iconType = iconType;
            _customImage = image;
            _customImagePath = path;

            if (_customImage != null)
            {
                _customImageTex = MapUtilities.LoadTexture(_customImage as Bitmap);
            }
            else
            {
                _customImageTex = null;
            }
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
            uint objAddress = posAngle.GetObjAddress();
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

        public virtual (double x, double y, double z)? GetDragPosition()
        {
            return null;
        }

        public virtual void SetDragPositionTopDownView(double? x = null, double? y = null, double? z = null)
        {
            // do nothing
        }

        public virtual void SetDragPositionOrthographicView(double? x = null, double? y = null, double? z = null)
        {
            // do nothing
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

        public virtual void NotifyMouseEvent(MouseEvent mouseEvent, bool isLeftButton, int mouseX, int mouseY, GLControl control)
        {
        }

        public virtual void CleanUp()
        {
        }

        public MapObjectHoverData GetHoverData(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (Config.CurrentMapGraphics.IsOrthographicViewEnabled)
            {
                return GetHoverDataOrthographicView(isForObjectDrag, forceCursorPosition);
            }
            else
            {
                return GetHoverDataTopDownView(isForObjectDrag, forceCursorPosition);
            }
        }

        public virtual MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            return null;
        }

        public virtual MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            return null;
        }

        public virtual List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            return new List<ToolStripItem>();
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

        public static MapObject FromXElement(XElement xElement)
        {
            string type = xElement.Attribute(XName.Get("type")).Value;
            MapObject mapObject;
            switch (type)
            {
                case "MapObjectAggregatedPath":
                    mapObject = new MapObjectAggregatedPath();
                    break;
                case "MapObjectAllObjectCeiling":
                    mapObject = new MapObjectAllObjectCeiling();
                    break;
                case "MapObjectAllObjectFloor":
                    mapObject = new MapObjectAllObjectFloor();
                    break;
                case "MapObjectAllObjectsWithName":
                    mapObject = new MapObjectAllObjectsWithName(xElement.Attribute(XName.Get("objectName")).Value);
                    break;
                case "MapObjectAllObjectWall":
                    mapObject = new MapObjectAllObjectWall();
                    break;
                case "MapObjectAngleRange":
                    mapObject = new MapObjectAngleRange(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectBranchPath":
                    mapObject = MapObjectBranchPath.Create(
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value),
                        xElement.Attribute(XName.Get("points")).Value);
                    break;
                case "MapObjectCamera":
                    mapObject = new MapObjectCamera();
                    break;
                case "MapObjectCameraFocus":
                    mapObject = new MapObjectCameraFocus();
                    break;
                case "MapObjectCameraView":
                    mapObject = new MapObjectCameraView();
                    break;
                case "MapObjectCellGridlines":
                    mapObject = new MapObjectCellGridlines();
                    break;
                case "MapObjectCoffinBox":
                    mapObject = new MapObjectCoffinBox(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectCompass":
                    mapObject = new MapObjectCompass();
                    break;
                case "MapObjectCoordinateLabels":
                    mapObject = new MapObjectCoordinateLabels();
                    break;
                case "MapObjectCorkBoxTester":
                    mapObject = new MapObjectCorkBoxTester();
                    break;
                case "MapObjectCUpFloor":
                    mapObject = new MapObjectCUpFloor();
                    break;
                case "MapObjectCurrentBackground":
                    mapObject = new MapObjectCurrentBackground();
                    break;
                case "MapObjectCurrentCell":
                    mapObject = new MapObjectCurrentCell(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectCurrentLevel":
                    mapObject = new MapObjectCurrentLevel();
                    break;
                case "MapObjectCurrentUnit":
                    mapObject = new MapObjectCurrentUnit(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectCustomBackground":
                    mapObject = new MapObjectCustomBackground();
                    break;
                case "MapObjectCustomCeiling":
                    mapObject = MapObjectCustomCeiling.Create(xElement.Attribute(XName.Get("triangles")).Value);
                    break;
                case "MapObjectCustomCylinder":
                    mapObject = new MapObjectCustomCylinder(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectCustomCylinderPoints":
                    mapObject = MapObjectCustomCylinderPoints.Create(xElement.Attribute(XName.Get("points")).Value, true);
                    break;
                case "MapObjectCustomFloor":
                    mapObject = MapObjectCustomFloor.Create(xElement.Attribute(XName.Get("triangles")).Value);
                    break;
                case "MapObjectCustomGridlines":
                    mapObject = new MapObjectCustomGridlines();
                    break;
                case "MapObjectCustomIconPoints":
                    mapObject = MapObjectCustomIconPoints.Create(xElement.Attribute(XName.Get("points")).Value, true);
                    break;
                case "MapObjectCustomLevel":
                    mapObject = new MapObjectCustomLevel();
                    break;
                case "MapObjectCustomPositionAngle":
                    mapObject = new MapObjectCustomPositionAngle(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectCustomPositionAngleArrow":
                    mapObject = new MapObjectCustomPositionAngleArrow(
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle1")).Value),
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle2")).Value));
                    break;
                case "MapObjectCustomSphere":
                    mapObject = new MapObjectCustomSphere(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectCustomSpherePoints":
                    mapObject = MapObjectCustomSpherePoints.Create(xElement.Attribute(XName.Get("points")).Value, true);
                    break;
                case "MapObjectCustomUnitPoints":
                    mapObject = MapObjectCustomUnitPoints.Create(xElement.Attribute(XName.Get("points")).Value, false);
                    break;
                case "MapObjectCustomWall":
                    mapObject = MapObjectCustomWall.Create(xElement.Attribute(XName.Get("triangles")).Value);
                    break;
                case "MapObjectDrawDistanceSphere":
                    mapObject = new MapObjectDrawDistanceSphere(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectDrawing":
                    mapObject = MapObjectDrawing.Create(xElement.Attribute(XName.Get("points")).Value);
                    break;
                case "MapObjectEffectiveHitboxCylinder":
                    mapObject = new MapObjectEffectiveHitboxCylinder(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectEffectiveHurtboxCylinder":
                    mapObject = new MapObjectEffectiveHurtboxCylinder(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectFacingDivider":
                    mapObject = new MapObjectFacingDivider(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectFloatGridlines":
                    mapObject = new MapObjectFloatGridlines();
                    break;
                case "MapObjectGhost":
                    mapObject = new MapObjectGhost();
                    break;
                case "MapObjectHitboxCylinder":
                    mapObject = new MapObjectHitboxCylinder(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectHitboxHackTriangle":
                    mapObject = new MapObjectHitboxTriangle(false);
                    break;
                case "MapObjectHolp":
                    mapObject = new MapObjectHolp();
                    break;
                case "MapObjectHome":
                    mapObject = new MapObjectHome(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectHomeLine":
                    mapObject = new MapObjectHomeLine(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectHurtboxCylinder":
                    mapObject = new MapObjectHurtboxCylinder(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectIwerlipses":
                    mapObject = new MapObjectIwerlipses();
                    break;
                case "MapObjectLedgeGrabChecker":
                    mapObject = new MapObjectLedgeGrabChecker();
                    break;
                case "MapObjectLevelCeiling":
                    mapObject = MapObjectLevelCeiling.Create(xElement.Attribute(XName.Get("triangles")).Value);
                    break;
                case "MapObjectLevelFloor":
                    mapObject = MapObjectLevelFloor.Create(xElement.Attribute(XName.Get("triangles")).Value);
                    break;
                case "MapObjectLevelWall":
                    mapObject = MapObjectLevelWall.Create(xElement.Attribute(XName.Get("triangles")).Value);
                    break;
                case "MapObjectLineSegment":
                    mapObject = new MapObjectLineSegment(
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle1")).Value),
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle2")).Value));
                    break;
                case "MapObjectMario":
                    mapObject = new MapObjectMario();
                    break;
                case "MapObjectMarioCeiling":
                    mapObject = new MapObjectMarioCeiling();
                    break;
                case "MapObjectMarioFacingArrow":
                    mapObject = new MapObjectMarioFacingArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioFloor":
                    mapObject = new MapObjectMarioFloor();
                    break;
                case "MapObjectMarioFloorArrow":
                    mapObject = new MapObjectMarioFloorArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioIntendedArrow":
                    mapObject = new MapObjectMarioIntendedArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioMovingArrow":
                    mapObject = new MapObjectMarioMovingArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioSlidingArrow":
                    mapObject = new MapObjectMarioSlidingArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioSpeedArrow":
                    mapObject = new MapObjectMarioSpeedArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioTwirlArrow":
                    mapObject = new MapObjectMarioTwirlArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectMarioWall":
                    mapObject = new MapObjectMarioWall();
                    break;
                case "MapObjectNextPositions":
                    mapObject = new MapObjectNextPositions();
                    break;
                case "MapObjectObject":
                    mapObject = new MapObjectObject(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectAngleToMarioArrow":
                    mapObject = new MapObjectObjectAngleToMarioArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectCeiling":
                    mapObject = new MapObjectObjectCeiling(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectCustomArrow":
                    mapObject = new MapObjectObjectCustomArrow(
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value),
                        ParsingUtilities.ParseHex(xElement.Attribute(XName.Get("yawOffset")).Value),
                        ParsingUtilities.ParseInt(xElement.Attribute(XName.Get("numBytes")).Value));
                    break;
                case "MapObjectObjectFacingArrow":
                    mapObject = new MapObjectObjectFacingArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectFloor":
                    mapObject = new MapObjectObjectFloor(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectGraphicsArrow":
                    mapObject = new MapObjectObjectGraphicsArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectMovingArrow":
                    mapObject = new MapObjectObjectMovingArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectSpeedArrow":
                    mapObject = new MapObjectObjectSpeedArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectObjectWall":
                    mapObject = new MapObjectObjectWall(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectPath":
                    mapObject = MapObjectPath.Create(
                        PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value),
                        xElement.Attribute(XName.Get("points")).Value);
                    break;
                case "MapObjectPoint":
                    mapObject = new MapObjectPoint();
                    break;
                case "MapObjectPreviousPositions":
                    mapObject = MapObjectPreviousPositions.Create(xElement.Attribute(XName.Get("points")).Value);
                    break;
                case "MapObjectPuGridlines":
                    mapObject = new MapObjectPuGridlines();
                    break;
                case "MapObjectPunchDetector":
                    mapObject = new MapObjectPunchDetector();
                    break;
                case "MapObjectPunchFloor":
                    mapObject = new MapObjectPunchFloor();
                    break;
                case "MapObjectPushHitboxCylinder":
                    mapObject = new MapObjectPushHitboxCylinder(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectSector":
                    mapObject = new MapObjectSector(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectSelf":
                    mapObject = new MapObjectSelf();
                    break;
                case "MapObjectSwooperEffectiveTargetArrow":
                    mapObject = new MapObjectSwooperEffectiveTargetArrow(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectTangibilitySphere":
                    mapObject = new MapObjectTangibilitySphere(PositionAngle.FromString(xElement.Attribute(XName.Get("positionAngle")).Value));
                    break;
                case "MapObjectUnitGridlines":
                    mapObject = new MapObjectUnitGridlines();
                    break;
                case "MapObjectWaters":
                    mapObject = new MapObjectWaters();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown MapObject type: " + type);
            }

            XElement settingsXElement = xElement.Element(XName.Get("Settings"));
            MapObjectSettings settings = MapObjectSettings.FromXElement(settingsXElement);
            mapObject.ApplySettings(settings);

            return mapObject;
        }
    }
}
