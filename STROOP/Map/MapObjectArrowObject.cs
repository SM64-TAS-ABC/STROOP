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

namespace STROOP.Map
{
    public class MapObjectArrowObject : MapLineObject
    {
        private readonly uint _objAddress;
        private readonly uint _yawOffset;
        private readonly int _numBytes;

        public MapObjectArrowObject(uint objAddress, uint yawOffset, int numBytes)
            : base()
        {
            _objAddress = objAddress;
            _yawOffset = yawOffset;
            _numBytes = numBytes;

            Size = 100;
            OutlineWidth = 3;
            OutlineColor = Color.Red;
        }

        protected override List<(float x, float z)> GetVertices()
        {
            float x = Config.Stream.GetSingle(_objAddress + ObjectConfig.XOffset);
            float z = Config.Stream.GetSingle(_objAddress + ObjectConfig.ZOffset);
            uint yaw = _numBytes == 2 ?
                Config.Stream.GetUInt16(_objAddress + _yawOffset) :
                Config.Stream.GetUInt32(_objAddress + _yawOffset);
            (float arrowHeadX, float arrowHeadZ) =
                ((float, float))MoreMath.AddVectorToPoint(Size, yaw, x, z);

            List<(float x, float z)> vertices = new List<(float x, float z)>();
            vertices.Add((x, z));
            vertices.Add((arrowHeadX, arrowHeadZ));
            return vertices;
        }

        public override string GetName()
        {
            return "Object Arrow";
        }

        public override Image GetImage()
        {
            return Config.ObjectAssociations.HolpImage;
        }
    }
}
