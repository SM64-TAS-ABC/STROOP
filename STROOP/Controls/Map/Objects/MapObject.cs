using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Controls.Map.Trackers;
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

        /// <summary>
        /// Is the item visible or not 
        /// </summary>
        public bool Visible
        {
            get => _tracked && _shown;
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
        public virtual bool Rotates { get; set; }
        public virtual int DisplayLayer { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                Tracker?.UpdateName();
            }
        }

        public Bitmap BitmapImage
        {
            get => GraphicsItems.FirstOrDefault()?.BitmapImage;
        }

        public virtual double GetDepth()
        {
            return 0;
        }

        public MapObject(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Called when the map state is updated
        /// </summary>
        public abstract void Update();
    }
}
