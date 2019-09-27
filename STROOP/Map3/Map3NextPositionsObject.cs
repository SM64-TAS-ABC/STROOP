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
        private double _numFrames = 4;

        public Map3NextPositionsObject()
            : base()
        {
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.MarioMapImage;
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
            float marioX = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.XOffset);
            float marioZ = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.ZOffset);
            float marioHSpeed = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.HSpeedOffset);
            ushort marioAngle = Config.Stream.GetUInt16(MarioConfig.StructAddress + MarioConfig.FacingYawOffset);
            List<(float, float)> nextPositions =
                Enumerable.Range(0, (int)(_numFrames * 4)).ToList()
                .ConvertAll(index => 0.25 + index / 4.0)
                .ConvertAll(frameStep => ((float, float))MoreMath.AddVectorToPoint(
                    frameStep * marioHSpeed, marioAngle, marioX, marioZ));
            List<(float, float)> nextPositionsOnControl = nextPositions.ConvertAll(
                pos => Map3Utilities.ConvertCoordsForControl(pos.Item1, pos.Item2));






        }
    }
}
