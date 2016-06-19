using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System.Windows.Forms;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MapManager
    {
        ProcessStream _stream;
        Config _config;
        MapAssociations _assoc;
        byte _currentLevel, _currentArea;
        Map _currentMap;
        List<Map> _currentMapList = null;
        MapGraphicsControl _mapGraphics;
        MapObject _marioMapObj;

        public bool Visible
        {
            get
            {
                return _mapGraphics.Control.Visible;
            }
            set
            {
                _mapGraphics.Control.Visible = value;
            }
        }

        public MapManager(ProcessStream stream, Config config, MapAssociations mapAssoc,
            GLControl mapControl)
        {
            _stream = stream;
            _config = config;
            _assoc = mapAssoc;

            _mapGraphics = new MapGraphicsControl(mapControl);
            _mapGraphics.Load();

            _marioMapObj = new MapObject(new Bitmap("Resources\\Object Images\\Mario.png"), new PointF());
            _mapGraphics.AddMapObject(_marioMapObj);
            ChangeCurrentMap(_assoc.DefaultMap);
        }



        public void Update()
        {
            // Get Mario's coordinates
            var marioCoord = new float[3];
            marioCoord[0] = BitConverter.ToSingle(_stream.ReadRam(_config.Mario.XOffset + _config.Mario.MarioPointerAddress, 4), 0);
            marioCoord[1] = BitConverter.ToSingle(_stream.ReadRam(_config.Mario.YOffset + _config.Mario.MarioPointerAddress, 4), 0);
            marioCoord[2] = BitConverter.ToSingle(_stream.ReadRam(_config.Mario.ZOffset + _config.Mario.MarioPointerAddress, 4), 0);

            // Get level and area
            byte level = _stream.ReadRam(_config.LevelAddress, 1)[0];
            byte area = _stream.ReadRam(_config.AreaAddress, 1)[0];

            // Find new map list
            if (_currentMapList == null || _currentLevel != level || _currentArea != area)
            {
                _currentLevel = level;
                _currentArea = area;
                _currentMapList = _assoc.GetLevelAreaMaps(level, area).Where((map) => marioCoord[1] >= map.Y).ToList();
            }

            // Find map from list
            if (_currentMapList.Count <= 0)
            {
                ChangeCurrentMap(_assoc.DefaultMap);
            }
            else
            {
                // Find the best map to use
                Map bestMap = _currentMapList[0];
                foreach (Map map in _currentMapList)
                {
                    if (map.Y < bestMap.Y)
                        bestMap = map;
                }

                ChangeCurrentMap(bestMap);
            }

            UpdateMapCoordinates(marioCoord[0], marioCoord[2]);

            _mapGraphics.OnPaint(this, new EventArgs());
        }

        private void ChangeCurrentMap(Map map)
        {
            if (_currentMap == map)
                return;

            _mapGraphics.SetMap(_assoc.GetMapImage(map));
            _currentMap = map;
        }

        private void UpdateMapCoordinates(float marioX, float marioZ)
        {

            RectangleF mapView = _mapGraphics.MapView;
            PointF marioCoord = new PointF(marioX, marioZ);

            // Calculate position on picture;
            _marioMapObj.Location.X = mapView.X + (marioCoord.X - _currentMap.Coordinates.X) * (mapView.Width / _currentMap.Coordinates.Width);
            _marioMapObj.Location.Y = mapView.Y + (marioCoord.Y - _currentMap.Coordinates.Y) * (mapView.Height / _currentMap.Coordinates.Height);
        }

        private void SetPictureBoxLocation(PictureBox box, PointF point)
        {
            box.Location = new Point((int)point.X - box.Width / 2,
                (int)point.Y - box.Height / 2);
        }
        
    }
}
