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
            double marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

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

            (double x, double y, double z) getPlaneLineIntersection((double x, double y, double z) p0)
            {
                // x = x0 + at
                // y = y0 + bt
                // z = z0 + ct

                double camAngleRadians = MoreMath.AngleUnitsToRadians(camAngle);
                double camPitchRadians = MoreMath.AngleUnitsToRadians(camPitch + 16384);

                double a = Math.Cos(camPitchRadians) * Math.Sin(camAngleRadians);
                double b = Math.Sin(camPitchRadians);
                double c = Math.Cos(camPitchRadians) * Math.Cos(camAngleRadians);

                // y = M
                // M = y0 + bt
                // bt = M - y0
                // t = (M - y0) / b

                double y = marioY;
                double t = (y - p0.y) / b;
                double x = p0.x + a * t;
                double z = p0.z + c * t;

                return (x, y, z);
            }

            (double x, double y, double z) finalBackLeft = getPlaneLineIntersection(pointBackLeft);
            (double x, double y, double z) finalBackRight = getPlaneLineIntersection(pointBackRight);
            (double x, double y, double z) finalFrontLeft = getPlaneLineIntersection(pointFrontLeft);
            (double x, double y, double z) finalFrontRight = getPlaneLineIntersection(pointFrontRight);

            return new List<List<(float x, float y, float z, bool isHovered)>>()
            {
                new List<(float x, float y, float z, bool isHovered)>()
                {
                    ((float)finalBackLeft.x, (float)finalBackLeft.y, (float)finalBackLeft.z, false),
                    ((float)finalBackRight.x, (float)finalBackRight.y, (float)finalBackRight.z, false),
                    ((float)finalFrontRight.x, (float)finalFrontRight.y, (float)finalFrontRight.z, false),
                    ((float)finalFrontLeft.x, (float)finalFrontLeft.y, (float)finalFrontLeft.z, false),
                },
            };
        }

        protected override List<List<(float x, float y, float z, bool isHovered)>> GetQuadList3D()
        {
            (double camX, double camY, double camZ, double camAngle) = PositionAngle.Camera.GetValues();
            double camPitch = Config.Stream.GetShort(CameraConfig.StructAddress + CameraConfig.FacingPitchOffset);
            double fov = Config.Stream.GetFloat(CameraConfig.FOVStructAddress + CameraConfig.FOVValueOffset);
            double marioY = Config.Stream.GetFloat(MarioConfig.StructAddress + MarioConfig.YOffset);

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

            (double x, double y, double z) getPlaneLineIntersection((double x, double y, double z) p0, double t)
            {
                // x = x0 + at
                // y = y0 + bt
                // z = z0 + ct

                double camAngleRadians = MoreMath.AngleUnitsToRadians(camAngle);
                double camPitchRadians = MoreMath.AngleUnitsToRadians(camPitch + 16384);

                double a = Math.Cos(camPitchRadians) * Math.Sin(camAngleRadians);
                double b = Math.Sin(camPitchRadians);
                double c = Math.Cos(camPitchRadians) * Math.Cos(camAngleRadians);

                // y = M
                // M = y0 + bt
                // bt = M - y0
                // t = (M - y0) / b

                double x = p0.x + a * t;
                double y = p0.y + b * t;
                double z = p0.z + c * t;

                return (x, y, z);
            }

            double tTop = 1000;
            double tBottom = -1000;

            (double x, double y, double z) finalBackLeftTop = getPlaneLineIntersection(pointBackLeft, tTop);
            (double x, double y, double z) finalBackLeftBottom = getPlaneLineIntersection(pointBackLeft, tBottom);
            (double x, double y, double z) finalBackRightTop = getPlaneLineIntersection(pointBackRight, tTop);
            (double x, double y, double z) finalBackRightBottom = getPlaneLineIntersection(pointBackRight, tBottom);
            (double x, double y, double z) finalFrontLeftTop = getPlaneLineIntersection(pointFrontLeft, tTop);
            (double x, double y, double z) finalFrontLeftBottom = getPlaneLineIntersection(pointFrontLeft, tBottom);
            (double x, double y, double z) finalFrontRightTop = getPlaneLineIntersection(pointFrontRight, tTop);
            (double x, double y, double z) finalFrontRightBottom = getPlaneLineIntersection(pointFrontRight, tBottom);

            List<(float x, float y, float z, bool isHovered)> createQuad(
                (double x, double y, double z) p1,
                (double x, double y, double z) p2,
                (double x, double y, double z) p3,
                (double x, double y, double z) p4)
            {
                return new List<(float x, float y, float z, bool isHovered)>()
                {
                    ((float)p1.x, (float)p1.y, (float)p1.z, false),
                    ((float)p2.x, (float)p2.y, (float)p2.z, false),
                    ((float)p3.x, (float)p3.y, (float)p3.z, false),
                    ((float)p4.x, (float)p4.y, (float)p4.z, false),
                };
            }

            return new List<List<(float x, float y, float z, bool isHovered)>>()
            {
                createQuad(finalBackLeftTop, finalBackLeftBottom, finalBackRightBottom, finalBackRightTop), // back
                createQuad(finalFrontLeftTop, finalFrontLeftBottom, finalFrontRightBottom, finalFrontRightTop), // front
                createQuad(finalBackLeftTop, finalBackRightTop, finalFrontRightTop, finalFrontLeftTop), // top
                createQuad(finalBackLeftBottom, finalBackRightBottom, finalFrontRightBottom, finalFrontLeftBottom), // bottom
                createQuad(finalBackLeftBottom, finalBackLeftTop, finalFrontLeftTop, finalFrontLeftBottom), // left
                createQuad(finalBackRightBottom, finalBackRightTop, finalFrontRightTop, finalFrontRightBottom), // right
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
