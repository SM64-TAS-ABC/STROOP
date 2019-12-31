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
using STROOP.Map3.Map.Graphics;

namespace STROOP.Map3
{
    public abstract class Map3SphereObject : Map3CircleObject
    {
        public Map3SphereObject()
            : base()
        {
        }

        protected override (float centerX, float centerZ, float radius) Get2DDimensions()
        {
            (float centerX, float centerY, float centerZ, float radius3D) = Get3DDimensions();
            float marioY = Config.Stream.GetSingle(MarioConfig.StructAddress + MarioConfig.YOffset);
            float yDiff = marioY - centerY;
            float radiusSquared = radius3D * radius3D - yDiff * yDiff;
            float radius2D = radiusSquared >= 0 ? (float)Math.Sqrt(radiusSquared) : 0;
            return (centerX, centerZ, radius2D);
        }

        protected abstract (float centerX, float centerY, float centerZ, float radius3D) Get3DDimensions();

        public override void DrawOn3DControl()
        {
            (float centerX, float centerY, float centerZ, float radius3D) = Get3DDimensions();

            List<float> thetaValues = Enumerable.Range(0, NUM_POINTS_3D).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS_3D) * 65536);
            List<float> phiValues = Enumerable.Range(0, NUM_POINTS_3D + 1).ToList()
                .ConvertAll(index => (index / (float)NUM_POINTS_3D) * 32768 - 16384);

            (float x, float y, float z) GetSpherePoint(
                double radius, double theta, double phi, float startX, float startY, float startZ)
            {
                (double relX, double relY, double relZ) = MoreMath.SphericalToEuler_AngleUnits(radius, theta, phi);
                return (startX + (float)relX, startY + (float)relY, startZ + (float)relZ);
            }

            List<Map4Vertex[]> vertexArrayForSurfaces = new List<Map4Vertex[]>();
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
                    Map4Vertex[] pointsArray = pointsList.ConvertAll(
                        vertex => new Map4Vertex(new Vector3(
                            vertex.x, vertex.y, vertex.z), Color4)).ToArray();
                    vertexArrayForSurfaces.Add(pointsArray);
                }
            }

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map4Camera.Matrix;
            GL.UniformMatrix4(Config.Map4Graphics.GLUniformView, false, ref viewMatrix);

            vertexArrayForSurfaces.ForEach(vertexes =>
            {
                int buffer = GL.GenBuffer();
                GL.BindTexture(TextureTarget.Texture2D, Map4GraphicsUtilities.WhiteTexture);
                GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
                GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map4Vertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                Config.Map4Graphics.BindVertices();
                GL.DrawArrays(PrimitiveType.Polygon, 0, vertexes.Length);
                GL.DeleteBuffer(buffer);
            });
        }
    }
}
