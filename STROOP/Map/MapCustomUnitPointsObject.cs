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

namespace STROOP.Map
{
    public class MapCustomUnitPointsObject : MapQuadObject
    {
        private readonly List<(int x, int z)> _unitPoints;

        public MapCustomUnitPointsObject(List<(int x, int z)> unitPoints)
            : base()
        {
            _unitPoints = unitPoints;

            Opacity = 0.5;
            Color = Color.Orange;
        }

        public static MapCustomUnitPointsObject Create(string text)
        {
            if (text == null) return null;
            List<int?> nullableIntList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseIntNullable(word));
            if (nullableIntList.Any(nullableInt => !nullableInt.HasValue))
            {
                return null;
            }
            List<int> intList = nullableIntList.ConvertAll(nullableInt => nullableInt.Value);
            if (intList.Count % 2 != 0)
            {
                return null;
            }
            List<(int x, int z)> unitPoints = new List<(int x, int z)>();
            for (int i = 0; i < intList.Count; i += 2)
            {
                unitPoints.Add((intList[i], intList[i + 1]));
            }
            return new MapCustomUnitPointsObject(unitPoints);
        }

        protected override List<List<(float x, float y, float z)>> GetQuadList()
        {
            return MapUtilities.ConvertUnitPointsToQuads(_unitPoints);
        }

        public override string GetName()
        {
            return "Custom Points";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CustomPointsImage;
        }
    }
}
