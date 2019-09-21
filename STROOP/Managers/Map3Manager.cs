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
        public MapLayout map;
        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint, _currentMissionLayout;
        List<MapLayout> _currentMapList = null;
        Map3Graphics _mapGraphics;

        Map3Object _background;
        Map3Object _gridlines;
        Map3Object _map;
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
            _marioMapObj = new Map3MarioObject(_mapGraphics);

            _isLoaded = true;

            // Set the default map
            ChangeCurrentMap(Config.MapAssociations.DefaultMap);

            // Add map objects
            _mapGraphics.AddMapObject(_background);
            _mapGraphics.AddMapObject(_gridlines);
            _mapGraphics.AddMapObject(_map);
            _mapGraphics.AddMapObject(_marioMapObj);
        }

        public void Update(bool updateView)
        {
            if (!updateView) return;
            if (!_isLoaded) return;

            // Get level and area
            byte level = Config.Stream.GetByte(MiscConfig.LevelAddress);
            byte area = Config.Stream.GetByte(MiscConfig.AreaAddress);
            ushort loadingPoint = Config.Stream.GetUInt16(MiscConfig.LoadingPointAddress);
            ushort missionLayout = Config.Stream.GetUInt16(MiscConfig.MissionAddress);

            // Find new map list
            if (_currentMapList == null || _currentLevel != level || _currentArea != area
                || _currentLoadingPoint != loadingPoint || _currentMissionLayout != missionLayout)
            {
                _currentLevel = level;
                _currentArea = area;
                _currentLoadingPoint = loadingPoint;
                _currentMissionLayout = missionLayout;
                _currentMapList = Config.MapAssociations.GetLevelAreaMaps(level, area);

                // Look for maps with correct loading points
                var mapListLPFiltered = _currentMapList.Where((map) => map.LoadingPoint == loadingPoint).ToList();
                if (mapListLPFiltered.Count > 0)
                    _currentMapList = mapListLPFiltered;
                else
                    _currentMapList = _currentMapList.Where((map) => !map.LoadingPoint.HasValue).ToList();

                var mapListMLFiltered = _currentMapList.Where((map) => map.MissionLayout == missionLayout).ToList();
                if (mapListMLFiltered.Count > 0)
                    _currentMapList = mapListMLFiltered;
                else
                    _currentMapList = _currentMapList.Where((map) => !map.MissionLayout.HasValue).ToList();
            }

            // Filter out all maps that are lower than Mario
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float relMarioY = (float)PuUtilities.GetRelativeCoordinate(marioY);
            var mapListYFiltered = _currentMapList.Where((map) => map.Y <= relMarioY).ToList();

            // If no map is available display the default image
            if (mapListYFiltered.Count <= 0)
            {
                ChangeCurrentMap(Config.MapAssociations.DefaultMap);
            }
            else
            {
                // Pick the map closest to mario (yet still above Mario)
                MapLayout bestMap = mapListYFiltered[0];
                foreach (MapLayout map in mapListYFiltered)
                {
                    if (map.Y > bestMap.Y)
                        bestMap = map;
                }

                ChangeCurrentMap(bestMap);
            }

            // Update gui by drawing images (invokes _mapGraphics.OnPaint())
            _mapGraphics.Control.Invalidate();
        }

        private void ChangeCurrentMap(MapLayout map)
        {
            // Change and set a new map
            _mapGraphics.SetMap(map.MapImage);
        }
    }
}
