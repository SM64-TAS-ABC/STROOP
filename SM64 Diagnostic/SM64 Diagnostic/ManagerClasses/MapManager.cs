using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SM64_Diagnostic.Structs;
using SM64_Diagnostic.Utilities;
using System.Windows.Forms;
using System.Drawing;

namespace SM64_Diagnostic.ManagerClasses
{
    public class MapManager
    {
        ProcessStream _stream;
        Config _config;
        MapAssociations _assoc;
        PictureBox _pictureBoxMap, _pictureBoxMario;
        byte _currentLevel, _currentArea;
        Map _currentMap;
        List<Map> _currentMapList = null;

        public bool Visible
        {
            get
            {
                return _pictureBoxMap.Visible;
            }
            set
            {
                _pictureBoxMap.Visible = value;
                _pictureBoxMario.Visible = value;
            }
        }

        public MapManager(ProcessStream stream, Config config, MapAssociations mapAssoc,
            PictureBox pictureBoxMap, PictureBox pictureBoxMario)
        {
            _stream = stream;
            _config = config;
            _assoc = mapAssoc;
            _pictureBoxMap = pictureBoxMap;
            _pictureBoxMario = pictureBoxMario;
            _pictureBoxMap.Image = _assoc.GetMapImage(_assoc.DefaultMap);
            _currentMap = _assoc.DefaultMap;
            _pictureBoxMap.Controls.Add(_pictureBoxMario);
            _pictureBoxMario.BackColor = Color.Transparent;
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
        }

        private void ChangeCurrentMap(Map map)
        {
            if (_currentMap == map)
                return;

            _pictureBoxMap.Image?.Dispose();
            _pictureBoxMap.Image = _assoc.GetMapImage(map);
            _currentMap = map;
        }

        private void UpdateMapCoordinates(float marioX, float marioZ)
        {
            RectangleF mapView = GetMapViewRegion();
            PointF marioCoord = new PointF(marioX, marioZ);

            // Calculate position on picture;
            marioCoord.X = mapView.X + (marioCoord.X - _currentMap.Coordinates.X) * (mapView.Width / _currentMap.Coordinates.Width);
            marioCoord.Y = mapView.Y + (marioCoord.Y - _currentMap.Coordinates.Y) * (mapView.Height / _currentMap.Coordinates.Height);

            // Make sure we don't go out of the image bounds
            marioCoord.X = Math.Max(Math.Min(marioCoord.X, mapView.Right), mapView.Left);
            marioCoord.Y = Math.Max(Math.Min(marioCoord.Y, mapView.Bottom), mapView.Top);

            SetPictureBoxLocation(_pictureBoxMario, marioCoord);
        }

        private void SetPictureBoxLocation(PictureBox box, PointF point)
        {
            box.Location = new Point((int)point.X - box.Width / 2,
                (int)point.Y - box.Height / 2);
        }

        private RectangleF GetMapViewRegion()
        {
            float hScale = (float)_pictureBoxMap.Width / _pictureBoxMap.Image.Width;
            float vScale = (float)_pictureBoxMap.Height / _pictureBoxMap.Image.Height;
            float scale = Math.Min(hScale, vScale);

            float marginV = 0;
            float marginH = 0;
            if (hScale > vScale)
                marginH = (_pictureBoxMap.Width - scale * _pictureBoxMap.Image.Width) / 2;
            else
                marginV = (_pictureBoxMap.Height - scale * _pictureBoxMap.Image.Height) / 2;

            return new RectangleF(marginH, marginV, _pictureBoxMap.Width - 2 * marginH, _pictureBoxMap.Height - 2 * marginV);
        }
    }
}
