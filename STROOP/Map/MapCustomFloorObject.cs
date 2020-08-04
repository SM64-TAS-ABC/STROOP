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

namespace STROOP.Map
{
    public class MapCustomFloorObject : MapFloorObject
    {
        private readonly List<uint> _triAddressList;

        public MapCustomFloorObject(List<uint> triAddressList)
            : base()
        {
            _triAddressList = triAddressList;
        }

        public static MapCustomFloorObject Create(string text)
        {
            if (text == null) return null;
            List<uint?> nullableUIntList = ParsingUtilities.ParseStringList(text)
                .ConvertAll(word => ParsingUtilities.ParseHexNullable(word));
            if (nullableUIntList.Any(nullableUInt => !nullableUInt.HasValue))
            {
                return null;
            }
            List<uint> uintList = nullableUIntList.ConvertAll(nullableUInt => nullableUInt.Value);
            return new MapCustomFloorObject(uintList);
        }

        protected override List<TriangleDataModel> GetTrianglesOfAnyDist()
        {
            return MapUtilities.GetTriangles(_triAddressList);
        }

        public override string GetName()
        {
            return "Custom Floor Tris";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.TriangleFloorImage;
        }
    }
}
