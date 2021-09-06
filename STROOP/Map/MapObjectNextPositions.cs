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
    public class MapObjectNextPositions : MapObject
    {
        private int _redMarioTex = -1;
        private int _blueMarioTex = -1;
        private int _orangeMarioTex = -1;

        private bool _useColoredMarios = true;
        private bool _showQuarterSteps = true;
        private double _numFrames = 4;

        private ToolStripMenuItem _itemUseColoredMarios;
        private ToolStripMenuItem _itemShowQuarterSteps;
        private ToolStripMenuItem _itemSetNumFrames;

        private static readonly string SET_NUM_FRAMES_TEXT = "Set Num Frames";

        public MapObjectNextPositions()
            : base()
        {
            InternalRotates = true;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.NextPositionsImage;
        }

        public override string GetName()
        {
            return "Next Positions";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Mario.Y;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z, float angle, int tex)> data = GetData();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                (float x, float y, float z, float angle, int tex) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, opacity);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z, float angle, int tex)> data = GetData();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                (float x, float y, float z, float angle, int tex) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlOrthographicView(x, y, z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, opacity);
            }
        }

        public override void DrawOn3DControl()
        {
            List<(float x, float y, float z, float angle, int tex)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, int tex) = dataPoint;

                Matrix4 viewMatrix = GetModelMatrix(x, y, z, angle);
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                Map3DVertex[] vertices = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map3DVertex.Size),
                    vertices, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Length);
                GL.DeleteBuffer(vertexBuffer);
            }
        }
        
        public Matrix4 GetModelMatrix(float x, float y, float z, float ang)
        {
            Image image = Config.ObjectAssociations.BlueMarioMapImage;
            SizeF _imageNormalizedSize = new SizeF(
                image.Width >= image.Height ? 1.0f : (float)image.Width / image.Height,
                image.Width <= image.Height ? 1.0f : (float)image.Height / image.Width);

            float angle = Rotates ? (float)MoreMath.AngleUnitsToRadians(ang - SpecialConfig.Map3DCameraYaw + 32768) : 0;
            Vector3 pos = new Vector3(x, y, z);

            float size = Size / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(angle)
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

        public List<(float x, float y, float z, float angle, int tex)> GetData()
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float marioYSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
            float marioHSpeed = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort preAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            ushort marioAngle = MoreMath.NormalizeAngleTruncated(preAngle);

            float floorY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.FloorYOffset);
            float multiplier = 1;
            if (marioY == floorY) // on the ground
            {
                uint floorTri = Config.Stream.GetUInt(MarioConfig.StructAddress + MarioConfig.FloorTriangleOffset);
                float yNorm = Config.Stream.GetFloat(floorTri + TriangleOffsetsConfig.NormY);
                multiplier = yNorm;
            }
            float effectiveSpeed = marioHSpeed * multiplier;

            List<(float x, float z)> points2D = Enumerable.Range(0, (int)(_numFrames * 4)).ToList()
                .ConvertAll(index => 0.25 + index / 4.0)
                .ConvertAll(frameStep => ((float x, float z))MoreMath.AddVectorToPoint(
                    frameStep * effectiveSpeed, marioAngle, marioX, marioZ));

            int fullStepTex = _useColoredMarios ? _blueMarioTex : _redMarioTex;
            int quarterStepTex = _useColoredMarios ? _orangeMarioTex : _redMarioTex;
            List<(float x, float y, float z, float angle, int tex)> data =
                new List<(float x, float y, float z, float angle, int tex)>();
            for (int i = 0; i < points2D.Count; i++)
            {
                bool isFullStep = i % 4 == 3;
                if (!isFullStep && !_showQuarterSteps) continue;
                (float x, float z) = points2D[i];
                int tex = isFullStep ? fullStepTex : quarterStepTex;
                data.Add((x, marioY, z, marioAngle, tex));
            }
            return data;
        }

        public override void Update()
        {
            if (_redMarioTex == -1)
            {
                _redMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.MarioMapImage as Bitmap);
            }
            if (_blueMarioTex == -1)
            {
                _blueMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.BlueMarioMapImage as Bitmap);
            }
            if (_orangeMarioTex == -1)
            {
                _orangeMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.OrangeMarioMapImage as Bitmap);
            }
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _itemUseColoredMarios = new ToolStripMenuItem("Use Colored Marios");
                _itemUseColoredMarios.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeNextPositionsUseColoredMarios: true, newNextPositionsUseColoredMarios: !_useColoredMarios);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUseColoredMarios.Checked = _useColoredMarios;

                _itemShowQuarterSteps = new ToolStripMenuItem("Show Quarter Steps");
                _itemShowQuarterSteps.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeShowQuarterSteps: true, newShowQuarterSteps: !_showQuarterSteps);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemShowQuarterSteps.Checked = _showQuarterSteps;

                string suffix = string.Format(" ({0})", _numFrames);
                _itemSetNumFrames = new ToolStripMenuItem(SET_NUM_FRAMES_TEXT + suffix);
                _itemSetNumFrames.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter num frames to the nearest 1/4th.");
                    double? numFramesNullable = ParsingUtilities.ParseDoubleNullable(text);
                    if (!numFramesNullable.HasValue) return;
                    double numFrames = numFramesNullable.Value;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeNextPositionsNumFrames: true, newNextPositionsNumFrames: numFrames);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(_itemUseColoredMarios);
                _contextMenuStrip.Items.Add(_itemShowQuarterSteps);
                _contextMenuStrip.Items.Add(_itemSetNumFrames);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangeNextPositionsUseColoredMarios)
            {
                _useColoredMarios = settings.NewNextPositionsUseColoredMarios;
                _itemUseColoredMarios.Checked = settings.NewNextPositionsUseColoredMarios;
            }

            if (settings.ChangeShowQuarterSteps)
            {
                _showQuarterSteps = settings.NewShowQuarterSteps;
                _itemShowQuarterSteps.Checked = settings.NewShowQuarterSteps;
            }

            if (settings.ChangeNextPositionsNumFrames)
            {
                _numFrames = settings.NewNextPositionsNumFrames;
                string suffix = string.Format(" ({0})", settings.NewNextPositionsNumFrames);
                _itemSetNumFrames.Text = SET_NUM_FRAMES_TEXT + suffix;
            }
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override MapObjectHoverData GetHoverDataTopDownView()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            var data = GetData();
            for (int i = 0; i < data.Count; i++)
            {
                var dataPoint = data[i];
                double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius)
                {
                    return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            var data = GetData();
            for (int i = 0; i < data.Count; i++)
            {
                var dataPoint = data[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(dataPoint.x, dataPoint.y, dataPoint.z);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                if (dist <= radius)
                {
                    return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var data = GetData();
            var dataPoint = data[hoverData.Index.Value];
            List<double> posValues = new List<double>() { dataPoint.x, dataPoint.y, dataPoint.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
