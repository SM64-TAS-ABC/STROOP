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

namespace STROOP.Map
{
    public class MapObjectCorkBoxTester : MapObject
    {
        private int _redCircleTex = -1;
        private int _blueCircleTex = -1;
        private int _yellowCircleTex = -1;

        public MapObjectCorkBoxTester()
            : base()
        {

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
                int threshold = 901;
                int tex = numFrames < threshold ? _redCircleTex : numFrames > threshold ? _blueCircleTex : _yellowCircleTex;
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
        
        public List<(double x, float y, double z, int numFrames)> GetData()
        {
            double xMin = Config.CurrentMapGraphics.MapViewXMin;
            double xMax = Config.CurrentMapGraphics.MapViewXMax;
            double zMin = Config.CurrentMapGraphics.MapViewZMin;
            double zMax = Config.CurrentMapGraphics.MapViewZMax;

            double xGap = (xMax - xMin) / 10;
            double zGap = (zMax - zMin) / 10;

            List<(double x, float y, double z, int numFrames)> data =
                new List<(double x, float y, double z, int numFrames)>();
            for (double x = xMin; x < xMax; x += xGap)
            {
                for (double z = zMin; z < zMax; z += zGap)
                {
                    var d = CorkBoxUtilities.GetNumFrames(x, z);
                    data.Add((x, d.y, z, d.numFrames));
                }
            }
            return data;
        }

        public override void Update()
        {
            if (_redCircleTex == -1)
            {
                _redCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.RedCircleMapImage as Bitmap);
            }
            if (_blueCircleTex == -1)
            {
                _blueCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.BlueCircleMapImage as Bitmap);
            }
            if (_yellowCircleTex == -1)
            {
                _yellowCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.YellowCircleMapImage as Bitmap);
            }
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            return null;

            //Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            //if (!relPosMaybe.HasValue) return null;
            //Point relPos = relPosMaybe.Value;
            //(float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            //var data = GetData();
            //for (int i = 0; i < data.Count; i++)
            //{
            //    var dataPoint = data[i];
            //    double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
            //    double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
            //    if (dist <= radius || forceCursorPosition)
            //    {
            //        return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i);
            //    }
            //}
            //return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            //var data = GetData();
            //var dataPoint = data[hoverData.Index.Value];
            //ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(dataPoint.x, dataPoint.y, dataPoint.z, "Position");
            //output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
