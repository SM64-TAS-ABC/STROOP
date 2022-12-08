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

namespace STROOP.Map
{
    public class MapObjectPath : MapObject
    {
        private static readonly float QSTEP_RATIO = 0.5f;

        private readonly PositionAngle _posAngle;
        private readonly Dictionary<uint, (float x, float y, float z)> _dictionary;
        private (byte level, byte area, ushort loadingPoint, ushort missionLayout) _currentLocationStats;
        private bool _resetPathOnLevelChange;
        private int _numSkips;
        private List<uint> _skippedKeys;
        private bool _useBlending;
        private bool _isPaused;
        private bool _truncatePoints;
        private bool _useValueAtStartOfGlobalTimer;
        private uint _highestGlobalTimerValue;
        private int _modulo;
        private bool _deduplicatePoints;
        private bool _showQuarterSteps;
        private float _imageSize;

        private ToolStripMenuItem _itemResetPathOnLevelChange;
        private ToolStripMenuItem _itemUseBlending;
        private ToolStripMenuItem _itemPause;
        private ToolStripMenuItem _itemTruncatePoints;
        private ToolStripMenuItem _itemUseValueAtStartOfGlobalTimer;
        private ToolStripMenuItem _itemSetModulo;
        private ToolStripMenuItem _itemDeduplicatePoints;
        private ToolStripMenuItem _itemShowQuarterSteps;
        private ToolStripMenuItem _itemSetIconSize;

        private static readonly string SET_MODULO_TEXT = "Set Modulo";
        private static readonly string SET_ICON_SIZE_TEXT = "Set Icon Size";

        public MapObjectPath(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _dictionary = new Dictionary<uint, (float x, float y, float z)>();
            _currentLocationStats = Config.MapAssociations.GetCurrentLocationStats();
            _resetPathOnLevelChange = false;
            _numSkips = 0;
            _skippedKeys = new List<uint>();
            _useBlending = true;
            _isPaused = false;
            _truncatePoints = false;
            _useValueAtStartOfGlobalTimer = true;
            _highestGlobalTimerValue = 0;
            _modulo = 1;
            _deduplicatePoints = false;
            _imageSize = 10;

            Size = 300;
            LineWidth = 3;
            Color = Color.Yellow;
            LineColor = Color.Red;
        }

        public MapObjectPath(PositionAngle posAngle, List<(uint globalTimer, float x, float y, float z)> points) : this(posAngle)
        {
            foreach (var p in points)
            {
                _dictionary[p.globalTimer] = (p.x, p.y, p.z);
            }
        }

        public static MapObjectPath Create(PositionAngle posAngle, string pointString)
        {
            List<double?> doubleListNullable = ParsingUtilities.ParseDoubleList(pointString);
            if (doubleListNullable.Any(d => !d.HasValue)) return null;
            List<double> doubleList = doubleListNullable.ConvertAll(d => d.Value);
            if (doubleList.Count % 4 != 0) return null;
            List<(uint globalTimer, float x, float y, float z)> points = new List<(uint globalTimer, float x, float y, float z)>();
            for (int i = 0; i < doubleList.Count; i += 4)
            {
                points.Add(((uint)doubleList[i], (float)doubleList[i + 1], (float)doubleList[i + 2], (float)doubleList[i + 3]));
            }
            return new MapObjectPath(posAngle, points);
        }

        private List<(float x, float y, float z)> GetDictionaryValues()
        {
            return _dictionary.Keys.ToList()
                .FindAll(key => key % _modulo == 0)
                .ConvertAll(key => _dictionary[key])
                .ConvertAll(v => _truncatePoints ? ((int)v.x, (int)v.y, (int)v.z) : v);
        }

        public List<MapObjectPathSegment> GetSegments()
        {
            List<MapObjectPathSegment> segments = new List<MapObjectPathSegment>();

            if (LineWidth == 0) return segments;

            List<(float x, float y, float z)> vertices = GetDictionaryValues();
            List<(float x, float z)> verticesForControl =
                vertices.ConvertAll(vertex => MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z, UseRelativeCoordinates));

