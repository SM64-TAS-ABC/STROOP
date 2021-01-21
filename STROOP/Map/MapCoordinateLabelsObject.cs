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
        private Dictionary<(bool isX, int coord), int> _texes;
        private static readonly int BUFFER = 40;

        public MapCoordinateLabelsObject()
            : base()
        {
            _texes = new Dictionary<(bool isX, int coord), int>();
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

            for (int x = xMin; x <= xMax; x++)
            {
                ((float x1, float z1), (float x2, float z2))? intersectionPoints = GetLineIntersectionWithBorder(true, x, BUFFER);
                if (!intersectionPoints.HasValue) continue;
                (float g, float z) = intersectionPoints.Value.Item1;
                (float xControl, float zControl) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                int tex = GetTex(true, x);
                float angle = 0;
                labelData.Add((xControl, zControl, angle, tex));
            }

            for (int z = zMin; z <= zMax; z++)
            {
                ((float x1, float z1), (float x2, float z2))? intersectionPoints = GetLineIntersectionWithBorder(false, z, BUFFER);
                if (!intersectionPoints.HasValue) continue;
                (float x, float g) = intersectionPoints.Value.Item1;
                (float xControl, float zControl) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                int tex = GetTex(false, z);
                float angle = 0;
                labelData.Add((xControl, zControl, angle, tex));
            }

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            foreach ((float x, float z, float angle, int tex) in labelData)
            {
                int width = 100;
                int height = 100;

                // Place and rotate texture to correct location on control
                GL.LoadIdentity();
                GL.Translate(new Vector3(x, z, 0));
                GL.Color4(1.0, 1.0, 1.0, 1.0);

                // Start drawing texture
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.Begin(PrimitiveType.Quads);

                // Set drawing coordinates
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-width / 2, height / 2);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(width / 2, height / 2);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(width / 2, -height / 2);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-width / 2, -height / 2);

                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
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
            Bitmap bmp = new Bitmap(100, 100, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            SizeF stringSize = gfx.MeasureString(text, drawFont);
            gfx.DrawString(text, drawFont, drawBrush, new PointF(50 - stringSize.Width / 2, 50 - stringSize.Height / 2));
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
    }
}
