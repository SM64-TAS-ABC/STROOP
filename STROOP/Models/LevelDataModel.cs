using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Models
{
    public class LevelDataModel : IUpdatableDataModel
    {
        #region Properties
        private byte _index;
        public byte Index
        {
            get => _index;
            set
            {
                if (Config.Stream.SetValue(value, MiscConfig.WarpDestinationAddress + MiscConfig.LevelOffset))
                    _index = value;
            }
        }

        private byte _area;
        public byte Area
        {
            get => _area;
            set
            {
                if (Config.Stream.SetValue(value, MiscConfig.WarpDestinationAddress + MiscConfig.AreaOffset))
                    _area = value;
            }
        }

        private ushort _loadingPoint;
        public ushort LoadingPoint
        {
            get => _loadingPoint;
            set
            {
                if (Config.Stream.SetValue(value, MiscConfig.LoadingPointAddress))
                    _loadingPoint = value;
            }
        }
        private ushort _missionLayout;
        public ushort MissionLayout
        {
            get => _missionLayout;
            set
            {
                if (Config.Stream.SetValue(value, MiscConfig.MissionAddress))
                    _missionLayout = value;
            }
        }
        #endregion

        public void Update()
        {
            // Get level and area
            _index = Config.Stream.GetByte(MiscConfig.WarpDestinationAddress + MiscConfig.LevelOffset);
            _area = Config.Stream.GetByte(MiscConfig.WarpDestinationAddress + MiscConfig.AreaOffset);
            _loadingPoint = Config.Stream.GetUShort(MiscConfig.LoadingPointAddress);
            _missionLayout = Config.Stream.GetUShort(MiscConfig.MissionAddress);
        }

        public void Update2() { }
    }
}
