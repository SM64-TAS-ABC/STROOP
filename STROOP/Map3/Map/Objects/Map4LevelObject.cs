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
using STROOP.Map3.Map.Graphics.Items;
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3.Map.Objects
{
    class Map4LevelObject : Map4Object
    {

        public enum ColorMethodType { WallsFloorsCeilings, RGBXYZNormalComponents, NormalY }

        Map4GraphicsBackgroundItem _background;
        Map4GraphicsImageItem _layout;
        Map4GraphicsTrianglesItem _triangles;

        MapLayout _currentMap;
        Bitmap _currentBackground;

        public override IEnumerable<Map4GraphicsItem> GraphicsItems => new List<Map4GraphicsItem>() { /*_background,*/ /* _layout, _triangles */ };

        public override Bitmap BitmapImage
        {
            get => null;
        }

        public ColorMethodType ColorMethod;

        public Map4LevelObject() : base("Level", null, null, false, null)
        {
            _background = new Map4GraphicsBackgroundItem(null);
            _layout = new Map4GraphicsImageItem(null);
            _triangles = new Map4GraphicsTrianglesItem();
        }

        public override void Update()
        {
            UpdateMap();
            UpdateBackground();
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
            if (backgroundChoice is BackgroundImage background)
            {
                ChangeBackground(background.Image);
            }
            else
            {
                ChangeBackground(bestMap.BackgroundImage);
            }
        }

        void UpdateTriangles()
        {
            List<Map4Vertex> vertices = new List<Map4Vertex>();
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


                vertices.Add(new Map4Vertex(new Vector3(tri.X1, tri.Y1, tri.Z1), color));
                vertices.Add(new Map4Vertex(new Vector3(tri.X2, tri.Y2, tri.Z2), color));
                vertices.Add(new Map4Vertex(new Vector3(tri.X3, tri.Y3, tri.Z3), color));
            }

            _triangles.SetTriangles(vertices);
        }

        private void ChangeCurrentMap(MapLayout map)
        {
            // Don't change the map if it isn't different
            if (map.Equals(_currentMap))
                return;

            _layout.ChangeImage(map.MapImage);
            _currentMap = map;

            _layout.Region = map.Coordinates;
            _layout.Y = map.Y != float.MinValue ? map.Y : 0.0f;
        }

        private void ChangeBackground(Bitmap background)
        {
            // Don't change the background if it isn't different
            if (_currentBackground == background)
                return;

            _background.ChangeImage(background);
            _currentBackground = background;
        }
    }
}
