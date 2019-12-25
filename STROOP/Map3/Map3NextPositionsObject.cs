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
            InternalRotates = true;
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.NextPositionsImage;
        }

        public override string GetName()
        {
            return "Next Positions";
        }

        public override float GetY()
        {
            return (float)PositionAngle.Mario.Y;
        }

        public override void DrawOn2DControl()
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

            float angleDegrees = Rotates ? Map3Utilities.ConvertAngleForControl(marioAngle) : 0;
            SizeF size = Map3Utilities.ScaleImageSizeForControl(Config.ObjectAssociations.BlueMarioMapImage.Size, Size);
            int fullStepTex = _useColoredMarios ? _blueMarioTex : _redMarioTex;
            int quarterStepTex = _useColoredMarios ? _orangeMarioText : _redMarioTex;
            for (int i = positionsOnControl.Count - 1; i >= 0; i--)
            {
                bool isFullStep = i % 4 == 3;
                if (!isFullStep && !_showQuarterSteps) continue;
                int tex = isFullStep ? fullStepTex : quarterStepTex;
                PointF point = new PointF(positionsOnControl[i].Item1, positionsOnControl[i].Item2);
                Map3Utilities.DrawTexture(tex, point, size, angleDegrees, Opacity);
            }
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemUseColoredMarios = new ToolStripMenuItem("Use Colored Marios");
                itemUseColoredMarios.Click += (sender, e) =>
                {
                    _useColoredMarios = !_useColoredMarios;
                    itemUseColoredMarios.Checked = _useColoredMarios;
                };
                itemUseColoredMarios.Checked = _useColoredMarios;

                ToolStripMenuItem itemShowQuarterSteps = new ToolStripMenuItem("Show Quarter Steps");
                itemShowQuarterSteps.Click += (sender, e) =>
                {
                    _showQuarterSteps = !_showQuarterSteps;
                    itemShowQuarterSteps.Checked = _showQuarterSteps;
                };
                itemShowQuarterSteps.Checked = _showQuarterSteps;

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
                _contextMenuStrip.Items.Add(itemUseColoredMarios);
                _contextMenuStrip.Items.Add(itemShowQuarterSteps);
                _contextMenuStrip.Items.Add(itemSetNumFrames);
            }

            return _contextMenuStrip;
        }

        public override bool ParticipatesInGlobalIconSize()
        {
            return true;
        }

        public override Map3DrawType GetDrawType()
        {
            return Map3DrawType.Overlay;
        }
    }
}
