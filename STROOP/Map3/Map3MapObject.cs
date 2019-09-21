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
    public class Map3MapObject : Map3IconRectangleObject
    {
        public Map3MapObject(Map3Graphics graphics)
            : base(graphics, () => Config.MapAssociations.GetBestMap().MapImage)
        {
        }

        protected override (PointF location, SizeF size) GetDimensions()
        {
            return (
                new PointF(
                    Graphics.MapView.X + Graphics.MapView.Width / 2,
                    Graphics.MapView.Y + Graphics.MapView.Height / 2),
                Graphics.MapView.Size);
        }
    }
}
