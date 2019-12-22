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
using System.Windows.Forms;
using OpenTK.Graphics;

namespace STROOP.Map3
{
    public abstract class Map3Object
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

        public bool ShowTriUnits = false;

        protected ContextMenuStrip _contextMenuStrip = null;

        public Map3Object()
        {
        }

        public abstract void DrawOn2DControl();

        public virtual void DrawOn3DControl() // TODO make abstract
        {
        }

        public virtual Matrix4 GetModelMatrix()
        {
            return Matrix4.Identity;
        }

        public abstract string GetName();

        public abstract Image GetImage();

        public virtual float GetY()
        {
            return float.PositiveInfinity;
        }

        public virtual bool ShouldDisplay(MapTrackerVisibilityType visiblityType)
        {
            return true;
        }

        public virtual void NotifyStoreBehaviorCritera()
        {
        }

        public virtual PositionAngle GetPositionAngle()
        {
            return null;
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
    }
}
