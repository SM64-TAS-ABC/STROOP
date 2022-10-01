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
using STROOP.Models;
using System.Windows.Forms;
using STROOP.Map.Map3D;
using OpenTK.Graphics;

namespace STROOP.Map
{
    public static class MapUtilities
    {
        public static int WhiteTexture { get; }
        private static readonly byte[] _whiteTexData = new byte[] { 0xFF };

        static MapUtilities()
        {
            WhiteTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, WhiteTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba8, 1, 1, 0, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, _whiteTexData);
        }

        public static Vector3 GetPositionOnViewFromCoordinate(Vector3 pos)
        {
            Vector4 vec = Vector4.Transform(new Vector4(pos, 1), Config.Map3DCamera.Matrix);
            vec.X /= vec.W;
            vec.Y /= vec.W;
            vec.Z = 0;
            return vec.Xyz;
        }

        /** Takes in in-game coordinates, outputs control coordinates. */
        public static (float x, float z) ConvertCoordsForControlTopDownView(float x, float z, bool useRelativeCoordinates)
        {
            if (useRelativeCoordinates)
            {
                x = (float)PuUtilities.GetRelativeCoordinate(x);
                z = (float)PuUtilities.GetRelativeCoordinate(z);
            }
            float xOffset = x - Config.CurrentMapGraphics.MapViewCenterXValue;
            float zOffset = z - Config.CurrentMapGraphics.MapViewCenterZValue;
            (float xOffsetRotated, float zOffsetRotated) =
                ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffset,
                    zOffset,
                    0,
                    0,
                    -1 * Config.CurrentMapGraphics.MapViewYawValue);
            float xOffsetPixels = xOffsetRotated * Config.CurrentMapGraphics.MapViewScaleValue;
            float zOffsetPixels = zOffsetRotated * Config.CurrentMapGraphics.MapViewScaleValue;
            float centerX = Config.MapGui.CurrentControl.Width / 2 + xOffsetPixels;
            float centerZ = Config.MapGui.CurrentControl.Height / 2 + zOffsetPixels;
            return (centerX, centerZ);
        }

        /** Takes in control coordinates, outputs in-game coordinates. */
        public static (float x, float z) ConvertCoordsForInGameTopDownView(float x, float z, GLControl control = null)
        {
            control = control ?? Config.MapGui.CurrentControl;
            MapGraphics graphics = MapGraphics.Dictionary[control];

            float xOffset = x - control.Width / 2;
            float zOffset = z - control.Height / 2;
            float xOffsetScaled = xOffset / graphics.MapViewScaleValue;
            float zOffsetScaled = zOffset / graphics.MapViewScaleValue;
            (float xOffsetScaledRotated, float zOffsetScaledRotated) =
                ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffsetScaled,
                    zOffsetScaled,
                    0,
                    0,
                    graphics.MapViewYawValue);
            float centerX = xOffsetScaledRotated + graphics.MapViewCenterXValue;
            float centerZ = zOffsetScaledRotated + graphics.MapViewCenterZValue;
            return (centerX, centerZ);
        }

        public static (float x, float z) ConvertCoordsForControlOrthographicView(float rawX, float rawY, float rawZ, bool useRelativeCoordinates)
        {
            float x = rawX;
            float y = rawY;
            float z = rawZ;
            if (useRelativeCoordinates)
            {
                x = (float)PuUtilities.GetRelativeCoordinate(rawX);
                y = (float)PuUtilities.GetRelativeCoordinate(rawY);
                z = (float)PuUtilities.GetRelativeCoordinate(rawZ);
            }
            float xOffset = x - Config.CurrentMapGraphics.MapViewCenterXValue;
            float yOffset = y - Config.CurrentMapGraphics.MapViewCenterYValue;
            float zOffset = z - Config.CurrentMapGraphics.MapViewCenterZValue;
            double angleRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewYawValue);
            float hOffset = (float)(Math.Sin(angleRadians) * zOffset - Math.Cos(angleRadians) * xOffset);

            (double x0, double y0, double z0, double t0) =
                MoreMath.GetPlaneLineIntersection(
                    Config.CurrentMapGraphics.MapViewCenterXValue,
                    Config.CurrentMapGraphics.MapViewCenterYValue,
                    Config.CurrentMapGraphics.MapViewCenterZValue,
                    Config.CurrentMapGraphics.MapViewYawValue,
                    Config.CurrentMapGraphics.MapViewPitchValue,
                    x, y, z,
                    Config.CurrentMapGraphics.MapViewYawValue,
                    Config.CurrentMapGraphics.MapViewPitchValue);
            double rightYaw = MoreMath.RotateAngleCW(
                Config.CurrentMapGraphics.MapViewYawValue, 16384);
            (double x1, double y1, double z1, double t1) =
                MoreMath.GetPlaneLineIntersection(
                    x0, y0, z0, rightYaw, 0,
                    Config.CurrentMapGraphics.MapViewCenterXValue,
                    Config.CurrentMapGraphics.MapViewCenterYValue,
                    Config.CurrentMapGraphics.MapViewCenterZValue,
                    rightYaw, 0);
            double hDiff = MoreMath.GetDistanceBetween(x1, z1, x0, z0);
            double yDiff = y1 - y0;
            double yDiffSign = yDiff >= 0 ? 1 : -1;
            double vOffsetMagnitude = MoreMath.GetHypotenuse(hDiff, yDiff);
            float vOffset = (float)(vOffsetMagnitude * yDiffSign);

            float hOffsetPixels = hOffset * Config.CurrentMapGraphics.MapViewScaleValue;
            float vOffsetPixels = vOffset * Config.CurrentMapGraphics.MapViewScaleValue;
            float centerH = Config.MapGui.CurrentControl.Width / 2 + hOffsetPixels;
            float centerV = Config.MapGui.CurrentControl.Height / 2 + vOffsetPixels;

            if (Config.CurrentMapGraphics.MapViewPitchValue == 0 && float.IsInfinity(rawX))
            {
                float yOffsetPixels = yOffset * Config.CurrentMapGraphics.MapViewScaleValue;
                float centerY = Config.MapGui.CurrentControl.Height / 2 - yOffsetPixels;
                if (float.IsNegativeInfinity(rawX)) return (0, centerY);
                else return (Config.MapGui.CurrentControl.Width, centerY);
            }

            return (centerH, centerV);
        }

        /** Takes in control coordinates, outputs in-game coordinates. */
        public static (float x, float y, float z) ConvertCoordsForInGameOrthographicView(float x, float z)
        {
            if (Config.CurrentMapGraphics.MapViewPitchValue != 0)
            {
                return (0, 0, 0);
            }

            float hOffset = x - Config.MapGui.CurrentControl.Width / 2;
            float vOffset = z - Config.MapGui.CurrentControl.Height / 2;
            float hOffsetScaled = hOffset / Config.CurrentMapGraphics.MapViewScaleValue;
            float vOffsetScaled = vOffset / Config.CurrentMapGraphics.MapViewScaleValue;
            double angleRadians = MoreMath.AngleUnitsToRadians(Config.CurrentMapGraphics.MapViewYawValue);
            float inGameX = Config.CurrentMapGraphics.MapViewCenterXValue - (float)Math.Cos(angleRadians) * hOffsetScaled;
            float inGameY = Config.CurrentMapGraphics.MapViewCenterYValue - vOffsetScaled;
            float inGameZ = Config.CurrentMapGraphics.MapViewCenterZValue + (float)Math.Sin(angleRadians) * hOffsetScaled;
            return (inGameX, inGameY, inGameZ);
        }

        /** Takes in in-game coordinates, outputs control coordinates. */
        public static (float x, float y, float z) ConvertCoordsForControlTopDownView(float x, float y, float z, bool useRelativeCoordinates)
        {
            (float convertedX, float convertedZ) = ConvertCoordsForControlTopDownView(x, z, useRelativeCoordinates);
            return (convertedX, y, convertedZ);
        }

        /** Takes in in-game angle, outputs control angle. */
        public static float ConvertAngleForControl(double angle)
        {
            angle += 32768 - Config.CurrentMapGraphics.MapViewYawValue;
            if (double.IsNaN(angle)) angle = 0;
            return (float)MoreMath.AngleUnitsToDegrees(angle);
        }

        public static SizeF ScaleImageSizeForControl(Size imageSize, float desiredRadius, bool scales)
        {
            float desiredDiameter = desiredRadius * 2;
            if (scales) desiredDiameter *= Config.CurrentMapGraphics.MapViewScaleValue;
            float scale = Math.Max(imageSize.Height / desiredDiameter, imageSize.Width / desiredDiameter);
            return new SizeF(imageSize.Width / scale, imageSize.Height / scale);
        }

        public static MapLayout GetMapLayout(object mapLayoutChoice = null)
        {
            mapLayoutChoice = mapLayoutChoice ?? Config.MapGui.comboBoxMapOptionsMap.SelectedItem;
            if (mapLayoutChoice is MapLayout mapLayout)
            {
                return mapLayout;
            }
            else
            {
                return Config.MapAssociations.GetBestMap();
            }
        }

        public static Image GetBackgroundImage(object backgroundChoice = null)
        {
            backgroundChoice = backgroundChoice ?? Config.MapGui.comboBoxMapOptionsBackground.SelectedItem;
            if (backgroundChoice is BackgroundImage background)
            {
                return background.Image;
            }
            else
            {
                return Config.MapAssociations.GetBestMap().BackgroundImage;
            }
        }

        public static List<(float x, float z)> GetPuCenters(int mod)
        {
            int puSize = 65536 * (SavedSettingsConfig.UseExtendedLevelBoundaries ? 4 : 1) * mod;

            int xMin = ((((int)Config.CurrentMapGraphics.MapViewXMin) / puSize) - 1) * puSize;
            int xMax = ((((int)Config.CurrentMapGraphics.MapViewXMax) / puSize) + 1) * puSize;
            int zMin = ((((int)Config.CurrentMapGraphics.MapViewZMin) / puSize) - 1) * puSize;
            int zMax = ((((int)Config.CurrentMapGraphics.MapViewZMax) / puSize) + 1) * puSize;
            List<(float x, float z)> centers = new List<(float x, float z)>();
            for (int x = xMin; x <= xMax; x += puSize)
            {
                for (int z = zMin; z <= zMax; z += puSize)
                {
                    centers.Add((x, z));
                }
            }
            return centers;
        }

        public static List<(float x, float z)> GetPuCoordinates(float relX, float relZ, int mod)
        {
            return GetPuCenters(mod).ConvertAll(center => (center.x + relX, center.z + relZ));
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

        public static List<TriangleDataModel> GetTriangles(uint triAddresses)
        {
            return GetTriangles(new List<uint>() { triAddresses });
        }

        public static List<TriangleDataModel> GetTriangles(List<uint> triAddresses)
        {
            return triAddresses.FindAll(triAddress => triAddress != 0)
                .ConvertAll(triAddress => TriangleDataModel.CreateLazy(triAddress));
        }
        
        public static List<List<(float x, float y, float z)>> ConvertUnitPointsToQuads(List<(int x, int z)> unitPoints)
        {
            List<List<(float x, float y, float z)>> quadList = new List<List<(float x, float y, float z)>>();
            Action<int, int, int, int> addQuad = (int xBase, int zBase, int xAdd, int zAdd) =>
            {
                quadList.Add(new List<(float x, float y, float z)>()
                {
                    (xBase, 0, zBase),
                    (xBase + xAdd, 0, zBase),
                    (xBase + xAdd, 0, zBase + zAdd),
                    (xBase, 0, zBase + zAdd),
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
                    addQuad(x, z, 1, MoreMath.Sign(z));
                    addQuad(x, z, -1, MoreMath.Sign(z));
                }
                else if (z == 0)
                {
                    addQuad(x, z, MoreMath.Sign(x), 1);
                    addQuad(x, z, MoreMath.Sign(x), -1);
                }
                else
                {
                    addQuad(x, z, MoreMath.Sign(x), MoreMath.Sign(z));
                }
            }
            return quadList;
        }

        public static float GetXWithMinAbsValue(List<(float x, float y, float z)> quad)
        {
            float bestX = float.PositiveInfinity;
            float bestAbs = float.PositiveInfinity;
            foreach ((float x, float y, float z) in quad)
            {
                float abs = Math.Abs(x);
                if (abs < bestAbs)
                {
                    bestX = x;
                    bestAbs = abs;
                }
            }
            return bestX;
        }

        public static float GetXWithMaxAbsValue(List<(float x, float y, float z)> quad)
        {
            float bestX = 0;
            float bestAbs = 0;
            foreach ((float x, float y, float z) in quad)
            {
                float abs = Math.Abs(x);
                if (abs > bestAbs)
                {
                    bestX = x;
                    bestAbs = abs;
                }
            }
            return bestX;
        }

        public static float GetZWithMinAbsValue(List<(float x, float y, float z)> quad)
        {
            float bestZ = float.PositiveInfinity;
            float bestAbs = float.PositiveInfinity;
            foreach ((float x, float y, float z) in quad)
            {
                float abs = Math.Abs(z);
                if (abs < bestAbs)
                {
                    bestZ = z;
                    bestAbs = abs;
                }
            }
            return bestZ;
        }

        public static float GetZWithMaxAbsValue(List<(float x, float y, float z)> quad)
        {
            float bestZ = 0;
            float bestAbs = 0;
            foreach ((float x, float y, float z) in quad)
            {
                float abs = Math.Abs(z);
                if (abs > bestAbs)
                {
                    bestZ = z;
                    bestAbs = abs;
                }
            }
            return bestZ;
        }

        public static bool IsWithinRectangularQuad(List<(float x, float y, float z)> quad, float x, float z)
        {
            float xMinAbs = GetXWithMinAbsValue(quad);
            float xMaxAbs = GetXWithMaxAbsValue(quad);
            float zMinAbs = GetZWithMinAbsValue(quad);
            float zMaxAbs = GetZWithMaxAbsValue(quad);

            if (xMaxAbs < 0)
            {
                if (x <= xMaxAbs) return false;
                if (x > xMinAbs) return false;
            }
            else
            {
                if (x < xMinAbs) return false;
                if (x >= xMaxAbs) return false;
            }

            if (zMaxAbs < 0)
            {
                if (z <= zMaxAbs) return false;
                if (z > zMinAbs) return false;
            }
            else
            {
                if (z < zMinAbs) return false;
                if (z >= zMaxAbs) return false;
            }

            return true;
        }

        public static bool IsWithinShapeForControl(List<(float x, float z)> quad, float x, float z, bool forceCursorPosition)
        {
            if (forceCursorPosition) return true;

            bool? leftOfLine = null;
            for (int i = 0; i < quad.Count; i++)
            {
                float x1 = quad[i].x;
                float z1 = quad[i].z;
                float x2 = quad[(i + 1) % quad.Count].x;
                float z2 = quad[(i + 1) % quad.Count].z;
                if (x1 == x2 && z1 == z2) // handle annihilated triangles
                {
                    return false;
                }
                bool left = MoreMath.IsPointLeftOfLine(x, z, x1, z1, x2, z2);
                if (leftOfLine.HasValue && leftOfLine.Value != left) return false;
                leftOfLine = left;
            }
            return true;
        }

        public static TriangleMapData Get2DWallDataFromTri(TriangleDataModel tri, float? heightNullable = null)
        {
            double uphillAngle = WatchVariableSpecialUtilities.GetTriangleUphillAngle(tri);
            double pushAngle = MoreMath.ReverseAngle(uphillAngle);

            if (!heightNullable.HasValue)
            {
                if (tri.X1 == tri.X2 && tri.Z1 == tri.Z2) // v2 is redundant
                    return new TriangleMapData(tri.X1, 0, tri.Z1, tri.X3, 0, tri.Z3, tri);
                if (tri.X1 == tri.X3 && tri.Z1 == tri.Z3) // v3 is redundant
                    return new TriangleMapData(tri.X1, 0, tri.Z1, tri.X2, 0, tri.Z2, tri);
                if (tri.X2 == tri.X3 && tri.Z2 == tri.Z3) // v3 is redundant
                    return new TriangleMapData(tri.X1, 0, tri.Z1, tri.X2, 0, tri.Z2, tri);

                double dist12 = MoreMath.GetDistanceBetween(tri.X1, tri.Z1, tri.X2, tri.Z2);
                double dist13 = MoreMath.GetDistanceBetween(tri.X1, tri.Z1, tri.X3, tri.Z3);
                double dist23 = MoreMath.GetDistanceBetween(tri.X2, tri.Z2, tri.X3, tri.Z3);

                if (dist12 >= dist13 && dist12 >= dist23)
                    return new TriangleMapData(tri.X1, 0, tri.Z1, tri.X2, 0, tri.Z2, tri);
                else if (dist13 >= dist23)
                    return new TriangleMapData(tri.X1, 0, tri.Z1, tri.X3, 0, tri.Z3, tri);
                else
                    return new TriangleMapData(tri.X2, 0, tri.Z2, tri.X3, 0, tri.Z3, tri);
            }

            float height = heightNullable.Value;
            (float pointAX, float pointAZ) = GetYOnLine(height, tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2);
            (float pointBX, float pointBZ) = GetYOnLine(height, tri.X1, tri.Y1, tri.Z1, tri.X3, tri.Y3, tri.Z3);
            (float pointCX, float pointCZ) = GetYOnLine(height, tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);

            List<(float x, float z)> points = new List<(float x, float z)>();
            if (!float.IsNaN(pointAX) && !float.IsNaN(pointAZ)) points.Add((pointAX, pointAZ));
            if (!float.IsNaN(pointBX) && !float.IsNaN(pointBZ)) points.Add((pointBX, pointBZ));
            if (!float.IsNaN(pointCX) && !float.IsNaN(pointCZ)) points.Add((pointCX, pointCZ));

            if (points.Count == 3)
            {
                double distAB = MoreMath.GetDistanceBetween(pointAX, pointAZ, pointBX, pointBZ);
                double distAC = MoreMath.GetDistanceBetween(pointAX, pointAZ, pointCX, pointCZ);
                double distBC = MoreMath.GetDistanceBetween(pointBX, pointBZ, pointCX, pointCZ);
                if (distAB >= distAC && distAB >= distBC)
                {
                    points.RemoveAt(2); // AB is biggest, so remove C
                }
                else if (distAC >= distBC)
                {
                    points.RemoveAt(1); // AC is biggest, so remove B
                }
                else
                {
                    points.RemoveAt(0); // BC is biggest, so remove A
                }
            }

            if (points.Count == 2)
            {
                return new TriangleMapData(points[0].x, 0, points[0].z, points[1].x, 0, points[1].z, tri);
            }

            return null;
        }

        public static TriangleMapData Get2DDataFromTri(TriangleDataModel tri)
        {
            if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
                (Config.CurrentMapGraphics.MapViewYawValue == 0 ||
                Config.CurrentMapGraphics.MapViewYawValue == 32768))
            {
                (float pointAX, float pointAY) = GetZOnLine(Config.CurrentMapGraphics.MapViewCenterZValue, tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2);
                (float pointBX, float pointBY) = GetZOnLine(Config.CurrentMapGraphics.MapViewCenterZValue, tri.X1, tri.Y1, tri.Z1, tri.X3, tri.Y3, tri.Z3);
                (float pointCX, float pointCY) = GetZOnLine(Config.CurrentMapGraphics.MapViewCenterZValue, tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);

                List<(float x, float y)> points = new List<(float x, float y)>();
                if (!float.IsNaN(pointAX) && !float.IsNaN(pointAY)) points.Add((pointAX, pointAY));
                if (!float.IsNaN(pointBX) && !float.IsNaN(pointBY)) points.Add((pointBX, pointBY));
                if (!float.IsNaN(pointCX) && !float.IsNaN(pointCY)) points.Add((pointCX, pointCY));

                if (points.Count == 3)
                {
                    double distAB = MoreMath.GetDistanceBetween(pointAX, pointAY, pointBX, pointBY);
                    double distAC = MoreMath.GetDistanceBetween(pointAX, pointAY, pointCX, pointCY);
                    double distBC = MoreMath.GetDistanceBetween(pointBX, pointBY, pointCX, pointCY);
                    if (distAB >= distAC && distAB >= distBC)
                    {
                        points.RemoveAt(2); // AB is biggest, so remove C
                    }
                    else if (distAC >= distBC)
                    {
                        points.RemoveAt(1); // AC is biggest, so remove B
                    }
                    else
                    {
                        points.RemoveAt(0); // BC is biggest, so remove A
                    }
                }

                if (points.Count == 2)
                {
                    return new TriangleMapData(
                        points[0].x, points[0].y, Config.CurrentMapGraphics.MapViewCenterZValue,
                        points[1].x, points[1].y, Config.CurrentMapGraphics.MapViewCenterZValue,
                        tri);
                }

                return null;
            }
            else if (Config.CurrentMapGraphics.MapViewPitchValue == 0 &&
               (Config.CurrentMapGraphics.MapViewYawValue == 16384 ||
               Config.CurrentMapGraphics.MapViewYawValue == 49152))
            {
                (float pointAY, float pointAZ) = GetXOnLine(Config.CurrentMapGraphics.MapViewCenterXValue, tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2);
                (float pointBY, float pointBZ) = GetXOnLine(Config.CurrentMapGraphics.MapViewCenterXValue, tri.X1, tri.Y1, tri.Z1, tri.X3, tri.Y3, tri.Z3);
                (float pointCY, float pointCZ) = GetXOnLine(Config.CurrentMapGraphics.MapViewCenterXValue, tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);

                List<(float y, float z)> points = new List<(float y, float z)>();
                if (!float.IsNaN(pointAY) && !float.IsNaN(pointAZ)) points.Add((pointAY, pointAZ));
                if (!float.IsNaN(pointBY) && !float.IsNaN(pointBZ)) points.Add((pointBY, pointBZ));
                if (!float.IsNaN(pointCY) && !float.IsNaN(pointCZ)) points.Add((pointCY, pointCZ));

                if (points.Count == 3)
                {
                    double distAB = MoreMath.GetDistanceBetween(pointAY, pointAZ, pointBY, pointBZ);
                    double distAC = MoreMath.GetDistanceBetween(pointAY, pointAZ, pointCY, pointCZ);
                    double distBC = MoreMath.GetDistanceBetween(pointBY, pointBZ, pointCY, pointCZ);
                    if (distAB >= distAC && distAB >= distBC)
                    {
                        points.RemoveAt(2); // AB is biggest, so remove C
                    }
                    else if (distAC >= distBC)
                    {
                        points.RemoveAt(1); // AC is biggest, so remove B
                    }
                    else
                    {
                        points.RemoveAt(0); // BC is biggest, so remove A
                    }
                }

                if (points.Count == 2)
                {
                    return new TriangleMapData(
                        Config.CurrentMapGraphics.MapViewCenterXValue, points[0].y, points[0].z,
                        Config.CurrentMapGraphics.MapViewCenterXValue, points[1].y, points[1].z,
                        tri);
                }

                return null;
            }
            else
            {
                (float pointAX, float pointAY, float pointAZ) = GetOnLine(
                    Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue,
                    Config.CurrentMapGraphics.MapViewCenterZValue, Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue,
                    tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2);
                (float pointBX, float pointBY, float pointBZ) = GetOnLine(
                    Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue,
                    Config.CurrentMapGraphics.MapViewCenterZValue, Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue,
                    tri.X1, tri.Y1, tri.Z1, tri.X3, tri.Y3, tri.Z3);
                (float pointCX, float pointCY, float pointCZ) = GetOnLine(
                    Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue,
                    Config.CurrentMapGraphics.MapViewCenterZValue, Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue,
                    tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);

                List<(float x, float y, float z)> points = new List<(float x, float y, float z)>();
                if (!float.IsNaN(pointAX) && !float.IsNaN(pointAY) && !float.IsNaN(pointAZ)) points.Add((pointAX, pointAY, pointAZ));
                if (!float.IsNaN(pointBX) && !float.IsNaN(pointBY) && !float.IsNaN(pointBZ)) points.Add((pointBX, pointBY, pointBZ));
                if (!float.IsNaN(pointCX) && !float.IsNaN(pointCY) && !float.IsNaN(pointCZ)) points.Add((pointCX, pointCY, pointCZ));

                if (points.Count == 3)
                {
                    double distAB = MoreMath.GetDistanceBetween(pointAX, pointAY, pointAZ, pointBX, pointBY, pointBZ);
                    double distAC = MoreMath.GetDistanceBetween(pointAX, pointAY, pointAZ, pointCX, pointCY, pointCZ);
                    double distBC = MoreMath.GetDistanceBetween(pointBX, pointBY, pointBZ, pointCX, pointCY, pointCZ);
                    if (distAB >= distAC && distAB >= distBC)
                    {
                        points.RemoveAt(2); // AB is biggest, so remove C
                    }
                    else if (distAC >= distBC)
                    {
                        points.RemoveAt(1); // AC is biggest, so remove B
                    }
                    else
                    {
                        points.RemoveAt(0); // BC is biggest, so remove A
                    }
                }

                if (points.Count == 2)
                {
                    return new TriangleMapData(
                        points[0].x, points[0].y, points[0].z,
                        points[1].x, points[1].y, points[1].z,
                        tri);
                }

                return null;
            }
        }

        private static (float y, float z) GetXOnLine(
            float x, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            if (x1 == x2 || x < Math.Min(x1, x2) || x > Math.Max(x1, x2))
                return (float.NaN, float.NaN);

            float p = (x - x1) / (x2 - x1);
            float py = y1 + p * (y2 - y1);
            float pz = z1 + p * (z2 - z1);
            return (py, pz);
        }

        private static (float x, float z) GetYOnLine(
            float y, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            if (y1 == y2 || y < Math.Min(y1, y2) || y > Math.Max(y1, y2))
                return (float.NaN, float.NaN);

            float p = (y - y1) / (y2 - y1);
            float px = x1 + p * (x2 - x1);
            float pz = z1 + p * (z2 - z1);
            return (px, pz);
        }

        private static (float x, float y) GetZOnLine(
            float z, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            if (z1 == z2 || z < Math.Min(z1, z2) || z > Math.Max(z1, z2))
                return (float.NaN, float.NaN);

            float p = (z - z1) / (z2 - z1);
            float px = x1 + p * (x2 - x1);
            float py = y1 + p * (y2 - y1);
            return (px, py);
        }

        private static (float x, float y, float z) GetOnLine(
            float x, float y, float z, float yaw, float pitch, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            (float x0, float y0, float z0, float t0) = ((float, float, float, float))
                MoreMath.GetPlaneLineIntersection(x, y, z, yaw, pitch, x1, y1, z1, x2, y2, z2);
            if (t0 < 0 || t0 > 1) return (float.NaN, float.NaN, float.NaN);
            return (x0, y0, z0);
        }

        public static void MaybeChangeMapCameraMode()
        {
            if (MapConfig.Map3DMode == Map3DCameraMode.InGame)
            {
                MapConfig.Map3DMode = Map3DCameraMode.CameraPosAndFocus;
            }
        }

        public static int MaybeReverse(int value)
        {
            return Config.MapGui.checkBoxMapOptionsEnableReverseDragging.Checked ? -1 * value : value;
        }

        private static string GetNumberWithCommas(int number)
        {
            string numberString = number.ToString();
            int count = 0;
            string outputString = "";
            for (int i = numberString.Length - 1; i >= 0; i--)
            {
                if (count > 0 && count % 3 == 0)
                {
                    outputString = "," + outputString;
                }
                outputString = numberString.Substring(i, 1) + outputString;
                count++;
            }
            return outputString;
        }

        public static void CreateTrackBarContextMenuStrip(TrackBarEx trackBar, Func<double> getterFunction)
        {
            List<int> maxValues = Enumerable.Range(1, 9).ToList().ConvertAll(p => (int)Math.Pow(10, p));
            maxValues.Add(65536);
            trackBar.ContextMenuStrip = new ContextMenuStrip();
            List<ToolStripMenuItem> items = maxValues.ConvertAll(
                maxValue => new ToolStripMenuItem("Max of " + GetNumberWithCommas(maxValue)));
            for (int i = 0; i < items.Count; i++)
            {
                int maxValue = maxValues[i];
                ToolStripMenuItem item = items[i];
                item.Click += (sender, e) =>
                {
                    trackBar.Maximum = maxValue;
                    trackBar.StartChangingByCode();
                    ControlUtilities.SetTrackBarValueCapped(trackBar, getterFunction());
                    trackBar.StopChangingByCode();
                    items.ForEach(it => it.Checked = it == item);
                };
                if (trackBar.Maximum == maxValue) item.Checked = true;
                trackBar.ContextMenuStrip.Items.Add(item);
            }
        }

        public static bool IsAbleToShowUnitPrecision()
        {
            return Config.CurrentMapGraphics.MapViewScaleValue > MapConfig.MapUnitPrecisionThreshold;
        }

        public static List<(double x, double y, double z)> ParsePoints(string text, bool useTriplets)
        {
            if (text == null) return null;

            List<double?> nullableDoubleList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseDoubleNullable(word));
            if (nullableDoubleList.Any(nullableDouble => !nullableDouble.HasValue))
            {
                return null;
            }
            List<double> doubleList = nullableDoubleList.ConvertAll(nullableDouble => nullableDouble.Value);

            int numbersPerGroup = useTriplets ? 3 : 2;
            if (doubleList.Count % numbersPerGroup != 0)
            {
                return null;
            }

            List<(double x, double y, double z)> points = new List<(double x, double y, double z)>();
            for (int i = 0; i < doubleList.Count; i += numbersPerGroup)
            {
                (double x, double y, double z) point =
                    useTriplets ?
                    (doubleList[i], doubleList[i + 1], doubleList[i + 2]) :
                    (doubleList[i], 0, doubleList[i + 1]);
                points.Add(point);
            }

            return points;
        }

        public static double GetSignedDistToCameraPlane(TriangleDataModel tri)
        {
            return MoreMath.Average(
                MoreMath.GetPlaneDistanceToPointSigned(
                    Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue, Config.CurrentMapGraphics.MapViewCenterZValue,
                    Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue, tri.X1, tri.Y1, tri.Z1),
                MoreMath.GetPlaneDistanceToPointSigned(
                    Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue, Config.CurrentMapGraphics.MapViewCenterZValue,
                    Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue, tri.X2, tri.Y2, tri.Z2),
                MoreMath.GetPlaneDistanceToPointSigned(
                    Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterYValue, Config.CurrentMapGraphics.MapViewCenterZValue,
                    Config.CurrentMapGraphics.MapViewYawValue, Config.CurrentMapGraphics.MapViewPitchValue, tri.X3, tri.Y3, tri.Z3));
        }

        public static void DrawLinesOn2DControlTopDownView(List<(float x, float y, float z)> vertices, float lineWidth, Color color, byte opacityByte, bool useRelativeCoordinates)
        {
            if (lineWidth == 0) return;

            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => ConvertCoordsForControlTopDownView(vertex.x, vertex.z, useRelativeCoordinates));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(color.R, color.G, color.B, opacityByte);
            GL.LineWidth(lineWidth);
            GL.Begin(PrimitiveType.Lines);
            foreach ((float x, float z) in veriticesForControl)
            {
                GL.Vertex2(x, z);
            }
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public static void DrawLinesOn2DControlOrthographicView(List<(float x, float y, float z)> vertices, float lineWidth, Color color, byte opacityByte, bool useRelativeCoordinates)
        {
            if (lineWidth == 0) return;

            List<(float x, float z)> veriticesForControl =
                vertices.ConvertAll(vertex => ConvertCoordsForControlOrthographicView(vertex.x, vertex.y, vertex.z, useRelativeCoordinates));

            GL.BindTexture(TextureTarget.Texture2D, -1);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Color4(color.R, color.G, color.B, opacityByte);
            GL.LineWidth(lineWidth);
            GL.Begin(PrimitiveType.Lines);
            foreach ((float x, float z) in veriticesForControl)
            {
                GL.Vertex2(x, z);
            }
            GL.End();
            GL.Color4(1, 1, 1, 1.0f);
        }

        public static void DrawLinesOn3DControl(List<(float x, float y, float z)> vertices, float lineWidth, Color color, byte opacityByte, Matrix4 modelMatrix)
        {
            if (lineWidth == 0) return;

            Color4 color4 = new Color4(color.R, color.G, color.B, opacityByte);
            Map3DVertex[] vertexArrayForEdges =
                vertices.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), color4)).ToArray();

            Matrix4 viewMatrix = modelMatrix * Config.Map3DCamera.Matrix;
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            int buffer = GL.GenBuffer();
            GL.BindTexture(TextureTarget.Texture2D, WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArrayForEdges.Length * Map3DVertex.Size),
                vertexArrayForEdges, BufferUsageHint.DynamicDraw);
            GL.LineWidth(lineWidth);
            Config.Map3DGraphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Lines, 0, vertexArrayForEdges.Length);
            GL.DeleteBuffer(buffer);
        }

        public static bool IsInVisibleSpace(double x, double z, double bufferDistance)
        {
            double dist = MoreMath.GetDistanceBetween(
                Config.CurrentMapGraphics.MapViewCenterXValue, Config.CurrentMapGraphics.MapViewCenterZValue, x, z);
            return dist < Config.CurrentMapGraphics.MapViewRadius + bufferDistance;
        }

        public static List<(double x, double z)> GetUnitPointsCrossSection(double bufferDistance)
        {
            float pointX = Config.CurrentMapGraphics.MapViewCenterXValue;
            float pointZ = Config.CurrentMapGraphics.MapViewCenterZValue;
            float lineAngle = Config.CurrentMapGraphics.MapViewYawValue - 16384;

            double xIntersection1 = MoreMath.GetLineIntersectionAtCoordinate(
                pointX, pointZ, lineAngle, Config.CurrentMapGraphics.MapViewZMin, false).x;
            double xIntersection2 = MoreMath.GetLineIntersectionAtCoordinate(
                pointX, pointZ, lineAngle, Config.CurrentMapGraphics.MapViewZMax, false).x;
            int xMin = (int)(Math.Max(Math.Min(xIntersection1, xIntersection2), Config.CurrentMapGraphics.MapViewXMin) - bufferDistance);
            int xMax = (int)(Math.Min(Math.Max(xIntersection1, xIntersection2), Config.CurrentMapGraphics.MapViewXMax) + bufferDistance);
            int z1 = (int)MoreMath.GetLineIntersectionAtCoordinate(pointX, pointZ, lineAngle, xMin, true).z;
            int z2 = (int)MoreMath.GetLineIntersectionAtCoordinate(pointX, pointZ, lineAngle, xMax, true).z;
            int zMin = Math.Min(z1, z2);
            int zMax = Math.Max(z1, z2);

            List<(double x, double z)> points = new List<(double x, double z)>();
            for (int x = xMin; x <= xMax; x++)
            {
                points.Add(MoreMath.GetLineIntersectionAtCoordinate(pointX, pointZ, lineAngle, x, true));
            }
            for (int z = zMin; z <= zMax; z++)
            {
                points.Add(MoreMath.GetLineIntersectionAtCoordinate(pointX, pointZ, lineAngle, z, false));
            }
            points = Enumerable.OrderBy(points, point => point.x).ToList();
            return points;
        }

        public static List<uint> ParseCustomTris(string text, TriangleClassification? classification)
        {
            if (text == null) return null;
            if (classification.HasValue && text == "")
            {
                uint currentTriangle = TriangleUtilities.GetCurrentTriangle(classification.Value);
                if (currentTriangle == 0) return null;
                return new List<uint>() { currentTriangle };
            }
            List<uint?> nullableUIntList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseHexNullable(word));
            if (nullableUIntList.Any(nullableUInt => !nullableUInt.HasValue))
            {
                return null;
            }
            return nullableUIntList.ConvertAll(nullableUInt => nullableUInt.Value);
        }

        public static double GetHoverOpacity()
        {
            long deltaTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() - MapObjectHoverData.HoverStartTime;
            double trig = Math.Cos(deltaTime / 150.0);
            double opacity = (trig + 1) / 4 + 0.5;
            return opacity;
        }

        public static byte GetHoverOpacityByte()
        {
            return (byte)(GetHoverOpacity() * 255);
        }

        public static ToolStripMenuItem CreateCopyItem(double x, double y, double z, string copyWord)
        {
            List<double> values = new List<double>() { x, y, z };
            List<double> lateralValues = new List<double>() { x, z };

            List<(string word, string character)> copyChoices =
                new List<(string word, string character)>()
                {
                    ("Commas", ","),
                    ("Spaces", " "),
                    ("Tabs", "\t"),
                    ("Line Breaks", "\r\n"),
                    ("Commas and Spaces", ", "),
                };

            ToolStripMenuItem copyItem = new ToolStripMenuItem("Copy " + copyWord + "...");
            foreach (var choice in copyChoices)
            {
                ToolStripMenuItem choiceItem = new ToolStripMenuItem("Copy " + copyWord + " with " + choice.word);
                choiceItem.Click += (sender, e) => Clipboard.SetText(string.Join(choice.character, values));
                copyItem.DropDownItems.Add(choiceItem);
            }
            copyItem.DropDownItems.Add(new ToolStripSeparator());
            foreach (var choice in copyChoices)
            {
                ToolStripMenuItem choiceItem = new ToolStripMenuItem("Copy Lateral " + copyWord + " with " + choice.word);
                choiceItem.Click += (sender, e) => Clipboard.SetText(string.Join(choice.character, lateralValues));
                copyItem.DropDownItems.Add(choiceItem);
            }
            return copyItem;
        }

        public static List<(float x, float z)> InterpolateQuarterSteps(List<(float x, float z)> points)
        {
            List<(float x, float z)> output = new List<(float x, float z)>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                (float x1, float z1) = points[i];
                (float x2, float z2) = points[i + 1];
                if (i == 0)
                {
                    output.Add((x1, z1));
                }
                for (int j = 1; j <= 4; j++)
                {
                    (float x, float z) point =
                        (x1 + (x2 - x1) * (j / 4f), z1 + (z2 - z1) * (j / 4f));
                    output.Add(point);
                }
            }
            return output;
        }

        public static List<(float x, float y, float z)> InterpolateQuarterSteps(List<(float x, float y, float z)> points)
        {
            List<(float x, float y, float z)> output = new List<(float x, float y, float z)>();
            for (int i = 0; i < points.Count - 1; i++)
            {
                (float x1, float y1, float z1) = points[i];
                (float x2, float y2, float z2) = points[i + 1];
                if (i == 0)
                {
                    output.Add((x1, y1, z1));
                }
                for (int j = 1; j <= 4; j++)
                {
                    (float x, float y, float z) point =
                        (x1 + (x2 - x1) * (j / 4f), y1 + (y2 - y1) * (j / 4f), z1 + (z2 - z1) * (j / 4f));
                    output.Add(point);
                }
            }
            return output;
        }

        public static List<(float x, float z)> GetFloatPositions(int limit)
        {
            float xMin = Config.CurrentMapGraphics.MapViewXMin;
            float xMax = Config.CurrentMapGraphics.MapViewXMax;
            float zMin = Config.CurrentMapGraphics.MapViewZMin;
            float zMax = Config.CurrentMapGraphics.MapViewZMax;

            List<(float x, float z)> output = new List<(float x, float z)>();
            for (float x = xMin; x <= xMax; x = MoreMath.GetNextFloat(x))
            {
                for (float z = zMin; z <= zMax; z = MoreMath.GetNextFloat(z))
                {
                    output.Add((x, z));
                    if (output.Count >= limit)
                    {
                        return new List<(float x, float z)>();
                    }
                }
            }
            return output;
        }

        public static MouseButtons GetMouseButton(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && KeyboardUtilities.IsShiftHeld())
            {
                return MouseButtons.Right;
            }
            return e.Button;
        }
    }
}
