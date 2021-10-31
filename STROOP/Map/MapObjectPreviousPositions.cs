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
using System.Xml.Linq;
using STROOP.Forms;

namespace STROOP.Map
{
    public class MapObjectPreviousPositions : MapObject
    {
        private int _redMarioTex = -1;
        private int _greenMarioTex = -1;
        private int _orangeMarioTex = -1;
        private int _purpleMarioTex = -1;
        private int _blueMarioTex = -1;
        private int _torquosieMarioTex = -1;
        private int _yellowMarioTex = -1;
        private int _pinkMarioTex = -1;
        private int _brownMarioTex = -1;
        private int _whiteMarioTex = -1;
        private int _greyMarioTex = -1;

        private bool _trackHistory;
        private bool _pauseHistory;
        private uint _highestGlobalTimerValue;
        private Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>> _dictionary;

        private ToolStripMenuItem _itemTrackHistory;
        private ToolStripMenuItem _itemPauseHistory;

        private DateTime _showEachPointStartTime = DateTime.MinValue;

        public MapObjectPreviousPositions()
            : base()
        {
            _trackHistory = false;
            _pauseHistory = false;
            _highestGlobalTimerValue = 0;
            _dictionary = new Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>>();

            InternalRotates = true;
        }

        public MapObjectPreviousPositions(Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>> dictionary)
            : this()
        {
            foreach (uint key in dictionary.Keys)
            {
                _dictionary[key] = dictionary[key];
            }
        }

        public static MapObjectPreviousPositions Create(string points)
        {
            Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>> dictionary =
                new Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>>();
            List<string> frames = points.Split('|').ToList();
            foreach (string frame in frames)
            {
                List<(float x, float y, float z, float angle, int tex, bool show)> partsCombined =
                    new List<(float x, float y, float z, float angle, int tex, bool show)>();
                List<string> parts = frame.Split(';').ToList();
                uint key = ParsingUtilities.ParseUInt(parts[0]);
                for (int i = 1; i < parts.Count; i++)
                {
                    string part = parts[i];
                    List<string> values = part.Split(',').ToList();
                    float x = ParsingUtilities.ParseFloat(values[0]);
                    float y = ParsingUtilities.ParseFloat(values[1]);
                    float z = ParsingUtilities.ParseFloat(values[2]);
                    float angle = ParsingUtilities.ParseFloat(values[3]);
                    int tex = ParsingUtilities.ParseInt(values[4]);
                    bool show = ParsingUtilities.ParseBool(values[5]);
                    var valuesCombined = (x, y, z, angle, tex, show);
                    partsCombined.Add(valuesCombined);
                }
                dictionary[key] = partsCombined;
            }
            return new MapObjectPreviousPositions(dictionary);
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.NextPositionsImage;
        }

