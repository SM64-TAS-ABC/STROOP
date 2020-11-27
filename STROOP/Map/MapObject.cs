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

        public MapObject()
        {
        }

        public void DrawOn2DControl()
        {
            if (Config.MapGui.checkBoxMapOptionsEnableSideView.Checked)
            {
                DrawOn2DControlSideView();
            }
            else
            {
                DrawOn2DControlTopDownView();
            }
        }

        public abstract void DrawOn2DControlTopDownView();

        public void DrawOn2DControlSideView() { } // TODO: make abstract

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
    }
}
