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
using System.Drawing.Imaging;

namespace STROOP.Map3
{
    public class Map3IwerlipseObject : Map3Object
    {
        private readonly static int NUM_POINTS = 256;

        public Map3IwerlipseObject()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Red;
        }

        public override void DrawOnControl()
        {
            for (int i = 1; i <= 12; i++)
            {
                DrawOnControl(i);
            }
        }

        private void DrawOnControl(int numQSteps)
        {
            MarioState marioState = MarioState.CreateMarioState();
            MarioState marioStateCenter = AirMovementCalculator.ApplyInputRepeatedly(marioState, RelativeDirection.Center, numQSteps);
            MarioState marioStateForward = AirMovementCalculator.ApplyInputRepeatedly(marioState, RelativeDirection.Forward, numQSteps);
            MarioState marioStateBackward = AirMovementCalculator.ApplyInputRepeatedly(marioState, RelativeDirection.Backward, numQSteps);
            MarioState marioStateLeft = AirMovementCalculator.ApplyInputRepeatedly(marioState, RelativeDirection.Left, numQSteps);

            ushort marioAngle = marioState.MarioAngle;
            (float cx, float cz) = (marioStateCenter.X, marioStateCenter.Z);
            (float fx, float fz) = (marioStateForward.X, marioStateForward.Z);
            (float bx, float bz) = (marioStateBackward.X, marioStateBackward.Z);
            (float lx, float lz) = (marioStateLeft.X, marioStateLeft.Z);

            double sideDist = MoreMath.GetDistanceBetween(cx, cz, lx, lz);
            double forwardDist = MoreMath.GetDistanceBetween(cx, cz, fx, fz);
            double backwardDist = MoreMath.GetDistanceBetween(cx, cz, bx, bz);

            DrawOnControl(cx, cz, sideDist, forwardDist, marioAngle, Color.Red);
            DrawOnControl(cx, cz, sideDist, backwardDist, marioAngle, Color.Blue);
        }

        private void DrawOnControl(float cx, float cz, double sideDist, double forwardDist, double rotatedAngle, Color color)
        {
            (float controlCenterX, float controlCenterZ) = Map3Utilities.ConvertCoordsForControl(cx, cz);
            List<(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, NUM_POINTS).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS) * 65536)
                .ConvertAll(angle => GetEllipsePoint(cx, cz, sideDist, forwardDist, rotatedAngle, angle))
                .ConvertAll(point => Map3Utilities.ConvertCoordsForControl((float)point.x, (float)point.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw circle
            GL.Color4(color.R, color.G, color.B, OpacityByte);
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Vertex2(controlCenterX, controlCenterZ);
            foreach ((float x, float z) in controlPoints)
            {
                GL.Vertex2(x, z);
            }
            GL.Vertex2(controlPoints[0].pointX, controlPoints[0].pointZ);
            GL.End();

            // Draw outline
            if (OutlineWidth != 0)
            {
                GL.Color4(OutlineColor.R, OutlineColor.G, OutlineColor.B, (byte)255);
                GL.LineWidth(OutlineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach ((float x, float z) in controlPoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        private (double x, double z) GetEllipsePoint(
            double centerX, double centerZ, double sidewaysDist, double forwardDist, double rotatedAngle, double angle)
        {
            double a = sidewaysDist;
            double b = forwardDist;
            double c = Math.Sqrt(a * a - b * b);

            double angleRadians = MoreMath.AngleUnitsToRadians(angle - rotatedAngle);
            double term1 = b * Math.Sin(angleRadians);
            double term2 = a * Math.Cos(angleRadians);
            double r = (a * b) / MoreMath.GetHypotenuse(term1, term2);

            return MoreMath.AddVectorToPoint(r, angle, centerX, centerZ);
        }

        public override string GetName()
        {
            return "Iwerlipses";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.PathImage;
        }
    }
}
