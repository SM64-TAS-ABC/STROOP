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
    public class MapObjectPendulumGraph : MapObject
    {
        private readonly PositionAngle _posAngle;
        private Dictionary<int, (float angle, float velocity)> _dictionary;

        public MapObjectPendulumGraph(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
            _dictionary = new Dictionary<int, (float angle, float velocity)>();

            Size = 100;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            if (_dictionary.Count > 1)
            {
                List<(int timer, float angle)> angleList = new List<(int timer, float angle)>();
                List<(int timer, float angle)> zeroVelocityAngleList = new List<(int timer, float angle)>();
                int maxTimer = -1;
                foreach (int key in _dictionary.Keys)
                {
                    var value = _dictionary[key];
                    angleList.Add((key, value.angle));
                    if (value.velocity == 0) zeroVelocityAngleList.Add((key, value.angle));
                    maxTimer = Math.Max(maxTimer, key);
                }

                List<(float x, float y, float z)> pointList = new List<(float x, float y, float z)>();
                for (int i = 0; i < angleList.Count - 1; i++)
                {
                    float x1 = angleList[i].angle;
                    float y1 = 0;
                    float z1 = Size * (angleList[i].timer - maxTimer);
                    pointList.Add((x1, y1, z1));

                    float x2 = angleList[i + 1].angle;
                    float y2 = 0;
                    float z2 = Size * (angleList[i + 1].timer - maxTimer);
                    pointList.Add((x2, y2, z2));
                }
                MapUtilities.DrawLinesOn2DControlTopDownView(pointList, 3, Color.Red, 255);

                List<(float x, float y, float z)> zeroVelocityPointList = new List<(float x, float y, float z)>();
                foreach (var entry in zeroVelocityAngleList)
                {
                    float x1 = entry.angle;
                    float y1 = 0;
                    float z1 = Size * (entry.timer - maxTimer);
                    zeroVelocityPointList.Add((x1, y1, z1));

                    float x2 = entry.angle;
                    float y2 = 0;
                    float z2 = 0;
                    zeroVelocityPointList.Add((x2, y2, z2));
                }
                MapUtilities.DrawLinesOn2DControlTopDownView(zeroVelocityPointList, 1, Color.Blue, 255);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        public override void Update()
        {
            uint objAddress = _posAngle.GetObjAddress();
            int timer = Config.Stream.GetInt(objAddress + ObjectConfig.TimerOffset);
            float angle = Config.Stream.GetFloat(objAddress + ObjectConfig.PendulumAngleOffset);
            float velocity = Config.Stream.GetFloat(objAddress + ObjectConfig.PendulumAngularVelocityOffset);
            _dictionary[timer] = (angle, velocity);
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Overlay;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.UnitGridlinesImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemResetData = new ToolStripMenuItem("Reset Data");
                itemResetData.Click += (sender, e) =>
                {
                    MapObjectSettings settings = new MapObjectSettings(doReset: true);
                    GetParentMapTracker().ApplySettings(settings);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemResetData);
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
        }

        public override string GetName()
        {
            return "Pendulum Graph";
        }
    }
}
