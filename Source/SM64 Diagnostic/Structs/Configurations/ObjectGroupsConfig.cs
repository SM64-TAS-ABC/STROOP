using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SM64_Diagnostic.Structs.Configurations;

namespace SM64_Diagnostic.Structs
{
    public static class ObjectGroupsConfig
    {
        public static readonly Dictionary<byte, Color> ProcessingGroupsColor =
            new Dictionary<byte, Color>()
            {
                [0x0B] = ColorTranslator.FromHtml("#FF00A5"),
                [0x09] = ColorTranslator.FromHtml("#FF0000"),
                [0x0A] = ColorTranslator.FromHtml("#FF5400"),
                [0x00] = ColorTranslator.FromHtml("#FFA100"),
                [0x05] = ColorTranslator.FromHtml("#FFF600"),
                [0x04] = ColorTranslator.FromHtml("#10FF00"),
                [0x02] = ColorTranslator.FromHtml("#00FFE9"),
                [0x06] = ColorTranslator.FromHtml("#0015FF"),
                [0x08] = ColorTranslator.FromHtml("#8000FF"),
                [0x0C] = ColorTranslator.FromHtml("#FF00FF"),
            };
        public static readonly List<byte> ProcessingGroups = ProcessingGroupsColor.Keys.ToList();

        public static uint VactantPointerAddress { get { return Config.SwitchRomVersion(VactantPointerAddressUS, VactantPointerAddressJP); } }
        public static readonly uint VactantPointerAddressUS = 0x80361150;
        public static readonly uint VactantPointerAddressJP = 0x8035FDE0;

        public static readonly Color VacantSlotColor = ColorTranslator.FromHtml("#AAAAAA");
        public static readonly uint ProcessNextLinkOffset = 0x60;
        public static readonly uint ProcessPreviousLinkOffset = 0x64;
        public static readonly uint ParentObjectOffset = 0x68;

        public static uint FirstGroupingAddress { get { return Config.SwitchRomVersion(FirstGroupingAddressUS, FirstGroupingAddressJP); } }
        public static readonly uint FirstGroupingAddressUS = 0x8033CBE0;
        public static readonly uint FirstGroupingAddressJP = 0x8033B870;

        public static readonly uint ProcessGroupStructSize = 0x68;
    }
}
