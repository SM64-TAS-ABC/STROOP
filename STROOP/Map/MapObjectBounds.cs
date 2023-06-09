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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectBounds : MapObject
    {
        public static MapObjectBounds LAST_INSTANCE = null;

        private int _blueCircleTex = -1;
        private int _lastHoveredPointIndex = 0;

        private List<(float x, float z)> _points =
            new List<(float x, float z)>()
            {
                (-1000, -1000),
                (-1000, 1000),
                (1000, 1000),
                (1000, -1000),
            };

        public MapObjectBounds()
            : base()
        {
            Size = 15;
            Opacity = 0.25;
            Color = Color.Yellow;
            LineWidth = 3;

            LAST_INSTANCE = this;
        }

        public MapObjectBounds(List<(float x, float z)> points) : this()
        {
            _points = points;
        }

        public static MapObjectBounds Create(string text)
        {
            List<(double x, double y, double z)> points = MapUtilities.ParsePoints(text, false);
            if (points == null) return null;
            List<(float x, float z)> floatPoints = points.ConvertAll(
                point => ((float)point.x, (float)point.z));
            return new MapObjectBounds(floatPoints);
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float x, float z)> dataForControl =
                _points.ConvertAll(d => MapUtilities.ConvertCoordsForControlTopDownView(d.x, d.z, UseRelativeCoordinates));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw quad
            GL.Begin(PrimitiveType.Quads);
            foreach (var d in dataForControl)
            {
                GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
                GL.Vertex2(d.x, d.z);
            }
            GL.End();

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                GL.LineWidth(LineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var d in dataForControl)
                {
                    GL.Vertex2(d.x, d.z);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);

            for (int i = _points.Count - 1; i >= 0; i--)
            {
                var dataPoint = _points[i];
                (float x, float z) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(x, z, UseRelativeCoordinates);
                float angleDegrees = 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueCircleMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = 1;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    _lastHoveredPointIndex = i;
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_blueCircleTex, point, size, angleDegrees, opacity);
            }
        }

        public override (double x, double y, double z)? GetDragPosition()
        {
            var point = _points[_lastHoveredPointIndex];
            return (point.x, 0, point.z);
        }

        public override void SetDragPositionTopDownView(double? x = null, double? y = null, double? z = null)
        {
            if (x.HasValue)
            {
                _points[_lastHoveredPointIndex] = ((float)x.Value, _points[_lastHoveredPointIndex].z);
            }

            if (z.HasValue)
            {
                _points[_lastHoveredPointIndex] = (_points[_lastHoveredPointIndex].x, (float)z.Value);
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

        public override string GetName()
        {
            return "Bounds";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.WatersImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override void Update()
        {
            if (_blueCircleTex == -1)
            {
                _blueCircleTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.BlueCircleMapImage as Bitmap);
            }
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            for (int i = _points.Count - 1; i >= 0; i--)
            {
                var point = _points[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(point.x, point.z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                if (dist <= radius || (forceCursorPosition && _lastHoveredPointIndex == i))
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, point.x, 0, point.z, index: i);
                }
            }
            return null;
        }

        public int GetXMin()
        {
            return (int)Math.Floor(_points.Min(p => p.x));
        }

        public int GetXMax()
        {
            return (int)Math.Ceiling(_points.Max(p => p.x));
        }

        public int GetZMin()
        {
            return (int)Math.Floor(_points.Min(p => p.z));
        }

        public int GetZMax()
        {
            return (int)Math.Ceiling(_points.Max(p => p.z));
        }

        public bool IsWithinBounds(float x, float z)
        {
            return MapUtilities.IsWithinShapeForControl(_points, x, z, false);
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = _points.ConvertAll(
                p => string.Format("({0},{1})", (double)p.x, (double)p.z));
            return new List<XAttribute>()
            {
                new XAttribute("points", string.Join(",", pointList)),
            };
        }
    }
}
