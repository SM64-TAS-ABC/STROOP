using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map.Graphics.Items;
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
        /// <summary>
        /// List of graphics items for the object
        /// </summary>
        public abstract IEnumerable<MapGraphicsItem> GraphicsItems { get; }

        /// <summary>
        /// Is the item visible or not 
        /// </summary>
        private bool _visible = true;
        public bool Visible
        {
            get => _visible;
            set
            {
                _visible = value;
                foreach (MapGraphicsItem item in GraphicsItems)
                {
                    item.Visible = value;
                }
            }
        }

        /// <summary>
        /// Called when the map state is updated
        /// </summary>
        public abstract void Update();
    }
}
