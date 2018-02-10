using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Extensions
{
    public static class RectangleExtensions
    {
        public static RectangleF Zoom(this RectangleF inRec, Size objZoom)
        {
            // Calculate scale of "zoom" view (make sure image fits fully within the region, 
            // it is at a maximum size, and the aspect ration is maintained 
            float hScale = inRec.Width / objZoom.Width;
            float vScale = inRec.Height / objZoom.Height;
            float scale = Math.Min(hScale, vScale);

            float marginV = 0;
            float marginH = 0;
            if (hScale > vScale)
                marginH = (inRec.Width - scale * objZoom.Width);
            else
                marginV = (inRec.Height - scale * objZoom.Height);

            // Calculate where the map image should be drawn
            return new RectangleF(marginH / 2 + inRec.X, marginV / 2 + inRec.Y, inRec.Width - marginH - inRec.X, inRec.Height - marginV - inRec.Y);
        }

        public static Rectangle Zoom(this Rectangle inRec, Size objZoom)
        {
            // Calculate scale of "zoom" view (make sure image fits fully within the region, 
            // it is at a maximum size, and the aspect ration is maintained 
            float hScale = (float)inRec.Width / objZoom.Width;
            float vScale = (float)inRec.Height / objZoom.Height;
            float scale = Math.Min(hScale, vScale);

            int marginV = 0;
            int marginH = 0;
            if (hScale > vScale)
                marginH = (int)(inRec.Width - scale * objZoom.Width);
            else
                marginV = (int)(inRec.Height - scale * objZoom.Height);

            // Calculate where the map image should be drawn
            return new Rectangle(marginH / 2 + inRec.X, marginV / 2 + inRec.Y, inRec.Width - marginH, inRec.Height - marginV);
        }
    }
}