            for (int i = 0; i < verticesForControl.Count - 1; i++)
            {
                Color color = LineColor;
                if (_useBlending)
                {
                    int distFromEnd = verticesForControl.Count - i - 2;
                    if (distFromEnd < Size)
                    {
                        color = ColorUtilities.InterpolateColor(
                            LineColor, Color, distFromEnd / (double)Size);
                    }
                    else
                    {
                        color = Color;
                    }
                }
                (float x1, float z1) = verticesForControl[i];
                (float x2, float z2) = verticesForControl[i + 1];
                MapObjectPathSegment segment = new MapObjectPathSegment(
                    index: i,
                    startX: x1,
                    startZ: z1,
                    endX: x2,
                    endZ: z2,
                    lineWidth: LineWidth,
                    color: color,
                    opacity: OpacityByte);
                segments.Add(segment);
            }

            return segments;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z)> vertices = GetDictionaryValues();
            List<(float x, float z)> verticesForControl =
                vertices.ConvertAll(vertex => MapUtilities.ConvertCoordsForControlTopDownView(vertex.x, vertex.z, UseRelativeCoordinates));

            if (LineWidth != 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.LineWidth(LineWidth);
                for (int i = 0; i < verticesForControl.Count - 1; i++)
                {
                    Color color = LineColor;
                    if (_useBlending)
                    {
                        int distFromEnd = verticesForControl.Count - i - 2;
                        if (distFromEnd < Size)
                        {
                            color = ColorUtilities.InterpolateColor(
                                LineColor, Color, distFromEnd / (double)Size);
                        }
                        else
                        {
                            color = Color;
                        }
                    }
                    (float x1, float z1) = verticesForControl[i];
                    (float x2, float z2) = verticesForControl[i + 1];
                    GL.Color4(color.R, color.G, color.B, OpacityByte);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(x1, z1);
                    GL.Vertex2(x2, z2);
                    GL.End();
                }
                GL.Color4(1, 1, 1, 1.0f);
            }

