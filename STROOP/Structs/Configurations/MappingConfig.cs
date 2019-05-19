using STROOP.Managers;
using STROOP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace STROOP.Structs.Configurations
{
    public static class MappingConfig
    {
        private static readonly Dictionary<uint, string> mappingUS = GetMappingDictionary(@"Mappings/MappingUS.map");
        private static readonly Dictionary<uint, string> mappingJP = GetMappingDictionary(@"Mappings/MappingJP.map");
        private static readonly Dictionary<uint, string> mappingSH = GetMappingDictionary(@"Mappings/MappingUS.map"); // TODO: fix this

        private static Dictionary<uint, string> mappingCurrent = null;
        private static Dictionary<string, uint> mappingCurrentReversed = null;

        public static Dictionary<uint, string> GetMappingDictionary(string filePath)
        {
            Dictionary<uint, string> dictionary = new Dictionary<uint, string>();
            List<string> lines = DialogUtilities.ReadFileLines(filePath);
            foreach (string line in lines)
            {
                List<string> parts = ParsingUtilities.ParseStringList(line, false);
                if (parts.Count != 2) continue;
                string part1 = parts[0];
                string part2 = parts[1];
                if (!part1.StartsWith("0x00000000")) continue;
                string addressString = "0x" + part1.Substring(10);
                uint? addressNullable = ParsingUtilities.ParseHexNullable(addressString);
                if (!addressNullable.HasValue) continue;
                uint address = addressNullable.Value;
                dictionary[address] = part2;
            }
            return dictionary;
        }

        public static uint HandleMapping(uint address)
        {
            if (mappingCurrent == null) return address;

            Dictionary<uint, string> originalDictionary;
            switch (RomVersionConfig.Version)
            {
                case RomVersion.US:
                    originalDictionary = mappingUS;
                    break;
                case RomVersion.JP:
                    originalDictionary = mappingJP;
                    break;
                case RomVersion.SH:
                    originalDictionary = mappingSH;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!originalDictionary.ContainsKey(address)) return address;
            string name = originalDictionary[address];
            if (!mappingCurrentReversed.ContainsKey(name)) return address;
            return mappingCurrentReversed[name];
        }

    }
}
