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
using STROOP.Forms;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapObjectHitboxTriangle : MapObjectTriangle
    {
        private readonly bool _isDefaultInstance;
        private readonly List<TriangleDataModel> _levelTriAddressList;
        private readonly List<TriangleDataModel> _objTriAddressList;

        public MapObjectHitboxTriangle(bool isDefaultInstance)
            : base()
        {
            _isDefaultInstance = isDefaultInstance;
            _levelTriAddressList = TriangleUtilities.GetLevelTriangles();
            _objTriAddressList = TriangleUtilities.GetObjectTriangles();

            Size = 40;
            LineWidth = 0;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            // do nothing
        }

        public override float GetWallRelativeHeightForOrthographicView()
        {
            return -30;
        }

        public override Color GetColorForOrthographicView(TriangleClassification classification)
        {
            switch (classification)
            {
                case TriangleClassification.Wall:
                    return Color.Green;
                case TriangleClassification.Floor:
                    return Color.Blue;
                case TriangleClassification.Ceiling:
                    return Color.Red;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override float GetSizeForOrthographicView(TriangleClassification classification)
        {
            switch (classification)
            {
                case TriangleClassification.Wall:
                    return 50;
                case TriangleClassification.Floor:
                    return 78;
                case TriangleClassification.Ceiling:
                    return 160;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void DrawOn2DControlOrthographicView(MapObjectHoverData hoverData)
        {
            if (_isDefaultInstance)
            {
                Opacity = 0.5;
                LineWidth = 1;
            }

            base.DrawOn2DControlOrthographicView(hoverData);
        }

        public override void DrawOn3DControl()
        {
            if (_isDefaultInstance)
            {
                Opacity = 1;
                LineWidth = 0;
            }

            List<List<(float x, float y, float z, Color color)>> triData = GetFilteredTriangles()
                .ConvertAll(tri => new List<(float x, float y, float z, Color color)>()
                {
                    (tri.X1, tri.Y1, tri.Z1, ColorUtilities.AddAlpha(GetColorForTri(tri, 1), OpacityByte)),
                    (tri.X2, tri.Y2, tri.Z2, ColorUtilities.AddAlpha(GetColorForTri(tri, 2), OpacityByte)),
                    (tri.X3, tri.Y3, tri.Z3, ColorUtilities.AddAlpha(GetColorForTri(tri, 3), OpacityByte)),
                });
            Map3DVertex[] vertexArray = triData.SelectMany(vertexList => vertexList).ToList()
                .ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), vertex.color)).ToArray();
            List<Map3DVertex[]> vertexArray2 = triData.ConvertAll(
                vertexList => vertexList.ConvertAll(vertex => new Map3DVertex(new Vector3(
                    vertex.x, vertex.y, vertex.z), LineColor)).ToArray());

            Matrix4 viewMatrix = GetModelMatrix() * Config.Map3DCamera.Matrix;
            GL.UniformMatrix4(Config.Map3DGraphics.GLUniformView, false, ref viewMatrix);

            int buffer1 = GL.GenBuffer();
            GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, buffer1);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Map3DVertex.Size), vertexArray, BufferUsageHint.DynamicDraw);
            Config.Map3DGraphics.BindVertices();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexArray.Length);
            GL.DeleteBuffer(buffer1);

            if (LineWidth != 0)
            {
                vertexArray2.ForEach(vertexes =>
                {
                    int buffer2 = GL.GenBuffer();
                    GL.BindTexture(TextureTarget.Texture2D, MapUtilities.WhiteTexture);
                    GL.BindBuffer(BufferTarget.ArrayBuffer, buffer2);
                    GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexes.Length * Map3DVertex.Size), vertexes, BufferUsageHint.DynamicDraw);
                    GL.LineWidth(LineWidth);
                    Config.Map3DGraphics.BindVertices();
                    GL.DrawArrays(PrimitiveType.LineLoop, 0, vertexes.Length);
                    GL.DeleteBuffer(buffer2);
                });
            }
        }

        private Color GetColorForTri(TriangleDataModel tri, int vertex)
        {
            double clampedNormY = MoreMath.Clamp(tri.NormY, -1, 1);
            Color color;
            switch (tri.Classification)
            {
                case TriangleClassification.Wall:
                    color = tri.XProjection ? Color.FromArgb(58, 116, 58) : Color.FromArgb(116, 203, 116);
                    break;
                case TriangleClassification.Floor:
                    color = Color.FromArgb(130, 130, 231).Darken(0.6 * (1 - clampedNormY));
                    break;
                case TriangleClassification.Ceiling:
                    color = Color.FromArgb(231, 130, 130).Darken(0.6 * (clampedNormY + 1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            double amount = MoreMath.Clamp(Size / 100, 0, 1);
            switch (vertex)
            {
                case 1:
                    return color.Lighten(amount);
                case 2:
                    return color;
                case 3:
                    return color.Darken(amount);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override List<TriangleDataModel> GetUnfilteredTriangles()
        {
            return _levelTriAddressList.Concat(_objTriAddressList).ToList();
        }

        public void Reset()
        {
            _levelTriAddressList.Clear();
            _levelTriAddressList.AddRange(TriangleUtilities.GetLevelTriangles());

            _objTriAddressList.Clear();
            _objTriAddressList.AddRange(TriangleUtilities.GetObjectTriangles());
        }

        public override string GetName()
        {
            return "Hitbox Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.HitboxTrisImage;
        }

        public override void Update()
        {
            int numAllTriangles = Config.Stream.GetInt(TriangleConfig.TotalTriangleCountAddress);
            int numLevelTriangles = Config.Stream.GetInt(TriangleConfig.LevelTriangleCountAddress);

            if (_levelTriAddressList.Count != numLevelTriangles)
            {
                _levelTriAddressList.Clear();
                _levelTriAddressList.AddRange(TriangleUtilities.GetLevelTriangles());
            }

            _objTriAddressList.Clear();
            _objTriAddressList.AddRange(TriangleUtilities.GetObjectTriangles());
        }

        public override ContextMenuStrip GetContextMenuStrip()
        {
            if (_contextMenuStrip == null)
            {
                _contextMenuStrip = new ContextMenuStrip();
                GetTriangleToolStripMenuItems().ForEach(item => _contextMenuStrip.Items.Add(item));
            }

            return _contextMenuStrip;
        }
    }
}
