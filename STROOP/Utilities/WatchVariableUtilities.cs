using STROOP.Managers;
using STROOP.Structs.Configurations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STROOP.Structs
{
    public static class WatchVariableUtilities
    {
        public static BaseAddressTypeEnum GetBaseAddressType(string stringValue)
        {
            return (BaseAddressTypeEnum)Enum.Parse(typeof(BaseAddressTypeEnum), stringValue);
        }

        public static WatchVariableSubclass GetSubclass(string stringValue)
        {
            if (stringValue == null) return WatchVariableSubclass.Number;
            return (WatchVariableSubclass)Enum.Parse(typeof(WatchVariableSubclass), stringValue);
        }

        public static WatchVariableCoordinate GetCoordinate(string stringValue)
        {
            return (WatchVariableCoordinate)Enum.Parse(typeof(WatchVariableCoordinate), stringValue);
        }

        public static VariableGroup GetVariableGroup(string stringValue)
        {
            return (VariableGroup)Enum.Parse(typeof(VariableGroup), stringValue);
        }

        public static List<VariableGroup> ParseVariableGroupList(string stringValue)
        {
            List<VariableGroup> variableGroupList = new List<VariableGroup>();
            if (stringValue != null)
            {
                string[] groupNames = stringValue.Split(',');
                foreach (string groupName in groupNames)
                {
                    variableGroupList.Add(GetVariableGroup(groupName));
                }
            }
            return variableGroupList;
        }

        private static readonly List<uint> BaseAddressListZero = new List<uint> { 0 };
        private static readonly List<uint> BaseAddressListEmpty = new List<uint> { };
        
        public static List<uint> GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum baseAddressType)
        {
            switch (baseAddressType)
            {
                case BaseAddressTypeEnum.None:
                    return BaseAddressListZero;

                case BaseAddressTypeEnum.Absolute:
                    return BaseAddressListZero;

                case BaseAddressTypeEnum.Relative:
                    return BaseAddressListZero;

                case BaseAddressTypeEnum.Mario:
                    return new List<uint> { MarioConfig.StructAddress };

                case BaseAddressTypeEnum.MarioObj:
                    return new List<uint> { Config.Stream.GetUInt32(MarioObjectConfig.PointerAddress) };

                case BaseAddressTypeEnum.Camera:
                    return new List<uint> { CameraConfig.StructAddress };

                case BaseAddressTypeEnum.File:
                    return new List<uint> { Config.FileManager.CurrentFileAddress };

                case BaseAddressTypeEnum.MainSave:
                    return new List<uint> { Config.MainSaveManager.CurrentMainSaveAddress };

                case BaseAddressTypeEnum.Object:
                    return Config.ObjectSlotsManager.SelectedSlotsAddresses.ToList();

                case BaseAddressTypeEnum.Triangle:
                    {
                        uint triangleAddress = Config.TriangleManager.TriangleAddress;
                        return triangleAddress != 0 ? new List<uint>() { triangleAddress } : BaseAddressListEmpty;
                    }

                case BaseAddressTypeEnum.TriangleExertionForceTable:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Triangle)
                        .ConvertAll(triangleAddress =>
                        {
                            uint exertionForceIndex = Config.Stream.GetByte(triangleAddress + TriangleOffsetsConfig.ExertionForceIndex);
                            return TriangleConfig.ExertionForceTableAddress + 2 * exertionForceIndex;
                        });

                case BaseAddressTypeEnum.InputCurrent:
                    return new List<uint> { InputConfig.CurrentInputAddress };

                case BaseAddressTypeEnum.InputJustPressed:
                    return new List<uint> { InputConfig.JustPressedInputAddress };

                case BaseAddressTypeEnum.InputBuffered:
                    return new List<uint> { InputConfig.BufferedInputAddress };

                case BaseAddressTypeEnum.Graphics:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + ObjectConfig.BehaviorGfxOffset));

                case BaseAddressTypeEnum.Animation:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + ObjectConfig.AnimationOffset));

                case BaseAddressTypeEnum.Waypoint:
                    return GetBaseAddressListFromBaseAddressType(BaseAddressTypeEnum.Object)
                        .ConvertAll(objAddress => Config.Stream.GetUInt32(objAddress + ObjectConfig.WaypointOffset));

                case BaseAddressTypeEnum.Water:
                    {
                        uint waterAddress = Config.Stream.GetUInt32(MiscConfig.WaterPointerAddress);
                        return waterAddress != 0 ? new List<uint>() { waterAddress } : BaseAddressListEmpty;
                    }

                case BaseAddressTypeEnum.Snow:
                    return new List<uint> { Config.Stream.GetUInt32(SnowConfig.SnowArrayPointerAddress) };

                case BaseAddressTypeEnum.Area:
                    return new List<uint> { Config.AreaManager.SelectedAreaAddress };

                case BaseAddressTypeEnum.LastCoin:
                    {
                        List<uint> coinAddresses = Config.ObjectSlotsManager.GetLoadedObjectsWithPredicate(
                            o => o.BehaviorAssociation?.Name == "Yellow Coin" || o.BehaviorAssociation?.Name == "Blue Coin")
                            .ConvertAll(objectDataModel => objectDataModel.Address);
                        return coinAddresses.Count > 0 ? new List<uint>() { coinAddresses.Last() } : BaseAddressListEmpty;
                    }

                case BaseAddressTypeEnum.Ghost:
                    return Config.ObjectSlotsManager.GetLoadedObjectsWithName("Mario Ghost")
                        .ConvertAll(objectDataModel => objectDataModel.Address);

                case BaseAddressTypeEnum.HackedArea:
                    return new List<uint> { MiscConfig.HackedAreaAddress };

                case BaseAddressTypeEnum.CamHack:
                    return new List<uint> { CamHackConfig.StructAddress };

                case BaseAddressTypeEnum.GfxNode:
                    {
                        GfxNode node  = Config.GfxManager.SelectedNode;
                        return node != null ? new List<uint>() { node.Address } : BaseAddressListEmpty;
                    }
                case BaseAddressTypeEnum.GhostHack:
                    return new List<uint>
                    {
                        GhostHackConfig.MemoryAddress +
                        GhostHackConfig.FrameDataStructSize *
                            (Config.Stream.GetUInt32(GhostHackConfig.NumFramesAddress) + GhostHackConfig.FrameOffset)
                    };

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
