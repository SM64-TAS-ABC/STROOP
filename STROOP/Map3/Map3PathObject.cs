using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;
using System.Windows.Forms;

namespace STROOP.Map3
{
    public class Map3PathObject : Map3Object
    {
        private readonly PositionAngle _posAngle;
        private readonly Dictionary<uint, (float x, float z)> _dictionary;
        private (byte level, byte area, ushort loadingPoint, ushort missionLayout) _currentLocationStats;
        private bool _resetPathOnLevelChange;
        private int _numSkips;
        private List<uint> _skippedKeys;
        private bool _isPaused;

        public Map3PathObject(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _dictionary = new Dictionary<uint, (float x, float z)>();
            _currentLocationStats = Config.MapAssociations.GetCurrentLocationStats();
            _resetPathOnLevelChange = false;
            _numSkips = 0;
            _skippedKeys = new List<uint>();
            _isPaused = false;

            Size = 50;
            OutlineWidth = 3;
            OutlineColor = Color.Red;
        }

        public override void DrawOnControl()
        {
            if (OutlineWidth == 0) return;

            List<(float x, float z)> vertices = _dictionary.Values.ToList();
            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => Map3Utilities.ConvertCoordsForControl(vertex.x, vertex.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LineWidth(OutlineWidth);
            for (int i = 0; i < veriticesForControl.Count - 1; i++)
            {
                (float x1, float z1) = veriticesForControl[i];
                (float x2, float z2) = veriticesForControl[i + 1];
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, OpacityByte);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x1, z1);
                GL.Vertex2(x2, z2);
                GL.End();
            }
            GL.Color4(1, 1, 1, 1.0f);
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
                float z = (float)_posAngle.Z;
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
                        _dictionary[globalTimer] = (x, z);
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

                ToolStripMenuItem itemPause = new ToolStripMenuItem("Pause");
                itemPause.Click += (sender, e) =>
                {
                    _isPaused = !_isPaused;
                    itemPause.Checked = _isPaused;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemResetPath);
                _contextMenuStrip.Items.Add(itemResetPathOnLevelChange);
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
    }
}
