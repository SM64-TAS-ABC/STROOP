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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectRenderTable : MapObject
    {
        private static float SQUARE_DIAMETER = 150;
        private static int BIG_TEXT_SIZE = 20;
        private static int SMALL_TEXT_SIZE = 15;
        private static int SMALL_TEXT_OFFSET = -20;

        private static Dictionary<(int x, int y), (string label, int size, int yOffset)> labelDictionary =
            new Dictionary<(int x, int y), (string label, int size, int yOffset)>()
            {
                [(2, 1)] = ("Active", BIG_TEXT_SIZE, 0),
                [(3, 1)] = ("Inactive", BIG_TEXT_SIZE, 0),
                [(1, 2)] = ("Visible", BIG_TEXT_SIZE, 0),
                [(1, 3)] = ("Invisible", BIG_TEXT_SIZE, 0),
                [(2, 2)] = ("Rendered", SMALL_TEXT_SIZE, SMALL_TEXT_OFFSET),
                [(2, 3)] = ("Not Rendered", SMALL_TEXT_SIZE, SMALL_TEXT_OFFSET),
                [(3, 2)] = ("Not Rendered", SMALL_TEXT_SIZE, SMALL_TEXT_OFFSET),
                [(3, 3)] = ("Not Rendered", SMALL_TEXT_SIZE, SMALL_TEXT_OFFSET),
            };

        private PositionAngle _posAngle;

        private Dictionary<(int x, int y), (int tex, float x, float y)> textDictionary =
            new Dictionary<(int x, int y), (int tex, float x, float y)>();

        public MapObjectRenderTable(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            List<List<(float x, float y)>> midpoints = GetMidpoints();
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    var key = (x, y);
                    if (!labelDictionary.ContainsKey(key)) continue;
                    var value = labelDictionary[key];
                    var midpoint = midpoints[x][y];
                    int tex = MapUtilities.LoadTexture(CreateTexture(value.label, value.size));
                    textDictionary[key] = (tex, midpoint.x, midpoint.y + value.yOffset);
                }
            }
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<List<(float x, float y, Color color)>> squares = GetSquares();

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw square
            foreach (var square in squares)
            {
                GL.Color3(square[0].color);
                GL.Begin(PrimitiveType.Polygon);
                foreach (var point in square)
                {
                    GL.Vertex2(point.x, point.y);
                }
                GL.End();
            }

            // Draw outline
            foreach (var square in squares)
            {
                GL.Color3(Color.Black);
                GL.LineWidth(LineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var point in square)
                {
                    GL.Vertex2(point.x, point.y);
                }
                GL.End();
            }

            foreach (var text in textDictionary.Values)
            {
                int tex = text.tex;
                (float x, float y) textPosition = (text.x, text.y);
                PointF loc = new PointF(textPosition.x, textPosition.y);
                SizeF size = new SizeF(SQUARE_DIAMETER, SQUARE_DIAMETER);

                // Place and rotate texture to correct location on control
                GL.LoadIdentity();
                GL.Translate(new Vector3(loc.X, loc.Y, 0));
                GL.Color4(1.0, 1.0, 1.0, 1.0);

                // Start drawing texture
                GL.BindTexture(TextureTarget.Texture2D, tex);
                GL.Begin(PrimitiveType.Quads);

                // Set drawing coordinates
                GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-size.Width / 2, size.Height / 2);
                GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(size.Width / 2, size.Height / 2);
                GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(size.Width / 2, -size.Height / 2);
                GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-size.Width / 2, -size.Height / 2);

                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        private Bitmap CreateTexture(string text, int size)
        {
            int imageSize = (int)SQUARE_DIAMETER;
            Bitmap bmp = new Bitmap(imageSize, imageSize, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(bmp);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            Font drawFont = new Font("Calibri", size, FontStyle.Bold);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            SizeF stringSize = gfx.MeasureString(text, drawFont);
            gfx.DrawString(text, drawFont, drawBrush, new PointF(imageSize / 2 - stringSize.Width / 2, imageSize / 2 - stringSize.Height / 2));
            return bmp;
        }

        private List<List<(float x, float y, Color color)>> GetSquares()
        {
            List<List<(float x, float y)>> midpoints = GetMidpoints();

            List<List<(float x, float y, Color color)>> squares =
                new List<List<(float x, float y, Color color)>>();
            for (int x = 1; x < 4; x++)
            {
                for (int y = 1; y < 4; y++)
                {
                    if (x == 1 && y == 1) continue;

                    Color color = Color.White;
                    var midpoint = midpoints[x][y];
                    List<(float x, float y, Color color)> square =
                        new List<(float x, float y, Color color)>()
                        {
                            (midpoint.x - SQUARE_DIAMETER / 2, midpoint.y - SQUARE_DIAMETER / 2, color),
                            (midpoint.x - SQUARE_DIAMETER / 2, midpoint.y + SQUARE_DIAMETER / 2, color),
                            (midpoint.x + SQUARE_DIAMETER / 2, midpoint.y + SQUARE_DIAMETER / 2, color),
                            (midpoint.x + SQUARE_DIAMETER / 2, midpoint.y - SQUARE_DIAMETER / 2, color),
                        };
                    squares.Add(square);
                }
            }
            return squares;
        }

        private List<List<(float x, float y)>> GetMidpoints()
        {
            float centerX = Config.MapGui.CurrentControl.Width / 2;
            float centerY = Config.MapGui.CurrentControl.Height / 2;

            List<List<(float x, float y)>> midpoints = new List<List<(float x, float y)>>();
            for (int x = 0; x < 4; x++)
            {
                List<(float x, float y)> oneRow = new List<(float x, float y)>();
                for (int y = 0; y < 4; y++)
                {
                    float midpointX = centerX + (x - 1.5f) * SQUARE_DIAMETER;
                    float midpointY = centerY + (y - 1.5f) * SQUARE_DIAMETER;
                    oneRow.Add((midpointX, midpointY));
                }
                midpoints.Add(oneRow);
            }
            return midpoints;
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CompassImage;
        }

        public override string GetName()
        {
            return "Render Table";
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
            };
        }
    }
}
