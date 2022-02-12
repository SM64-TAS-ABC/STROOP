using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;
using STROOP.Map.Map3D;
using STROOP.Models;

namespace STROOP.Map
{
    public class MapObjectCorkBoxTester : MapObject
    {
        private int _redCircleTex = -1;
        private int _redLightCircleTex = -1;
        private int _redDarkCircleTex = -1;
        private int _blueCircleTex = -1;
        private int _blueLightCircleTex = -1;
        private int _blueDarkCircleTex = -1;
        private int _yellowCircleTex = -1;

        private int _levelTriangleCount;
        private CellSnapshot _cellSnapshot;
        private Dictionary<(double x, double z), (float y, int numFrames)> _cache;

        public MapObjectCorkBoxTester()
            : base()
        {
            Size = 10;

            _levelTriangleCount = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);
            _cellSnapshot = new CellSnapshot();
            _cache = new Dictionary<(double x, double z), (float y, int numFrames)>();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.YellowCircleMapImage;
        }

        public override string GetName()
        {
            return "Cork Box Tester";
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(double x, float y, double z, int numFrames)> data = GetData();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                (double x, float y, double z, int numFrames) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView((float)x, (float)z);
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.RedCircleMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                int tex = GetTexForNumFrames(numFrames);
                MapUtilities.DrawTexture(tex, point, size, 0, opacity);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        private int GetTexForNumFrames(int numFrames)
        {
            if (numFrames <= 500)
            {
                return _redDarkCircleTex;
            }
            else if (numFrames <= 800)
            {
                return _redCircleTex;
            }
            else if (numFrames <= 900)
            {
                return _redLightCircleTex;
            }
            else if (numFrames <= 901)
            {
                return _yellowCircleTex;
            }
            else if (numFrames <= 950)
            {
                return _blueLightCircleTex;
            }
            else if (numFrames <= 1000)
            {
                return _blueCircleTex;
            }
            else
            {
                return _blueDarkCircleTex;
            }
        }

        public List<(double x, float y, double z, int numFrames)> GetData()
        {
            double xMin = Config.CurrentMapGraphics.MapViewXMin;
            double xMax = Config.CurrentMapGraphics.MapViewXMax;
            double zMin = Config.CurrentMapGraphics.MapViewZMin;
            double zMax = Config.CurrentMapGraphics.MapViewZMax;

            double xRange = xMax - xMin;
            double zRange = zMax - zMin;
            double maxRange = Math.Max(xRange, zRange);
            double power = Math.Log10(maxRange);
            double powerOffset = power - 0.5;
            double powerFloor = Math.Floor(powerOffset);
            double floorDiff = powerOffset - powerFloor;
            double gap = Math.Pow(10, powerFloor);
            if (floorDiff < 0.6)
            {
                gap /= 2;
            }

            int xMultipleMin = (int)(xMin / gap) - 1;
            int xMultipleMax = (int)(xMax / gap) + 1;
            int zMultipleMin = (int)(zMin / gap) - 1;
            int zMultipleMax = (int)(zMax / gap) + 1;

            List<(double x, float y, double z, int numFrames)> data =
                new List<(double x, float y, double z, int numFrames)>();
            for (int xMultiple = xMultipleMin; xMultiple <= xMultipleMax; xMultiple++)
            {
                for (int zMultiple = zMultipleMin; zMultiple <= zMultipleMax; zMultiple++)
                {
                    double x = xMultiple * gap;
                    double z = zMultiple * gap;
                    var d = GetNumFramesFromCache(x, z, _cellSnapshot);
                    data.Add((x, d.y, z, d.numFrames));
                }
            }
            return data;
        }

        private (float y, int numFrames) GetNumFramesFromCache(double x, double z, CellSnapshot cellSnapshot)
        {
            if (!_cache.ContainsKey((x, z)))
            {
                _cache[(x, z)] = CorkBoxUtilities.GetNumFrames(x, z, cellSnapshot);
            }
            return _cache[(x, z)];
        }

        public override void Update()
        {
            if (_redCircleTex == -1)
            {
                _redCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.RedCircleMapImage as Bitmap);
                _redLightCircleTex = MapUtilities.LoadTexture(
                    ImageUtilities.ChangeColor(Config.ObjectAssociations.RedCircleMapImage, 0.5) as Bitmap);
                _redDarkCircleTex = MapUtilities.LoadTexture(
                    ImageUtilities.ChangeColor(Config.ObjectAssociations.RedCircleMapImage, -0.5) as Bitmap);

                _blueCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.BlueCircleMapImage as Bitmap);
                _blueLightCircleTex = MapUtilities.LoadTexture(
                    ImageUtilities.ChangeColor(Config.ObjectAssociations.BlueCircleMapImage, 0.8) as Bitmap);
                _blueDarkCircleTex = MapUtilities.LoadTexture(
                    ImageUtilities.ChangeColor(Config.ObjectAssociations.BlueCircleMapImage, -0.5) as Bitmap);

                _yellowCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.YellowCircleMapImage as Bitmap);
            }

            int levelTriangleCount = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);
            if (levelTriangleCount != _levelTriangleCount)
            {
                _levelTriangleCount = levelTriangleCount;
                _cellSnapshot = new CellSnapshot();
                _cache.Clear();
            }
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            var data = GetData();
            for (int i = 0; i < data.Count; i++)
            {
                var dataPoint = data[i];
                double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i, info: "NumFrames=" + dataPoint.numFrames);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var data = GetData();
            var dataPoint = data[hoverData.Index.Value];
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(dataPoint.x, dataPoint.y, dataPoint.z, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
