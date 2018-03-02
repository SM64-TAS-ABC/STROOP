using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Structs.Configurations;
using STROOP.Controls.Map.Graphics;
using STROOP.Controls.Map.Graphics.Items;

namespace STROOP.Controls.Map
{
    public class MapController
    {
        List<MapObject> _mapObjects = new List<MapObject>();
        MapGraphics _graphics;

        public MapController(MapGraphics graphics)
        {
            _graphics = graphics;
        }
        
        public void AddMapObject(MapObject mapObj)
        {
            _mapObjects.Add(mapObj);
            foreach (MapGraphicsItem graphicsItem in mapObj.GraphicsItems)
                _graphics.AddMapItem(graphicsItem);
        }

        public void Update()
        {
            foreach (MapObject obj in _mapObjects)
                obj.Update();

            _graphics.Invalidate();
        }

        public void RemoveMapObject(MapObject mapObj)
        {
            _mapObjects.Remove(mapObj);
            foreach (MapGraphicsItem graphicsItem in mapObj.GraphicsItems)
                _graphics.RemoveMapObject(graphicsItem);
        }
    }
}
