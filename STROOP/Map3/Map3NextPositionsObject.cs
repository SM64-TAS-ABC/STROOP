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
    public class Map3NextPositionsObject : Map3Object
    {
        private int _redMarioTex = -1;
        private int _blueMarioTex = -1;
        private int _orangeMarioText = -1;

        private bool _useColoredMarios = true;
        private bool _showQuarterSteps = true;
        private double _numFrames = 4;

        public Map3NextPositionsObject()
            : base()
        {
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.TriangleWallImage;
        }

        public override string GetName()
        {
            return "Next Positions";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Mario.Y;
        }

        public override void DrawOnControl()
        {
            if (_redMarioTex == -1)
            {
                _redMarioTex = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.MarioMapImage as Bitmap);
            }
            if (_blueMarioTex == -1)
            {
                _blueMarioTex = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.BlueMarioMapImage as Bitmap);
            }
            if (_orangeMarioText == -1)
            {
                _orangeMarioText = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.OrangeMarioMapImage as Bitmap);
            }

            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float marioHSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            List<(float, float)> positions =
                Enumerable.Range(0, (int)(_numFrames * 4)).ToList()
                .ConvertAll(index => 0.25 + index / 4.0)
                .ConvertAll(frameStep => ((float, float))MoreMath.AddVectorToPoint(
                    frameStep * marioHSpeed, marioAngle, marioX, marioZ));
            List<(float, float)> positionsOnControl = positions.ConvertAll(
                pos => Map3Utilities.ConvertCoordsForControl(pos.Item1, pos.Item2));

            float angleDegrees = Map3Utilities.ConvertAngleForControl(marioAngle);
            SizeF size = Map3Utilities.ScaleImageSize(Config.ObjectAssociations.BlueMarioMapImage.Size, Size);
            for (int i = positionsOnControl.Count - 1; i >= 0; i--)
            {
                int tex = (i % 4 == 3) ? _blueMarioTex : _orangeMarioText;
                PointF point = new PointF(positionsOnControl[i].Item1, positionsOnControl[i].Item2);
                Map3Utilities.DrawTexture(tex, point, size, angleDegrees, Opacity);
            }
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemToggleUseColoredMarios = new ToolStripMenuItem("Toggle Use Colored Marios");
                itemToggleUseColoredMarios.Click += (sender, e) => _useColoredMarios = !_useColoredMarios;

                ToolStripMenuItem itemToggleShowQuarterSteps = new ToolStripMenuItem("Toggle Show Quarter Steps");
                itemToggleShowQuarterSteps.Click += (sender, e) => _showQuarterSteps = !_showQuarterSteps;

                ToolStripMenuItem itemSetNumFrames = new ToolStripMenuItem("Set Num Frames...");
                itemSetNumFrames.Click += (sender, e) =>
                {
                    string text = DialogUtilities.GetStringFromDialog(labelText: "Enter num frames to the nearest 1/4th.");
                    double? numFramesNullable = ParsingUtilities.ParseDoubleNullable(text);
                    if (!numFramesNullable.HasValue) return;
                    double numFrames = numFramesNullable.Value;
                    _numFrames = numFrames;
                };

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemToggleUseColoredMarios);
                _contextMenuStrip.Items.Add(itemToggleShowQuarterSteps);
                _contextMenuStrip.Items.Add(itemSetNumFrames);
            }

            return _contextMenuStrip;
        }
    }
}
