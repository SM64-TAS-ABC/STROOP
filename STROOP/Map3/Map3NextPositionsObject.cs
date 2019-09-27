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

namespace STROOP.Map3
{
    public class Map3NextPositionsObject : Map3Object
    {
        private int _fullStepTexture = -1;
        private int _quarterStepTexture = -1;

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
            if (_fullStepTexture == -1)
            {
                _fullStepTexture = Map3Utilities.LoadTexture(
                    Config.ObjectAssociations.BlueMarioMapImage as Bitmap);
            }
            if (_quarterStepTexture == -1)
            {
                _quarterStepTexture = Map3Utilities.LoadTexture(
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
                int tex = (i % 4 == 3) ? _fullStepTexture : _quarterStepTexture;
                PointF point = new PointF(positionsOnControl[i].Item1, positionsOnControl[i].Item2);
                Map3Utilities.DrawTexture(tex, point, size, angleDegrees, Opacity);
            }
        }
    }
}
