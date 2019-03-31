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

        private static readonly Dictionary<byte, Color> ProcessingGroupsColor =
            new Dictionary<byte, Color>()
            {
                [0x0B] = Color.FromArgb(255, 000, 165), // pink
                [0x09] = Color.FromArgb(255, 000, 000), // red
                [0x0A] = Color.FromArgb(255, 084, 000), // red orange
                [0x00] = Color.FromArgb(255, 161, 000), // orange
                [0x05] = Color.FromArgb(255, 246, 000), // yellow
                [0x04] = Color.FromArgb(016, 255, 000), // green
                [0x02] = Color.FromArgb(000, 255, 233), // light blue
                [0x06] = Color.FromArgb(000, 021, 255), // dark blue
                [0x08] = Color.FromArgb(128, 000, 255), // purple
                [0x0C] = Color.FromArgb(155, 095, 028), // brown
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

        public static uint ProcessGroupsStartAddress { get => RomVersionConfig.Switch(ProcessGroupsStartAddressUS, ProcessGroupsStartAddressJP, ProcessGroupsStartAddressSH); }
        public static readonly uint ProcessGroupsStartAddressUS = 0x8033CBE0;
        public static readonly uint ProcessGroupsStartAddressJP = 0x8033B870;
        public static readonly uint ProcessGroupsStartAddressSH = 0x80343428;

        public static uint ObjectSlotsStartAddress { get => RomVersionConfig.Switch(ObjectSlotsStartAddressUS, ObjectSlotsStartAddressJP, ObjectSlotsStartAddressSH); }
        public static readonly uint ObjectSlotsStartAddressUS = 0x8033D488;
        public static readonly uint ObjectSlotsStartAddressJP = 0x8033C118;
        public static readonly uint ObjectSlotsStartAddressSH = 0x8031F648;

        public static uint VacantSlotsPointerAddress { get => RomVersionConfig.Switch(VacantSlotsPointerAddressUS, VacantSlotsPointerAddressJP, VacantSlotsPointerAddressSH); }
        public static readonly uint VacantSlotsPointerAddressUS = 0x80361150;
        public static readonly uint VacantSlotsPointerAddressJP = 0x8035FDE0;
        public static readonly uint VacantSlotsPointerAddressSH = 0x80343310;

        public static uint UnusedSlotAddress { get => RomVersionConfig.Switch(UnusedSlotAddressUS, UnusedSlotAddressJP); }
        public static readonly uint UnusedSlotAddressUS = 0x80360E88;
        public static readonly uint UnusedSlotAddressJP = 0x8035FB18;
    }
}
