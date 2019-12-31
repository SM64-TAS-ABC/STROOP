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
using System.Windows.Forms;
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public class Map3IwerlipsesObject : Map3Object
    {
        private readonly static int NUM_POINTS = 256;

        private bool _lockPositions = false;
        private MarioState _marioState = null;
        private bool _showQuarterSteps = true;

        public Map3IwerlipsesObject()
            : base()
        {
            Size = 12;
            Opacity = 0.5;
            Color = Color.Red;
        }

        public override void DrawOn2DControl()
        {
            for (int i = 1; i <= Size; i++)
            {
                if (i % 4 == 0 || _showQuarterSteps)
                {
                    DrawOn2DControl(i);
                }
            }
        }

        private void DrawOn2DControl(int numQSteps)
        {
            if (!_lockPositions)
            {
                _marioState = MarioState.CreateMarioState();
            }
            MarioState marioStateCenter = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Center, numQSteps);
            MarioState marioStateForward = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Forward, numQSteps);
            MarioState marioStateBackward = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Backward, numQSteps);
            MarioState marioStateLeft = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Left, numQSteps);

            ushort marioAngle = _marioState.MarioAngle;
            (float cx, float cz) = (marioStateCenter.X, marioStateCenter.Z);
            (float fx, float fz) = (marioStateForward.X, marioStateForward.Z);
            (float bx, float bz) = (marioStateBackward.X, marioStateBackward.Z);
            (float lx, float lz) = (marioStateLeft.X, marioStateLeft.Z);

            double sideDist = MoreMath.GetDistanceBetween(cx, cz, lx, lz);
            double forwardDist = MoreMath.GetDistanceBetween(cx, cz, fx, fz);
            double backwardDist = MoreMath.GetDistanceBetween(cx, cz, bx, bz);

            (float controlCenterX, float controlCenterZ) = Map3Utilities.ConvertCoordsForControl(cx, cz);
            List<(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, NUM_POINTS).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS) * 65536)
                .ConvertAll(angle => GetEllipsePoint(cx, cz, sideDist, forwardDist, backwardDist, marioAngle, angle))
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

        public override void DrawOn3DControl()
        {
            for (int i = 1; i <= Size; i++)
            {
                if (i % 4 == 0 || _showQuarterSteps)
                {
                    DrawOn3DControl(i);
                }
            }
        }

        private void DrawOn3DControl(int numQSteps)
        {
            if (!_lockPositions)
            {
                _marioState = MarioState.CreateMarioState();
            }
            MarioState marioStateCenter = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Center, numQSteps);
            MarioState marioStateForward = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Forward, numQSteps);
            MarioState marioStateBackward = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Backward, numQSteps);
            MarioState marioStateLeft = AirMovementCalculator.ApplyInputRepeatedly(_marioState, RelativeDirection.Left, numQSteps);

            ushort marioAngle = _marioState.MarioAngle;
            (float cx, float cz) = (marioStateCenter.X, marioStateCenter.Z);
            (float fx, float fz) = (marioStateForward.X, marioStateForward.Z);
            (float bx, float bz) = (marioStateBackward.X, marioStateBackward.Z);
            (float lx, float lz) = (marioStateLeft.X, marioStateLeft.Z);

            double sideDist = MoreMath.GetDistanceBetween(cx, cz, lx, lz);
            double forwardDist = MoreMath.GetDistanceBetween(cx, cz, fx, fz);
            double backwardDist = MoreMath.GetDistanceBetween(cx, cz, bx, bz);

            List<(float x, float y, float z)> points = Enumerable.Range(0, NUM_POINTS).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS) * 65536)
                .ConvertAll(angle => GetEllipsePoint(cx, cz, sideDist, forwardDist, backwardDist, marioAngle, angle))
                .ConvertAll(point => ((float)point.x, (float)marioStateCenter.Y, (float)point.z));

            Map4Vertex[] vertexArrayForSurfaces = points.ConvertAll(
                vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), Color4)).ToArray();
            Map4Vertex[] vertexArrayForEdges = points.ConvertAll(
                vertex => new Map4Vertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), OutlineColor)).ToArray();

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map4Camera.Matrix;
            GL.UniformMatrix4(Config.Map4Graphics.GLUniformView, false, ref viewMatrix);

            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArrayForSurfaces.Length * Map4Vertex.Size),
                    vertexArrayForSurfaces, BufferUsageHint.DynamicDraw);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexArrayForSurfaces.Length);
                GL.DeleteBuffer(buffer);
            }

            if (OutlineWidth != 0)
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, Config.Map4Graphics.Utilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArrayForEdges.Length * Map4Vertex.Size),
                    vertexArrayForEdges, BufferUsageHint.DynamicDraw);
                GL.LineWidth(OutlineWidth);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexArrayForEdges.Length);
                GL.DeleteBuffer(buffer);
            }
        }

        private (double x, double z) GetEllipsePoint(
            double centerX, double centerZ, double sidewaysDist, double forwardDist, double backwardDist, double marioAngle, double angle)
        {
            double a = sidewaysDist;
            double b = MoreMath.GetAngleDistance(marioAngle, angle) < 16384 ? forwardDist : backwardDist;

            double angleRadians = MoreMath.AngleUnitsToRadians(angle - marioAngle);
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
            return Config.ObjectAssociations.IwerlipsesImage;
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                ToolStripMenuItem itemLockPositions = new ToolStripMenuItem("Lock Positions");
                itemLockPositions.Click += (sender, e) =>
                {
                    _lockPositions = !_lockPositions;
                    itemLockPositions.Checked = _lockPositions;
                };
                itemLockPositions.Checked = _lockPositions;

                ToolStripMenuItem itemShowQuarterSteps = new ToolStripMenuItem("Show Quarter Steps");
                itemShowQuarterSteps.Click += (sender, e) =>
                {
                    _showQuarterSteps = !_showQuarterSteps;
                    itemShowQuarterSteps.Checked = _showQuarterSteps;
                };
                itemShowQuarterSteps.Checked = _showQuarterSteps;

                _contextMenuStrip = new ContextMenuStrip();
                _contextMenuStrip.Items.Add(itemLockPositions);
                _contextMenuStrip.Items.Add(itemShowQuarterSteps);
            }

            return _contextMenuStrip;
        }

        public override Map3DrawType GetDrawType()
        {
            return Map3DrawType.Perspective;
        }
    }
}
