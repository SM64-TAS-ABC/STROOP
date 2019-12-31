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
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public class Map3NextPositionsObject : Map3Object
    {
        private int _redMarioTex = -1;
        private int _blueMarioTex = -1;
        private int _orangeMarioText = -1;

        private bool _useColoredMarios = true;
        private bool _showQuarterSteps = true;
        private double _numFrames = 4;

        public Map3NextPositionsObject()
            : base()
        {
            InternalRotates = true;
        }

        public override Image GetImage()
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

        public override void DrawOn2DControl()
        {
            List<(float x, float y, float z, float angle, int tex)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, int tex) = dataPoint;
                (float x, float z) positionOnControl = Map3Utilities.ConvertCoordsForControl(x, z);
                float angleDegrees = Rotates ? Map3Utilities.ConvertAngleForControl(angle) : 0;
                SizeF size = Map3Utilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                Map3Utilities.DrawTexture(tex, point, size, angleDegrees, Opacity);
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
                GL.UniformMatrix4(Config.Map4Graphics.GLUniformView, false, ref viewMatrix);

                Map4Vertex[] vertices = GetVertices();
                int vertexBuffer = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * Map4Vertex.Size),
                    vertices, BufferUsageHint.StaticDraw);
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                Config.Map4Graphics.BindVertices();
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

            float angle = (float)MoreMath.AngleUnitsToRadians(ang - SpecialConfig.Map3DCameraYaw + 32768);
            Vector3 pos = new Vector3(x, y, z);

            float size = Size / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(angle)
                * Matrix4.CreateScale(1.0f / Config.Map4Graphics.NormalizedWidth, 1.0f / Config.Map4Graphics.NormalizedHeight, 1)
                * Matrix4.CreateTranslation(Config.Map4Graphics.Utilities.GetPositionOnViewFromCoordinate(pos));
        }
        
        private Map4Vertex[] GetVertices()
        {
            return new Map4Vertex[]
            {
                new Map4Vertex(new Vector3(-1, -1, 0), Color4, new Vector2(0, 1)),
                new Map4Vertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
                new Map4Vertex(new Vector3(-1, 1, 0), Color4, new Vector2(0, 0)),
                new Map4Vertex(new Vector3(1, 1, 0), Color4, new Vector2(1, 0)),
                new Map4Vertex(new Vector3(-1, 1, 0), Color4,  new Vector2(0, 0)),
                new Map4Vertex(new Vector3(1, -1, 0), Color4, new Vector2(1, 1)),
            };
        }

        public List<(float x, float y, float z, float angle, int tex)> GetData()
        {
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float marioYSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YSpeedOffset);
            float marioHSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

            List<(float x, float z)> points2D = Enumerable.Range(0, (int)(_numFrames * 4)).ToList()
                .ConvertAll(index => 0.25 + index / 4.0)
                .ConvertAll(frameStep => ((float x, float z))MoreMath.AddVectorToPoint(
                    frameStep * marioHSpeed, marioAngle, marioX, marioZ));

            int fullStepTex = _useColoredMarios ? _blueMarioTex : _redMarioTex;
            int quarterStepTex = _useColoredMarios ? _orangeMarioText : _redMarioTex;
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
                _redMarioTex = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.MarioMapImage as Bitmap);
            }
            if (_blueMarioTex == -1)
            {
                _blueMarioTex = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.BlueMarioMapImage as Bitmap);
            }
            if (_orangeMarioText == -1)
            {
                _orangeMarioText = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.OrangeMarioMapImage as Bitmap);
            }
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemUseColoredMarios = new ToolStripMenuItem("Use Colored Marios");
                itemUseColoredMarios.Click += (sender, e) =>
                {
                    _useColoredMarios = !_useColoredMarios;
                    itemUseColoredMarios.Checked = _useColoredMarios;
                };
                itemUseColoredMarios.Checked = _useColoredMarios;

                ToolStripMenuItem itemShowQuarterSteps = new ToolStripMenuItem("Show Quarter Steps");
                itemShowQuarterSteps.Click += (sender, e) =>
                {
                    _showQuarterSteps = !_showQuarterSteps;
                    itemShowQuarterSteps.Checked = _showQuarterSteps;
                };
                itemShowQuarterSteps.Checked = _showQuarterSteps;

                ToolStripMenuItem itemSetNumFrames = new ToolStripMenuItem("Set Num Frames...");
                itemSetNumFrames.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter num frames to the nearest 1/4th.");
                    double? numFramesNullable = ParsingUtilities.ParseDoubleNullable(text);
                    if (!numFramesNullable.HasValue) return;
                    double numFrames = numFramesNullable.Value;
                    _numFrames = numFrames;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemUseColoredMarios);
                _contextMenuStrip.Items.Add(itemShowQuarterSteps);
                _contextMenuStrip.Items.Add(itemSetNumFrames);
            }

            return _contextMenuStrip;
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }

        public override Map3DrawType GetDrawType()
        {
            return Map3DrawType.Overlay;
        }
    }
}
