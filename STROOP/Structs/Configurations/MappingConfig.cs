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
        private static readonly Dictionary<string, uint> mappingUSReversed = DictionaryUtilities.ReverseDictionary(mappingUS);
        private static readonly Dictionary<string, uint> mappingJPReversed = DictionaryUtilities.ReverseDictionary(mappingJP);

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

            Dictionary<uint, string> mappingOriginal;
            switch (RomVersionConfig.Version)
            {
                case RomVersion.US:
                    mappingOriginal = mappingUS;
                    break;
                case RomVersion.JP:
                    mappingOriginal = mappingJP;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!mappingOriginal.ContainsKey(address)) return address;
            string name = mappingOriginal[address];
            if (!mappingCurrentReversed.ContainsKey(name)) return address;
            return mappingCurrentReversed[name];
        }

        public static uint HandleReverseMapping(uint address)
        {
            if (mappingCurrent == null) return address;

            Dictionary<string, uint> mappingOriginalReversed;
            switch (RomVersionConfig.Version)
            {
                case RomVersion.US:
                    mappingOriginalReversed = mappingUSReversed;
                    break;
                case RomVersion.JP:
                    mappingOriginalReversed = mappingJPReversed;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (!mappingCurrent.ContainsKey(address)) return address;
            string name = mappingCurrent[address];
            if (!mappingOriginalReversed.ContainsKey(name)) return address;
            return mappingOriginalReversed[name];
        }

        /**
         *  Gets user added variables from the mapping,
         *  assuming they're suffixed with "_f32", "_s16", etc.
         */
        public static List<WatchVariableControl> GetVariables()
        {
            if (mappingCurrent == null) return new List<WatchVariableControl>();

            List<WatchVariableControl> controls = new List<WatchVariableControl>();
            foreach (uint address in mappingCurrent.Keys)
            {
                string stringValue = mappingCurrent[address];
                (Type type, string name) = GetInfoIfUserAddedWord(stringValue);
                if (type == null) continue;
                string typeString = TypeUtilities.TypeToString[type];

                WatchVariable watchVar = new WatchVariable(
                    name: name,
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
                    name: name,
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

        private static (Type type, string name) GetInfoIfUserAddedWord(string word)
        {
            if (_suffixes.Any(suff => word.EndsWith(suff)) &&
                _ignoredWords.All(ignored => word != ignored))
            {
                string suffix = _suffixes.First(suff => word.EndsWith(suff));
                Type type = GetTypeFromSuffix(suffix);
                string name = word.Substring(0, word.Length - suffix.Length);
                return (type, name);
            }
            return (null, null);
        }

        private static List<string> _suffixes = new List<string>()
        {
            "_s8",
            "_u8",
            "_s16",
            "_u16",
            "_s32",
            "_u32",
            "_s64",
            "_u64",
            "_f32",
            "_f64",
        };

        private static List<string> _ignoredWords = new List<string>()
        {
            "m64_read_u8",
            "m64_read_s16",
            "m64_read_compressed_u16",
            "string_to_u32",
            "approach_s32",
            "approach_f32",
            "random_u16",
            "gd_clamp_f32",
        };

        private static Type GetTypeFromSuffix(string suffix)
        {
            switch (suffix.ToLower())
            {
                case "_s8": return typeof(sbyte);
                case "_u8": return typeof(byte);
                case "_s16": return typeof(short);
                case "_u16": return typeof(ushort);
                case "_s32": return typeof(int);
                case "_u32": return typeof(uint);
                case "_s64": return typeof(long);
                case "_u64": return typeof(ulong);
                case "_f32": return typeof(float);
                case "_f64": return typeof(double);
                default: return null;
            }
        }
    }
}
