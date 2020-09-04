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
    public class MapCustomSpherePointsObject : MapSphereObject
    {
        private readonly List<(float x, float y, float z)> _points;

        public MapCustomSpherePointsObject(List<(float x, float y, float z)> points)
            : base()
        {
            _points = points;

            Size = 100;
        }

        public static MapCustomSpherePointsObject Create(string text, bool useTriplets)
        {
            List<(double x, double y, double z)> points = MapUtilities.ParsePoints(text, useTriplets);
            if (points == null) return null;
            List<(float x, float y, float z)> floatPoints = points.ConvertAll(
                point => ((float)point.x, (float)point.y, (float)point.z));
            return new MapCustomSpherePointsObject(floatPoints);
        }

        protected override List<(float centerX, float centerY, float centerZ, float radius3D)> Get3DDimensions()
        {
            return _points.ConvertAll(point => (point.x, point.y, point.z, Size));
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.SphereImage;
        }

        public override string GetName()
        {
            return "Custom Sphere Points";
        }
    }
}
