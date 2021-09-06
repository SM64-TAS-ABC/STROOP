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
    public abstract class MapObjectCircle : MapObject
    {
        private float _imageSize;
        private ToolStripMenuItem _itemSetIconSize;
        private static readonly string SET_ICON_SIZE_TEXT = "Set Icon Size";

        public MapObjectCircle()
            : base()
        {
            _imageSize = 8;

            Opacity = 0.5;
            Color = Color.Red;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float centerX, float centerZ, float radius)> dimensionList = Get2DDimensions();

            for (int i = 0; i < dimensionList.Count; i++)
            {
                (float centerX, float centerZ, float radius) = dimensionList[i];
                (float controlCenterX, float controlCenterZ) = MapUtilities.ConvertCoordsForControlTopDownView(centerX, centerZ);
                float controlRadius = radius * Config.CurrentMapGraphics.MapViewScaleValue;
                List <(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, SpecialConfig.MapCircleNumPoints2D).ToList()
                    .ConvertAll(index => (index / (float)SpecialConfig.MapCircleNumPoints2D) * 65536)
                    .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(controlRadius, angle, controlCenterX, controlCenterZ));

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw circle
                byte opacityByte = OpacityByte;
                if (this == hoverData?.MapObject && i == hoverData?.Index && hoverData.Index2 == null)
                {
                    opacityByte = MapUtilities.GetHoverOpacityByte();
                }
                GL.Color4(Color.R, Color.G, Color.B, opacityByte);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(controlCenterX, controlCenterZ);
                foreach ((float x, float z) in controlPoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.Vertex2(controlPoints[0].pointX, controlPoints[0].pointZ);
                GL.End();

                // Draw outline
                if (LineWidth != 0)
                {
                    GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                    GL.LineWidth(LineWidth);
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in controlPoints)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }

                if (_customImage != null)
                {
                    List<(float x, float z)> positions = MapUtilities.GetFloatPositions(10_000);
                    for (int j = 0; j < positions.Count; j++)
                    {
                        (float x, float z) = positions[j];
                        float dist = (float)MoreMath.GetDistanceBetween(centerX, centerZ, x, z);
                        if (dist >= radius) continue;
                        (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                        SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, _imageSize, Scales);
                        double opacity = 1;
                        if (this == hoverData?.MapObject && i == hoverData?.Index && j == hoverData?.Index2)
                        {
                            opacity = MapUtilities.GetHoverOpacity();
                        }
                        MapUtilities.DrawTexture(_customImageTex.Value, new PointF(controlX, controlZ), size, 0, opacity);
                    }
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        protected abstract List<(float centerX, float centerZ, float radius)> Get2DDimensions();

        protected abstract List<(float x, float y, float z)> GetPoints();

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        protected List<ToolStripMenuItem> GetCircleToolStripMenuItems()
        {
            string suffix = string.Format(" ({0})", _imageSize);
            _itemSetIconSize = new ToolStripMenuItem(SET_ICON_SIZE_TEXT + suffix);
            _itemSetIconSize.Click += (sender, e) =>
            {
                string text = DialogUtilities.GetStringFromDialog(labelText: "Enter icon size.");
                float? sizeNullable = ParsingUtilities.ParseFloatNullable(text);
                if (!sizeNullable.HasValue) return;
                MapObjectSettings settings = new MapObjectSettings(
                    changeIconSize: true, newIconSize: sizeNullable.Value);
                GetParentMapTracker().ApplySettings(settings);
            };

            return new List<ToolStripMenuItem>()
            {
                _itemSetIconSize,
            };
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeIconSize)
            {
                _imageSize = settings.NewIconSize;
                string suffix = string.Format(" ({0})", _imageSize);
                _itemSetIconSize.Text = SET_ICON_SIZE_TEXT + suffix;
            }
        }

        public override MapObjectHoverData GetHoverDataTopDownView()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            List<(float centerX, float centerZ, float radius)> dimensionList = Get2DDimensions();
            for (int i = dimensionList.Count - 1; i >= 0 ; i--)
            {
                var dimension = dimensionList[i];

                if (_customImage != null)
                {
                    List<(float x, float z)> positions = MapUtilities.GetFloatPositions(10_000);
                    List<(float x, float z)> controlPositions = positions.ConvertAll(
                        p => MapUtilities.ConvertCoordsForControlTopDownView(p.x, p.z));
                    for (int j = controlPositions.Count - 1; j >= 0; j--)
                    {
                        var position = positions[j];
                        var controlPosition = controlPositions[j];
                        double controlDist = MoreMath.GetDistanceBetween(controlPosition.x, controlPosition.z, relPos.X, relPos.Y);
                        double radius = Scales ? _imageSize * Config.CurrentMapGraphics.MapViewScaleValue : _imageSize;
                        if (controlDist <= radius)
                        {
                            return new MapObjectHoverData(this, position.x, 0, position.z, index: i, index2: j);
                        }
                    }
                }

                double dist = MoreMath.GetDistanceBetween(dimension.centerX, dimension.centerZ, inGameX, inGameZ);
                if (dist <= dimension.radius)
                {
                    return new MapObjectHoverData(this, dimension.centerX, 0, dimension.centerZ, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            if (hoverData.Index2.HasValue)
            {
                List<(float x, float z)> positions = MapUtilities.GetFloatPositions(10_000);
                var position = positions[hoverData.Index2.Value];
                List<double> posValues = new List<double>() { position.x, position.z };
                ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
                output.Insert(0, copyPositionItem);
            }
            else
            {
                var points = GetPoints();
                var point = points[hoverData.Index.Value];
                List<double> posValues = new List<double>() { point.x, point.y, point.z };
                ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
                output.Insert(0, copyPositionItem);
            }

            return output;
        }
    }
}
