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
        object _currentSelectedItem;
        MapLayout _currentMap;
        List<MapLayout> _currentMapList = null;

        public override IEnumerable<MapGraphicsItem> GraphicsItems => new List<MapGraphicsItem>() { _background, _layout, _triangles };

        public override Bitmap BitmapImage
        {
            get => null;
        }

        public ColorMethodType ColorMethod;

        public MapLevelObject() : base("Level", null, null, false)
        {
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

                _currentLevel = level.Index;
                _currentArea = level.Area;
                _currentLoadingPoint = level.LoadingPoint;
                _currentMissionLayout = level.MissionLayout;
                _currentSelectedItem = Config.MapGui.ComboBoxLevel.SelectedItem;
                float marioRelY = DataModels.Mario.PURelative_Y;
                _currentMapList = Config.MapAssociations.GetLevelAreaMaps(level.Index, level.Area, level.LoadingPoint, level.MissionLayout, marioRelY);


            // Filter out all maps that are lower than Mario
            var mapListYFiltered = _currentMapList;

            // If no map is available display the default image
            MapLayout newMap;
            if (mapListYFiltered.Count <= 0)
            {
                newMap = Config.MapAssociations.DefaultMap;
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
                newMap = bestMap;
            }

            object mapLayoutChoice = Config.MapGui.ComboBoxLevel.SelectedItem;
            if (mapLayoutChoice is MapLayout)
            {
                newMap = (MapLayout)mapLayoutChoice;
            }

            ChangeCurrentMap(newMap);
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
            using (var mapBackground = Config.MapAssociations.GetMapBackgroundImage(map))
                _background.ChangeImage(mapBackground);

            using (var mapLayout = Config.MapAssociations.GetMapImage(map))
                _layout.ChangeImage(mapLayout);

            _layout.Region = map.Coordinates;
            _layout.Y = map.Y != float.MinValue ? map.Y : 0.0f;

            _currentMap = map;
        }
    }
}
