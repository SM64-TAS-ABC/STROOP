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
using System.Xml.Linq;

namespace STROOP.Map
{
    public class MapObjectPyramidPlatformNormals : MapObject
    {
        private readonly PositionAngle _posAngle;

        public MapObjectPyramidPlatformNormals(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            Opacity = 0.5;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            uint objAddress = _posAngle.GetObjAddress();
            float normalX = Config.Stream.GetFloat(objAddress + ObjectConfig.PyramidPlatformNormalXOffset);
            float normalZ = Config.Stream.GetFloat(objAddress + ObjectConfig.PyramidPlatformNormalZOffset);

            DrawCircles();
            DrawHyperbolas(true, normalX, Color.DarkRed);
            DrawHyperbolas(false, normalZ, Color.Lime);
        }

        private void DrawCircles()
        {
            uint objAddress = _posAngle.GetObjAddress();
            float normalY = Config.Stream.GetFloat(objAddress + ObjectConfig.PyramidPlatformNormalYOffset);

            double r1 = 500 * Math.Sqrt(1 / ((normalY + 0.01) * (normalY + 0.01)) - 1);
            double r2 = 500 * Math.Sqrt(1 / ((normalY) * (normalY)) - 1);
            double r3 = 500 * Math.Sqrt(1 / ((normalY - 0.01) * (normalY - 0.01)) - 1);

            Color lightCyan = Color.FromArgb(200, 255, 255);
            ShadeBetweenCircles((float)_posAngle.X, (float)_posAngle.Z, (float)r1, (float)r2, lightCyan);
            ShadeBetweenCircles((float)_posAngle.X, (float)_posAngle.Z, (float)r2, (float)r3, lightCyan);

            DrawCircle((float)_posAngle.X, (float)_posAngle.Z, (float)r1, Color.Cyan);
            DrawCircle((float)_posAngle.X, (float)_posAngle.Z, (float)r2, Color.Cyan);
            DrawCircle((float)_posAngle.X, (float)_posAngle.Z, (float)r3, Color.Cyan);
        }

        private void DrawCircle(float centerX, float centerZ, float radius, Color color)
        {
            (float controlCenterX, float controlCenterZ) = MapUtilities.ConvertCoordsForControlTopDownView(centerX, centerZ, UseRelativeCoordinates);
            float controlRadius = radius * Config.CurrentMapGraphics.MapViewScaleValue;
            List<(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 65536)
                .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(controlRadius, angle, controlCenterX, controlCenterZ));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw outline
            if (LineWidth != 0)
            {
                GL.Color4(color.R, color.G, color.B, (byte)255);
                GL.LineWidth(LineWidth);
                GL.Begin(PrimitiveType.LineLoop);
                foreach ((float x, float z) in controlPoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.End();
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        private void ShadeBetweenCircles(float centerX, float centerZ, float radius1, float radius2, Color color)
        {
            (float controlCenterX, float controlCenterZ) = MapUtilities.ConvertCoordsForControlTopDownView(centerX, centerZ, UseRelativeCoordinates);
            float controlRadius1 = radius1 * Config.CurrentMapGraphics.MapViewScaleValue;
            float controlRadius2 = radius2 * Config.CurrentMapGraphics.MapViewScaleValue;
            List<(float pointX, float pointZ)> controlPoints1 = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 65536)
                .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(controlRadius1, angle, controlCenterX, controlCenterZ));
            List<(float pointX, float pointZ)> controlPoints2 = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 65536)
                .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(controlRadius2, angle, controlCenterX, controlCenterZ));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            // Draw circle
            GL.Color4(color.R, color.G, color.B, OpacityByte);
            GL.Begin(PrimitiveType.QuadStrip);
            for (int i = 0; i <= controlPoints1.Count; i++)
            {
                int index = i % controlPoints1.Count;
                GL.Vertex2(controlPoints1[index].pointX, controlPoints1[index].pointZ);
                GL.Vertex2(controlPoints2[index].pointX, controlPoints2[index].pointZ);
            }
            GL.End();

            GL.Color4(1, 1, 1, 1.0f);
        }

        private void DrawHyperbolas(bool isForX, float normal, Color color)
        {
            List<double> offsets = new List<double>() { -0.01, 0, 0.01 };
            double range = 2000;
            foreach (double offset in offsets)
            {
                List<(float pointX, float pointZ)> controlPoints;
                if (isForX)
                {
                    controlPoints = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                        .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 2 * range - range + _posAngle.Z)
                        .ConvertAll(z => (Math.Sign(normal + offset) * Math.Sqrt((250000 + ((z - _posAngle.Z) * (z - _posAngle.Z))) / ((1 / ((normal + offset) * (normal + offset))) - 1)) + _posAngle.X, z))
                        .ConvertAll(p => MapUtilities.ConvertCoordsForControlTopDownView((float)p.Item1, (float)p.z, UseRelativeCoordinates));
                }
                else
                {
                    controlPoints = Enumerable.Range(0, MapConfig.MapCircleNumPoints2D).ToList()
                        .ConvertAll(index => (index / (float)MapConfig.MapCircleNumPoints2D) * 2 * range - range + _posAngle.X)
                        .ConvertAll(x => (Math.Sign(normal + offset) * Math.Sqrt((250000 + ((x - _posAngle.X) * (x - _posAngle.X))) / ((1 / ((normal + offset) * (normal + offset))) - 1)) + _posAngle.Z, x))
                        .ConvertAll(p => MapUtilities.ConvertCoordsForControlTopDownView((float)p.x, (float)p.Item1, UseRelativeCoordinates));
                }

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw outline
                if (LineWidth != 0)
                {
                    GL.Color4(color.R, color.G, color.B, (byte)255);
                    GL.LineWidth(LineWidth);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach ((float x, float z) in controlPoints)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }

                GL.Color4(1, 1, 1, 1.0f);
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override void DrawOn3DControl()
        {
            // do nothing
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override string GetName()
        {
            return "Pyramid Platform Normals for " + _posAngle.GetMapName();
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CoffinBoxImage;
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
