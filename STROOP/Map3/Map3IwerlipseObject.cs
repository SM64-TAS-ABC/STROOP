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
            MarioState marioState = MarioState.CreateMarioState();
            MarioState marioStateCenter = AirMovementCalculator.ApplyInput(marioState, RelativeDirection.Center);
            MarioState marioStateForward = AirMovementCalculator.ApplyInput(marioState, RelativeDirection.Forward);
            MarioState marioStateBackward = AirMovementCalculator.ApplyInput(marioState, RelativeDirection.Backward);
            MarioState marioStateLeft = AirMovementCalculator.ApplyInput(marioState, RelativeDirection.Left);
            MarioState marioStateRight = AirMovementCalculator.ApplyInput(marioState, RelativeDirection.Right);

            ushort marioAngle = marioState.MarioAngle;
            (float cx, float cz) = (marioStateCenter.X, marioStateCenter.Z);
            (float fx, float fz) = (marioStateForward.X, marioStateForward.Z);
            (float bx, float bz) = (marioStateBackward.X, marioStateBackward.Z);
            (float lx, float lz) = (marioStateLeft.X, marioStateLeft.Z);
            (float rx, float rz) = (marioStateRight.X, marioStateRight.Z);

            double sideDist = MoreMath.GetDistanceBetween(cx, cz, lx, lz);
            double forwardDist = MoreMath.GetDistanceBetween(cx, cz, fx, fz);
            double backwardDist = MoreMath.GetDistanceBetween(cx, cz, bx, bz);

            (float controlCenterX, float controlCenterZ) = Map3Utilities.ConvertCoordsForControl(cx, cz);
            List<(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, NUM_POINTS).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS) * 65536)
                .ConvertAll(angle => GetEllipsePoint(cx, cz, sideDist, forwardDist, marioAngle, angle))
                .ConvertAll(point => Map3Utilities.ConvertCoordsForControl((float)point.x, (float)point.z));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw circle
            GL.Color4(Color.R, Color.G, Color.B, OpacityByte);
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
            double e = c / a;

            double angleRadians = MoreMath.AngleUnitsToRadians(angle - rotatedAngle);
            double term1 = b * Math.Sin(angleRadians);
            double term2 = a * Math.Cos(angleRadians);
            double r = (a * b) / MoreMath.GetHypotenuse(term1, term2);



            //double angleRadians = MoreMath.AngleUnitsToRadians(angle);
            //double term1 = forwardDist * Math.Cos(angleRadians);
            //double term2 = sidewaysDist * Math.Sin(angleRadians);
            //double radius = MoreMath.GetHypotenuse(term1, term2);
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
