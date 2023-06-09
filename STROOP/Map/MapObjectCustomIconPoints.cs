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
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapObjectCustomIconPoints : MapObject
    {
        private int _tex = -1;

        private readonly List<(float x, float y, float z)> _points;

        public MapObjectCustomIconPoints(List<(float x, float y, float z)> points)
            : base()
        {
            _points = points;

            LineWidth = 0;
        }

        public static MapObjectCustomIconPoints Create(string text, bool useTriplets)
        {
            List<(double x, double y, double z)> points = MapUtilities.ParsePoints(text, useTriplets);
            if (points == null) return null;
            List<(float x, float y, float z)> floatPoints = points.ConvertAll(
                point => ((float)point.x, (float)point.y, (float)point.z));
            return new MapObjectCustomIconPoints(floatPoints);
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            for (int i = 0; i <_points.Count; i++)
            {
                var p = _points[i];
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(p.x, p.z, UseRelativeCoordinates);
                Image image = _customImage ?? Config.ObjectAssociations.GreenMarioMapImage;
                SizeF size = MapUtilities.ScaleImageSizeForControl(image.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_customImageTex ?? _tex, point, size, 0, opacity);
            }

            if (LineWidth != 0)
            {
                for (int i = 0; i < _points.Count - 1; i++)
                {
                    var p1 = _points[i];
                    var p2 = _points[i + 1];
                    MapUtilities.DrawLinesOn2DControlTopDownView(new List<(float x, float y, float z)>() { p1, p2 }, LineWidth, LineColor, 255, UseRelativeCoordinates);
                }
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            for (int i = 0; i < _points.Count; i++)
            {
                var p = _points[i];
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlOrthographicView(p.x, p.y, p.z, UseRelativeCoordinates);
                Image image = _customImage ?? Config.ObjectAssociations.GreenMarioMapImage;
                SizeF size = MapUtilities.ScaleImageSizeForControl(image.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(_customImageTex ?? _tex, point, size, 0, opacity);
            }

            if (LineWidth != 0)
            {
                for (int i = 0; i < _points.Count - 1; i++)
                {
                    var p1 = _points[i];
                    var p2 = _points[i + 1];
                    MapUtilities.DrawLinesOn2DControlOrthographicView(new List<(float x, float y, float z)>() { p1, p2 }, LineWidth, LineColor, 255, UseRelativeCoordinates);
                }
            }
        }

        public override void DrawOn3DControl()
        {
            foreach (var p in _points)
            {
                Matrix4 viewMatrix = GetModelMatrix(p.x, p.y, p.z);
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                Map3DVertex[] vertices = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                    vertices, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, _customImageTex ?? _tex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
                GL.DeleteBuffer(vertexBuffer);
            }

            if (LineWidth != 0)
            {
                for (int i = 0; i < _points.Count - 1; i++)
                {
                    var p1 = _points[i];
                    var p2 = _points[i + 1];
                    MapUtilities.DrawLinesOn3DControl(new List<(float x, float y, float z)>() { p1, p2 }, LineWidth, LineColor, 255, GetModelMatrix());
                }
            }
        }

        public Matrix4 GetModelMatrix(float x, float y, float z)
        {
            Image image = _customImage ?? Config.ObjectAssociations.GreenMarioMapImage;
            SizeF _imageNormalizedSize = new SizeF(
                image.Width >= image.Height ? 1.0f : (float)image.Width / image.Height,
                image.Width <= image.Height ? 1.0f : (float)image.Height / image.Width);

            Vector3 pos = new Vector3(x, y, z);

            float size = Size / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(0)
                * Matrix4.CreateScale(1.0f / Config.Map3DGraphics.NormalizedWidth, 1.0f / Config.Map3DGraphics.NormalizedHeight, 1)
                * Matrix4.CreateTranslation(MapUtilities.GetPositionOnViewFromCoordinate(pos));
        }

        private Map3DVertex[] GetVertices()
        {
            return new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(-1, -1, 0), Color4, new Vector2(0, 1)),
                new Map3DVertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color4, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, 1, 0), Color4, new Vector2(1, 0)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color4,  new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
            };
        }

        public override void Update()
        {
            if (_tex == -1)
            {
                _tex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.GreenMarioMapImage as Bitmap);
            }
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.GreenMarioMapImage;
        }

        public override string GetName()
        {
            return $"Custom Icon Points ({_points.Count})";
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem copyPoints = new ToolStripMenuItem("Copy Points");
                copyPoints.Click += (sender, e) =>
                {
                    List<string> lines = _points.ConvertAll(p => p.x + "\t" + p.y + "\t" + p.z);
                    Clipboard.SetText(string.Join("\r\n", lines));
                };

                ToolStripMenuItem copyPointsLaterally = new ToolStripMenuItem("Copy Points Laterally");
                copyPointsLaterally.Click += (sender, e) =>
                {
                    List<string> lines = _points.ConvertAll(p => p.x + "\t" + p.z);
                    Clipboard.SetText(string.Join("\r\n", lines));
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(copyPoints);
                _contextMenuStrip.Items.Add(copyPointsLaterally);
            }

            return _contextMenuStrip;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            for (int i = _points.Count - 1; i >= 0; i--)
            {
                var point = _points[i];
                double dist = MoreMath.GetDistanceBetween(point.x, point.z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, point.x, point.y, point.z, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            for (int i = _points.Count - 1; i >= 0; i--)
            {
                var point = _points[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(point.x, point.y, point.z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, point.x, point.y, point.z, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var point = _points[hoverData.Index.Value];
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(point.x, point.y, point.z, "Position");
            output.Insert(0, copyPositionItem);

            ToolStripMenuItem goToPositionItem = new ToolStripMenuItem("Go to Position");
            goToPositionItem.Click += (sender, e) =>
            {
                Config.Stream.Suspend();
                Config.Stream.SetValue((float)hoverData.X, MarioConfig.StructAddress + MarioConfig.XOffset);
                Config.Stream.SetValue((float)hoverData.Y, MarioConfig.StructAddress + MarioConfig.YOffset);
                Config.Stream.SetValue((float)hoverData.Z, MarioConfig.StructAddress + MarioConfig.ZOffset);
                Config.Stream.Resume();
            };
            output.Insert(1, goToPositionItem);

            ToolStripMenuItem goToPositionLaterallyItem = new ToolStripMenuItem("Go to Position Laterally");
            goToPositionLaterallyItem.Click += (sender, e) =>
            {
                Config.Stream.Suspend();
                Config.Stream.SetValue((float)hoverData.X, MarioConfig.StructAddress + MarioConfig.XOffset);
                Config.Stream.SetValue((float)hoverData.Z, MarioConfig.StructAddress + MarioConfig.ZOffset);
                Config.Stream.Resume();
            };
            output.Insert(2, goToPositionLaterallyItem);

            ToolStripMenuItem removePointItem = new ToolStripMenuItem("Remove Point");
            removePointItem.Click += (sender, e) =>
            {
                _points.RemoveAt(hoverData.Index.Value);
            };
            output.Insert(3, removePointItem);

            return output;
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = _points.ConvertAll(
                p => string.Format("({0},{1},{2})", (double)p.x, (double)p.y, (double)p.z));
            return new List<XAttribute>()
            {
                new XAttribute("points", string.Join(",", pointList)),
            };
        }
    }
}
