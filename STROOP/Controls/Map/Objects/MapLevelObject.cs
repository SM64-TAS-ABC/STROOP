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

        MapLayout _currentMap;
        Bitmap _currentBackground;

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
            float marioRelY = DataModels.Mario.PURelative_Y;
            MapLayout bestMap = Config.MapAssociations.GetBestMap(
                level.Index, level.Area, level.LoadingPoint, level.MissionLayout, marioRelY);

            object mapLayoutChoice = Config.MapGui.ComboBoxLevel.SelectedItem;
            if (mapLayoutChoice is MapLayout)
            {
                bestMap = (MapLayout)mapLayoutChoice;
            }

            ChangeCurrentMap(bestMap);
        }

        private void UpdateBackground()
        {
            LevelDataModel level = DataModels.Level;
            float marioRelY = DataModels.Mario.PURelative_Y;
            MapLayout bestMap = Config.MapAssociations.GetBestMap(
                level.Index, level.Area, level.LoadingPoint, level.MissionLayout, marioRelY);

            object backgroundChoice = Config.MapGui.ComboBoxBackground.SelectedItem;
            if (backgroundChoice is Bitmap)
            {
                ChangeBackground((Bitmap)backgroundChoice);
            }
            else
            {
                ChangeBackground(bestMap.BackgroundImage);
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
            _background.ChangeImage(map.BackgroundImage);
            _layout.ChangeImage(map.MapImage);

            _layout.Region = map.Coordinates;
            _layout.Y = map.Y != float.MinValue ? map.Y : 0.0f;

            _currentMap = map;
        }

        private void ChangeBackground(Bitmap background)
        {
            // Don't change the map if it isn't different
            if (_currentBackground == background)
                return;

            // Change and set a new map
            using (var mapBackground = background)
                _background.ChangeImage(mapBackground);
        }
    }
}