            if (_customImage != null)
            {
                List<(float x, float z)> imagePoints =
                    _showQuarterSteps ? MapUtilities.InterpolateQuarterSteps(verticesForControl) : verticesForControl;
                for (int i = 0; i < imagePoints.Count; i++)
                {
                    (float x, float z) = imagePoints[i];
                    float imageSize = _showQuarterSteps && i % 4 != 0 ? _imageSize * QSTEP_RATIO : _imageSize;
                    SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, imageSize, Scales);
                    double opacity = Opacity;
                    if (this == hoverData?.MapObject && i == hoverData?.Index)
                    {
                        opacity = MapUtilities.GetHoverOpacity();
                    }
                    MapUtilities.DrawTexture(_customImageTex.Value, new PointF(x, z), size, 0, opacity);
                }
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<(float x, float y, float z)> vertices = GetDictionaryValues();
            List<(float x, float z)> verticesForControl =
                vertices.ConvertAll(vertex => MapUtilities.ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z, UseRelativeCoordinates));

            if (LineWidth != 0)
            {
                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();
                GL.LineWidth(LineWidth);
                for (int i = 0; i < verticesForControl.Count - 1; i++)
                {
                    Color color = LineColor;
                    if (_useBlending)
                    {
                        int distFromEnd = verticesForControl.Count - i - 2;
                        if (distFromEnd < Size)
                        {
                            color = ColorUtilities.InterpolateColor(
                                LineColor, Color, distFromEnd / (double)Size);
                        }
                        else
                        {
                            color = Color;
                        }
                    }
                    (float x1, float z1) = verticesForControl[i];
                    (float x2, float z2) = verticesForControl[i + 1];
                    GL.Color4(color.R, color.G, color.B, OpacityByte);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex2(x1, z1);
                    GL.Vertex2(x2, z2);
                    GL.End();
                }
                GL.Color4(1, 1, 1, 1.0f);
            }

            if (_customImage != null)
            {
                List<(float x, float z)> imagePoints =
                    _showQuarterSteps ? MapUtilities.InterpolateQuarterSteps(verticesForControl) : verticesForControl;
                for (int i = 0; i < imagePoints.Count; i++)
                {
                    (float x, float z) = imagePoints[i];
                    float imageSize = _showQuarterSteps && i % 4 != 0 ? _imageSize * QSTEP_RATIO : _imageSize;
                    SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, imageSize, Scales);
                    double opacity = Opacity;
                    if (this == hoverData?.MapObject && i == hoverData?.Index)
                    {
                        opacity = MapUtilities.GetHoverOpacity();
                    }
                    MapUtilities.DrawTexture(_customImageTex.Value, new PointF(x, z), size, 0, opacity);
                }
            }
        }

        public override void DrawOn3DControl()
        {
            List<(float x, float y, float z)> vertices = GetDictionaryValues();

            if (LineWidth != 0)
            {
                List<Map3DVertex[]> vertexArrayList = new List<Map3DVertex[]>();
                for (int i = 0; i < vertices.Count - 1; i++)
                {
                    Color color = LineColor;
                    if (_useBlending)
                    {
                        int distFromEnd = vertices.Count - i - 2;
                        if (distFromEnd < Size)
                        {
                            color = ColorUtilities.InterpolateColor(
                                LineColor, Color, distFromEnd / (double)Size);
                        }
                        else
                        {
                            color = Color;
                        }
                    }
                    (float x1, float y1, float z1) = vertices[i];
                    (float x2, float y2, float z2) = vertices[i + 1];

                    vertexArrayList.Add(new Map3DVertex[]
                    {
                    new Map3DVertex(new Vector3(x1, y1, z1), color),
                    new Map3DVertex(new Vector3(x2, y2, z2), color),
                    });
                }

                Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                vertexArrayList.ForEach(vertexes =>
                {
                    int buffer = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(LineWidth);
                    Config.Map3DGraphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.Lines, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer);
                });
            }

            if (_customImage != null)
            {
                List<(float x, float y, float z)> imagePoints =
                    _showQuarterSteps ? MapUtilities.InterpolateQuarterSteps(vertices) : vertices;
                for (int i = 0; i < imagePoints.Count; i++)
                {
                    (float x, float y, float z) = imagePoints[i];
                    float imageSize = _showQuarterSteps && i % 4 != 0 ? _imageSize * QSTEP_RATIO : _imageSize;

                    Matrix4 viewMatrix = GetModelMatrix(x, y, z, 0, imageSize);
                    GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                    Map3DVertex[] vertices2 = GetVertices();
                    int vertexBuffer = GL.GenBuffer();
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices2.Length * Map3DVertex.Size),
                        vertices2, BufferUsageHint.StaticDraw);
                    GL.BindTexture(TextureTarget.Texture2D, _customImageTex.Value);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer);
                    Config.Map3DGraphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.Triangles, 0, vertices2.Length);
                    GL.DeleteBuffer(vertexBuffer);
                }
            }
        }

        public Matrix4 GetModelMatrix(float x, float y, float z, float ang, float imageSize)
        {
            SizeF _imageNormalizedSize = new SizeF(
                _customImage.Width >= _customImage.Height ? 1.0f : (float)_customImage.Width / _customImage.Height,
                _customImage.Width <= _customImage.Height ? 1.0f : (float)_customImage.Height / _customImage.Width);

            Vector3 pos = new Vector3(x, y, z);

            float size = imageSize / 200;
            return Matrix4.CreateScale(size * _imageNormalizedSize.Width, size * _imageNormalizedSize.Height, 1)
                * Matrix4.CreateRotationZ(0)
                * Matrix4.CreateScale(1.0f / Config.Map3DGraphics.NormalizedWidth, 1.0f / Config.Map3DGraphics.NormalizedHeight, 1)
                * Matrix4.CreateTranslation(MapUtilities.GetPositionOnViewFromCoordinate(pos));
        }

        private Map3DVertex[] GetVertices()
        {
            return new Map3DVertex[]
            {
                new Map3DVertex(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1)),
                new Map3DVertex(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0)),
                new Map3DVertex(new Vector3(-1, 1, 0), Color.White,  new Vector2(0, 0)),
                new Map3DVertex(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1)),
            };
        }

        public override void Update()
        {
            (byte level, byte area, ushort loadingPoint, ushort missionLayout) currentLocationStats =
                Config.MapAssociations.GetCurrentLocationStats();
            if (currentLocationStats.level != _currentLocationStats.level ||
                currentLocationStats.area != _currentLocationStats.area ||
                currentLocationStats.loadingPoint != _currentLocationStats.loadingPoint ||
                currentLocationStats.missionLayout != _currentLocationStats.missionLayout)
            {
                _currentLocationStats = currentLocationStats;
                if (_resetPathOnLevelChange)
                {
                    _dictionary.Clear();
                    _numSkips = 5;
                    _skippedKeys.Clear();
                }
            }

            if (!_isPaused)
            {
                uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
                float x = (float)_posAngle.X;
                float y = (float)_posAngle.Y;
                float z = (float)_posAngle.Z;

                if (globalTimer < _highestGlobalTimerValue)
                {
                    Dictionary<uint, (float x, float y, float z)> tempDictionary = new Dictionary<uint, (float x, float y, float z)>();
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

                if (!_useValueAtStartOfGlobalTimer || !_dictionary.ContainsKey(globalTimer))
                {
                    if (_numSkips > 0)
                    {
                        if (!_skippedKeys.Contains(globalTimer))
                        {
                            _skippedKeys.Add(globalTimer);
                            _numSkips--;
                        }
                    }
                    else
                    {
                        _dictionary[globalTimer] = (x, y, z);
                        _highestGlobalTimerValue = globalTimer;
                    }
                }
            }
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemResetPath = new ToolStripMenuItem("Reset Path");
                itemResetPath.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(doReset: true);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemResetPathOnLevelChange = new ToolStripMenuItem("Reset Path on Level Change");
                _itemResetPathOnLevelChange.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePathResetPathOnLevelChange: true,
                        newPathResetPathOnLevelChange: !_resetPathOnLevelChange);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemResetPathOnLevelChange.Checked = _resetPathOnLevelChange;

                _itemUseBlending = new ToolStripMenuItem("Use Blending");
                _itemUseBlending.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePathUseBlending: true,
                        newPathUseBlending: !_useBlending);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUseBlending.Checked = _useBlending;

                _itemPause = new ToolStripMenuItem("Pause");
                _itemPause.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePathPaused: true,
                        newPathPaused: !_isPaused);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemPause.Checked = _isPaused;

                _itemTruncatePoints = new ToolStripMenuItem("Truncate Points");
                _itemTruncatePoints.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePathTruncatePoints: true,
                        newPathTruncatePoints: !_truncatePoints);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemTruncatePoints.Checked = _isPaused;

                _itemUseValueAtStartOfGlobalTimer = new ToolStripMenuItem("Use Value at Start of Global Timer");
                _itemUseValueAtStartOfGlobalTimer.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changePathUseValueAtStartOfGlobalTimer: true,
                        newPathUseValueAtStartOfGlobalTimer: !_useValueAtStartOfGlobalTimer);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemUseValueAtStartOfGlobalTimer.Checked = _useValueAtStartOfGlobalTimer;

                string suffix1 = string.Format(" ({0})", _modulo);
                _itemSetModulo = new ToolStripMenuItem(SET_MODULO_TEXT + suffix1);
                _itemSetModulo.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter modulo.");
                    int? moduloNullable = ParsingUtilities.ParseIntNullable(text);
                    if (!moduloNullable.HasValue || moduloNullable.Value <= 0) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changePathModulo: true, newPathModulo: moduloNullable.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _itemDeduplicatePoints = new ToolStripMenuItem("Deduplicate Points");
                _itemDeduplicatePoints.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeDeduplicatePoints: true,
                        newDeduplicatePoints: !_deduplicatePoints);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemDeduplicatePoints.Checked = _deduplicatePoints;

                _itemShowQuarterSteps = new ToolStripMenuItem("Show Quarter Steps");
                _itemShowQuarterSteps.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(
                        changeShowQuarterSteps: true,
                        newShowQuarterSteps: !_showQuarterSteps);
                    GetParentMapTracker().ApplySettings(settings);
                };
                _itemShowQuarterSteps.Checked = _showQuarterSteps;

                string suffix2 = string.Format(" ({0})", _imageSize);
                _itemSetIconSize = new ToolStripMenuItem(SET_ICON_SIZE_TEXT + suffix2);
                _itemSetIconSize.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter icon size.");
                    float? sizeNullable = ParsingUtilities.ParseFloatNullable(text);
                    if (!sizeNullable.HasValue) return;
                    MapObjectSettings settings = new MapObjectSettings(
                        changeIconSize: true, newIconSize: sizeNullable.Value);
                    GetParentMapTracker().ApplySettings(settings);
                };

                ToolStripMenuItem itemCopyPoints = new ToolStripMenuItem("Copy Points");
                itemCopyPoints.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(doPathCopyPoints: true);
                    GetParentMapTracker().ApplySettings(settings);
                };

                ToolStripMenuItem itemPastePoints = new ToolStripMenuItem("Paste Points");
                itemPastePoints.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(doPathPastePoints: true);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemResetPath);
                _contextMenuStrip.Items.Add(_itemResetPathOnLevelChange);
                _contextMenuStrip.Items.Add(_itemUseBlending);
                _contextMenuStrip.Items.Add(_itemPause);
                _contextMenuStrip.Items.Add(_itemTruncatePoints);
                _contextMenuStrip.Items.Add(_itemUseValueAtStartOfGlobalTimer);
                _contextMenuStrip.Items.Add(_itemSetModulo);
                _contextMenuStrip.Items.Add(_itemDeduplicatePoints);
                _contextMenuStrip.Items.Add(_itemShowQuarterSteps);
                _contextMenuStrip.Items.Add(_itemSetIconSize);
                _contextMenuStrip.Items.Add(itemCopyPoints);
                _contextMenuStrip.Items.Add(itemPastePoints);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.DoReset)
            {
                _dictionary.Clear();
            }

            if (settings.ChangePathResetPathOnLevelChange)
            {
                _resetPathOnLevelChange = settings.NewPathResetPathOnLevelChange;
                _itemResetPathOnLevelChange.Checked = _resetPathOnLevelChange;
            }

            if (settings.ChangePathUseBlending)
            {
                _useBlending = settings.NewPathUseBlending;
                _itemUseBlending.Checked = _useBlending;
            }

            if (settings.ChangePathPaused)
            {
                _isPaused = settings.NewPathPaused;
                _itemPause.Checked = _isPaused;
            }

            if (settings.ChangePathTruncatePoints)
            {
                _truncatePoints = settings.NewPathTruncatePoints;
                _itemTruncatePoints.Checked = _truncatePoints;
            }

            if (settings.ChangePathUseValueAtStartOfGlobalTimer)
            {
                _useValueAtStartOfGlobalTimer = settings.NewPathUseValueAtStartOfGlobalTimer;
                _itemUseValueAtStartOfGlobalTimer.Checked = _useValueAtStartOfGlobalTimer;
            }

            if (settings.ChangePathModulo)
            {
                _modulo = settings.NewPathModulo;
                string suffix = string.Format(" ({0})", _modulo);
                _itemSetModulo.Text = SET_MODULO_TEXT + suffix;
            }

            if (settings.ChangeDeduplicatePoints)
            {
                _deduplicatePoints = settings.NewDeduplicatePoints;
                _itemDeduplicatePoints.Checked = _deduplicatePoints;
            }

            if (settings.ChangeShowQuarterSteps)
            {
                _showQuarterSteps = settings.NewShowQuarterSteps;
                _itemShowQuarterSteps.Checked = _showQuarterSteps;
            }

            if (settings.ChangeIconSize)
            {
                _imageSize = settings.NewIconSize;
                string suffix = string.Format(" ({0})", _imageSize);
                _itemSetIconSize.Text = SET_ICON_SIZE_TEXT + suffix;
            }

            if (settings.DoPathCopyPoints)
            {
                if (KeyboardUtilities.IsCtrlHeld()) // record q steps
                {
                    StringBuilder builder = new StringBuilder();
                    uint globalTimerCounter = 0;
                    List<uint> keys = _dictionary.Keys.ToList();
                    for (int i = 0; i < keys.Count - 1; i++)
                    {
                        uint key1 = keys[i];
                        uint key2 = keys[i + 1];
                        (float x1, float y1, float z1) = _dictionary[key1];
                        (float x2, float y2, float z2) = _dictionary[key2];
                        if (i == 0)
                        {
                            builder.Append(
                                string.Format(
                                    "{0}\t{1}\t{2}\t{3}\r\n",
                                    key1,
                                    (double)x1,
                                    (double)y1,
                                    (double)z1));
                            globalTimerCounter = key1;
                        }
                        for (int q = 1; q <= 4; q++)
                        {
                            float x = x1 + (q / 4f) * (x2 - x1);
                            float y = y1 + (q / 4f) * (y2 - y1);
                            float z = z1 + (q / 4f) * (z2 - z1);
                            globalTimerCounter++;
                            builder.Append(
                                string.Format(
                                    "{0}\t{1}\t{2}\t{3}\r\n",
                                    globalTimerCounter,
                                    (double)x,
                                    (double)y,
                                    (double)z));
                        }
                    }
                    Clipboard.SetText(builder.ToString());
                }
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var entry in _dictionary)
                    {
                        builder.Append(
                            string.Format(
                                "{0}\t{1}\t{2}\t{3}\r\n",
                                entry.Key,
                                (double)entry.Value.x,
                                (double)entry.Value.y,
                                (double)entry.Value.z));
                    }
                    Clipboard.SetText(builder.ToString());
                }
            }

            if (settings.DoPathPastePoints)
            {
                _dictionary.Clear();
                List<double?> values = ParsingUtilities.ParseDoubleList(Clipboard.GetText());
                for (int i = 0; i < values.Count - 3; i += 4)
                {
                    uint globalTimer = (uint)(values[i] ?? 0);
                    float x = (float)(values[i + 1] ?? 0);
                    float y = (float)(values[i + 2] ?? 0);
                    float z = (float)(values[i + 3] ?? 0);
                    _dictionary[globalTimer] = (x, y, z);
                }
            }
        }

        public override string GetName()
        {
            return "Path for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.PathImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (_customImage == null) return null;

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);

            List<(float x, float y, float z)> preData = GetDictionaryValues();
            List<(float x, float y, float z)> data = _showQuarterSteps ? MapUtilities.InterpolateQuarterSteps(preData) : preData;
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                double dist = MoreMath.GetDistanceBetween(dataPoint.x, dataPoint.z, inGameX, inGameZ);
                float imageSize = _showQuarterSteps && i % 4 != 0 ? _imageSize * QSTEP_RATIO : _imageSize;
                double radius = Scales ? imageSize : imageSize / Config.CurrentMapGraphics.MapViewScaleValue;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, dataPoint.x, dataPoint.y, dataPoint.z, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            if (_customImage == null) return null;

            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            List<(float x, float y, float z)> preData = GetDictionaryValues();
            List<(float x, float y, float z)> data = _showQuarterSteps ? MapUtilities.InterpolateQuarterSteps(preData) : preData;
            for (int i = data.Count - 1; i >= 0; i--)
            {
                var dataPoint = data[i];
                (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlOrthographicView(dataPoint.x, dataPoint.y, dataPoint.z, UseRelativeCoordinates);
                double dist = MoreMath.GetDistanceBetween(controlX, controlZ, relPos.X, relPos.Y);
                float imageSize = _showQuarterSteps && i % 4 != 0 ? _imageSize * QSTEP_RATIO : _imageSize;
                double radius = Scales ? imageSize * Config.CurrentMapGraphics.MapViewScaleValue : imageSize;
                if (dist <= radius || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, MapObjectHoverDataEnum.Icon, dataPoint.x, dataPoint.y, dataPoint.z, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            List<(float x, float y, float z)> preData = GetDictionaryValues();
            List<(float x, float y, float z)> data = _showQuarterSteps ? MapUtilities.InterpolateQuarterSteps(preData) : preData;
            var dataPoint = data[hoverData.Index.Value];
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(dataPoint.x, dataPoint.y, dataPoint.z, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = new List<string>();
            foreach (uint key in _dictionary.Keys)
            {
                (float x, float y, float z) = _dictionary[key];
                pointList.Add(string.Format("({0},{1},{2},{3})", key, (double)x, (double)y, (double)z));
            }
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
                new XAttribute("points", string.Join(",", pointList)),
            };
        }
    }
}
