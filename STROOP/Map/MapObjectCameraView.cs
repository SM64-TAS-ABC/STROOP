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
    public class MapObjectCameraView : MapObjectQuad
    {
        public MapObjectCameraView()
            : base()
        {
            Size = 300;
            Opacity = 0.5;
            Color = Color.Yellow;
        }

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList(MapObjectHoverData hoverData)
        {
            (double camX, double camY, double camZ, double camAngle) = PositionAngle.Camera.GetValues();
            double camPitch = Config.Stream.GetShort(CameraConfig.StructAddress + CameraConfig.FacingPitchOffset);
            double fov = Config.Stream.GetFloat(CameraConfig.FOVStructAddress + CameraConfig.FOVValueOffset);
            double nearZ = 100 - Size;
            double farZ = 20_000 + Size;
            double angleDegrees = fov / 2 + 1;
            double angleRadians = angleDegrees / 360 * 2 * Math.PI;
            double nearXRadius = nearZ * Math.Tan(angleRadians) + Size;
            double farXRadius = farZ * Math.Tan(angleRadians) + Size;

            (double x, double y, double z) pointBackCenter = MoreMath.AddVectorToPointWithPitch(nearZ, camAngle, -camPitch, camX, camY, camZ, false);
            (double x, double y, double z) pointBackLeft = MoreMath.AddVectorToPoint(nearXRadius, camAngle + 16384, pointBackCenter.x, pointBackCenter.y, pointBackCenter.z);
            (double x, double y, double z) pointBackRight = MoreMath.AddVectorToPoint(nearXRadius, camAngle - 16384, pointBackCenter.x, pointBackCenter.y, pointBackCenter.z);
            (double x, double y, double z) pointFrontCenter = MoreMath.AddVectorToPointWithPitch(farZ, camAngle, camPitch, camX, camY, camZ, false);
            (double x, double y, double z) pointFrontLeft = MoreMath.AddVectorToPoint(farXRadius, camAngle + 16384, pointFrontCenter.x, pointFrontCenter.y, pointFrontCenter.z);
            (double x, double y, double z) pointFrontRight = MoreMath.AddVectorToPoint(farXRadius, camAngle - 16384, pointFrontCenter.x, pointFrontCenter.y, pointFrontCenter.z);

            return new List<List<(float x, float y, float z, bool isHovered)>>()
            {
                new List<(float x, float y, float z, bool isHovered)>()
                {
                    ((float)pointBackLeft.x, (float)pointBackLeft.y, (float)pointBackLeft.z, false),
                    ((float)pointBackRight.x, (float)pointBackRight.y, (float)pointBackRight.z, false),
                    ((float)pointFrontRight.x, (float)pointFrontRight.y, (float)pointFrontRight.z, false),
                    ((float)pointFrontLeft.x, (float)pointFrontLeft.y, (float)pointFrontLeft.z, false),
                }
            };
        }

        public override string GetName()
        {
            return "Camera View";
        }

        public override Image GetInternalImage()
        {
            return Config.ObjectAssociations.CameraViewImage;
        }
    }
}
