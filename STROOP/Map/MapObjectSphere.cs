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

namespace STROOP.Map
{
    public abstract class MapObjectSphere : MapObjectCircle
    {
        public MapObjectSphere()
            : base()
        {
        }

        protected override List<(float centerX, float centerZ, float radius)> Get2DDimensions()
        {
            List<(float centerX, float centerY, float centerZ, float radius3D)> dimensions3D = Get3DDimensions();
            List<(float centerX, float centerZ, float radius)> dimensions2D = dimensions3D.ConvertAll(
                dimensions =>
                {
                    float marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);
                    float yDiff = marioY - dimensions.centerY;
                    float radiusSquared = dimensions.radius3D * dimensions.radius3D - yDiff * yDiff;
                    float radius2D = radiusSquared >= 0 ? (float)Math.Sqrt(radiusSquared) : 0;
                    return (dimensions.centerX, dimensions.centerZ, radius2D);
                });
            return dimensions2D;
        }

        protected abstract List<(float centerX, float centerY, float centerZ, float radius3D)> Get3DDimensions();

        protected override List<(float x, float y, float z)> GetPoints()
        {
            return Get3DDimensions().ConvertAll(d => (d.centerX, d.centerY, d.centerZ));
        }

        private List<(float centerX, float centerZ, float radius)> GetOrthographicDimensionsForControl()
        {
            return Get3DDimensions().ConvertAll(dimension =>
            {
                switch (Config.CurrentMapGraphics.MapViewYawValue)
                {
                    case 0:
                    case 32768:
                        {
                            float zDiff = Config.CurrentMapGraphics.MapViewCenterZValue - dimension.centerZ;
                            float xDistSquared = dimension.radius3D * dimension.radius3D - zDiff * zDiff;
                            float xDist = xDistSquared >= 0 ? (float)Math.Sqrt(xDistSquared) : 0;
                            float radius = xDist * Config.CurrentMapGraphics.MapViewScaleValue;
                            (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(
                                dimension.centerX, dimension.centerY, dimension.centerZ);
                            return (x, z, radius);
                        }
                    case 16384:
                    case 49152:
                        {
                            float xDiff = Config.CurrentMapGraphics.MapViewCenterXValue - dimension.centerX;
                            float zDistSquared = dimension.radius3D * dimension.radius3D - xDiff * xDiff;
                            float zDist = zDistSquared >= 0 ? (float)Math.Sqrt(zDistSquared) : 0;
                            float radius = zDist * Config.CurrentMapGraphics.MapViewScaleValue;
                            (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(
                                dimension.centerX, dimension.centerY, dimension.centerZ);
                            return (x, z, radius);
                        }
                    default:
                        {
                            float aDiff = (float)MoreMath.GetPlaneDistanceToPoint(
                                Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue,
                                Config.CurrentMapGraphics.MapViewCenterZValue, Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue,
                                dimension.centerX, dimension.centerY, dimension.centerZ);
                            float bDistSquared = dimension.radius3D * dimension.radius3D - aDiff * aDiff;
                            float bDist = bDistSquared >= 0 ? (float)Math.Sqrt(bDistSquared) : 0;
                            float radius = bDist * Config.CurrentMapGraphics.MapViewScaleValue;
                            (float x, float z) = MapUtilities.ConvertCoordsForControlOrthographicView(
                                dimension.centerX, dimension.centerY, dimension.centerZ);
                            return (x, z, radius);
                        }
                }
            });
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            List<(float centerX, float centerZ, float radius)> dimensionList = GetOrthographicDimensionsForControl();

            for (int i = 0; i < dimensionList.Count; i++)
            {
                (float controlCenterX, float controlCenterZ, float controlRadius) = dimensionList[i];
                List<(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, SpecialConfig.MapCircleNumPoints2D).ToList()
                    .ConvertAll(index => (index / (float)SpecialConfig.MapCircleNumPoints2D) * 65536)
                    .ConvertAll(angle => ((float, float))MoreMath.AddVectorToPoint(controlRadius, angle, controlCenterX, controlCenterZ));

                GL.BindTexture(TextureTarget.Texture2D, -1);
                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                // Draw circle
                byte opacityByte = OpacityByte;
                if (this == hoverData?.MapObject && i == hoverData?.Index)
                {
                    opacityByte = MapUtilities.GetHoverOpacityByte();
                }
                GL.Color4(Color.R, Color.G, Color.B, opacityByte);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex2(controlCenterX, controlCenterZ);
                foreach ((float x, float z) in controlPoints)
                {
                    GL.Vertex2(x, z);
                }
                GL.Vertex2(controlPoints[0].pointX, controlPoints[0].pointZ);
                GL.End();

                // Draw outline
                if (LineWidth != 0)
                {
                    GL.Color4(LineColor.R, LineColor.G, LineColor.B, (byte)255);
                    GL.LineWidth(LineWidth);
                    GL.Begin(PrimitiveType.LineLoop);
                    foreach ((float x, float z) in controlPoints)
                    {
                        GL.Vertex2(x, z);
                    }
                    GL.End();
                }
            }

            GL.Color4(1, 1, 1, 1.0f);
        }

        public override void DrawOn3DControl()
        {
            List<float> thetaValues = Enumerable.Range(0, SpecialConfig.MapCircleNumPoints3D).ToList()
                .ConvertAll(index => (index / (float)SpecialConfig.MapCircleNumPoints3D) * 65536);
            List<float> phiValues = Enumerable.Range(0, SpecialConfig.MapCircleNumPoints3D + 1).ToList()
                .ConvertAll(index => (index / (float)SpecialConfig.MapCircleNumPoints3D) * 32768 - 16384);

            (float x, float y, float z) GetSpherePoint(
                double radius, double theta, double phi, float startX, float startY, float startZ)
            {
                (double relX, double relY, double relZ) = MoreMath.SphericalToEuler_AngleUnits(radius, theta, phi);
                return (startX + (float)relX, startY + (float)relY, startZ + (float)relZ);
            }

            List<(float centerX, float centerY, float centerZ, float radius3D)> dimensionList = Get3DDimensions();

            foreach ((float centerX, float centerY, float centerZ, float radius3D) in dimensionList)
            {
                List<Map3DVertex[]> vertexArrayForSurfaces = new List<Map3DVertex[]>();
                for (int p = 0; p < phiValues.Count - 1; p++)
                {
                    float phi1 = phiValues[p];
                    float phi2 = phiValues[p + 1];
                    for (int t = 0; t < thetaValues.Count; t++)
                    {
                        float theta1 = thetaValues[t];
                        float theta2 = thetaValues[(t + 1) % thetaValues.Count];
                        List<(float x, float y, float z)> pointsList = new List<(float x, float y, float z)>()
                        {
                            GetSpherePoint(radius3D, theta1, phi1, centerX, centerY, centerZ),
                            GetSpherePoint(radius3D, theta1, phi2, centerX, centerY, centerZ),
                            GetSpherePoint(radius3D, theta2, phi2, centerX, centerY, centerZ),
                            GetSpherePoint(radius3D, theta2, phi1, centerX, centerY, centerZ),
                        };
                        Map3DVertex[] pointsArray = pointsList.ConvertAll(
                            vertex => new Map3DVertex(new Vector3(
                                vertex.x, vertex.y, vertex.z), Color4)).ToArray();
                        vertexArrayForSurfaces.Add(pointsArray);
                    }
                }

                Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
                GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

                vertexArrayForSurfaces.ForEach(vertexes =>
                {
                    int buffer = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    Config.Map3DGraphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer);
                });
            }
        }

        public override MapObjectHoverData GetHoverDataOrthographicView()
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe();
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            List<(float centerX, float centerZ, float radius)> dimensionList = GetOrthographicDimensionsForControl();
            for (int i = dimensionList.Count - 1; i >= 0; i--)
            {
                var dimension = dimensionList[i];
                double dist = MoreMath.GetDistanceBetween(dimension.centerX, dimension.centerZ, relPos.X, relPos.Y);
                if (dist <= dimension.radius)
                {
                    var inGameDimensionList = GetPoints();
                    var inGameDimension = inGameDimensionList[i];
                    return new MapObjectHoverData(this, inGameDimension.x, inGameDimension.y, inGameDimension.z, index: i);
                }
            }
            return null;
        }
    }
}
