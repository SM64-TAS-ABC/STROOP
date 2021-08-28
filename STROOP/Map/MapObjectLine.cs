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
    public abstract class MapObjectLine : MapObject
    {
        public MapObjectLine()
            : base()
        {
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            MapUtilities.DrawLinesOn2DControlTopDownView(GetVerticesTopDownView(), LineWidth, LineColor, OpacityByte);

            if (_customImage != null)
            {
                List<(float x, float z)> positions = GetCustomImagePositions();
                for (int i = 0; i < positions.Count; i++)
                {
                    (float x, float z) = positions[i];
                    (float controlX, float controlZ) = MapUtilities.ConvertCoordsForControlTopDownView(x, z);
                    SizeF size = MapUtilities.ScaleImageSizeForControl(_customImage.Size, 8, Scales);
                    MapUtilities.DrawTexture(_customImageTex.Value, new PointF(controlX, controlZ), size, 0, 1);
                }
            }
        }

        public override void DrawOn2DControlOrthographicView()
        {
            MapUtilities.DrawLinesOn2DControlOrthographicView(GetVerticesOrthographicView(), LineWidth, LineColor, OpacityByte);
        }

        public override void DrawOn3DControl()
        {
            MapUtilities.DrawLinesOn3DControl(GetVertices3D(), LineWidth, LineColor, OpacityByte, GetModelMatrix());
        }

        protected abstract List<(float x, float y, float z)> GetVerticesTopDownView();

        protected virtual List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            return GetVerticesTopDownView();
        }

        protected virtual List<(float x, float y, float z)> GetVertices3D()
        {
            return GetVerticesTopDownView();
        }

        protected virtual List<(float x, float z)> GetCustomImagePositions()
        {
            return new List<(float x, float z)>();
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
