using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;
using STROOP.Structs;
using STROOP.Structs.Configurations;
using STROOP.Forms;

namespace STROOP.Utilities
{
    public static class ObjectOrderingUtilities
    {

        private static List<List<uint>> GetProcessGroups()
        {
            List<List<uint>> processGroups = new List<List<uint>>();
            int slotIndex = 0;

            // processed slots
            foreach (byte processGroupByte in ObjectSlotsConfig.ProcessingGroups)
            {
                uint processGroupStructAddress = ObjectSlotsConfig.FirstGroupingAddress + processGroupByte * ObjectSlotsConfig.ProcessGroupStructSize;
                List<uint> processGroup = new List<uint>();
                uint objAddress = Config.Stream.GetUInt32(processGroupStructAddress + ObjectConfig.ProcessedNextLinkOffset);
                while ((objAddress != processGroupStructAddress && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    processGroup.Add(objAddress);
                    slotIndex++;
                    objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);
                }
                processGroups.Add(processGroup);
            }

            // vacant slots
            {
                List<uint> processGroup = new List<uint>();
                uint objAddress = Config.Stream.GetUInt32(ObjectSlotsConfig.VactantPointerAddress);
                while ((objAddress != 0 && slotIndex < ObjectSlotsConfig.MaxSlots))
                {
                    processGroup.Add(objAddress);
                    slotIndex++;
                    objAddress = Config.Stream.GetUInt32(objAddress + ObjectConfig.ProcessedNextLinkOffset);
                }
                processGroups.Add(processGroup);
            }

            return processGroups;
        }

        public static void Debug()
        {
            List<List<string>> labelLists = GetProcessGroups().ConvertAll(
                processGroup => processGroup.ConvertAll(
                    objAddress => Config.ObjectSlotsManager.GetDescriptiveSlotLabelFromAddress(objAddress, true)));
            string output = String.Join("\r\n", labelLists.ConvertAll(labelList => String.Join(", ", labelList)));
            InfoForm.ShowValue(output);
        }

    }
}
