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
    public class MapCompassObject : MapObject
    {
        private int _texXP = -1;
        private int _texXM = -1;
        private int _texZP = -1;
        private int _texZM = -1;

        private int _tex0 = -1;
        private int _tex16384 = -1;
        private int _tex32768 = -1;
        private int _tex49152 = -1;
        private int _texM16384 = -1;
        private int _texM32768 = -1;

        public MapCompassObject()
            : base()
        {
            Color = Color.White;
        }

        public override void DrawOn2DControl()
        {
            List<CompassArrow> arrows = Enumerable.Range(0, 4).ToList().ConvertAll(index => new CompassArrow(16384 * index));

            List<List<(float x, float z)>> triPoints = new List<List<(float x, float z)>>();
            foreach (CompassArrow arrow in arrows)
            {
                triPoints.Add(new List<(float x, float z)>() { arrow.ArrowHeadPoint, arrow.ArrowHeadCornerLeft, arrow.ArrowHeadCornerRight });
                triPoints.Add(new List<(float x, float z)>() { arrow.ArrowHeadInnerCornerRight, arrow.ArrowHeadInnerCornerLeft, arrow.ArrowBaseLeft });
                triPoints.Add(new List<(float x, float z)>() { arrow.ArrowBaseLeft, arrow.ArrowBaseRight, arrow.ArrowHeadInnerCornerRight });
                triPoints.Add(new List<(float x, float z)>() { arrow.ArrowBaseRight, arrow.ArrowBaseLeft, (SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ) });
            }
            List<List<(float x, float z)>> triPointsForControl =
                triPoints.ConvertAll(tri => tri.ConvertAll(
                    vertex => RotatePoint(vertex.x, vertex.z)));

            List<(float x, float z)> outlinePoints = arrows.ConvertAll(arrow => arrow.GetOutlinePoints()).SelectMany(points => points).ToList();
            List<(float x, float z)> outlinePointsForControl = outlinePoints.ConvertAll(point => RotatePoint(point.x, point.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw polygon
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
            GL.Begin(PrimitiveType.Triangles);
            foreach (List<(float x, float z)> tri in triPointsForControl)
            {
                foreach ((float x, float z) in tri)
                {
                    GL.Vertex2(x, z);
                }
            }
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach ((float x, float z) in outlinePointsForControl)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            // Draw direction labels
            if (SpecialConfig.CompassShowDirectionText != 0)
            {
                List<int> directionTexs = new List<int>() { _texZP, _texXP, _texZM, _texXM };
                for (int i = 0; i < arrows.Count; i++)
                {
                    CompassArrow arrow = arrows[i];
                    int tex = directionTexs[i];

                    (float x, float z) textPosition = arrow.DirectionTextPosition;
                    textPosition = RotatePoint(textPosition.x, textPosition.z);
                    PointF loc = new PointF(textPosition.x, textPosition.z);
                    SizeF size = new SizeF((int)SpecialConfig.CompassDirectionTextSize, (int)SpecialConfig.CompassDirectionTextSize);

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
            }

            // Draw angle labels
            if (SpecialConfig.CompassShowAngleText != 0)
            {
                List<int> angleTexs =
                    SpecialConfig.CompassAngleTextSigned != 0 ?
                    new List<int>() { _tex0, _tex16384, _texM32768, _texM16384 } :
                    new List<int>() { _tex0, _tex16384, _tex32768, _tex49152 };
                for (int i = 0; i < arrows.Count; i++)
                {
                    CompassArrow arrow = arrows[i];
                    int tex = angleTexs[i];

                    (float x, float z) textPosition = arrow.AngleTextPosition;
                    textPosition = RotatePoint(textPosition.x, textPosition.z);
                    PointF loc = new PointF(textPosition.x, textPosition.z);
                    SizeF size = new SizeF((int)SpecialConfig.CompassAngleTextSize, (int)SpecialConfig.CompassAngleTextSize);

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
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        private (float x, float z) RotatePoint(float x, float z)
        {
            return ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                x, z, SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ, -1 * Config.MapGraphics.MapViewAngleValue);
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
            return "Compass";
        }

        public override void Update()
        {
            if (_texXP == -1)
            {
                _texXP = MapUtilities.LoadTexture(CreateTexture("X+"));
            }
            if (_texXM == -1)
            {
                _texXM = MapUtilities.LoadTexture(CreateTexture("X-"));
            }
            if (_texZP == -1)
            {
                _texZP = MapUtilities.LoadTexture(CreateTexture("Z+"));
            }
            if (_texZM == -1)
            {
                _texZM = MapUtilities.LoadTexture(CreateTexture("Z-"));
            }

            if (_tex0 == -1)
            {
                _tex0 = MapUtilities.LoadTexture(CreateTexture("0"));
            }
            if (_tex16384 == -1)
            {
                _tex16384 = MapUtilities.LoadTexture(CreateTexture("16384"));
            }
            if (_tex32768 == -1)
            {
                _tex32768 = MapUtilities.LoadTexture(CreateTexture("32768"));
            }
            if (_tex49152 == -1)
            {
                _tex49152 = MapUtilities.LoadTexture(CreateTexture("49152"));
            }
            if (_texM16384 == -1)
            {
                _texM16384 = MapUtilities.LoadTexture(CreateTexture("-16384"));
            }
            if (_texM32768 == -1)
            {
                _texM32768 = MapUtilities.LoadTexture(CreateTexture("-32768"));
            }
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
                ToolStripMenuItem itemOpenSettings = new ToolStripMenuItem("Open Settings");
                itemOpenSettings.Click += (sender, e) =>
                {
                    List<(string specialType, string varName, WatchVariableSubclass subclass)> varData =
                    new List<(string specialType, string varName, WatchVariableSubclass subclass)>()
                        {
                            ("CompassPosition", "Position", WatchVariableSubclass.String),

                            ("CompassLineHeight", "Line Height", WatchVariableSubclass.Number),
                            ("CompassLineWidth", "Line Width", WatchVariableSubclass.Number),

                            ("CompassArrowHeight", "Arrow Height", WatchVariableSubclass.Number),
                            ("CompassArrowWidth", "Arrow Width", WatchVariableSubclass.Number),

                            ("CompassHorizontalMargin", "Horizontal Margin", WatchVariableSubclass.Number),
                            ("CompassVerticalMargin", "Vertical Margin", WatchVariableSubclass.Number),

                            ("CompassDirectionTextSize", "Direction Text Size", WatchVariableSubclass.Number),
                            ("CompassAngleTextSize", "Angle Text Size", WatchVariableSubclass.Number),

                            ("CompassDirectionTextPosition", "Direction Text Position", WatchVariableSubclass.Number),
                            ("CompassAngleTextPosition", "Angle Text Position", WatchVariableSubclass.Number),

                            ("CompassShowDirectionText", "Show Direction Text", WatchVariableSubclass.Boolean),
                            ("CompassShowAngleText", "Show Angle Text", WatchVariableSubclass.Boolean),

                            ("CompassAngleTextSigned", "Angle Text Signed", WatchVariableSubclass.Boolean),
                        };

                    List<WatchVariableControl> controls = new List<WatchVariableControl>();
                    foreach ((string specialType, string varName, WatchVariableSubclass subclass) in varData)
                    {
                        WatchVariable watchVar = new WatchVariable(
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

        public class CompassArrow
        {
            public readonly (float x, float z) ArrowBaseRight;
            public readonly (float x, float z) ArrowHeadInnerCornerRight;
            public readonly (float x, float z) ArrowHeadCornerRight;
            public readonly (float x, float z) ArrowHeadPoint;
            public readonly (float x, float z) ArrowHeadCornerLeft;
            public readonly (float x, float z) ArrowHeadInnerCornerLeft;
            public readonly (float x, float z) ArrowBaseLeft;
            public readonly (float x, float z) DirectionTextPosition;
            public readonly (float x, float z) AngleTextPosition;

            public CompassArrow(int angle)
            {
                double angleUp = angle;
                double angleDown = angle + 32768;
                double angleLeft = angle + 16384;
                double angleRight = angle - 16384;
                double angleUpLeft = angle + 8192;
                double angleUpRight = angle - 8192;
                double angleDownLeft = angle + 24576;
                double angleDownRight = angle - 24576;

                ArrowBaseLeft = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineWidth / Math.Sqrt(2), angleUpLeft, SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ);
                ArrowBaseRight = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineWidth / Math.Sqrt(2), angleUpRight, SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ);
                ArrowHeadInnerCornerLeft = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineHeight, angleUp, ArrowBaseLeft.x, ArrowBaseLeft.z);
                ArrowHeadInnerCornerRight = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineHeight, angleUp, ArrowBaseRight.x, ArrowBaseRight.z);
                ArrowHeadCornerLeft = ((float, float))MoreMath.AddVectorToPoint((SpecialConfig.CompassArrowWidth - SpecialConfig.CompassLineWidth) / 2, angleLeft, ArrowHeadInnerCornerLeft.x, ArrowHeadInnerCornerLeft.z);
                ArrowHeadCornerRight = ((float, float))MoreMath.AddVectorToPoint((SpecialConfig.CompassArrowWidth - SpecialConfig.CompassLineWidth) / 2, angleRight, ArrowHeadInnerCornerRight.x, ArrowHeadInnerCornerRight.z);
                ArrowHeadPoint = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineHeight + SpecialConfig.CompassArrowHeight, angleUp, SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ);
                DirectionTextPosition = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineHeight + SpecialConfig.CompassArrowHeight * 0.45 + SpecialConfig.CompassDirectionTextPosition, angleUp, SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ);
                AngleTextPosition = ((float, float))MoreMath.AddVectorToPoint(SpecialConfig.CompassLineHeight + SpecialConfig.CompassArrowHeight * 0.45 + SpecialConfig.CompassAngleTextPosition, angleUp, SpecialConfig.CompassCenterX, SpecialConfig.CompassCenterZ);
            }

            public List<(float x, float z)> GetOutlinePoints()
            {
                return new List<(float x, float z)>()
                {
                    ArrowHeadInnerCornerRight,
                    ArrowHeadCornerRight,
                    ArrowHeadPoint,
                    ArrowHeadCornerLeft,
                    ArrowHeadInnerCornerLeft,
                    ArrowBaseLeft,
                };
            }
        }
    }
}