        public override string GetName()
        {
            return "Previous Positions";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Mario.Y;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            var data = GetAllFrameData();
            for (int i = 0; i < data.Count; i++)
            {
                DrawOn2DControlTopDownView(data[i], i, hoverData);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            var data = GetAllFrameData();
            for (int i = 0; i < data.Count; i++)
            {
                DrawOn2DControlOrthographicView(data[i], i, hoverData);
            }
        }

        public override void DrawOn3DControl()
        {
            foreach (var data in GetAllFrameData())
            {
                DrawOn3DControl(data);
            }
        }

        public void DrawOn2DControlTopDownView(
            List<(float x, float y, float z, float angle, int tex, bool show)> data,
            int index,
            MapObjectHoverData hoverData)
        {
            for (int j = 0; j < data.Count; j++)
            {
                var dataPoint = data[j];
                (float x, float y, float z, float angle, int tex, bool show) = dataPoint;
                if (!show) continue;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && index == hoverData?.Index && j == hoverData?.Index2)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, opacity);
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
                    (float x1, float y1, float z1, float angle1, int tex1, bool show1) = data[i];
                    (float x2, float y2, float z2, float angle2, int tex2, bool show2) = data[i + 1];
                    (float x, float z) vertex1ForControl = MapUtilities.ConvertCoordsForControlTopDownView(x1, z1);
                    (float x, float z) vertex2ForControl = MapUtilities.ConvertCoordsForControlTopDownView(x2, z2);
                    GL.Vertex2(vertex1ForControl.x, vertex1ForControl.z);
                    GL.Vertex2(vertex2ForControl.x, vertex2ForControl.z);
                }
                GL.End();
                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        public void DrawOn2DControlOrthographicView(
            List<(float x, float y, float z, float angle, int tex, bool show)> data,
            int index,
            MapObjectHoverData hoverData)
        {
            for (int j = 0; j < data.Count; j++)
            {
                var dataPoint = data[j];
                (float x, float y, float z, float angle, int tex, bool show) = dataPoint;
                if (!show) continue;
                (float x, float z) positionOnControl = MapUtilities.ConvertCoordsForControlOrthographicView(x, y, z);
                float angleDegrees = Rotates ? MapUtilities.ConvertAngleForControl(angle) : 0;
                SizeF size = MapUtilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size, Scales);
                PointF point = new PointF(positionOnControl.x, positionOnControl.z);
                double opacity = Opacity;
                if (this == hoverData?.MapObject && index == hoverData?.Index && j == hoverData?.Index2)
                {
                    opacity = MapUtilities.GetHoverOpacity();
                }
                MapUtilities.DrawTexture(tex, point, size, angleDegrees, opacity);
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
                    (float x1, float y1, float z1, float angle1, int tex1, bool show1) = data[i];
                    (float x2, float y2, float z2, float angle2, int tex2, bool show2) = data[i + 1];
                    (float x, float z) vertex1ForControl = MapUtilities.ConvertCoordsForControlOrthographicView(x1, y1, z1);
                    (float x, float z) vertex2ForControl = MapUtilities.ConvertCoordsForControlOrthographicView(x2, y2, z2);
                    GL.Vertex2(vertex1ForControl.x, vertex1ForControl.z);
                    GL.Vertex2(vertex2ForControl.x, vertex2ForControl.z);
                }
                GL.End();
                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        public void DrawOn3DControl(List<(float x, float y, float z, float angle, int tex, bool show)> data)
        {
            foreach (var dataPoint in data)
            {
                (float x, float y, float z, float angle, int tex, bool show) = dataPoint;
                if (!show) continue;

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

            if (LineWidth != 0)
            {
                List<(float x, float y, float z)> vertexList = new List<(float x, float y, float z)>();
                for (int i = 0; i < data.Count - 1; i++)
                {
                    (float x1, float y1, float z1, float angle1, int tex1, bool show1) = data[i];
                    (float x2, float y2, float z2, float angle2, int tex2, bool show2) = data[i + 1];
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
            Image image = Config.ObjectAssociations.MarioImage;
            SizeF _imageNormalizedSize = new SizeF(
                image.Width >= image.Height ? 1.0f : (float)image.Width / image.Height,
                image.Width <= image.Height ? 1.0f : (float)image.Height / image.Width);

            float angle = Rotates ? (float)MoreMath.AngleUnitsToRadians(ang - MapConfig.Map3DCameraYaw + 32768) : 0;
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

        public List<List<(float x, float y, float z, float angle, int tex, bool show)>> GetAllFrameData()
        {
            List<List<(float x, float y, float z, float angle, int tex, bool show)>> values =
                new List<List<(float x, float y, float z, float angle, int tex, bool show)>>();
            foreach (var value in _dictionary.Values)
            {
                values.Add(value);
            }
            return values;
        }

        public List<(float x, float y, float z, float angle, int tex, bool show)> GetCurrentFrameData()
        {
            float pos01X = Config.Stream.GetFloat(0x80372F00);
            float pos01Y = Config.Stream.GetFloat(0x80372F04);
            float pos01Z = Config.Stream.GetFloat(0x80372F08);
            float pos01A = Config.Stream.GetUShort(0x80372F0E);

            float pos02X = Config.Stream.GetFloat(0x80372F10);
            float pos02Y = Config.Stream.GetFloat(0x80372F14);
            float pos02Z = Config.Stream.GetFloat(0x80372F18);
            float pos02A = Config.Stream.GetUShort(0x80372F1E);

            float pos03X = Config.Stream.GetFloat(0x80372F20);
            float pos03Y = Config.Stream.GetFloat(0x80372F24);
            float pos03Z = Config.Stream.GetFloat(0x80372F28);
            float pos03A = Config.Stream.GetUShort(0x80372F2E);

            float pos04X = Config.Stream.GetFloat(0x80372F30);
            float pos04Y = Config.Stream.GetFloat(0x80372F34);
            float pos04Z = Config.Stream.GetFloat(0x80372F38);
            float pos04A = Config.Stream.GetUShort(0x80372F3E);

            float pos05X = Config.Stream.GetFloat(0x80372F40);
            float pos05Y = Config.Stream.GetFloat(0x80372F44);
            float pos05Z = Config.Stream.GetFloat(0x80372F48);
            float pos05A = Config.Stream.GetUShort(0x80372F4E);

            float pos06X = Config.Stream.GetFloat(0x80372F50);
            float pos06Y = Config.Stream.GetFloat(0x80372F54);
            float pos06Z = Config.Stream.GetFloat(0x80372F58);
            float pos06A = Config.Stream.GetUShort(0x80372F5E);

            float pos07X = Config.Stream.GetFloat(0x80372F60);
            float pos07Y = Config.Stream.GetFloat(0x80372F64);
            float pos07Z = Config.Stream.GetFloat(0x80372F68);
            float pos07A = Config.Stream.GetUShort(0x80372F6E);

            float pos08X = Config.Stream.GetFloat(0x80372F70);
            float pos08Y = Config.Stream.GetFloat(0x80372F74);
            float pos08Z = Config.Stream.GetFloat(0x80372F78);
            float pos08A = Config.Stream.GetUShort(0x80372F7E);

            float pos09X = Config.Stream.GetFloat(0x80372F80);
            float pos09Y = Config.Stream.GetFloat(0x80372F84);
            float pos09Z = Config.Stream.GetFloat(0x80372F88);
            float pos09A = Config.Stream.GetUShort(0x80372F8E);

            float pos10X = Config.Stream.GetFloat(0x80372F90);
            float pos10Y = Config.Stream.GetFloat(0x80372F94);
            float pos10Z = Config.Stream.GetFloat(0x80372F98);
            float pos10A = Config.Stream.GetUShort(0x80372F9E);

            float pos11X = Config.Stream.GetFloat(0x80372FA0);
            float pos11Y = Config.Stream.GetFloat(0x80372FA4);
            float pos11Z = Config.Stream.GetFloat(0x80372FA8);
            float pos11A = Config.Stream.GetUShort(0x80372FAE);

            float pos12X = Config.Stream.GetFloat(0x80372FB0);
            float pos12Y = Config.Stream.GetFloat(0x80372FB4);
            float pos12Z = Config.Stream.GetFloat(0x80372FB8);
            float pos12A = Config.Stream.GetUShort(0x80372FBE);

            float pos13X = Config.Stream.GetFloat(0x80372FC0);
            float pos13Y = Config.Stream.GetFloat(0x80372FC4);
            float pos13Z = Config.Stream.GetFloat(0x80372FC8);
            float pos13A = Config.Stream.GetUShort(0x80372FCE);

            float pos14X = Config.Stream.GetFloat(0x80372FD0);
            float pos14Y = Config.Stream.GetFloat(0x80372FD4);
            float pos14Z = Config.Stream.GetFloat(0x80372FD8);
            float pos14A = Config.Stream.GetUShort(0x80372FDE);

            float pos15X = Config.Stream.GetFloat(0x80372FE0);
            float pos15Y = Config.Stream.GetFloat(0x80372FE4);
            float pos15Z = Config.Stream.GetFloat(0x80372FE8);
            float pos15A = Config.Stream.GetUShort(0x80372FEE);

            float pos16X = Config.Stream.GetFloat(0x80372FF0);
            float pos16Y = Config.Stream.GetFloat(0x80372FF4);
            float pos16Z = Config.Stream.GetFloat(0x80372FF8);
            float pos16A = Config.Stream.GetUShort(0x80372FFE);

            float pos17X = Config.Stream.GetFloat(0x80373000);
            float pos17Y = Config.Stream.GetFloat(0x80373004);
            float pos17Z = Config.Stream.GetFloat(0x80373008);
            float pos17A = Config.Stream.GetUShort(0x8037300E);

            float pos18X = Config.Stream.GetFloat(0x80373010);
            float pos18Y = Config.Stream.GetFloat(0x80373014);
            float pos18Z = Config.Stream.GetFloat(0x80373018);
            float pos18A = Config.Stream.GetUShort(0x8037301E);

            float pos19X = Config.Stream.GetFloat(0x80373020);
            float pos19Y = Config.Stream.GetFloat(0x80373024);
            float pos19Z = Config.Stream.GetFloat(0x80373028);
            float pos19A = Config.Stream.GetUShort(0x8037302E);

            float pos20X = Config.Stream.GetFloat(0x80373030);
            float pos20Y = Config.Stream.GetFloat(0x80373034);
            float pos20Z = Config.Stream.GetFloat(0x80373038);
            float pos20A = Config.Stream.GetUShort(0x8037303E);

            float pos21X = Config.Stream.GetFloat(0x80373040);
            float pos21Y = Config.Stream.GetFloat(0x80373044);
            float pos21Z = Config.Stream.GetFloat(0x80373048);
            float pos21A = Config.Stream.GetUShort(0x8037304E);

            float pos22X = Config.Stream.GetFloat(0x80373050);
            float pos22Y = Config.Stream.GetFloat(0x80373054);
            float pos22Z = Config.Stream.GetFloat(0x80373058);
            float pos22A = Config.Stream.GetUShort(0x8037305E);

            float pos23X = Config.Stream.GetFloat(0x80373060);
            float pos23Y = Config.Stream.GetFloat(0x80373064);
            float pos23Z = Config.Stream.GetFloat(0x80373068);
            float pos23A = Config.Stream.GetUShort(0x8037306E);

            int variable = Config.Stream.GetInt(0x80372E3C);
            int numQFrames = (variable - 112) / 64;
            int numPoints = 7 + 4 * numQFrames;

            List<(float x, float y, float z, float angle, int tex)> allResults =
                new List<(float x, float y, float z, float angle, int tex)>()
                {
                    (pos01X, pos01Y, pos01Z, pos01A, _pinkMarioTex), // initial
                    (pos02X, pos02Y, pos02Z, pos02A, _yellowMarioTex), // warp_area
                    (pos03X, pos03Y, pos03Z, pos03A, _purpleMarioTex), // check_instant_warp
                    (pos04X, pos04Y, pos04Z, pos04A, _greyMarioTex), // platform displacement
                    (pos05X, pos05Y, pos05Z, pos05A, _torquosieMarioTex), // wall0A
                    (pos06X, pos06Y, pos06Z, pos06A, _greenMarioTex), // wall0B
                    (pos07X, pos07Y, pos07Z, pos07A, _brownMarioTex), // obj interactions
                    (pos08X, pos08Y, pos08Z, pos08A, _orangeMarioTex), // qstep1
                    (pos09X, pos09Y, pos09Z, pos09A, _torquosieMarioTex), // wall1A
                    (pos10X, pos10Y, pos10Z, pos10A, _greenMarioTex), // wall1B
                    (pos11X, pos11Y, pos11Z, pos11A, _blueMarioTex), // floor1
                    (pos12X, pos12Y, pos12Z, pos12A, _orangeMarioTex), // qstep2
                    (pos13X, pos13Y, pos13Z, pos13A, _torquosieMarioTex), // wall2A
                    (pos14X, pos14Y, pos14Z, pos14A, _greenMarioTex), // wall2B
                    (pos15X, pos15Y, pos15Z, pos15A, _blueMarioTex), // floor2
                    (pos16X, pos16Y, pos16Z, pos16A, _orangeMarioTex), // qstep3
                    (pos17X, pos17Y, pos17Z, pos17A, _torquosieMarioTex), // wall3A
                    (pos18X, pos18Y, pos18Z, pos18A, _greenMarioTex), // wall3B
                    (pos19X, pos19Y, pos19Z, pos19A, _blueMarioTex), // floor3
                    (pos20X, pos20Y, pos20Z, pos20A, _orangeMarioTex), // qstep4
                    (pos21X, pos21Y, pos21Z, pos21A, _torquosieMarioTex), // wall4A
                    (pos22X, pos22Y, pos22Z, pos22A, _greenMarioTex), // wall4B
                    (pos23X, pos23Y, pos23Z, pos23A, _blueMarioTex), // floor4
                };

            double secondsPerPoint = 0.5;
            double totalSeconds = secondsPerPoint * numPoints;
            double elapsedSeconds = DateTime.Now.Subtract(_showEachPointStartTime).TotalSeconds;
            int? pointToShow;
            if (elapsedSeconds < totalSeconds)
            {
                pointToShow = (int)(elapsedSeconds / secondsPerPoint);
            }
            else
            {
                _showEachPointStartTime = DateTime.MinValue;
                pointToShow = null;
            }

            List<(float x, float y, float z, float angle, int tex, bool show)> partialResults =
                new List<(float x, float y, float z, float angle, int tex, bool show)>();
            for (int i = 0; i < Math.Min(numPoints, allResults.Count); i++)
            {
                (float x, float y, float z, float angle, int tex) = allResults[i];
                bool show = pointToShow.HasValue ? i == pointToShow.Value : true;
                partialResults.Add((x, y, z, angle, tex, show));
            }

            partialResults.Reverse();
            return partialResults;
        }

        public override void Update()
        {
            if (_redMarioTex == -1)
            {
                _redMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.MarioMapImage as Bitmap);
            }
            if (_greenMarioTex == -1)
            {
                _greenMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.GreenMarioMapImage as Bitmap);
            }
            if (_orangeMarioTex == -1)
            {
                _orangeMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.OrangeMarioMapImage as Bitmap);
            }
            if (_purpleMarioTex == -1)
            {
                _purpleMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.PurpleMarioMapImage as Bitmap);
            }
            if (_blueMarioTex == -1)
            {
                _blueMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.BlueMarioMapImage as Bitmap);
            }
            if (_torquosieMarioTex == -1)
            {
                _torquosieMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.TurqoiseMarioMapImage as Bitmap);
            }
            if (_yellowMarioTex == -1)
            {
                _yellowMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.YellowMarioMapImage as Bitmap);
            }
            if (_pinkMarioTex == -1)
            {
                _pinkMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.PinkMarioMapImage as Bitmap);
            }
            if (_brownMarioTex == -1)
            {
                _brownMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.BrownMarioMapImage as Bitmap);
            }
            if (_whiteMarioTex == -1)
            {
                _whiteMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.WhiteMarioMapImage as Bitmap);
            }
            if (_greyMarioTex == -1)
            {
                _greyMarioTex = MapUtilities.LoadTexture(
                    Config.ObjectAssociations.GreyMarioMapImage as Bitmap);
            }

            if (!_pauseHistory)
            {
                if (!_trackHistory) _dictionary.Clear();

                uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                List<(float x, float y, float z, float angle, int tex, bool show)> data = GetCurrentFrameData();

                if (globalTimer < _highestGlobalTimerValue)
                {
                    Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>> tempDictionary =
                        new Dictionary<uint, List<(float x, float y, float z, float angle, int tex, bool show)>>();
                    foreach (uint key in _dictionary.Keys)
                    {
                        tempDictionary[key] = _dictionary[key];
                    }
                    _dictionary.Clear();
                    foreach (uint key in tempDictionary.Keys)
                    {
                        if (key <= globalTimer)
                        {
                            _dictionary[key] = tempDictionary[key];
                            _highestGlobalTimerValue = key;
                        }
                    }
                }

                if (!_dictionary.ContainsKey(globalTimer))
                {
                    _dictionary[globalTimer] = data;
                    _highestGlobalTimerValue = globalTimer;
                }
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

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemShowEachPoint = new ToolStripMenuItem("Show Each Point");
                itemShowEachPoint.Click += (sender, e) =>
                {
                    _showEachPointStartTime = DateTime.Now;
                };

                _itemTrackHistory = new ToolStripMenuItem("Track History");
                _itemTrackHistory.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePreviousPositionsTrackHistory: true, newPreviousPositionsTrackHistory: !_trackHistory);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemPauseHistory = new ToolStripMenuItem("Pause History");
                _itemPauseHistory.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePreviousPositionsPauseHistory: true, newPreviousPositionsPauseHistory: !_pauseHistory);
                    GetParentMapTracker().ApplySettings(settings);
                };

                ToolStripMenuItem itemClearHistory = new ToolStripMenuItem("Clear History");
                itemClearHistory.Click += (sender, e) =>
                {
                    _dictionary.Clear();
                };

                ToolStripMenuItem itemSeeKey = new ToolStripMenuItem("See Key");
                itemSeeKey.Click += (sender, e) =>
                {
                    InfoForm.ShowValue(GetKeyString(), "Previous Positions", "Key");
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemShowEachPoint);
                _contextMenuStrip.Items.Add(_itemTrackHistory);
                _contextMenuStrip.Items.Add(_itemPauseHistory);
                _contextMenuStrip.Items.Add(itemClearHistory);
                _contextMenuStrip.Items.Add(itemSeeKey);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.ChangePreviousPositionsTrackHistory)
            {
                _trackHistory = settings.NewPreviousPositionsTrackHistory;
                _itemTrackHistory.Checked = settings.NewPreviousPositionsTrackHistory;
            }

            if (settings.ChangePreviousPositionsPauseHistory)
            {
                _pauseHistory = settings.NewPreviousPositionsPauseHistory;
                _itemPauseHistory.Checked = settings.NewPreviousPositionsPauseHistory;
            }
        }

        public override MapObjectHoverData GetHoverDataTopDownView()
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe();
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            var allFrameData = GetAllFrameData();
            for (int i = allFrameData.Count - 1; i >= 0; i--)
            {
                var singleFrameData = allFrameData[i];
                for (int j = singleFrameData.Count - 1; j >= 0; j--)
                {
                    var dataPoint = singleFrameData[j];
                    double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                    double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                    if (dist <= radius)
                    {
                        return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i, index2: j);
                    }
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView()
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe();
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            var allFrameData = GetAllFrameData();
            for (int i = allFrameData.Count - 1; i >= 0; i--)
            {
                var singleFrameData = allFrameData[i];
                for (int j = singleFrameData.Count - 1; j >= 0; j--)
                {
                    var dataPoint = singleFrameData[j];
                    (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(dataPoint.x, dataPoint.y, dataPoint.z);
                    double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                    double radius = Scales ? Size * Config.CurrentMapGraphics.MapViewScaleValue : Size;
                    if (dist <= radius)
                    {
                        return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i, index2: j);
                    }
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            var data = GetAllFrameData();
            var dataPoint = data[hoverData.Index.Value][hoverData.Index2.Value];
            List<double> posValues = new List<double>() { dataPoint.x, dataPoint.y, dataPoint.z };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posValues, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = new List<string>();
            foreach (uint key in _dictionary.Keys)
            {
                List<(float x, float y, float z, float angle, int tex, bool show)> values = _dictionary[key];
                List<object> frameData = values.ConvertAll(value =>
                {
                    List<object> parts = new List<object>()
                    {
                        (double)value.x,
                        (double)value.y,
                        (double)value.z,
                        (double)value.angle,
                        value.tex,
                        value.show,
                    };
                    return (object)string.Join(",", parts);
                });
                frameData.Insert(0, key);
                pointList.Add(string.Join(";", frameData));
            }
            return new List<XAttribute>()
            {
                new XAttribute("points", string.Join("|", pointList)),
            };
        }

        public (float x, float y, float z) GetMidpoint()
        {
            List<float> xValues = new List<float>();
            List<float> yValues = new List<float>();
            List<float> zValues = new List<float>();

            var allFrameData = GetAllFrameData();
            foreach (var singleFrameData in allFrameData)
            {
                foreach (var data in singleFrameData)
                {
                    xValues.Add(data.x);
                    yValues.Add(data.y);
                    zValues.Add(data.z);
                }
            }

            if (xValues.Count == 0) return (0, 0, 0);

            float xMin = xValues.Min();
            float xMax = xValues.Max();
            float yMin = yValues.Min();
            float yMax = yValues.Max();
            float zMin = zValues.Min();
            float zMax = zValues.Max();

            float xMidpoint = (xMin + xMax) / 2;
            float yMidpoint = (yMin + yMax) / 2;
            float zMidpoint = (zMin + zMax) / 2;

            return (xMidpoint, yMidpoint, zMidpoint);
        }

        public static string GetKeyString()
        {
            List<string> stringList = new List<string>()
            {
                "[01] pink: initial",
                "[02] yellow: warp_area",
                "[03] purple: check_instant_warp",
                "[04] grey: platform displacement",
                "[05] turquoise: wall0A",
                "[06] green: wall0B",
                "[07] brown: obj interactions",
                "",
                "[08] orange: qstep1",
                "[09] turquoise: wall1A",
                "[10] green: wall1B",
                "[11] blue: floor1",
                "",
                "[12] orange: qstep2",
                "[13] turquoise: wall2A",
                "[14] green: wall2B",
                "[15] blue: floor2",
                "",
                "[16] orange: qstep3",
                "[17] turquoise: wall3A",
                "[18] green: wall3B",
                "[19] blue: floor3",
                "",
                "[20] orange: qstep4",
                "[21] turquoise: wall4A",
                "[22] green: wall4B",
                "[23] blue: floor4",
            };
            return string.Join("\r\n", stringList);
        }
    }
}
