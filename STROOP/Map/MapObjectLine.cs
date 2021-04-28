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

        public override void DrawOn2DControlTopDownView()
        {
            MapUtilities.DrawLinesOn2DControlTopDownView(GetVerticesTopDownView(), OutlineWidth, OutlineColor, OpacityByte);
        }

        public override void DrawOn2DControlOrthographicView()
        {
            MapUtilities.DrawLinesOn2DControlOrthographicView(GetVerticesOrthographicView(), OutlineWidth, OutlineColor, OpacityByte);
        }

        public override void DrawOn3DControl()
        {
            MapUtilities.DrawLinesOn3DControl(GetVerticesTopDownView(), OutlineWidth, OutlineColor, GetModelMatrix());
        }

        protected abstract List<(float x, float y, float z)> GetVerticesTopDownView();

        protected virtual List<(float x, float y, float z)> GetVerticesOrthographicView()
        {
            return GetVerticesTopDownView();
        }

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }
    }
}
