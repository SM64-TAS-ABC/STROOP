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
    public class MapObjectBranchPath : MapObject
    {
        private readonly PositionAngle _posAngle;
        private readonly List<(uint globalTimer, float x, float y, float z)> _list;
        private bool _useBlending;
        private bool _isPaused;

        private ToolStripMenuItem _itemUseBlending;
        private ToolStripMenuItem _itemPause;

        public MapObjectBranchPath(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _list = new List<(uint globalTimer, float x, float y, float z)>();
            _useBlending = true;
            _isPaused = false;

            Size = 30;
            LineWidth = 3;
            Color = Color.Yellow;
            LineColor = Color.Red;
        }

        public MapObjectBranchPath(PositionAngle posAngle, List<(uint globalTimer, float x, float y, float z)> points) : this(posAngle)
        {
            foreach (var p in points)
            {
                _list.Add((p.globalTimer, p.x, p.y, p.z));
            }
        }

        public static MapObjectBranchPath Create(PositionAngle posAngle, string pointString)
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
            return new MapObjectBranchPath(posAngle, points);
        }

        private List<MapBranchPathObjectSegment> GetSegments()
        {
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
            List<MapBranchPathObjectSegment> segments = new List<MapBranchPathObjectSegment>();
            for (int i = 0; i < _list.Count - 1; i++)
            {
                (uint globalTimer1, float x1, float y1, float z1) = _list[i];
                (uint globalTimer2, float x2, float y2, float z2) = _list[i + 1];
                if (globalTimer1 + 1 != globalTimer2) continue;
                if (globalTimer2 > globalTimer) continue;
                MapBranchPathObjectSegment segment = new MapBranchPathObjectSegment(globalTimer2, x1, y1, z1, x2, y2, z2);
                segments.Add(segment);
            }
            return segments;
        }

        public override void DrawOn2DControlTopDownView()
        {
            if (LineWidth == 0) return;

            List<MapBranchPathObjectSegment> segments = GetSegments();
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LineWidth(LineWidth);
            foreach (MapBranchPathObjectSegment segment in segments)
            {
                Color color = LineColor;
                if (_useBlending)
                {
                    int time1 = (int)(globalTimer - Size);
                    int time2 = (int)globalTimer;
                    if (segment.GlobalTimer >= time2)
                    {
                        color = LineColor;
                    }
                    else if (segment.GlobalTimer <= time1)
                    {
                        color = Color;
                    }
                    else
                    {
                        int distFromEnd = (int)(time2 - segment.GlobalTimer);
                        color = ColorUtilities.InterpolateColor(
                            LineColor, Color, distFromEnd / (double)Size);
                    }
                }
                (float x1, float z1) = MapUtilities.ConvertCoordsForControlTopDownView(segment.StartX, segment.StartZ);
                (float x2, float z2) = MapUtilities.ConvertCoordsForControlTopDownView(segment.EndX, segment.EndZ);
                GL.Color4(color.R, color.G, color.B, OpacityByte);
                GL.Begin(PrimitiveType.Lines);
                GL.Vertex2(x1, z1);
                GL.Vertex2(x2, z2);
                GL.End();
            }
            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlOrthographicView()
        {
            if (LineWidth == 0) return;

            List<MapBranchPathObjectSegment> segments = GetSegments();
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.LineWidth(LineWidth);
            foreach (MapBranchPathObjectSegment segment in segments)
            {
                Color color = LineColor;
                if (_useBlending)
                {
                    int time1 = (int)(globalTimer - Size);
                    int time2 = (int)globalTimer;
                    if (segment.GlobalTimer >= time2)
                    {
                        color = LineColor;
                    }
                    else if (segment.GlobalTimer <= time1)
                    {
                        color = Color;
                    }
                    else
                    {
                        int distFromEnd = (int)(time2 - segment.GlobalTimer);
                        color = ColorUtilities.InterpolateColor(
                            LineColor, Color, distFromEnd / (double)Size);
                    }
                }
                (float x1, float z1) = MapUtilities.ConvertCoordsForControlOrthographicView(segment.StartX, segment.StartY, segment.StartZ);
                (float x2, float z2) = MapUtilities.ConvertCoordsForControlOrthographicView(segment.EndX, segment.EndY, segment.EndZ);
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
            if (LineWidth == 0) return;

            List<MapBranchPathObjectSegment> segments = GetSegments();
            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);

            List<Map3DVertex[]> vertexArrayList = new List<Map3DVertex[]>();
            foreach (MapBranchPathObjectSegment segment in segments)
            {
                Color color = LineColor;
                if (_useBlending)
                {
                    int time1 = (int)(globalTimer - Size);
                    int time2 = (int)globalTimer;
                    if (segment.GlobalTimer >= time2)
                    {
                        color = LineColor;
                    }
                    else if (segment.GlobalTimer <= time1)
                    {
                        color = Color;
                    }
                    else
                    {
                        int distFromEnd = (int)(time2 - segment.GlobalTimer);
                        color = ColorUtilities.InterpolateColor(
                            LineColor, Color, distFromEnd / (double)Size);
                    }
                }

                vertexArrayList.Add(new Map3DVertex[]
                {
                    new Map3DVertex(new Vector3(segment.StartX, segment.StartY, segment.StartZ), color),
                    new Map3DVertex(new Vector3(segment.EndX, segment.EndY, segment.EndZ), color),
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

        public override void Update()
        {
            if (_isPaused) return;

            uint globalTimer = Config.Stream.GetUInt(MiscConfig.GlobalTimerAddress);
            float x = (float)_posAngle.X;
            float y = (float)_posAngle.Y;
            float z = (float)_posAngle.Z;

            // replace the last item in list
            if (_list.Count > 0 && _list[_list.Count - 1].globalTimer == globalTimer)
            {
                //_list[_list.Count - 1] = (globalTimer, x, y, z);
            }
            // add new item to list
            else
            {
                _list.Add((globalTimer, x, y, z));
            }
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemResetPath = new ToolStripMenuItem("Reset Path");
                itemResetPath.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(doPathReset: true);
                    GetParentMapTracker().ApplySettings(settings);
                };

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

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemResetPath);
                _contextMenuStrip.Items.Add(_itemUseBlending);
                _contextMenuStrip.Items.Add(_itemPause);
            }

            return _contextMenuStrip;
        }

        public override void ApplySettings(MapObjectSettings settings)
        {
            base.ApplySettings(settings);

            if (settings.DoPathReset)
            {
                _list.Clear();
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
        }

        public override string GetName()
        {
            return "Branch Path for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.BranchPathImage;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        private class MapBranchPathObjectSegment
        {
            public readonly uint GlobalTimer;
            public readonly float StartX;
            public readonly float StartY;
            public readonly float StartZ;
            public readonly float EndX;
            public readonly float EndY;
            public readonly float EndZ;

            public MapBranchPathObjectSegment(
                uint globalTimer,
                float startX,
                float startY,
                float startZ,
                float endX,
                float endY,
                float endZ)
            {
                GlobalTimer = globalTimer;
                StartX = startX;
                StartY = startY;
                StartZ = startZ;
                EndX = endX;
                EndY = endY;
                EndZ = endZ;
            }
        }

        public override List<XAttribute> GetXAttributes()
        {
            List<string> pointList = _list.ConvertAll(
                p => string.Format("({0},{1},{2},{3})", p.globalTimer, (double)p.x, (double)p.y, (double)p.z));
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
                new XAttribute("points", string.Join(",", pointList)),
            };
        }
    }
}
