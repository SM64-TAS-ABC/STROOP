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
using STROOP.Controls.Map;
using STROOP.Controls.Map.Objects;

namespace STROOP.Managers
{
    public class MapManager
    {        
        public bool IsLoaded { get; private set; }
        public bool Visible { get => _graphics.Visible; set => _graphics.Visible = value; }

        MapGui _mapGui;
        MapController _controller;
        MapGraphics _graphics;
        MapAssociations _mapAssoc;

        public MapManager(MapAssociations mapAssoc, MapGui mapGui)
        {
            _mapAssoc = mapAssoc;
            _mapGui = mapGui;
        }

        public void Load()
        {
            // Create new graphics control
            _graphics = new MapGraphics(_mapGui.GLControl);
            _graphics.Load();
            _controller = new MapController(_graphics);

            IsLoaded = true;

            // Test
            _controller.AddMapObject(new MapLevelObject(_mapAssoc));
            _controller.AddMapObject(new MapMarioObject());
        }

        public void Update()
        {
            // Make sure the control has successfully loaded
            if (!IsLoaded)
                return;
            
            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _controller.Update();

            // Update labels
            /*_mapGui.PuValueLabel.Text = string.Format("[{0}:{1}:{2}]", puX, puY, puZ);
            _mapGui.QpuValueLabel.Text = string.Format("[{0}:{1}:{2}]", qpuX, qpuY, qpuZ);
            _mapGui.MapIdLabel.Text = string.Format("[{0}:{1}:{2}:{3}]", level, area, loadingPoint, missionLayout);
            _mapGui.MapNameLabel.Text = _currentMap.Name;
            _mapGui.MapSubNameLabel.Text = (_currentMap.SubName != null) ? _currentMap.SubName : "";*/
        }
    }
}
