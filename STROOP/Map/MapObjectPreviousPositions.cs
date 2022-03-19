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
        private int _turquosieMarioTex = -1;
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
            return Config.ObjectAssociations.PreviousPositionsImage;
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
            Image image = Config.ObjectAssociations.BlueMarioMapImage;
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
            uint startAddress = RomVersionConfig.SwitchMap(0x80372F00, 0x80400010);

            List<int> texValues = new List<int>()
            {
                _pinkMarioTex, // initial
                _yellowMarioTex, // warp_area
                _purpleMarioTex, // check_instant_warp
                _greyMarioTex, // platform displacement
                _turquosieMarioTex, // wall0A
                _greenMarioTex, // wall0B
                _brownMarioTex, // obj interactions
                _orangeMarioTex, // qstep1
                _turquosieMarioTex, // wall1A
                _greenMarioTex, // wall1B
                _blueMarioTex, // floor1
                _orangeMarioTex, // qstep2
                _turquosieMarioTex, // wall2A
                _greenMarioTex, // wall2B
                _blueMarioTex, // floor2
                _orangeMarioTex, // qstep3
                _turquosieMarioTex, // wall3A
                _greenMarioTex, // wall3B
                _blueMarioTex, // floor3
                _orangeMarioTex, // qstep4
                _turquosieMarioTex, // wall4A
                _greenMarioTex, // wall4B
                _blueMarioTex, // floor4
            };

            List<(float x, float y, float z, ushort angle, int tex)> allResults =
                new List<(float x, float y, float z, ushort angle, int tex)>();
            for (int i = 0; i < 23; i++)
            {
                float x = Config.Stream.GetFloat(startAddress + (uint)i * 0x10 + 0x0);
                float y = Config.Stream.GetFloat(startAddress + (uint)i * 0x10 + 0x4);
                float z = Config.Stream.GetFloat(startAddress + (uint)i * 0x10 + 0x8);
                ushort angle = Config.Stream.GetUShort(startAddress + (uint)i * 0x10 + 0xE);
                allResults.Add((x, y, z, angle, texValues[i]));
            }

            int variable = Config.Stream.GetInt(RomVersionConfig.SwitchMap(0x80372E3C, 0x80400000));
            int numQFrames = (variable - 112) / 64;
            int numPoints = 7 + 4 * numQFrames;

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
            if (_turquosieMarioTex == -1)
            {
                _turquosieMarioTex = MapUtilities.LoadTexture(
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

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            var allFrameData = GetAllFrameData();
            for (int i = allFrameData.Count - 1; i >= 0; i--)
            {
                var singleFrameData = allFrameData[i];
                for (int j = singleFrameData.Count - 1; j >= 0; j--)
                {
                    var dataPoint = singleFrameData[j];
                    double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                    double radius = Scales ? Size : Size / Config.CurrentMapGraphics.MapViewScaleValue;
                    if (dist <= radius || forceCursorPosition)
                    {
                        return new MapObjectHoverData(this, dataPoint.x, dataPoint.y, dataPoint.z, index: i, index2: j);
                    }
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
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
                    if (dist <= radius || forceCursorPosition)
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
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(dataPoint.x, dataPoint.y, dataPoint.z, "Position");
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
