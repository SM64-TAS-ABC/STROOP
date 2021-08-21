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
    public class MapObjectHolpDisplayer : MapObject
    {
        private int _tex = -1;

        public MapObjectHolpDisplayer()
            : base()
        {
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.GreenHolpImage;
        }

        public override string GetName()
        {
            return "HOLP Displayer";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Mario.Y;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z)> data = GetData();
            for (int i = 0; i < data.Count; i++)
            {
                var dataPoint = data[i];
                (float x, float y, float z) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size, Scales);
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
                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, OpacityByte);
                GL.LineWidth(LineWidth);
                GL.Begin(PrimitiveType.Lines);
                for (int i = 0; i < data.Count - 1; i++)
                {
                    (float x1, float y1, float z1) = data[i];
                    (float x2, float y2, float z2) = data[i + 1];
                    (float x, float z) vertex1ForControl = MapUtilities.ConvertCoordsForControlTopDownView(x1, z1);
                    (float x, float z) vertex2ForControl = MapUtilities.ConvertCoordsForControlTopDownView(x2, z2);
                    GL.Vertex2(vertex1ForControl.x, vertex1ForControl.z);
                    GL.Vertex2(vertex2ForControl.x, vertex2ForControl.z);
                }
                GL.End();
                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        public override void DrawOn2DControlOrthographicView()
        {
            List<(float x, float y, float z)> data = GetData();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z) = dataPoint;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlOrthographicView(x, y, z);
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                MapUtilities.DrawTexture(_customImageTex ?? _tex, point, size, 0, Opacity);
            }

            if (LineWidth != 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.Color4(LineColor.R, LineColor.G, LineColor.B, OpacityByte);
                GL.LineWidth(LineWidth);
                GL.Begin(PrimitiveType.Lines);
                for (int i = 0; i < data.Count - 1; i++)
                {
                    (float x1, float y1, float z1) = data[i];
                    (float x2, float y2, float z2) = data[i + 1];
                    (float x, float z) vertex1ForControl = MapUtilities.ConvertCoordsForControlOrthographicView(x1, y1, z1);
                    (float x, float z) vertex2ForControl = MapUtilities.ConvertCoordsForControlOrthographicView(x2, y2, z2);
                    GL.Vertex2(vertex1ForControl.x, vertex1ForControl.z);
                    GL.Vertex2(vertex2ForControl.x, vertex2ForControl.z);
                }
                GL.End();
                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        public override void DrawOn3DControl()
        {
            List<(float x, float y, float z)> data = GetData();
            data.Reverse();
            foreach (var dataPoint in data)
            {
                (float x, float y, float z) = dataPoint;

                Matrix4 viewMatrix = GetModelMatrix(x, y, z, 0);
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
                List<(float x, float y, float z)> vertexList = new List<(float x, float y, float z)>();
                for (int i = 0; i < data.Count - 1; i++)
                {
                    (float x1, float y1, float z1) = data[i];
                    (float x2, float y2, float z2) = data[i + 1];
                    vertexList.Add((x1, y1, z1));
                    vertexList.Add((x2, y2, z2));
                }

                Map3DVertex[] vertexArrayForEdges =
                    vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                        vertex.x, vertex.y, vertex.z), LineColor)).ToArray();

                Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArrayForEdges.Length * Map3DVertex.Size),
                    vertexArrayForEdges, BufferUsageHint.DynamicDraw);
                GL.LineWidth(LineWidth);
                Config.Map3DGraphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Lines, 0, vertexArrayForEdges.Length);
                GL.DeleteBuffer(buffer);
            }
        }

        public Matrix4 GetModelMatrix(float x, float y, float z, float ang)
        {
            Image image = GetImage();
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

        public List<(float x, float y, float z)> GetData()
        {
            float marioX = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
            float marioZ = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.ZOffset);
            ushort marioAngle = Config.Stream.GetUShort(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);

            List<(float x, float y, float z)> data = new List<(float x, float y, float z)>();
            for (int i = 0; i < HolpCalculator.STANDING_COUNT; i++)
            {
                data.Add(HolpCalculator.GetHolpForStanding(i, marioX, marioY, marioZ, marioAngle));
            }
            return data;
        }

        public override void Update()
        {
            if (_tex == -1)
            {
                _tex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.GreenHolpImage as Bitmap);
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

        public override MapObjectHoverData GetHoverData()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            List<(float x, float y, float z)> data = GetData();
            int? hoverIndex = null;
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius)
                {
                    hoverIndex = i;
                    break;
                }
            }
            return hoverIndex.HasValue ? new MapObjectHoverData(this, index: hoverIndex) : null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            List<(float x, float y, float z)> data = GetData();
            var dataPoint = data[hoverData.Index.Value];
            List<object> posObjs = new List<object>() { dataPoint.x, dataPoint.y, dataPoint.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posObjs, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
