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
        MapGui _mapGui;
        List<MapObject> _mapObjects = new List<MapObject>();
        MapGraphics _mapGraphics;

        public bool IsLoaded { get; private set; } = false;

        public bool Visible { get => _mapGraphics.Control.Visible; set => _mapGraphics.Control.Visible = value; }

        public MapController(MapGui mapGui)
        {
            _mapGui = mapGui;
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new MapGraphics(_mapGui.GLControl);
            _mapGraphics.Load();

            IsLoaded = true;
        }

        public void Update()
        {
            // Make sure the control has successfully loaded
            if (!IsLoaded)
                return;

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }
        
        public void AddMapObject(MapObject mapObj)
        {
            _mapObjects.Add(mapObj);
            foreach (MapGraphicsItem graphicsItem in mapObj.GraphicsItems)
                _mapGraphics.AddMapItem(graphicsItem);
        }

        public void RemoveMapObject(MapObject mapObj)
        {
            _mapObjects.Remove(mapObj);
            foreach (MapGraphicsItem graphicsItem in mapObj.GraphicsItems)
                _mapGraphics.RemoveMapObject(graphicsItem);
        }
    }
}
