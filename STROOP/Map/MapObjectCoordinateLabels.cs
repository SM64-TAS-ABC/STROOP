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
using STROOP.Map.Map3D;
using System.Windows.Forms;
using STROOP.Controls;
using STROOP.Forms;

namespace STROOP.Map
{
    public class MapObjectCoordinateLabels : MapObject
    {
        private Dictionary<(bool isX, double coord), int> _texes;
        private Color _previousOutlineColor;
        private float _previousSize;
        private double _previousBoldText;

        public MapObjectCoordinateLabels()
            : base()
        {
            Size = 100;
            LineColor = Color.Blue;
            InternalRotates = true;

            _texes = new Dictionary<(bool isX, double coord), int>();
            _previousOutlineColor = LineColor;
            _previousSize = Size;
            _previousBoldText = MapConfig.CoordinateLabelsBoldText;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            double spacing;
            if (MapConfig.CoordinateLabelsCustomSpacing == 0)
            {
                double totalMultiplies = MapConfig.CoordinateLabelsLabelDensity / Config.CurrentMapGraphics.MapViewScaleValue;
                double numMultiplies = (int)Math.Ceiling(Math.Log(totalMultiplies) / Math.Log(2));
                spacing = Math.Pow(2, numMultiplies);
            }
            else
            {
                spacing = MapConfig.CoordinateLabelsCustomSpacing;
            }

            int xMinMultiplier = (int)(Config.CurrentMapGraphics.MapViewXMin / spacing) - 1;
            int xMaxMultiplier = (int)(Config.CurrentMapGraphics.MapViewXMax / spacing) + 1;
            int zMinMultiplier = (int)(Config.CurrentMapGraphics.MapViewZMin / spacing) - 1;
            int zMaxMultiplier = (int)(Config.CurrentMapGraphics.MapViewZMax / spacing) + 1;

            List<(float x, float z, float angle, int tex)> labelData =
                new List<(float x, float z, float angle, int tex)>();

            (float x1, float z1) getSuperlativePoint(bool isX, bool useHigh, ((float x, float z) p1, (float x, float z) p2) points)
            {
                float value1 = isX ? points.p1.x : points.p1.z;
                float value2 = isX ? points.p2.x : points.p2.z;
                bool isP1Winner = useHigh ? value1 > value2 : value1 < value2;
                return isP1Winner ? points.p1 : points.p2;
            }

            if (MapConfig.CoordinateLabelsShowXLabels == 1)
            {
                for (double x = xMinMultiplier * spacing; x <= xMaxMultiplier * spacing; x += spacing)
                {
                    ((float x1, float z1), (float x2, float z2))? intersectionPoints = GetLineIntersectionWithBorder(true, (float)x, (float)MapConfig.CoordinateLabelsMargin);
                    if (!intersectionPoints.HasValue) continue;
                    (float g, float z) = getSuperlativePoint(false, MapConfig.CoordinateLabelsUseHighZ == 1, intersectionPoints.Value);
                    (float xControl, float zControl) = MapUtilities.ConvertCoordsForControlTopDownView((float)x, z);
                    float angle = -1 * Config.CurrentMapGraphics.MapViewYawValue + 16384;
                    if (MoreMath.GetAngleDistance(0, angle) > 16384) angle = (float)MoreMath.ReverseAngle(angle);
                    float angleDegrees = Rotates ? (float)MoreMath.AngleUnitsToDegrees(angle) : 0;
                    int tex = GetTex(true, x);
                    labelData.Add((xControl, zControl, angleDegrees, tex));
                }
            }

            if (MapConfig.CoordinateLabelsShowZLabels == 1)
            {
                for (double z = zMinMultiplier * spacing; z <= zMaxMultiplier * spacing; z += spacing)
                {
                    ((float x1, float z1), (float x2, float z2))? intersectionPoints = GetLineIntersectionWithBorder(false, (float)z, (float)MapConfig.CoordinateLabelsMargin);
                    if (!intersectionPoints.HasValue) continue;
                    (float x, float g) = getSuperlativePoint(true, MapConfig.CoordinateLabelsUseHighX == 1, intersectionPoints.Value);
                    (float xControl, float zControl) = MapUtilities.ConvertCoordsForControlTopDownView(x, (float)z);
                    float angle = -1 * Config.CurrentMapGraphics.MapViewYawValue + 32768;
                    if (MoreMath.GetAngleDistance(0, angle) > 16384) angle = (float)MoreMath.ReverseAngle(angle);
                    float angleDegrees = Rotates ? (float)MoreMath.AngleUnitsToDegrees(angle) : 0;
                    int tex = GetTex(false, z);
                    labelData.Add((xControl, zControl, angleDegrees, tex));
                }
            }

            foreach ((float x, float z, float angle, int tex) in labelData)
            {
                MapUtilities.DrawTexture(tex, new PointF(x, z), new SizeF(Size, Size), angle, Opacity);
            }

            if (MapConfig.CoordinateLabelsShowCursorPos == 1)
            {
                Point relPos = Config.MapGui.CurrentControl.PointToClient(Cursor.Position);
                (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);
                double roundedX = Math.Round(inGameX, 3);
                double roundedZ = Math.Round(inGameZ, 3);
                Bitmap texture = CreateTexture(roundedX + "\r\n" + roundedZ);
                int tex = MapUtilities.LoadTexture(texture);
                MapUtilities.DrawTexture(tex, new PointF(relPos.X + 15 + (int)Size / 2, relPos.Y), new SizeF(Size, Size), 0, Opacity);
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

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CoordinateLabelsImage;
        }

        public override string GetName()
        {
            return "Coordinate Labels";
        }

        public int GetTex(bool isX, double coord)
        {
            if (!_texes.ContainsKey((isX, coord)))
            {
                string prefix = isX ? "X=" : "Z=";
                string label = prefix + coord;
                Bitmap texture = CreateTexture(label);
                int tex = MapUtilities.LoadTexture(texture);
                _texes.Add((isX, coord), tex);
            }
            return _texes[(isX, coord)];
        }

        private Bitmap CreateTexture(string text)
        {
            int size = (int)Size;
            Bitmap bmp = new Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font drawFont = new Font("Arial", size / 6, MapConfig.CoordinateLabelsBoldText == 1 ? FontStyle.Bold : FontStyle.Regular);
            SolidBrush drawBrush = new SolidBrush(LineColor);
            SizeF stringSize = gfx.MeasureString(text, drawFont);
            gfx.DrawString(text, drawFont, drawBrush, new PointF(size / 2 - stringSize.Width / 2, size / 2 - stringSize.Height / 2));
            return bmp;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemOpenSettings = new ToolStripMenuItem("Open Settings");
                itemOpenSettings.Click += (sender, e) =>
                {
                    List<(string specialType, string varName, WatchVariableSubclass subclass)> varData =
                    new List<(string specialType, string varName, WatchVariableSubclass subclass)>()
                    {
                        ("CoordinateLabelsCustomSpacing", "Custom Spacing", WatchVariableSubclass.Number),
                        ("CoordinateLabelsMargin", "Margin", WatchVariableSubclass.Number),
                        ("CoordinateLabelsLabelDensity", "Label Density", WatchVariableSubclass.Number),

                        ("CoordinateLabelsShowCursorPos", "Show Cursor Pos", WatchVariableSubclass.Boolean),
                        ("CoordinateLabelsShowXLabels", "Show X Labels", WatchVariableSubclass.Boolean),
                        ("CoordinateLabelsShowZLabels", "Show Z Labels", WatchVariableSubclass.Boolean),
                        ("CoordinateLabelsUseHighX", "Use High X", WatchVariableSubclass.Boolean),
                        ("CoordinateLabelsUseHighZ", "Use High Z", WatchVariableSubclass.Boolean),
                        ("CoordinateLabelsBoldText", "Bold Text", WatchVariableSubclass.Boolean),
                    };

                    List<WatchVariableControl> controls = new List<WatchVariableControl>();
                    foreach ((string specialType, string varName, WatchVariableSubclass subclass) in varData)
                    {
                        WatchVariable watchVar = new WatchVariable(
                            name: varName,
                            memoryTypeName: null,
                            specialType: specialType,
                            baseAddressType: BaseAddressTypeEnum.None,
                            offsetUS: null,
                            offsetJP: null,
                            offsetSH: null,
                            offsetEU: null,
                            offsetDefault: null,
                            mask: null,
                            shift: null,
                            handleMapping: true);
                        WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                            name: varName,
                            watchVar: watchVar,
                            subclass: subclass,
                            backgroundColor: null,
                            displayType: null,
                            roundingLimit: null,
                            useHex: null,
                            invertBool: null,
                            isYaw: null,
                            coordinate: null,
                            groupList: new List<VariableGroup>() { VariableGroup.Custom });
                        WatchVariableControl control = precursor.CreateWatchVariableControl();
                        controls.Add(control);
                    }

                    VariablePopOutForm form = new VariablePopOutForm();
                    form.Initialize(controls);
                    form.ShowForm();
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemOpenSettings);
            }

