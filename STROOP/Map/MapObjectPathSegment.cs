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
using System.Windows.Forms;
using STROOP.Map.Map3D;

namespace STROOP.Map
{
    public class MapObjectPathSegment
    {
        public readonly int Index;

        public readonly float StartX;
        public readonly float StartZ;

        public readonly float EndX;
        public readonly float EndZ;

        public readonly float LineWidth;
        public readonly Color Color;
        public readonly byte Opacity;

        public MapObjectPathSegment(
            int index,
            float startX,
            float startZ,
            float endX,
            float endZ,
            float lineWidth,
            Color color,
            byte opacity)
        {
            Index = index;

            StartX = startX;
            StartZ = startZ;

            EndX = endX;
            EndZ = endZ;

            LineWidth = lineWidth;
            Color = color;
            Opacity = opacity;
        }
    }
}
