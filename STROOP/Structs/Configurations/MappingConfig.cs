using STROOP.Controls;
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

        public static void OpenMapping()
        {
            OpenFileDialog openFileDialog = DialogUtilities.CreateOpenFileDialog(FileType.Mapping);
            DialogResult result = openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            string fileName = openFileDialog.FileName;
            mappingCurrent = GetMappingDictionary(fileName);
            mappingCurrentReversed = DictionaryUtilities.ReverseDictionary(mappingCurrent);
        }

        public static void ClearMapping()
        {
            mappingCurrent = null;
            mappingCurrentReversed = null;
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

        public static List<WatchVariableControl> GetVariables()
        {
            if (mappingCurrent == null) return new List<WatchVariableControl>();

            List<WatchVariableControl> controls = new List<WatchVariableControl>();
            foreach (uint address in mappingCurrent.Keys)
            {
                string stringValue = mappingCurrent[address];
                int markerIndex = stringValue.IndexOf("___");
                if (markerIndex == -1) continue;
                string varName = stringValue.Substring(0, markerIndex);
                string suffix = stringValue.Substring(markerIndex + 3);
                Type type = GetTypeFromSuffix(suffix);
                if (type == null) continue;
                string typeString = TypeUtilities.TypeToString[type];

                WatchVariable watchVar = new WatchVariable(
                    memoryTypeName: typeString,
                    specialType: null,
                    baseAddressType: BaseAddressTypeEnum.Relative,
                    offsetUS: address,
                    offsetJP: address,
                    offsetSH: address,
                    offsetEU: address,
                    offsetDefault: null,
                    mask: null,
                    shift: null,
                    handleMapping: false);
                WatchVariableControlPrecursor precursor = new WatchVariableControlPrecursor(
                    name: varName,
                    watchVar: watchVar,
                    subclass: WatchVariableSubclass.Number,
                    backgroundColor: null,
                    displayType: null,
                    roundingLimit: null,
                    useHex: null,
                    invertBool: null,
                    isYaw: null,
                    coordinate: null,
                    groupList: new List<VariableGroup>() { VariableGroup.Custom });
                WatchVariableControl control = precursor.CreateWatchVariableControl();
                controls.Add(control);
            }
            return controls;
        }

        private static Type GetTypeFromSuffix(string suffix)
        {
            switch (suffix.ToLower())
            {
                case "s8": return typeof(sbyte);
                case "u8": return typeof(byte);
                case "s16": return typeof(short);
                case "u16": return typeof(ushort);
                case "s32": return typeof(int);
                case "u32": return typeof(uint);
                case "s64": return typeof(long);
                case "u64": return typeof(ulong);
                case "f32": return typeof(float);
                case "f64": return typeof(double);
                default: return null;
            }
        }
    }
}
