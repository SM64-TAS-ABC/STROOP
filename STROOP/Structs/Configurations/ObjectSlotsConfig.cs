using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using STROOP.Structs.Configurations;
using STROOP.Models;

namespace STROOP.Structs
{
    public static class ObjectSlotsConfig
    {
        public static readonly uint ProcessGroupStructSize = 0x68;

        public static readonly int MaxSlots = 240;

        public static readonly Color Pink = Color.FromArgb(255, 000, 165);
        public static readonly Color Red = Color.FromArgb(255, 000, 000);
        public static readonly Color RedOrange = Color.FromArgb(255, 084, 000);
        public static readonly Color Orange = Color.FromArgb(255, 161, 000);
        public static readonly Color Yellow = Color.FromArgb(255, 246, 000);
        public static readonly Color Green = Color.FromArgb(016, 255, 000);
        public static readonly Color LightBlue = Color.FromArgb(000, 255, 233);
        public static readonly Color Blue = Color.FromArgb(000, 021, 255);
        public static readonly Color Purple = Color.FromArgb(128, 000, 255);
        public static readonly Color Brown = Color.FromArgb(155, 095, 028);

        public static readonly Dictionary<byte, Color> ProcessingGroupsColor =
            new Dictionary<byte, Color>()
            {
                [0x0B] = Pink,
                [0x09] = Red,
                [0x0A] = RedOrange,
                [0x00] = Orange,
                [0x05] = Yellow,
                [0x04] = Green,
                [0x02] = LightBlue,
                [0x06] = Blue,
                [0x08] = Purple,
                [0x0C] = Brown,
            };
        public static readonly Color VacantSlotColor = Color.FromArgb(170, 170, 170); // grey
        public static Color GetProcessingGroupColor(byte? group)
        {
            if (group.HasValue)
                return ProcessingGroupsColor[group.Value];
            else
                return VacantSlotColor;
        }
        public static readonly List<byte> ProcessingGroups = ProcessingGroupsColor.Keys.ToList();

        public static readonly List<Color> RngUsageColors = new List<Color>()
        {
            Color.White,
            Red,
            Orange,
            Yellow,
            Green,
            LightBlue,
            Blue,
            Purple,
        };

        public static uint ProcessGroupsStartAddress { get => RomVersionConfig.SwitchMap(ProcessGroupsStartAddressUS, ProcessGroupsStartAddressJP, ProcessGroupsStartAddressSH, ProcessGroupsStartAddressEU); }
        public static readonly uint ProcessGroupsStartAddressUS = 0x8033CBE0;
        public static readonly uint ProcessGroupsStartAddressJP = 0x8033B870;
        public static readonly uint ProcessGroupsStartAddressSH = 0x80343428;
        public static readonly uint ProcessGroupsStartAddressEU = 0x8032EE98;

        public static uint ObjectSlotsStartAddress { get => RomVersionConfig.SwitchMap(ObjectSlotsStartAddressUS, ObjectSlotsStartAddressJP, ObjectSlotsStartAddressSH, ObjectSlotsStartAddressEU); }
        public static readonly uint ObjectSlotsStartAddressUS = 0x8033D488;
        public static readonly uint ObjectSlotsStartAddressJP = 0x8033C118;
        public static readonly uint ObjectSlotsStartAddressSH = 0x8031F648;
        public static readonly uint ObjectSlotsStartAddressEU = 0x8030B0B8;

        public static uint VacantSlotsNodeAddress { get => RomVersionConfig.SwitchMap(VacantSlotsNodeAddressUS, VacantSlotsNodeAddressJP, VacantSlotsNodeAddressSH, VacantSlotsNodeAddressEU); }
        public static readonly uint VacantSlotsNodeAddressUS = 0x803610F0;
        public static readonly uint VacantSlotsNodeAddressJP = 0x8035FD80;
        public static readonly uint VacantSlotsNodeAddressSH = 0x803432B0;
        public static readonly uint VacantSlotsNodeAddressEU = 0x8032ED20;

        public static uint UnusedSlotAddress { get => RomVersionConfig.SwitchMap(UnusedSlotAddressUS, UnusedSlotAddressJP, UnusedSlotAddressSH, UnusedSlotAddressEU); }
        public static readonly uint UnusedSlotAddressUS = 0x80360E88;
        public static readonly uint UnusedSlotAddressJP = 0x8035FB18;
        public static readonly uint UnusedSlotAddressSH = 0x80343048;
        public static readonly uint UnusedSlotAddressEU = 0x8032EAB8;
    }
}
