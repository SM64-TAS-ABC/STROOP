using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STROOP.Controls.Map.Graphics.Items;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using STROOP.Models;
using System.Drawing;
using STROOP.Controls.Map.Graphics;
using OpenTK;
using OpenTK.Graphics;

namespace STROOP.Controls.Map.Objects
{
    class MapLevelObject : MapObject
    {

        public enum ColorMethodType { WallsFloorsCeilings, RGBXYZNormalComponents, NormalY }

        MapGraphicsBackgroundItem _background;
        MapGraphicsImageItem _layout;
        MapGraphicsTrianglesItem _triangles;

        byte _currentLevel, _currentArea;
        ushort _currentLoadingPoint, _currentMissionLayout;
        MapLayout _currentMap;
        List<MapLayout> _currentMapList = null;
        MapAssociations _mapAssoc;

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new List<MapGraphicsItem>() { _background, _layout, _triangles };

        public ColorMethodType ColorMethod;

        public MapLevelObject(MapAssociations mapAssoc)
        {
            _mapAssoc = mapAssoc;
            _background = new MapGraphicsBackgroundItem(null);
            _layout = new MapGraphicsImageItem(null);
            _triangles = new MapGraphicsTrianglesItem();
        }

        public override void Update()
        {
            UpdateMap();
            UpdateTriangles();
        }

        private void UpdateMap()
        {
            LevelDataModel level = DataModels.Level;

            // Find new map list
            if (_currentMapList == null || _currentLevel != level.Index || _currentArea != level.Area
                || _currentLoadingPoint != level.LoadingPoint || _currentMissionLayout != level.MissionLayout)
            {
                _currentLevel = level.Index;
                _currentArea = level.Area;
                _currentLoadingPoint = level.LoadingPoint;
                _currentMissionLayout = level.MissionLayout;
                _currentMapList = _mapAssoc.GetLevelAreaMaps(level.Index, level.Area);

                // Look for maps with correct loading points
                var mapListLPFiltered = _currentMapList.Where((map) => map.LoadingPoint == level.LoadingPoint).ToList();
                if (mapListLPFiltered.Count > 0)
                    _currentMapList = mapListLPFiltered;
                else
                    _currentMapList = _currentMapList.Where((map) => !map.LoadingPoint.HasValue).ToList();

                var mapListMLFiltered = _currentMapList.Where((map) => map.MissionLayout == level.MissionLayout).ToList();
                if (mapListMLFiltered.Count > 0)
                    _currentMapList = mapListMLFiltered;
                else
                    _currentMapList = _currentMapList.Where((map) => !map.MissionLayout.HasValue).ToList();
            }

            float marioRelY = DataModels.Mario.PURelative_Y;

            // Filter out all maps that are lower than Mario
            var mapListYFiltered = _currentMapList.Where((map) => map.Y <= marioRelY).ToList();

            // If no map is available display the default image
            if (mapListYFiltered.Count <= 0)
            {
                ChangeCurrentMap(_mapAssoc.DefaultMap);
            }
            else
            {
                // Pick the map closest to mario (yet still above Mario)
                MapLayout bestMap = mapListYFiltered.First();
                foreach (MapLayout map in mapListYFiltered)
                {
                    if (map.Y > bestMap.Y)
                        bestMap = map;
                }

                ChangeCurrentMap(bestMap);
            }
        }

        void UpdateTriangles()
        {
            List<Vertex> vertices = new List<Vertex>();
            foreach(TriangleDataModel tri in TriangleUtilities.GetAllTriangles())
            {
                Color4 color = Color4.Black;

                switch (ColorMethod)
                {
                    case ColorMethodType.WallsFloorsCeilings:
                        switch (tri.Classification)
                        {
                            case TriangleClassification.Wall:
                                color = Color4.LightGreen;
                                break;
                            case TriangleClassification.Floor:
                                color = Color4.LightBlue;
                                break;
                            case TriangleClassification.Ceiling:
                                color = Color4.Pink;
                                break;
                        }                         

                        var i = (float)(Math.Atan2(tri.NormY, tri.NormX) / Math.PI / 2) * 0.1f - 0.2f;
                        i += tri.NormY * 0.1f - 0.2f;
                        color.R += i;
                        color.B += i;
                        color.G += i;
                        break;

                    case ColorMethodType.RGBXYZNormalComponents:
                        color = new Color4(Math.Abs(tri.NormX), Math.Abs(tri.NormY), Math.Abs(tri.NormZ), 1.0f);
                        break;

                    case ColorMethodType.NormalY:
                        color = new Color4(Math.Abs(tri.NormY), Math.Abs(tri.NormY), Math.Abs(tri.NormY), 1.0f);
                        break;
                }


                vertices.Add(new Vertex(new Vector3(tri.X1, tri.Y1, tri.Z1), color));
                vertices.Add(new Vertex(new Vector3(tri.X2, tri.Y2, tri.Z2), color));
                vertices.Add(new Vertex(new Vector3(tri.X3, tri.Y3, tri.Z3), color));
            }

            _triangles.SetTriangles(vertices);
        }

        private void ChangeCurrentMap(MapLayout map)
        {
            // Don't change the map if it isn't different
            if (_currentMap == map)
                return;

            // Change and set a new map
            using (var mapBackground = _mapAssoc.GetMapBackgroundImage(map))
                _background.ChangeImage(mapBackground);

            using (var mapLayout = _mapAssoc.GetMapImage(map))
                _layout.ChangeImage(mapLayout);

            _layout.Region = map.Coordinates;
            _layout.Y = map.Y != float.MinValue ? map.Y : 0.0f;

            _currentMap = map;
        }
    }
}
