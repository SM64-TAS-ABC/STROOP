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

namespace STROOP.Map
{
    public class MapObjectBounds : MapObject
    {
        private int _blueCircleTex = -1;

        public MapObjectBounds()
            : base()
        {
            Size = 15;
            Opacity = 0.25;
            Color = Color.Magenta;
            LineWidth = 3;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float x, float z)> data = GetData();

            List<(float x, float z)> dataForControl =
                data.ConvertAll(d => MapUtilities.ConvertCoordsForControlTopDownView(d.x, d.z, UseRelativeCoordinates));

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

            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                (float x, float z) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(x, z, UseRelativeCoordinates);
                float angleDegrees = 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueCircleMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = 1;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_blueCircleTex, point, size, angleDegrees, opacity);
            }
        }

        private List<(float x, float z)> GetData()
        {
            return new List<(float x, float z)>()
            {
                (-100, -100),
                (-100, 100),
                (100, 100),
                (100, -100),
            };
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

            List<(float x, float z)> data = GetData();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var point = data[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(point.x, point.z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, point.x, 0, point.z, index: i);
                }
            }
            return null;
        }
    }
}
