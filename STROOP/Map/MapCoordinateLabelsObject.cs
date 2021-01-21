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
    public class MapCoordinateLabelsObject : MapObject
    {
        private static int BUFFER = 40;
        private static bool SHOW_X_LABELS = true;
        private static bool SHOW_Z_LABELS = true;
        private static bool USE_HIGH_X = false;
        private static bool USE_HIGH_Z = false;
        private static bool SHOW_CURSOR_POS = true;
        private static bool BOLD = true;

        private Dictionary<(bool isX, int coord), int> _texes;
        private Color _previousOutlineColor;
        private float _previousSize;
        private bool _previousBold;

        public MapCoordinateLabelsObject()
            : base()
        {
            Size = 100;
            OutlineColor = Color.Blue;
            InternalRotates = true;

            _texes = new Dictionary<(bool isX, int coord), int>();
            _previousOutlineColor = OutlineColor;
            _previousSize = Size;
            _previousBold = BOLD;
        }

        public override void DrawOn2DControlTopDownView()
        {
            if (!MapUtilities.IsAbleToShowUnitPrecision()) return;

            int xMin = (int)Config.MapGraphics.MapViewXMin - 1;
            int xMax = (int)Config.MapGraphics.MapViewXMax + 1;
            int zMin = (int)Config.MapGraphics.MapViewZMin - 1;
            int zMax = (int)Config.MapGraphics.MapViewZMax + 1;

            List<(float x, float z, float angle, int tex) > labelData =
                new List<(float x, float z, float angle, int tex)>();

            (float x1, float z1) getSuperlativePoint(bool isX, bool useHigh, ((float x, float z) p1, (float x, float z) p2) points)
            {
                float value1 = isX ? points.p1.x : points.p1.z;
                float value2 = isX ? points.p2.x : points.p2.z;
                bool isP1Winner = useHigh ? value1 > value2 : value1 < value2;
                return isP1Winner ? points.p1 : points.p2;
            }

            if (SHOW_X_LABELS)
            {
                for (int x = xMin; x <= xMax; x++)
                {
                    ((float x1, float z1), (float x2, float z2))? intersectionPoints = GetLineIntersectionWithBorder(true, x, BUFFER);
                    if (!intersectionPoints.HasValue) continue;
                    (float g, float z) = getSuperlativePoint(false, USE_HIGH_Z, intersectionPoints.Value);
                    (float xControl, float zControl) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                    float angle = -1 * Config.MapGraphics.MapViewYawValue + 16384;
                    if (MoreMath.GetAngleDistance(0, angle) > 16384) angle = (float)MoreMath.ReverseAngle(angle);
                    float angleDegrees = Rotates ? (float)MoreMath.AngleUnitsToDegrees(angle) : 0;
                    int tex = GetTex(true, x);
                    labelData.Add((xControl, zControl, angleDegrees, tex));
                }
            }

            if (SHOW_Z_LABELS)
            {
                for (int z = zMin; z <= zMax; z++)
                {
                    ((float x1, float z1), (float x2, float z2))? intersectionPoints = GetLineIntersectionWithBorder(false, z, BUFFER);
                    if (!intersectionPoints.HasValue) continue;
                    (float x, float g) = getSuperlativePoint(true, USE_HIGH_X, intersectionPoints.Value);
                    (float xControl, float zControl) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                    float angle = -1 * Config.MapGraphics.MapViewYawValue + 32768;
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

            if (SHOW_CURSOR_POS)
            {
                Point relPos = Config.MapGui.GLControlMap2D.PointToClient(Cursor.Position);
                (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);
                double stringX = Math.Round(inGameX, 3);
                double stringZ = Math.Round(inGameZ, 3);
                Bitmap texture = CreateTexture(stringX + "\r\n" + stringZ);
                int tex = MapUtilities.LoadTexture(texture);
                MapUtilities.DrawTexture(tex, new PointF(relPos.X + 15 + (int)Size / 2, relPos.Y), new SizeF(Size, Size), 0, Opacity);
            }
        }

        public override void DrawOn2DControlOrthographicView()
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
            return Config.ObjectAssociations.ArrowImage;
        }

        public override string GetName()
        {
            return "Coordinate Labels";
        }

        public int GetTex(bool isX, int coord)
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
            Font drawFont = new Font("Arial", size / 6, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(OutlineColor);
            SizeF stringSize = gfx.MeasureString(text, drawFont);
            gfx.DrawString(text, drawFont, drawBrush, new PointF(size / 2 - stringSize.Width / 2, size / 2 - stringSize.Height / 2));
            return bmp;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _contextMenuStrip = new ContextMenuStrip();
            }

            return _contextMenuStrip;
        }

        public ((float x1, float z1), (float x2, float z2))? GetLineIntersectionWithBorder(bool isX, int coord, int margin)
        {
            (float topLeftX, float topLeftZ) = MapUtilities.ConvertCoordsForInGame(margin, margin);
            (float topRightX, float topRightZ) = MapUtilities.ConvertCoordsForInGame(Config.MapGui.GLControlMap2D.Width - margin, margin);
            (float bottomRightX, float bottomRightZ) = MapUtilities.ConvertCoordsForInGame(Config.MapGui.GLControlMap2D.Width - margin, Config.MapGui.GLControlMap2D.Height - margin);
            (float bottomLeftX, float bottomLeftZ) = MapUtilities.ConvertCoordsForInGame(margin, Config.MapGui.GLControlMap2D.Height - margin);

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
            if (OutlineColor != _previousOutlineColor)
            {
                _previousOutlineColor = OutlineColor;
                _texes.Clear();
            }

            if (Size != _previousSize)
            {
                _previousSize = Size;
                _texes.Clear();
            }

            if (BOLD != _previousBold)
            {
                _previousBold = BOLD;
                _texes.Clear();
            }
        }
    }
}
