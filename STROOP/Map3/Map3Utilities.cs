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
using STROOP.Models;

namespace STROOP.Map3
{
    public static class Map3Utilities
    {
        /** Takes in in-game coordinates, outputs control coordinates. */
        public static (float x, float z) ConvertCoordsForControl(float x, float z)
        {
            x = Config.Map3Graphics.MapViewEnablePuView ? x : (float)PuUtilities.GetRelativeCoordinate(x);
            z = Config.Map3Graphics.MapViewEnablePuView ? z : (float)PuUtilities.GetRelativeCoordinate(z);
            float xOffset = x - Config.Map3Graphics.MapViewCenterXValue;
            float zOffset = z - Config.Map3Graphics.MapViewCenterZValue;
            (float xOffsetRotated, float zOffsetRotated) =
                ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffset,
                    zOffset,
                    0,
                    0,
                    -1 * Config.Map3Graphics.MapViewAngleValue);
            float xOffsetPixels = xOffsetRotated * Config.Map3Graphics.MapViewScaleValue;
            float zOffsetPixels = zOffsetRotated * Config.Map3Graphics.MapViewScaleValue;
            float centerX = Config.Map3Gui.GLControl.Width / 2 + xOffsetPixels;
            float centerZ = Config.Map3Gui.GLControl.Height / 2 + zOffsetPixels;
            return (centerX, centerZ);
        }

        /** Takes in in-game angle, outputs control angle. */
        public static float ConvertAngleForControl(double angle)
        {
            angle += 32768 - Config.Map3Graphics.MapViewAngleValue;
            if (double.IsNaN(angle)) angle = 0;
            return (float)MoreMath.AngleUnitsToDegrees(angle);
        }

        public static SizeF ScaleImageSizeForControl(Size imageSize, float radius)
        {
            float scale = Math.Max(imageSize.Height / (2 * radius), imageSize.Width / (2 * radius));
            return new SizeF(imageSize.Width / scale, imageSize.Height / scale);
        }

        public static MapLayout GetMapLayout()
        {
            object mapLayoutChoice = Config.Map3Gui.comboBoxMap3OptionsLevel.SelectedItem;
            if (mapLayoutChoice is MapLayout mapLayout)
            {
                return mapLayout;
            }
            else
            {
                return Config.MapAssociations.GetBestMap();
            }
        }

        public static List<(float x, float z)> GetPuCenters()
        {
            int xMin = ((((int)Config.Map3Graphics.MapViewXMin) / 65536) - 1) * 65536;
            int xMax = ((((int)Config.Map3Graphics.MapViewXMax) / 65536) + 1) * 65536;
            int zMin = ((((int)Config.Map3Graphics.MapViewZMin) / 65536) - 1) * 65536;
            int zMax = ((((int)Config.Map3Graphics.MapViewZMax) / 65536) + 1) * 65536;
            List<(float x, float z)> centers = new List<(float x, float z)>();
            for (int x = xMin; x <= xMax; x += 65536)
            {
                for (int z = zMin; z <= zMax; z += 65536)
                {
                    centers.Add((x, z));
                }
            }
            return centers;
        }

        public static List<(float x, float z)> GetPuCoordinates(float relX, float relZ)
        {
            return GetPuCenters().ConvertAll(center => (center.x + relX, center.z + relZ));
        }

        public static int LoadTexture(Bitmap bmp)
        {
            // Create texture and id
            int id = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, id);

            // Set Bi-Linear Texture Filtering
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapNearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Get data from bitmap image
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            // Store bitmap data as OpenGl texture
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, bmp.Width, bmp.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            // Generate mipmaps for texture filtering
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            return id;
        }

        public static void DrawTexture(int tex, PointF loc, SizeF size, float angle, double opacity)
        {
            // Place and rotate texture to correct location on control
            GL.LoadIdentity();
            GL.Translate(new Vector3(loc.X, loc.Y, 0));
            GL.Rotate(360 - angle, Vector3.UnitZ);
            GL.Color4(1.0, 1.0, 1.0, opacity);

            // Start drawing texture
            GL.BindTexture(TextureTarget.Texture2D, tex);
            GL.Begin(PrimitiveType.Quads);

            // Set drawing coordinates
            GL.TexCoord2(0.0f, 1.0f); GL.Vertex2(-size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 1.0f); GL.Vertex2(size.Width / 2, size.Height / 2);
            GL.TexCoord2(1.0f, 0.0f); GL.Vertex2(size.Width / 2, -size.Height / 2);
            GL.TexCoord2(0.0f, 0.0f); GL.Vertex2(-size.Width / 2, -size.Height / 2);

            GL.End();
        }

        public static List<List<(float x, float z)>> GetTriangleVertexLists(uint triAddresses)
        {
            return GetTriangleVertexLists(new List<uint>() { triAddresses });
        }

        public static List<List<(float x, float z)>> GetTriangleVertexLists(List<uint> triAddresses)
        {
            return triAddresses.ConvertAll(triAddress =>
            {
                if (triAddress == 0) return new List<(float, float)>();
                return new TriangleDataModel(triAddress).Get2DVertices();
            });
        }
        
        public static List<List<(float x, float z)>> ConvertUnitPointsToQuads(List<(int x, int z)> unitPoints)
        {
            List<List<(float x, float z)>> quadList = new List<List<(float x, float z)>>();
            Action<int, int, int, int> addQuad = (int xBase, int zBase, int xAdd, int zAdd) =>
            {
                quadList.Add(new List<(float, float)>()
                {
                    (xBase, zBase),
                    (xBase + xAdd, zBase),
                    (xBase + xAdd, zBase + zAdd),
                    (xBase, zBase + zAdd),
                });
            };
            foreach ((int x, int z) in unitPoints)
            {
                if (x == 0 && z == 0)
                {
                    addQuad(x, z, 1, 1);
                    addQuad(x, z, 1, -1);
                    addQuad(x, z, -1, 1);
                    addQuad(x, z, -1, -1);
                }
                else if (x == 0)
                {
                    addQuad(x, z, 1, Math.Sign(z));
                    addQuad(x, z, -1, Math.Sign(z));
                }
                else if (z == 0)
                {
                    addQuad(x, z, Math.Sign(x), 1);
                    addQuad(x, z, Math.Sign(x), -1);
                }
                else
                {
                    addQuad(x, z, Math.Sign(x), Math.Sign(z));
                }
            }
            return quadList;
        }

        public static (float x1, float z1, float x2, float z2, bool xProjection) GetWallDataFromTri(TriangleDataModel tri)
        {
            if (tri.X1 == tri.X2 && tri.Z1 == tri.Z2)
                return (tri.X1, tri.Z1, tri.X3, tri.Z3, tri.XProjection);
            if (tri.X1 == tri.X3 && tri.Z1 == tri.Z3)
                return (tri.X1, tri.Z1, tri.X2, tri.Z2, tri.XProjection);
            else
                return (tri.X2, tri.Z2, tri.X3, tri.Z3, tri.XProjection);
        }
    }
}
