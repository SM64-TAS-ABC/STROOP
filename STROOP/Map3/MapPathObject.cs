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
    public class MapPathObject : MapObject
    {
        private readonly PositionAngle _posAngle;
        private readonly Dictionary<uint, (float x, float y, float z)> _dictionary;
        private (byte level, byte area, ushort loadingPoint, ushort missionLayout) _currentLocationStats;
        private bool _resetPathOnLevelChange;
        private int _numSkips;
        private List<uint> _skippedKeys;
        private bool _useBlending;
        private bool _isPaused;
        private uint _highestGlobalTimerValue;

        public MapPathObject(PositionAngle posAngle)
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
            _highestGlobalTimerValue = 0;

            Size = 300;
            OutlineWidth = 3;
            Color = Color.Yellow;
            OutlineColor = Color.Red;
        }

        public override void DrawOn2DControl()
        {
            if (OutlineWidth == 0) return;

            List<(float x, float y, float z)> vertices = _dictionary.Values.ToList();
            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => MapUtilities.ConvertCoordsForControl(vertex.x, vertex.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LineWidth(OutlineWidth);
            for (int i = 0; i < veriticesForControl.Count - 1; i++)
            {
                Color color = OutlineColor;
                if (_useBlending)
                {
                    int distFromEnd = veriticesForControl.Count - i - 2;
                    if (distFromEnd < Size)
                    {
                        color = ColorUtilities.InterpolateColor(
                            OutlineColor, Color, distFromEnd / (double)Size);
                    }
                    else
                    {
                        color = Color;
                    }
                }
                (float x1, float z1) = veriticesForControl[i];
                (float x2, float z2) = veriticesForControl[i + 1];
                GL.Color4(color.R, color.G, color.B, OpacityByte);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x1, z1);
                GL.Vertex2(x2, z2);
                GL.End();
            }
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            if (OutlineWidth == 0) return;

            List<(float x, float y, float z)> vertices = _dictionary.Values.ToList();
            List<Map4Vertex[]> vertexArrayList = new List<Map4Vertex[]>();
            for (int i = 0; i < vertices.Count - 1; i++)
            {
                Color color = OutlineColor;
                if (_useBlending)
                {
                    int distFromEnd = vertices.Count - i - 2;
                    if (distFromEnd < Size)
                    {
                        color = ColorUtilities.InterpolateColor(
                            OutlineColor, Color, distFromEnd / (double)Size);
                    }
                    else
                    {
                        color = Color;
                    }
                }
                (float x1, float y1, float z1) = vertices[i];
                (float x2, float y2, float z2) = vertices[i + 1];

                vertexArrayList.Add(new Map4Vertex[]
                {
                    new Map4Vertex(new Vector3(x1, y1, z1), color),
                    new Map4Vertex(new Vector3(x2, y2, z2), color),
                });
            }

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map4Camera.Matrix;
            GL.UniformMatrix4(Config.Map4Graphics.GLUniformView, false, ref viewMatrix);

            vertexArrayList.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                GL.LineWidth(OutlineWidth);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Lines, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });
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
                uint globalTimer = Config.Stream.GetUInt32(MiscConfig.GlobalTimerAddress);
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

                if (!_dictionary.ContainsKey(globalTimer))
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
                itemResetPath.Click += (sender, e) => _dictionary.Clear();

                ToolStripMenuItem itemResetPathOnLevelChange = new ToolStripMenuItem("Reset Path on Level Change");
                itemResetPathOnLevelChange.Click += (sender, e) =>
                {
                    _resetPathOnLevelChange = !_resetPathOnLevelChange;
                    itemResetPathOnLevelChange.Checked = _resetPathOnLevelChange;
                };
                itemResetPathOnLevelChange.Checked = _resetPathOnLevelChange;

                ToolStripMenuItem itemUseBlending = new ToolStripMenuItem("Use Blending");
                itemUseBlending.Click += (sender, e) =>
                {
                    _useBlending = !_useBlending;
                    itemUseBlending.Checked = _useBlending;
                };
                itemUseBlending.Checked = _useBlending;

                ToolStripMenuItem itemPause = new ToolStripMenuItem("Pause");
                itemPause.Click += (sender, e) =>
                {
                    _isPaused = !_isPaused;
                    itemPause.Checked = _isPaused;
                };
                itemPause.Checked = _isPaused;

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemResetPath);
                _contextMenuStrip.Items.Add(itemResetPathOnLevelChange);
                _contextMenuStrip.Items.Add(itemUseBlending);
                _contextMenuStrip.Items.Add(itemPause);
            }

            return _contextMenuStrip;
        }

        public override string GetName()
        {
            return "Path for " + _posAngle.GetMapName();
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.PathImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
