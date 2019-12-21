using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Controls.Map.Graphics.Items
{
    public abstract class Map4GraphicsItem
    {
        /// <summary>
        /// Should the item be drawn
        /// </summary>
        public bool Visible { get; set; }

        public virtual Bitmap BitmapImage { get; set; }

        /// <summary>
        /// Drawing type
        /// </summary>
        public enum DrawType
        {
            Perspective,
            Overlay,
            Background,
        };

        public abstract DrawType Type { get; }

        /// <summary>
        /// List of camera types the object should be drawn on
        /// </summary>
        public abstract IEnumerable<Type> DrawOnCameraTypes { get; }

        /// <summary>
        /// The priority which the topdown item is shown. Larger is lower, behind smaller-valued items.
        /// Make null for 3D items.
        /// </summary>
        public abstract float? Depth { get; }

        public Map4GraphicsItem(bool visible)
        {
            Visible = visible;
        }

        /// <summary>
        /// Model transformtation matrix
        /// </summary>
        public virtual Matrix4 GetModelMatrix(Map4Graphics graphics)
        {
            return Matrix4.Identity;
        }
   
        /// <summary>
        /// Called when the item is loaded and added to the map graphics.
        /// Load texture here.
        /// </summary>
        /// <param name="graphics"></param>
        public abstract void Load(Map4Graphics graphics);

        /// <summary>
        /// Draw the item onto the map
        /// </summary>
        /// <param name="graphics"></param>
        public abstract void Draw(Map4Graphics graphics);

        /// <summary>
        /// Dispose of the item. Remove all textures here
        /// <param name="graphics"></param>
        /// </summary>
        public abstract void Unload(Map4Graphics graphics);

        protected static List<Type> CameraTypeAny = new List<Type>()
        {
            typeof(MapCameraTopView),

        };
    }
}
