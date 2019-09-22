using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Controls.Map;
using OpenTK.Graphics.OpenGL;
using STROOP.Utilities;
using STROOP.Structs.Configurations;
using STROOP.Structs;
using OpenTK;

namespace STROOP.Map3
{
    public abstract class Map3Object : IDisposable
    {
        public float Size = 50;
        public double Opacity = 1;
        public byte OpacityByte { get => (byte)(Opacity * 255); }
        public Color Color = SystemColors.Control;

        public Map3Object()
        {
        }

        public abstract void DrawOnControl();

        public abstract void Dispose();
    }
}
