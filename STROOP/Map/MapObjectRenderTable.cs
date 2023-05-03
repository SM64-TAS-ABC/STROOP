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
        private static float SQUARE_DIAMETER = 180;
        private static float BIG_TEXT_SCALE = 0.41f;
        private static float SMALL_TEXT_SCALE = 0.25f;
        private static float BIG_TEXT_OFFSET = 6;
        private static float SMALL_TEXT_OFFSET = 70;
        private static float ARROW_CLOSER_OFFSET = 28;
        private static float ARROW_DOUBLE_OFFSET = 50;
        private static float IMAGE_HEIGHT = 120;
        private static float IMAGE_WIDTH = 155;
        private static float IMAGE_OFFSET = -13;

        private static Color lightRed = Color.FromArgb(255, 213, 213);
        private static Color lightBlue = Color.FromArgb(185, 252, 255);
        private static Color lightPurple = Color.FromArgb(249, 217, 255);
        private static Color darkGray = Color.FromArgb(150, 150, 150);

        private Image _activeImage = null;
        private Image _inactiveImage = null;
        private Image _visibleImage = null;
        private Image _invisibleImage = null;
        private Image _renderedImage = null;
        private Image _notRenderedImage = null;
        private Image _redArrowImage = null;
        private Image _blueArrowImage = null;
        private Image _blueArrowDistanceImage = null;
        private Image _grayArrowFlickerImage = null;
        private Image _renderedObjectImage = null;
        private Image _notRenderedObjectImage = null;

        private int _activeTex = -1;
        private int _inactiveTex = -1;
        private int _visibleTex = -1;
        private int _invisibleTex = -1;
        private int _renderedTex = -1;
        private int _notRenderedTex = -1;
        private int _redArrowTex = -1;
        private int _blueArrowTex = -1;
        private int _blueArrowDistanceTex = -1;
        private int _grayArrowFlickerTex = -1;
        private int _renderedObjectTex = -1;
        private int _notRenderedObjectTex = -1;

        private PositionAngle _posAngle;

        public MapObjectRenderTable(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;
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
                GL.LineWidth(4);
                GL.Begin(PrimitiveType.LineLoop);
                foreach (var point in square)
                {
                    GL.Vertex2(point.x, point.y);
                }
                GL.End();
            }

            var imageDatas = GetImageDatas();
            foreach (var imageData in imageDatas)
            {
                int tex = imageData.tex;
                (float x, float y) textPosition = (imageData.x, imageData.y);
                PointF loc = new PointF(textPosition.x, textPosition.y);
                SizeF size = new SizeF(imageData.image.Width * imageData.scale, imageData.image.Height * imageData.scale);

                // Place and rotate texture to correct location on control
                GL.LoadIdentity();
                GL.Translate(new Vector3(loc.X, loc.Y, 0));
                GL.Color4(1.0, 1.0, 1.0, 1.0);

                // Start drawing texture
                GL.TextureParameter(tex, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
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

        private List<List<(float x, float y, Color color)>> GetSquares()
        {
            List<List<(float x, float y)>> midpoints = GetMidpoints();
            bool isActive = Config.Stream.GetByte(_posAngle.GetObjAddress() + 0x3, mask: 0x01) != 0;
            bool isVisible = Config.Stream.GetByte(_posAngle.GetObjAddress() + 0x3, mask: 0x10) == 0;
            int activeX = isActive ? 2 : 3;
            int activeY = 1;
            int visibleX = 1;
            int visibleY = isVisible ? 2 : 3;
            bool isUnloaded = Config.Stream.GetUShort(_posAngle.GetObjAddress() + 0x74) == 0;

            List<List<(float x, float y, Color color)>> squares =
                new List<List<(float x, float y, Color color)>>();
            for (int x = 1; x < 4; x++)
            {
                for (int y = 1; y < 4; y++)
                {
                    if (x == 1 && y == 1) continue;

                    Color color = Color.White;
                    if (x == activeX && y == activeY)
                    {
                        color = lightRed;
                    }
                    if (x == visibleX && y == visibleY)
                    {
                        color = lightBlue;
                    }
                    if (x == activeX && y == visibleY)
                    {
                        color = lightPurple;
                    }

                    if (isUnloaded)
                    {
                        color = darkGray;
                    }

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

        private List<(Image image, int tex, float x, float y, float scale)> GetImageDatas()
        {
            List<List<(float x, float y)>> midpoints = GetMidpoints();
            bool isActive = Config.Stream.GetByte(_posAngle.GetObjAddress() + 0x3, mask: 0x01) != 0;
            bool isVisible = Config.Stream.GetByte(_posAngle.GetObjAddress() + 0x3, mask: 0x10) == 0;
            int activeX = isActive ? 2 : 3;
            int activeY = 0;
            int visibleX = 0;
            int visibleY = isVisible ? 2 : 3;
            int action = Config.Stream.GetInt(_posAngle.GetObjAddress() + ObjectConfig.ActionOffset);
            int timer = Config.Stream.GetInt(_posAngle.GetObjAddress() + ObjectConfig.TimerOffset);
            bool isVisible2 = !((action == 2 && timer >= 32 && timer % 2 == 0) || (action == 2 && timer > 70));
            int visible2X = 0;
            int visible2Y = isVisible2 ? 2 : 3;

            List<(Image image, int tex, float x, float y, float scale)> texts =
                new List<(Image image, int tex, float x, float y, float scale)>();
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    var midpoint = midpoints[x][y];
                    if (x == 2 && y == 1)
                    {
                        texts.Add((_activeImage, _activeTex, midpoint.x, midpoint.y + BIG_TEXT_OFFSET, BIG_TEXT_SCALE));
                    }
                    if (x == 3 && y == 1)
                    {
                        texts.Add((_inactiveImage, _inactiveTex, midpoint.x, midpoint.y + BIG_TEXT_OFFSET, BIG_TEXT_SCALE));
                    }
                    if (x == 1 && y == 2)
                    {
                        texts.Add((_visibleImage, _visibleTex, midpoint.x, midpoint.y + BIG_TEXT_OFFSET, BIG_TEXT_SCALE));
                    }
                    if (x == 1 && y == 3)
                    {
                        texts.Add((_invisibleImage, _invisibleTex, midpoint.x, midpoint.y + BIG_TEXT_OFFSET, BIG_TEXT_SCALE));
                    }

                    if (x == 2 && y == 2)
                    {
                        texts.Add((_renderedImage, _renderedTex, midpoint.x, midpoint.y + SMALL_TEXT_OFFSET, SMALL_TEXT_SCALE));
                    }
                    if (x == 2 && y == 3)
                    {
                        texts.Add((_notRenderedImage, _notRenderedTex, midpoint.x, midpoint.y + SMALL_TEXT_OFFSET, SMALL_TEXT_SCALE));
                    }
                    if (x == 3 && y == 2)
                    {
                        texts.Add((_notRenderedImage, _notRenderedTex, midpoint.x, midpoint.y + SMALL_TEXT_OFFSET, SMALL_TEXT_SCALE));
                    }
                    if (x == 3 && y == 3)
                    {
                        texts.Add((_notRenderedImage, _notRenderedTex, midpoint.x, midpoint.y + SMALL_TEXT_OFFSET, SMALL_TEXT_SCALE));
                    }
                    
                    if (_renderedObjectTex != -1)
                    {
                        if (x == 2 && y == 2)
                        {
                            texts.Add((_renderedObjectImage, _renderedObjectTex, midpoint.x, midpoint.y + IMAGE_OFFSET,
                                Math.Min(IMAGE_HEIGHT / _renderedObjectImage.Height, IMAGE_WIDTH / _renderedObjectImage.Width)));
                        }
                    }
                    if (_notRenderedObjectTex != -1)
                    {
                        if (x == 2 && y == 3)
                        {
                            texts.Add((_notRenderedObjectImage, _notRenderedObjectTex, midpoint.x, midpoint.y + IMAGE_OFFSET,
                                Math.Min(IMAGE_HEIGHT / _notRenderedObjectImage.Height, IMAGE_WIDTH / _notRenderedObjectImage.Width)));
                        }
                        if (x == 3 && y == 2)
                        {
                            texts.Add((_notRenderedObjectImage, _notRenderedObjectTex, midpoint.x, midpoint.y + IMAGE_OFFSET,
                                Math.Min(IMAGE_HEIGHT / _notRenderedObjectImage.Height, IMAGE_WIDTH / _notRenderedObjectImage.Width)));
                        }
                        if (x == 3 && y == 3)
                        {
                            texts.Add((_notRenderedObjectImage, _notRenderedObjectTex, midpoint.x, midpoint.y + IMAGE_OFFSET,
                                Math.Min(IMAGE_HEIGHT / _notRenderedObjectImage.Height, IMAGE_WIDTH / _notRenderedObjectImage.Width)));
                        }
                    }

                    if (x == activeX && y == activeY)
                    {
                        texts.Add((_redArrowImage, _redArrowTex, midpoint.x, midpoint.y + ARROW_CLOSER_OFFSET, 1));
                    }

                    if (Scales)
                    {
                        if (x == visible2X && y == visible2Y)
                        {
                            texts.Insert(0, (_grayArrowFlickerImage, _grayArrowFlickerTex, midpoint.x + ARROW_CLOSER_OFFSET, midpoint.y - ARROW_DOUBLE_OFFSET, 1));
                        }
                        if (x == visibleX && y == visibleY)
                        {
                            texts.Add((_blueArrowDistanceImage, _blueArrowDistanceTex, midpoint.x + ARROW_CLOSER_OFFSET, midpoint.y + ARROW_DOUBLE_OFFSET, 1));
                        }
                    }
                    else
                    {
                        if (x == visibleX && y == visibleY)
                        {
                            texts.Add((_blueArrowImage, _blueArrowTex, midpoint.x + ARROW_CLOSER_OFFSET, midpoint.y, 1));
                        }
                    }
                }
            }
            return texts;
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

        public override void Update()
        {
            if (_activeTex == -1)
            {
                _activeImage = Image.FromFile("Resources/Text Images/Active.png");
                _activeTex = MapUtilities.LoadTexture(_activeImage as Bitmap);
            }
            if (_inactiveTex == -1)
            {
                _inactiveImage = Image.FromFile("Resources/Text Images/Inactive.png");
                _inactiveTex = MapUtilities.LoadTexture(_inactiveImage as Bitmap);
            }
            if (_visibleTex == -1)
            {
                _visibleImage = Image.FromFile("Resources/Text Images/Visible.png");
                _visibleTex = MapUtilities.LoadTexture(_visibleImage as Bitmap);
            }
            if (_invisibleTex == -1)
            {
                _invisibleImage = Image.FromFile("Resources/Text Images/Invisible.png");
                _invisibleTex = MapUtilities.LoadTexture(_invisibleImage as Bitmap);
            }
            if (_renderedTex == -1)
            {
                _renderedImage = Image.FromFile("Resources/Text Images/Rendered.png");
                _renderedTex = MapUtilities.LoadTexture(_renderedImage as Bitmap);
            }
            if (_notRenderedTex == -1)
            {
                _notRenderedImage = Image.FromFile("Resources/Text Images/Not Rendered.png");
                _notRenderedTex = MapUtilities.LoadTexture(_notRenderedImage as Bitmap);
            }

            if (_redArrowTex == -1)
            {
                _redArrowImage = Image.FromFile("Resources/Text Images/red arrow.png");
                _redArrowTex = MapUtilities.LoadTexture(_redArrowImage as Bitmap);
            }
            if (_blueArrowTex == -1)
            {
                _blueArrowImage = Image.FromFile("Resources/Text Images/blue arrow.png");
                _blueArrowTex = MapUtilities.LoadTexture(_blueArrowImage as Bitmap);
            }
            if (_blueArrowDistanceTex == -1)
            {
                _blueArrowDistanceImage = Image.FromFile("Resources/Text Images/blue arrow distance.png");
                _blueArrowDistanceTex = MapUtilities.LoadTexture(_blueArrowDistanceImage as Bitmap);
            }
            if (_grayArrowFlickerTex == -1)
            {
                _grayArrowFlickerImage = Image.FromFile("Resources/Text Images/gray arrow flicker.png");
                _grayArrowFlickerTex = MapUtilities.LoadTexture(_grayArrowFlickerImage as Bitmap);
            }
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

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemSelectRenderedObjectImage = new ToolStripMenuItem("Select Rendered Object Image");
                itemSelectRenderedObjectImage.Click += (sender, e) =>
                {
                    string filePath = DialogUtilities.GetFilePath(FileType.Image);
                    if (filePath == null) return;
                    _renderedObjectImage = Image.FromFile(filePath);
                    _renderedObjectTex = MapUtilities.LoadTexture(_renderedObjectImage as Bitmap);
                };

                ToolStripMenuItem itemSelectNotRenderedObjectImage = new ToolStripMenuItem("Select Not Rendered Object Image");
                itemSelectNotRenderedObjectImage.Click += (sender, e) =>
                {
                    string filePath = DialogUtilities.GetFilePath(FileType.Image);
                    if (filePath == null) return;
                    _notRenderedObjectImage = Image.FromFile(filePath);
                    _notRenderedObjectTex = MapUtilities.LoadTexture(_notRenderedObjectImage as Bitmap);
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemSelectRenderedObjectImage);
                _contextMenuStrip.Items.Add(itemSelectNotRenderedObjectImage);
            }

            return _contextMenuStrip;
        }
    }
}
