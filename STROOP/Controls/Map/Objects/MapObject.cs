using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Controls.Map.Trackers;
using STROOP.Structs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map
{
    public abstract class MapObject
    {
        public MapTracker Tracker;

        /// <summary>
        /// List of graphics items for the object
        /// </summary>
        public abstract IEnumerable<MapGraphicsItem> GraphicsItems { get; }

        /** Whether the object is on the tracked list or is implicitly tracked. */
        private bool _tracked = false;
        public bool Tracked
        {
            get => _tracked;
            set
            {
                _tracked = value;
                UpdateItemVisibility();
            }
        }

        /** Whether the object has its eye icon open. */
        private bool _shown = true;
        public bool Shown
        {
            get => _shown;
            set
            {
                _shown = value;
                UpdateItemVisibility();
            }
        }

        /** Whether the object is internally meant to be displayed.
         *  For example, whether an object is loaded or a triangle address is non-zero.
         */
        private bool _displayed = true;
        public bool Displayed
        {
            get => _displayed;
            set
            {
                _displayed = value;
                UpdateItemVisibility();
            }
        }

        /// <summary>
        /// Is the item visible or not 
        /// </summary>
        public bool Visible
        {
            get => _tracked && _shown && _displayed;
        }

        private void UpdateItemVisibility()
        {
            foreach (MapGraphicsItem item in GraphicsItems)
            {
                item.Visible = Visible;
            }
        }

        public virtual float Opacity { get; set; }
        public virtual float Size { get; set; }
        public virtual Color? MyColor { get; set; }
        public virtual int DisplayLayer { get; set; }
        public virtual MapTrackerVisibilityType VisibilityType { get; set; } = MapTrackerVisibilityType.VisibleWhenLoaded;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Tracker?.UpdateName(_name);
            }
        }

        private Bitmap _bitmapImage;
        public virtual Bitmap BitmapImage
        {
            get => _bitmapImage;
            set
            {
                _bitmapImage = value;
                Tracker?.UpdateImage(_bitmapImage);
            }
        }

        private Color? _backColor;
        public Color? BackColor
        {
            get => _backColor;
            set
            {
                if (_backColor == value) return;
                _backColor = value;
                Tracker?.UpdateBackColor(_backColor);
            }
        }

        private bool _rotates;
        public bool Rotates
        {
            get => _rotates;
            set
            {
                if (_rotates == value) return;
                _rotates = value;
                Tracker?.SetRotates(_rotates);
            }
        }

        public virtual double GetDepth()
        {
            return 0;
        }

        public MapObject(string name, Bitmap bitmapImage, Color? backColor, bool rotates, Color? color)
        {
            Name = name;
            BitmapImage = bitmapImage;
            BackColor = backColor;
            Rotates = rotates;
            MyColor = color;
        }

        /// <summary>
        /// Called when the map state is updated
        /// </summary>
        public abstract void Update();
    }
}
