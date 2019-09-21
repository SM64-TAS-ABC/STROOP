using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Structs;
using STROOP.Models;
using STROOP.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using STROOP.Structs.Configurations;
using STROOP.Controls.Map;
using STROOP.Map3;

namespace STROOP.Managers
{
    public class Map3Manager
    {
        Map3Graphics _mapGraphics;

        Map3Object _background;
        Map3Object _gridlines;
        Map3Object _map;
        Map3Object _holpMapObj;
        Map3Object _marioMapObj;

        Map3Gui _mapGui;
        bool _isLoaded = false;

        public Map3Manager(Map3Gui mapGui)
        {
            _mapGui = mapGui;
        }

        public void Load()
        {
            // Create new graphics control
            _mapGraphics = new Map3Graphics(_mapGui.GLControl);
            _mapGraphics.Load();

            _background = new Map3BackgroundObject(_mapGraphics);
            _gridlines = new Map3GridlinesObject(_mapGraphics);
            _map = new Map3MapObject(_mapGraphics);
            _holpMapObj = new Map3HolpObject(_mapGraphics);
            _marioMapObj = new Map3MarioObject(_mapGraphics);

            _isLoaded = true;

            // Add map objects
            _mapGraphics.AddMapObject(_background);
            _mapGraphics.AddMapObject(_gridlines);
            _mapGraphics.AddMapObject(_map);
            _mapGraphics.AddMapObject(_holpMapObj);
            _mapGraphics.AddMapObject(_marioMapObj);
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
            if (!_isLoaded) return;

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }
    }
}
