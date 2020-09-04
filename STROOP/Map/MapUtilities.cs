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
        public static (float x, float z) ConvertCoordsForControl(float x, float z)
        {
            x = Config.MapGraphics.MapViewEnablePuView ? x : (float)PuUtilities.GetRelativeCoordinate(x);
            z = Config.MapGraphics.MapViewEnablePuView ? z : (float)PuUtilities.GetRelativeCoordinate(z);
            float xOffset = x - Config.MapGraphics.MapViewCenterXValue;
            float zOffset = z - Config.MapGraphics.MapViewCenterZValue;
            (float xOffsetRotated, float zOffsetRotated) =
                ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffset,
                    zOffset,
                    0,
                    0,
                    -1 * Config.MapGraphics.MapViewAngleValue);
            float xOffsetPixels = xOffsetRotated * Config.MapGraphics.MapViewScaleValue;
            float zOffsetPixels = zOffsetRotated * Config.MapGraphics.MapViewScaleValue;
            float centerX = Config.MapGui.GLControlMap2D.Width / 2 + xOffsetPixels;
            float centerZ = Config.MapGui.GLControlMap2D.Height / 2 + zOffsetPixels;
            return (centerX, centerZ);
        }

        /** Takes in control coordinates, outputs in-game coordinates. */
        public static (float x, float z) ConvertCoordsForInGame(float x, float z)
        {
            float xOffset = x - Config.MapGui.GLControlMap2D.Width / 2;
            float zOffset = z - Config.MapGui.GLControlMap2D.Height / 2;
            float xOffsetScaled = xOffset / Config.MapGraphics.MapViewScaleValue;
            float zOffsetScaled = zOffset / Config.MapGraphics.MapViewScaleValue;
            (float xOffsetScaledRotated, float zOffsetScaledRotated) =
                ((float, float))MoreMath.RotatePointAboutPointAnAngularDistance(
                    xOffsetScaled,
                    zOffsetScaled,
                    0,
                    0,
                    Config.MapGraphics.MapViewAngleValue);
            float centerX = xOffsetScaledRotated + Config.MapGraphics.MapViewCenterXValue;
            float centerZ = zOffsetScaledRotated + Config.MapGraphics.MapViewCenterZValue;
            return (centerX, centerZ);
        }

        /** Takes in in-game coordinates, outputs control coordinates. */
        public static (float x, float y, float z) ConvertCoordsForControl(float x, float y, float z)
        {
            (float convertedX, float convertedZ) = ConvertCoordsForControl(x, z);
            return (convertedX, y, convertedZ);
        }

        /** Takes in in-game angle, outputs control angle. */
        public static float ConvertAngleForControl(double angle)
        {
            angle += 32768 - Config.MapGraphics.MapViewAngleValue;
            if (double.IsNaN(angle)) angle = 0;
            return (float)MoreMath.AngleUnitsToDegrees(angle);
        }

        public static SizeF ScaleImageSizeForControl(Size imageSize, float desiredRadius)
        {
            float desiredDiameter = desiredRadius * 2;
            if (Config.MapGraphics.MapViewScaleIconSizes) desiredDiameter *= Config.MapGraphics.MapViewScaleValue;
            float scale = Math.Max(imageSize.Height / desiredDiameter, imageSize.Width / desiredDiameter);
            return new SizeF(imageSize.Width / scale, imageSize.Height / scale);
        }

        public static MapLayout GetMapLayout(object mapLayoutChoice = null)
        {
            mapLayoutChoice = mapLayoutChoice ?? Config.MapGui.comboBoxMapOptionsLevel.SelectedItem;
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

        public static List<(float x, float z)> GetPuCenters()
        {
            int xMin = ((((int)Config.MapGraphics.MapViewXMin) / 65536) - 1) * 65536;
            int xMax = ((((int)Config.MapGraphics.MapViewXMax) / 65536) + 1) * 65536;
            int zMin = ((((int)Config.MapGraphics.MapViewZMin) / 65536) - 1) * 65536;
            int zMax = ((((int)Config.MapGraphics.MapViewZMax) / 65536) + 1) * 65536;
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

        public static List<TriangleDataModel> GetTriangles(uint triAddresses)
        {
            return GetTriangles(new List<uint>() { triAddresses });
        }

        public static List<TriangleDataModel> GetTriangles(List<uint> triAddresses)
        {
            return triAddresses.FindAll(triAddress => triAddress != 0)
                .ConvertAll(triAddress => new TriangleDataModel(triAddress));
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

        public static (float x1, float z1, float x2, float z2, bool xProjection, double pushAngle)? Get2DWallDataFromTri(
            TriangleDataModel tri, float? heightNullable = null)
        {
            double uphillAngle = WatchVariableSpecialUtilities.GetTriangleUphillAngle(tri);
            double pushAngle = MoreMath.ReverseAngle(uphillAngle);

            if (!heightNullable.HasValue)
            {
                if (tri.X1 == tri.X2 && tri.Z1 == tri.Z2) // v2 is redundant
                    return (tri.X1, tri.Z1, tri.X3, tri.Z3, tri.XProjection, pushAngle);
                if (tri.X1 == tri.X3 && tri.Z1 == tri.Z3) // v3 is redundant
                    return (tri.X1, tri.Z1, tri.X2, tri.Z2, tri.XProjection, pushAngle);
                if (tri.X2 == tri.X3 && tri.Z2 == tri.Z3) // v3 is redundant
                    return (tri.X1, tri.Z1, tri.X2, tri.Z2, tri.XProjection, pushAngle);

                double dist12 = MoreMath.GetDistanceBetween(tri.X1, tri.Z1, tri.X2, tri.Z2);
                double dist13 = MoreMath.GetDistanceBetween(tri.X1, tri.Z1, tri.X3, tri.Z3);
                double dist23 = MoreMath.GetDistanceBetween(tri.X2, tri.Z2, tri.X3, tri.Z3);

                if (dist12 >= dist13 && dist12 >= dist23)
                    return (tri.X1, tri.Z1, tri.X2, tri.Z2, tri.XProjection, pushAngle);
                else if (dist13 >= dist23)
                    return (tri.X1, tri.Z1, tri.X3, tri.Z3, tri.XProjection, pushAngle);
                else
                    return (tri.X2, tri.Z2, tri.X3, tri.Z3, tri.XProjection, pushAngle);
            }

            float height = heightNullable.Value;
            (float pointAX, float pointAZ) = GetHeightOnLine(height, tri.X1, tri.Y1, tri.Z1, tri.X2, tri.Y2, tri.Z2);
            (float pointBX, float pointBZ) = GetHeightOnLine(height, tri.X1, tri.Y1, tri.Z1, tri.X3, tri.Y3, tri.Z3);
            (float pointCX, float pointCZ) = GetHeightOnLine(height, tri.X2, tri.Y2, tri.Z2, tri.X3, tri.Y3, tri.Z3);

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
                return (points[0].x, points[0].z, points[1].x, points[1].z, tri.XProjection, pushAngle);
            }

            return null;
        }

        private static (float x, float z) GetHeightOnLine(
            float height, float x1, float y1, float z1, float x2, float y2, float z2)
        {
            if (y1 == y2 || height < Math.Min(y1, y2) || height > Math.Max(y1, y2))
                return (float.NaN, float.NaN);

            float p = (height - y1) / (y2 - y1);
            float px = x1 + p * (x2 - x1);
            float pz = z1 + p * (z2 - z1);
            return (px, pz);
        }

        public static void MaybeChangeMapCameraMode()
        {
            if (SpecialConfig.Map3DMode == Map3DCameraMode.InGame)
            {
                SpecialConfig.Map3DMode = Map3DCameraMode.CameraPosAndFocus;
            }
        }

        public static int MaybeReverse(int value)
        {
            return Config.MapGui.checkBoxMapOptionsReverseDragging.Checked ? -1 * value : value;
        }

        public static void CreateTrackBarContextMenuStrip(TrackBar trackBar)
        {
            List<int> maxValues = Enumerable.Range(1, 9).ToList().ConvertAll(p => (int)Math.Pow(10, p));
            trackBar.ContextMenuStrip = new ContextMenuStrip();
            List<ToolStripMenuItem> items = maxValues.ConvertAll(
                maxValue => new ToolStripMenuItem("Max of " + maxValue));
            for (int i = 0; i < items.Count; i++)
            {
                int maxValue = maxValues[i];
                ToolStripMenuItem item = items[i];
                item.Click += (sender, e) =>
                {
                    trackBar.Maximum = maxValue;
                    items.ForEach(it => it.Checked = it == item);
                };
                if (trackBar.Maximum == maxValue) item.Checked = true;
                trackBar.ContextMenuStrip.Items.Add(item);
            }
        }

        public static bool IsAbleToShowUnitPrecision()
        {
            int xMin = (int)Config.MapGraphics.MapViewXMin - 1;
            int xMax = (int)Config.MapGraphics.MapViewXMax + 1;
            int zMin = (int)Config.MapGraphics.MapViewZMin - 1;
            int zMax = (int)Config.MapGraphics.MapViewZMax + 1;

            int xDiff = xMax - xMin;
            int zDiff = zMax - zMin;

            return xDiff < Config.MapGui.GLControlMap2D.Width &&
                zDiff < Config.MapGui.GLControlMap2D.Height;
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
    }
}
