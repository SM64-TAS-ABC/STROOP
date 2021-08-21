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

namespace STROOP.Map
{
    public abstract class MapObjectCircle : MapObject
    {
        public MapObjectCircle()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Red;
        }

        public override void DrawOn2DControlTopDownView(MapObjectHoverData hoverData)
        {
            List<(float centerX, float centerZ, float radius)> dimensionList = Get2DDimensions();

            for (int i = 0; i < dimensionList.Count; i++)
            {
                (float centerX, float centerZ, float radius) = dimensionList[i];
                (float controlCenterX, float controlCenterZ) = MapUtilities.ConvertCoordsForControlTopDownView(centerX, centerZ);
                float controlRadius = radius * Config.CurrentMapGraphics.MapViewScaleValue;
                List <(float pointX, float pointZ)> controlPoints = Enumerable.Range(0, SpecialConfig.MapCircleNumPoints2D).ToList()
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

        protected abstract List<(float centerX, float centerZ, float radius)> Get2DDimensions();

        public override MapDrawType GetDrawType()
        {
            return MapDrawType.Perspective;
        }

        public override MapObjectHoverData GetHoverData()
        {
            Point relPos = Config.MapGui.CurrentControl.PointToClient(MapObjectHoverData.GetCurrentPoint());
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGame(relPos.X, relPos.Y);

            List<(float centerX, float centerZ, float radius)> dimensionList = Get2DDimensions();
            int? hoverIndex = null;
            for (int i = 0; i < dimensionList.Count; i++)
            {
                var dimension = dimensionList[i];
                double dist = MoreMath.GetDistanceBetween(dimension.centerX, dimension.centerZ, inGameX, inGameZ);
                if (dist <= dimension.radius)
                {
                    hoverIndex = i;
                    break;
                }
            }
            return hoverIndex.HasValue ? new MapObjectHoverData(this, index: hoverIndex) : null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            List<(float centerX, float centerZ, float radius)> dimensionList = Get2DDimensions();
            var dimension = dimensionList[hoverData.Index.Value];
            List<object> posObjs = new List<object>() { dimension.centerX, dimension.centerZ };
            ToolStripMenuItem copyPositionItem = MapUtilities.CreateCopyItem(posObjs, "Position");
            output.Insert(0, copyPositionItem);

            return output;
        }
    }
}
