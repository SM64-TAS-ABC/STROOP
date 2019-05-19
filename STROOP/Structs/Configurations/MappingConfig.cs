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
        private static Dictionary<uint, string> mappingUS = GetMappingDictionary(@"Mappings/MappingUS.map");
        //private static Dictionary<uint, string> mappingJP = GetMappingDictionary(@"Mappings/MappingJP.map");

        public static Dictionary<uint, string> GetMappingDictionary(string filePath)
        {
            List<string> lines = DialogUtilities.ReadFileLines(filePath);
            return null;
        }

        public static uint HandleMapping(uint address)
        {
            return address;
        }

    }
}
