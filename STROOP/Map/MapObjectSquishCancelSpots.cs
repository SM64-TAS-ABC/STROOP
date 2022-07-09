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
using STROOP.Models;

namespace STROOP.Map
{
    public class MapObjectSquishCancelSpots : MapObjectQuad
    {
        private CellSnapshot _cellSnapshot;

        public MapObjectSquishCancelSpots()
            : base()
        {
            Opacity = 0.5;
            Color = Color.Purple;

            _cellSnapshot = new CellSnapshot();
        }

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            if (!MapUtilities.IsAbleToShowUnitPrecision())
            {
                return new List<List<(float x, float y, float z, bool isHovered)>>();
            }

            int xMin = (int)Config.CurrentMapGraphics.MapViewXMin - 1;
            int xMax = (int)Config.CurrentMapGraphics.MapViewXMax + 1;
            int zMin = (int)Config.CurrentMapGraphics.MapViewZMin - 1;
            int zMax = (int)Config.CurrentMapGraphics.MapViewZMax + 1;

            float y = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

            List<List<(float x, float y, float z, bool isHovered)>> quads =
                new List<List<(float x, float y, float z, bool isHovered)>>();
            for (int x = xMin; x <= xMax; x++)
            {
                for (int z = zMin; z <= zMax; z++)
                {
                    (TriangleDataModel floorTri, float floorY) = _cellSnapshot.FindFloorAndY(x, y, z);
                    (TriangleDataModel ceilingTri, float ceilingY) = _cellSnapshot.FindCeilingAndY(x, floorY + 80, z);

                    if (floorTri == null || ceilingTri == null) continue;
                    if (!floorTri.BelongsToObject && !ceilingTri.BelongsToObject) continue;
                    if (floorTri.NormY >= 0.5f && ceilingTri.NormY <= -0.5f) continue;

                    float ceilToFloorDist = ceilingY - floorY;
                    if (0 <= ceilToFloorDist && ceilToFloorDist <= 150.0f)
                    {
                        bool painful = ceilToFloorDist < 10.1f;
                        List<List<(float x, float y, float z)>> test = MapUtilities.ConvertUnitPointsToQuads(new List<(int x, int z)>() { (x, z) });
                        quads.AddRange(test.ConvertAll(test2 => test2.ConvertAll(test3 => (test3.x, test3.y, test3.z, false))));
                    }
                }
            }
            return quads;
        }

        public override string GetName()
        {
            return "Squish Cancel Spots";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomPointsImage;
        }

        public override void Update()
        {
            _cellSnapshot.Update();
        }

        public override MapObjectHoverData GetHoverDataTopDownView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;
            (float inGameX, float inGameZ) = MapUtilities.ConvertCoordsForInGameTopDownView(relPos.X, relPos.Y);
            var quadList = GetQuadList(null);
            for (int i = quadList.Count - 1; i >= 0; i--)
            {
                var quad = quadList[i];
                var simpleQuad = quad.ConvertAll(q => (q.x, q.y, q.z));
                if (MapUtilities.IsWithinRectangularQuad(simpleQuad, inGameX, inGameZ) || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, 0, 0, 0, index: i);
                }
            }
            return null;
        }

        public override MapObjectHoverData GetHoverDataOrthographicView(bool isForObjectDrag, bool forceCursorPosition)
        {
            Point? relPosMaybe = MapObjectHoverData.GetPositionMaybe(isForObjectDrag, forceCursorPosition);
            if (!relPosMaybe.HasValue) return null;
            Point relPos = relPosMaybe.Value;

            var quadList = GetQuadList(null);
            for (int i = quadList.Count - 1; i >= 0; i--)
            {
                var quad = quadList[i];
                var quadForControl = quad.ConvertAll(p => MapUtilities.ConvertCoordsForControlOrthographicView(p.x, p.y, p.z, UseRelativeCoordinates));
                if (MapUtilities.IsWithinShapeForControl(quadForControl, relPos.X, relPos.Y) || forceCursorPosition)
                {
                    return new MapObjectHoverData(this, 0, 0, 0, index: i);
                }
            }
            return null;
        }

        public override List<ToolStripItem> GetHoverContextMenuStripItems(MapObjectHoverData hoverData)
        {
            List<ToolStripItem> output = base.GetHoverContextMenuStripItems(hoverData);

            return output;
        }
    }
}
