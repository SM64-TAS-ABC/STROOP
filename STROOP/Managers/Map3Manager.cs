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
        Map3Object _background;
        Map3Object _gridlines;
        Map3Object _map;
        Map3Object _holpMapObj;
        Map3Object _cameraMapObj;
        Map3Object _marioMapObj;
        Map3Object _floorMapObj;

        Map3Gui _mapGui;
        bool _isLoaded = false;

        public Map3Manager(Map3Gui mapGui)
        {
            _mapGui = mapGui;
        }

        public void Load()
        {
            // Create new graphics control
            Config.Map3Graphics = new Map3Graphics(_mapGui.GLControl);
            Config.Map3Graphics.Load();
            _isLoaded = true;

            _background = new Map3BackgroundObject();
            _gridlines = new Map3GridlinesObject();
            _map = new Map3MapObject();
            _holpMapObj = new Map3HolpObject();
            _cameraMapObj = new Map3CameraObject();
            _marioMapObj = new Map3MarioObject();
            _floorMapObj = new Map3FloorObject();

            // Add map objects
            Config.Map3Graphics.AddMapObject(_background);
            Config.Map3Graphics.AddMapObject(_gridlines);
            Config.Map3Graphics.AddMapObject(_map);
            Config.Map3Graphics.AddMapObject(_holpMapObj);
            Config.Map3Graphics.AddMapObject(_cameraMapObj);
            Config.Map3Graphics.AddMapObject(_marioMapObj);
            Config.Map3Graphics.AddMapObject(_floorMapObj);
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
            if (!_isLoaded) return;

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            Config.Map3Graphics.Control.Invalidate();
        }
    }
}