            return _contextMenuStrip;
        }

        public ((float x1, float z1), (float x2, float z2))? GetLineIntersectionWithBorder(bool isX, float coord, float margin)
        {
            (float topLeftX, float topLeftZ) = MapUtilities.ConvertCoordsForInGameTopDownView(margin, margin);
            (float topRightX, float topRightZ) = MapUtilities.ConvertCoordsForInGameTopDownView(Config.MapGui.CurrentControl.Width - margin, margin);
            (float bottomRightX, float bottomRightZ) = MapUtilities.ConvertCoordsForInGameTopDownView(Config.MapGui.CurrentControl.Width - margin, Config.MapGui.CurrentControl.Height - margin);
            (float bottomLeftX, float bottomLeftZ) = MapUtilities.ConvertCoordsForInGameTopDownView(margin, Config.MapGui.CurrentControl.Height - margin);

            List<(float x, float z)> corners = new List<(float x, float z)>()
            {
                (topLeftX, topLeftZ),
                (topRightX, topRightZ),
                (bottomRightX, bottomRightZ),
                (bottomLeftX, bottomLeftZ),
            };

            List<((float x1, float z1), (float x2, float z2))> cornerSegments =
                new List<((float x1, float z1), (float x2, float z2))>();
            for (int i = 0; i < corners.Count; i++)
            {
                cornerSegments.Add((corners[i], corners[(i + 1) % corners.Count]));
            }

            List<(float x, float z)> intersectionPoints = new List<(float x, float z)>();
            foreach (((float x1, float z1), (float x2, float z2)) in cornerSegments)
            {
                if (isX)
                {
                    if (x1 == x2 || coord < Math.Min(x1, x2) || coord > Math.Max(x1, x2)) continue;
                    float p = (coord - x1) / (x2 - x1);
                    float z = z1 + p * (z2 - z1);
                    intersectionPoints.Add((coord, z));
                }
                else
                {
                    if (z1 == z2 || coord < Math.Min(z1, z2) || coord > Math.Max(z1, z2)) continue;
                    float p = (coord - z1) / (z2 - z1);
                    float x = x1 + p * (x2 - x1);
                    intersectionPoints.Add((x, coord));
                }
            }
            if (intersectionPoints.Count < 2) return null;

            double biggestDist = double.MinValue;
            (float p1X, float p1Z) = (float.NaN, float.NaN);
            (float p2X, float p2Z) = (float.NaN, float.NaN);
            for (int i = 0; i < intersectionPoints.Count; i++)
            {
                for (int j = i + 1; j < intersectionPoints.Count; j++)
                {
                    (float q1X, float q1Z) = intersectionPoints[i];
                    (float q2X, float q2Z) = intersectionPoints[j];
                    double dist = MoreMath.GetDistanceBetween(q1X, q1Z, q2X, q2Z);
                    if (dist > biggestDist)
                    {
                        biggestDist = dist;
                        (p1X, p1Z) = (q1X, q1Z);
                        (p2X, p2Z) = (q2X, q2Z);
                    }
                }
            }
            return ((p1X, p1Z), (p2X, p2Z));
        }

        public override void Update()
        {
            if (LineColor != _previousOutlineColor)
            {
                _previousOutlineColor = LineColor;
                _texes.Clear();
            }

            if (Size != _previousSize)
            {
                _previousSize = Size;
                _texes.Clear();
            }

            if (MapConfig.CoordinateLabelsBoldText != _previousBoldText)
            {
                _previousBoldText = MapConfig.CoordinateLabelsBoldText;
                _texes.Clear();
            }
        }
    }
}
