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
using System.Xml.Linq;
using STROOP.Models;
using STROOP.Controls;

namespace STROOP.Map
{
    public class MapObjectObjectTargetArrow : MapObjectArrow
    {
        private readonly PositionAngle _posAngle;
        private readonly WatchVariableWrapper _targetVar;
        private readonly List<uint> _fixedAddresses;

        public MapObjectObjectTargetArrow(PositionAngle posAngle)
            : base()
        {
            _posAngle = posAngle;

            ObjectDataModel obj = new ObjectDataModel(posAngle.GetObjAddress());
            List<WatchVariableControl> vars = obj.BehaviorAssociation.WatchVariableControls;
            _targetVar = vars.FirstOrDefault(v => v.VarName.Contains("Target"))?.WatchVarWrapper;
            _fixedAddresses = new List<uint>() { _posAngle.GetObjAddress() };
        }

        public override PositionAngle GetPositionAngle()
        {
            return _posAngle;
        }

        protected override double GetYaw()
        {
            return _targetVar == null ? 0 : ParsingUtilities.ParseDouble(_targetVar.GetValue(addresses: _fixedAddresses));
        }

        protected override double GetPitch()
        {
            return -1 * Config.Stream.GetShort(_posAngle.GetObjAddress() + ObjectConfig.PitchFacingOffset);
        }

        protected override double GetRecommendedSize()
        {
            return Config.Stream.GetFloat(_posAngle.GetObjAddress() + ObjectConfig.HSpeedOffset);
        }

        protected override void SetRecommendedSize(double size)
        {
            Config.Stream.SetValue((float)size, _posAngle.GetObjAddress() + ObjectConfig.HSpeedOffset);
        }

        protected override void SetYaw(double yaw)
        {
            if (_targetVar != null)
            {
                _targetVar.SetValue(yaw, false, _fixedAddresses);
            }
        }

        public override string GetName()
        {
            return "Object Target Arrow for " + _posAngle.GetMapName();
        }

        public override List<XAttribute> GetXAttributes()
        {
            return new List<XAttribute>()
            {
                new XAttribute("positionAngle", _posAngle),
            };
        }
    }
}
